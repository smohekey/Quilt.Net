namespace Quilt.Mac.CodeGen {
	using System;
  using System.Diagnostics.CodeAnalysis;
  using System.Reflection;
  using Sigil;
  using Sigil.NonGeneric;

  [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Only called by GenerationContext and guaranteed not to be null.")]
	public sealed class StringMarshaler : TypeMarshaler {
		private Local? _gch;
		private Local? _utf8;

		public StringMarshaler(GenerationContext context, ParameterInfo parameter) : base(context, parameter) {

		}

		public override Type NativeParameterType => Types.ByteArray;

		public override Type NativeReturnType => Types.BytePtr;

		public override void EmitParameterSetup(Emit emit, ushort index) {
			_gch = emit.DeclareLocal(Types.GCHandle);
			_utf8 = emit.DeclareLocal(Types.BytePtr);

			emit.Call(Methods.Encoding_UTF8);
			emit.LoadArgument(index);
			emit.Call(Methods.Encoding_GetBytes);
			emit.LoadConstant(3);
			emit.Call(Methods.GCHandle_Alloc);
			emit.StoreLocal(_gch);
			emit.LoadLocalAddress(_gch);
			emit.Call(Methods.GCHandle_AddrOfPinnedObject);
			emit.Call(Methods.IntPtr_ExplicitCast);
			emit.StoreLocal(_utf8);

		}

		public override void EmitMarshalParameterIn(Emit emit, ushort index) {
			emit.LoadLocal(_utf8);
		}

		public override void EmitMarshalParameterOutEpilogue(Emit emit, ushort index) {
			// TODO: emit out parameter marshaling
		}

		public override void EmitParameterCleanup(Emit emit, ushort index) {
			emit.LoadLocalAddress(_gch);
			emit.Call(Methods.GCHandle_Free);
		}

		public override void EmitMarshalReturnParameter(Emit emit) {
			using var utf8 = emit.DeclareLocal(Types.BytePtr);
			using var p = emit.DeclareLocal(Types.BytePtr);
			
			var loop = emit.DefineLabel();

			emit.Duplicate();
			emit.StoreLocal(utf8);
			emit.StoreLocal(p);
			emit.MarkLabel(loop);
			emit.LoadLocal(p);
			emit.Duplicate();
			emit.LoadConstant(1);
			emit.Add();
			emit.StoreLocal(p);
			emit.LoadIndirect(Types.UInt8);
			emit.BranchIfTrue(loop);
			emit.Call(Methods.Encoding_UTF8);
			emit.LoadLocal(utf8);
			emit.LoadLocal(p);
			emit.LoadLocal(utf8);
			emit.Subtract();
			emit.LoadConstant(1);
			emit.Subtract();
			emit.Convert(Types.Int32);
			emit.Call(Methods.Encoding_GetString);
		}
	}
}
