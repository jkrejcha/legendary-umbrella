using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestServicesNoLib
{
	class Program
	{
		public static void Main(string[] args)
		{
			/*
			bool reverse = false;
			if (args.Length > 0 && args[0] == "-r") reverse = true;
			DisableTaskManager(!reverse);
			SetRegistryKeys(reverse);
			*/
		}

		private static void SetRegistryKeys(bool reverse)
		{
			RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default);
			//key.SetValue()
		}

		private static void DisableTaskManager(bool enabled)
		{
			const string TaskMgrDisableString = "DisableTaskMgr";
			const string enabledValue = "0";
			const string disabledValue = "1";
			const string subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
			RegistryKey regkey;

			try
			{
				regkey = Registry.CurrentUser.CreateSubKey(subKey);
				regkey.SetValue(TaskMgrDisableString, enabled ? enabledValue : disabledValue);
				regkey.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
		}

		private static void SetValue(RegistryKey key, string name, object value)
		{
			try
			{
				key.SetValue(name, value);
			} catch (Exception e)
			{
				Console.WriteLine("Error setting registry key value " + name + " with " + value.ToString());
				Console.WriteLine(e.Message);
			}
		}
	}
}
