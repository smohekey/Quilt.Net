namespace Quilt.GL.Unmanaged {
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct Shader {
		public static readonly Shader Zero = new Shader { ID = 0 };

		uint ID;
	}
}
