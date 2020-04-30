namespace Quilt.Mac.ObjectiveC {
	using System;

	public class MethodNotFoundException : Exception {
		public Class Class { get; }
		public Selector Selector { get; }

		public MethodNotFoundException(Class @class, Selector selector) {
			Class = @class;
			Selector = selector;
		}

		public override string Message => $"Method {Selector.Name} not found on class {Class.Name}";
	}
}
