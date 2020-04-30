namespace Quilt.VG {
	using System;
	using System.IO;
	using System.Numerics;
	using System.Runtime.InteropServices;
	using Quilt.GL;
	using Quilt.Mac.AppKit;
	using Quilt.Typography;

	public class Surface {
		private static readonly Lazy<Font> __lazyDefaultFont;

		static Surface() {
			__lazyDefaultFont = new Lazy<Font>(LoadDefaultFont);
		}

		internal readonly GLContext _gl;
		internal readonly StrokeRenderer _strokeRenderer;
		internal readonly FillRenderer _fillRenderer;

		internal Matrix3x2 _transform;
		internal Font _font;
		internal float _fontSize;
		internal Color _strokeColor;
		internal float _strokeWidth;
		internal float _strokeMiterLimit;
		internal StrokeJoinStyle _strokeJoinStyle;
		internal StrokeCapStyle _strokeCapStyle;
		internal Color _fillColor;

		internal GlyphSet _glyphSet;

		public Surface(GLContext gl) {
			_gl = gl;

			_strokeRenderer = new StrokeRenderer(gl);
			_fillRenderer = new FillRenderer(gl);

			var (systemFont, systemFontSize) = GetSystemFontAndSize();

			_transform = Matrix3x2.Identity;
			_font = systemFont ?? __lazyDefaultFont.Value;
			_fontSize = systemFontSize ?? 12;
			_strokeColor = Color.Black;
			_strokeWidth = 1f;
			_strokeMiterLimit = 10f;
			_strokeJoinStyle = StrokeJoinStyle.Miter;
			_strokeCapStyle = StrokeCapStyle.Butt;
			_fillColor = Color.Black;

			_glyphSet = new GlyphSet(this, _font);
		}

		private static Font LoadDefaultFont() {
			var assembly = typeof(Surface).Assembly;

			return Font.Load(() => assembly.GetManifestResourceStream("Quilt.VG.Fonts.trim.ttf")!)!;
		}

		public static Font DefaultFont => __lazyDefaultFont.Value;

		private (Font?, float?) GetSystemFontAndSize() {
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {

			} else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
				var font = NSFont.SystemFont(0.0);
				var fontDescriptor = font.FontDescriptor;
				var url = fontDescriptor.FontUrl;
				var absoluteString = url.AbsoluteString;
				var path = url.Path;
				var boundingRect = font.BoundingRectForFont;
				var ascender = font.Ascender;
				var descender = font.Descender;
				var capHeight = font.CapHeight;
				var fixedPitch = font.FixedPitch;
				var italicAngle = font.ItalicAngle;
				var leading = font.Leading;
				var pointSize = font.PointSize;
				var underlinePosition = font.UnderlinePosition;
				var underlineThickness = font.UnderlineThickness;
				var xHeight = font.XHeight;

				return (Font.Load(new FileInfo(url.Path.ToString())), font.PointSize.ToSingle());
			}

			return (null, null);
		}

		public IPathBuilder CreatePath() {
			return new Path.Builder(this);
		}

		public Context Begin(int width, int height, float xScale, float yScale) {
			return new Context(
											this,
											Matrix4x4.CreateOrthographicOffCenter(0, width, height, 0, -1, 1),
											new Vector2(width, height),
											xScale,
											yScale
			);
		}
	}
}
