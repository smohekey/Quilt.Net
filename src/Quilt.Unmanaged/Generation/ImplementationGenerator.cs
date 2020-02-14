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
		private readonly Dictionary<Type, Type> _implementedTypes = new Dictionary<Type, Type>();

		public bool TryGenerate<TInterface, TBaseClass>(UnmanagedLibrary library, [NotNullWhen(true)] out Type? type) where TBaseClass : class {
			return TryGenerate(library, typeof(TInterface), typeof(TBaseClass), out type);
		}

		public bool TryGenerate<TInterface>(UnmanagedLibrary library, [NotNullWhen(true)] out Type? type) {
			return TryGenerate(library, typeof(TInterface), null, out type);
		}

		public bool TryGenerate(UnmanagedLibrary library, Type interfaceType, Type? baseType, [NotNullWhen(true)] out Type? type) {
			if (_implementedTypes.TryGetValue(interfaceType, out type)) {
				return true;
			}

			return new Context(library, interfaceType, baseType).TryGenerate(out type);
		}

		private class Context {
			private static readonly Type __objectType = typeof(object);
			private static readonly ConstructorInfo __objectConstructor = __objectType.GetConstructor(Array.Empty<Type>());

			private static readonly Type __stringType = typeof(string);
			private static readonly Type __intPtrType = typeof(IntPtr);

			private static readonly Type __unmanagedLibraryType = typeof(UnmanagedLibrary);
			private static readonly MethodInfo __getSymbolMethod = __unmanagedLibraryType.GetMethod(nameof(UnmanagedLibrary.GetSymbol), new[] { __stringType })!;

			private static readonly Type __marshalType = typeof(Marshal);
			private static readonly MethodInfo __getDelegateForFunctionPointerMethod = __marshalType.GetMethod(nameof(Marshal.GetDelegateForFunctionPointer), new[] { __intPtrType })!;

			private static readonly Type __unmanagedFunctionPointerType = typeof(UnmanagedFunctionPointerAttribute);
			private static readonly Type[] __constructorParameterTypes = new[] { typeof(UnmanagedLibrary) };
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
					{ UnmanagedType.BStr, __marshalType.GetMethod(nameof(Marshal.StringToBSTR), new[] { __stringType }) },
					{ UnmanagedType.LPWStr, __marshalType.GetMethod(nameof(Marshal.StringToHGlobalUni), new[] { __stringType }) },
					{ UnmanagedType.LPStr, __marshalType.GetMethod(nameof(Marshal.StringToHGlobalAnsi), new[] { __stringType }) }
				};

				__ptrToStringMethods = new Dictionary<UnmanagedType, MethodInfo> {
					{ UnmanagedType.BStr, __marshalType.GetMethod(nameof(Marshal.PtrToStringBSTR), new[] { __intPtrType }) },
					{ UnmanagedType.LPWStr, __marshalType.GetMethod(nameof(Marshal.PtrToStringUni), new [] { __intPtrType}) },
					{ UnmanagedType.LPStr, __marshalType.GetMethod(nameof(Marshal.PtrToStringAnsi), new [] { __intPtrType}) },
					{ UnmanagedType.LPTStr, __marshalType.GetMethod(nameof(Marshal.PtrToStringAuto), new[] { __intPtrType}) }
				};
			}

			private readonly UnmanagedLibrary _library;
			private readonly Type _interfaceType;
			private readonly Type? _baseType;

			private AssemblyBuilder _assemblyBuilder;
			private ModuleBuilder _moduleBuilder;

			public Context(UnmanagedLibrary library, Type interfaceType, Type? baseType) {
				_library = library;
				_interfaceType = interfaceType;
				_baseType = baseType;

				var buffer = new byte[16];

				__random.NextBytes(buffer);

				var assemblyName = new AssemblyName($"{_interfaceType.Assembly.GetName().Name}.x{BitConverter.ToString(buffer).Replace("-", "")}");
				_assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
				_moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name!);
			}

			public bool TryGenerate([NotNullWhen(true)] out Type? type) {
				if (!_interfaceType.IsInterface) {
					throw new ArgumentException("Type must be an interface.", _interfaceType.Name);
				}

				var interfaceAttribute = _interfaceType.GetCustomAttribute<UnmanagedInterfaceAttribute>();

				if (interfaceAttribute == null) {
					throw new ArgumentException($"Type must have the {nameof(UnmanagedInterfaceAttribute)}");
				}

				var buffer = new byte[16];

				__random.NextBytes(buffer);

				var typeBuilder = _moduleBuilder.DefineType($"{_interfaceType.Namespace}.x{BitConverter.ToString(buffer).Replace("-", "")}.{_interfaceType.Name}", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class, _baseType);

				typeBuilder.AddInterfaceImplementation(_interfaceType);

				var libraryFieldBuilder = GenerateLibraryField(typeBuilder);
				var constructorEmit = GenerateConstructor(typeBuilder, libraryFieldBuilder);

				foreach (var methodInfo in _interfaceType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(m => m.IsAbstract)) {
					GenerateMethod(interfaceAttribute, typeBuilder, libraryFieldBuilder, constructorEmit, methodInfo);
				}

				constructorEmit.Return();
				constructorEmit.CreateConstructor();

				type = typeBuilder.CreateType();

				return type != null;
			}

			private void GenerateMethod(UnmanagedInterfaceAttribute interfaceAttribute, TypeBuilder typeBuilder, FieldBuilder libraryFieldBuilder, Emit constructorEmit, MethodInfo methodInfo) {
				var methodAttribute = methodInfo.GetCustomAttribute<UnmanagedMethodAttribute>();

				var callingConvention = methodAttribute?.CallingConvention ?? interfaceAttribute.CallingConvention;
				var charSet = methodAttribute?.CharSet ?? interfaceAttribute.CharSet;
				var setLastError = methodAttribute?.SetLastError ?? interfaceAttribute.SetLastError;
				var prefix = methodAttribute?.Prefix ?? interfaceAttribute.Prefix;
				var unmanagedName = methodAttribute?.Name ?? $"{prefix}{methodInfo.Name}";
				var parameterInfos = methodInfo.GetParameters();
				var parameterTypes = parameterInfos.Select(p => p.ParameterType).ToArray();

				var (delegateType, delegateInvokeMethod) = GenerateDelegateType(typeBuilder, methodInfo, callingConvention, charSet, setLastError, methodInfo.GetCustomAttribute<SuppressUnmanagedCodeSecurityAttribute>() != null, parameterInfos, parameterTypes);
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
				emit.CreateMethod();

				GenerateDelegateFieldConstructorInitialization(constructorEmit, libraryFieldBuilder, delegateType, delegateFieldBuilder, unmanagedName);
			}

			private FieldBuilder GenerateLibraryField(TypeBuilder typeBuilder) {
				return typeBuilder.DefineField("_library", __unmanagedLibraryType, FieldAttributes.Private | FieldAttributes.InitOnly);
			}

			private Emit GenerateConstructor(TypeBuilder typeBuilder, FieldBuilder libraryField) {
				var emit = Emit.BuildConstructor(__constructorParameterTypes, typeBuilder, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.HideBySig, CallingConventions.HasThis);
				
				emit.LoadArgument(0);
				emit.Call(__objectConstructor);
				emit.LoadArgument(0);
				emit.LoadArgument(1);
				emit.StoreField(libraryField);

				return emit;
			}

			private (Type, MethodInfo) GenerateDelegateType(TypeBuilder typeBuilder, MethodInfo methodInfo, CallingConvention callingConvention, CharSet charSet, bool setLastError, bool suppressCodeSecurity, ParameterInfo[] parameters, Type[] parameterTypes) {
				var name = $"{methodInfo.Name}Delegate";

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

				return (delegateType, delegateType.GetMethod("Invoke"));
			}

			private FieldBuilder GenerateDelegateField(TypeBuilder typeBuilder, MethodInfo methodInfo, Type delegateType) {
				return typeBuilder.DefineField($"{methodInfo.Name}_delegate", delegateType, FieldAttributes.Private | FieldAttributes.InitOnly);
			}

			private void GenerateDelegateFieldConstructorInitialization(Emit emit, FieldBuilder libraryFieldBuilder, Type delegateType, FieldBuilder delegateFieldBuilder, string unmanagedName) {
				emit.LoadArgument(0);
				emit.LoadArgument(0);
				emit.LoadField(libraryFieldBuilder);
				emit.LoadConstant(unmanagedName);
				emit.CallVirtual(__getSymbolMethod);
				emit.Call(__getDelegateForFunctionPointerMethod.MakeGenericMethod(delegateType));
				emit.StoreField(delegateFieldBuilder);
			}

			private static CustomAttributeBuilder CreateCustomAttributeBuilder(CustomAttributeData customAttribute) {
				var namedFields = customAttribute.NamedArguments?.Where(a => a.IsField).ToList() ?? new List<CustomAttributeNamedArgument>();
				var namedProperties = customAttribute.NamedArguments?.Where(a => a.MemberInfo is PropertyInfo).ToList() ?? new List<CustomAttributeNamedArgument>();

				return new CustomAttributeBuilder(
					customAttribute.Constructor,
					customAttribute.ConstructorArguments.Select(a => a.Value).ToArray(),
					namedProperties.Select(p => p.MemberInfo).Cast<PropertyInfo>().ToArray(),
					namedProperties.Select(p => p.TypedValue.Value).ToArray(),
					namedFields.Select(f => f.MemberInfo).Cast<FieldInfo>().ToArray(),
					namedFields.Select(f => f.TypedValue.Value).ToArray()
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
