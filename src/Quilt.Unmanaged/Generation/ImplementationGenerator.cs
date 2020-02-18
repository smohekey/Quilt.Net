namespace Quilt.Unmanaged.Generation {
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Reflection;
	using System.Reflection.Emit;
	using System.Runtime.InteropServices;
	using System.Security;
	using Sigil.NonGeneric;

	class ImplementationGenerator {
		public Type Generate<T>(UnmanagedLibrary library) {
			return new Context(library, typeof(T)).Generate();
		}

		private class Context {
			private static readonly Type __objectType = typeof(object);
			private static readonly ConstructorInfo __objectConstructor = __objectType.GetConstructor(Array.Empty<Type>())!;

			private static readonly Type __stringType = typeof(string);
			private static readonly Type __intPtrType = typeof(IntPtr);

			private static readonly Type __unmanagedLibraryType = typeof(UnmanagedLibrary);
			private static readonly MethodInfo __getSymbolMethod = __unmanagedLibraryType.GetMethod(nameof(UnmanagedLibrary.LoadSymbol), new[] { __stringType })!;

			private static readonly Type __unmanagedObjectType = typeof(UnmanagedObject);
			private static readonly MethodInfo __loadSymbolMethod = __unmanagedObjectType.GetMethod(UnmanagedObject.LOAD_SYMBOL_NAME, BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { __stringType }, null)!;

			private static readonly Type __marshalType = typeof(Marshal);
			private static readonly MethodInfo __getDelegateForFunctionPointerMethod = __marshalType.GetMethod(nameof(Marshal.GetDelegateForFunctionPointer), new[] { __intPtrType })!;

			private static readonly Type __unmanagedFunctionPointerType = typeof(UnmanagedFunctionPointerAttribute);
			private static readonly ConstructorInfo __unmanagedFunctionPointerConstructor = __unmanagedFunctionPointerType.GetConstructor(new[] { typeof(CallingConvention) })!;
			private static readonly FieldInfo __charSetField = __unmanagedFunctionPointerType.GetField(nameof(UnmanagedFunctionPointerAttribute.CharSet))!;
			private static readonly FieldInfo __setLastErrorField = __unmanagedFunctionPointerType.GetField(nameof(UnmanagedFunctionPointerAttribute.SetLastError))!;

			private static readonly Type __suppressUnmanagedCodeSecurityType = typeof(SuppressUnmanagedCodeSecurityAttribute);
			private static readonly ConstructorInfo __suppressUnmanagedCodeSecurityConstructor = __suppressUnmanagedCodeSecurityType.GetConstructor(Array.Empty<Type>())!;

			private static readonly Dictionary<UnmanagedType, MethodInfo> __stringToPtrMethods;
			private static readonly Dictionary<UnmanagedType, MethodInfo> __ptrToStringMethods;

			private static readonly Random __random = new Random();

			static Context() {
				__stringToPtrMethods = new Dictionary<UnmanagedType, MethodInfo> {
					{ UnmanagedType.BStr, __marshalType.GetMethod(nameof(Marshal.StringToBSTR), new[] { __stringType })! },
					{ UnmanagedType.LPWStr, __marshalType.GetMethod(nameof(Marshal.StringToHGlobalUni), new[] { __stringType })! },
					{ UnmanagedType.LPStr, __marshalType.GetMethod(nameof(Marshal.StringToHGlobalAnsi), new[] { __stringType })! }
				};

				__ptrToStringMethods = new Dictionary<UnmanagedType, MethodInfo> {
					{ UnmanagedType.BStr, __marshalType.GetMethod(nameof(Marshal.PtrToStringBSTR), new[] { __intPtrType })! },
					{ UnmanagedType.LPWStr, __marshalType.GetMethod(nameof(Marshal.PtrToStringUni), new [] { __intPtrType})! },
					{ UnmanagedType.LPStr, __marshalType.GetMethod(nameof(Marshal.PtrToStringAnsi), new [] { __intPtrType})! },
					{ UnmanagedType.LPTStr, __marshalType.GetMethod(nameof(Marshal.PtrToStringAuto), new[] { __intPtrType})! }
				};
			}

			private readonly UnmanagedLibrary _library;
			private readonly Type _type;

			private readonly Dictionary<string, int> _delegateNameCounts = new Dictionary<string, int>();
			private readonly AssemblyBuilder _assemblyBuilder;
			private readonly ModuleBuilder _moduleBuilder;

			public Context(UnmanagedLibrary library, Type type) {
				_library = library;
				_type = type;

				var buffer = new byte[16];

				__random.NextBytes(buffer);

				var assemblyName = new AssemblyName($"{_type.Assembly.GetName().Name}.x{BitConverter.ToString(buffer).Replace("-", "")}");
				_assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
				_moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name!);
			}

			public Type Generate() {
				if (!_type.IsAbstract) {
					throw new ArgumentException($"Type '{_type.Name}' must be abstract.");
				}

				var interfaceAttribute = _type.GetCustomAttribute<UnmanagedObjectAttribute>(true);

				if (interfaceAttribute == null) {
					throw new ArgumentException($"Type '{_type.Name}' must have the {nameof(UnmanagedObjectAttribute)}");
				}

				var buffer = new byte[16];

				__random.NextBytes(buffer);

				var typeBuilder = _moduleBuilder.DefineType($"{_type.Namespace}.x{BitConverter.ToString(buffer).Replace("-", "")}.{_type.Name}", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class, _type);

				GenerateConstructors(typeBuilder);

				var baseLoadDelegatesMethod = _type.GetMethod(UnmanagedObject.LOAD_DELEGATES_NAME, BindingFlags.Instance | BindingFlags.NonPublic)!;

				var loadDelegatesEmit = GenerateLoadDelegatesMethod(typeBuilder);

				foreach (var methodInfo in _type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(m => m.IsAbstract && m != baseLoadDelegatesMethod)) {
					GenerateMethod(interfaceAttribute, typeBuilder, loadDelegatesEmit, methodInfo);
				}

				loadDelegatesEmit.Return();

				typeBuilder.DefineMethodOverride(loadDelegatesEmit.CreateMethod(), baseLoadDelegatesMethod);

				return typeBuilder.CreateType()!;
			}

			private void GenerateMethod(UnmanagedObjectAttribute interfaceAttribute, TypeBuilder typeBuilder, Emit loadDelegatesEmit, MethodInfo methodInfo) {
				var methodAttribute = methodInfo.GetCustomAttribute<UnmanagedMethodAttribute>();

				var callingConvention = methodAttribute?.CallingConvention ?? interfaceAttribute.CallingConvention;
				var charSet = methodAttribute?.CharSet ?? interfaceAttribute.CharSet;
				var setLastError = methodAttribute?.SetLastError ?? interfaceAttribute.SetLastError;
				var prefix = methodAttribute?.Prefix ?? interfaceAttribute.Prefix;
				var unmanagedName = methodAttribute?.Name ?? $"{prefix}{methodInfo.Name}";
				var parameterInfos = methodInfo.GetParameters();
				var parameterTypes = parameterInfos.Select(p => p.ParameterType).ToArray();

				var (delegateType, delegateInvokeMethod) = GenerateDelegateType(typeBuilder, methodInfo, callingConvention, charSet, setLastError, methodInfo.GetCustomAttribute<SuppressUnmanagedCodeSecurityAttribute>() != null, parameterInfos);
				var delegateFieldBuilder = GenerateDelegateField(typeBuilder, methodInfo, delegateType);

				var emit = Emit.BuildInstanceMethod(methodInfo.ReturnType, parameterTypes, typeBuilder, methodInfo.Name, methodInfo.Attributes & ~MethodAttributes.Abstract);

				emit.LoadArgument(0);
				emit.LoadField(delegateFieldBuilder);

				for (ushort i = 0; i < parameterInfos.Length; i++) {
					var parameterInfo = parameterInfos[i];

					emit.LoadArgument((ushort)(i + 1));

					if (parameterInfo.ParameterType == __stringType) {
						var unmanagedType = GetStringParameterUnmanagedType(parameterInfo);

						emit.Call(GetManagedToUnmanagedStringMarshalMethod(unmanagedType));
					}
				}

				emit.Call(delegateInvokeMethod);

				if (methodInfo.ReturnType == __stringType) {
					var unmanagedType = GetStringParameterUnmanagedType(methodInfo.ReturnParameter);

					emit.Call(GetUnmanagedToManagedStringMarshalMethod(unmanagedType));
				}

				emit.Return();

				typeBuilder.DefineMethodOverride(emit.CreateMethod(), methodInfo);

				GenerateDelegateFieldLoad(loadDelegatesEmit, delegateType, delegateFieldBuilder, unmanagedName);
			}

			private Emit GenerateLoadDelegatesMethod(TypeBuilder typeBuilder) {
				var emit = Emit.BuildInstanceMethod(typeof(void), Array.Empty<Type>(), typeBuilder, UnmanagedObject.LOAD_DELEGATES_NAME, MethodAttributes.Virtual | MethodAttributes.Family);

				return emit;
			}

			private void GenerateConstructors(TypeBuilder typeBuilder) {
				var constructors = _type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

				foreach (var constructor in constructors) {
					var parameters = constructor.GetParameters();

					if (parameters.Length == 0 || parameters[0].ParameterType != typeof(UnmanagedLibrary)) {
						continue;
					}

					var emit = Emit.BuildConstructor(parameters.Select(p => p.ParameterType).ToArray(), typeBuilder, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.HideBySig, CallingConventions.HasThis);

					emit.LoadArgument(0);

					for (ushort i = 1; i <= parameters.Length; i++) {
						emit.LoadArgument(i);
					}

					emit.Call(constructor);
					emit.Return();
					emit.CreateConstructor();
				}
			}

			private (Type, MethodInfo) GenerateDelegateType(TypeBuilder typeBuilder, MethodInfo methodInfo, CallingConvention callingConvention, CharSet charSet, bool setLastError, bool suppressCodeSecurity, ParameterInfo[] parameterInfos) {
				var name = $"{methodInfo.Name}Delegate";

				if (!_delegateNameCounts.TryGetValue(name, out var count)) {
					_delegateNameCounts[name] = 1;
				} else {
					count++;

					_delegateNameCounts[name] = count;

					name = $"{name}{count}";
				}

				//var delegateTypeBuilder = typeBuilder.DefineNestedType(name, TypeAttributes.Class | TypeAttributes.NestedPublic | TypeAttributes.Sealed | TypeAttributes.AnsiClass | TypeAttributes.AutoClass, typeof(MulticastDelegate));
				var delegateTypeBuilder = _moduleBuilder.DefineType(name, TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.AnsiClass | TypeAttributes.AutoClass, typeof(MulticastDelegate));

				delegateTypeBuilder.SetCustomAttribute(
					new CustomAttributeBuilder(
						__unmanagedFunctionPointerConstructor,
						new object[] { callingConvention },
						new[] { __charSetField, __setLastErrorField },
						new object[] { charSet, setLastError }
					)
				);

				if (suppressCodeSecurity) {
					delegateTypeBuilder.SetCustomAttribute(
						new CustomAttributeBuilder(
							__suppressUnmanagedCodeSecurityConstructor,
							Array.Empty<object>()
						)
					);
				}

				var delegateConstructorBuilder = delegateTypeBuilder.DefineConstructor(MethodAttributes.RTSpecialName | MethodAttributes.HideBySig | MethodAttributes.Public, CallingConventions.Standard, new[] { __objectType, __intPtrType });

				delegateConstructorBuilder.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

				var parameterTypes = parameterInfos.Select(p =>
					p.ParameterType == __stringType ? __intPtrType : p.ParameterType
				).ToArray();

				var returnType = methodInfo.ReturnType == __stringType ? __intPtrType : methodInfo.ReturnType;

				var delegateInvokeMethodBuilder = delegateTypeBuilder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, returnType, parameterTypes);

				/*if (methodInfo.ReturnParameter.GetCustomAttributes().Any()) {
					var returnParameterBuilder = delegateInvokeMethodBuilder.DefineParameter(0, methodInfo.ReturnParameter.Attributes, methodInfo.ReturnParameter.Name);

					foreach (var attribute in methodInfo.ReturnParameter.GetCustomAttributesData()) {
						returnParameterBuilder.SetCustomAttribute(CreateCustomAttributeBuilder(attribute));
					}
				}

				for(int i = 0; i < parameters.Length; i++) {
					var parameter = parameters[i];

					var parameterBuilder = delegateInvokeMethodBuilder.DefineParameter(i + 1, parameter.Attributes, parameter.Name);

					foreach (var attribute in parameter.GetCustomAttributesData()) {
						parameterBuilder.SetCustomAttribute(CreateCustomAttributeBuilder(attribute));
					}
				}*/

				delegateInvokeMethodBuilder.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

				var delegateType = delegateTypeBuilder.CreateType();

				return (delegateType, delegateType.GetMethod("Invoke")!);
			}

			private FieldBuilder GenerateDelegateField(TypeBuilder typeBuilder, MethodInfo methodInfo, Type delegateType) {
				return typeBuilder.DefineField($"{methodInfo.Name}_delegate", delegateType, FieldAttributes.Private | FieldAttributes.InitOnly);
			}

			private void GenerateDelegateFieldLoad(Emit emit, Type delegateType, FieldBuilder delegateFieldBuilder, string unmanagedName) {
				var symbolLocal = emit.DeclareLocal<IntPtr>();
				var failLabel = emit.DefineLabel();
				var successLabel = emit.DefineLabel();

				emit.LoadArgument(0);
				emit.LoadConstant(unmanagedName);
				emit.CallVirtual(__loadSymbolMethod);
				emit.StoreLocal(symbolLocal);
				emit.LoadLocal(symbolLocal);
				emit.LoadNull();
				emit.BranchIfEqual(failLabel);
				emit.LoadArgument(0);
				emit.LoadLocal(symbolLocal);
				emit.Call(__getDelegateForFunctionPointerMethod.MakeGenericMethod(delegateType));
				emit.StoreField(delegateFieldBuilder);
				emit.Branch(successLabel);
				emit.MarkLabel(failLabel);
				emit.WriteLine($"Couldn't load symbol {unmanagedName}.");
				emit.MarkLabel(successLabel);
			}

			private static CustomAttributeBuilder CreateCustomAttributeBuilder(CustomAttributeData customAttribute) {
				var namedFields = customAttribute.NamedArguments?.Where(a => a.IsField).ToList() ?? new List<CustomAttributeNamedArgument>();
				var namedProperties = customAttribute.NamedArguments?.Where(a => a.MemberInfo is PropertyInfo).ToList() ?? new List<CustomAttributeNamedArgument>();

				return new CustomAttributeBuilder(
					customAttribute.Constructor,
					customAttribute.ConstructorArguments.Select(a => a.Value).ToArray(),
					namedProperties.Select(p => p.MemberInfo).Cast<PropertyInfo>().ToArray(),
					namedProperties.Select(p => p.TypedValue.Value).ToArray()!,
					namedFields.Select(f => f.MemberInfo).Cast<FieldInfo>().ToArray(),
					namedFields.Select(f => f.TypedValue.Value).ToArray()!
				);
			}

			private UnmanagedType GetStringParameterUnmanagedType(ParameterInfo parameter) {
				return parameter.GetCustomAttribute<MarshalAsAttribute>()?.Value ?? UnmanagedType.LPStr;
			}

			private MethodInfo GetManagedToUnmanagedStringMarshalMethod(UnmanagedType unmanagedType) {
				return unmanagedType switch
				{
					UnmanagedType.LPTStr => RuntimeInformation.FrameworkDescription.Contains("Mono") ? __stringToPtrMethods[UnmanagedType.LPWStr] : __stringToPtrMethods[UnmanagedType.LPTStr],
					_ => __stringToPtrMethods[unmanagedType]
				};
			}

			private MethodInfo GetUnmanagedToManagedStringMarshalMethod(UnmanagedType unmanagedType) {
				return unmanagedType switch
				{
					UnmanagedType.LPTStr => RuntimeInformation.FrameworkDescription.Contains("Mono") ? __ptrToStringMethods[UnmanagedType.LPWStr] : __ptrToStringMethods[UnmanagedType.LPTStr],
					_ => __ptrToStringMethods[unmanagedType]
				};
			}
		}
	}
}
