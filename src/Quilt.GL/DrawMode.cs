﻿namespace Quilt.GL {
	public enum DrawMode : uint {
		Points = 0x0000,
		Lines = 0x0001,
		LineLoop = 0x0002,
		LineStrip = 0x0003,
		Triangles = 0x0004,
		TriangleStrip = 0x0005,
		TriangleFan = 0x0006,
		LinesWithAdjacency = 0x000A,
		LineStripWithAdjacency = 0x000B,
		TrianglesAdjacency = 0x000C,
		TriangleStripWithAdjacency = 0x000D,
		Patches = 0x000E
	}
}
