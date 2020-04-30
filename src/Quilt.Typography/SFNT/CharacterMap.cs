namespace Quilt.Typography.SFNT {
	using System;
	using System.Collections.Generic;
	using System.IO;

	public abstract class CharacterMap {
		public PlatformID PlatformID { get; }
		public ushort EncodingID { get; }
		public uint LanguageID { get; }

		protected CharacterMap(PlatformID platformID, ushort encodingID, uint languageID) {
			PlatformID = platformID;
			EncodingID = encodingID;
			LanguageID = languageID;
		}

		public static CharacterMap Load(ReadOnlySpan<byte> span, PlatformID platformID, ushort encodingID) {
			var offset = 0;
			var format = span.ReadUInt16(ref offset);

			var cmap = default(CharacterMap?);

			if (format < 8) {
				var length = span.ReadUInt16(ref offset);
				var languageID = span.ReadUInt16(ref offset);
				var subSpan = span.Slice(offset, length - offset);

				switch (format) {
					case 0: {
						cmap = new Format0(subSpan, platformID, encodingID, languageID);

						break;
					}

					case 2: {
						cmap = new Format2(subSpan, platformID, encodingID, languageID);

						break;
					}

					case 4: {
						cmap = new Format4(subSpan, platformID, encodingID, languageID);

						break;
					}

					case 6: {
						cmap = new Format6(subSpan, platformID, encodingID, languageID);

						break;
					}

					default: {
						throw new NotSupportedException();
					}
				}
			} else {
				offset += 2; // skip over minor version
				var length = span.ReadUInt32(ref offset);
				var languageID = span.ReadUInt32(ref offset);
				var subSpan = span.Slice(offset, (int)length - offset);

				switch (format) {
					case 8: {
						cmap = new Format8(subSpan, platformID, encodingID, languageID);

						break;
					}

					case 10: {
						cmap = new Format10(subSpan, platformID, encodingID, languageID);

						break;
					}

					case 12: {
						cmap = new Format12(subSpan, platformID, encodingID, languageID);

						break;
					}

					case 13: {
						cmap = new Format13(subSpan, platformID, encodingID, languageID);

						break;
					}

					case 14: {
						cmap = new Format14(subSpan, platformID, encodingID, languageID);

						break;
					}

					default: {
						throw new NotSupportedException();
					}
				}
			}

			return cmap;
		}

		public abstract uint GetGlyphIdForChar(char c);
		public abstract int Format { get; }

		private class Format0 : CharacterMap {
			private readonly byte[] _index;

			public override int Format => 0;

			public Format0(ReadOnlySpan<byte> span, PlatformID platformID, ushort encodingID, ushort languageID)
				: base(platformID, encodingID, languageID) {

				if(span.Length != 256) {
					throw new InvalidDataException();
				}

				_index = span.ToArray();
			}

			public override uint GetGlyphIdForChar(char c) {
				if (c < 256) {
					return _index[c];
				}

				return 0;
			}
		}

		private class Format2 : CharacterMap {
			public override int Format => 2;

			public Format2(ReadOnlySpan<byte> span, PlatformID platformID, ushort encodingID, ushort languageID)
				: base(platformID, encodingID, languageID) {

				// TODO: support format 2
			}

			public override uint GetGlyphIdForChar(char c) {
				return 0;
			}
		}

		private class Format4 : CharacterMap {
			private readonly Dictionary<ushort, ushort> _index = new Dictionary<ushort, ushort>();

			private readonly ushort _segCount;
			private readonly ushort _searchRange;
			private readonly ushort _entrySelector;
			private readonly ushort _shiftRange;

			private readonly Segment[] _segments = Array.Empty<Segment>();
			private readonly ushort[] _glyphIndexArray = Array.Empty<ushort>();

			public override int Format => 4;

			public Format4(ReadOnlySpan<byte> span, PlatformID platformID, ushort encodingID, ushort languageID)
				: base(platformID, encodingID, languageID) {

				var offset = 0;

				_segCount = (ushort)(span.ReadUInt16(ref offset) >> 1);
				_searchRange = span.ReadUInt16(ref offset);
				_entrySelector = span.ReadUInt16(ref offset);
				_shiftRange = span.ReadUInt16(ref offset);
				
				_segments = new Segment[_segCount];

				for (var i = 0; i < _segCount; i++) {
					_segments[i] = new Segment();
				}

				for(var i = 0; i < _segCount; i ++) {
					_segments[i].EndCode = span.ReadUInt16(ref offset);
				}

				offset += 2; // skip reservedPad

				for(var i =0; i< _segCount; i++) {
					_segments[i].StartCode = span.ReadUInt16(ref offset);
				}

				for(var i = 0; i < _segCount; i++) {
					_segments[i].Delta = span.ReadUInt16(ref offset);
				}

				for(var i = 0; i < _segCount; i++) {
					_segments[i].RangeOffset = span.ReadUInt16(ref offset);
				}

				var glyphIndexArrayLength = (span.Length - offset) / sizeof(ushort);

				_glyphIndexArray = new ushort[glyphIndexArrayLength];
				
				for(var i = 0; i < glyphIndexArrayLength; i++) {
					_glyphIndexArray[i] = span.ReadUInt16(ref offset);
				}
			}

			public override uint GetGlyphIdForChar(char c) {
				// TODO: make use of _searchRange, _entrySelector, and _shiftRange

				for (var i = 0; i < _segments.Length; i++) {
					ref var segment = ref _segments[i];

					if (segment.EndCode >= c) {
						if (segment.StartCode > c) {
							return 0;
						}

						if (segment.RangeOffset != 0) {
							var index = segment.RangeOffset + 2 * (c - segment.StartCode) + (_segCount - i * 8);

							return _glyphIndexArray[index];
						} else {
							return (ushort)(c + segment.Delta & 0xFFFF);
						}
					}
				}

				return 0;
			}

			private struct Segment {
				public ushort StartCode;
				public ushort EndCode;
				public ushort Delta;
				public ushort RangeOffset;
			}
		}

		private class Format6 : CharacterMap {
			private readonly ushort _firstCode;
			private readonly ushort[] _index;

			public override int Format => 6;

			public Format6(ReadOnlySpan<byte> span, PlatformID platformID, ushort encodingID, ushort languageID)
				: base(platformID, encodingID, languageID) {

				var offset = 0;

				_firstCode = span.ReadUInt16(ref offset);

				var entryCount = span.ReadUInt16(ref offset);

				_index = new ushort[entryCount];

				for (var i = 0; i < entryCount; i++) {
					_index[i] = span.ReadUInt16(ref offset);
				}
			}

			public override uint GetGlyphIdForChar(char c) {
				if (c >= _firstCode && c < _firstCode + _index.Length) {
					return _index[c - _firstCode];
				}

				return 0;
			}
		}

		private class Format8 : CharacterMap {
			private readonly byte[] _isU32;
			private readonly Group[] _groups;

			public override int Format => 8;

			public Format8(ReadOnlySpan<byte> span, PlatformID platformID, ushort encodingID, uint languageID)
				: base(platformID, encodingID, languageID) {

				_isU32 = span.Slice(0, 8192).ToArray();

				var offset = 8192;

				var groupCount = span.ReadUInt32(ref offset);

				_groups = new Group[groupCount];

				for (var i = 0; i < groupCount; i++) {
					_groups[i] = new Group {
						StartCharCode = span.ReadUInt32(ref offset),
						EndCharCode = span.ReadUInt32(ref offset),
						StartGlyphCode = span.ReadUInt32(ref offset)
					};
				}
			}

			public override uint GetGlyphIdForChar(char c) {
				/*if((_isU32[c / 8] & (1 << (c % 8))) != 0) {

				}*/
				// TODO: implement format 8

				throw new NotImplementedException();
			}

			private struct Group {
				public uint StartCharCode;
				public uint EndCharCode;
				public uint StartGlyphCode;
			}
		}

		private class Format10 : CharacterMap {
			private readonly uint _startCharCode;
			private readonly ushort[] _index;

			public override int Format => 10;

			public Format10(ReadOnlySpan<byte> span, PlatformID platformID, ushort encodingID, uint languageID)
				: base(platformID, encodingID, languageID) {

				var offset = 0;

				_startCharCode = span.ReadUInt32(ref offset);

				var glyphCount = span.ReadUInt32(ref offset);

				_index = new ushort[glyphCount];

				for (var i = 0; i < glyphCount; i++) {
					_index[i] = span.ReadUInt16(ref offset);
				}
			}

			public override uint GetGlyphIdForChar(char c) {
				throw new NotImplementedException();
			}
		}

		private class Format12 : CharacterMap {
			private readonly SequentialMapGroup[] _groups;

			public override int Format => 12;

			public Format12(ReadOnlySpan<byte> span, PlatformID platformID, ushort encodingID, uint languageID)
				: base(platformID, encodingID, languageID) {

				var offset = 0;
				var numGroups = span.ReadUInt32(ref offset);

				_groups = new SequentialMapGroup[numGroups];

				for(var i = 0; i < numGroups; i++) {
					_groups[i] = new SequentialMapGroup {
						StartCharCode = span.ReadUInt32(ref offset),
						EndCharCode = span.ReadUInt32(ref offset),
						StartGlyphID = span.ReadUInt32(ref offset)
					};
				}
			}

			public override uint GetGlyphIdForChar(char c) {
				for(var i = 0; i < _groups.Length; i++) {
					ref var group = ref _groups[i];

					if(group.StartCharCode <= c && c <= group.EndCharCode) {
						return group.StartGlyphID + (c - group.StartCharCode);
					}
				}

				return 0;
			}

			private struct SequentialMapGroup {
				public uint StartCharCode;
				public uint EndCharCode;
				public uint StartGlyphID;
			}
		}

		private class Format13 : CharacterMap {
			public override int Format => 13;

			public Format13(ReadOnlySpan<byte> span, PlatformID platformID, ushort encodingID, uint languageID)
				: base(platformID, encodingID, languageID) {

				// TODO: support format 13
			}

			public override uint GetGlyphIdForChar(char c) {
				throw new NotImplementedException();
			}
		}

		private class Format14 : CharacterMap {
			public override int Format => 14;

			public Format14(ReadOnlySpan<byte> span, PlatformID platformID, ushort encodingID, uint languageID)
				: base(platformID, encodingID, languageID) {

				// TODO: support format 14
			}

			public override uint GetGlyphIdForChar(char c) {
				throw new NotImplementedException();
			}
		}
	}
}
