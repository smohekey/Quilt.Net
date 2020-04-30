namespace Quilt.Abstractions {

	public interface IModule {
		string Id { get; }
		string Name { get; }

		//IEnumerable<IStitchPoint> GetStitchPoints();
	}
}
