namespace Quilt.Mac.AppKit {
	using System;
  using Quilt.Mac.CodeGen;
  using Quilt.Mac.Core.Graphics;
  using Quilt.Mac.Foundation;
  using Quilt.Mac.ObjectiveC;

	[Class]
	public abstract class NSFont : NSObject<NSFont, NSFont.MetaClass> {
		public NSFont(IntPtr handle) : base(handle) {

		}

		/// <summary>
		/// Returns the Aqua system font used for standard interface items that are rendered in boldface type in the specified size.
		/// 
		/// If fontSize is 0 or negative, returns the boldface system font at the default size.
		/// </summary>
		/// <param name="ofSize">The desired font size specified in points. If you specify 0.0 or a negative number for this parameter, the method returns the system font at the default size.</param>
		/// <returns>A font object of the specified size.</returns>
		public static NSFont BoldSystemFont(CGFloat ofSize) => Meta.BoldSystemFont(ofSize);

		/// <summary>
		/// Returns the font used for the content of controls in the specified size.
		/// 
		/// For example, in a table, the user’s input uses the control content font, and the table’s header uses another font. 
		/// If fontSize is 0 or negative, returns the control content font at the default size.
		/// </summary>
		/// <param name="ofSize">The size in points to which the font is scaled.</param>
		/// <returns>A font object of the specified size.</returns>
		public static NSFont ControlContentFont(CGFloat ofSize) => Meta.ControlContentFont(ofSize);

		/// <summary>
		/// Returns the font used for standard interface labels in the specified size.
		/// </summary>
		/// <param name="ofSize">The size in points to which the font is scaled.</param>
		/// <returns>A font object of the specified size.</returns>
		public static NSFont LabelFont(CGFloat ofSize) => Meta.LabelFont(ofSize);

		/// <summary>
		/// Returns the font used for menu items, in the specified size.
		/// 
		/// If fontSize is 0 or negative, returns the menu items font with the default size.
		/// </summary>
		/// <param name="ofSize">The size in points to which the font is scaled.</param>
		/// <returns></returns>
		public static NSFont MenuFont(CGFloat ofSize) => Meta.MenuFont(ofSize);

		/// <summary>
		/// Returns the font used for menu bar items, in the specified size.
		/// 
		/// If fontSize is 0 or negative, returns the menu bar font with the default size.
		/// </summary>
		/// <param name="ofSize">The size in points to which the font is scaled.</param>
		/// <returns>A font object of the specified size.</returns>
		public static NSFont MenuBarFont(CGFloat ofSize) => Meta.MenuBarFont(ofSize);

		/// <summary>
		/// Returns the font used for standard interface items, such as button labels, menu items, and so on, in the specified size.
		/// 
		/// If fontSize is 0 or negative, returns this font at the default size. This method is equivalent to <see cref="SystemFont(CGFloat)"/>.
		/// </summary>
		/// <param name="ofSize">The size in points to which the font is scaled.</param>
		/// <returns>A font object of the specified size.</returns>
		public static NSFont MessageFont(CGFloat ofSize) => Meta.MessageFont(ofSize);

		/// <summary>
		/// Returns a monospace version of the system font with the specified size and weight.
		/// 
		/// Use the returned font for interface items that require monospaced glyphs. 
		/// The returned font includes monospaced glyphs for the Latin characters and the symbols commonly found in source code. 
		/// Glyphs for other symbols are usually wider or narrower than the monospaced characters. 
		/// To ensure the font uses fixed spacing for all characters, apply the <see cref="NSFontFixedAdvanceAttribute"/> attribute to the any strings you render.
		/// </summary>
		/// <param name="ofSize">The desired font size specified in points. If you specify 0.0 or a negative number for this parameter, the method returns the system font at the default size.</param>
		/// <param name="weight">The desired weight of font lines, specified as one of the constants in <see cref="NSFontWeight"/>.</param>
		/// <returns>A font object containing a monospace version of the system font at the specified size and weight.</returns>
		public static NSFont MonospacedSystemFont(CGFloat ofSize, NSFontWeight weight) => Meta.MonospacedSystemFont(ofSize, weight);

		/// <summary>
		/// Returns a version of the standard system font that contains monospaced digit glyphs.
		/// 
		/// The font returned by this method has monospaced digit glyphs. 
		/// Glyphs for other characters and symbols may be wider or narrower than the monospaced characters. 
		/// To ensure the font uses fixed spacing for all characters, apply the <see cref="NSFontFixedAdvanceAttribute"/> attribute to the any strings you render.
		/// </summary>
		/// <param name="ofSize">The desired font size specified in points. If you specify 0.0 or a negative number for this parameter, the method returns the system font at the default size.</param>
		/// <param name="weight">The desired weight of font lines, specified as one of the constants in <see cref="NSFontWeight"/>.</param>
		/// <returns>A font object containing the system font with monospace digits at the specified size and weight.</returns>
		public static NSFont MonospacedDigitSystemFont(CGFloat ofSize, NSFontWeight weight) => Meta.MonospacedDigitSystemFont(ofSize, weight);

