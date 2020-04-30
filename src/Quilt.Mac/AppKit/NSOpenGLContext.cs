namespace Quilt.Mac.AppKit {
	using System;
  using Quilt.Mac.CodeGen;
  using Quilt.Mac.Foundation;
	using Quilt.Mac.ObjectiveC;

	[Class]
	public abstract class NSOpenGLContext : NSObject<NSOpenGLContext, NSOpenGLContext.MetaClass> {
		protected NSOpenGLContext(IntPtr handle) : base(handle) {

		}

		[Import]
		public abstract NSOpenGLContext InitWith(NSOpenGLPixelFormat format, NSOpenGLContext shareContext);

		[Import]
		public abstract void MakeCurrentContext();

		[Import]
		public abstract void FlushBuffer();

		[Import]
		public abstract NSView View { get; }

		public new abstract class MetaClass : NSObject<NSOpenGLContext, MetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}
		}
	}
}
