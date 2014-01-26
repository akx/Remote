using System.Collections.Generic;
using Remote.Logging;

namespace Remote.Data
{
    public abstract class SessionProvider
    {
        protected static Logger _log;

        protected SessionProvider()
        {
            _log = Logger.Get(GetType().Name);
        }

        public virtual IEnumerable<Session> GetSessions()
        {
            return null;
        }

        public virtual IEnumerable<SessionAction> GetSessionActions(Session session)
        {
            return null;
        }

        public virtual SettingsObject GetSettingsObject()
        {
            return null;
        }
    }
}