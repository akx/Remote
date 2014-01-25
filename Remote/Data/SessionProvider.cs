using System.Collections.Generic;

namespace Remote.Data
{
    public abstract class SessionProvider
    {
        public virtual IEnumerable<Session> GetSessions()
        {
            return null;
        }

        public virtual IEnumerable<SessionAction> GetSessionActions(Session session)
        {
            return null;
        }

        public virtual object GetSettingsObject()
        {
            return null;
        }
    }
}