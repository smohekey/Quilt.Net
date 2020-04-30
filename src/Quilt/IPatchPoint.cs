namespace Quilt {
	using System.Collections.Generic;

	public interface IPatchPoint {

	}

	public interface IStitchPoint<TStitch> where TStitch : IStitch {
		IEnumerable<TStitch> GetStitches();
	}
}
