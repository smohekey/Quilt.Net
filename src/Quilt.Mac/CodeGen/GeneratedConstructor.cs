namespace Quilt.Mac.CodeGen {
	using System;
  using System.Diagnostics.CodeAnalysis;
  using System.Reflection.Emit;

	[SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "<Pending>")]
	public struct GeneratedConstructor : IEquatable<GeneratedConstructor> {
		public readonly ConstructorBuilder Constructor;
		public readonly Type[] ParameterTypes;

		public GeneratedConstructor(ConstructorBuilder constructor, Type[] parameterTypes) {
			Constructor = constructor;
			ParameterTypes = parameterTypes;
		}

		public void Deconstruct(out ConstructorBuilder constructor, out Type[] parameterTypes) {
			constructor = Constructor;
			parameterTypes = ParameterTypes;
		}

		public override bool Equals(object obj) {
			return obj is GeneratedConstructor constructor && Equals(constructor);
		}

		public bool Equals(GeneratedConstructor other) {
			return Constructor == other.Constructor;
		}

		public override int GetHashCode() {
			return Constructor.GetHashCode();
		}

		public static bool operator ==(GeneratedConstructor left, GeneratedConstructor right) {
			return left.Equals(right);
		}

		public static bool operator !=(GeneratedConstructor left, GeneratedConstructor right) {
			return !(left == right);
		}
	}
}
