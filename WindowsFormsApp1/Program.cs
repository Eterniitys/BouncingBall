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

			var thread = new Thread(mapView);
			thread.Start();
			Application.Run(new TabletteView());
		}

		private static void mapView()
		{
			Application.Run(new MapView());
		}
	}
}
