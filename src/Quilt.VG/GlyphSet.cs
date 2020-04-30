namespace Quilt.VG {
  using System;
  using System.Collections.Generic;
  using Quilt.Typography;

	public class GlyphSet {
		private readonly Dictionary<uint, GlyphPath> _glyphPathsByIndex = new Dictionary<uint, GlyphPath>();
		private readonly Dictionary<char, GlyphPath> _glyphPathsByCodePoint = new Dictionary<char, GlyphPath>();

		private readonly Surface _vg;
		private readonly Font _font;

		public GlyphSet(Surface vg, Font font) {
			_vg = vg;
			_font = font;
		}

		public GlyphPath GetGlyphPath(char codePoint) {
			if(!_glyphPathsByCodePoint.TryGetValue(codePoint, out var glyphPath)) {
				var index = _font.GetGlyphIndex(codePoint);

				if (!_glyphPathsByIndex.TryGetValue(index, out glyphPath)) {
					var glyph = _font.GetGlyph(index);

					glyphPath = GeneratePathForGlyph(_vg, glyph);

					_glyphPathsByIndex[index] = glyphPath;
				}

				_glyphPathsByCodePoint[codePoint] = glyphPath;
			}

			return glyphPath;
		}

		private static GlyphPath GeneratePathForGlyph(Surface vg, Glyph glyph) {
			var builder = vg.CreatePath();

			foreach (var contour in glyph) {
				var prev = default(GlyphPoint?);
				var curr = contour[contour.Count - 1];
				var next = contour[0];

				if (curr.OnCurve) {
					builder.MoveTo(curr.X, curr.Y);
				} else {
					if (next.OnCurve) {
						builder.MoveTo(next.X, next.Y);
					} else {
						builder.MoveTo((curr.X + next.X) * 0.5f, (curr.Y + next.Y) * 0.5f);
					}
				}

				for (var i = 0; i < contour.Count; i++) {
					prev = curr;
					curr = next;
					next = contour[(i + 1) % contour.Count];

					if (curr.OnCurve) {
						builder.LineTo(curr.X, curr.Y);
					} else {
						var prev2 = prev;
						var next2 = next;

						if (!prev.Value.OnCurve) {
							prev2 = new GlyphPoint((curr.X + prev.Value.X) * 0.5f, (curr.Y + prev.Value.Y) * 0.5f, false);
						}

						if (!next.OnCurve) {
							next2 = new GlyphPoint((curr.X + next.X) * 0.5f, (curr.Y + next.Y) * 0.5f, false);
						}

						builder.BezierTo(curr.X, curr.Y, next2.X, next2.Y);
					}
				}

				builder.Close();
			}

			var path = builder.Build();

			return new GlyphPath(glyph, path);
		}
	}
}
