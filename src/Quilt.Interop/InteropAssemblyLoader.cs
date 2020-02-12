namespace Quilt.Interop {
  using System.Collections.Generic;
  using System.Reflection;

	public class InteropAssemblyLoader {
		private readonly Dictionary<string, List<string>> _unmanagedDllAliases = new Dictionary<string, List<string>>();

		public void AddAliases(string name, params string[] aliases) {
			if(!_unmanagedDllAliases.TryGetValue(name, out var aliasList)) {
				_unmanagedDllAliases[name] = aliasList = new List<string>();
			}

			aliasList.AddRange(aliases);
		}

		public InteropAssembly Load(AssemblyName assemblyName) {
			var interopAssembly = new InteropAssembly(assemblyName);
			var context = new InteropAssemblyLoadContext(interopAssembly, _unmanagedDllAliases);

			context.LoadFromAssemblyName(assemblyName);

			return interopAssembly;
		}
	}
}
