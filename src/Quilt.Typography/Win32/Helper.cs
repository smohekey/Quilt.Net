namespace Quilt.Typography.Win32 {
  using System;
  using System.IO;
  using System.Runtime.InteropServices;

	public class Helper {
		[DllImport("user32.dll")]
		private static extern bool SystemParametersInfo(SystemParametersInfoAction action, uint param1, ref NONCLIENTMETRICSA param2, uint fWinIni);

		[DllImport("gdi32.dll")]
		private static extern IntPtr CreateDC(string lpszDriver, string? lpszDevice, string? lpszOutput, IntPtr lpInitData);

		[DllImport("gdi32.dll")]
		private static extern IntPtr CreateFontIndirect(ref LOGFONTA lplf);

		[DllImport("gdi32.dll")]
		private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool DeleteObject(IntPtr hgdiobj);

		[DllImport("gdi32.dll")]
		private static extern uint GetFontData(IntPtr hdc, uint dwTable, uint dwOffset, byte[]? lpvBuffer, uint cbData);

		public static (Font?, int) GetSystemFont() {
			var ncm = new NONCLIENTMETRICSA {
				cbSize = Marshal.SizeOf<NONCLIENTMETRICSA>()
			};

			if(SystemParametersInfo(SystemParametersInfoAction.SPI_GETNONCLIENTMETRICS, (uint)ncm.cbSize, ref ncm, 0)) {
				var hdc = CreateDC("DISPLAY", null, null, IntPtr.Zero);

				var hfont = CreateFontIndirect(ref ncm.lfMessageFont);

				SelectObject(hdc, hfont);

				var length = GetFontData(hdc, 0, 0, null, 0);

				var buffer = new byte[length];

				if(length != GetFontData(hdc, 0, 0, buffer, length)) {
					return (null, 0);
				}

				return (Font.Load(buffer), -ncm.lfMessageFont.lfHeight);
			}

			return (null, 0);
		}

		public enum SystemParametersInfoAction : uint {
			SPI_GETNONCLIENTMETRICS = 0x0029
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct NONCLIENTMETRICSA {
			public int cbSize;
			public int iBorderWidth;
			public int iScrollWidth;
			public int iScrollHeight;
			public int iCaptionWidth;
			public int iCaptionHeight;
			public LOGFONTA lfCaptionFont;
			public int iSmCaptionWidth;
			public int iSmCaptionHeight;
			public LOGFONTA lfSmCaptionFont;
			public int iMenuWidth;
			public int iMenuHeight;
			public LOGFONTA lfMenuFont;
			public LOGFONTA lfStatusFont;
			public LOGFONTA lfMessageFont;
			public int iPaddedBorderWidth;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct LOGFONTA {
			public const int LF_FACESIZE = 32;

			public int lfHeight;
			public int lfWidth;
			public int lfEscapement;
			public int lfOrientation;
			public int lfWeight;
			public byte lfItalic;
			public byte lfUnderline;
			public byte lfStrikeOut;
			public byte lfCharSet;
			public byte lfOutPrecision;
			public byte lfClipPrecision;
			public byte lfQuality;
			public byte lfPitchAndFamily;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = LF_FACESIZE)]
			public string lfFaceName;
		}
	}
}
