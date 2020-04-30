namespace Quilt.Typography.SFNT {
  using System;
  using System.Collections.Generic;

	using NameIDType = System.ValueTuple<PlatformID, ushort, ushort, NameID>;

	public class NameTable : Table {
		public static readonly string TAG = "name";

		private Dictionary<NameIDType, Name> _records = new Dictionary<NameIDType, Name>();

		public NameTable(string tag, uint checkSum, uint offset, uint length) : base(tag, checkSum, offset, length) {
			
		}

		protected override void Load(SFNTFont font, ReadOnlySpan<byte> span) {
			var offset = 0;

			var format = span.ReadUInt16(ref offset);
			var count = span.ReadUInt16(ref offset);
			var stringOffset = span.ReadUInt16(ref offset);

			var rawValues = new Dictionary<uint, byte[]>();
			var values = new Dictionary<uint, string>();

			var stringSpan = span.Slice(stringOffset);

			for(var i = 0; i < count; i++) {
				var nameRecord = new Name();

				nameRecord.Load(span.Slice(6 + (i * 12)), stringSpan, rawValues, values);

				_records[(nameRecord.PlatformID, nameRecord.EncodingID, nameRecord.LanguageID, nameRecord.NameID)] = nameRecord;
			}

			if (format == 1) {
				// TODO: add format 1 support
			}
		}
	}
}
