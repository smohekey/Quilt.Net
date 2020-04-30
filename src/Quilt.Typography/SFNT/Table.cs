namespace Quilt.Typography.SFNT {
  using System;
  using System.IO;

	public abstract class Table {
		private bool _isLoaded;
		public string Tag { get; }
		public uint CheckSum { get; }
		public uint ByteOffset { get; }
		public uint ByteLength { get; }

		protected Table(string tag, uint checkSum, uint offset, uint length) {
			Tag = tag;
			CheckSum = checkSum;
			ByteOffset = offset;
			ByteLength = length;
		}

		public void Load(SFNTFont font) {
			using var stream = font.OpenStream();

			Load(font, stream);
		}

		public void Load(SFNTFont font, Stream stream) {
			if(_isLoaded) {
				return;
			}

			var old = stream.Seek(ByteOffset, SeekOrigin.Begin);

			var buffer = new byte[ByteLength];
			
			if(ByteLength != stream.Read(buffer, 0, (int)ByteLength)) {
				throw new EndOfStreamException();
			}

			Load(font, new ReadOnlySpan<byte>(buffer));

			stream.Seek(old, SeekOrigin.Begin);

			_isLoaded = true;
		}

		protected abstract void Load(SFNTFont font, ReadOnlySpan<byte> span);

		protected static BinaryReader CreateBinaryReader(Stream stream) {
			return BitConverter.IsLittleEndian ? new BEBinaryReader(stream) : new BinaryReader(stream);
		}
	}
}
