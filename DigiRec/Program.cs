using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO;
using System.Configuration.Install;
using System.Collections;

namespace DigiRec {
	class Program : ServiceBase {

		private Controller c = null;


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// 
		/*
		[DllImport("kernel32.dll",
			EntryPoint = "GetStdHandle",
			SetLastError = true,
			CharSet = CharSet.Auto,
			CallingConvention = CallingConvention.StdCall)]
		private static extern IntPtr GetStdHandle(int nStdHandle);
		[DllImport("kernel32.dll",
			EntryPoint = "AllocConsole",
			SetLastError = true,
			CharSet = CharSet.Auto,
			CallingConvention = CallingConvention.StdCall)]
		private static extern int AllocConsole();
		private const int STD_OUTPUT_HANDLE = -11;
		private const int MY_CODE_PAGE = 437;
		*/

		public Program() {
			this.ServiceName = Settings.getInstance().getServiceDescription();

			String path = System.Reflection.Assembly.GetExecutingAssembly().Location;
			path = System.IO.Path.GetDirectoryName(path);
			Directory.SetCurrentDirectory(path);
		}

		static void Main(string[] args) {
			/*
			AllocConsole();
			IntPtr stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
			SafeFileHandle safeFileHandle = new SafeFileHandle(stdHandle, true);
			FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
			Encoding encoding = System.Text.Encoding.GetEncoding(MY_CODE_PAGE);
			StreamWriter standardOutput = new StreamWriter(fileStream, encoding);
			standardOutput.AutoFlush = true;
			Console.SetOut(standardOutput); 
			*/

			if (args.Length == 0) {
				ServiceBase.Run(new Program());
				return;
			}
			
            try {
                foreach (string arg in args) {
                    switch (arg) {
                        case "-i":
                        case "-install":
							Install(false, args);
							break;
                        case "-u":
                        case "-uninstall":
							Install(true, args);
							break;
                    }
                }
            }
            catch (Exception ex) {
                Console.Error.WriteLine(ex.Message);
            }
			
		}

		static void Install(bool undo, string[] args) {
			try {
				Console.WriteLine(undo ? "uninstalling" : "installing");
				using (AssemblyInstaller inst = new AssemblyInstaller(typeof(Program).Assembly, args)) {
					IDictionary state = new Hashtable();
					inst.UseNewContext = true;
					try {
						if (undo) {
							inst.Uninstall(state);
						}
						else {
							inst.Install(state);
							inst.Commit(state);
						}
					}
					catch {
						try {
							inst.Rollback(state);
						}
						catch { }
						throw;
					}
				}
			}
			catch (Exception ex) {
				Console.Error.WriteLine(ex.Message);
			}
		} 

		protected override void OnStart(string[] args) {
			base.OnStart(args);

			c = new Controller();
			if(c != null) c.Start();
		}

		protected override void OnStop() {
			base.OnStop();

			if(c != null) c.Stop();
		}
	}
}
