namespace Quilt.OpenGL.Unmanaged {
	using System.Runtime.InteropServices;
	using Quilt.Unmanaged;

	[UnmanagedInterface(CallingConvention = CallingConvention.Cdecl, Prefix = "glfw")]
	public interface IGLFW {
		void Init();
		void Terminate();
		void GetVersion(out int major, out int minor, out int revision);
		string GetVersionString();

		Window? CreateWindow(int width, int height, string title, Monitor? monitor = null, Window? share = null);
	}
}
