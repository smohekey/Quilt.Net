namespace Quilt.Utilities {
	using System;
  using System.Reflection;
  using System.Reflection.Emit;
  
	public static class ModuleBuilderExtensions {
		public static TypeBuilder DefineConcreteType(this ModuleBuilder @this, Type parentType, string name = null) {
			name ??= parentType.Name;

			return @this.DefineType($"{parentType.Namespace}.x{RandomUtils.RandomHexString(16)}.{name}", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class, parentType);
		}
	}
}
