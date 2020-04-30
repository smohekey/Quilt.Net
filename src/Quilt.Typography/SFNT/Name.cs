namespace Quilt.Typography.SFNT {
  using System;
  using System.Collections.Generic;
  using System.Diagnostics.CodeAnalysis;
  using System.Text;

  public struct Name {
		public PlatformID PlatformID;
		public ushort EncodingID;
		public ushort LanguageID;
		public NameID NameID;
		public byte[] RawValue;

		public string Value;

		public void Load(ReadOnlySpan<byte> span, ReadOnlySpan<byte> stringSpan, Dictionary<uint, byte[]> rawValues, Dictionary<uint, string> values) {
			var offset = 0;

			PlatformID = (PlatformID)span.ReadUInt16(ref offset);
			EncodingID = span.ReadUInt16(ref offset);
			LanguageID = span.ReadUInt16(ref offset);
			NameID = (NameID)span.ReadUInt16(ref offset);
			var valueLength = span.ReadUInt16(ref offset);
			var valueOffset = span.ReadUInt16(ref offset);

			if(values.TryGetValue(valueOffset, out var value)) {
				Value = value;
				RawValue = rawValues[valueOffset];
			} else {
				rawValues[valueOffset] = RawValue = stringSpan.Slice(valueOffset, valueLength).ToArray();
				
				if(TryGetDecoder(PlatformID, EncodingID, LanguageID, out var decode)) {
					values[valueOffset] = Value = decode(RawValue);
				}
			}
		}

		private static bool TryGetDecoder(PlatformID platformID, ushort encodingID, ushort languageID, [NotNullWhen(true)] out Func<byte[], string>? decode) {
			switch (platformID) {
				case PlatformID.Unicode: {
					decode = DecodeUTF16;

					return true;
				}

				case PlatformID.Microsoft: {
					if(encodingID == 1 || encodingID == 10) {
						decode = DecodeUTF16;

						return true;
					}

					break;
				}

				case PlatformID.Macintosh: {

					break;
				}
			}

			decode = null;

			return false;
		}

		private static string DecodeUTF16(byte[] buffer) {
			return Encoding.BigEndianUnicode.GetString(buffer);
		}
	}
}
