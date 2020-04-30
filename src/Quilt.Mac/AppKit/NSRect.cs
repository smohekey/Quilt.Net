namespace Quilt.Mac.AppKit {
  using System;
  using System.Diagnostics.CodeAnalysis;
  using System.Runtime.InteropServices;
  using Quilt.Mac.CodeGen;
  using Quilt.Mac.Core.Graphics;

  [StructLayout(LayoutKind.Sequential)]
	[MarshalWith(typeof(StructMarshaler))]
	[SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "<Pending>")]
	public struct NSRect : IEquatable<NSRect> {
		
		public NSPoint Origin;
		public NSSize Size;

		public NSRect(NSPoint origin, NSSize size) {
			Origin = origin;
			Size = size;
		}

		public NSRect(CGFloat x, CGFloat y, CGFloat width, CGFloat height) {
			Origin = new NSPoint(x, y);
			Size = new NSSize(width, height);
		}

		public void Deconstruct(out NSPoint origin, out NSSize size) {
			origin = Origin;
			size = Size;
		}

		public void Deconstruct(out CGFloat x, out CGFloat y, out CGFloat width, out CGFloat height) {
			x = Origin.X;
			y = Origin.Y;
			width = Size.Width;
			height = Size.Height;
		}

		public override bool Equals(object obj) {
			return obj is NSRect other && Equals(other);
		}

		public bool Equals(NSRect other) {
			return
				Origin == other.Origin &&
				Size == other.Size;
		}

		public override int GetHashCode() {
			var hash = 17;

			hash = hash * 23 + Origin.GetHashCode();
			hash = hash * 23 + Size.GetHashCode();

			return hash;
		}

		public static bool operator ==(NSRect left, NSRect right) {
			return left.Equals(right);
		}

		public static bool operator !=(NSRect left, NSRect right) {
			return !(left == right);
		}
	}
}
