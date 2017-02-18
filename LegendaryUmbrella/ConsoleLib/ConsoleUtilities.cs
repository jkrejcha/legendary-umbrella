using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace LegendaryUmbrella.ConsoleLib
{
	public static class ConsoleUtilities
	{
		private static readonly string[] TrueStrings = { "y", "yes", "true", "t", "1" };
		private static readonly string[] FalseStrings = { "n", "no", "false", "f", "0" };

		public static void WriteVersionInformation()
		{
			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
			Console.WriteLine(fvi.ProductName + " [Version " + fvi.ProductVersion + "]");
			Console.WriteLine(fvi.LegalCopyright);
		}

		public static bool PromptTrueFalse(String prompt, bool includeYNQuestion = true, bool loopUntilValidInput = true)
		{
			bool hasLooped = false;
			if (includeYNQuestion) prompt += " (Y/N?)";
			while (hasLooped || loopUntilValidInput)
			{
				Console.Write(prompt + ">");
				String response = Console.ReadLine();
				if (IsBooleanString(response, true)) return true;
				if (IsBooleanString(response, false)) return false;
				hasLooped = true;
			}
			throw new AggregateException("User provided an invalid response");
		}

		public static String PromptCurrentDirectory()
		{
			Console.Write(Environment.CurrentDirectory + ">");
			return Console.ReadLine();
		}

		public static Win32Exception GetNativeException(Exception ex)
		{
			Contract.Requires<ArgumentNullException>(ex != null);
			Exception e = ex.InnerException;
			while (e != null)
			{
				if (e is Win32Exception)
				{
					return (Win32Exception)e;
				}
				e = e.InnerException;
			}
			return null;
		}

		public static String GetNativeErrorStringFromException(Exception ex)
		{
			Contract.Requires<ArgumentNullException>(ex != null);
			Exception e = ex.InnerException;
			while (e != null)
			{
				if (e is Win32Exception)
				{
					return e.Message;
				}
			}
			return null;
		}

		private static bool IsBooleanString(String cmp, bool boolean)
		{
			foreach (String str in boolean ? TrueStrings : FalseStrings)
			{
				if (cmp.Equals(str, StringComparison.InvariantCultureIgnoreCase)) return true;
			}
			return false;
		}
	}
}
