using System;
using System.Diagnostics;
using System.Reflection;

namespace LegendaryUmbrella.ConsoleLib
{
	public sealed class ConsoleUtilities
	{
		private ConsoleUtilities() { }

		public void WriteVersionInformation()
		{
			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
			Console.WriteLine(fvi.ProductName + " [Version " + fvi.ProductVersion + "]");
			Console.WriteLine(fvi.LegalCopyright);
		}

		public String PromptCurrentDirectory()
		{
			Console.Write(Environment.CurrentDirectory + ">");
			return Console.ReadLine();
		}
	}
}
