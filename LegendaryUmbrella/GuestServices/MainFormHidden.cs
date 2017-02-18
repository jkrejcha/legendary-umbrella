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
		

		public MainFormHidden()
		{
			InitializeComponent();
			if (!Properties.Settings.Default.Restrictions)
			{
				//disableRestrictionsToolStripMenuItem_Click(null, null);
			}
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
