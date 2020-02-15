namespace Quilt.GL.Unmanaged {
	public enum Error : int {
		None = 0,
		InvalidEnum = 0x0500,
		InvalidValue = 0x0501,
		InvalidOperatiorn = 0x0502,
		StackOverflow = 0x0503,
		StackUnderflow = 0x0504,
		OutOfMemory = 0x0505,
		InvalidFramebufferOperation = 0x0506,
	}
}
