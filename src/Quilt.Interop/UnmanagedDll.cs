namespace Quilt.Interop {
	using System;
	using System.Collections.Generic;
  using System.Diagnostics.CodeAnalysis;
  using System.Linq;
	using System.Reflection;
	using System.Reflection.Emit;
  using System.Runtime.InteropServices;
  using System.Text;

  public class UnmanagedDll {
		private static readonly string[] __prefixes;
		private static readonly string[] __suffixes;

		static UnmanagedDll() {
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

		/// <summary>
		/// The name of the unmanaged dll as used in DllImport attributes.
		/// </summary>
		/// <value></value>
		public string Name { get; }

		/// <summary>
		/// The alies used to load the unmanaged dll.
		/// </summary>
		public string LoadedName { get; }

		/// <summary>
		/// The path the unmanaged dll was ultimately loaded from.
		/// </summary>
		/// <value></value>
		public string Path { get; }

		/// <summary>
		/// The handle to the unmanaged dll.
		/// </summary>
		/// <value></value>
		public IntPtr Handle { get; }

		internal UnmanagedDll(string name, string loadedName, string path, IntPtr handle) {
			Name = name;
			LoadedName = loadedName;
			Path = path;
			Handle = handle;
		}

		[DllImport("Kernel32.dll")]
		private static extern IntPtr LoadLibrary(string name);

		[DllImport("Kernel32.dll")]
		private static extern int GetModuleFileName(IntPtr library, StringBuilder path, int size);

		[DllImport("Kernel32.dll")]
		private static extern IntPtr GetProcAddress(IntPtr library, string name);
		
		public IntPtr GetProcAddress(string name) {
			return GetProcAddress(Handle, name);
		}

		public static bool TryLoad(string name, [NotNullWhen(returnValue: true)] out UnmanagedDll? unmanagedDll, params string[] aliases) {
			foreach (var (candidate, alias) in GeneratePaths(name, aliases)) {
				var handle = LoadLibrary(candidate);

				if (handle != IntPtr.Zero) {
					var path = new StringBuilder(256);

					int length;

					while(path.Capacity < (length = GetModuleFileName(handle, path, path.Capacity))) {
						path.EnsureCapacity(length);
					}

					unmanagedDll = new UnmanagedDll(name, candidate, path.ToString(), handle);

					return true;
				}
			}

			unmanagedDll = null;

			return false;
		}
		
		private static IEnumerable<(string, string)> GeneratePaths(string name, string[] aliases) {
			foreach (var prefix in __prefixes) {
				foreach (var alias in aliases.Prepend(name)) {
					foreach (var suffix in __suffixes) {
						yield return ($"{prefix}{alias}{suffix}", alias);
					}
				}
			}
		}

	}
}
