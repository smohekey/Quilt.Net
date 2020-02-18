﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;
namespace Quilt.UI {
	using System.ComponentModel;
	using System.Threading;
	using System;
	using Quilt.GLFW;
	using Quilt.GL;
  using System.Runtime.InteropServices;

  [QuiltElement(Namespace.URI)]
	public class Window : IEquatable<Window> {
		private static int __nextId = 0;

		protected Application Application { get; }

		private readonly GLFWWindow _window;
		private readonly int _id;

		private readonly GLProgram _passThroughProgram;
		private readonly GLProgram _solidLinesProgram;

		private readonly GLVertexArray _vertexArray;
		private readonly GLBuffer _vertexBuffer;
		private readonly GLBuffer _elementBuffer;

		protected Window(Application application) {
			Application = application;

			var glfw = Application._glfw;

			glfw.WindowHint(Hint.Visible, false);

			_window = glfw.CreateWindow(100, 100, "Quilt");
			_id = Interlocked.Increment(ref __nextId);

			Application._windows.Add(this);

			_window.OnWindowClose += HandleWindowClose;
			_window.OnWindowIconify += HandleWindowIconify;
			_window.OnWindowMaximize += HandleWindowMaximize;
			_window.OnWindowFocus += HandleWindowFocus;
			_window.OnWindowSize += HandleWindowSize;
			_window.OnWindowPos += HandleWindowPos;
			_window.OnWindowContentScale += HandleWindowContentScale;
			_window.OnWindowRefresh += HandleWindowRefresh;
			_window.OnKey += HandleKey;
			_window.OnChar += HandleChar;
			_window.OnCursorPos += HandleCursorPos;
			_window.OnCursorEnter += HandleCursorEnter;
			_window.OnMouseButton += HandleMouseButton;
			_window.OnScroll += HandleScroll;
			_window.OnFramebufferSize += HandleFramebufferSize;

			var gl = _window.GetGLContext();

			_passThroughProgram = CreateProgram(gl, "passThrough");
			_solidLinesProgram = CreateProgram(gl, "solidLines");

			var vertices = new[] {
				0.5f,  0.5f, 0.0f,  // top right
				0.5f, -0.5f, 0.0f,  // bottom right
				-0.5f, -0.5f, 0.0f,  // bottom left
				-0.5f,  0.5f, 0.0f   // top left 
			};

			var indices = new uint[] {  // note that we start from 0!
				0, 1, 3,   // first triangle
				1, 2, 3    // second triangle
			};

			_vertexArray = gl.CreateVertexArray();
			(_vertexBuffer, _elementBuffer) = gl.CreateBuffers(2);

			using var vaBinding = _vertexArray.Bind();
			using var vbBinding = _vertexBuffer.Bind(BufferType.Array);
			using var ebBinding = _elementBuffer.Bind(BufferType.ElementArray);

			vbBinding.BufferData(vertices, BufferUsage.StaticDraw);
			ebBinding.BufferData(indices, BufferUsage.StaticDraw);

			//vaBinding.VertexAttributePointer(0, 3, DataType.Float, false, Marshal.SizeOf<float>() * 3, 0);
			vaBinding.VertexAttributePointer(0, 3, DataType.Float, false, 0, 0);
			vaBinding.EnableVertexAttribute(0);
		}

		protected GLProgram CreateProgram(GLContext gl, string name) {
			var assembly = typeof(Window).Assembly;
			using var solidLinesVertexShaderSource = assembly.GetManifestResourceStream($"Quilt.UI.Shaders.{name}.vert")!;
			using var solidLinesFragmentShaderSource = assembly.GetManifestResourceStream($"Quilt.UI.Shaders.{name}.frag")!;

			using var solidLinesVertexShader = gl.CreateVertexShader(solidLinesVertexShaderSource);
			using var solidLinesFragmentShader = gl.CreateFragmentShader(solidLinesFragmentShaderSource);

			return gl.CreateProgram(solidLinesVertexShader, solidLinesFragmentShader);
		}

		public Window() : this(Application.Instance) {

		}

