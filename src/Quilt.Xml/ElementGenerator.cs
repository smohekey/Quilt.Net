namespace Quilt.Xml {
	using System;
	using System.Reflection;
	using System.Reflection.Emit;

	using Sigil.NonGeneric;

	class ElementGenerator {
		private readonly Random _random = new Random();

		public Type Generate(Type type) {
			return new Context(this, type).Generate();
		}

		private class Context {
			private static readonly Type __voidType = typeof(void);
			private static readonly Type __applicationType = typeof(Application);
			private static readonly Type __quiltElementType = typeof(Element);
			private static readonly MethodInfo __invokePropertyChangedMethod = __quiltElementType.GetMethod(Element.INVOKE_PROPERTY_CHANGED_NAME, BindingFlags.NonPublic | BindingFlags.Instance)!;
			private static readonly Type[] __constructorParameterTypes = new[] { __applicationType };
			private static readonly Type[] __emptyTypes = new Type[] { };

			private readonly ElementGenerator _generator;
			private readonly Type _type;

			private readonly AssemblyName _assemblyName;
			private readonly AssemblyBuilder _assemblyBuilder;
			private readonly ModuleBuilder _moduleBuilder;
			private readonly TypeBuilder _typeBuilder;

			public Context(ElementGenerator generator, Type type) {
				_generator = generator;
				_type = type;

				byte[] buffer = new byte[16];

				_generator._random.NextBytes(buffer);

				_assemblyName = new AssemblyName($"{_type.Assembly.GetName().Name}.x{BitConverter.ToString(buffer).Replace("-", "")}");
				_assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(_assemblyName, AssemblyBuilderAccess.RunAndCollect);
				_moduleBuilder = _assemblyBuilder.DefineDynamicModule(_assemblyName.FullName);

				_generator._random.NextBytes(buffer);

				_typeBuilder = _moduleBuilder.DefineType($"{_type.Namespace}.x{BitConverter.ToString(buffer).Replace("-", "")}.{_type.Name}", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class, _type);

			}

			public Type Generate() {
				if (!_type.IsAbstract) {
					throw new InvalidOperationException();
				}

				GenerateConstructor();
				GenerateProperties();

				return _typeBuilder.CreateTypeInfo() ?? throw new GenerationFailedException();
			}

			private void GenerateConstructor() {
				var constructorInfo = _type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, __constructorParameterTypes, null);

				if (constructorInfo == null) {
					throw new GenerationFailedException($"Type {_type.FullName} doesn't have the required constructor.");
				}

				var emit = Emit.BuildConstructor(__constructorParameterTypes, _typeBuilder, constructorInfo.Attributes);

				emit.LoadArgument(0);
				emit.LoadArgument(1);
				emit.Call(constructorInfo);
				emit.Return();

				emit.CreateConstructor();
			}

			private void GenerateProperties() {
				foreach (var propertyInfo in _type.GetProperties()) {
					if (propertyInfo.GetCustomAttribute<QuiltAttributeAttribute>() is QuiltAttributeAttribute attribute) {
						if (propertyInfo.GetGetMethod() is MethodInfo getMethodInfo) {
							GeneratePropertyGetMethod(propertyInfo, attribute, getMethodInfo);
						}

						if (propertyInfo.GetSetMethod() is MethodInfo setMethodInfo) {
							GeneratePropertySetMethod(propertyInfo, attribute, setMethodInfo);
						}
					}
				}
			}

			private void GeneratePropertyGetMethod(PropertyInfo propertyInfo, QuiltAttributeAttribute attribute, MethodInfo getMethodInfo) {
				var propertyType = propertyInfo.PropertyType;
				var emit = Emit.BuildInstanceMethod(propertyType, __emptyTypes, _typeBuilder, getMethodInfo.Name, getMethodInfo.Attributes & ~MethodAttributes.Abstract);

				if (getMethodInfo.IsVirtual) {
					// load value from base
					emit.LoadArgument(0);
					emit.Call(getMethodInfo);
					emit.Return();
				} else {
					throw new GenerationFailedException($"Type '{propertyInfo.DeclaringType!.FullName}' has a property '{propertyInfo.Name}' attributed with QuiltAttribute that is not virtual.");
				}

				_typeBuilder.DefineMethodOverride(emit.CreateMethod(), getMethodInfo);
			}

			private void GeneratePropertySetMethod(PropertyInfo propertyInfo, QuiltAttributeAttribute attribute, MethodInfo setMethodInfo) {
				var propertyType = propertyInfo.PropertyType;
				var emit = Emit.BuildInstanceMethod(__voidType, new[] { propertyType }, _typeBuilder, setMethodInfo.Name, setMethodInfo.Attributes & ~MethodAttributes.Abstract);

				if (!setMethodInfo.IsVirtual) {
					throw new GenerationFailedException($"Type '{propertyInfo.DeclaringType!.FullName}' has a property '{propertyInfo.Name}' attributed with QuiltAttribute that is not virtual.");
				}

				// set value on base
				emit.LoadArgument(0);
				emit.LoadArgument(1);
				emit.Call(setMethodInfo);

				// notify of property change
				emit.LoadArgument(0);
				emit.LoadConstant(propertyInfo.Name);
				emit.Call(__invokePropertyChangedMethod);

				emit.Return();

				_typeBuilder.DefineMethodOverride(emit.CreateMethod(), setMethodInfo);
			}
		}
	}
}
