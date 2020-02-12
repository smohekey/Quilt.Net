using System.Security;
namespace Quilt.OpenGL.Interop {
	using System;
	using System.Runtime.InteropServices;
	using Quilt.Interop;

	public static class GLFW {
		private const string DLL = "glfw3";
		private const string PREFIX = "glfw";

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = PREFIX + nameof(Init)), SuppressUnmanagedCodeSecurity]
		public static extern bool Init();

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = PREFIX + nameof(Terminate)), SuppressUnmanagedCodeSecurity]
		public static extern void Terminate();

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = PREFIX + nameof(GetVersion)), SuppressUnmanagedCodeSecurity]
		public static extern void GetVersion(out int major, out int minor, out int revision);

		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		private static extern IntPtr glfwGetVersionString();

		public static string GetVersionString() {
			var ptr = glfwGetVersionString();

			return MarshalExt.FromUTF8(ptr);
		}
	}
}
