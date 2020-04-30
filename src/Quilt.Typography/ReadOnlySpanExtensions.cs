using System;

namespace Quilt.Typography {
	public static class ReadOnlySpanExtensions {
		public static byte[] ReadBytes(this ReadOnlySpan<byte> @this, ref int offset, int length) {
			var array = @this.Slice(offset, length).ToArray();

			offset += length;

			return array;
		}

		public static short ToInt16(this ReadOnlySpan<byte> @this, int offset) {
			return ReadInt16(@this, ref offset);
		}

		public static short ReadInt16(this ReadOnlySpan<byte> @this, ref int offset) {
			short result = 0;

			result |= (short)(@this[offset++] << 8);
			result |= (short)@this[offset++];

			return result;
		}

		public static ushort ToUInt16(this ReadOnlySpan<byte> @this, int offset) {
			return ReadUInt16(@this, ref offset);
		}

		public static ushort ReadUInt16(this ReadOnlySpan<byte> @this, ref int offset) {
			ushort result = 0;

			result |= (ushort)(@this[offset++] << 8);
			result |= (ushort)@this[offset++];

			return result;
		}

		public static int ToInt32(this ReadOnlySpan<byte> @this, int offset) {
			return ReadInt32(@this, ref offset);
		}

		public static int ReadInt32(this ReadOnlySpan<byte> @this, ref int offset) {
			int result = 0;

			result |= @this[offset++] << 24;
			result |= @this[offset++] << 16;
			result |= @this[offset++] << 8;
			result |= @this[offset++];

			return result;
		}

		public static uint ToUInt32(this ReadOnlySpan<byte> @this, int offset) {
			return ReadUInt32(@this, ref offset);
		}

		public static uint ReadUInt32(this ReadOnlySpan<byte> @this, ref int offset) {
			uint result = 0;

			result |= (uint)@this[offset++] << 24;
			result |= (uint)@this[offset++] << 16;
			result |= (uint)@this[offset++] << 8;
			result |= @this[offset++];

			return result;
		}

		public static long ToInt64(this ReadOnlySpan<byte> @this, int offset) {
			return ReadInt64(@this, ref offset);
		}

		public static long ReadInt64(this ReadOnlySpan<byte> @this, ref int offset) {
			long result = 0;

			result |= (long)@this[offset++] << 56;
			result |= (long)@this[offset++] << 48;
			result |= (long)@this[offset++] << 40;
			result |= (long)@this[offset++] << 32;
			result |= (long)@this[offset++] << 24;
			result |= (long)@this[offset++] << 16;
			result |= (long)@this[offset++] << 8;
			result |= @this[offset++];

			return result;
		}

		public static ulong ToUInt64(this byte[] buffer, int offset) {
			return ReadUInt64(buffer, ref offset);
		}

		public static ulong ToUInt64(this ReadOnlySpan<byte> @this, int offset) {
			return ReadUInt64(@this, ref offset);
		}

		public static ulong ReadUInt64(this ReadOnlySpan<byte> @this, ref int offset) {
			ulong result = 0;

			result |= (ulong)@this[offset++] << 56;
			result |= (ulong)@this[offset++] << 48;
			result |= (ulong)@this[offset++] << 40;
			result |= (ulong)@this[offset++] << 32;
			result |= (ulong)@this[offset++] << 24;
			result |= (ulong)@this[offset++] << 16;
			result |= (ulong)@this[offset++] << 8;
			result |= @this[offset];

			return result;
		}
	}
}
