namespace Quilt.Mac.CodeGen {
  using System;
  using System.Reflection;

	public static class Fields {
		public static readonly FieldInfo IntPtr_Zero = Types.IntPtr.GetField(nameof(IntPtr.Zero));
	}
}
