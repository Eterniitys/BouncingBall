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

			var properties = BouncingBall.Properties.Settings.Default;
			int room_width = properties.iRoomWidth;
			int room_lenght = properties.iRoomHeight;

			TabletView tablet = new TabletView(room_width, room_lenght);

			Application.Run(tablet);
		}
	}
}
