namespace Quilt.Typography {
	public struct GlyphPoint {
		public float X;
		public float Y;
		public bool OnCurve;

		public GlyphPoint(float x, float y, bool onCurve) {
			X = x;
			Y = y;
			OnCurve = onCurve;
		}
	}
}
