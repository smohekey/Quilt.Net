namespace Quilt.GL.Unmanaged {
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct Buffer {
		public static readonly Buffer Zero = new Buffer { ID = 0 };

		uint ID;
	}
}
