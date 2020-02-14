namespace Quilt.Unmanaged {
	using System;
	using System.Runtime.InteropServices;

	class UnixLoader : Loader {
		[StructLayout(LayoutKind.Sequential)]
		private struct DL_info {
			[MarshalAs(UnmanagedType.LPStr)]
			public string dli_fname;

			public IntPtr dli_fbase;

			[MarshalAs(UnmanagedType.LPStr)]
			public string dli_sname;

			public IntPtr dli_saddr;
		}

		internal UnixLoader(bool useLibC) : base(
			useLibC ? (Func<string, IntPtr>)C.Open : DL.Open,
			useLibC ? (Func<IntPtr, string, IntPtr>)C.Sym : DL.Sym,
			useLibC ? (Func<IntPtr, string>)C.GetLibraryPath : DL.GetLibraryPath
		) {

		}

		private static class C {
			private const string NAME = "c";

			[DllImport(NAME, EntryPoint = "dlopen")]
			public static extern IntPtr Open(string name);

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
			public static extern IntPtr Open(string name);

			[DllImport(NAME, EntryPoint = "dlsym")]
			public static extern IntPtr Sym(IntPtr library, string name);

			[DllImport(NAME, EntryPoint = "dladdr")]
			private static extern int Addr(IntPtr library, ref DL_info info);

			public static string? GetLibraryPath(IntPtr library) {
				DL_info info = default;

				if(0 == Addr(library, ref info)) {
					return string.Empty;
				}

				return info.dli_fname;
			}
		}
	}
}
