using Microsoft.Win32;
using Remote.Util;

namespace Remote.Data {
	internal class PuTTYSession : Session {
		public override string ProgramName {
			get { return "PuTTY"; }
		}

		public static PuTTYSession LoadFromRegistry(RegistryKey key, string registryKey) {
			var session = new PuTTYSession();
			session.DataBag.Update(RegistryUtil.GetDataBag(key, registryKey));
			return session;
		}

		public override void Launch() {
			PuTTYSessionProvider.LaunchPuTTY(this);
		}
	}
}