namespace Quilt.Typography {
	using System.IO;

	public class BigEndianBinaryReader : BinaryReader {
		private readonly byte[] _buffer = new byte[8];

		public BigEndianBinaryReader(Stream input) : base(input) {
		}

		public override short ReadInt16() {
			if (2 != Read(_buffer, 0, 2)) {
				throw new EndOfStreamException();
			}

			return (short)(_buffer[0] << 8 | _buffer[1]);
		}

		public override ushort ReadUInt16() {
			if (2 != Read(_buffer, 0, 2)) {
				throw new EndOfStreamException();
			}

			return (ushort)(_buffer[0] << 8 | _buffer[1]);
		}

		public override int ReadInt32() {
			if (4 != Read(_buffer, 0, 4)) {
				throw new EndOfStreamException();
			}

			return _buffer[0] << 24 | _buffer[1] << 16 | _buffer[2] << 8 | _buffer[3];
		}

		public override uint ReadUInt32() {
			if (4 != Read(_buffer, 0, 4)) {
				throw new EndOfStreamException();
			}

			return (uint)(_buffer[0] << 24 | _buffer[1] << 16 | _buffer[2] << 8 | _buffer[3]);
		}
	}
}
