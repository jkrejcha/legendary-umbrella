using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LegendaryUmbrella.ConsoleLib;

namespace LegendaryUmbrella.CurrentDateTime
{
	class Program
	{
		static int Main(string[] args)
		{
			if (args.Length == 0) args = new string[]{ "f" };
			String argsJoined = String.Join(" ", args);
			if (argsJoined.ToLowerInvariant().Equals("-?"))
			{
				PrintHelp();
				return 0;
			}
			try
			{
				Console.WriteLine(DateTime.Now.ToString(argsJoined));
			}
			catch (FormatException)
			{
				Console.WriteLine("Invalid format");
				return 1;
			}
			return 0;
		}

		private static void PrintHelp()
		{
			String[,] commandLineParams = { { "format", Properties.Resources.ParamFormat } };
			ConsoleUtilities.PrintHelp(Properties.Resources.Description, "[format]", commandLineParams, null);
		}
	}
}
