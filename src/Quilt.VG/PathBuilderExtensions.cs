namespace Quilt.VG {
	using System.Numerics;

	public static class PathBuilderExtensions {
		public static IFinishingPathBuilder MoveTo(this IPathBuilder @this, float x, float y) {
			return @this.MoveTo(new Vector2(x, y));
		}
	}
}
