namespace Quilt.Mac.AppKit {
	using System;

	[Flags]
	public enum NSWindowStyleMask : ulong {
		Borderless = 0 << 0,
		Titled = 1 << 0,
		Closable = 1 << 1,
		Miniaturizable = 1 << 2,
		Resizable = 1 << 3,
		Utility = 1 << 4,
		DocModal = 1 << 6,
		NonactivatingPanel = 1 << 7,
		TexturedBackground = 1 << 8,
		Unscaled = 1 << 11,
		UnifiedTitleAndToolbar = 1 << 12,
		Hud = 1 << 13,
		FullScreenWindow = 1 << 14,
		FullSizeContentView = 1 << 15
	}
}
