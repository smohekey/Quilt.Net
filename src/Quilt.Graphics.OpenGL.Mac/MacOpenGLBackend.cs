namespace Quilt.Graphics.OpenGL.Mac {
	using System;
  using System.Collections.Generic;
  using System.Diagnostics.CodeAnalysis;
  using Quilt.Graphics;
	using Quilt.Mac.AppKit;

	[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "This is instantiated by Quilt.Graphics.Backend")]
	internal sealed class MacOpenGLBackend : OpenGLBackend {
		public override bool IsPreferred => false;

		public override Context? CreateContext(ContextOptions options, IntPtr window) {
			using var pixelFormat = CreatePixelFormat(options);

			var nsglContext = NSOpenGLContext.Alloc().InitWith(pixelFormat, null);

			return new MacOpenGLContext(nsglContext);
		}

		public override Context? CreateContext(ContextOptions options, Context shareContext, IntPtr window) {
			using var pixelFormat = CreatePixelFormat(options);

			var glShareContext = shareContext as MacOpenGLContext ?? throw new ArgumentException("Attempt to mix contexts from different backends.", nameof(shareContext));
			var nsglContext = NSOpenGLContext.Alloc().InitWith(pixelFormat, glShareContext.NSGLContext);

			return new MacOpenGLContext(nsglContext);
		}

		private static NSOpenGLPixelFormat CreatePixelFormat(ContextOptions options) {
			var pixelFormat = options.PixelFormat;
			var attributes = new List<NSOpenGLPixelFormatAttribute>();
			
			if(pixelFormat.DataType == PixelFormatDataType.Float) {
				attributes.Add(NSOpenGLPixelFormatAttribute.ColorFloat);
			}

			attributes.Add(NSOpenGLPixelFormatAttribute.Accelerated);
			attributes.Add(NSOpenGLPixelFormatAttribute.ClosestPolicy);
			attributes.Add(NSOpenGLPixelFormatAttribute.OpenGLProfile);
			attributes.Add((int)NSOpenGLProfile.Core4_1);
			attributes.Add(NSOpenGLPixelFormatAttribute.DoubleBuffer);

			var colorBits = pixelFormat.RedBits + pixelFormat.GreenBits + pixelFormat.BlueBits;

			if (colorBits > 0) {
				if (colorBits < 15) {
					colorBits = 15;
				}

				attributes.Add(NSOpenGLPixelFormatAttribute.ColorSize);
				attributes.Add((byte)colorBits);
			}

			if (pixelFormat.AlphaBits > 0) {
				attributes.Add(NSOpenGLPixelFormatAttribute.AlphaSize);
				attributes.Add(pixelFormat.AlphaBits);
			}

			if(pixelFormat.DepthBits > 0) {
				attributes.Add(NSOpenGLPixelFormatAttribute.DepthSize);
				attributes.Add(pixelFormat.DepthBits);
			}

			if(pixelFormat.StencilBits > 0) {
				attributes.Add(NSOpenGLPixelFormatAttribute.StencilSize);
				attributes.Add(pixelFormat.StencilBits);
			}

			return NSOpenGLPixelFormat.Alloc().InitWith(attributes.ToArray());
		}
	}
}
