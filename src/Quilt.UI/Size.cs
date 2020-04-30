namespace Quilt.UI {
	using System;
  using System.Diagnostics.CodeAnalysis;

  [SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "<Pending>")]
	public struct Size : IEquatable<Size> {
		public readonly float Width;
		public readonly float Height;

		public Size(float width, float height) {
			Width = width;
			Height = height;
		}

		public void Deconstruct(out float width, out float height) {
			width = Width;
			height = Height;
		}

		public override bool Equals(object obj) {
			return obj is Size size && Equals(size);
		}

		public bool Equals(Size other) {
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

		public static bool operator ==(Size left, Size right) {
			return left.Equals(right);
		}

		public static bool operator !=(Size left, Size right) {
			return !(left == right);
		}
	}
}
