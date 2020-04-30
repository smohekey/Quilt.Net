namespace Quilt.Abstractions {
	using System;

	[Flags]
	public enum SupportedPlatforms {
		Windows = (1 << 0),
		Linux = (1 << 1),
		OSX = (1 << 2),
		FreeBSD = (1 << 3),
		All = -1
	}
}
