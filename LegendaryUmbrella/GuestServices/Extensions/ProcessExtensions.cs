using System;
using System.Diagnostics;

namespace GuestServices
{
	public static class ProcessExtensions
	{
		public static bool TryKill(this Process p)
		{
			try
			{
				p.Kill();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
