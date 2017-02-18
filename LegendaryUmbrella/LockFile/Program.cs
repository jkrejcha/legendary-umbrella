using LegendaryUmbrella.ConsoleLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LegendaryUmbrella.LockFile
{
	class Program
	{
		private static FileStream stream;

		static int Main(string[] args)
		{
			if (args.Length < 1 || args[0] == "-?" || args[0].ToLowerInvariant() == "-help")
			{
				PrintHelp();
				return 0;
			}
			Options o = new Options(args);
			try
			{
				stream = new FileStream(o.FileName, FileMode.Open, FileAccess.Read, o.SharingType);
            }
			catch (Exception e) when (e is UnauthorizedAccessException || e is IOException || e is ArgumentException)
			{
				Win32Exception ex = ConsoleUtilities.GetNativeException(e);
				Console.WriteLine(ex.Message);
				return ex.NativeErrorCode;
			}
			Console.CancelKeyPress += Console_CancelKeyPress;
			Console.WriteLine(Properties.Resources.OpenSuccess);
			while (true) System.Threading.Thread.Sleep(Int32.MaxValue);
		}

		private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
		{
			if (stream != null) stream.Close(); // properly close our handle
		}

		private static void PrintHelp()
		{
			String[,] commandLineParams = new String[5, 2];
			commandLineParams[0, 0] = "file";
			commandLineParams[1, 0] = "-r";
			commandLineParams[2, 0] = "-w";
			commandLineParams[3, 0] = "-d";
			commandLineParams[4, 0] = "-n";
			commandLineParams[0, 1] = Properties.Resources.ParamFile;
			commandLineParams[1, 1] = Properties.Resources.ParamAllowRead;
			commandLineParams[2, 1] = Properties.Resources.ParamAllowWrite;
			commandLineParams[3, 1] = Properties.Resources.ParamAllowDelete;
			commandLineParams[4, 1] = Properties.Resources.ParamNoAccess;
			ConsoleUtilities.PrintHelp(Properties.Resources.Description, "[-r|-w|-d|-n] file", commandLineParams, Properties.Resources.HelpNotes);
		}
	}
}
