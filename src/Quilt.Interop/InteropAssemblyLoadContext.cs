namespace Quilt.Interop {
	using System.IO;
	using System;
	using System.Runtime.InteropServices;
	using System.Reflection;
	using System.Runtime.Loader;
	using System.Collections.Generic;

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

		private readonly InteropAssembly _interopAssembly;
		private readonly Dictionary<string, List<string>> _unmanagedDllAliases;

		public InteropAssemblyLoadContext(InteropAssembly interopAssembly, Dictionary<string, List<string>> unmanagedDllAliases) {
			_interopAssembly = interopAssembly;
			_unmanagedDllAliases = unmanagedDllAliases;
		}

		protected override Assembly Load(AssemblyName assemblyName) {
			if (assemblyName.Name == _interopAssembly.AssemblyName.Name) {
				_interopAssembly.Assembly = Default.LoadFromAssemblyName(assemblyName);

				return _interopAssembly.Assembly;
			}

			return Default.LoadFromAssemblyName(assemblyName);
		}

		protected override IntPtr LoadUnmanagedDll(string name) {
			foreach ((string path, string alias) in GeneratePaths(name)) {
				Console.WriteLine($"Attempting to load unmanaged dll {name} from path {path}.");

				if (File.Exists(path)) {
					try {
						var handle = LoadUnmanagedDllFromPath(path);
						var unmanagedDll = new UnmanagedDll(name, alias, path, handle);

						_interopAssembly._unmanagedDlls[name] = unmanagedDll;

						return handle;
					} catch (Exception e) {

					}
				}
			}

			return base.LoadUnmanagedDll(name);
		}

		private IEnumerable<(string, string)> GeneratePaths(string name) {
			foreach (var prefix in __prefixes) {
				foreach (var alias in GetNames(name)) {
					foreach (var suffix in __suffixes) {
						yield return ($"{prefix}{alias}{suffix}", alias);
					}
				}
			}
		}

		private IEnumerable<string> GetNames(string name) {
			yield return name;

			if (_unmanagedDllAliases.TryGetValue(name, out var aliases)) {
				foreach (var alias in aliases) {
					yield return alias;
				}
			}
		}
	}
}
