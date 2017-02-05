using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GuestServices
{
	static class ExternalUtil
	{
		[DllImport("Shell32.dll")]
		private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

		[DllImport("user32.dll")]
		public static extern int FindWindow(string lpClassName, string lpWindowName);
		[DllImport("user32.dll")]
		public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool PostMessage(int hWnd, uint Msg, int wParam, int lParam);

		public static void SetDesktopInvisible()
		{
			int hwnd;
			hwnd = FindWindow("Progman", null);
			PostMessage(hwnd, 0x12, 0, 0);
			return;
		}

		public static void RefreshDesktop()
		{
			SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero);
		}

		/*[DllImport("user32.dll")]
		private static extern int FindWindow(string className, string windowText);
		[DllImport("user32.dll")]
		private static extern int ShowWindow(int hwnd, int command);

		private const int SW_HIDE = 0;
		private const int SW_SHOW = 1;

		public static bool DesktopVisible
		{
			set
			{
				int hwnd = FindWindow("Shell_TrayWnd", "");
				ShowWindow(hwnd, value ? SW_SHOW : SW_HIDE);
			}
		}*/
	}
}
