namespace Quilt.Graphics.Metal {
	using System;
	using System.Runtime.InteropServices;
	using Quilt.Graphics;

	internal sealed class MetalBackend : Backend {
		public override bool IsSupported => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

		public override bool IsPreferred => true;

		public override Context? CreateContext(ContextOptions options, IntPtr window) {
			return null;
		}

		public override Context? CreateContext(ContextOptions options, Context shareContext, IntPtr window) {
			return null;
		}
	}
}
