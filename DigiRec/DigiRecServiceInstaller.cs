using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Install;
using System.ComponentModel;
using System.ServiceProcess;

namespace DigiRec {

	[RunInstaller(true)] 
	public sealed class DigiRecServiceInstallerProcess : ServiceProcessInstaller  {
	
		public DigiRecServiceInstallerProcess() {
			this.Account = ServiceAccount.LocalSystem;
		}
	}

	[RunInstaller(true)]
	public sealed class TSRecorderServiceInstaller : ServiceInstaller {
		Settings settings = Settings.getInstance();

		public TSRecorderServiceInstaller() {
			this.Description = settings.getServiceDescription();
			this.DisplayName = settings.getServiceName();
			this.ServiceName = settings.getCodeName();
			this.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
		}
	}
}
