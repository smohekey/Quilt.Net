namespace Quilt.Typography.SFNT {
  using System;
  using System.IO;

	public class HeaderTable : Table {
		public static readonly string TAG = "head";

		public uint MajorVersion { get; set; }
		public uint MinorVersion { get; set; }
		public Fixed Revision { get; set; }
		public uint CheckSumAdjustment { get; set; }
		public uint MagicNumber { get; set; }
		public ushort Flags { get; set; }
		public ushort UnitsPerEM { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }
		public short XMin { get; set; }
		public short YMin { get; set; }
		public short XMax { get; set; }
		public short YMax { get; set; }
		public MacStyle MacStyle { get; set; }
		public ushort LowestRecPPEM { get; set; }
		public ushort FontDirectionHint { get; set; }
		public short IndexToLocFormat { get; set; }
		public short GlyphDataFormat { get; set; }

		public HeaderTable(string tag, uint checkSum, uint offset, uint length) : base(tag, checkSum, offset, length) {

		}

		protected override void Load(SFNTFont font, ReadOnlySpan<byte> span) {
			var offset = 0;

			MajorVersion = span.ReadUInt16(ref offset);
			MinorVersion = span.ReadUInt16(ref offset);
			Revision = span.ReadFixed(ref offset);
			CheckSumAdjustment = span.ReadUInt32(ref offset);
			MagicNumber = span.ReadUInt32(ref offset);

			if(MagicNumber != 0x5f0f3cf5) {
				throw new InvalidDataException();
			}

			Flags = span.ReadUInt16(ref offset);
			UnitsPerEM = span.ReadUInt16(ref offset);
			Created = span.ReadLongDateTime(ref offset);
			Modified = span.ReadLongDateTime(ref offset);
			XMin = span.ReadInt16(ref offset);
			YMin = span.ReadInt16(ref offset);
			XMax = span.ReadInt16(ref offset);
			YMax = span.ReadInt16(ref offset);
			MacStyle = (MacStyle)span.ReadUInt16(ref offset);
			LowestRecPPEM = span.ReadUInt16(ref offset);
			FontDirectionHint = span.ReadUInt16(ref offset);
			IndexToLocFormat = span.ReadInt16(ref offset);
			GlyphDataFormat = span.ReadInt16(ref offset);
		}
	}
}
