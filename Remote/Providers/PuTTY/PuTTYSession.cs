using Microsoft.Win32;
using Remote.Data;
using Remote.Util;

namespace Remote.Providers.PuTTY
{
    internal class PuTTYSession : Session
    {
        public override string ProgramName
        {
            get { return "PuTTY"; }
        }

        public override string ProgramAbbrev
        {
            get { return "P"; }
        }

        public static PuTTYSession LoadFromRegistry(RegistryKey key, string registryKey)
        {
            var session = new PuTTYSession();
            session.DataBag.Update(RegistryUtil.GetDataBag(key, registryKey));
            return session;
        }

        public override void Launch()
        {
            PuTTYSessionProvider.LaunchPuTTY(this);
        }
    }
}