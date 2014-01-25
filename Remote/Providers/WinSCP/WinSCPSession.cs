using Microsoft.Win32;
using Remote.Data;
using Remote.Util;

namespace Remote.Providers.WinSCP {
	internal class WinSCPSession: Session {
		public override string ProgramName {
			get { return "WinSCP"; }
		}

	    public override string ProgramAbbrev
	    {
	        get { return "S"; }
	    }

	    public static WinSCPSession LoadFromRegistry(RegistryKey key, string registryKey) {
			var session = new WinSCPSession();
			session.DataBag.Update(RegistryUtil.GetDataBag(key, registryKey));
			return session;
		}

		public override void Launch() {
			WinSCPSessionProvider.LaunchWinSCP(this);
		}
	}
}