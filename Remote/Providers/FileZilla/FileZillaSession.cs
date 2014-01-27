using System.Diagnostics;
using System.Drawing;
using Remote.Data;

namespace Remote.Providers.FileZilla
{
    public class FileZillaSession : Session
    {
        public override string ProgramName
        {
            get { return "FileZilla"; }
        }

        public override string ProgramAbbrev
        {
            get { return "FZ"; }
        }

    	public override Color ProgramColor {
    		get { return Color.DarkRed; }
    	}

    	public override Process Launch()
        {
            return FileZillaSessionProvider.LaunchFileZilla(this);
        }
    }
}