		//public event PropertyChangedEventHandler? PropertyChanged;
		//public event PropertyChangingEventHandler? PropertyChanging;

		#region GLFWWindow.OnWindowClose Event
		public event Func<Window, bool>? OnWindowClosing;
		private void HandleWindowClose(GLFWWindow window) {
			if (OnWindowClosing?.Invoke(this) ?? true) {
				_window.Hide();

				Application._windows.Remove(this);
			}
		}
		#endregion

		#region GLFWWindow.OnWindowIconify Event

		public event Action<Window>? OnWindowIconified;
		private void HandleWindowIconify(GLFWWindow window) {
			OnWindowIconified?.Invoke(this);
		}
		#endregion

		#region GLFWWindow.OnWindowMaximise Event
		public event Action<Window>? OnMaximise;
		private void HandleWindowMaximize(GLFWWindow window) {
			OnMaximise?.Invoke(this);
		}
		#endregion

		#region GLFWWindow.OnWindowFocus Event
		public Action<Window>? OnFocused;
		private void HandleWindowFocus(GLFWWindow window) {
			OnFocused?.Invoke(this);
		}
		#endregion

		#region GLFWWindow.OnWindowResize Event
		public Action<Window, int, int>? OnResized;
		private void HandleWindowSize(GLFWWindow window, int width, int height) {
			OnResized?.Invoke(this, width, height);
		}
		#endregion

		#region GLFWWindow.OnWindowPos Event
		public event Action<Window, int, int>? OnMoved;
		private void HandleWindowPos(GLFWWindow window, int x, int y) {
			OnMoved?.Invoke(this, x, y);
		}
		#endregion

		#region GLFWWindow.OnWindowContentScale Event
		public event Action<Window, float, float>? OnContentScaled;
		private void HandleWindowContentScale(GLFWWindow window, float xscale, float yscale) {
			OnContentScaled?.Invoke(this, xscale, yscale);
		}
		#endregion

		#region GLFWWindow.OnWindowRefresh Event
		private void HandleWindowRefresh(GLFWWindow window) {
			var (width, height) = window.FramebufferSize;
			var gl = window.GetGLContext();

			gl.Viewport(0, 0, width, height);

			gl.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
			gl.Clear(BufferBit.Color | BufferBit.Depth | BufferBit.Stencil);

			using var program = _passThroughProgram.Use();
			using var binding = _vertexArray.Bind();

			gl.DrawElements(DrawMode.Triangles, 6, DataType.UnsignedInt, 0);

			/*gl.Begin(DrawMode.Triangles);

			gl.Color(0.5f, 0, 0);

			gl.Vertex(300.0f, 210.0f);
			gl.Vertex(340.0f, 215.0f);
			gl.Vertex(320.0f, 250.0f);

			gl.End();

			gl.Flush();*/

			window.SwapBuffers();
		}
		#endregion

		#region GLFWWindow.OnKey Event
		private void HandleKey(GLFWWindow window, Key key, int scanCode, InputState state, ModifierKeys modifiers) {

		}
		#endregion

		#region GLFWWindow.OnChar Event
		private void HandleChar(GLFWWindow window, int codepoint) {

		}
		#endregion

		#region GLFWWindow.OnCursorPos Event
		public event Action<Window, double, double>? OnMouseMoved;
		private void HandleCursorPos(GLFWWindow window, double x, double y) {
			OnMouseMoved?.Invoke(this, x, y);
		}
		#endregion

		#region GLFWWindow.OnCursorEnter Event
		public event Action<Window, double, double>? OnMouseEntered;
		public event Action<Window, double, double>? OnMouseExited;
		private void HandleCursorEnter(GLFWWindow window, bool entered) {
			var (x, y) = window.CursorPosition;

			if (entered) {
				OnMouseEntered?.Invoke(this, x, y);
			} else {
				OnMouseExited?.Invoke(this, x, y);
			}
		}
		#endregion

		#region GLFWWindow.OnMouseButton Event
		public event Action<Window, MouseButton, ModifierKeys>? OnMousePressed;
		public event Action<Window, MouseButton, ModifierKeys>? OnMouseReleased;
		public event Action<Window, MouseButton, ModifierKeys>? OnMouseRepeated;

