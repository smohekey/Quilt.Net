namespace Quilt.GLFW {
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct Image {
		public int Width;
		public int Height;
		public byte[] Pixels;
	}
}
