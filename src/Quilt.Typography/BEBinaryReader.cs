namespace Quilt.Typography {
  using System.IO;

	public class BEBinaryReader : BinaryReader {
		private readonly byte[] _buffer = new byte[8];

		public BEBinaryReader(Stream input) : base(input) {

		}

		public BEBinaryReader(byte[] buffer, int offset, int length = -1) : base(new MemoryStream(buffer, offset, length == -1 ? buffer.Length : length)) {
			
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

			return (uint)_buffer[0] << 24 | (uint)_buffer[1] << 16 | (uint)_buffer[2] << 8 | _buffer[3];
		}

		public override long ReadInt64() {
			if (8 != Read(_buffer, 0, 8)) {
				throw new EndOfStreamException();
			}

			return (long)_buffer[0] << 56
				| (long)_buffer[1] << 48
				| (long)_buffer[2] << 40
				| (long)_buffer[3] << 32
				| (long)_buffer[4] << 24
				| (long)_buffer[5] << 16
				| (long)_buffer[6] << 8
				| _buffer[7]
			;
		}

		public override ulong ReadUInt64() {
			if (8 != Read(_buffer, 0, 8)) {
				throw new EndOfStreamException();
			}

			return (ulong)_buffer[0] << 56 
				| (ulong)_buffer[1] << 48 
				| (ulong)_buffer[2] << 40 
				| (ulong)_buffer[3] << 32 
				| (ulong)_buffer[4] << 24 
				| (ulong)_buffer[5] << 16 
				| (ulong)_buffer[6] << 8 
				| _buffer[7]
			;
		}
	}
}
