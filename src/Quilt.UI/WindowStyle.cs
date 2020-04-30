namespace Quilt.UI {
	using System;

	[Flags]
	public enum WindowStyle {
		HasTitle = 1 << 0,
		HasBorder = 1 << 1,
		HasCloseButton = 1 << 2,
		HasMaximizeButton = 1 << 3,
		HasMinimizeButton = 1 << 4,
		IsResizable = 1 << 5,
		IsMoveable = 1 << 6
	}
}
