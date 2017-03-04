using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendaryUmbrella.FixExtensions
{
	class Program
	{
		static int Main(string[] args)
		{
			if (args.Length != 0)
			{
				if (args[0].Contains("help") || args[0].Equals("-?")) PrintHelp();
				return 0;
			}
			throw new NotImplementedException();
		}

		private static void PrintHelp()
		{
			ConsoleLib.ConsoleUtilities.PrintHelp(Properties.Resources.Description, null, new string[,] { { "", "" } }, Properties.Resources.HelpNotes);
		}
	}
}
