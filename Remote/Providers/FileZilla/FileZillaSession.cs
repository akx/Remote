using System.Collections.Generic;
using Remote.Data;
using Remote.Util;

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

        public override void Launch()
        {
            FileZillaSessionProvider.LaunchFileZilla(this);
        }
    }
}