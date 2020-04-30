namespace Quilt.Typography.SFNT {
	using System;

	[Flags]
	public enum SelectionFlags : ushort {
		Italic = 1 << 0,
		Underscore = 1 << 1,
		Negative = 1 << 2,
		Outlined = 1 << 3,
		Strikeout = 1 << 4,
		Bold = 1 << 5,
		Regular = 1 << 6,
		UseTypoMetrics = 1 << 7,
		WeightWidthSlope = 1 << 8,
		Oblique = 1 << 9
	}
}
