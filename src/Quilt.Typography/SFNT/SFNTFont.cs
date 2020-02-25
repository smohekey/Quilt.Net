namespace Quilt.Typography.SFNT {
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.IO;

	public class SFNTFont : Font {
		private uint _version;
		private ushort _searchRange;
		private ushort _entrySelector;
		private ushort _rangeShift;
		private Dictionary<string, TableRecord> _tables;

		private SFNTFont(FileInfo file, uint version, ushort searchRange, ushort entrySelector, ushort rangeShift, Dictionary<string, TableRecord> tables) : base(file) {
			_version = version;
			_searchRange = searchRange;
			_entrySelector = entrySelector;
			_rangeShift = rangeShift;
			_tables = tables;
		}

		public static bool TryLoad(FileInfo file, BinaryReader reader, [NotNullWhen(true)] out Font? font) {
			var version = reader.ReadUInt32();

			switch (version) {
				case 0x00010000:
				case 0x74727565: {
					// truetype

					break;
				}

				case 0x74797031: {
					// postscript

					break;
				}

				case 0x4F54544F: {
					// OpenType Font

					break;
				}

				case 0x74746366: {
					// ttcf

					break;
				}

				default: {
					font = null;

					return false;
				}
			}

			var numTables = reader.ReadUInt16();
			var searchRange = reader.ReadUInt16();
			var entrySelector = reader.ReadUInt16();
			var rangeShift = reader.ReadUInt16();

			var tables = new Dictionary<string, TableRecord>();

			for (var i = 0; i < numTables; i++) {
				var tableRecord = new TableRecord(reader);

				tables.Add(tableRecord.Tag, tableRecord);
			}

			font = new SFNTFont(file, version, searchRange, entrySelector, rangeShift, tables);

			return true;
		}
	}
}
