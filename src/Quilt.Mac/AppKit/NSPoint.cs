namespace Quilt.Mac.AppKit {
  using System;
  using System.Diagnostics.CodeAnalysis;
  using System.Runtime.InteropServices;
  using Quilt.Mac.CodeGen;
  using Quilt.Mac.Core.Graphics;

  [StructLayout(LayoutKind.Sequential)]
	[MarshalWith(typeof(StructMarshaler))]
	[SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "<Pending>")]
	public struct NSPoint : IEquatable<NSPoint> {
		public readonly CGFloat X;
		public readonly CGFloat Y;

		public NSPoint(CGFloat x, CGFloat y) {
			X = x;
			Y = y;
		}

		public void Deconstruct(out CGFloat x, out CGFloat y) {
			x = X;
			y = Y;
		}

		public override bool Equals(object obj) {
			return obj is NSPoint other && Equals(other);
		}

		public bool Equals(NSPoint other) {
			return
				X == other.X &&
				Y == other.Y;
		}

		public override int GetHashCode() {
			var hash = 17;

			hash = hash * 23 + X.GetHashCode();
			hash = hash * 23 + Y.GetHashCode();

			return hash;
		}

		public static bool operator ==(NSPoint left, NSPoint right) {
			return left.Equals(right);
		}

		public static bool operator !=(NSPoint left, NSPoint right) {
			return !(left == right);
		}
	}
}
