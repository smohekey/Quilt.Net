namespace Quilt.Typography.SFNT {
	using System.IO;

	public class HeadTable {
		public uint MajorVersion { get; }
		public uint MinorVersion { get; }
		public Fixed Revision { get; }
		public uint CheckSumAdjustment { get; }
		public uint MagicNumber { get; }
		public HeadFlags Flags { get; }

		public HeadTable(BinaryReader reader) {

		}
	}
}
