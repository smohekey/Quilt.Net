namespace Quilt.GLFW {
	using System;

	public enum Hint : int {
		Focused = 0x00020001,
		Resizable = 0x00020003,
		Visible = 0x00020004,
		Decorated = 0x00020005,
		AutoIconify = 0x00020006,
		Floating = 0x00020007,
		Maximized = 0x00020008,
		RedBits = 0x00021001,
		GreenBits = 0x00021002,
		BlueBits = 0x00021003,
		AlphaBits = 0x00021004,
		DepthBits = 0x00021005,
		StencilBits = 0x00021006,
		[Obsolete]
		AccumRedBits = 0x00021007,
		[Obsolete]
		AccumGreenBits = 0x00021008,
		[Obsolete]
		AccumBlueBits = 0x00021009,
		[Obsolete]
		AccumAlphaBits = 0x0002100a,
		[Obsolete]
		AuxBuffers = 0x0002100b,
		Stereo = 0x0002100c,
		Samples = 0x0002100d,
		SrgbCapable = 0x0002100e,
		Doublebuffer = 0x00021010,
		RefreshRate = 0x0002100f,
		ClientApi = 0x00022001,
		ContextCreationApi = 0x0002200b,
		ContextVersionMajor = 0x00022002,
		ContextVersionMinor = 0x00022003,
		ContextRobustness = 0x00022005,
		OpenglForwardCompatible = 0x00022006,
		OpenglDebugContext = 0x00022007,
		OpenglProfile = 0x00022008,
		ContextReleaseBehavior = 0x00022009,
		ContextNoError = 0x0002200a,
		JoystickHatButtons = 0x00050001,
		CocoaChDirResources = 0x00051001,
		CocoaMenuBar = 0x00051002,
		CenterCursor = 0x00020009,
		TransparentFramebuffer = 0x0002000A,
		FocusOnShow = 0x0002000C,
		ScaleToMonitor = 0x0002200C,
		CocoaRetinaFrameBuffer = 0x00023001,
		CocoaFrameName = 0x00023002,
		CocoaGraphicsSwitching = 0x00023003,
		X11ClassName = 0x00024001,
		X11InstanceName = 0x00024002
	}
}