		protected void HandleMouseButton(GLFWWindow window, MouseButton button, InputState state, ModifierKeys modifiers) {
			switch (state) {
				case InputState.Press: {
					OnMousePressed?.Invoke(this, button, modifiers);

					break;
				}

				case InputState.Release: {
					OnMouseReleased?.Invoke(this, button, modifiers);

					break;
				}

				case InputState.Repeat: {
					OnMouseRepeated?.Invoke(this, button, modifiers);

					break;
				}
			}
		}
		#endregion

		#region GLFWWindow.Scroll Event
		public event Action<Window, double, double>? OnMouseScrolled;
		private void HandleScroll(GLFWWindow window, double deltaX, double deltaY) {
			OnMouseScrolled?.Invoke(this, deltaX, deltaY);
		}
		#endregion

		#region GLFWWindow.FramebufferSize
		private void HandleFramebufferSize(GLFWWindow window, int width, int height) {
			var gl = window.GetGLContext();

			gl.Viewport(0, 0, width, height);
		}
		#endregion

		[QuiltAttribute]
		public virtual string Title {
			get {
				return _window.Title;
			}
			set {
				_window.Title = value;
			}
		}

		[QuiltAttribute]
		public virtual (int Left, int Right) Position {
			get {
				return _window.Position;
			}
			set {
				_window.Position = value;
			}
		}

		[QuiltAttribute]
		public virtual (int Width, int Height) Size {
			get {
				return _window.Size;
			}
			set {
				_window.Size = value;
			}
		}

		public void Show() {
			_window.Show();
		}

		/*protected virtual void Render(Context context) {
			const int CORNER_RADIUS = 3;

			int x = 100;
			int y = 100;
			int w = 400;
			int h = 400;

			_context.Save();
			_context.BeginPath();
			_context.RoundedRect(x, y, w, h, CORNER_RADIUS);
			_context.FillColor(NVG.RGBA(28, 30, 34, 192));
			_context.Fill();

			var shadowPaint = _context.BoxGradient(x, y + 2, w, h, CORNER_RADIUS * 2, 10, NVG.RGBA(0, 0, 0, 128), NVG.RGBA(0, 0, 0, 0));
			_context.BeginPath();
			_context.Rect(x - 10, y - 10, w + 20, h + 30);
			_context.RoundedRect(x, y, w, h, CORNER_RADIUS);
			_context.PathWinding(Solidity.Hole);
			_context.FillPaint(shadowPaint);
			_context.Fill();

			var headerPaint = _context.LinearGradient(x, y, x, y + 15, NVG.RGBA(255, 255, 255, 8), NVG.RGBA(0, 0, 0, 16));
			_context.BeginPath();
			_context.RoundedRect(x + 1, y + 1, w - 2, 30, CORNER_RADIUS - 1);
			_context.FillPaint(headerPaint);
			_context.Fill();
			_context.MoveTo(x + 0.5f, y + 0.5f + 30);
			_context.LineTo(x + 0.5f + w - 1, y + 0.5f + 30);
			_context.StrokeColor(NVG.RGBA(0, 0, 0, 32));
			_context.Stroke();

			_context.FontSize(18.0f);
			_context.FontFace("sans-bold");
			_context.TextAlign(Align.Center | Align.Middle);
			_context.FontBlur(2);
			_context.FillColor(NVG.RGBA(0, 0, 0, 128));
			_context.Text(x + w / 2, y + 16 + 1, "hello there");

			_context.FontBlur(0);
			_context.FillColor(NVG.RGBA(220, 220, 220, 160));
			_context.Text(x + w / 2, y + 16, "hello there");
			_context.Restore();

			foreach (var child in ChildNodes) {
				switch (child) {
					case Component component: {
						component.Render(context);

						break;
					}

					default: {
						break;
					}
				}
			}
		}*/

		public override int GetHashCode() {
			return _id;
		}

		public override bool Equals(object? obj) {
			return obj is Window other && Equals(other);
		}

		public bool Equals(Window? other) {
			return other != null && _id == other._id;
		}
	}
}
