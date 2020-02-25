namespace Quilt.FreeType {
	using System;
	using System.Runtime.InteropServices;
	using System.Threading;
	using Quilt.Unmanaged;

	[UnmanagedObject(CallingConvention = CallingConvention.Cdecl, Prefix = "FT_")]
	public abstract class FTContext : UnmanagedObject {
		private static volatile int __count;

		public static FTContext Create() {
			int initialCount;

			do {
				initialCount = __count;

				if (initialCount == 1) {
					throw new InvalidOperationException();
				}
			} while (initialCount != Interlocked.CompareExchange(ref __count, 1, initialCount));

			if (!UnmanagedLibrary.TryLoad("freetype", out var ftLibrary)) {
				throw new Exception();
			}

			return ftLibrary.CreateObject<FTContext>();
		}

		protected FTContext(UnmanagedLibrary library) : base(library) {

		}

		protected abstract int Init_FreeType(out IntPtr library);
		public FontLibrary CreateLibrary() {
			Init_FreeType(out var handle);

			return _library.CreateObject<FontLibrary>(handle);
		}
	}
}
