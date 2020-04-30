namespace Quilt.Mac.CodeGen {
  using System.Reflection;

	public sealed class StructMarshaler : TypeMarshaler {
		public StructMarshaler(GenerationContext context, ParameterInfo parameter) : base(context, parameter) {

		}

		public override string MsgSendName => "objc_msgSend_stret";
	}
}
