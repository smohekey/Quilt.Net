namespace Quilt.Mac.CodeGen {
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Reflection.Emit;

	using Quilt.Mac.Foundation;
	using Quilt.Utilities;

	public sealed partial class Generator {
		private static readonly Lazy<Generator> __lazyInstance = new Lazy<Generator>(() => new Generator());

		public static Generator Instance => __lazyInstance.Value;

		private readonly Dictionary<Type, Type> _concreteTypes = new Dictionary<Type, Type>();

		private readonly AssemblyName _assemblyName;
		private readonly AssemblyBuilder _assemblyBuilder;
		private readonly ModuleBuilder _moduleBuilder;

		private Generator() {
			var assembly = typeof(Generator).Assembly;

			_assemblyName = assembly.GetName().GenerateRandomVariant();
			_assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(_assemblyName, AssemblyBuilderAccess.RunAndCollect);
			_moduleBuilder = _assemblyBuilder.DefineDynamicModule(_assemblyName.Name);
		}

		public Type GetConcreteType(Type type) {
			lock (_concreteTypes) {
				if (!_concreteTypes.TryGetValue(type, out var concreteType)) {
					_concreteTypes[type] = concreteType = GenerateConcreteType(type);
				}

				return concreteType;
			}
		}

		private Type GenerateConcreteType(Type type) {
			return new GenerationContext(this, _moduleBuilder, type).ConcreteType;
		}

		public TAbstract Create<TAbstract, T1>(T1 parameter1) where TAbstract : NSObject {
			return (TAbstract)Create(typeof(TAbstract), parameter1);
		}

		public NSObject Create<T1>(Type abstractType, T1 arg1) {
			var concreteType = GetConcreteType(abstractType);
			var constructor = concreteType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(T1) }, null);

			return (NSObject)constructor.Invoke(new object[] { arg1! });
		}
	}
}
