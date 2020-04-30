namespace Quilt.Utilities {
  using System.Reflection;

	public static class AssemblyNameExtensions {
		public static AssemblyName GenerateRandomVariant(this AssemblyName @this) {
			return new AssemblyName($"{@this.Name}.x{RandomUtils.RandomHexString(16)}");
		}
	}
}
