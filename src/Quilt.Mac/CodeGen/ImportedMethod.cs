namespace Quilt.Mac.CodeGen {
  using System;
  using System.Diagnostics.CodeAnalysis;
  using System.Reflection;
  using System.Reflection.Emit;
  using Quilt.Mac.ObjectiveC;

  /// <summary>
  /// ImportedMethod is used to denote both imported methods and imported property getters/setters
  /// </summary>
  [SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "<Pending>")]
	public struct ImportedMethod : IEquatable<ImportedMethod> {
		public readonly MethodInfo BaseMethod;
		public readonly Type[] ParameterTypes;
		public readonly TypeMarshaler ReturnTypeMarshaler;
		public readonly TypeMarshaler[] ParameterTypeMarshalers;
		public readonly Selector Selector;
		public readonly int SelectorIndex;

		public ImportedMethod(MethodInfo baseMethod, Type[] parameterTypes, TypeMarshaler returnTypeMarshaler, TypeMarshaler[] parameterTypeMarshalers, Selector selector, int selectorIndex) {
			BaseMethod = baseMethod;
			ParameterTypes = parameterTypes;
			ReturnTypeMarshaler = returnTypeMarshaler;
			ParameterTypeMarshalers = parameterTypeMarshalers;
			Selector = selector;
			SelectorIndex = selectorIndex;
		}

		public void Deconstruct(out MethodInfo baseMethod, out Type[] parameterTypes, out TypeMarshaler returnTypeMarshaler, out TypeMarshaler[] parameterTypeMarshalers, out Selector selector, out int selectorIndex) {
			baseMethod = BaseMethod;
			parameterTypes = ParameterTypes;
			returnTypeMarshaler = ReturnTypeMarshaler;
			parameterTypeMarshalers = ParameterTypeMarshalers;
			selector = Selector;
			selectorIndex = SelectorIndex;
		}

		public override bool Equals(object obj) {
			return obj is ImportedMethod other && Equals(other);
		}

		public bool Equals(ImportedMethod other) {
			return BaseMethod == other.BaseMethod;
		}

		public override int GetHashCode() {
			return BaseMethod.GetHashCode();
		}

		public static bool operator ==(ImportedMethod left, ImportedMethod right) {
			return left.Equals(right);
		}

		public static bool operator !=(ImportedMethod left, ImportedMethod right) {
			return !(left == right);
		}
	}
}
