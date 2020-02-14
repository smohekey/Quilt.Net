namespace Quilt.Unmanaged {
	using System;
	using System.Runtime.InteropServices;
  using System.Text;

  class WindowsLoader : Loader {
		internal WindowsLoader() : base(Kernel32.LoadLibrary, Kernel32.GetProcAddress, Kernel32.GetLibraryPath) {

		}

		private static class Kernel32 {
			private const string NAME = "Kernel32";

			[DllImport(NAME)]
			public static extern IntPtr LoadLibrary(string name);

			[DllImport(NAME)]
			public static extern IntPtr GetProcAddress(IntPtr library, string name);

			[DllImport(NAME)]
			public static extern int GetModuleFileName(IntPtr library, StringBuilder path, int length);

			public static string? GetLibraryPath(IntPtr library) {
				var path = new StringBuilder(256);

				int length;

				while (path.Capacity < (length = GetModuleFileName(library, path, path.Capacity))) {
					path.EnsureCapacity(length);
				}

				return path.ToString();
			}
		}
	}
}
