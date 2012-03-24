using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace DigiRec {
	class Recorder {

		public String remoteAddress { get; set; }
		public int remotePort { get; set; }
		public String fileName { get; set; }

		private volatile UdpClient udpClient;
		private volatile bool stopRequested;
		private volatile Thread thread;

		public bool Start() {
			if (remoteAddress.Length == 0) return false;
			if (remotePort == 0) return false;
			if (fileName.Length == 0) return false;

			stopRequested = false;
			thread = new Thread(this.Run);
			thread.Start();

			return true;
		}

		public void Stop() {
			stopRequested = true;
			CloseSocket();
			thread.Join();
		}

		public bool isRecording() {
			return thread != null && thread.IsAlive;
		}

		private void Run() {
			FileStream stream = null;

			try {
				udpClient = new UdpClient(remotePort);
				udpClient.Client.ReceiveBufferSize = 40960;
				udpClient.JoinMulticastGroup(IPAddress.Parse(remoteAddress), 1);
				IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);

				stream = new FileStream(fileName, FileMode.Create);
				BinaryWriter writer = new BinaryWriter(stream);

				byte[] data;
				while (!stopRequested) {
					data = udpClient.Receive(ref ipEndPoint);
					stream.Write(data, 0, data.Length);
				}
			}
			catch (SocketException) {
			}
			catch (ArgumentOutOfRangeException) {
				Log.Warn("Multicast IP address incorrect");
			}
			catch (Exception e) {
				Log.Warn(e.ToString());
				CloseSocket();
			}

			if (stream != null) {
				stream.Close();
			}
		}

		private void CloseSocket() {
			if (udpClient == null) return;

			udpClient.DropMulticastGroup(IPAddress.Parse(remoteAddress));
			udpClient.Close();
			udpClient = null;
		}
	}
}
