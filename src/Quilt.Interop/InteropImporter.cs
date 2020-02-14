namespace Quilt.Interop {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Reflection.Emit;

	public class InteropImporter {
		private readonly static Random __random = new Random();

		private readonly Dictionary<Type, Type> _implementedTypes = new Dictionary<Type, Type>();

		public TAbstract Import<TAbstract>() where TAbstract : class {
			var type = typeof(TAbstract);

			if (!_implementedTypes.TryGetValue(type, out var implementedType)) {
				if (!type.IsAbstract) {
					throw new ArgumentException("Type must be abstract.", nameof(TAbstract));
				}

				var unmanagedAttribute = type.GetCustomAttribute<UnmanagedInterface>();

				if (unmanagedAttribute == null) {
					throw new ArgumentException($"Type must have the {nameof(UnmanagedInterface)}");
				}

				if (!UnmanagedDll.TryLoad(unmanagedAttribute.Name, out var unmanagedDll, unmanagedAttribute.Aliases)) {
					throw new InvalidOperationException();
				}

				var buffer = new byte[16];

				__random.NextBytes(buffer);

				var assemblyName = new AssemblyName($"{type.Assembly.GetName().Name}.x{BitConverter.ToString(buffer).Replace("-", "")}");
				var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
				var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

				__random.NextBytes(buffer);

				var typeBuilder = moduleBuilder.DefineType($"{type.Namespace}.x{BitConverter.ToString(buffer).Replace("-", "")}.{type.Name}", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class, type);

				foreach (var methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(m => m.IsAbstract)) {
					var importAttribute = methodInfo.GetCustomAttribute<UnmanagedImportAttribute>();
					var callingConvention = importAttribute.CallingConvention ?? unmanagedAttribute.CallingConvention;
					var charSet = importAttribute.CharSet ?? unmanagedAttribute.CharSet;
					var prefix = importAttribute.Prefix ?? unmanagedAttribute.Prefix;

					if (importAttribute == null) {
						throw new InvalidOperationException($"Method {methodInfo.Name} is abstract, but doesn't have {nameof(UnmanagedImportAttribute)}");
					}

					var parameters = methodInfo.GetParameters();
					var parameterTypes = parameters.Select(p => p.ParameterType).ToArray();

					var pinvokeName = $"{prefix}{methodInfo.Name}";

					var pmethodBuilder = typeBuilder.DefinePInvokeMethod(pinvokeName, unmanagedDll.LoadedName, MethodAttributes.Static | MethodAttributes.Private, CallingConventions.Standard, methodInfo.ReturnType, parameterTypes, callingConvention, charSet);

					var methodBuilder = typeBuilder.DefineMethod(methodInfo.Name, methodInfo.Attributes & ~(MethodAttributes.Abstract | MethodAttributes.NewSlot), methodInfo.ReturnType, parameterTypes);
					var il = methodBuilder.GetILGenerator();

					for (ushort i = 0; i < parameters.Length; i++) {
						il.Emit(OpCodes.Ldarg, i);
					}

					il.EmitCall(OpCodes.Call, pmethodBuilder, Array.Empty<Type>());
					il.Emit(OpCodes.Ret); ;

					typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
				}

				implementedType = typeBuilder.CreateType();
			}

			return (TAbstract)Activator.CreateInstance(implementedType);
		}
	}
}
