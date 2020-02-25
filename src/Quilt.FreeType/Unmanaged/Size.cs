namespace Quilt.FreeType.Unmanaged {
	using System;
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public class Size {
		private IntPtr face;      /* parent face object              */
		public Generic generic;   /* generic pointer for client uses */
		SizeMetrics metrics;   /* size metrics                    */

		private IntPtr _internal;
	}
}