		/// <summary>
		/// Returns the font used for palette window title bars, in the specified size.
		/// </summary>
		/// <param name="ofSize">The size in points to which the font is scaled.</param>
		/// <returns>A font object of the specified size.</returns>
		public static NSFont PaletteFont(CGFloat ofSize) => Meta.PaletteFont(ofSize);

		/// <summary>
		/// Returns the Aqua system font used for standard interface items, such as button labels, menu items, and so on, in the specified size.
		/// </summary>
		/// <param name="ofSize">The desired font size specified in points. If you specify 0.0 or a negative number for this parameter, the method returns the system font at the default size.</param>
		/// <returns>A font object of the specified size.</returns>
		public static NSFont SystemFont(CGFloat ofSize) => Meta.SystemFont(ofSize);

		/// <summary>
		/// Returns the standard system font with the specified size and weight.
		/// </summary>
		/// <param name="ofSize">The desired font size specified in points. If you specify 0.0 or a negative number for this parameter, the method returns the system font at the default size.</param>
		/// <param name="weight">The desired weight of font lines, specified as one of the constants in <see cref="NSFontWeight"/>.</param>
		/// <returns>A font object containing the system font at the specified size and weight.</returns>
		public static NSFont SystemFont(CGFloat ofSize, NSFontWeight weight) => Meta.SystemFont(ofSize, weight);

		/// <summary>
		/// Returns the font used for window title bars, in the specified size.Returns the font used for window title bars, in the specified size.
		/// 
		/// If fontSize is 0 or negative, returns the title bar font at the default size. This method is equivalent to <see cref="BoldSystemFont"/>.
		/// </summary>
		/// <param name="ofSize">The size in points to which the font is scaled.</param>
		/// <returns>A font object of the specified size.</returns>
		public static NSFont TitleBarFont(CGFloat ofSize) => Meta.TitleBarFont(ofSize);

		/// <summary>
		/// Returns the font used for tool tips labels, in the specified size.
		/// 
		/// If fontSize is 0 or negative, returns the tool tips font at the default size
		/// </summary>
		/// <param name="ofSize">The size in points to which the font is scaled.</param>
		/// <returns>A font object of the specified size.</returns>
		public static NSFont ToolTipsFont(CGFloat ofSize) => Meta.ToolTipsFont(ofSize);

		/// <summary>
		/// Returns the font used by default for documents and other text under the user’s control (that is, text whose font the user can normally change), in the specified size.
		/// 
		/// If fontSize is 0 or negative, returns the user font at the default size.
		/// </summary>
		/// <param name="ofSize">The size in points to which the font is scaled.</param>
		/// <returns>A font object of the specified size.</returns>
		public static NSFont UserFont(CGFloat ofSize) => Meta.UserFont(ofSize);

		/// <summary>
		/// Returns the font used by default for documents and other text under the user’s control (that is, text whose font the user can normally change), when that font should be fixed-pitch, in the specified size.
		/// 
		/// If fontSize is 0 or negative, returns the fixed-pitch font at the default size.
		///
		/// The system does not guarantee that all the glyphs in a fixed-pitch font are the same width.For example, certain Japanese fonts are dual - pitch, and other fonts may have nonspacing marks that can affect the display of other glyphs.
		/// </summary>
		/// <param name="ofSize">The size in points to which the font is scaled.</param>
		/// <returns>A font object of the specified size.</returns>
		public static NSFont UserFixedPitchFont(CGFloat ofSize) => Meta.UserFixedPitchFont(ofSize);

		/// <summary>
		/// Returns the size of the standard system font.
		/// </summary>
		public static CGFloat SystemFontSize => Meta.SystemFontSize;

		/// <summary>
		/// Returns the size of the standard small system font.
		/// </summary>
		public static CGFloat SmallSystemFontSize => Meta.SmallSystemFontSize;

		/// <summary>
		/// Returns the size of the standard label font.
		/// 
		/// The label font (Lucida Grande Regular 10 point) is used for the labels on toolbar buttons and to label tick marks on full-size sliders. 
		/// See The macOS Environment in macOS Human Interface Guidelines for more information about system fonts.
		/// </summary>
		public static CGFloat LabelFontSize => Meta.LabelFontSize;

		/// <summary>
		/// The font descriptor object for the font.
		/// 
		/// The font descriptor contains a mutable dictionary of optional attributes for creating an NSFont object. For more information about font descriptors, <see cref="NSFontDescriptor"/>.
		/// </summary>
		[Import]
		public abstract NSFontDescriptor FontDescriptor { get; }

