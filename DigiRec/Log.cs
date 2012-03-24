using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DigiRec {
	class Log {

		private static String source = Settings.getInstance().getProgramName();
		private static String log = "Application";


		public static void Info(String msg) {
			if (!EventLog.SourceExists(source))
				EventLog.CreateEventSource(source, log);

			Console.WriteLine(msg);
			EventLog.WriteEntry(source, msg, EventLogEntryType.Information);
		}

		public static void Warn(String msg) {
			if (!EventLog.SourceExists(source))
				EventLog.CreateEventSource(source, log);

			Console.WriteLine(msg);
			EventLog.WriteEntry(source, msg, EventLogEntryType.Warning);
		}
	}
}
