namespace Window {
	using System;
	using Quilt;
  using Quilt.OpenGL;
  using Quilt.OpenGL.Unmanaged;
  using Quilt.Unmanaged;

  class Program {
		static void Main(string[] args) {
			var application = Application.Load("Application.xml");

			if(!UnmanagedLibrary.TryLoad("opengl", out var unmanagedGL, "opengl32", "GL")) {
				Console.WriteLine("Couldn't open unmanaged library opengl.");
				Environment.Exit(-1);
			}

			if (!UnmanagedLibrary.TryLoad("glfw3", out var unmanagedGLFW)) {
				Console.WriteLine("Couldn't open unmanaged library glfw3.");
				Environment.Exit(-1);
			}

			if (!unmanagedGLFW.TryQueryInterface<IGLFW>(out var glfw)) {
				Console.WriteLine("Couldn't get managed interface for glfw.");
			}

			glfw.Init();

			glfw.GetVersion(out var major, out var minor, out var revision);

			Console.WriteLine($"GLFW {major}.{minor}.{revision}");

			var window = glfw.CreateWindow(1024, 1024, "Cadre");
			Console.WriteLine(glfw.GetVersionString());
		}
	}
}
