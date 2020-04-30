namespace Quilt.Typography.SFNT {
	using System.Text;

	public struct Tag {
		public byte[] Value;

		public unsafe override string ToString() {
			fixed (byte* valuePtr = Value) {
				return Encoding.ASCII.GetString(valuePtr, 4);
			}
		}
	}
}
