namespace Quilt.GL {
	public enum StencilOperation : uint {
		Zero = 0,
		Invert = 0x150A,
		Keep = 0x1E00,
		Replace = 0x1E01,
		Increment = 0x1E02,
		Decrement = 0x1E03,
		IncrementWrap = 0x8507,
		DecrementWrap = 0x8508
	}
}
