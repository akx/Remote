using System.Diagnostics;
using Remote.Data;

namespace Remote.Providers.PuTTY
{
    public class PuTTYSession : Session
    {
        public override string ProgramName
        {
            get { return "PuTTY"; }
        }

        public override string ProgramAbbrev
        {
            get { return "P"; }
        }

        public override Process Launch()
        {
            return PuTTYSessionProvider.LaunchPuTTY(this);
        }
    }
}