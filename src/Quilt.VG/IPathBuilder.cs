namespace Quilt.VG {
	using System.Numerics;

	public interface IPathBuilder : IBasePathBuilder<IPathBuilder> {
		IFinishingPathBuilder MoveTo(Vector2 position);
	}
}
