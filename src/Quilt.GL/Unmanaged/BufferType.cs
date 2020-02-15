﻿namespace Quilt.GL.Unmanaged {
	public enum BufferType : int {
		Array = 0x8892,
		AtomicCounter = 0x92C0,
		CopyRead = 0x8F36,
		CopyWrite = 0x8F37,
		DispatchIndirect = 0x90EE,
		DrawIndirect = 0x8F3F,
		ElementArray = 0x8893,
		PixelPack = 0x88EB,
		PixelUnpack = 0x88EC,
		Query = 0x9192,
		ShaderStorage = 0x90D2,
		Texture = 0x8C2A,
		TransformFeedback = 0x8C8E,
		Uniform = 0x8A11
	}
}
