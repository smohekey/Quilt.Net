namespace Window {
	using System;
	using Quilt;
	using Quilt.GLFW;

	class Program {

		static void Main(string[] args) {
			var mainWindow = new Quilt.UI.Window() {
				Title = "Cadre",
				Position = (100, 100),
				Size = (400, 400)
			};

			mainWindow.Show();

			Application.Run();

			/*if (!UnmanagedLibrary.TryLoad("glfw3", out var unmanagedGLFW, "glfw")) {
				Console.WriteLine("Couldn't open unmanaged library glfw3.");
				Environment.Exit(-1);
			}

			if (!unmanagedGLFW.TryCreateObject<IGLFW>(out var glfw)) {
				Console.WriteLine("Couldn't get managed interface for glfw.");
			}

			glfw.Init();

			glfw.GetVersion(out var major, out var minor, out var revision);

			Console.WriteLine($"GLFW {major}.{minor}.{revision}");
			Console.WriteLine(glfw.GetVersionString());

			glfw.WindowHint(Hint.ContextVersionMajor, 3);
			glfw.WindowHint(Hint.ContextVersionMinor, 3);
			glfw.WindowHint(Hint.OpenglProfile, Profile.Core);

			glfw.SetErrorCallback(Error);

			ref Window window = ref glfw.CreateWindow(1024, 1024, "Cadre");

			glfw.SetCursor(ref window, ref glfw.CreateStandardCursor(CursorType.Hand));

			glfw.SetWindowCloseCallback(ref window, WindowClose);
			glfw.SetKeyCallback(ref window, Key);
			glfw.SetCursorPosCallback(ref window, CursorPos);
			glfw.SetCursorEnterCallback(ref window, CursorEnter);
			glfw.SetMouseButtonCallback(ref window, MouseButton);
			glfw.SetScrollCallback(ref window, Scroll);
			glfw.SetFramebufferSizeCallback(ref window, FramebufferSize);

			glfw.MakeContextCurrent(ref window);

			if (!UnmanagedLibrary.TryLoad("opengl", out var unmanagedGL, new GLFWUnmanagedLoader(glfw, UnmanagedLoader.Instance), "opengl32", "GL")) {
				Console.WriteLine("Couldn't open unmanaged library opengl.");
				Environment.Exit(-1);
			}

			if (!unmanagedGL.TryCreateObject<GL>(out __gl)) {
				Console.WriteLine("Couldn't get managed interface for gl.");
				Environment.Exit(-1);
			}

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

			var vertexArray = __gl.GenVertexArray();
			__gl.BindVertexArray(vertexArray);

			var vertexBuffer = __gl.GenBuffer();
			var elementBuffer = __gl.GenBuffer();

			__gl.BindBuffer(BufferType.Array, vertexBuffer);
			__gl.BufferData(BufferType.Array, vertices, BufferUsage.StaticDraw);

			__gl.BindBuffer(BufferType.ElementArray, elementBuffer);
			__gl.BufferData(BufferType.ElementArray, indices, BufferUsage.StaticDraw);

			__gl.VertexAttribPointer(0, 3, DataType.Float, false, sizeof(float) * 3, 0);
			__gl.EnableVertexAttribArray(0);


			__gl.BindBuffer(BufferType.Array, Quilt.GL.Unmanaged.Buffer.Zero);
			__gl.BindVertexArray(VertexArray.Zero);



			var vertexShader = __gl.CreateShader(ShaderType.Vertex);
			__gl.ShaderSource(vertexShader,
				"#version 330 core\n" +
				"layout (location = 0) in vec3 position;\n" +
				"uniform float thickness;\n" +
				"attribute float lineMiter;\n" +
				"attribute vec2 lineNormal;\n" +
				"layout (location = 0) in vec3 aPos;\n" +
				"void main() {\n" +
				"  vec3 pointPos = position.xyz + vec3(lineNormal * thickness / 2.0 * lineMiter, 0.0);\n" +
				"  gl_Position = vec4(pointPos, 1.0);" +
				"}"
			);

			__gl.CompileShader(vertexShader);
			__gl.GetShaderiv(vertexShader, ShaderProperty.CompileStatus, out var success);

			if (success == 0) {
				Console.WriteLine(__gl.GetShaderInfoLog(vertexShader));
			}

			var location = __gl.GetUniformLocation(vertexShader, "thickness");

			__gl.Uniform1f(location, 10);

			var fragmentShader = __gl.CreateShader(ShaderType.Fragment);
			__gl.ShaderSource(fragmentShader,
				"#version 330 core\n" +
				"out vec4 FragColor;\n" +

				"void main() {\n" +
				"	 FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);\n" +
				"}"
			);

			__gl.CompileShader(fragmentShader);
			__gl.GetShaderiv(fragmentShader, ShaderProperty.CompileStatus, out success);

			if (success == 0) {
				Console.WriteLine(__gl.GetShaderInfoLog(fragmentShader));
			}

			var program = __gl.CreateProgram();

			__gl.AttachShader(program, vertexShader);
			__gl.AttachShader(program, fragmentShader);
			__gl.LinkProgram(program);

			__gl.GetProgramiv(program, ProgramProperty.LinkStatus, out success);

			if (success == 0) {
				Console.WriteLine(__gl.GetProgramInfoLog(program));
			}

			__gl.DeleteShader(vertexShader);
			__gl.DeleteShader(fragmentShader);

			while (!glfw.WindowShouldClose(ref window)) {
				glfw.GetFramebufferSize(ref window, out var fbWidth, out var fbHeight);

				__gl.Viewport(0, 0, fbWidth, fbHeight);
				__gl.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
				__gl.Clear();

				__gl.UseProgram(program);
				__gl.BindVertexArray(vertexArray);
				//__gl.DrawArrays(DrawMode.Triangles, 0, 3);
				__gl.DrawElements(DrawMode.Triangles, 6, DataType.UnsignedInt, 0);

				glfw.SwapBuffers(ref window);
				glfw.WaitEvents();
			}

			glfw.Terminate();*/
		}

		/*private static void FramebufferSize(ref Window window, int width, int height) {
			__gl.Viewport(0, 0, width, height);
		}

		private static void Scroll(ref Window window, double xoffset, double yoffset) {
			Console.WriteLine($"Scroll: {xoffset}, {yoffset}.");
		}

		private static void MouseButton(ref Window window, MouseButton button, InputState state, ModifierKeys modifiers) {
			Console.WriteLine($"Mouse Button: {button}, {state}, {modifiers}.");
		}
		private static void CursorEnter(ref Window window, bool entered) {
			Console.WriteLine($"Cursor Enter: {entered}.");
		}

		private static void WindowClose(ref Window window) {
			Console.WriteLine("Window Close.");
		}

		private static void Key(ref Window window, Key key, int scanCode, InputState state, ModifierKeys modifiers) {
			Console.WriteLine($"Key: {key}, {scanCode}, {state}, {modifiers}");
		}

		private static void CursorPos(ref Window window, double x, double y) {
			Console.WriteLine($"Cursor Pos: {x}, {y}.");
		}

		private static void Error(int errorCode, string description) {
			Console.WriteLine($"Error: {errorCode}, {description}.");
		}*/
	}
}
