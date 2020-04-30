namespace Quilt.UI {
	using System;
  using System.Diagnostics.CodeAnalysis;

  [SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "<Pending>")]
	public struct Point : IEquatable<Point> {	
		public readonly float X;
		public readonly float Y;

		public Point(float x, float y) {
			X = x;
			Y = y;
		}

		public override bool Equals(object obj) {
			return obj is Point other && Equals(other);
		}

		public bool Equals(Point other) {
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

		public static bool operator ==(Point left, Point right) {
			return left.Equals(right);
		}

		public static bool operator !=(Point left, Point right) {
			return !(left == right);
		}
	}
}
