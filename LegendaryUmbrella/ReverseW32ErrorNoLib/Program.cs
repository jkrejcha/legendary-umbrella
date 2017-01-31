using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendaryUmbrella.NoLib.ReverseW32Error
{
	class Program
	{
		static int Main(string[] args)
		{
			Console.CursorVisible = false;
			for (int i = 0; i < Console.BufferWidth - 2; i++)
			{
				Console.Write(" ");
			}
			if (args.Length == 0)
			{
				Console.WriteLine("Usage: " + Environment.GetCommandLineArgs()[0] + " <error string>");
				return 1;
			}
			String errorName = String.Join(" ", args);
			if (errorName.LastIndexOf(".") == errorName.Length - 1) errorName = errorName.Substring(0, errorName.Length - 1);
			for (int i = 0; i < Int32.MaxValue; i++)
			{
				int l = i.ToString().Length;
				Console.SetCursorPosition(Console.CursorLeft - l, Console.CursorTop);
				Console.Write(i);
				Win32Exception e = new Win32Exception(i);
				if (e.Message.ToLower().Equals(errorName.ToLower()))
				{
					Console.WriteLine(e.NativeErrorCode);
					return 0;
				}
			}
			Console.WriteLine("No error code found");
			return 1;
		}
	}
}
