using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TabletApplication {
	static class Program {
		/// <summary>
		/// Point d'entrée principal de l'application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			int room_width = int.Parse(PropertyReader.getProperty("iRoomWidth"));
			int room_lenght = int.Parse(PropertyReader.getProperty("iRoomHeight"));

			TabletView tablet = new TabletView(room_width, room_lenght);

			Application.Run(tablet);
		}
	}
}
