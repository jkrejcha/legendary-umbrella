using System;
using System.Collections.Generic;
using System.Text;

namespace LegendaryUmbrella.NoLib.Crash
{
	class Program
	{
		static void Main()
		{
			System.Runtime.InteropServices.Marshal.WriteByte(IntPtr.Zero, 0xFF);
		}
	}
}
