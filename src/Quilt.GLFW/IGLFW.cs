namespace Quilt.GLFW {
	using System;
  using System.Runtime.InteropServices;
	using Quilt.Unmanaged;

	public delegate void ErrorCallback(int errorCode, string description);
	
	public delegate void WindowCloseCallback(ref Window window);
	public delegate void WindowIconifyCallback(ref Window window);
	public delegate void WindowMaximizeCallback(ref Window window);
	public delegate void WindowFocusCallback(ref Window window);
	public delegate void WindowRefreshCallback(ref Window window);
	public delegate void WindowSizeCallback(ref Window window, int width, int height);
	public delegate void WindowPosCallback(ref Window window, int left, int top);
	public delegate void WindowContentScaleCallback(ref Window window, float xscale, float yscale);

	public delegate void KeyCallback(ref Window window, Key key, int scanCode, InputState state, ModifierKeys modifiers);

	public delegate void CursorPosCallback(ref Window window, int x, int y);
	public delegate void CursorEnterCallback(ref Window window, bool entered);

	public delegate void MouseButtonCallback(ref Window window, MouseButton button, InputState state, ModifierKeys modifiers);
	public delegate void ScrollCallback(ref Window window, double xoffset, double yoffset);

	public delegate void FramebufferSizeCallback(ref Window window, int width, int height);

	//public delegate void MonitorCallback(ref Monitor monitor, )

	[UnmanagedInterface(CallingConvention = CallingConvention.Cdecl, Prefix = "glfw")]
	public interface IGLFW {
		void Init();

		void Terminate();

		void GetVersion(out int major, out int minor, out int revision);

		string GetVersionString();

		int GetError(out string description);

		ErrorCallback SetErrorCallback(ErrorCallback callback);

		bool ExtensionSupported(string name);

		IntPtr GetProcAddress(string name);

		void PollEvents();

		void WaitEvents();

		void WaitEventsTimeout(double timeout);

		void PostEmptyEvent();

		#region Monitors
		Monitor GetPrimaryMonitor();

		ref VideoMode GetVideoMode(ref Monitor monitor);
		#endregion

		#region Windows
		void WindowHint(Hint hint, int value);

		void WindowHint(Hint hint, string value);

		ref Window CreateWindow(int width, int height, string title, IntPtr monitor, IntPtr share);

		bool WindowShouldClose(ref Window window);

		void SetWindowShouldClose(ref Window window, bool value);

		void SetWindowCloseCallback(ref Window window, WindowCloseCallback callback);

		void IconifyWindow(ref Window window);

		void RestoreWindow(ref Window window);

		void SetWindowIconifyCallback(ref Window window, WindowIconifyCallback callback);

		void MaximizeWindow(ref Window window);

		void SetWindowMaximizeCallback(ref Window window, WindowMaximizeCallback callback);

		void FocusWindow(ref Window window);

		void SetWindowFocusCallback(ref Window window, WindowFocusCallback callback);

		void SetWindowRefreshCallback(ref Window window, WindowRefreshCallback callback);

		void ShowWindow(ref Window window);

		void HideWindow(ref Window window);

		void GetWindowSize(ref Window window, out int width, out int height);

		void SetWindowSize(ref Window window, int width, int height);

		void SetWindowSizeCallback(ref Window window, WindowSizeCallback callback);

		void GetWindowFrameSize(ref Window window, out int left, out int top, out int right, out int bottom);

		void SetWindowSizeLimits(ref Window window, int minWidth, int minHeight, int maxWidth, int maxHeight);

		void SetWindowAspectRatio(ref Window window, int numerator, int denominator);

		void GetWindowPos(ref Window window, out int left, out int top);

		void SetWindowPos(ref Window window, int left, int top);

		void SetWindowPosCallback(ref Window window, WindowPosCallback callback);

		void SetWindowTitle(ref Window window, string title);

		void SetWindowIcon(ref Window window, int count, Image[] image);

		float GetWindowOpacity(ref Window window);

		void SetWindowOpacity(ref Window window, float opacity);

		int GetWindowAttrib(ref Window window, WindowAttribute attribute);

		void SetWindowAttrib(ref Window window, WindowAttribute attribute, int value);

		void SwapBuffers(ref Window window);

		void SwapInterval(ref Window window, int interval);

		ref Monitor GetWindowMonitor(ref Window window);

		void SetWindowMonitor(ref Window window, ref Monitor monitor, int left, int top, int width, int height, int refreshRate);

		void GetWindowContentScale(ref Window window, out float xscale, out float yscale);

		void SetWindowContentScaleCallback(ref Window window, WindowContentScaleCallback callback);

		void RequestWindowAttention(ref Window window);

		void GetFramebufferSize(ref Window window, out int width, out int height);

		void SetFramebufferSizeCallback(ref Window window, FramebufferSizeCallback callback);
		#endregion

		#region Input
		void SetKeyCallback(ref Window window, KeyCallback callback);

		void GetCursorPos(ref Window window, out int x, out int y);

		//ref Cursor CreateCursor()
		ref Cursor CreateStandardCursor(CursorType type);

		void SetCursor(ref Window window, ref Cursor cursor);

		void DestroyCursor(ref Cursor cursor);

		void SetCursorPosCallback(ref Window window, CursorPosCallback callback);

		void SetCursorEnterCallback(ref Window window, CursorEnterCallback callback);

		InputState GetMouseButton(ref Window window, MouseButton button);

		void SetMouseButtonCallback(ref Window window, MouseButtonCallback callback);

		void SetScrollCallback(ref Window window, ScrollCallback callback);

		string GetClipboardString(ref Window window);

		void SetClipboardString(ref Window window, string value);
		#endregion

		#region Contexts
		void MakeContextCurrent(ref Window window);

		ref Window GetCurrentContext();

		
		#endregion
	}
}
