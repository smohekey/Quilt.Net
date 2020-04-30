namespace Quilt.UI {
  using System;
  using System.Diagnostics.CodeAnalysis;
  
	[SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "<Pending>")]
	public struct Rectangle : IEquatable<Rectangle> {
		public readonly float Left;
		public readonly float Top;
		public readonly float Width;
		public readonly float Height;

		public Rectangle(float left, float top, float width, float height) {
			Left = left;
			Top = top;
			Width = width;
			Height = height;
		}

		public void Deconstruct(out float left, out float top, out float width, out float height) {
			left = Left;
			top = Top;
			width = Width;
			height = Height;
		}

		public override bool Equals(object obj) {
			return obj is Rectangle other && Equals(other);
		}

		public bool Equals(Rectangle other) {
			return
				Left == other.Left &&
				Top == other.Top &&
				Width == other.Width &&
				Height == other.Height;
		}

		public override int GetHashCode() {
			var hash = 17;

			hash = hash * 23 + Left.GetHashCode();
			hash = hash * 23 + Top.GetHashCode();
			hash = hash * 23 + Width.GetHashCode();
			hash = hash * 23 + Height.GetHashCode();

			return hash;
		}

		public static bool operator ==(Rectangle left, Rectangle right) {
			return left.Equals(right);
		}

		public static bool operator !=(Rectangle left, Rectangle right) {
			return !(left == right);
		}
	}
}
