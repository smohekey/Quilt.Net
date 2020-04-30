namespace Quilt.Typography.SFNT {
  using System;
  using System.Runtime.CompilerServices;

  public static class ReadOnlySpanExtensions {
		public unsafe static Tag ReadTag(this ReadOnlySpan<byte> @this, ref int offset) {
			return new Tag { Value = @this.ReadBytes(ref offset, 4) };
		}

		public static Fixed ReadFixed(this ReadOnlySpan<byte> @this, ref int offset) {
			return new Fixed {
				High = @this.ReadUInt16(ref offset),
				Low = @this.ReadUInt16(ref offset)
			};
		}

		public static DateTime ReadLongDateTime(this ReadOnlySpan<byte> @this, ref int offset) {
			var macTime = @this.ReadInt64(ref offset);

			return new DateTime(1904, 1, 1).AddSeconds(macTime);
		}

		public static unsafe T ReadStruct<T>(this ReadOnlySpan<byte> @this, ref int offset) where T : struct {
			var bytes = @this.ReadBytes(ref offset, Unsafe.SizeOf<T>());

			fixed (byte* bytesPtr = bytes) {
				return Unsafe.Read<T>(bytesPtr);
			}
		}
	}
}
