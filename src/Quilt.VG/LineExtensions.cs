namespace Quilt.VG {
  using System.Numerics;

  public static class LineExtensions {
		public static IPathBuilder LineTo(this IPathBuilder @this, float x, float y) {
			return @this.LineTo(new Vector2(x, y));
		}

		public static IPathBuilder LineTo(this IPathBuilder @this, Vector2 p) {
			return @this.SetPosition(p);
		}
	}
}
