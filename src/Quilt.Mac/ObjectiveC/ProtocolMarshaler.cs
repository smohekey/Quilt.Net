namespace Quilt.Mac.ObjectiveC {
  using System;
  using System.Reflection;
	using Quilt.Mac.CodeGen;
  using Sigil.NonGeneric;

  public sealed class ProtocolMarshaler : TypeMarshaler {
		public ProtocolMarshaler(GenerationContext context, ParameterInfo parameter) : base(context, parameter) {

		}

		public override Type NativeParameterType => Types.IntPtr;

		public override void EmitMarshalParameterIn(Emit emit, ushort index) {
			emit.LoadArgument(index);
			emit.CastClass(Types.NSObject);
			emit.Call(Methods.NSObject_GetHandle);
		}
	}
}
