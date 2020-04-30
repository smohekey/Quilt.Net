namespace Quilt.UI.Mac {
	using System.Runtime.InteropServices;

	internal sealed class CocoaBackend : Backend {
		public override bool IsSupported => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

		public override Window CreateWindow(float left, float top, float width, float height, WindowStyle windowStyle) {
			return new CocoaWindow(left, top, width, height, windowStyle);
		}
	}
}
