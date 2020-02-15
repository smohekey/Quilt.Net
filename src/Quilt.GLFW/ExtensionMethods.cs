namespace Quilt.GLFW {
	using System;
  using System.Runtime.CompilerServices;

  public static class ExtensionMethods {
		public static ref Window CreateWindow(this IGLFW @this, int width, int height, string title) {
			return ref @this.CreateWindow(width, height, title, IntPtr.Zero, IntPtr.Zero);
		}

		public static unsafe ref Window CreateWindow(this IGLFW @this, int width, int height, string title, ref Monitor monitor) {
			var monitorPtr = new IntPtr(Unsafe.AsPointer(ref monitor));

			return ref @this.CreateWindow(width, height, title, monitorPtr, IntPtr.Zero);
		}

		public static unsafe ref Window CreateWindow(this IGLFW @this, int width, int height, string title, ref Monitor monitor, ref Window share) {
			var monitorPtr = new IntPtr(Unsafe.AsPointer(ref monitor));
			var sharePtr = new IntPtr(Unsafe.AsPointer(ref share));

			return ref @this.CreateWindow(width, height, title, monitorPtr, sharePtr);
		}
	}
}
