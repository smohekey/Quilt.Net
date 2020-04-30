namespace Quilt.Utilities {
	using System;
  using System.Text;

  public static class RandomUtils {
		private static readonly Random __random = new Random();

		public static unsafe string RandomHexString(int length) {
			var result = stackalloc byte[length];
			var buffer = stackalloc byte[length / 2];
			var span = new Span<byte>(buffer, length / 2);

			__random.NextBytes(span);

			for(int i = 0, j = 0; i < length / 2; i++) {
				byte high = (byte)((buffer[i] & 0xF0) >> 8);
				byte low = (byte)(buffer[i] & 0x0F);

				result[j++] = high < 10 ? (byte)('0' + high) : (byte)('A' + high - 10);
				result[j++] = low < 10 ? (byte)('0' + low) : (byte)('A' + low - 10);
			}

			return Encoding.UTF8.GetString(result, length);
		}
	}
}
