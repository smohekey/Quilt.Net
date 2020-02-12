namespace Quilt.Interop {
	using System.Collections.Generic;
	using System.Reflection;
	using Microsoft.Extensions.Logging;

	public class InteropAssembly {
		internal readonly Dictionary<string, UnmanagedDll> _unmanagedDlls = new Dictionary<string, UnmanagedDll>();

		public AssemblyName AssemblyName { get; }
		public Assembly? Assembly { get; internal set; }

		internal InteropAssembly(AssemblyName assemblyName) {
			AssemblyName = assemblyName;
		}

		/*public TProxy CreateProxy<TProxy>() where TProxy : class {
			return null;
		}*/

		public UnmanagedDll? GetUnmanagedDll(string name) {
			_unmanagedDlls.TryGetValue(name, out var unmanagedDll);

			return unmanagedDll;
		}
	}
}
