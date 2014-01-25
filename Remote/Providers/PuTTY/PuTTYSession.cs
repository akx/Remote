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

        public override void Launch()
        {
            PuTTYSessionProvider.LaunchPuTTY(this);
        }
    }
}