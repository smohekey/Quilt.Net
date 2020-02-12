namespace Quilt.OpenGL {
	using System.Runtime.InteropServices;
	using Quilt.Unmanaged;

	[UnmanagedLibrary("glfw3", CallingConvention = CallingConvention.Cdecl, Prefix = "glfw")]
	internal abstract class GLFW {
		public abstract void Init();
		public abstract void Terminate();
		public abstract void GetVersion(out int major, out int minor, out int revision);
	}
}
