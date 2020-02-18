namespace Quilt.GLFW {
	using System;
	using System.Runtime.InteropServices;
	using Quilt.GL;
	using Quilt.Unmanaged;

	[UnmanagedObject(CallingConvention = CallingConvention.Cdecl, Prefix = "glfw")]
	public abstract class GLFWWindow : UnmanagedObject {
		private readonly GLFWContext _glfw;
		private readonly GLContext _gl;
		private readonly IntPtr _window;
		private string _title;


		// we have to store a reference to each of the delegates to ensure they're not garbage collected
		private readonly WindowCloseCallback _windowCloseDelegate;
		private readonly WindowIconifyCallback _windowIconifyDelegate;
		private readonly WindowMaximizeCallback _windowMaximizeDelegate;
		private readonly WindowFocusCallback _windowFocusDelegate;
		private readonly WindowSizeCallback _windowSizeDelegate;
		private readonly WindowPosCallback _windowPosDelegate;
		private readonly WindowContentScaleCallback _windowContentScaleDelegate;
		private readonly WindowRefreshCallback _windowRefreshDelegate;
		private readonly KeyCallback _keyDelegate;
		private readonly CharCallback _charDelegate;
		private readonly CursorPosCallback _cursorPosDelegate;
		private readonly CursorEnterCallback _cursorEnterDelegate;
		private readonly MouseButtonCallback _mouseButtonDelegate;
		private readonly ScrollCallback _scrollDelegate;
		private readonly FramebufferSizeCallback _framebufferSizeDelegate;

		protected GLFWWindow(UnmanagedLibrary library, GLFWContext glfw, IntPtr handle, string title) : base(library) {
			_glfw = glfw;
			_window = handle;
			_title = title;

			MakeContextCurrent(_window);

			_gl = glfw.GLLibrary.CreateObject<GLContext>();

			SetWindowCloseCallback(_window, _windowCloseDelegate = new WindowCloseCallback(HandleWindowClose));
			SetWindowIconifyCallback(_window, _windowIconifyDelegate = new WindowIconifyCallback(HandleWindowIconify));
			SetWindowMaximizeCallback(_window, _windowMaximizeDelegate = new WindowMaximizeCallback(HandleWindowMaximize));
			SetWindowFocusCallback(_window, _windowFocusDelegate = new WindowFocusCallback(HandleWindowFocus));
			SetWindowSizeCallback(_window, _windowSizeDelegate = new WindowSizeCallback(HandleWindowSize));
			SetWindowPosCallback(_window, _windowPosDelegate = new WindowPosCallback(HandleWindowPos));
			SetWindowContentScaleCallback(_window, _windowContentScaleDelegate = new WindowContentScaleCallback(HandleWindowContentScale));
			SetWindowRefreshCallback(_window, _windowRefreshDelegate = new WindowRefreshCallback(HandleWindowRefresh));
			SetKeyCallback(_window, _keyDelegate = new KeyCallback(HandleKey));
			SetCharCallback(_window, _charDelegate = new CharCallback(HandleChar));
			SetCursorPosCallback(_window, _cursorPosDelegate = new CursorPosCallback(HandleCursorPos));
			SetCursorEnterCallback(_window, _cursorEnterDelegate = new CursorEnterCallback(HandleCursorEnter));
			SetMouseButtonCallback(_window, _mouseButtonDelegate = new MouseButtonCallback(HandleMouseButton));
			SetScrollCallback(_window, _scrollDelegate = new ScrollCallback(HandleScroll));
			SetFramebufferSizeCallback(_window, _framebufferSizeDelegate = new FramebufferSizeCallback(HandleFramebufferSize));
		}

		protected abstract void SetWindowTitle(IntPtr window, string title);

		public string Title {
			get {
				return _title;
			}
			set {
				_title = value;

				SetWindowTitle(_window, value);
			}
		}

		protected abstract void GetWindowPos(IntPtr window, out int x, out int y);
		protected abstract void SetWindowPos(IntPtr window, int x, int y);

		public (int X, int Y) Position {
			get {
				GetWindowPos(_window, out var x, out var y);

				return (x, y);
			}
			set {
				SetWindowPos(_window, value.X, value.Y);
			}
		}

		protected abstract void GetWindowSize(IntPtr window, out int width, out int height);
		protected abstract void SetWindowSize(IntPtr window, int width, int height);

		public (int Width, int Height) Size {
			get {
				GetWindowSize(_window, out var width, out var height);

				return (width, height);
			}
			set {
				SetWindowSize(_window, value.Width, value.Height);
			}
		}

		protected abstract void GetFramebufferSize(IntPtr window, out int width, out int height);

		public (int Width, int Height) FramebufferSize {
			get {
				GetFramebufferSize(_window, out var width, out var height);

				return (width, height);
			}
		}

		protected abstract void GetCursorPos(IntPtr window, out double x, out double y);

		public (double X, double Y) CursorPosition {
			get {
				GetCursorPos(_window, out var x, out var y);

				return (x, y);
			}
		}

		protected abstract void HideWindow(IntPtr window);

		public void Hide() {
			HideWindow(_window);
		}

		protected abstract void ShowWindow(IntPtr window);

		public void Show() {
			ShowWindow(_window);
		}

		public void Close() {
			HideWindow(_window);
		}

		#region WindowClose Event
		protected delegate void WindowCloseCallback(IntPtr window);
		protected abstract WindowCloseCallback? SetWindowCloseCallback(IntPtr window, WindowCloseCallback callback);
		public event Action<GLFWWindow>? OnWindowClose;
		protected void HandleWindowClose(IntPtr window) {
			OnWindowClose?.Invoke(this);
		}
		#endregion

		#region WindowIconify Event
		protected delegate void WindowIconifyCallback(IntPtr window);
		protected abstract WindowIconifyCallback? SetWindowIconifyCallback(IntPtr window, WindowIconifyCallback callback);
		public event Action<GLFWWindow>? OnWindowIconify;
		protected void HandleWindowIconify(IntPtr window) {
			OnWindowIconify?.Invoke(this);
		}
		#endregion

		#region WindowMaximize Event
		protected delegate void WindowMaximizeCallback(IntPtr window);
		protected abstract WindowMaximizeCallback? SetWindowMaximizeCallback(IntPtr window, WindowMaximizeCallback callback);
		public event Action<GLFWWindow>? OnWindowMaximize;
		protected void HandleWindowMaximize(IntPtr window) {
			OnWindowMaximize?.Invoke(this);
		}
		#endregion

		#region WindowFocus Event
		protected delegate void WindowFocusCallback(IntPtr window);
		protected abstract WindowFocusCallback? SetWindowFocusCallback(IntPtr window, WindowFocusCallback callback);
		public event Action<GLFWWindow>? OnWindowFocus;
		protected void HandleWindowFocus(IntPtr window) {
			OnWindowFocus?.Invoke(this);
		}
		#endregion

		#region WindowSize Event
		protected delegate void WindowSizeCallback(IntPtr window, int width, int height);
		protected abstract WindowSizeCallback? SetWindowSizeCallback(IntPtr window, WindowSizeCallback callback);
		public event Action<GLFWWindow, int, int>? OnWindowSize;
		protected void HandleWindowSize(IntPtr window, int width, int height) {
			OnWindowSize?.Invoke(this, width, height);
		}
		#endregion

		#region WindowPos Event
		protected delegate void WindowPosCallback(IntPtr window, int left, int top);
		protected abstract WindowPosCallback? SetWindowPosCallback(IntPtr window, WindowPosCallback callback);
		public event Action<GLFWWindow, int, int>? OnWindowPos;
		protected void HandleWindowPos(IntPtr window, int left, int top) {
			OnWindowPos?.Invoke(this, left, top);
		}
		#endregion

		#region WindowContentScale Event
		protected delegate void WindowContentScaleCallback(IntPtr window, float xscale, float yscale);
		protected abstract WindowContentScaleCallback? SetWindowContentScaleCallback(IntPtr window, WindowContentScaleCallback callback);
		public event Action<GLFWWindow, float, float>? OnWindowContentScale;
		protected void HandleWindowContentScale(IntPtr window, float xscale, float yscale) {
			OnWindowContentScale?.Invoke(this, xscale, yscale);
		}
		#endregion

		#region WindowRefresh Event
		protected delegate void WindowRefreshCallback(IntPtr window);
		protected abstract WindowRefreshCallback? SetWindowRefreshCallback(IntPtr window, WindowRefreshCallback callback);
		public event Action<GLFWWindow>? OnWindowRefresh;
		protected void HandleWindowRefresh(IntPtr window) {
			OnWindowRefresh?.Invoke(this);
		}
		#endregion

		#region Key Event
		protected delegate void KeyCallback(IntPtr window, Key key, int scanCode, InputState state, ModifierKeys modifiers);
		protected abstract KeyCallback? SetKeyCallback(IntPtr window, KeyCallback callback);
		public event Action<GLFWWindow, Key, int, InputState, ModifierKeys>? OnKey;
		protected void HandleKey(IntPtr window, Key key, int scanCode, InputState state, ModifierKeys modifiers) {
			OnKey?.Invoke(this, key, scanCode, state, modifiers);
		}
		#endregion

		#region Char Event
		protected delegate void CharCallback(IntPtr window, int codepoint);
		protected abstract CharCallback? SetCharCallback(IntPtr window, CharCallback callback);
		public event Action<GLFWWindow, int>? OnChar;
		protected void HandleChar(IntPtr window, int codepoint) {
			OnChar?.Invoke(this, codepoint);
		}
		#endregion

		#region CursorPos Event
		protected delegate void CursorPosCallback(IntPtr window, double x, double y);
		protected abstract CursorPosCallback? SetCursorPosCallback(IntPtr window, CursorPosCallback callback);
		public event Action<GLFWWindow, double, double>? OnCursorPos;
		protected void HandleCursorPos(IntPtr window, double x, double y) {
			OnCursorPos?.Invoke(this, x, y);
		}
		#endregion

		#region CursorEnter Event
		protected delegate void CursorEnterCallback(IntPtr window, bool entered);
		protected abstract CursorEnterCallback? SetCursorEnterCallback(IntPtr window, CursorEnterCallback callback);
		public event Action<GLFWWindow, bool>? OnCursorEnter;
		protected void HandleCursorEnter(IntPtr window, bool entered) {
			OnCursorEnter?.Invoke(this, entered);
		}
		#endregion

		#region MouseButton Event
		protected delegate void MouseButtonCallback(IntPtr window, MouseButton button, InputState state, ModifierKeys modifiers);
		protected abstract MouseButtonCallback? SetMouseButtonCallback(IntPtr window, MouseButtonCallback callback);
		public Action<GLFWWindow, MouseButton, InputState, ModifierKeys>? OnMouseButton;
		protected void HandleMouseButton(IntPtr window, MouseButton button, InputState state, ModifierKeys modifiers) {
			OnMouseButton?.Invoke(this, button, state, modifiers);
		}
		#endregion

		#region Scroll Event
		protected delegate void ScrollCallback(IntPtr window, double xdelta, double ydelta);
		protected abstract ScrollCallback? SetScrollCallback(IntPtr window, ScrollCallback callback);

		public event Action<GLFWWindow, double, double>? OnScroll;
		protected void HandleScroll(IntPtr window, double xdelta, double ydelta) {
			OnScroll?.Invoke(this, xdelta, ydelta);
		}
		#endregion

		#region FramebufferSize Event
		protected delegate void FramebufferSizeCallback(IntPtr window, int width, int height);
		protected abstract FramebufferSizeCallback? SetFramebufferSizeCallback(IntPtr window, FramebufferSizeCallback callback);
		public Action<GLFWWindow, int, int>? OnFramebufferSize;
		protected void HandleFramebufferSize(IntPtr window, int width, int height) {
			OnFramebufferSize?.Invoke(this, width, height);
		}
		#endregion


		protected abstract void MakeContextCurrent(IntPtr window);

		public GLContext GetGLContext() {
			MakeContextCurrent(_window);

			return _gl;
		}

		protected abstract void SwapBuffers(IntPtr window);

		public void SwapBuffers() {
			SwapBuffers(_window);
		}

		protected abstract void DestroyWindow(IntPtr window);

		protected override void DisposeUnmanaged() {
			DestroyWindow(_window);
		}
	}
}
