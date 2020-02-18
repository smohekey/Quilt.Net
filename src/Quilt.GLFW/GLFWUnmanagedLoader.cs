namespace Quilt.GLFW {
	using System;
	using Quilt.Unmanaged;

	public class GLFWUnmanagedLoader : UnmanagedLoader {
		private readonly GLFWContext _context;
		private readonly UnmanagedLoader _loader;

		public GLFWUnmanagedLoader(GLFWContext context, UnmanagedLoader loader) {
			_context = context;
			_loader = loader;
		}

		public override IntPtr LoadLibrary(string name) {
			return _loader.LoadLibrary(name);
		}

		public override IntPtr LoadSymbol(IntPtr library, string name) {
			var symbol = _context.GetProcAddress(name);

			return symbol != IntPtr.Zero ? symbol : _loader.LoadSymbol(library, name);
		}

		public override string? GetLibraryPath(IntPtr library) {
			return _loader.GetLibraryPath(library);
		}
	}
}
