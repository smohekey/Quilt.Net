using System.Runtime.CompilerServices;
namespace Quilt {
	using System.Collections.Generic;
	using System.Linq;

	public static class DestructureExtensions {
		public static void Deconstruct<T>(this IEnumerable<T> @this, out T v0) {
			v0 = default!;

			var e = @this.GetEnumerator();

			if (!e.MoveNext()) {
				return;
			}

			v0 = e.Current;
		}

		public static void Deconstruct<T>(this IEnumerable<T> @this, out T v0, out T v1) {
			v0 = default!;
			v1 = default!;

			var e = @this.GetEnumerator();

			if (!e.MoveNext()) {
				return;
			}

			v0 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v1 = e.Current;
		}
	}
}
