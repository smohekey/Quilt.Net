namespace Quilt.Typography.SFNT {
	using System;
	using System.Collections;
	using System.Collections.Generic;

  public class HorizontalMetricsTable : Table, IReadOnlyList<HorizontalMetric> {
		public static readonly string TAG = "hmtx";

		private HorizontalMetric[] _metrics = Array.Empty<HorizontalMetric>();

		public HorizontalMetricsTable(string tag, uint checkSum, uint offset, uint length) : base(tag, checkSum, offset, length) {

		}

		public HorizontalMetric this[int index] => _metrics[index];

		public int Count => _metrics.Length;

		protected override void Load(SFNTFont font, ReadOnlySpan<byte> span) {
			var maxp = font.GetTable<MaximumProfileTable>();
			var hhea = font.GetTable<HorizontalHeaderTable>();

			_metrics = new HorizontalMetric[maxp.NumGlyphs];

			var offset = 0;
			var advanceWidth = default(ushort);

			for(var i = 0; i < maxp.NumGlyphs; i++) {
				if(i < hhea.NumberOfHMetrics) {
					_metrics[i] = new HorizontalMetric {
						AdvanceWidth = advanceWidth = span.ReadUInt16(ref offset),
						LeftSideBearing = span.ReadInt16(ref offset)
					};
				} else {
					_metrics[i] = new HorizontalMetric {
						AdvanceWidth = advanceWidth,
						LeftSideBearing = span.ReadInt16(ref offset)
					};
				}
			}
		}

		public IEnumerator<HorizontalMetric> GetEnumerator() {
			return ((IReadOnlyList<HorizontalMetric>)_metrics).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}
