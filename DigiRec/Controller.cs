using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DigiRec {
	class Controller {

		private volatile bool stopRequested;
		private volatile Thread thread;

		public bool Start() {
			stopRequested = false;

			thread = new Thread(this.Run);
			thread.Start();

			return true;
		}

		public void Stop() {
			stopRequested = true;
		}

		private void Run() {
			Scheduler s = new Scheduler();
			Recorder c = new Recorder();
			stopRequested = false;

			while (!stopRequested) {
				while (!c.isRecording() && s.getNextEntry()) {
					if (s.shouldStart()) {
						Log.Info("Starting recording: IP "+s.ipAddress+", file "+s.fileName);
						c.remoteAddress = s.ipAddress;
						c.remotePort = 1234;
						c.fileName = s.fileName;
						c.Start();
						break;
					}
				}

				if (c.isRecording() && s.shouldEnd()) {
					Log.Info("Stopping recording: IP " + s.ipAddress + ", file " + s.fileName);
					c.Stop();
				}

				Thread.Sleep(30000);
			}
		}
	}
}
