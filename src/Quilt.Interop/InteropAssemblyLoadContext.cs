namespace Quilt.Interop {
	using System.IO;
	using System;
	using System.Runtime.InteropServices;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.Loader;
	using Microsoft.Extensions.Logging;

	public class InteropAssemblyLoadContext : AssemblyLoadContext {
		private static readonly string[] __prefixes;
		private static readonly string[] __suffixes;

		static InteropAssemblyLoadContext() {
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
				__prefixes = new[] { "" };
				__suffixes = new[] { ".dll" };
			} else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
				__prefixes = new[] { "", "lib" };
				__suffixes = new[] { ".so", ".so.1" };
			} else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
				__prefixes = new[] { "", "lib" };
				__suffixes = new[] { ".dylib" };
			} else {
				__prefixes = Array.Empty<string>();
				__suffixes = Array.Empty<string>();
			}
		}

		private readonly ILogger _logger;
		private readonly InteropAssembly _interopAssembly;

		public InteropAssemblyLoadContext(ILogger logger, InteropAssembly interopAssembly) {
			_logger = logger;
			_interopAssembly = interopAssembly;
		}

		protected override Assembly Load(AssemblyName assemblyName) {
			if (assemblyName == _interopAssembly.AssemblyName) {
				_interopAssembly.Assembly = Default.LoadFromAssemblyName(assemblyName);

				return _interopAssembly.Assembly;
			}

			return Default.LoadFromAssemblyName(assemblyName);
		}

		protected override IntPtr LoadUnmanagedDll(string name) {
			foreach (var path in __prefixes.SelectMany(prefix => __suffixes.Select(suffix => $"{prefix}{name}{suffix}"))) {
				if (File.Exists(path)) {
					try {
						var handle = LoadUnmanagedDllFromPath(path);
						var unmanagedDll = new UnmanagedDll(name, path, handle);

						_interopAssembly._unmanagedDlls[name] = unmanagedDll;

						return handle;
					} catch (Exception e) {
						_logger.LogDebug(e, "Failed loading unmanaged dll {Name} using path {Path}", name, path);
					}
				}
			}

			return base.LoadUnmanagedDll(name);
		}
	}
}
