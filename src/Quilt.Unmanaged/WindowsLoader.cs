namespace Quilt.Unmanaged {
	using System;
	using System.Runtime.InteropServices;

	class WindowsLoader : Loader {
		internal WindowsLoader() : base(Kernel32.LoadLibrary, Kernel32.GetProcAddress) {

		}

		private static class Kernel32 {
			private const string NAME = "Kernel32";

			[DllImport(NAME)]
			public static extern IntPtr LoadLibrary(string name);

			[DllImport(NAME)]
			public static extern IntPtr GetProcAddress(IntPtr library, string name);
		}
	}
}
