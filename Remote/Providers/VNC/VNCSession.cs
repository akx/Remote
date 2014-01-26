using System;
using System.Collections.Generic;
using System.Diagnostics;
using Remote.Data;

namespace Remote.Providers.VNC
{
    abstract class VNCSession: Session
    {
        public override string ProgramName
        {
            get { return "VNC"; }
        }

        public override string ProgramAbbrev
        {
            get { return "V"; }
        }

        public static T FromNameAndDataBag<T>(string name, Dictionary<string, object> dataBag) where T : VNCSession, new()
        {
            string host;
            int port;
            if (name.Contains("::"))
            {
                var bits = name.Split(new []{"::"}, 2, StringSplitOptions.None);
                host = bits[0];
                port = Convert.ToInt32(bits[1]);
            }
            else if (name.Contains(":"))
            {
                var bits = name.Split(':');
                host = bits[0];
                port = 5900 + Convert.ToInt32(bits[1]);
            }
            else
            {
                host = name;
                port = 5900;
            }
            dataBag["HostName"] = host;
            dataBag["Port"] = port;
            dataBag["Name"] = string.Format("{0} ({1})", host, port);

            var session = new T();
            session.DataBag.Update(dataBag);
            

            return session;
        }

        public override Process Launch()
        {
            return VNCSessionProvider.LaunchVNC(this);
        }
    }
}
