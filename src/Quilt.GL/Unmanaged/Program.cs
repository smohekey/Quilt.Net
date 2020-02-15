namespace Quilt.GL.Unmanaged {
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct Program {
		public static readonly Program Zero = new Program { ID = 0 };

		uint ID;
	}
}
