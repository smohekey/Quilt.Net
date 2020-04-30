namespace Quilt.Mac.Foundation {
	using System;
  using System.Diagnostics.CodeAnalysis;
  using System.Reflection;
	using Quilt.Mac.CodeGen;
	using Sigil.NonGeneric;

	[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Only called by GenerationContext and guaranteed not to be null.")]
	public sealed class NSObjectMarshaler : TypeMarshaler {
		public NSObjectMarshaler(GenerationContext context, ParameterInfo parameter) : base(context, parameter) {

		}

		public override Type NativeParameterType => Types.IntPtr;
		public override Type NativeReturnType => Types.IntPtr;

		public override void EmitMarshalParameterIn(Emit emit, ushort index) {
			var label1 = emit.DefineLabel();
			var label2 = emit.DefineLabel();

			emit.LoadArgument(index);
			emit.BranchIfFalse(label1);
			emit.LoadArgument(index);
			emit.CastClass(Types.NSObject);
			emit.Call(Methods.NSObject_GetHandle);
			emit.Branch(label2);
			emit.MarkLabel(label1);
			emit.LoadField(Fields.IntPtr_Zero);
			emit.MarkLabel(label2);
		}

		public override void EmitMarshalReturnParameter(Emit emit) {
			TypeMarshaler.EmitNativeToManaged(Context, emit, Parameter.ParameterType);
		}
	}
}
