using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DigiRec {
	class Settings {

		private static Settings instance;

		private Settings() { }

		public static Settings getInstance() {
			if(instance == null) instance = new Settings();
			return instance;
		}

		public String getProgramName() {
			Assembly asm = Assembly.GetExecutingAssembly();
			return asm.FullName;
		}

		public String getCodeName() {
			return "digirec";
		}

		public String getScheduleFileName() {
			return "schedule.txt";
		}

		public String getServiceName() {
			return "DigiTV Recorder";
		}

		public String getServiceDescription() {
			return "Service scheduled recording of DigiTV UDP broadcasts";
		}
	}
}
