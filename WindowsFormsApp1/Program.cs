using System;
using System.Collections.Generic;
using System.Configuration;
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

			var roomSettings = ConfigurationManager.AppSettings;
			int room_width = int.Parse(roomSettings["iRoom_width"]);
			int room_lenght = int.Parse(roomSettings["iRoom_lenght"]);

			// Tablette principale
			// Map de jeu
			MapView map = new MapView(room_width, room_lenght);
			// Fenêtre de la premiére tablette instanciée/lancée
			TabletteView tablette = new TabletteView(room_width, room_lenght);
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
