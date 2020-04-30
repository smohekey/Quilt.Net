namespace Quilt.Mac.CodeGen {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
  using System.Reflection.Emit;
  using Quilt.Mac.ObjectiveC;
  using Sigil.NonGeneric;

  public partial class GenerationContext {
		private void GatherExportedMethods(out bool hasExports) {
			var lookup = BaseType.GetTypeInfo().ImplementedInterfaces.Select(i => BaseType.GetInterfaceMap(i))
				.SelectMany(im => im.TargetMethods.Select((tm, i) => (TargetMethod: tm, InterfaceMethod: im.InterfaceMethods[i])))
				.ToDictionary(t => t.TargetMethod, t => t.InterfaceMethod);

			hasExports = false;

			foreach (var baseMethod in BaseType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).Where(m => !m.IsAbstract)) {
				if (!(FindExportAttribute(lookup, baseMethod) is ExportAttribute exportAttribute)) {
					continue;
				}

				hasExports = true;

				var selector = exportAttribute.Name ?? Selector.From(baseMethod);

				var parameters = baseMethod.GetParameters();
				var parameterTypes = new[] { Types.IntPtr, Types.Selector }.Concat(parameters.Select(TransformType)).ToArray();

				ExportedMethods.Add(new ExportedMethod(baseMethod, parameterTypes, selector));
			}

			Type TransformType(ParameterInfo parameter) {
				var type = parameter.ParameterType;

				if (type == Types.String) {
					return Types.ByteArray;
				} else if (Types.NSObject.IsAssignableFrom(type)) {
					return Types.IntPtr;
				} else {
					return type;
				}
			}
		}

		private static ExportAttribute? FindExportAttribute(IDictionary<MethodInfo, MethodInfo> lookup, MethodInfo method) {
			var exportAttribute = method.GetCustomAttribute<ExportAttribute>();

			if (exportAttribute != null) {
				return exportAttribute;
			}

			if (lookup.TryGetValue(method, out var interfaceMethod) && interfaceMethod.GetCustomAttribute<ExportAttribute>() is ExportAttribute exportAttribute1) {
				return exportAttribute1;
			}

			return null;
		}

		private Type GenerateExportsType(out FieldBuilder objectRefsField) {
			var typeName = $"{TypeName}Exports";

			var typeBuilder = ModuleBuilder.DefineType(typeName, TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Public);

			GenerateExportsTypeInitializer(typeBuilder, out objectRefsField);

#if DEBUG
			Console.WriteLine($"** Attempting to generate exports type {typeName}");
#endif

			foreach(var (baseMethod, parameterTypes, selector) in ExportedMethods) {
#if DEBUG
				Console.WriteLine($"Attemping to export method {baseMethod.Name}.");
#endif
				var parameters = baseMethod.GetParameters();

				var emit = Emit.BuildStaticMethod(baseMethod.ReturnType, parameterTypes, typeBuilder, baseMethod.Name, MethodAttributes.Static | MethodAttributes.Public);

#if DEBUG
				emit.WriteLine($"Export {baseMethod.Name} called");
#endif

				emit.LoadField(objectRefsField);
				emit.LoadArgument(0);
				emit.Call(Methods.ObjectRefList_GetObject);
				emit.CastClass(BaseType);

				for (var i = 0; i < parameters.Length; i++) {
					var parameter = parameters[i];
					var type = parameter.ParameterType;

					emit.LoadArgument((ushort)(i + 2));

					if (type == Types.String) {
						emit.Call(Methods.Marshal_PtrToStringUTF8);
					} else if (Types.NSObject.IsAssignableFrom(type)) {
						TypeMarshaler.EmitNativeToManaged(this, emit, type);
					}
				}

				emit.Call(baseMethod);
				emit.Return();

				emit.CreateMethod();
			}

#if DEBUG
			Console.WriteLine($"  Generated exports type {typeName}.");
#endif

			return typeBuilder.CreateType();
		}

		private void GenerateExportsTypeInitializer(TypeBuilder typeBuilder, out FieldBuilder objectRefsField) {
			var emit = Emit.BuildTypeInitializer(typeBuilder);

			objectRefsField = typeBuilder.DefineField("__objectRefs", Types.ObjectRefList, FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.InitOnly);

			if (!Class.AddInstanceVariable<int>("__objectRef")) {
				throw new NotSupportedException();
			}

			var objectRefVar = Class.GetInstanceVariable("__objectRef")!;

			var offset = objectRefVar.Offset;

			emit.LoadConstant(offset);
			emit.NewObject(Types.ObjectRefList, Types.Int32);
			emit.StoreField(objectRefsField);

			emit.Return();

			emit.CreateTypeInitializer();
		}

		private void GenerateConcreteTypeExportedMethods(TypeBuilder typeBuilder, Emit typeInitEmit, FieldBuilder classField) {
			foreach (var (baseMethod, thunkParameterTypes, selector) in ExportedMethods) {
				var thunkMethod = ExportsType.GetMethod(baseMethod.Name, thunkParameterTypes);
				var parameterTypes = thunkMethod.GetParameters().Select(p => p.ParameterType).ToArray();
				var thunkType = GenerateDelegateType(thunkMethod);
				var thunkField = typeBuilder.DefineField($"{baseMethod}Thunk", thunkType, FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly);

				typeInitEmit.LoadNull();
				typeInitEmit.LoadFunctionPointer(thunkMethod);
				typeInitEmit.NewObject(thunkType, new[] { Types.Object, Types.IntPtr });
				typeInitEmit.StoreField(thunkField);
				typeInitEmit.LoadField(classField);
				typeInitEmit.LoadConstant(selector.Name);
				typeInitEmit.NewObject(Types.Selector, Types.String);
				typeInitEmit.LoadField(thunkField);
				typeInitEmit.Call(Methods.Class_AddMethod.MakeGenericMethod(thunkType));
				typeInitEmit.Pop();
			}
		}

		private Type GenerateDelegateType(MethodInfo method) {
			var name = $"{method.Name}Delegate";

			var delegateTypeBuilder = ModuleBuilder.DefineType(name, TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.AnsiClass | TypeAttributes.AutoClass, typeof(MulticastDelegate));

			/*delegateTypeBuilder.SetCustomAttribute(
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
			}*/

			var delegateConstructorBuilder = delegateTypeBuilder.DefineConstructor(MethodAttributes.RTSpecialName | MethodAttributes.HideBySig | MethodAttributes.Public, CallingConventions.Standard, new[] { Types.Object, Types.IntPtr });

			delegateConstructorBuilder.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

			var parameters = method.GetParameters();
			var parameterTypes = parameters.Select(p => {
				var type = p.ParameterType;

				if (type == Types.String) {
					return Types.IntPtr;
				} else if (Types.NSObject.IsAssignableFrom(type)) {
					return Types.IntPtr;
				}

				return type;
			}).ToArray();

			//var returnType = method.ReturnType == __stringType ? __intPtrType : methodInfo.ReturnType;

			var returnType = method.ReturnType;
			var delegateInvokeMethodBuilder = delegateTypeBuilder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, returnType, parameterTypes);

			delegateInvokeMethodBuilder.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

			return delegateTypeBuilder.CreateType();
		}
	}
}
