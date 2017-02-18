using LegendaryUmbrella.ConsoleLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendaryUmbrella.LockFile
{
	class Program
	{
		static int Main(string[] args)
		{
			if (args.Length < 1)
			{
				Console.WriteLine("Bad syntax");
				return 1;
			}
			Options o = new Options(args);
			try
			{
				FileStream f = new FileStream(o.FileName, FileMode.Open, FileAccess.Read, o.SharingType);
			}
			catch (UnauthorizedAccessException)
			{
				Win32Exception ex = new Win32Exception(5); //Access is denied
				Console.Write(ex.Message);
				return ex.NativeErrorCode;
			}
			catch (IOException e)
			{
				Win32Exception ex = ConsoleUtilities.GetNativeException(e);
				if (ex != null)
				{
					Console.WriteLine(ex.Message);
					return ex.NativeErrorCode;
				}
			}
			Console.WriteLine("Opened handle to file. Press CTRL+C to exit...");
			while (true) System.Threading.Thread.Sleep(Int32.MaxValue);
		}
	}
}
