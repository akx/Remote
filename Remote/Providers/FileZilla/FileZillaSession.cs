using System.Collections.Generic;
using System.Diagnostics;
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

        public override Process Launch()
        {
            return FileZillaSessionProvider.LaunchFileZilla(this);
        }
    }
}