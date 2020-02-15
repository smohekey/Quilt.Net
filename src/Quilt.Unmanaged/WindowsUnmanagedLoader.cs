namespace Quilt.Unmanaged {
	using System;
	using System.Runtime.InteropServices;
  using System.Text;

  class WindowsUnmanagedLoader : UnmanagedLoader {
		internal WindowsUnmanagedLoader() {

		}

		public override IntPtr LoadLibrary(string name) => Kernel32.LoadLibrary(name);
		public override IntPtr LoadSymbol(IntPtr library, string name) => Kernel32.GetProcAddress(library, name);
		public override string? GetLibraryPath(IntPtr library) => Kernel32.GetLibraryPath(library);

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
