namespace Quilt.GL.Unmanaged {
  using System;
  using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct GLsizei {
		IntPtr Value;

		public static implicit operator int(GLsizei size) => size.Value.ToInt32();
		public static implicit operator GLsizei(int value) => new GLsizei { Value = new IntPtr(value) };
	}
}
