namespace Quilt.Unmanaged {
	using System;
	using System.Runtime.InteropServices;

	class UnixLoader : Loader {
		internal UnixLoader(bool useLibC) : base(
			useLibC ? (Func<string, IntPtr>)C.Open : DL.Open,
			useLibC ? (Func<IntPtr, string, IntPtr>)C.Sym : DL.Sym
		) {

		}

		private static class C {
			private const string NAME = "c";

			[DllImport(NAME, EntryPoint = "dlopen")]
			public static extern IntPtr Open(string name);

			[DllImport(NAME, EntryPoint = "dlsym")]
			public static extern IntPtr Sym(IntPtr library, string name);
		}

		private static class DL {
			private const string NAME = "dl";

			[DllImport(NAME, EntryPoint = "dlopen")]
			public static extern IntPtr Open(string name);

			[DllImport(NAME, EntryPoint = "dlsym")]
			public static extern IntPtr Sym(IntPtr library, string name);
		}
	}
}
