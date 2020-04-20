using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BouncingBall
{
	static class Program
	{
		/// <summary>
		/// Point d'entrée principal de l'application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			TabletteView tablette = new TabletteView();

			MapView map = new MapView(tablette.getTablette());
			Thread t = new Thread(() => runTabView(tablette));
			t.Start();
			Application.Run(map);
		}

		private static void runTabView(TabletteView t)
		{
			Application.Run(t);
		}
	}
}
