namespace Quilt.Mac.CodeGen {
  using System;
  using System.Reflection;
  using Quilt.Mac.ObjectiveC;

  public struct ExportedMethod {
		public readonly MethodInfo BaseMethod;
		public readonly Type[] ParameterTypes;
		public readonly Selector Selector;

		public ExportedMethod(MethodInfo baseMethod, Type[] parameterTypes, Selector selector) {
			BaseMethod = baseMethod;
			ParameterTypes = parameterTypes;
			Selector = selector;
		}

		public void Deconstruct(out MethodInfo baseMethod, out Type[] parameterTypes, out Selector selector) {
			baseMethod = BaseMethod;
			parameterTypes = ParameterTypes;
			selector = Selector;
		}
	}
}
