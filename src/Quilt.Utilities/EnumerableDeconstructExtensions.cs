
namespace Quilt.Utilities {
	using System.Collections.Generic;

	public static class EnumerableDeconstructExtensions {
	
		public static void Deconstruct<T>(this IEnumerable<T> @this, out T? v0) where T : struct {
			v0 = default;
			
			var e = @this.GetEnumerator();

			if (!e.MoveNext()) {
				return;
			}

			v0 = e.Current;
		}
		
		public static void Deconstruct<T>(this IEnumerable<T> @this, out T? v0) where T : class {
			v0 = default;
			
			var e = @this.GetEnumerator();

			if (!e.MoveNext()) {
				return;
			}

			v0 = e.Current;
		}
		public static void Deconstruct<T>(this IEnumerable<T> @this, out T? v0, out T? v1) where T : struct {
			v0 = default;
			v1 = default;
			
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
		
		public static void Deconstruct<T>(this IEnumerable<T> @this, out T? v0, out T? v1) where T : class {
			v0 = default;
			v1 = default;
			
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
		public static void Deconstruct<T>(this IEnumerable<T> @this, out T? v0, out T? v1, out T? v2) where T : struct {
			v0 = default;
			v1 = default;
			v2 = default;
			
			var e = @this.GetEnumerator();

			if (!e.MoveNext()) {
				return;
			}

			v0 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v1 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v2 = e.Current;
		}
		
		public static void Deconstruct<T>(this IEnumerable<T> @this, out T? v0, out T? v1, out T? v2) where T : class {
			v0 = default;
			v1 = default;
			v2 = default;
			
			var e = @this.GetEnumerator();

			if (!e.MoveNext()) {
				return;
			}

			v0 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v1 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v2 = e.Current;
		}
		public static void Deconstruct<T>(this IEnumerable<T> @this, out T? v0, out T? v1, out T? v2, out T? v3) where T : struct {
			v0 = default;
			v1 = default;
			v2 = default;
			v3 = default;
			
			var e = @this.GetEnumerator();

			if (!e.MoveNext()) {
				return;
			}

			v0 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v1 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v2 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v3 = e.Current;
		}
		
		public static void Deconstruct<T>(this IEnumerable<T> @this, out T? v0, out T? v1, out T? v2, out T? v3) where T : class {
			v0 = default;
			v1 = default;
			v2 = default;
			v3 = default;
			
			var e = @this.GetEnumerator();

			if (!e.MoveNext()) {
				return;
			}

			v0 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v1 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v2 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v3 = e.Current;
		}
		public static void Deconstruct<T>(this IEnumerable<T> @this, out T? v0, out T? v1, out T? v2, out T? v3, out T? v4) where T : struct {
			v0 = default;
			v1 = default;
			v2 = default;
			v3 = default;
			v4 = default;
			
			var e = @this.GetEnumerator();

			if (!e.MoveNext()) {
				return;
			}

			v0 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v1 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v2 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v3 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v4 = e.Current;
		}
		
		public static void Deconstruct<T>(this IEnumerable<T> @this, out T? v0, out T? v1, out T? v2, out T? v3, out T? v4) where T : class {
			v0 = default;
			v1 = default;
			v2 = default;
			v3 = default;
			v4 = default;
			
			var e = @this.GetEnumerator();

			if (!e.MoveNext()) {
				return;
			}

			v0 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v1 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v2 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v3 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v4 = e.Current;
		}
		public static void Deconstruct<T>(this IEnumerable<T> @this, out T? v0, out T? v1, out T? v2, out T? v3, out T? v4, out T? v5) where T : struct {
			v0 = default;
			v1 = default;
			v2 = default;
			v3 = default;
			v4 = default;
			v5 = default;
			
			var e = @this.GetEnumerator();

			if (!e.MoveNext()) {
				return;
			}

			v0 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v1 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v2 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v3 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v4 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v5 = e.Current;
		}
		
		public static void Deconstruct<T>(this IEnumerable<T> @this, out T? v0, out T? v1, out T? v2, out T? v3, out T? v4, out T? v5) where T : class {
			v0 = default;
			v1 = default;
			v2 = default;
			v3 = default;
			v4 = default;
			v5 = default;
			
			var e = @this.GetEnumerator();

			if (!e.MoveNext()) {
				return;
			}

			v0 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v1 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v2 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v3 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v4 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v5 = e.Current;
		}
		public static void Deconstruct<T>(this IEnumerable<T> @this, out T? v0, out T? v1, out T? v2, out T? v3, out T? v4, out T? v5, out T? v6) where T : struct {
			v0 = default;
			v1 = default;
			v2 = default;
			v3 = default;
			v4 = default;
			v5 = default;
			v6 = default;
			
			var e = @this.GetEnumerator();

			if (!e.MoveNext()) {
				return;
			}

			v0 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v1 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v2 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v3 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v4 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v5 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v6 = e.Current;
		}
		
		public static void Deconstruct<T>(this IEnumerable<T> @this, out T? v0, out T? v1, out T? v2, out T? v3, out T? v4, out T? v5, out T? v6) where T : class {
			v0 = default;
			v1 = default;
			v2 = default;
			v3 = default;
			v4 = default;
			v5 = default;
			v6 = default;
			
			var e = @this.GetEnumerator();

			if (!e.MoveNext()) {
				return;
			}

			v0 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v1 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v2 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v3 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v4 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v5 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v6 = e.Current;
		}
		public static void Deconstruct<T>(this IEnumerable<T> @this, out T? v0, out T? v1, out T? v2, out T? v3, out T? v4, out T? v5, out T? v6, out T? v7) where T : struct {
			v0 = default;
			v1 = default;
			v2 = default;
			v3 = default;
			v4 = default;
			v5 = default;
			v6 = default;
			v7 = default;
			
			var e = @this.GetEnumerator();

			if (!e.MoveNext()) {
				return;
			}

			v0 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v1 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v2 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v3 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v4 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v5 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v6 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v7 = e.Current;
		}
		
		public static void Deconstruct<T>(this IEnumerable<T> @this, out T? v0, out T? v1, out T? v2, out T? v3, out T? v4, out T? v5, out T? v6, out T? v7) where T : class {
			v0 = default;
			v1 = default;
			v2 = default;
			v3 = default;
			v4 = default;
			v5 = default;
			v6 = default;
			v7 = default;
			
			var e = @this.GetEnumerator();

			if (!e.MoveNext()) {
				return;
			}

			v0 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v1 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v2 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v3 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v4 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v5 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v6 = e.Current;

			if (!e.MoveNext()) {
				return;
			}

			v7 = e.Current;
		}
	}
}