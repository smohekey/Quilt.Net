namespace Quilt.Interop {
	using System.Collections.Generic;
	using System.Reflection;
	using Microsoft.Extensions.Logging;

	public class InteropAssembly {
		internal readonly Dictionary<string, UnmanagedDll> _unmanagedDlls = new Dictionary<string, UnmanagedDll>();

		public AssemblyName AssemblyName { get; }
		public Assembly Assembly { get; internal set; }

		private InteropAssembly(AssemblyName assemblyName) {
			AssemblyName = assemblyName;
		}

		public TAbstract Implement<TAbstract>() where TAbstract : class {

		}

		public static InteropAssembly Load(ILogger logger, AssemblyName assemblyName) {
			var interopAssembly = new InteropAssembly(assemblyName);
			var context = new InteropAssemblyLoadContext(logger, interopAssembly);

			context.LoadFromAssemblyName(assemblyName);

			return interopAssembly;
		}
	}
}
