namespace Quilt.Unmanaged {
	using System;
	using System.Runtime.InteropServices;

	class UnixUnmanagedLoader : UnmanagedLoader {
		[StructLayout(LayoutKind.Sequential)]
		private struct DL_info {
			[MarshalAs(UnmanagedType.LPStr)]
			public string dli_fname;

			public IntPtr dli_fbase;

			[MarshalAs(UnmanagedType.LPStr)]
			public string dli_sname;

			public IntPtr dli_saddr;
		}

		private readonly Func<string, int, IntPtr> _loadLibrary;
		private readonly Func<IntPtr, string, IntPtr> _loadSymbol;
		private readonly Func<IntPtr, string?> _getLibraryPath;

		internal UnixUnmanagedLoader(bool useLibC) {
			_loadLibrary = useLibC ? (Func<string, int, IntPtr>)C.Open : DL.Open;
			_loadSymbol = useLibC ? (Func<IntPtr, string, IntPtr>)C.Sym : DL.Sym;
			_getLibraryPath = useLibC ? (Func<IntPtr, string?>)C.GetLibraryPath : DL.GetLibraryPath;
		}

		public override IntPtr LoadLibrary(string name) => _loadLibrary(name, 2);

		public override IntPtr LoadSymbol(IntPtr library, string name) => _loadSymbol(library, name);

		public override string? GetLibraryPath(IntPtr library) => _getLibraryPath(library);

		private static class C {
			private const string NAME = "c";

			[DllImport(NAME, EntryPoint = "dlopen")]
			public static extern IntPtr Open(string name, int flags);

			[DllImport(NAME, EntryPoint = "dlsym")]
			public static extern IntPtr Sym(IntPtr library, string name);

			[DllImport(NAME, EntryPoint = "dladdr")]
			private static extern int Addr(IntPtr library, ref DL_info info);

			public static string? GetLibraryPath(IntPtr library) {
				DL_info info = default;

				if (0 == Addr(library, ref info)) {
					return string.Empty;
				}

				return info.dli_fname;
			}
		}

		private static class DL {
			private const string NAME = "dl";
			private const int RTLD_DI_ORIGIN = 6;


			[DllImport(NAME, EntryPoint = "dlopen")]
			public static extern IntPtr Open(string name, int flags);

			[DllImport(NAME, EntryPoint = "dlsym")]
			public static extern IntPtr Sym(IntPtr library, string name);

			[DllImport(NAME, EntryPoint = "dladdr")]
			private static extern int Addr(IntPtr library, ref DL_info info);

			public static string? GetLibraryPath(IntPtr library) {
				DL_info info = default;

				if (0 == Addr(library, ref info)) {
					return string.Empty;
				}

				return info.dli_fname;
			}
		}
	}
}
