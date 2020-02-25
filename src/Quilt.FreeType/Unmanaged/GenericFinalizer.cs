namespace Quilt.FreeType.Unmanaged {
	using System;
	using System.Runtime.InteropServices;

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GenericFinalizer(IntPtr data);
}
