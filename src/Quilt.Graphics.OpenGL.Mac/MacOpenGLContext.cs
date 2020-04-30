namespace Quilt.Graphics.OpenGL.Mac {
	using Quilt.Mac.AppKit;

	sealed class MacOpenGLContext : OpenGLContext {
		public NSOpenGLContext NSGLContext { get; }

		public MacOpenGLContext(NSOpenGLContext nsglContext) {
			NSGLContext = nsglContext;
		}
	}
}
