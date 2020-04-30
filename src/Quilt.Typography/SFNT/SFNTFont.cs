namespace Quilt.Typography.SFNT {
  using System;
  using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.IO;
  using System.Linq;
  using System.Reflection;
  using Quilt.Utilities;

  public class SFNTFont : Font {
		private const string TAG_NAME = nameof(HeaderTable.TAG);
		private static readonly Dictionary<Type, string> __tableTypeToTag = new Dictionary<Type, string>();
		private static readonly Dictionary<string, Type> __tableTagToType = new Dictionary<string, Type>();
		private static readonly Dictionary<string, ConstructorInfo> __tableConstructors = new Dictionary<string, ConstructorInfo>();

		private static readonly byte[] __0100Tag = new byte[] { 0x0, 0x1, 0x0, 0x0 };

		static SFNTFont() {
			var assembly = typeof(SFNTFont).Assembly;
			var tableType = typeof(Table);
			var stringType = typeof(string);
			var uintType = typeof(uint);
			var constructorTypes = new[] { stringType, uintType, uintType, uintType };
			var constructorTypeNames = string.Join(", ", constructorTypes.Select(t => t.Name));

			foreach(var type in assembly.GetTypes().Where(t => tableType.IsAssignableFrom(t) && t != tableType)) {
				var tagField = type.GetField(TAG_NAME, BindingFlags.Public | BindingFlags.Static);

				if (tagField == null || tagField.FieldType != stringType) {
					throw new InvalidOperationException($"Type {type.FullName} extends {tableType.FullName} without providing a static field of type string named {TAG_NAME}.");
				}


				if (!(tagField.GetValue(null) is string tag)) {
					throw new InvalidOperationException($"Type {type.FullName} extends {tableType.FullName} with a null value for the static field named {TAG_NAME}.");
				}

				if(!(type.GetConstructor(constructorTypes) is var constructor)) {
					throw new InvalidOperationException($"Type {type.FullName} extends {tableType.FullName} but doesn't implement a public constructor taking ({constructorTypeNames}).");
				}

				__tableTagToType[tag] = type;
				__tableTypeToTag[type] = tag;
				__tableConstructors[tag] = constructor;
			}
		}

		private readonly Tag _version;
		private readonly OutlineFormat _outlineFormat;
		private readonly ushort _searchRange;
		private readonly ushort _entrySelector;
		private readonly ushort _rangeShift;
		private readonly Dictionary<string, Table> _tables;

		private readonly HeaderTable _head;
		private readonly MaximumProfileTable _maxp;
		private readonly NameTable _name;
		private readonly GlyphTable _glyf;
		private readonly CharacterMapTable _cmap;
		private readonly HorizontalHeaderTable _hhea;
		private readonly OS2Table _os2;

		private SFNTFont(Func<Stream> openStream, Tag version, OutlineFormat outlineFormat, ushort searchRange, ushort entrySelector, ushort rangeShift, Dictionary<string, Table> tables) : base(openStream) {
			_version = version;
			_outlineFormat = outlineFormat;
			_searchRange = searchRange;
			_entrySelector = entrySelector;
			_rangeShift = rangeShift;
			_tables = tables;

			_head = GetTable<HeaderTable>();
			_maxp = GetTable<MaximumProfileTable>();
			_name = GetTable<NameTable>();
			_glyf = GetTable<GlyphTable>();
			_cmap = GetTable<CharacterMapTable>();
			_hhea = GetTable<HorizontalHeaderTable>();
			_os2 = GetTable<OS2Table>();
		}

		public override int UnitsPerEM => _head.UnitsPerEM;
		public override int Ascender => _os2.Selection.IsSet(SelectionFlags.UseTypoMetrics) ? _os2.TypoAscender : (int)_os2.WinAscent;
		public override int Descender => _os2.Selection.IsSet(SelectionFlags.UseTypoMetrics) ? _os2.TypoDescender : -_os2.WinDescent;

		public new static Font? Load(Func<Stream> openStream) {
			using var stream = openStream();
			var offset = 0;
			var buffer = new byte[12];

			if(buffer.Length != stream.Read(buffer, 0, buffer.Length)) {
				return null;
			}

			var span = new ReadOnlySpan<byte>(buffer);

			var version = span.ReadTag(ref offset);
			var versionString = version.ToString();
			var outlineFormat = OutlineFormat.TrueType;
			var numTables = 0u;

			if (version.Value.SequenceEqual(__0100Tag) || versionString == "true" || versionString == "typ1") {
				outlineFormat = OutlineFormat.TrueType;

				numTables = span.ReadUInt16(ref offset);

			} else if(versionString == "OTTO") {
				outlineFormat = OutlineFormat.CFF;

				numTables = span.ReadUInt32(ref offset);
			} /*else if(versionString == "wOFF") {
				var embeddedType = span.ReadTag(ref offset);

				if(embeddedType.Value.SequenceEqual(__0100Tag)) {
					outlineFormat = OutlineFormat.TrueType;
				} else if(embeddedType.ToString() == "OTTO") {
					outlineFormat = OutlineFormat.CFF;
				} else {
					font = null;

					return false;
				}
			} */else {
				return null;
			}

			var searchRange = span.ReadUInt16(ref offset);
			var entrySelector = span.ReadUInt16(ref offset);
			var rangeShift = span.ReadUInt16(ref offset);

			var tables = new Dictionary<string, Table>();

			for (var i = 0; i < numTables; i++) {
				buffer = new byte[16];

				if(buffer.Length != stream.Read(buffer, 0, buffer.Length)) {
					return null;
				}

				var table = ReadTable(new ReadOnlySpan<byte>(buffer));

				if (table != null) {
					tables.Add(table.Tag, table);
				}
			}

			return new SFNTFont(openStream, version, outlineFormat, searchRange, entrySelector, rangeShift, tables);
		}

		public T GetTable<T>() where T : Table {
			var type = typeof(T);

			return (T)GetTable(type);
		}

		public Table GetTable(Type type) {
			var table = _tables[__tableTypeToTag[type]];

			table.Load(this);

			return table;
		}

		private static Table ReadTable(ReadOnlySpan<byte> span) {
			var offset = 0;

			var tag = span.ReadTag(ref offset).ToString();
			var checkSum = span.ReadUInt32(ref offset);
			var tableOffset = span.ReadUInt32(ref offset);
			var length = span.ReadUInt32(ref offset);

			if(!__tableConstructors.TryGetValue(tag, out var constructor)) {
				return new UnknownTable(tag, checkSum, tableOffset, length);
			}

			return (Table)constructor.Invoke(new object[] { tag, checkSum, tableOffset, length });
		}

		private static uint CalculateTableCheckSum(BinaryReader reader, Table table) {
			var old = reader.BaseStream.Seek(table.ByteOffset, SeekOrigin.Begin);
			var sum = 0u;
			var nlongs = ((table.ByteLength + 3) / 4);

			while (nlongs-- > 0) {
				sum = (sum + reader.ReadUInt32() & 0xffffffff) >> 0;
			}

			reader.BaseStream.Seek(old, SeekOrigin.Begin);

			return sum;
		}

		public override uint GetGlyphIndex(char codePoint) {
			return _cmap.GetGlyphIndex(codePoint);
		}

		public override Typography.Glyph GetGlyph(uint index) {
			return _glyf.GetGlyph(index);
		}

		public override IEnumerator<Typography.Glyph> GetEnumerator() {
			return _glyf.GetEnumerator();
		}
	}
}
