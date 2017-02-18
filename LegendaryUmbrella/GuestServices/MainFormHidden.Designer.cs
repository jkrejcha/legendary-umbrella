namespace GuestServices
{
	partial class MainFormHidden
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.tmrProcessCheck = new System.Windows.Forms.Timer(this.components);
			this.fswNoteWatcher = new System.IO.FileSystemWatcher();
			((System.ComponentModel.ISupportInitialize)(this.fswNoteWatcher)).BeginInit();
			this.SuspendLayout();
			// 
			// tmrProcessCheck
			// 
			this.tmrProcessCheck.Enabled = global::GuestServices.Properties.Settings.Default.Restrictions;
			this.tmrProcessCheck.Interval = 1500;
			// 
			// fswNoteWatcher
			// 
			this.fswNoteWatcher.EnableRaisingEvents = global::GuestServices.Properties.Settings.Default.Restrictions;
			this.fswNoteWatcher.Filter = "Note.txt";
			this.fswNoteWatcher.NotifyFilter = ((System.IO.NotifyFilters)((((System.IO.NotifyFilters.FileName | System.IO.NotifyFilters.DirectoryName) 
            | System.IO.NotifyFilters.Attributes) 
            | System.IO.NotifyFilters.LastWrite)));
			this.fswNoteWatcher.SynchronizingObject = this;
			// 
			// MainFormHidden
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(0, 0);
			this.ControlBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "MainFormHidden";
			this.Opacity = 0D;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.TransparencyKey = System.Drawing.SystemColors.Control;
			this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			((System.ComponentModel.ISupportInitialize)(this.fswNoteWatcher)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer tmrProcessCheck;
		private System.IO.FileSystemWatcher fswNoteWatcher;
	}
}