namespace Quilt.Mac.CodeGen {
	using System;
	using System.Linq;
	using System.Reflection;
	using System.Reflection.Emit;
	using System.Runtime.InteropServices;
	using Quilt.Mac.Foundation;
	using Quilt.Mac.ObjectiveC;
	using Sigil.NonGeneric;

	public partial class GenerationContext {
		private void GatherImportedMethods() {
			var i = 0;

			foreach (var baseMethod in BaseType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).Where(m => m.IsAbstract)) {
				var selector = default(Selector);

				if (baseMethod.GetCustomAttribute<ImportAttribute>() is ImportAttribute importAttribute) {
					selector = importAttribute.Name ?? Selector.From(baseMethod);

					var current = Class;
					var found = false;

					do {
						if (current.GetInstanceMethod(selector) is Method method) {
							found = true;

							break;
						}

						if (current.Superclass is Class super) {
							current = super;
						} else {
							break;
						}
					} while (current.Name != nameof(NSObject));

					if (!found) {
						throw new MethodNotFoundException(Class, selector);
					}
				} else {
					// see if this method is a getter/setter for a property with the PropertyAttribute

					if (!baseMethod.IsSpecialName) {
						throw new NotSupportedException($"Type {BaseType.Name} has abstract method {baseMethod.Name} without {nameof(ImportAttribute)}");
					}

					var isGetter = baseMethod.Name.StartsWith("get_", StringComparison.InvariantCulture);
					var isSetter = baseMethod.Name.StartsWith("set_", StringComparison.InvariantCulture);

					if (!(isGetter || isSetter)) {
						throw new NotSupportedException($"Type {BaseType.Name} has abstract method {baseMethod.Name} without {nameof(ImportAttribute)}");
					}

					var propertyName = baseMethod.Name.Substring(4);

					if (!(BaseType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy) is PropertyInfo propertyInfo)) {
						throw new NotSupportedException($"Type {BaseType.Name} has abstract method {baseMethod.Name} without a matching property {propertyName}");
					}

					if (!(propertyInfo.GetCustomAttribute<ImportAttribute>() is ImportAttribute importAttribute1)) {
						throw new NotSupportedException($"Type {BaseType.Name} has abstract method {baseMethod.Name} without a matching property {propertyName} with {nameof(ImportAttribute)}");
					}

					var objcPropertyName = importAttribute1.Name ?? $"{char.ToLowerInvariant(propertyName[0])}{propertyName.Substring(1)}";

					if (!(Class.GetProperty(objcPropertyName) is Property property)) {
						throw new PropertyNotFoundException(Class, objcPropertyName);
					}

					if (isGetter) {
						selector = property.Getter?.Name ?? throw new NotSupportedException($"Type {BaseType.Name} defines a getter for property {propertyName} which doesn't have a matching ObjectiveC getter.");
					} else if (isSetter) {
						selector = property.Setter?.Name ?? Class.GetInstanceMethod($"set{propertyName}:")?.Name ?? throw new NotSupportedException($"Type {BaseType.Name} defines a setter for property {propertyName} which doesn't have a matching ObjectiveC setter.");
					}
				}

				var returnParameter = baseMethod.ReturnParameter;
				var returnTypeMarshaler = TypeMarshaler.CreateTypeMarshaler(this, returnParameter);
				var returnType = returnTypeMarshaler.NativeReturnType;
				var parameterTypeMarshalers = baseMethod.GetParameters().Select(p => TypeMarshaler.CreateTypeMarshaler(this, p)).ToArray();
				var parameterTypes = new[] { Types.IntPtr, Types.Selector }.Concat(parameterTypeMarshalers.Select(tm => tm.NativeParameterType)).ToArray();

				var functionName = returnTypeMarshaler.MsgSendName;

#if DEBUG
				var parameters = parameterTypes.Select(p => p.Name).Zip(new[] { "self", "selector" }.Concat(baseMethod.GetParameters().Select(p => p.Name)), (first, second) => $"{first} {second}");

				Console.WriteLine($"  {returnType.Name} {baseMethod.Name}({string.Join(", ", parameters)});");
				Console.WriteLine($"  {baseMethod.ReturnType.Name} {baseMethod.Name}({string.Join(", ", baseMethod.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"))});");
