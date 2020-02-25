namespace Quilt.FreeType.Unmanaged {
	public class SizeMetrics {
		public ushort x_ppem;      /* horizontal pixels per EM               */
		public ushort y_ppem;      /* vertical pixels per EM                 */

		public long x_scale;     /* scaling values used to convert font    */
		public long y_scale;     /* units to 26.6 fractional pixels        */

		public long ascender;    /* ascender in 26.6 frac. pixels          */
		public long descender;   /* descender in 26.6 frac. pixels         */
		public long height;      /* text height in 26.6 frac. pixels       */
		public long max_advance; /* max horizontal advance, in 26.6 pixels */
	}
}
