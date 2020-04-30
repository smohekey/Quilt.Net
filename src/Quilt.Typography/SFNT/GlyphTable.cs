namespace Quilt.Typography.SFNT {
  using System;
  using System.Collections;
	using System.Collections.Generic;

	public class GlyphTable : Table, IEnumerable<Typography.Glyph> {
		public static readonly string TAG = "glyf";

		private Glyph[] _glyphs = Array.Empty<Glyph>();

		public GlyphTable(string tag, uint checkSum, uint offset, uint length) : base(tag, checkSum, offset, length) {

		}

		protected override void Load(SFNTFont font, ReadOnlySpan<byte> span) {
			var maxp = font.GetTable<MaximumProfileTable>();
			var loca = font.GetTable<LocationTable>();
			var hmtx = font.GetTable<HorizontalMetricsTable>();
		
			_glyphs = new Glyph[maxp.NumGlyphs];

			for(var i = loca.Length - 2; i >= 0; i--) {
				var offset = loca[i];
				var nextOffset = loca[i + 1];
				var hMetric = hmtx[i];

				if (offset != nextOffset) {
					_glyphs[i] = new Glyph(span.Slice((int)offset, (int)(nextOffset - offset)), hMetric.AdvanceWidth, hMetric.LeftSideBearing);
				} else {
					_glyphs[i] = new Glyph(hMetric.AdvanceWidth, hMetric.LeftSideBearing);
				}
			}
		}

		public Glyph GetGlyph(uint index) {
			return _glyphs[index];
		}

		public IEnumerator<Typography.Glyph> GetEnumerator() {
			return ((IEnumerable<Typography.Glyph>)_glyphs).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}
