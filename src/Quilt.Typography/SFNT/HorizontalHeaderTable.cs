namespace Quilt.Typography.SFNT {
	using System;

	public class HorizontalHeaderTable : Table {
		public static readonly string TAG = "hhea";
		
		public ushort MajorVersion { get; set; }
		public ushort MinorVersion { get; set; }
		public short Ascender { get; set; }
		public short Descender { get; set; }
		public short LineGap { get; set; }
		public ushort AdvanceWidthMax { get; set; }
		public short MinLeftSideBearing { get; set; }
		public short MinRightSideBearing { get; set; }
		public short XMaxExtent { get; set; }
		public short CaretSlopeRise { get; set; }
		public short CaretSlopeRun { get; set; }
		public short CaretOffset { get; set; }
		public short MetricDataFormat { get; set; }
		public ushort NumberOfHMetrics { get; set; }

		public HorizontalHeaderTable(string tag, uint checkSum, uint offset, uint length) : base(tag, checkSum, offset, length) {

		}

		protected override void Load(SFNTFont font, ReadOnlySpan<byte> span) {
			var offset = 0;

			MajorVersion = span.ReadUInt16(ref offset);
			MinorVersion = span.ReadUInt16(ref offset);
			Ascender = span.ReadInt16(ref offset);
			Descender = span.ReadInt16(ref offset);
			LineGap = span.ReadInt16(ref offset);
			AdvanceWidthMax = span.ReadUInt16(ref offset);
			MinLeftSideBearing = span.ReadInt16(ref offset);
			MinRightSideBearing = span.ReadInt16(ref offset);
			XMaxExtent = span.ReadInt16(ref offset);
			CaretSlopeRise = span.ReadInt16(ref offset);
			CaretSlopeRun = span.ReadInt16(ref offset);
			CaretOffset = span.ReadInt16(ref offset);

			// skip over reserved fields
			offset += 8;

			MetricDataFormat = span.ReadInt16(ref offset);
			NumberOfHMetrics = span.ReadUInt16(ref offset);
		}
	}
}
