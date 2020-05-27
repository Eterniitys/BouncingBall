﻿using BouncingBall;
using BouncingBall.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrokerApplication {
	static class Program {
		/// <summary>
		/// Point d'entrée principal de l'application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			int room_width = Settings.Default.iRoomWidth;
			int room_lenght = Settings.Default.iRoomHeight;
			Application.Run(new MapView(room_width,room_lenght));
		}
	}
}
