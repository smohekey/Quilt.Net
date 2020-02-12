namespace Window {
	using System;
	using Quilt;
  using Quilt.OpenGL;

  class Program {
		static void Main(string[] args) {
			var application = Application.Load("Application.xml");

			GL.GetVersion(out var major, out var minor, out var revision);

			Console.WriteLine($"GLFW {major}.{minor}.{revision}");
			Console.WriteLine(GL.GetVersionString());
		}
	}
}
