namespace Quilt.Mac.AppKit {
  using System;
  using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct NSOpenGLPixelFormatAttribute {
		private uint _value;

		public NSOpenGLPixelFormatAttribute(uint value) {
			_value = value;
		}

		public static implicit operator NSOpenGLPixelFormatAttribute(uint value) => new NSOpenGLPixelFormatAttribute(value);

		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that the pixel format selection is open to all available renderers, including debug and special-purpose renderers that are not OpenGL compliant.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute AllRenderers = 1;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that only double-buffered pixel formats are considered. Otherwise, only single-buffered pixel formats are considered.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute DoubleBuffer = 5;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that only triple-buffered pixel formats are considered. Otherwise, only single-buffered pixel formats are considered.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute TripleBuffer = 3;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that only stereo pixel formats are considered. Otherwise, only monoscopic pixel formats are considered.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute Stereo = 6;
		/// <summary>
		/// Value is a nonnegative integer that indicates the desired number of auxiliary buffers. Pixel formats with the smallest number of auxiliary buffers that meets or exceeds the specified number are preferred.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute AuxBuffers = 7;
		/// <summary>
		/// Value is a nonnegative buffer size specification. A color buffer that most closely matches the specified size is preferred. If unspecified, OpenGL chooses a color size that matches the screen.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute ColorSize = 8;
		/// <summary>
		/// Value is a nonnegative buffer size specification. An alpha buffer that most closely matches the specified size is preferred.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute AlphaSize = 11;
		/// <summary>
		/// Value is a nonnegative depth buffer size specification. A depth buffer that most closely matches the specified size is preferred.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute DepthSize = 12;
		/// <summary>
		/// Value is a nonnegative integer that indicates the desired number of stencil bitplanes. The smallest stencil buffer of at least the specified size is preferred.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute StencilSize = 13;
		/// <summary>
		/// Value is a nonnegative buffer size specification. An accumulation buffer that most closely matches the specified size is preferred.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute AccumSize = 14;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that the pixel format choosing policy is altered for the color, depth, and accumulation buffers such that only buffers of size greater than or equal to the desired size are considered.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute MinimumPolicy = 51;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that the pixel format choosing policy is altered for the color, depth, and accumulation buffers such that, if a nonzero buffer size is requested, the largest available buffer is preferred.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute MaximumPolicy = 52;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that only renderers that are capable of rendering to an offscreen memory area and have buffer depth exactly equal to the desired buffer depth are considered. The NSOpenGLPFAClosestPolicy attribute is implied.
		/// </summary>
		[Obsolete]
		public static readonly NSOpenGLPixelFormatAttribute OffScreen = 53;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that only renderers that are capable of rendering to a full-screen drawable are considered. The NSOpenGLPFASingleRenderer attribute is implied.
		/// </summary>
		[Obsolete]
		public static readonly NSOpenGLPixelFormatAttribute FullScreen = 54;
		/// <summary>
		/// Value is a nonnegative number indicating the number of multisample buffers.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute SampleBuffers = 55;
		/// <summary>
		/// Value is a nonnegative indicating the number of samples per multisample buffer.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute Samples = 56;
		/// <summary>
		/// Each auxiliary buffer has its own depth stencil.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute AuxDepthStencil = 57;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that only renderers that are capable using buffers storing floating point pixels are considered. This should be accompanied by a NSOpenGLPFAColorSize of 64 (for half float pixel components) or 128 (for full float pixel components). Note, not all hardware supports floating point color buffers thus the returned pixel format could be NULL.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute ColorFloat = 58;
		/// <summary>
		/// A Boolean attribute. If present and used with NSOpenGLPFASampleBuffers and NSOpenGLPFASamples, this attribute hints to OpenGL to prefer multi-sampling. Multi-sampling will sample textures at the back buffer dimensions vice the multi-sample buffer dimensions and use that single sample for all fragments with coverage on the back buffer location. This means less total texture samples than with super-sampling (by a factor of the number of samples requested) and will likely be faster though less accurate (texture sample wise) than super-sampling. If the underlying video card does not have enough VRAM to support this feature, this hint does nothing.
		/// The NSOpenGLPFASampleBuffers and NSOpenGLPFASamples attributes must be configured to request anti-aliasing as follows:
		/// <code>NSOpenGLPFAMultisample,
		/// NSOpenGLPFASampleBuffers, 1
		/// NSOpenGLPFASamples, 4
		/// </code>
		/// If after adding these options, multisamping still does not work, try removing the NSOpenGLPFAPixelBuffer attribute (if present). Some graphics cards may not support this option in specific versions of macOS. If removing the attribute still does not enable multisampling, try adding the NSOpenGLPFANoRecovery attribute.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute Multisample = 59;
		/// <summary>
		/// A Boolean attribute. If present and used with NSOpenGLPFASampleBuffers and NSOpenGLPFASamples, this attribute hints to OpenGL to prefer super-sampling. Super-sampling will process fragments with a texture sample per fragment and would likely be slower than multi-sampling. If the pixel format is not requesting anti-aliasing, this hint does nothing.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute Supersample = 60;
		/// <summary>
		/// A Boolean attribute. If present and used with NSOpenGLPFASampleBuffers and NSOpenGLPFASampleBuffers, this attribute hints to OpenGL to update multi-sample alpha values to ensure the most accurate rendering. If pixel format is not requesting antialiasing then this hint does nothing.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute SampleAlpha = 61;
		/// <summary>
		/// Value is a nonnegative renderer ID number. OpenGL renderers that match the specified ID are preferred. Constants to select specific renderers are provided in the CGLRenderers.h header of the OpenGL framework. Of note is kCGLRendererGenericID which selects the Apple software renderer. The other constants select renderers for specific hardware vendors.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute RendererID = 70;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that a single rendering engine is chosen. On systems with multiple screens, this disables OpenGL’s ability to drive different monitors through different graphics accelerator cards with a single context. This attribute is not generally useful.
		/// </summary>
		[Obsolete]
		public static readonly NSOpenGLPixelFormatAttribute SingleRenderer = 71;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that OpenGL’s failure recovery mechanisms are disabled. Normally, if an accelerated renderer fails due to lack of resources, OpenGL automatically switches to another renderer. This attribute disables these features so that rendering is always performed by the chosen renderer. This attribute is not generally useful.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute NoRecovery = 72;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that only hardware-accelerated renderers are considered. If not present, accelerated renderers are still preferred.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute Accelerated = 73;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that the pixel format choosing policy is altered for the color buffer such that the buffer closest to the requested size is preferred, regardless of the actual color buffer depth of the supported graphics device.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute ClosestPolicy = 74;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that OpenGL only considers renderers that have a back color buffer the full size of the drawable (regardless of window visibility) and that guarantee the back buffer contents to be valid after a call to NSOpenGLContext object’s flushBuffer.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute BackingStore = 76;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that only renderers that are capable of rendering to a window are considered. This attribute is implied if neither NSOpenGLPFAFullScreen nor NSOpenGLPFAOffScreen is specified.
		/// </summary>
		[Obsolete]
		public static readonly NSOpenGLPixelFormatAttribute Window = 80;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that pixel format selection is only open to OpenGL-compliant renderers. This attribute is implied unless NSOpenGLPFAAllRenderers is specified. This attribute is not useful in the attribute array.
		/// </summary>
		[Obsolete]
		public static readonly NSOpenGLPixelFormatAttribute Compliant = 83;
		/// <summary>
		/// Value is a bit mask of supported physical screens. All screens specified in the bit mask are guaranteed to be supported by the pixel format. Screens not specified in the bit mask may still be supported. The bit mask is managed by the CoreGraphics’s DirectDisplay, available in the CGDirectDisplay.h header of the ApplicationServices umbrella framework. A CGDirectDisplayID must be converted to an OpenGL display mask using the function CGDisplayIDToOpenGLDisplayMask. This attribute is not generally useful.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute ScreenMask = 84;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that rendering to a pixel buffer is enabled.
		/// </summary>
		[Obsolete]
		public static readonly NSOpenGLPixelFormatAttribute PixelBuffer = 90;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that rendering to a pixel buffer on an offline renderer is enabled.
		/// </summary>
		[Obsolete]
		public static readonly NSOpenGLPixelFormatAttribute RemotePixelBuffer = 91;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that offline renderers may be used.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute AllowOfflineRenderers = 96;
		/// <summary>
		/// If present, this attribute indicates that only renderers that can execute OpenCL programs should be used.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute AcceleratedCompute = 97;
		/// <summary>
		/// The associated value can be any of the constants defined in OpenGL Profiles. If it is present in the attribute arrays, only renderers capable of supporting an OpenGL context that provides the functionality promised by the profile are considered.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute OpenGLProfile = 99;
		/// <summary>
		/// The number of virtual screens in this format.
		/// </summary>
		public static readonly NSOpenGLPixelFormatAttribute VirtualScreenCount = 128;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that only renderers that do not have any failure modes associated with a lack of video card resources are considered. This attribute is not generally useful.
		/// </summary>
		[Obsolete]
		public static readonly NSOpenGLPixelFormatAttribute Robust = 75;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that the renderer is multi-processor safe.
		/// </summary>
		[Obsolete]
		public static readonly NSOpenGLPixelFormatAttribute MPSafe = 78;
		/// <summary>
		/// A Boolean attribute. If present, this attribute indicates that only renderers capable of driving multiple screens are considered. This attribute is not generally useful.
		/// </summary>
		[Obsolete]
		public static readonly NSOpenGLPixelFormatAttribute MultiScreen = 81;
	}
}
