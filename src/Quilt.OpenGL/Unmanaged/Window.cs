﻿namespace Quilt.OpenGL.Unmanaged {
  using System;
  using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct Window {
		IntPtr Handle;
	}
}