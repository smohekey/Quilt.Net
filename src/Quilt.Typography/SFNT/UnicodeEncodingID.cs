namespace Quilt.Typography.SFNT {
	public enum UnicodeEncodingID : ushort {
		/// <summary>
		/// Unicode 1.0 semantics
		/// </summary>
		Unicode_1_0 = 0,
		/// <summary>
		/// Unicode 1.1 semantics
		/// </summary>
		Unicode_1_1 = 1,
		/// <summary>
		/// ISO/IEC 10646 semantics
		/// </summary>
		ISO_IEC_10646 = 2,
		/// <summary>
		/// Unicode 2.0 and onwards semantics, Unicode BMP only ('cmap' subtable formats 0, 4, 6).
		/// </summary>
		Unicode_2_0_BMP = 3,
		/// <summary>
		/// Unicode 2.0 and onwards semantics, Unicode full repertoire ('cmap' subtable formats 0, 4, 6, 10, 12).
		/// </summary>
		Unicode_2_0 = 4,
		/// <summary>
		/// Unicode Variation Sequences ('cmap' subtable format 14).
		/// </summary>
		Unicode_VariationSequences = 5,
		/// <summary>
		/// Unicode full repertoire ('cmap' subtable formats 0, 4, 6, 10, 12, 13).
		/// </summary>
		Unicode = 6
	}
}
