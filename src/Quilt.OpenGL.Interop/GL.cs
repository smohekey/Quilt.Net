namespace Quilt.OpenGL.Interop {
  using System.Runtime.InteropServices;
  using Quilt.Interop;

  [UnmanagedDll("opengl", "opengl32", "GL", Prefix = "gl", CallingConvention = CallingConvention.Cdecl)]
	public abstract class GL {
		[UnmanagedImport]
		public abstract int Viewport(int x, int y, int width, int height);
	}
}
