namespace Quilt.GLFW {
	using System;
	using Quilt.Unmanaged;

	public class GLFWUnmanagedLoader : UnmanagedLoader {
		private readonly IGLFW _glfw;
		private readonly UnmanagedLoader _loader;

		public GLFWUnmanagedLoader(IGLFW glfw, UnmanagedLoader loader) {
			_glfw = glfw;
			_loader = loader;

		}

		public override IntPtr LoadLibrary(string name) {
			return _loader.LoadLibrary(name);
		}

		public override IntPtr LoadSymbol(IntPtr library, string name) {
			var symbol = _glfw.GetProcAddress(name);
			
			return symbol != IntPtr.Zero ? symbol : _loader.LoadSymbol(library, name);
		}

		public override string? GetLibraryPath(IntPtr library) {
			return _loader.GetLibraryPath(library);
		}
	}
}
