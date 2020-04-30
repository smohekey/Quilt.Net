namespace Quilt.Mac.Core.Graphics {
	using System;
  using System.Diagnostics.CodeAnalysis;
  using System.Reflection;
  using Quilt.Mac.CodeGen;
  using Sigil.NonGeneric;

	[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Only called by GenerationContext and guaranteed not to be null.")]
	public sealed class CGFloatMarshaler : TypeMarshaler {
		private const string INTPTR_SIZE_NOTSUPPORTED = "IntPtr sizes other than 4 and 8 are not supported.";

		public CGFloatMarshaler(GenerationContext context, ParameterInfo parameter) : base(context, parameter) {

		}

		public override string MsgSendName => "objc_msgSend_fpret";

		public override Type NativeReturnType {
			get {
				if (IntPtr.Size == 4) {
					return Types.Single;
				} else if (IntPtr.Size == 8) {
					return Types.Double;
				} else {
					throw new NotSupportedException(INTPTR_SIZE_NOTSUPPORTED);
				}
			}
		}

		public override void EmitMarshalReturnParameter(Emit emit) {
			if (IntPtr.Size == 4) {
				emit.NewObject<CGFloat, float>();
			} else if (IntPtr.Size == 8) {
				emit.NewObject<CGFloat, double>();
			} else {
				throw new NotSupportedException(INTPTR_SIZE_NOTSUPPORTED);
			}
		}
	}
}
