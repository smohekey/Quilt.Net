namespace Quilt.Mac.AppKit {
	using System;
  using Quilt.Mac.CodeGen;
  using Quilt.Mac.Foundation;
	using Quilt.Mac.ObjectiveC;

	[Class]
	public abstract class NSOpenGLPixelFormat : NSObject<NSOpenGLPixelFormat, NSOpenGLPixelFormat.MetaClass> {
		protected NSOpenGLPixelFormat(IntPtr handle) : base(handle) {

		}

		[Import]
		protected abstract unsafe NSOpenGLPixelFormat InitWith(NSOpenGLPixelFormatAttribute* attributes);

		public unsafe NSOpenGLPixelFormat InitWith(params NSOpenGLPixelFormatAttribute[] attributes) {
			NSOpenGLPixelFormatAttribute* attributesPtr = stackalloc NSOpenGLPixelFormatAttribute[attributes.Length + 1];

			for(var i = 0; i < attributes.Length; i++) {
				attributesPtr[i] = attributes[i];
			}

			attributesPtr[attributes.Length] = 0;

			return InitWith(attributesPtr);
		}

		public new abstract class MetaClass : NSObject<NSOpenGLPixelFormat, MetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}
		}
	}
}
