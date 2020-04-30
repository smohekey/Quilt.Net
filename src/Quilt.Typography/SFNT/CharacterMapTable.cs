namespace Quilt.Typography.SFNT {
	using System;
	using System.IO;
  using System.Linq;

  public class CharacterMapTable : Table {
		public static readonly string TAG = "cmap";

		public ushort Version { get; set; }

		private CharacterMap[] _records = Array.Empty<CharacterMap>();

		public CharacterMapTable(string tag, uint checkSum, uint offset, uint length) : base(tag, checkSum, offset, length) {

		}

		protected override void Load(SFNTFont font, ReadOnlySpan<byte> span) {
			var offset = 0;

			Version = span.ReadUInt16(ref offset);

			var recordCount = span.ReadUInt16(ref offset);

			_records = new CharacterMap[recordCount];

			for (var i = 0; i < recordCount; i++) {
				var platformID = (PlatformID)span.ReadUInt16(ref offset);
				var encodingID = span.ReadUInt16(ref offset);
				var subOffset = span.ReadUInt32(ref offset);

				_records[i] = CharacterMap.Load(span.Slice((int)subOffset), platformID, encodingID);
			}
		}

		public uint GetGlyphIndex(char codePoint) {
			foreach (var record in _records.Where(r => r.PlatformID != PlatformID.Unicode && r.Format != 4)) {
				var index = record.GetGlyphIdForChar(codePoint);

				if (index != 0) {
					return index;
				}
			}

			return 0;
		}
	}
}
