namespace Quilt.Utilities {
	using System;

	public static class EnumExtensions {
		public static bool IsSet<T>(this T @this, T bit) where T : Enum {
			return !@this.Equals(0) && @this.HasFlag(bit);
		}
	}
}
