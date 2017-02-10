using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuestServices
{
	public partial class MainFormHidden : Form
	{
		private readonly byte[] WarningMsgBytes = Encoding.UTF8.GetBytes(Properties.Resources.WarningMsg);
        private readonly string[] NoExec = new String[] { "taskmgr", "regedit", "reg", "ftp", "telnet", "driverlist",
														  "virtualbox", "cpuz", "Task Explorer", "Task Explorer-x64",
														  "cmd", "takeown"};
		private Dictionary<int, int> FailedPIDs = new Dictionary<int, int>();

		public MainFormHidden()
		{
			InitializeComponent();
			fswNoteWatcher.Path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			if (!Properties.Settings.Default.Restrictions)
			{
				disableRestrictionsToolStripMenuItem_Click(null, null);
			}
		}

		private void tmrProcessCheck_Tick(object sender, EventArgs e)
		{
			Process[] processList = Process.GetProcesses();
			foreach (Process process in processList)
			{
				int pid = process.Id;
				if (FailedPIDs.Keys.Contains(pid) && FailedPIDs[pid] > 3) continue;
				foreach (String noExecProcess in NoExec)
				{
					if (process.ProcessName.ToLower().Equals(noExecProcess.ToLower()))
					{
						Debug.WriteLine(Environment.UserName);
						try
						{
							process.Kill(); // no
						}
						catch (Exception)
						{
							if (FailedPIDs.Keys.Contains(pid))
							{
								Debug.WriteLine("Failed. Times failed: " + FailedPIDs[pid]);
								FailedPIDs[pid]++;
							}
							else
							{
								FailedPIDs.Add(pid, 1);
							}
							continue;
						}
						CreateRestrictionsMsgThread();
					}
				}
			}
		}

		private void CreateRestrictionsMsgThread()
		{
			Thread t = new Thread(new ThreadStart(ShowRestrictionsMsg));
			t.Priority = ThreadPriority.Lowest;
			t.Start();
		}

		private void ShowRestrictionsMsg()
		{
			MessageBox.Show(Properties.Resources.RestrictionsMsg, "Restrictions", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void TryKillExplorer()
		{
			return;
		}

		private void MainFormHidden_Load(object sender, EventArgs e)
		{
			ExternalUtil.SetDesktopInvisible();
			MessageBox.Show(Properties.Resources.WarningMsg, "User Account Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			RelaunchExplorer();
		}


		private void RelaunchExplorer()
		{
			Process explorer = new Process();
			explorer.StartInfo.FileName =
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe");
			explorer.StartInfo.ErrorDialog = false;
			try
			{
				explorer.Start();
				if (!Properties.Settings.Default.Restrictions) tmrProcessCheck.Enabled = true;
			}
			catch (Win32Exception e)
			{
				tmrProcessCheck.Enabled = false; // disable restrictions temporarily
				MessageBox.Show(Properties.Resources.ShellLaunchFailMsg.Replace("%0", e.Message), "User Account Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				RelaunchExplorer();
			}
		}

		private void fswNoteWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			if (e.ChangeType == WatcherChangeTypes.Created) return; // it's being created; let's ignore
			if (!File.Exists(e.FullPath)) return;
			// Detected a change (either by our process or another). Adding readonly attribute.
			RefreshAttributes(e.FullPath, true);
		}

		private void fswNoteWatcher_Deleted(object sender, FileSystemEventArgs e)
		{
			Debug.WriteLine("Someone deleted our file :("); // you jerk
			try
			{
				FileStream stream = new FileStream(e.FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
				stream.Write(WarningMsgBytes, 0, WarningMsgBytes.Length);
				stream.Close();
				RefreshAttributes(e.FullPath);
				ExternalUtil.RefreshDesktop();
				CreateRestrictionsMsgThread();
			}
			catch (Exception)
			{ }
		}

		private void fswNoteWatcher_Renamed(object sender, RenamedEventArgs e)
		{
			if (e.FullPath.Contains("$Recycle.bin"))
			{
				try
				{
					File.Move(e.FullPath, e.OldFullPath);
					CreateRestrictionsMsgThread();
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
		private void RefreshAttributes(String FilePath, bool ShowRestrictionsMessage = false)
		{
			FileAttributes attributes = File.GetAttributes(FilePath);
			File.SetAttributes(FilePath, attributes | FileAttributes.ReadOnly);
			if (ShowRestrictionsMessage && !attributes.HasFlag(FileAttributes.ReadOnly))
			{
				CreateRestrictionsMsgThread();
			}
		}

		private void disableRestrictionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			tmrProcessCheck.Stop();
			fswNoteWatcher.EnableRaisingEvents = false;
			MessageBox.Show(Properties.Resources.RestrictionsDisabledMsg, "Restrictions", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		/// <summary>
		/// ALT+TAB begone!
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				var Params = base.CreateParams;
				Params.ExStyle |= 0x80;
				return Params;
			}
		}
	}
}
