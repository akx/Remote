using Microsoft.Win32;
using Remote.Util;

namespace Remote.Data {
	internal class WinSCPSession: Session {
		public override string ProgramName {
			get { return "WinSCP"; }
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