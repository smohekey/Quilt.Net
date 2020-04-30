namespace Quilt.Typography {
	using System.Collections;
	using System.Collections.Generic;

	public abstract class Glyph : IEnumerable<GlyphContour> {
		public abstract short XMin { get; }
		public abstract short XMax { get; }
		public abstract short YMin { get; }
		public abstract short YMax { get; }

		public ushort AdvanceWidth { get; }
		public short LeftSideBearing { get; }
		public short RightSideBearing => (short)(AdvanceWidth - YMax);

		protected Glyph(ushort advanceWidth, short leftSideBearing) {
			AdvanceWidth = advanceWidth;
			LeftSideBearing = leftSideBearing;
		}

		public abstract IEnumerator<GlyphContour> GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}
