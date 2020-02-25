namespace Quilt.Typography.SFNT {
	using System.IO;
	using System.Text;

	public class TableRecord {
		public string Tag { get; }
		public uint CheckSum { get; }
		public uint Offset { get; }
		public uint Length { get; }

		internal TableRecord(BinaryReader reader) {
			var buffer = reader.ReadBytes(4);

			Tag = Encoding.ASCII.GetString(buffer);

			CheckSum = reader.ReadUInt32();
			Offset = reader.ReadUInt32();
			Length = reader.ReadUInt32();
		}
	}
}
