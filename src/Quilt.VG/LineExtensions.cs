namespace Quilt.VG {
	using System.Numerics;

	public static class LineExtensions {
		public static IFinishingPathBuilder LineTo(this IFinishingPathBuilder @this, float x, float y) {
			return @this.LineTo(new Vector2(x, y));
		}

		public static IFinishingPathBuilder LineTo(this IFinishingPathBuilder @this, Vector2 p) {
			return @this.AddPoint(p);
		}
	}
}
