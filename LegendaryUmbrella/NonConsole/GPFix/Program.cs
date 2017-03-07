using LegendaryUmbrella.ConsoleLib;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Security.Principal;

namespace GPFix
{
	class Program
	{
		private const String CurrentVersionPolicies = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\";
		private const String CVSystemPolicies = CurrentVersionPolicies + "System";
		private const String CVExplorerPolicies = CurrentVersionPolicies + "Explorer";

		private const String SoftwarePolicies = "Software\\Policies\\Microsoft\\Windows\\";

		private const RegistryRights WriteAccess = RegistryRights.WriteKey | RegistryRights.SetValue;
		private const RegistryRights ReadAccess = RegistryRights.ReadKey | RegistryRights.QueryValues;
		private const RegistryRights WritePerms = RegistryRights.ChangePermissions;
		private const RegistryRights ReadPerms = RegistryRights.ReadPermissions;

		private static bool? PauseOnError;
        static void Main(string[] args)
		{
			PauseOnError = ConsoleUtilities.PromptTrueFalse("Pause on error");
			WaitForInputOnError();

			Console.WriteLine("Writing to registry...");
			DisableKeys(CVSystemPolicies, "DisableTaskMgr", "NoDispCPL", "DisableLockWorkstation");
			Write(CVSystemPolicies, "MaxProfileSize", Int32.MaxValue, true, true, false);

			DisableKey(SoftwarePolicies + "System", "DisableCMD");

			DisableKeys(CVExplorerPolicies, "NoRun", "NoDrives");
			DisableKey(CurrentVersionPolicies + "ActiveDesktop", "NoChangingWallPaper");
			TryDisableGPUpdate();
			ReloadExplorer();
        }

		private static void ReloadExplorer()
		{
			try
			{
				foreach (Process p in Process.GetProcessesByName("explorer")) p.Kill();
			}
			catch (Exception e)
			{
				Console.WriteLine("Error in attempt to kill explorer: " + e.Message);
				WaitForInputOnError();
			}
		}

		private static void TryDisableGPUpdate()
		{
			Console.WriteLine("Attempting to disable Group Policy updates...");
			Write(SoftwarePolicies + "System", "GroupPolicyRefreshTime", Int32.MaxValue, true, true, false);
			Write(SoftwarePolicies + "System", "DisableBkGndGroupPolicy", 1, true, true, false);
		}

		private static void DisableKeys(String path, params String[] names)
		{
			foreach (String name in names) DisableKey(path, name);
		}

		private static void DisableKey(String path, String name, bool AttemptToSetWriteBefore = true, bool SetNoWriteAfter = true, bool SetNoReadAfter = true, RegistryHive hive = RegistryHive.CurrentUser)
		{
			Write(path, name, 0, AttemptToSetWriteBefore, SetNoWriteAfter, SetNoReadAfter, hive);
		}

		private static void Write(String path, String name, object value, bool AttemptToSetWriteBefore = true, bool SetNoWriteAfter = true, bool SetNoReadAfter = true, RegistryHive hive = RegistryHive.CurrentUser)
		{
			Console.WriteLine("Attempting to set key " + name + " to " + value.ToString());
			RegistryKey key = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64);
			IdentityReference everyoneReference = GetIdentityReference(WellKnownSidType.WorldSid);
			RegistryRights permissionRights = RegistryRights.ChangePermissions | RegistryRights.ReadPermissions;
			RegistryAccessRule everyoneWriteRule = new RegistryAccessRule(everyoneReference, RegistryRights.WriteKey | permissionRights, AccessControlType.Allow);

			RegistryAccessRule noOneWriteRule = new RegistryAccessRule(everyoneReference, WriteAccess, AccessControlType.Deny);
			RegistryAccessRule noOneReadRule = new RegistryAccessRule(everyoneReference, RegistryRights.ReadKey, AccessControlType.Deny);

			RegistryAccessRule noOneCantWriteRule = new RegistryAccessRule(everyoneReference, 0, AccessControlType.Deny);
			try
			{
				key = key.OpenSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree, WriteAccess | permissionRights);
				if (AttemptToSetWriteBefore)
				{
					Console.WriteLine("Setting security...");
					RegistrySecurity permissions = key.GetAccessControl();
					permissions.RemoveAccessRuleAll(everyoneWriteRule);
					permissions.AddAccessRule(everyoneWriteRule);
					key.SetAccessControl(permissions);
				}
				Console.WriteLine("Setting value...");
				key.SetValue(name, value);
				RegistrySecurity permission = key.GetAccessControl();
				if (SetNoWriteAfter) permission.AddAccessRule(noOneWriteRule);
				if (SetNoReadAfter) permission.AddAccessRule(noOneReadRule);
				Console.WriteLine("Setting security...");
				key.SetAccessControl(permission);
			}
			catch (UnauthorizedAccessException e)
			{
				Console.WriteLine("Error on attempt to modify registry key:");
				Console.WriteLine(e.Message);
				WaitForInputOnError();
			}
		}

		private static void WaitForInputOnError()
		{
			if (PauseOnError == true)
			{
				Console.WriteLine("Pause on error enabled. Press any key to continue...");
				Console.ReadKey();
			}
		}

		public static IdentityReference GetIdentityReference(WellKnownSidType SidType)
		{
			SecurityIdentifier sid = new SecurityIdentifier(SidType, null);
			return ((NTAccount)sid.Translate(typeof(NTAccount)));
		}
	}
}
