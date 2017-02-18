using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace LegendaryUmbrella.ConsoleLib
{
	public static class ConsoleUtilities
	{
		private static readonly string[] TrueStrings = { "y", "yes", "true", "t", "1" };
		private static readonly string[] FalseStrings = { "n", "no", "false", "f", "0" };

		public static void WriteVersionInformation()
		{
			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(Assembly.GetCallingAssembly().Location);
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

		public static void PrintHelp(String description, String commandLine, String[,] options, String notes)
		{
			Contract.Requires<ArgumentNullException>(options != null);
			Contract.Requires<ArgumentException>(options.GetLength(1) == 2);
			const int notesMaxLine = 80;
			const int explanationMaxLine = 64;
			if (description != null)
			{
				Console.WriteLine(description);
				Console.WriteLine();
			}
			Console.WriteLine(Assembly.GetCallingAssembly().GetName().Name.ToUpperInvariant() + " " + commandLine);
			Console.WriteLine();
			for (int i = 0; i < options.GetLength(0); i++)
			{
				bool first = true;
				bool exit = false;
				String explanation = options[i, 1];
				int length = explanation.Length;
				int j = 0;
				foreach (String str in CreateWrappedString(explanation, explanationMaxLine))
				{
					WriteParameterHelp(first ? options[i, 0] : "", str);
					first = false;
				}
			}
			Console.WriteLine();
			foreach (String str in CreateWrappedString(notes, notesMaxLine)) Console.WriteLine(str);
			Console.WriteLine();
		}

		private static List<String> CreateWrappedString(String input, int wrapAt)
		{
			String[] words = input.Split(' ');
			List<String> lines = words.Skip(1).Aggregate(words.Take(1).ToList(), (l, w) =>
			{
				if (l.Last().Length + w.Length >= wrapAt)
					l.Add(w);
				else
					l[l.Count - 1] += " " + w;
				return l;
			});
			return lines;
		}

		private static void WriteParameterHelp(String param, String explanation)
		{
			const int paramMax = 9;
			int length = param.Length;
			Console.Write(" " + param);
			if (length > paramMax)
			{
				Console.WriteLine();
			}
			else
			{
				for (int j = param.Length; j < paramMax; j++)
				{
					Console.Write(" ");
				}
			}
			Console.WriteLine(explanation);
		}

		public static Win32Exception GetNativeException(Exception ex)
		{
			Contract.Requires<ArgumentNullException>(ex != null);
			return new Win32Exception(ex.HResult);
		}

		public static String GetNativeErrorStringFromException(Exception ex)
		{
			Contract.Requires<ArgumentNullException>(ex != null);
			return GetNativeException(ex).Message;
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
