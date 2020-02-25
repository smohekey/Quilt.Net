namespace Quilt.FreeType {
	using System;
	using System.Runtime.InteropServices;
	using Quilt.Unmanaged;

	[UnmanagedObject(CallingConvention = CallingConvention.Cdecl, Prefix = "FT_")]
	public abstract class FontLibrary : UnmanagedObject {
		private readonly IntPtr _handle;

		protected FontLibrary(UnmanagedLibrary library, IntPtr handle) : base(library) {
			_handle = handle;
		}

		protected abstract int New_Face(IntPtr library, string path, long index, out IntPtr face);

		public FontFace OpenFont(string path) {

		}
	}
}
