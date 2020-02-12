namespace Quilt.OpenGL {
	using System.Reflection;
	using Quilt.Interop;
	using Quilt.OpenGL.Interop;

	public static class GL {
		private static readonly Interop.GL __gl;
		private static readonly Interop.GLFW __glfw;

		static GL() {
			var importer = new InteropImporter();

			__gl = importer.Import<Interop.GL>();
			__glfw = importer.Import<Interop.GLFW>();
		}

		public static void GetVersion(out int major, out int minor, out int revision) {
			__glfw.GetVersion(out major, out minor, out revision);
		}

		public static string GetVersionString() {
			return __glfw.GetVersionString();
		}

		public static Window CreateWindow() {
			return null;
		}
	}
}
