namespace Quilt.Mac.ObjectiveC {
	using System;
  using System.Linq.Expressions;
  using System.Reflection;
  using System.Runtime.InteropServices;
  using System.Text;

  [StructLayout(LayoutKind.Sequential)]
	public struct Selector : IEquatable<Selector> {
		private readonly IntPtr _selector;

		public Selector(IntPtr selector) {
			_selector = selector;
		}


		[DllImport(Runtime.LIBRARY)]
		private static extern unsafe Selector sel_registerName(byte* name);

		public unsafe Selector(string name) {
			if (name is null) {
				throw new ArgumentNullException(nameof(name));
			}

			var nameLengthBytes = Encoding.UTF8.GetMaxByteCount(name.Length);

			byte* utf8NamePtr = stackalloc byte[nameLengthBytes];

			fixed (char* namePtr = name) {
				Encoding.UTF8.GetBytes(namePtr, name.Length, utf8NamePtr, nameLengthBytes);
			}

			_selector = sel_registerName(utf8NamePtr);
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern IntPtr sel_getName(Selector selector);

		public string Name {
			get {
				return Marshal.PtrToStringUTF8(sel_getName(_selector));
			}
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern bool sel_isEqual(Selector lhs, Selector rhs);

		public bool Equals(Selector other) {
			return sel_isEqual(this, other);
		}

		public static Selector From<T>(Expression<Action<T>> expression) {
			expression = expression ?? throw new ArgumentNullException(nameof(expression));

			return From(((MethodCallExpression)expression.Body).Method);
		}

		public static Selector From(MethodInfo method) {
			method = method ?? throw new ArgumentNullException(nameof(method));

			var builder = new StringBuilder();
			var methodName = method.Name;

			builder.Append(char.ToLowerInvariant(methodName[0]));
			builder.Append(methodName.Substring(1));

			bool first = true;

			foreach (var parameter in method.GetParameters()) {
				var parameterName = parameter.Name;

				var c = char.ToLowerInvariant(parameterName[0]);

				if (first) {
					first = false;

					c = char.ToUpperInvariant(parameterName[0]);
				}

				builder.Append(c);
				builder.Append(parameterName.Substring(1));

				builder.Append(':');
			}

			return builder.ToString();
		}

		public static implicit operator Selector(string name) => new Selector(name);
		public static implicit operator Selector(IntPtr selector) => new Selector(selector);
		public static implicit operator IntPtr(Selector selector) => selector._selector;

		public override bool Equals(object obj) {
			return obj is Selector selector ? Equals(selector) : false;
		}

		public override int GetHashCode() {
			return _selector.GetHashCode();
		}

		public override string ToString() {
			return Name;
		}

		public static bool operator ==(Selector left, Selector right) {
			return left.Equals(right);
		}

		public static bool operator !=(Selector left, Selector right) {
			return !(left == right);
		}
	}
}
