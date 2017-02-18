using System;
using System.Diagnostics;
using System.IO;

namespace GuestServices
{
	public static class FileInfoExtensions
	{
		public static bool SetReadOnly(this FileInfo file, bool showRestrictionsMsg = false)
		{
			FileAttributes attributes;
			try
			{
				attributes = file.Attributes;
				file.Attributes |= FileAttributes.ReadOnly;
			}
			catch (Exception)
			{
				return false;
			}
			if (showRestrictionsMsg && !attributes.HasFlag(FileAttributes.ReadOnly))
			{
				Program.ShowRestrictionsError();
			}
			return true;
		}

		public static bool TryCreateFile(this FileInfo file, out FileStream stream)
		{
			try
			{
				stream = file.Create();
				return true;
			}
			catch (Exception)
			{
				stream = null;
				return false;
			}
		}
	}
}
