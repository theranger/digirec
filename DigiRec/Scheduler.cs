using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DigiRec {
	class Scheduler {
		private DateTime dateStart;
		private DateTime dateEnd;
		private StreamReader f;
		private bool hasLine;

		public String ipAddress;
		public String fileName;

		public bool getNextEntry() {
			try {
				if (f == null) f = File.OpenText(Settings.getInstance().getScheduleFileName());

				String line = null;
				if ((line = f.ReadLine()) == null) {
					f.Close();
					f = null;
					return false;
				}

				hasLine = parseLine(line);
				return true;
			}
			catch (FileNotFoundException) {
				Log.Warn("Schedule file not found");
			}
			catch (PathTooLongException) {
				Log.Warn("File path too long");
			}
			catch (UnauthorizedAccessException) {
				Log.Warn("Access to schedule file denied");
			}
			catch (Exception) {
				Log.Warn("Error opening schedule file");
			}

			return false;
		}

		public bool shouldStart() {
			if (!hasLine) return false;
			if (dateEnd == null || dateStart == null) return false;

			if (dateEnd > DateTime.Now && dateStart < DateTime.Now) {
				f.Close();
				f = null;
				return true;
			}

			return false;
		}

		public bool shouldEnd() {
			if (dateEnd == null) return false;

			return dateEnd <= DateTime.Now;
		}

		private bool parseLine(String line) {
			//<date start> <date end> <ip> <file>
			String[] fields = line.Split('\t');
			if (fields.Length != 4) return false;

			try {
				dateStart = DateTime.Parse(fields[0]);
				dateEnd = DateTime.Parse(fields[1]);
				ipAddress = fields[2];
				fileName = fields[3];
				return true;
			}
			catch (FormatException) {
				Log.Warn("Error parsing date");
			}
			catch (ArgumentNullException) {
				Log.Warn("Date is null");
			}

			return false;
		}
	}
}
