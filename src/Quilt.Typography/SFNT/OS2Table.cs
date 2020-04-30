namespace Quilt.Typography.SFNT {
	using System;

	public class OS2Table : Table {
		public static readonly string TAG = "OS/2";

		public ushort Version { get; set; }
		public short XAvgCharWidth { get; set; }
		public ushort WeightClass { get; set; }
		public ushort WidthClass { get; set; }
		public ushort Type { get; set; }
		public short SubscriptXSize { get; set; }
		public short SubscriptYSize { get; set; }
		public short SubscriptXOffset { get; set; }
		public short SubscriptYOffset { get; set; }
		public short SuperscriptXSize { get; set; }
		public short SuperscriptYSize { get; set; }
		public short SuperscriptXOffset { get; set; }
		public short SuperscriptYOffset { get; set; }
		public short StrikeoutSize { get; set; }
		public short StrikeoutPosition { get; set; }
		public short FamilyClass { get; set; }
		public byte[] Panose { get; set; } = new byte[10];
		public byte[] UnicodeRange { get; set; } = new byte[16];
		public Tag VendorID { get; set; }
		public SelectionFlags Selection { get; set; }
		public ushort FirstCharIndex { get; set; }
		public ushort LastCharIndex { get; set; }
		public short TypoAscender { get; set; }
		public short TypoDescender { get; set; }
		public short TypoLineGap { get; set; }
		public ushort WinAscent { get; set; }
		public ushort WinDescent { get; set; }
		public byte[] CodePageRange { get; set; } = new byte[8];
		public short Height { get; set; }
		public short CapHeight { get; set; }
		public ushort DefaultChar { get; set; }
		public ushort BreakChar { get; set; }
		public ushort MaxContext { get; set; }
		public ushort LowerOpticalPointSize { get; set; }
		public ushort UpperOpticalPointSize { get; set; }

		public OS2Table(string tag, uint checkSum, uint offset, uint length) : base(tag, checkSum, offset, length) {

		}

		protected override void Load(SFNTFont font, ReadOnlySpan<byte> span) {
			var offset = 0;

			Version = span.ReadUInt16(ref offset);
			XAvgCharWidth = span.ReadInt16(ref offset);
			WeightClass = span.ReadUInt16(ref offset);
			WidthClass = span.ReadUInt16(ref offset);
			Type = span.ReadUInt16(ref offset);
			SubscriptXSize = span.ReadInt16(ref offset);
			SubscriptYSize = span.ReadInt16(ref offset);
			SubscriptXOffset = span.ReadInt16(ref offset);
			SubscriptYOffset = span.ReadInt16(ref offset);
			SuperscriptXSize = span.ReadInt16(ref offset);
			SuperscriptYSize = span.ReadInt16(ref offset);
			SuperscriptXOffset = span.ReadInt16(ref offset);
			SuperscriptYOffset = span.ReadInt16(ref offset);
			StrikeoutSize = span.ReadInt16(ref offset);
			StrikeoutPosition = span.ReadInt16(ref offset);
			FamilyClass = span.ReadInt16(ref offset);
			Panose = span.ReadBytes(ref offset, 10);
			UnicodeRange = span.ReadBytes(ref offset, 16);
			VendorID = span.ReadTag(ref offset);
			Selection = (SelectionFlags)span.ReadUInt16(ref offset);
			FirstCharIndex = span.ReadUInt16(ref offset);
			LastCharIndex = span.ReadUInt16(ref offset);
			TypoAscender = span.ReadInt16(ref offset);
			TypoDescender = span.ReadInt16(ref offset);
			TypoLineGap = span.ReadInt16(ref offset);
			WinAscent = span.ReadUInt16(ref offset);
			WinDescent = span.ReadUInt16(ref offset);

			if(Version == 0) {
				return;
			}

			CodePageRange = span.ReadBytes(ref offset, 8);

			if(Version == 1) {
				return;
			}

			Height = span.ReadInt16(ref offset);
			CapHeight = span.ReadInt16(ref offset);
			DefaultChar = span.ReadUInt16(ref offset);
			BreakChar = span.ReadUInt16(ref offset);
			MaxContext = span.ReadUInt16(ref offset);

			if(Version == 2 || Version == 3 || Version == 4) {
				return;
			}

			LowerOpticalPointSize = span.ReadUInt16(ref offset);
			UpperOpticalPointSize = span.ReadUInt16(ref offset);
		}
	}
}
