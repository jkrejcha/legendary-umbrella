using System;
using System.IO;

namespace LegendaryUmbrella.LockFile
{
	internal class Options
	{
		private string[] args;

		public Options(string[] args)
		{
			if (args.Length == 0) throw new ArgumentException();
			String firstSwitch = args[0];
			if (!(firstSwitch.StartsWith("-") || firstSwitch.StartsWith("/")))
			{
				SharingType = FileShare.Read;
				FileName = ParseFileNameFromCommandLine(args, 0);
			}
			else
			{
				SharingType = ParseSharingTypeFromCommandLine(firstSwitch.Substring(1));
				if (args.Length == 1) throw new ArgumentException();
				FileName = ParseFileNameFromCommandLine(args, 1);
			}
		}

		private static String ParseFileNameFromCommandLine(String[] args, int position)
		{
			if (args.Length < position) throw new ArgumentException();

		}

		private static FileShare ParseSharingTypeFromCommandLine(String commandLineSwitch)
		{
			FileShare share = FileShare.None;
			while (commandLineSwitch.Length >= 1)
			{
				if (commandLineSwitch.StartsWith("r")) share |= FileShare.Read;
				else if (commandLineSwitch.StartsWith("w")) share |= FileShare.Write;
				else if (commandLineSwitch.StartsWith("d")) share |= FileShare.Delete;
				else if (commandLineSwitch.StartsWith("n")) share = FileShare.None;
				else throw new ArgumentException();
				if (commandLineSwitch.Length == 1) break;
				commandLineSwitch = commandLineSwitch.Substring(1);
			}
			return share;
		}

		public Options(string FileName, FileShare SharingType)
		{
			this.FileName = FileName;
			this.SharingType = SharingType;
		}

		public string FileName { get; private set; }

		public FileShare SharingType { get; private set; }
	}
}