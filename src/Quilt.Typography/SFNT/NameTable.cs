namespace Quilt.Typography.SFNT {
	public class NameTable {
		public ushort Format { get; }
		public ushort Count { get; }
		public ushort StringOffset { get; }

		public NameRecord[] Names { get; }
	}
}
