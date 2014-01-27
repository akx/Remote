using System.Diagnostics;
using System.Drawing;
using Remote.Data;

namespace Remote.Providers.WinSCP
{
    public class WinSCPSession : Session
    {
        public override string ProgramName
        {
            get { return "WinSCP"; }
        }

        public override string ProgramAbbrev
        {
            get { return "S"; }
        }

    	public override Color ProgramColor {
    		get { return Color.Beige; }
    	}

    	public override Process Launch()
        {
            return WinSCPSessionProvider.LaunchWinSCP(this);
        }
    }
}