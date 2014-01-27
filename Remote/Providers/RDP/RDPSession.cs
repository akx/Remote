using System.Diagnostics;
using System.Drawing;
using Remote.Data;
using Remote.Util;

namespace Remote.Providers.RDP
{
    public class RDPSession : Session
    {
        public override string ProgramName
        {
            get { return "Remote Desktop Connection"; }
        }

        public override string ProgramAbbrev
        {
            get { return "RD"; }
        }

    	public override Color ProgramColor {
    		get { return Color.DodgerBlue; }
    	}

    	public static RDPSession FromHostAndPort(string hostname, int port)
        {
            var session = new RDPSession();
            session.DataBag["Host"] = hostname;
            session.DataBag["Port"] = port;
            session.DataBag["Name"] = string.Format("{0} ({1})", session.HostName, session.Port);
            return session;
        }

        public override Process Launch()
        {
            var mstsc = Locator.LocateExecutable("mstsc");
            if (mstsc == null) throw new Problem("Remote Desktop client (mstsc) not found.");
            return Process.Start(new ProcessStartInfo
            {
                FileName = mstsc,
                Arguments = string.Format("/v:{0}:{1}", HostName, Port)
            });
        }
    }
}