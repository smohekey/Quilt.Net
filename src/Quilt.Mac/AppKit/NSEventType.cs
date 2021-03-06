﻿namespace Quilt.Mac.AppKit {
	public enum NSEventType : ulong {
		LeftMouseDown = 1,
		LeftMouseUp = 2,
		RightMouseDown = 3,
		RightMouseUp = 4,
		MouseMoved = 5,
		LeftMouseDragged = 6,
		RightMouseDragged = 7,
		MouseEntered = 8,
		MouseExited = 9,
		KeyDown = 10,
		KeyUp = 11,
		FlagsChanged = 12,
		AppKitDefined = 13,
		SystemDefined = 14,
		ApplicationDefined = 15,
		Periodic = 16,
		CursorUpdate = 17,
		Rotate = 18,
		BeginGesture = 19,

		EndGesture = 20,
		ScrollWheel = 22,
		TabletPoint = 23,
		TabletProximity = 24,
		OtherMouseDown = 25,
		OtherMouseUp = 26,
		OtherMouseDragged = 27,

		Gesture = 29,
		Magnify = 30,
		Swipe = 31,
		SmartMagnify = 32,
		QuickLook = 33,
		Pressure = 34,

		DirectTouch = 37
	}
}
