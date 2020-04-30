namespace Quilt.Mac.AppKit {
	using System;
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct NSFontWeight : IEquatable<NSFontWeight> {
		private readonly double _value;

		private NSFontWeight(double value) {
			_value = value;
		}

		public static readonly NSFontWeight UltraLight = new NSFontWeight(-0.8);
		public static readonly NSFontWeight Thin = new NSFontWeight(-0.6);
		public static readonly NSFontWeight Light = new NSFontWeight(-0.4);
		public static readonly NSFontWeight Regular = new NSFontWeight(0);
		public static readonly NSFontWeight Medium = new NSFontWeight(0.23);
		public static readonly NSFontWeight Semibold = new NSFontWeight(0.3);
		public static readonly NSFontWeight Bold = new NSFontWeight(0.4);
		public static readonly NSFontWeight Heavy = new NSFontWeight(0.56);
		public static readonly NSFontWeight Black = new NSFontWeight(0.62);

		public override bool Equals(object? obj) {
			return obj is NSFontWeight weight && Equals(weight);
		}

		public override int GetHashCode() {
			return _value.GetHashCode();
		}

		public static bool operator ==(NSFontWeight left, NSFontWeight right) {
			return left.Equals(right);
		}

		public static bool operator !=(NSFontWeight left, NSFontWeight right) {
			return !(left == right);
		}

		public bool Equals(NSFontWeight other) {
			return other._value == _value;
		}
	}
}
