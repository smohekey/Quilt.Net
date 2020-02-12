namespace Quilt.Unmanaged {
	using System.Runtime.InteropServices;
	using System;

	public abstract class Loader {
		private static readonly Lazy<Loader> __instance = new Lazy<Loader>(() => {
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
				return new WindowsLoader();
			} else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
				return new UnixLoader(false);
			} else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
				return new UnixLoader(true);
			} else {
				throw new InvalidOperationException();
			}
		});

		public static Loader Instance => __instance.Value;

		private readonly Func<string, IntPtr> _loadLibrary;
		private readonly Func<IntPtr, string, IntPtr> _loadSymbol;

		protected Loader(Func<string, IntPtr> loadLibrary, Func<IntPtr, string, IntPtr> loadSymbol) {
			_loadLibrary = loadLibrary;
			_loadSymbol = loadSymbol;
		}

		public IntPtr LoadLibrary(string name) => _loadLibrary(name);

		public IntPtr LoadSymbol(IntPtr library, string name) => _loadSymbol(library, name);
	}
}
