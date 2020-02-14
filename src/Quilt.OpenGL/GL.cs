using System;
namespace Quilt.OpenGL {
	using System.IO;
	using System.Reflection;
	using System.Runtime.InteropServices;
	
	public static class GL {

		static GL() {
			
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
