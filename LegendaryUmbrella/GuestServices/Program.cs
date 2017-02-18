using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuestServices
{
	static class Program
	{
		public const int ProcessCheckInterval = 1700;

		private static byte[] WarningMsgBytes = Encoding.UTF8.GetBytes(Properties.Resources.WarningMsg);
		private readonly static string[] NoExec = new String[] { "taskmgr", "regedit", "reg", "ftp", "telnet",
																"driverlist", "virtualbox", "cpuz", "Task Explorer",
																"Task Explorer-x64", "cmd", "takeown"};

		private static Dictionary<int, int> failedPIDs = new Dictionary<int, int>();

		private static FileSystemWatcher noteWatcher = new FileSystemWatcher();

		private static bool restrictProcesses = true;


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			if (!Properties.Settings.Default.Restrictions) restrictProcesses = false;
			Application.EnableVisualStyles();
			SetupNoteWatcher();
			SetupTimer();
			Logon(true);
			//Logon(args.Length >= 1 && args[0] == "-shell");
			while (true) Thread.Sleep(Int32.MaxValue);
		}

		private static void Logon(bool isShell)
		{
			if (!isShell) ExternalUtil.SetDesktopInvisible();
			MessageBox.Show(Properties.Resources.WarningMsg, "User Account Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			RelaunchExplorer();
		}

		private static void ProcessCheck(bool manualCheck = false)
		{
			if (restrictProcesses)
			{
				Process[] processList = Process.GetProcesses();
				foreach (Process process in processList)
				{
					int pid = process.Id;
					if (failedPIDs.Keys.Contains(pid) && failedPIDs[pid] > 3) continue;
					foreach (String noExecProcess in NoExec)
					{
						if (process.ProcessName.ToLower().Equals(noExecProcess.ToLower()))
						{
							if (process.TryKill())
							{
								Debug.WriteLine("killed bad process");
								ShowRestrictionsError();
							}
							else
							{
								if (failedPIDs.Keys.Contains(pid))
								{
									Debug.WriteLine("Failed. Times failed: " + failedPIDs[pid]);
									failedPIDs[pid]++;
								}
								else
								{
									failedPIDs.Add(pid, 1);
								}
							}
						}
					}
				}
			}
			if (manualCheck) return;
			Thread.Sleep(ProcessCheckInterval);
		}

		public static void ShowRestrictionsError(bool createNewThread = true)
		{
			if (createNewThread)
			{
				Thread t = new Thread(() => ShowRestrictionsError(false));
				t.Priority = ThreadPriority.Lowest;
				t.TrySetApartmentState(ApartmentState.STA);
				t.Start();
				return;
			}
			MessageBox.Show(Properties.Resources.RestrictionsMsg, "Restrictions", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
		}

		private static void TryKillExplorer()
		{
			return;
		}

		private static void RelaunchExplorer()
		{
			Process explorer = new Process();
			explorer.StartInfo.FileName =
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe");
			explorer.StartInfo.ErrorDialog = false;
			try
			{
				explorer.Start();
			}
			catch (Win32Exception e)
			{
				restrictProcesses = false;
				MessageBox.Show(Properties.Resources.ShellLaunchFailMsg.Replace("%0", e.Message), "User Account Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				RelaunchExplorer();
			}
		}

		private static void NoteChangedHandler(object sender, FileSystemEventArgs e)
		{
			if (e.ChangeType == WatcherChangeTypes.Created) return; // it's being created; let's ignore
			if (!File.Exists(e.FullPath)) return;
			// Detected a change (either by our process or another). Adding readonly attribute.
			RefreshAttributes(e.FullPath, true);
		}

		private static void NoteDeletedHandler(object sender, FileSystemEventArgs e)
		{
			Debug.WriteLine("Someone deleted our file :("); // you jerk
			FileStream stream;
			FileInfo file = new FileInfo(e.FullPath);
			if (!file.TryCreateFile(out stream)) return;
			stream.Write(WarningMsgBytes, 0, WarningMsgBytes.Length);
			stream.Close();
			file.SetReadOnly();
			ExternalUtil.RefreshDesktop();
			ShowRestrictionsError();
		}

		private static void NoteRenamedHandler(object sender, RenamedEventArgs e)
		{
			if (e.FullPath.Contains("$Recycle.bin"))
			{
				try
				{
					File.Move(e.FullPath, e.OldFullPath);
					ShowRestrictionsError();
				}
				catch (Exception)
				{ }
			}
		}

		/// <summary>
		/// Refreshes attributes on file, optionally showing the "restrictions" message
		/// </summary>
		/// <param name="FilePath"></param>
		/// <param name="ShowRestrictionsMessage"></param>
		private static void RefreshAttributes(String FilePath, bool ShowRestrictionsMessage = false)
		{
			FileAttributes attributes = File.GetAttributes(FilePath);
			File.SetAttributes(FilePath, attributes | FileAttributes.ReadOnly);
			if (ShowRestrictionsMessage && !attributes.HasFlag(FileAttributes.ReadOnly))
			{
				ShowRestrictionsError();
			}
		}

		private static void SetupNoteWatcher()
		{
			noteWatcher.Path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			noteWatcher.Filter = "Note.txt";
			noteWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Attributes | NotifyFilters.LastWrite;
			noteWatcher.Changed += NoteChangedHandler;
			noteWatcher.Deleted += NoteDeletedHandler;
			noteWatcher.Renamed += NoteRenamedHandler;
		}

		private static void SetupTimer()
		{
			Thread t = new Thread(() => { while (true) ProcessCheck(); });
			t.Start();
		}
	}
}
