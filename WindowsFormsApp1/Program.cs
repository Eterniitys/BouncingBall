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
			
			// Tablette principale
			TabletteView tablette = new TabletteView();
			// Map de jeu (prend une référence à la tablette principale)
			MapView map = new MapView(tablette.getTablette());
			// Fenêtre de la premiére tablette instanciée/lancée
			Thread t = new Thread(() => RunTabView(tablette));
			t.Start();
			// Serveur lancé
			Application.Run(map);
		}

		private static void RunTabView(TabletteView t)
		{
			Application.Run(t);
		}
	}
}