#endif

				ImportedMethods.Add(new ImportedMethod(baseMethod, parameterTypes, returnTypeMarshaler, parameterTypeMarshalers, selector, i++));
			}
		}

		private Type GenerateImportsType() {
			var typeName = $"{TypeName}Imports";

			var typeBuilder = ModuleBuilder.DefineType(typeName, TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Public);

#if DEBUG
			Console.WriteLine($"** Attempting to generate imports type {typeName}");
#endif

			foreach (var (baseMethod, parameterTypes, returnTypeMarshaler, parameterTypeMarshalers, selector, selectorIndex) in ImportedMethods) {
#if DEBUG
				Console.WriteLine($"  Generating PInvoke method {baseMethod.Name} for selector {selector} of class {Class.Name}");
#endif

				var returnParameter = baseMethod.ReturnParameter;
				var returnType = returnTypeMarshaler.NativeReturnType;
				var functionName = returnTypeMarshaler.MsgSendName;

				var pinvokeMethodBuilder = typeBuilder.DefinePInvokeMethod(
					baseMethod.Name,
					Runtime.LIBRARY,
					functionName,
					MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.PinvokeImpl,
					CallingConventions.Standard,
					returnType,
					parameterTypes,
					CallingConvention.Cdecl,
					CharSet.Unicode
				);

				pinvokeMethodBuilder.SetImplementationFlags(pinvokeMethodBuilder.GetMethodImplementationFlags() | MethodImplAttributes.PreserveSig);
			}

			var pinvokeType = typeBuilder.CreateType();

#if DEBUG
			Console.WriteLine($"  Generated PInvoke type {TypeName}PInvoke.");
#endif

			return pinvokeType;
		}

		private void GenerateConcreteTypeImportedMethods(TypeBuilder typeBuilder, Emit typeInitEmit, FieldBuilder selectorsField) {
			foreach (var (baseMethod, pinvokeParameterTypes, returnTypeMarshaler, parameterTypeMarshalers, selector, selectorIndex) in ImportedMethods) {
				typeInitEmit.LoadField(selectorsField);
				typeInitEmit.LoadConstant(selectorIndex);
				typeInitEmit.LoadConstant(selector.Name);
				typeInitEmit.NewObject<Selector, string>();
				typeInitEmit.StoreElement<Selector>();

				var pinvokeMethod = ImportsType.GetMethod(baseMethod.Name, pinvokeParameterTypes);
				var returnType = baseMethod.ReturnType;
				var parameters = baseMethod.GetParameters();
				var parameterTypes = parameters.Select(p => p.ParameterType).ToArray();

				var emit = Emit.BuildInstanceMethod(returnType, parameterTypes, typeBuilder, baseMethod.Name, baseMethod.Attributes & ~MethodAttributes.Abstract);

				for (var i = 0; i < parameters.Length; i++) {
					parameterTypeMarshalers[i].EmitParameterSetup(emit, (ushort)(i + 1));
				}

#if DEBUG
				emit.WriteLine($"Calling import {selector.Name}");
#endif

				emit.LoadArgument(0);
				emit.Call(Methods.NSObject_GetHandle);
				emit.LoadField(selectorsField);
				emit.LoadConstant(selectorIndex);
				emit.LoadElement<Selector>();

				for (var i = 0; i < parameters.Length; i++) {
					var parameter = parameters[i];
					var index = (ushort)(i + 1);

					if (!parameter.IsOut || parameter.ParameterType.IsByRef) {
						parameterTypeMarshalers[i].EmitMarshalParameterIn(emit, index);
					}

					if (parameter.IsOut) {
						parameterTypeMarshalers[i].EmitMarshalParameterOutPrologue(emit, index);
					}
				}

				emit.Call(pinvokeMethod);

				for (var i = 0; i < parameters.Length; i++) {
					var parameter = parameters[i];
					var index = (ushort)(i + 1);

					if (parameter.IsOut) {
						parameterTypeMarshalers[i].EmitMarshalParameterOutEpilogue(emit, index);
					}
				}

				for (var i = 0; i < parameters.Length; i++) {
					var index = (ushort)(i + 1);

					parameterTypeMarshalers[i].EmitParameterCleanup(emit, index);
				}

				var returnParameter = baseMethod.ReturnParameter;

				returnTypeMarshaler.EmitMarshalReturnParameter(emit);

				emit.Return();

				var newMethod = emit.CreateMethod();

				typeBuilder.DefineMethodOverride(newMethod, baseMethod);
			}
		}
	}
}
