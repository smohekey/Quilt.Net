namespace Quilt.Unmanaged {
  using System;
  using System.Collections.Generic;
  using System.Diagnostics.CodeAnalysis;
  using System.Linq;
  using System.Runtime.InteropServices;
  //using System.Runtime.Loader;
  using Quilt.Unmanaged.Generation;

  public class UnmanagedLibrary {
		private static readonly string[] __paths;
		private static readonly string[] __prefixes;
		private static readonly string[] __suffixes;

		private static readonly ImplementationGenerator __generator = new ImplementationGenerator();
		private static readonly Dictionary<string, UnmanagedLibrary> __unmanagedLibraries = new Dictionary<string, UnmanagedLibrary>();
		private static readonly Dictionary<Type, Type> __unmanagedObjectTypes = new Dictionary<Type, Type>();

		static UnmanagedLibrary() {
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
			__paths = Array.Empty<string>();
			__prefixes = new[] { "" };
			__suffixes = new[] { ".dll" };
			} else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
			__paths = new[] { "/usr/local/lib", "/use/libs" };
			__prefixes = new[] { "", "lib" };
			__suffixes = new[] { ".so", ".so.1", ".so.2", ".so.3" };
			} else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
			__paths = new[] { "/usr/local/lib", "/usr/lib", "/usr/X11/lib", "/System/Library/Frameworks/OpenGL.framework/Libraries" };
			__prefixes = new[] { "", "lib" };
			__suffixes = new[] { ".dylib", ".1.dylib" };
			} else {
			__paths = Array.Empty<string>();
			__prefixes = Array.Empty<string>();
			__suffixes = Array.Empty<string>();
			}
		}

		private readonly UnmanagedLoader _loader;

		public string Name { get; }

		public string NameUsed { get; }

		public string? Path { get; }

		public IntPtr Handle { get; }

		private UnmanagedLibrary(UnmanagedLoader loader, string name, string nameUsed, string? path, IntPtr handle) {
			_loader = loader;
			Name = name;
			NameUsed = nameUsed;
			Path = path;
			Handle = handle;
		}

		public IntPtr LoadSymbol(string name) {
			return _loader.LoadSymbol(Handle, name);
		}

		public T CreateObject<T>(params object[] parameters) where T : UnmanagedObject {
			var objectType = typeof(T);

			if (!__unmanagedObjectTypes.TryGetValue(objectType, out var implementationType)) {
			implementationType = __generator.Generate<T>(this);

			__unmanagedObjectTypes[objectType] = implementationType;
			}

			return (T)Activator.CreateInstance(implementationType, parameters.Prepend(this).ToArray())!;
		}

		public static bool TryLoad(string name, [NotNullWhen(true)] out UnmanagedLibrary? unmanagedLibrary, params string[] aliases) {
			return TryLoad(name, UnmanagedLoader.Default, out unmanagedLibrary, aliases);
		}

		public static bool TryLoad(string name, UnmanagedLoader loader, [NotNullWhen(true)] out UnmanagedLibrary? unmanagedLibrary, params string[] aliases) {
			if (__unmanagedLibraries.TryGetValue(name, out unmanagedLibrary)) {
			return false;
			}

			//var resolver = new AssemblyDependencyResolver(AppDomain.CurrentDomain.BaseDirectory!);

			foreach (var candidate in GenerateCandidates(name, aliases)) {
				//var path = resolver.ResolveUnmanagedDllToPath(candidate);
				var path = default(string);

				if (path != null) {
					if (TryLoadFromPath(loader, name, candidate, path, out unmanagedLibrary)) {
						return true;
					}
				} else {
					if (TryLoadFromPath(loader, name, candidate, candidate, out unmanagedLibrary)) {
						return true;
					}
				}
			}

			return false;
		}

		private static bool TryLoadFromPath(UnmanagedLoader loader, string name, string candidate, string path, [NotNullWhen(true)] out UnmanagedLibrary? unmanagedLibrary) {
			var handle = loader.LoadLibrary(path);

			if (handle == IntPtr.Zero) {
			unmanagedLibrary = null;
			} else {
			unmanagedLibrary = new UnmanagedLibrary(loader, name, candidate, loader.GetLibraryPath(handle), handle);

			__unmanagedLibraries[name] = unmanagedLibrary;
			}

			return unmanagedLibrary != null;
		}

		private static IEnumerable<string> GenerateCandidates(string name, string[] aliases) {
			//return aliases.Prepend(name);

			foreach (var path in __paths.Append("")) {
				foreach (var prefix in __prefixes) {
					foreach (var alias in aliases.Prepend(name)) {
						foreach (var suffix in __suffixes) {
							yield return System.IO.Path.Combine(path, $"{prefix}{alias}{suffix}");
						}
					}
				}
			}
		}
  }
}
