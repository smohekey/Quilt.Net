namespace Quilt.Mac.CodeGen {
	using System;

	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct)]
	public sealed class MarshalWithAttribute : Attribute {
		public Type Type { get; }

		public MarshalWithAttribute(Type type) {
			Type = type;
		}
	}
}
