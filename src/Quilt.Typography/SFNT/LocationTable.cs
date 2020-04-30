namespace Quilt.Typography.SFNT {
	using System;
	
	public class LocationTable : Table {
		public static readonly string TAG = "loca";

		private uint[] _locations = Array.Empty<uint>();

		public LocationTable(string tag, uint checkSum, uint offset, uint length) : base(tag, checkSum, offset, length) {

		}

		protected override void Load(SFNTFont font, ReadOnlySpan<byte> span) {
			var head = font.GetTable<HeaderTable>();
			var maxp = font.GetTable<MaximumProfileTable>();

			var offset = 0;

			_locations = new uint[maxp.NumGlyphs];

			if (head.IndexToLocFormat == 0) {
				for (var i = 0; i < _locations.Length; i++) {
					_locations[i] = span.ReadUInt16(ref offset) * 2u;
				}
			} else {
				for (var i = 0; i < _locations.Length; i++) {
					_locations[i] = span.ReadUInt32(ref offset);
				}
			}
		}

		public int Length => _locations.Length;

		public uint this[int index] {
			get {
				return _locations[index];
			}
		}
	}
}
