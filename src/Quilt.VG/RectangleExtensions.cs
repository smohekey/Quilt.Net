using System.Runtime.CompilerServices;
namespace Quilt.VG {
	public static class RectangleExtensions {
		public static IFinishingPathBuilder RoundedRectangle(this IPathBuilder @this, float x, float y, float w, float h, float r) {
			return @this
				.MoveTo(x + r, y)
				.LineTo(x + w - r, y) // top
				.ArcTo(x + w, y + r, r, true)
				.LineTo(x + w, y + h - r) // right
				.ArcTo(x + w - r, y + h, r, true)
				.LineTo(x + r, y + h) // bottom
				.ArcTo(x, y + h - r, r, true)
				.LineTo(x, y + r) // left
				.ArcTo(x + r, y, r, true);
		}

		public static IFinishingPathBuilder Rectangle(this IPathBuilder @this, float x, float y, float w, float h) {
			return @this
				.MoveTo(x, y)
				.LineTo(x + w, y)
				.LineTo(x + w, y + h)
				.LineTo(x, y + h)
				.LineTo(x, y);
		}
	}
}
