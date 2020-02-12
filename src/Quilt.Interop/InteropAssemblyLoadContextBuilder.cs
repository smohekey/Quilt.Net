namespace Quilt.Interop {
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public class InteropAssemblyLoadContextBuilder {
		private readonly List<string> _sharedAssemblyNames = new List<string>();
		private readonly Dictionary<string, List<string>> _unmanagedDllAliases = new Dictionary<string, List<string>>();

		public string BaseAssemblyPath { get; set; }
		public InteropAssemblyLoadContextBuilder(string baseAssemblyPath) {
			BaseAssemblyPath = baseAssemblyPath;
		}

		public InteropAssemblyLoadContextBuilder AddSharedAssembly(string name) {
			_sharedAssemblyNames.Add(name);

			return this;
		}

		public InteropAssemblyLoadContextBuilder AddSharedAssembly(AssemblyName assemblyName) {
			return AddSharedAssembly(assemblyName.Name);
		}

		public InteropAssemblyLoadContextBuilder AddUnmanagedDllAliases(string name, params string[] aliases) {
			if (!_unmanagedDllAliases.TryGetValue(name, out var aliasList)) {
				_unmanagedDllAliases[name] = aliasList = new List<string>();
			}

			aliasList.AddRange(aliases);

			return this;
		}

		public InteropAssemblyLoadContext Build() {
			return new InteropAssemblyLoadContext(BaseAssemblyPath, _sharedAssemblyNames, _unmanagedDllAliases, Enumerable.Empty<string>());
		}
	}
}
