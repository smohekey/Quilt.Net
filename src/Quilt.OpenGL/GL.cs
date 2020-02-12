using System;
namespace Quilt.OpenGL {
	using System.IO;
	using System.Reflection;
	using System.Runtime.InteropServices;
	using Quilt.Interop;
	using Quilt.OpenGL.Interop;

	public static class GL {
		static GL() {
			var context = new InteropAssemblyLoadContextBuilder(AppContext.BaseDirectory)
				.AddUnmanagedDllAliases("opengl32", "GL")
				.Build();

			context.LoadFromAssemblyName(new AssemblyName("Quilt.OpenGL.Interop"));

		}

		public static void GetVersion(out int major, out int minor, out int revision) {
			major = minor = revision = 0;
			//GLFW.GetVersion(out major, out minor, out revision); 
		}

		public static string GetVersionString() {
			//return __glfw.GetVersionString();
			return null;
		}

		public static Window CreateWindow() {
			return null;
		}
	}
}
