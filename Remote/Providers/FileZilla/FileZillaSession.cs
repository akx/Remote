using System.Collections.Generic;
using Remote.Data;
using Remote.Util;

namespace Remote.Providers.FileZilla
{
    internal class FileZillaSession : Session
    {
        public override string ProgramName
        {
            get { return "FileZilla"; }
        }

        public override string ProgramAbbrev
        {
            get { return "FZ"; }
        }

        public override void Launch()
        {
            FileZillaSessionProvider.LaunchFileZilla(this);
        }

        public static FileZillaSession FromDataBag(Dictionary<string, object> dataBag)
        {
            var session = new FileZillaSession();
            session.DataBag.Update(dataBag);
            return session;
        }
    }
}