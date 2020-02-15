namespace Quilt.GL.Unmanaged {
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct VertexArray {
		public static readonly VertexArray Zero = new VertexArray { ID = 0 };

		uint ID;
	}
}
