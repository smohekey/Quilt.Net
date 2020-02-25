namespace Quilt.VG {
	using System.Collections.Generic;
	using Quilt.Collections;

	public interface IPath : IEnumerable<PathPoint>, IReverseEnumerable<PathPoint> {
		WindingOrder WindingOrder { get; }

		IPath Fill();
		IPath Stroke();

		IFrameBuilder Finish();
	}
}
