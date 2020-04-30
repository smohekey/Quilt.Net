namespace Quilt.Mac.CodeGen {
	using System;
	using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
	using System.Reflection.Emit;
  using System.Text;
  using Quilt.Mac.Foundation;
	using Quilt.Mac.ObjectiveC;
  using Quilt.Utilities;
  using Sigil.NonGeneric;

  public partial class GenerationContext {
		public List<GeneratedConstructor> Constructors { get; } = new List<GeneratedConstructor>();
		public List<ImportedMethod> ImportedMethods { get; } = new List<ImportedMethod>();
		public List<ExportedMethod> ExportedMethods { get; } = new List<ExportedMethod>();

		public Generator Generator { get; }
		public ModuleBuilder ModuleBuilder { get; }
		public Type BaseType { get; }

		private readonly bool _hasExports;
		public bool HasExports => _hasExports;

		public bool IsMetaClass { get; }
		/// <summary>
		/// When IsMetaClass is true, this is a reference to the containing type of the meta class type.
		/// </summary>
		public Type ActualType { get; }

		public Class Class { get; }

		public Class Superclass { get; }

		public string TypeName { get; }

		public Type ImportsType { get; }
		public Type? ExportsType { get; }
		public Type ConcreteType { get; }

		public GenerationContext(Generator generator, ModuleBuilder moduleBuilder, Type baseType) {
			Generator = generator ?? throw new ArgumentNullException(nameof(generator));
			ModuleBuilder = moduleBuilder ?? throw new ArgumentNullException(nameof(moduleBuilder));
			BaseType = baseType ?? throw new ArgumentNullException(nameof(baseType));

			IsMetaClass = typeof(NSObject.MetaClass).IsAssignableFrom(BaseType);

			if (IsMetaClass) {
				var declaringType = BaseType.DeclaringType;

				if (declaringType.IsGenericTypeDefinition) {
					ActualType = declaringType.MakeGenericType(BaseType.GetGenericArguments());
				} else {
					ActualType = declaringType;
				}
			} else {
				ActualType = BaseType;
			}

			if (!(ActualType.GetCustomAttribute<ClassAttribute>() is ClassAttribute classAttribute)) {
				throw new NotSupportedException($"Type {ActualType} isn't attributed with ClassAttribute.");
			}

			var typeName = SanitizeTypeName(ActualType);

			TypeName = IsMetaClass ? typeName + "_MetaClass" : typeName;

			var className = classAttribute.Name ?? SanitizeClassName(ActualType.Name);

			GatherExportedMethods(out _hasExports);

			if (HasExports) {
				var abstractBaseType = ActualType.BaseType;
				var superclass = default(Class);

				if (abstractBaseType == Types.NSObject || (abstractBaseType.IsGenericType && (abstractBaseType.GetGenericTypeDefinition() == Types.NSObjectGeneric1 || abstractBaseType.GetGenericTypeDefinition() == Types.NSObjectGeneric2))) {
					superclass = Runtime.GetClass(nameof(NSObject));

					if (superclass == null) {
						throw new ClassNotFoundException(abstractBaseType);
					}
				} else {
					_ = Generator.GetConcreteType(ActualType.BaseType);

					var baseTypeName = SanitizeClassName(abstractBaseType.Name);

					superclass = Runtime.GetClass(abstractBaseType.Name);

					if (superclass == null) {
						throw new ClassNotFoundException(abstractBaseType);
					}
				}

				Superclass = superclass;

				Class = Runtime.AllocateClassPair(superclass, className, 0) ?? throw new InvalidOperationException();
			} else {
				Class = (IsMetaClass ? Runtime.GetMetaClass(className) : Runtime.GetClass(className)) ?? throw new ClassNotFoundException(BaseType);

				Superclass = Class.Superclass!;
			}

			GatherImportedMethods();

			ImportsType = GenerateImportsType();

			var objectRefsField = default(FieldBuilder);

			ExportsType = HasExports ? GenerateExportsType(out objectRefsField) : null;

			ConcreteType = GenerateConcreteType(objectRefsField);
		}

		private static string SanitizeTypeName(Type type) {
			var builder = new StringBuilder();

			builder.Append(SanitizeClassName(type.Name.Replace('`', '_')));

			if(type.IsGenericType) {
				foreach(var typeArgument in type.GetTypeInfo().GetGenericArguments()) {
					builder.Append('_');
					builder.Append(typeArgument.Name);
				}
			}

			return builder.ToString();
		}

		private static string SanitizeClassName(string name) {
			var index = name.IndexOf('`');

			if (index > 0) {
				return name.Substring(0, index);
			}

			return name;
		}

		private Type GenerateConcreteType(FieldBuilder? objectRefsField) {
			var typeBuilder = ModuleBuilder.DefineConcreteType(BaseType, TypeName);

			GenerateConcreteTypeInitializer(typeBuilder, out var typeInitEmit, out var classField, out var selectorsField);
			GenerateConcreteTypeConstructors(typeBuilder, objectRefsField);
			GenerateConcreteTypeImportedMethods(typeBuilder, typeInitEmit, selectorsField);
			GenerateConcreteTypeExportedMethods(typeBuilder, typeInitEmit, classField);

			typeInitEmit.Return();
			typeInitEmit.CreateTypeInitializer();

			if (HasExports) {
				Runtime.RegisterClassPair(Class);
			}

			return typeBuilder.CreateType();
		}

		private void GenerateConcreteTypeInitializer(TypeBuilder typeBuilder, out Emit emit, out FieldBuilder classField, out FieldBuilder selectorsField) {
			classField = typeBuilder.DefineField("__class", Types.Class, FieldAttributes.Static | FieldAttributes.Private | FieldAttributes.InitOnly);
			selectorsField = typeBuilder.DefineField("__selectors", Types.SelectorArray, FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly);

			emit = Emit.BuildTypeInitializer(typeBuilder);

			emit.LoadConstant(Class.Name);
			emit.Call(Methods.Runtime_GetClass);
			emit.StoreField(classField);
			emit.LoadConstant(ImportedMethods.Count);
			emit.NewArray<Selector>();
			emit.StoreField(selectorsField);
		}

		private void GenerateConcreteTypeConstructors(TypeBuilder typeBuilder, FieldBuilder? objectRefsField) {
			foreach (var baseConstructor in BaseType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
				var parameters = baseConstructor.GetParameters();
				var parameterTypes = parameters.Select(p => p.ParameterType).ToArray();
				var emit = Emit.BuildConstructor(parameterTypes, typeBuilder, baseConstructor.Attributes & ~MethodAttributes.Private | MethodAttributes.Public);

				for (var i = 0; i <= parameters.Length; i++) {
					emit.LoadArgument((ushort)i);
				}

				emit.Call(baseConstructor);

				if (objectRefsField != null) {
					emit.LoadField(objectRefsField);
					emit.LoadArgument(1);
					emit.LoadArgument(0);
					emit.Call(Methods.ObjectRefList_AddReference);
				}

				emit.Return();

				Constructors.Add(new GeneratedConstructor(emit.CreateConstructor(), parameterTypes));
			}
		}
	}
}
