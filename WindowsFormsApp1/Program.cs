using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BouncingBall {
	static class Program {
		/// <summary>
		/// Point d'entrée principal de l'application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var roomSettings = ConfigurationManager.AppSettings;
			int room_width = int.Parse(roomSettings["iRoom_width"]);
			int room_lenght = int.Parse(roomSettings["iRoom_lenght"]);
			bool isBroker = bool.Parse(roomSettings["bIsBroker"]);

			// Tablette principale
			// Map de jeu
			if (isBroker) {
				MapView map = new MapView(room_width, room_lenght);
				Thread t = new Thread(() => RunMapView(map));
				t.Start();
			}
			// Fenêtre de la premiére tablette instanciée/lancée
			TabletView tablette = new TabletView(room_width, room_lenght);
			// Serveur lancé
			Application.Run(tablette);
		}

		private static void RunMapView(MapView map) {
			Application.Run(map);
		}
	}
}
