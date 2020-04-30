namespace Quilt.Typography.SFNT {
  using System;

	public class UnknownTable : Table {
		public static readonly string TAG = "   ";

		public UnknownTable(string tag, uint checkSum, uint offset, uint length) : base(tag, checkSum, offset, length) {

		}

		protected override void Load(SFNTFont font,ReadOnlySpan<byte> span) {
			
		}
	}
}
