using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendaryUmbrella.TestApp
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			SecurityDialog.Show(IntPtr.Zero, "C:\\Windows\\system32\\cmd.exe");
		}
	}
}
