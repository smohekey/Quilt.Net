namespace Quilt.Mac.AppKit {
	using System;

	public enum NSBackingStoreType : ulong {
		[Obsolete]
		Retained = 0,
		[Obsolete]
		Nonretained = 1,
		Buffered = 2
	}
}
