namespace Quilt.VG {
	public interface IFinishingPathBuilder : IBasePathBuilder<IFinishingPathBuilder> {
		IFinishingPathBuilder Fill();
		IFinishingPathBuilder Stroke();

		IFrameBuilder Finish();
	}
}
