namespace Quilt.VG {
	using System;

	public static class Constants {
#if DEBUG
		public const int BUFFER_SIZE = 32;
#else
		public const int BUFFER_SIZE = 1024;
#endif

		public const float DEG_2_RAD = MathF.PI / 180;
		public const float RAD_2_DEG = 180 / MathF.PI;
	}
}
