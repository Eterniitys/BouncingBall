using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ObjectLibrary {
	class PropertyReader {
		public static string getProperty(string property) {
			XmlDocument d = new XmlDocument();
			try {
				d.Load("BouncingBall.exe.config");
			} catch {
			}
			return d.SelectNodes("//setting[@name=\"" + property + "\"]")[0].InnerText;
		}

		public static string[] getPropertyAsArray(string property) {
			XmlDocument d = new XmlDocument();
			string[] array = null;
			try {
				d.Load("BouncingBall.exe.config");
				var a = d.SelectNodes("//setting[@name=\"" + property + "\"]//ArrayOfString")[0];
				array = new string[a.ChildNodes.Count];
				for (int i = 0; i < a.ChildNodes.Count; i++) {
					array[i] = a.ChildNodes[i].InnerText;
				}
			} catch {
			}
			return array;
		}
	}


}
