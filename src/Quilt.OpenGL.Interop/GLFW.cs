namespace Quilt.OpenGL.Interop {
  using System;
  using System.Runtime.InteropServices;
  using Quilt.Interop;
  
	[UnmanagedDll("glfw3", Prefix = "glfw", CallingConvention = CallingConvention.Cdecl)]
  public abstract class GLFW {
		[UnmanagedImport]
		public abstract bool Init();

		[UnmanagedImport]
		public abstract void Terminate();

		[UnmanagedImport]
		public abstract void GetVersion(out int major, out int minor, out int revision);

		[UnmanagedImport(Prefix = "")]
		protected abstract IntPtr glfwGetVersionString();

		public string GetVersionString() {
			var ptr = glfwGetVersionString();

			return MarshalExt.FromUTF8(ptr); 
		}
	}
}
