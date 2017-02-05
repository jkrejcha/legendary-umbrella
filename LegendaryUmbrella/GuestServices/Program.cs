using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuestServices
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			/*if (args.Length > 0 && args[0] == "-r")
			{
				Properties.Settings.Default.Restrictions = !Properties.Settings.Default.Restrictions;
				return;
			}
			Properties.Settings.Default.Restrictions = true;*/
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainFormHidden());
		}
	}
}
