namespace Quilt.Typography.SFNT {
	public enum HeadFlags {
		/*
		bit 0 - y value of 0 specifies baseline
		bit 1 - x position of left most black bit is LSB
		bit 2 - scaled point size and actual point size will differ (i.e. 24 point glyph differs from 12 point glyph scaled by factor of 2)
		bit 3 - use integer scaling instead of fractional
		bit 4 - (used by the Microsoft implementation of the TrueType scaler)
		bit 5 - This bit should be set in fonts that are intended to e laid out vertically, and in which the glyphs have been drawn such that an x-coordinate of 0 corresponds to the desired vertical baseline.
		bit 6 - This bit must be set to zero.
		bit 7 - This bit should be set if the font requires layout for correct linguistic rendering (e.g. Arabic fonts).
		bit 8 - This bit should be set for an AAT font which has one or more metamorphosis effects designated as happening by default.
		bit 9 - This bit should be set if the font contains any strong right-to-left glyphs.
		bit 10 - This bit should be set if the font contains Indic-style rearrangement effects.
		bits 11-13 - Defined by Adobe.
		bit 14 - This bit should be set if the glyphs in the font are simply generic symbols for code point ranges, such as for a last resort font.
		*/
		BaselineIsY0 = 1 << 0,
		LeftIsX0 = 1 << 1,

	}
}
