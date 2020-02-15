namespace Quilt.Unmanaged {
	using System.Runtime.InteropServices;
	using System;

	public abstract class UnmanagedLoader {
		private static readonly Lazy<UnmanagedLoader> __instance = new Lazy<UnmanagedLoader>(() => {
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
				return new WindowsUnmanagedLoader();
			} else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
				return new UnixUnmanagedLoader(false);
			} else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
				return new UnixUnmanagedLoader(true);
			} else {
				throw new InvalidOperationException();
			}
		});

		public static UnmanagedLoader Instance => __instance.Value;

		public abstract IntPtr LoadLibrary(string name);

		public abstract IntPtr LoadSymbol(IntPtr library, string name);

		public abstract string? GetLibraryPath(IntPtr library);
	}
}
