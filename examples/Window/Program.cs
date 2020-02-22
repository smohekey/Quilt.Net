namespace Window {
	using System;
	using Quilt;
	using Quilt.GLFW;

	class Program {

		static void Main(string[] args) {
			var mainWindow = new Quilt.UI.Window() {
				Title = "Cadre",
				Position = (200, 200),
				Size = (512, 512)
			};

			mainWindow.Show();

			Application.Run();
		}
	}
}
