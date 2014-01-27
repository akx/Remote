using System.Diagnostics;
using System.Drawing;
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

    	public override Color ProgramColor {
    		get { return Color.DarkSlateGray; }
    	}

    	public override Process Launch()
        {
            return PuTTYSessionProvider.LaunchPuTTY(this);
        }
    }
}