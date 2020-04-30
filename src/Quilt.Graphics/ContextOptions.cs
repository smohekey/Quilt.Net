namespace Quilt.Graphics {
	using System;
  using System.Diagnostics.CodeAnalysis;

  [SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "<Pending>")]
	public struct ContextOptions : IEquatable<ContextOptions> {
		public readonly PixelFormat PixelFormat;

		public ContextOptions(PixelFormat pixelFormat) {
			PixelFormat = pixelFormat;
		}

		public override bool Equals(object obj) {
			return obj is ContextOptions other && Equals(other);
		}

		public bool Equals(ContextOptions other) {
			return
				other.PixelFormat == PixelFormat;
		}

		public override int GetHashCode() {
			unchecked {
				var hash = 17;

				hash = hash * 23 + PixelFormat.GetHashCode();

				return hash;
			}
		}

		public static bool operator ==(ContextOptions left, ContextOptions right) {
			return left.Equals(right);
		}

		public static bool operator !=(ContextOptions left, ContextOptions right) {
			return !(left == right);
		}
	}
}
