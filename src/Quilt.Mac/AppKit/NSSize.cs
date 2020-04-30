namespace Quilt.Mac.AppKit {
  using System;
  using System.Diagnostics.CodeAnalysis;
  using System.Runtime.InteropServices;
  using Quilt.Mac.CodeGen;
  using Quilt.Mac.Core.Graphics;

  [StructLayout(LayoutKind.Sequential)]
	[MarshalWith(typeof(StructMarshaler))]
	[SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "<Pending>")]
	public struct NSSize : IEquatable<NSSize> {
		public readonly CGFloat Width;
		public readonly CGFloat Height;

		public NSSize(CGFloat width, CGFloat height) {
			Width = width;
			Height = height;
		}

		public void Deconstruct(out CGFloat width, out CGFloat height) {
			width = Width;
			height = Height;
		}

		public override bool Equals(object obj) {
			return obj is NSSize other && Equals(other);
		}

		public bool Equals(NSSize other) {
			return
				Width == other.Width &&
				Height == other.Height;
		}

		public override int GetHashCode() {
			var hash = 17;

			hash = hash * 23 + Width.GetHashCode();
			hash = hash * 23 + Height.GetHashCode();

			return hash;
		}

		public static bool operator ==(NSSize left, NSSize right) {
			return left.Equals(right);
		}

		public static bool operator !=(NSSize left, NSSize right) {
			return !(left == right);
		}
	}
}
