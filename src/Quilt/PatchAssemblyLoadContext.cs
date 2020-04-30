namespace Quilt {
  using System;
  using System.IO;
  using System.Reflection;
  using System.Runtime.InteropServices;
  using System.Runtime.Loader;

	public class PatchAssemblyLoadContext : AssemblyLoadContext {
		private static readonly string[] __prefixes;
		private static readonly string[] __suffixes;

		static PatchAssemblyLoadContext() {
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
				__prefixes = new[] { "" };
				__suffixes = new[] { ".dll" };
			} else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
				__prefixes = new[] { "", "lib" };
				__suffixes = new[] { ".so", ".so.1", ".so.2", ".so.3" };
			} else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
				__prefixes = new[] { "", "lib" };
				__suffixes = new[] { ".dylib", ".1.dylib" };
			} else {
				__prefixes = Array.Empty<string>();
				__suffixes = Array.Empty<string>();
			}
		}

		private readonly string _path;

		public PatchAssemblyLoadContext(string path) : base(true) {
			_path = path;
		}

		protected override Assembly? Load(AssemblyName assemblyName) {
			assemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));

			if(assemblyName.Name == null) {
				return null;
			}

			var filePath = Path.Combine(_path, $"{assemblyName.Name}.dll");

			if(!File.Exists(filePath)) {
				return base.Load(assemblyName);
			}

			return LoadFromAssemblyPath(filePath);
		}

		protected override IntPtr LoadUnmanagedDll(string unmanagedDllName) {
			unmanagedDllName = unmanagedDllName ?? throw new ArgumentNullException(nameof(unmanagedDllName));

			if(unmanagedDllName.EndsWith(".dll", StringComparison.InvariantCulture)) {
				var name = unmanagedDllName[0..^4];

				if(FindUnmanagedDLL(name) is string filePath1) {
					return LoadUnmanagedDllFromPath(filePath1);
				}
			}

			if(FindUnmanagedDLL(unmanagedDllName) is string filePath2) {
				return LoadUnmanagedDllFromPath(filePath2);
			}

			return base.LoadUnmanagedDll(unmanagedDllName);
		}

		private string? FindUnmanagedDLL(string name) {
			foreach (var prefix in __prefixes) {
				foreach (var suffix in __suffixes) {
					var filePath = Path.Combine(_path, $"{prefix}{name}{suffix}");

					if(File.Exists(filePath)) {
						return filePath;
					}
				}
			}

			return null;
		}
	}
}
