namespace Quilt.Typography {
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.IO;
	using Quilt.Typography.SFNT;

	public abstract class Font {
		protected readonly FileInfo _file;

		protected Font(FileInfo file) {
			_file = file;
		}

		internal static bool TryLoad(FileInfo file, [NotNullWhen(true)] out Font? font) {
			using var stream = file.OpenRead();
			using var reader = BitConverter.IsLittleEndian ? new BigEndianBinaryReader(stream) : new BinaryReader(stream);

			if (SFNTFont.TryLoad(file, reader, out font)) {
				return true;
			}

			font = null;

			return false;
		}
	}
}