		/// <summary>
		/// A Boolean value indicating whether all glyphs in the font have the same advancement.
		/// 
		/// The value of this property is true when all glyphs have the same advancement or false when they do not.
		/// Some Japanese fonts encoded with the scheme “EUC12-NJE-CFEncoding” return that they have the same advancement, but actually encode glyphs with one of two advancements, for historical compatibility. You may need to handle such fonts specially for some applications.
		/// </summary>
		[Import]
		public abstract bool FixedPitch { get; }

		/// <summary>
		/// The point size of the font.
		/// 
		/// If the font has a nonstandard matrix, the point size is the effective vertical point size.
		/// </summary>
		[Import]
		public abstract CGFloat PointSize { get; }

		/// <summary>
		/// The top y-coordinate, offset from the baseline, of the font’s longest ascender.
		/// 
		/// The value of this property is the distance of the longest ascender’s top y-coordinate from the baseline, measured in points.
		/// </summary>
		[Import]
		public abstract CGFloat Ascender { get; }

		/// <summary>
		/// The bottom y-coordinate, offset from the baseline, of the font’s longest descender.
		/// 
		/// For example, if the longest descender extends 2 points below the baseline, the value in this property is –2.
		/// </summary>
		[Import]
		public abstract CGFloat Descender { get; }

		/// <summary>
		/// The cap height of the font.
		/// </summary>
		[Import]
		public abstract CGFloat CapHeight { get; }

		/// <summary>
		/// The leading value of the font.
		/// </summary>
		[Import]
		public abstract CGFloat Leading { get; }

		/// <summary>
		/// The x-height of the font.
		/// </summary>
		[Import]
		public abstract CGFloat XHeight { get; }

		/// <summary>
		/// The number of degrees that the font is slanted counterclockwise from the vertical.
		/// 
		/// The italic angle value is read from the font’s AFM file. Because the slant is measured counterclockwise, English italic fonts typically return a negative value.
		/// </summary>
		[Import]
		public abstract CGFloat ItalicAngle { get; }

		/// <summary>
		/// The baseline offset to use when drawing underlines with the font.
		/// 
		/// The value in this property is determined by the font’s AFM file. The value is usually negative, which must be considered when drawing in a flipped coordinate system.
		/// </summary>
		[Import]
		public abstract CGFloat UnderlinePosition { get; }

		/// <summary>
		/// The thickness to use when drawing underlines with the font.
		/// 
		/// The value in this property is determined by the font’s AFM file.
		/// </summary>
		[Import]
		public abstract CGFloat UnderlineThickness { get; }

		/// <summary>
		/// The font’s bounding rectangle, scaled to the font’s size.
		/// 
		/// The bounding rectangle is the union of the bounding rectangles of every glyph in the font.
		/// </summary>
		[Import]
		public abstract CGRect BoundingRectForFont { get; }

		public new abstract class MetaClass : NSObject<NSFont, MetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}

			[Import]
			public abstract NSFont BoldSystemFont(CGFloat ofSize);

			[Import]
			public abstract NSFont ControlContentFont(CGFloat ofSize);

			[Import]
			public abstract NSFont LabelFont(CGFloat ofSize);

			[Import]
			public abstract NSFont MenuFont(CGFloat ofSize);

			[Import]
			public abstract NSFont MenuBarFont(CGFloat ofSize);

			[Import]
			public abstract NSFont MessageFont(CGFloat ofSize);

			[Import]
			public abstract NSFont MonospacedSystemFont(CGFloat ofSize, NSFontWeight weight);

			[Import]
			public abstract NSFont MonospacedDigitSystemFont(CGFloat ofSize, NSFontWeight weight);

			[Import]
			public abstract NSFont PaletteFont(CGFloat ofSize);

			[Import]
			public abstract NSFont SystemFont(CGFloat ofSize);

			[Import]
			public abstract NSFont SystemFont(CGFloat ofSize, NSFontWeight weight);

			[Import]
			public abstract NSFont TitleBarFont(CGFloat ofSize);

			[Import]
			public abstract NSFont ToolTipsFont(CGFloat ofSize);

			[Import]
			public abstract NSFont UserFont(CGFloat ofSize);

			[Import]
			public abstract NSFont UserFixedPitchFont(CGFloat ofSize);

			//[Method("systemFontSizeForControlSize:")]
			//public abstract CGFloat SystemFontSizeForControlSize(NSControlSize controlSize);

			[Import]
			public abstract CGFloat SystemFontSize { get; }

			[Import]
			public abstract CGFloat SmallSystemFontSize { get; }

			[Import]
			public abstract CGFloat LabelFontSize { get; }
		}
	}
}
