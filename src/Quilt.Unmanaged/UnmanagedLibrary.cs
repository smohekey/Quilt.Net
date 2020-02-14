namespace Quilt.Unmanaged {
	using System;
  using System.Collections.Generic;
  using System.Diagnostics.CodeAnalysis;
  using System.Linq;
  using System.Runtime.InteropServices;
  
  using Quilt.Unmanaged.Generation;

  public class UnmanagedLibrary {
		private static readonly string[] __prefixes;
		private static readonly string[] __suffixes;

		private static readonly ImplementationGenerator __generator = new ImplementationGenerator();
		private static readonly Dictionary<string, UnmanagedLibrary> __unmanagedLibraries = new Dictionary<string, UnmanagedLibrary>();
		private static readonly Dictionary<Type, Type> __unmanagedInterfaces = new Dictionary<Type, Type>();

		static UnmanagedLibrary() {
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

		private readonly Loader _loader;

		public string Name { get; }

		public string NameUsed { get; }

		public string? Path { get; }

		public IntPtr Handle { get; }

		private UnmanagedLibrary(Loader loader, string name, string nameUsed, string? path, IntPtr handle) {
			_loader = loader;
			Name = name;
			NameUsed = nameUsed;
			Path = path;
			Handle = handle;
		}

		public IntPtr GetSymbol(string name) {
			return _loader.LoadSymbol(Handle, name);
		}

		public bool TryQueryInterface<T>([NotNullWhen(true)] out T? @interface) where T : class {
			var interfaceType = typeof(T);

			if (!__unmanagedInterfaces.TryGetValue(interfaceType, out var implementationType)) {
				if (!__generator.TryGenerate<T>(this, out implementationType!)) {
					@interface = null;

					return false;
				}

				__unmanagedInterfaces[interfaceType] = implementationType;
			}

			@interface = (T)Activator.CreateInstance(implementationType, this);

			return true;
		}

		public static bool TryLoad(string name, [NotNullWhen(returnValue: false)] out UnmanagedLibrary? unmanagedLibrary, params string[] aliases) {
			if (!__unmanagedLibraries.TryGetValue(name, out unmanagedLibrary)) {
				var loader = Loader.Instance;

				foreach (var candidate in GeneratePaths(name, aliases)) {
					var handle = loader.LoadLibrary(candidate);

					if (handle != IntPtr.Zero) {
						__unmanagedLibraries[name] = unmanagedLibrary = new UnmanagedLibrary(loader, name, candidate, loader.GetLibraryPath(handle), handle);

						return true;
					}
				}
			}

			unmanagedLibrary = null;

			return false;
		}

		private static IEnumerable<string> GeneratePaths(string name, string[] aliases) {
			foreach (var prefix in __prefixes) {
				foreach (var alias in aliases.Prepend(name)) {
					foreach (var suffix in __suffixes) {
						yield return $"{prefix}{alias}{suffix}";
					}
				}
			}
		}
	}
}
