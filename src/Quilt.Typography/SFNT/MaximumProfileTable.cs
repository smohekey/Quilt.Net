namespace Quilt.Typography.SFNT {
  using System;
  
	public class MaximumProfileTable : Table {
		public static readonly string TAG = "maxp";

		public ushort MajorVersion { get; set; }
		public ushort MinorVersion { get; set; }
		public ushort NumGlyphs { get; set; }
		public ushort MaxPoints { get; set; }
		public ushort MaxContours { get; set; }
		public ushort MaxCompositePoints { get; set; }
		public ushort MaxCompositeContours { get; set; }
		public ushort MaxZones { get; set; }
		public ushort MaxTwilightPoints { get; set; }
		public ushort MaxStorage { get; set; }
		public ushort MaxFunctionDefs { get; set; }
		public ushort MaxInstructionDefs { get; set; }
		public ushort MaxStackElements { get; set; }
		public ushort MaxSizeOfInstructions { get; set; }
		public ushort MaxComponentElements { get; set; }
		public ushort MaxComponentDepth { get; set; }

		public MaximumProfileTable(string tag, uint checkSum, uint offset, uint length) : base(tag, checkSum, offset, length) {

		}

		protected override void Load(SFNTFont font, ReadOnlySpan<byte> span) {
			var offset = 0;

			MajorVersion = span.ReadUInt16(ref offset);
			MinorVersion = span.ReadUInt16(ref offset);
			NumGlyphs = span.ReadUInt16(ref offset);

			if(MajorVersion == 1 && MinorVersion == 0) {
				MaxPoints = span.ReadUInt16(ref offset);
				MaxContours = span.ReadUInt16(ref offset);
				MaxCompositePoints = span.ReadUInt16(ref offset);
				MaxCompositeContours = span.ReadUInt16(ref offset);
				MaxZones = span.ReadUInt16(ref offset);
				MaxTwilightPoints = span.ReadUInt16(ref offset);
				MaxFunctionDefs = span.ReadUInt16(ref offset);
				MaxInstructionDefs = span.ReadUInt16(ref offset);
				MaxStackElements = span.ReadUInt16(ref offset);
				MaxSizeOfInstructions = span.ReadUInt16(ref offset);
				MaxComponentElements = span.ReadUInt16(ref offset);
				MaxComponentDepth = span.ReadUInt16(ref offset);
			}
		}
	}
}
