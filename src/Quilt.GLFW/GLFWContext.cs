namespace Quilt.GLFW {
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;
	using System.Threading;
	using Quilt.Unmanaged;

	[UnmanagedObject(CallingConvention = CallingConvention.Cdecl, Prefix = "glfw")]
	public abstract class GLFWContext : UnmanagedObject {
		private static volatile int __count;

		public static GLFWContext Create() {
			int initialCount;

			do {
				initialCount = __count;

				if (initialCount == 1) {
					throw new InvalidOperationException();
				}
			} while (initialCount != Interlocked.CompareExchange(ref __count, 1, initialCount));

			if (!UnmanagedLibrary.TryLoad("glfw", out var glfwLibrary, "glfw3")) {
				throw new Exception();
			}

			return glfwLibrary.CreateObject<GLFWContext>();
		}

		protected delegate void ErrorCallback(int error, string description);
		protected delegate void MonitorCallback(IntPtr monitor, int @event);

		internal readonly UnmanagedLibrary _glLibrary;

		private readonly ErrorCallback _errorDelegate;
		private readonly MonitorCallback _monitorDelegate;

		protected GLFWContext(UnmanagedLibrary library) : base(library) {
			Init();

			WindowHint(Hint.ContextVersionMajor, 3);
			WindowHint(Hint.ContextVersionMinor, 3);
			WindowHint(Hint.OpenglProfile, (int)Profile.Core);

			var loader = new GLFWUnmanagedLoader(this, UnmanagedLoader.Default);

			if (!UnmanagedLibrary.TryLoad("GL", loader, out var glLibrary, "opengl32")) {
				throw new Exception("Couldn't load GL");
			}

			_glLibrary = glLibrary;

			SetErrorCallback(_errorDelegate = new ErrorCallback(HandleError));
			SetMonitorCallback(_monitorDelegate = new MonitorCallback(HandleMonitor));
		}

		private void HandleError(int error, string description) {
			OnError?.Invoke(error, description);
		}

		private void HandleMonitor(IntPtr monitor, int @event) {

		}

		protected override void DisposeUnmanaged() {
			Terminate();

			__count = 0;
		}

		public event Action<int, string>? OnError;

		protected abstract void Init();

		public abstract void WindowHint(Hint hint, int value);

		public void WindowHint(Hint hint, bool value) {
			WindowHint(hint, value ? 1 : 0);
		}

		public abstract void WindowHint(Hint hint, string value);

		protected abstract void Terminate();

		public abstract void GetVersion(out int major, out int minor, out int revision);

		public abstract string GetVersionString();

		public abstract int GetError(out string description);

		protected abstract ErrorCallback? SetErrorCallback(ErrorCallback callback);

		protected abstract MonitorCallback? SetMonitorCallback(MonitorCallback callback);

		public abstract bool ExtensionSupported(string name);

		public abstract IntPtr GetProcAddress(string name);

		public abstract void PollEvents();

		public abstract void WaitEvents();

		public abstract void WaitEventsTimeout(double timeout);

		public abstract void PostEmptyEvent();

		protected abstract IntPtr CreateWindow(int width, int height, string title, IntPtr monitor, IntPtr share);

		public GLFWWindow CreateWindow(int width, int height, string title) {
			var windowPtr = CreateWindow(width, height, title, IntPtr.Zero, IntPtr.Zero);

			return _library.CreateObject<GLFWWindow>(this, windowPtr, title);
		}
	}
}
