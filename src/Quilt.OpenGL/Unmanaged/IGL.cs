namespace Quilt.OpenGL.Unmanaged {
  using System.Runtime.InteropServices;
  using Quilt.Interop;

  [UnmanagedInterface("opengl", "opengl32", "GL", Prefix = "gl", CallingConvention = CallingConvention.Cdecl)]
	public interface IGL {
		int Viewport(int x, int y, int width, int height);
	}
}
