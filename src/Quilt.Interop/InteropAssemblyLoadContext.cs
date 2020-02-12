using System.Linq;
namespace Quilt.Interop {
	using System.IO;
	using System;
	using System.Runtime.InteropServices;
	using System.Reflection;
	using System.Runtime.Loader;
	using System.Collections.Generic;

	public class InteropAssemblyLoadContext : AssemblyLoadContext {
		private static readonly string[] __managedSuffixes = new[] {
			".dll",
			".ni.dll",
			".exe",
			".ni.exe"
		};

		private static readonly string[] __unmanagedPrefixes;
		private static readonly string[] __unmanagedSuffixes;

		static InteropAssemblyLoadContext() {
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
				__unmanagedPrefixes = new[] { "" };
				__unmanagedSuffixes = new[] { ".dll" };
			} else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
				__unmanagedPrefixes = new[] { "", "lib" };
				__unmanagedSuffixes = new[] { ".so", ".so.1" };
			} else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
				__unmanagedPrefixes = new[] { "", "lib" };
				__unmanagedSuffixes = new[] { ".dylib" };
			} else {
				__unmanagedPrefixes = Array.Empty<string>();
				__unmanagedSuffixes = Array.Empty<string>();
			}
		}

		private readonly string _baseAssemblyPath;
		private readonly IReadOnlyCollection<string> _defaultAssemblyNames;
		private readonly IReadOnlyDictionary<string, List<string>> _unmanagedDllAliases;

		private readonly string _basePath;

		private readonly string[] _resourceRoots;

		public InteropAssemblyLoadContext(string baseAssemblyPath, IReadOnlyCollection<string> defaultAssemblyNames, IReadOnlyDictionary<string, List<string>> unmanagedDllAliases, IEnumerable<string> resourceRoots) {
			_baseAssemblyPath = baseAssemblyPath ?? throw new ArgumentNullException(nameof(baseAssemblyPath));
			_defaultAssemblyNames = defaultAssemblyNames ?? throw new ArgumentNullException(nameof(defaultAssemblyNames));
			_unmanagedDllAliases = unmanagedDllAliases ?? throw new ArgumentNullException(nameof(unmanagedDllAliases));

			_basePath = Path.GetDirectoryName(baseAssemblyPath);

			_resourceRoots = resourceRoots.Prepend(_basePath).ToArray();
		}

		protected override Assembly? Load(AssemblyName assemblyName) {
			if (assemblyName.Name == null) {
				return null;
			}

			if (_defaultAssemblyNames.Contains(assemblyName.Name)) {
				try {
					var assembly = Default.LoadFromAssemblyName(assemblyName);

					if (assembly != null) {
						return assembly;
					}
				} catch {

				}

				// Resource assembly binding does not use the TPA. Instead, it probes PLATFORM_RESOURCE_ROOTS (a list of folders)
				// for $folder/$culture/$assemblyName.dll
				// See https://github.com/dotnet/coreclr/blob/3fca50a36e62a7433d7601d805d38de6baee7951/src/binder/assemblybinder.cpp#L1232-L1290

				if (!string.IsNullOrEmpty(assemblyName.CultureName) && !string.Equals("neutral", assemblyName.CultureName)) {
					foreach (var resourceRoot in _resourceRoots) {
						var resourcePath = Path.Combine(resourceRoot, assemblyName.CultureName, assemblyName.Name + ".dll");
						if (File.Exists(resourcePath)) {
							return LoadFromAssemblyPath(resourcePath);
						}
					}

					return null;
				}
			}

			var localFile = Path.Combine(_basePath, assemblyName.Name + ".dll");

			if (File.Exists(localFile)) {
				var result = LoadFromAssemblyPath(localFile);

				return result;
			}

			return null;
		}

		protected override IntPtr LoadUnmanagedDll(string name) {
			foreach ((string path, string alias) in GeneratePaths(name)) {
				Console.WriteLine($"Attempting to load unmanaged dll {name} from path {path}.");

				if (File.Exists(path)) {
					try {
						var handle = LoadUnmanagedDllFromPath(path);
						//var unmanagedDll = new UnmanagedDll(name, alias, path, handle);

						return handle;
					} catch (Exception e) {

					}
				}
			}

			return base.LoadUnmanagedDll(name);
		}

		private IEnumerable<(string, string)> GeneratePaths(string name) {
			foreach (var prefix in __unmanagedPrefixes) {
				foreach (var alias in GetNames(name)) {
					foreach (var suffix in __unmanagedSuffixes) {
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
