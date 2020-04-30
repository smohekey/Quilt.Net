namespace Quilt.GLFW {
  using System;
  using System.Runtime.InteropServices;
	using Quilt.Unmanaged;

	[UnmanagedObject(CallingConvention = CallingConvention.Cdecl, Prefix = "glfw")]
	public abstract class GLFWMonitor : UnmanagedObject {
		private readonly GLFWContext _glfw;
		private readonly IntPtr _monitor;

		protected GLFWMonitor(UnmanagedLibrary library, GLFWContext glfw, IntPtr monitor) : base(library) {
			_glfw = glfw;
			_monitor = monitor;
		}

		[UnmanagedMethod(Name = "glfwGetMonitorPhysicalSize")]
		protected abstract void _GetMonitorPhysicalSize(IntPtr monitor, out int widthMM, out int heightMM);

		public (int WidthMM, int HeightMM) PhysicalSize {
			get {
				_GetMonitorPhysicalSize(_monitor, out var widthMM, out var heightMM);

				return (widthMM, heightMM);
			}
		}
	}
}
