namespace Quilt.Mac.CodeGen {
  using System.Diagnostics.CodeAnalysis;
  using System.Reflection;
	using Sigil.NonGeneric;

	[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Only called by GenerationContext and guaranteed not to be null.")]
	public sealed class DefaultMarshaler : TypeMarshaler {
		public DefaultMarshaler(GenerationContext context, ParameterInfo parameter) : base(context, parameter) {

		}

		public override void EmitMarshalParameterIn(Emit emit, ushort index) {
			emit.LoadArgument(index);
		}
	}
}
