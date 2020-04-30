namespace Quilt {
  using System.Collections.Generic;
  using System.Runtime.InteropServices;

	public interface IModule {
		string Id { get; }
		string Name { get; }

		bool IsPlatformSupported(OSPlatform platform);

		//IEnumerable<IStitchPoint> GetStitchPoints();
	}
}
