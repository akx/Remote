using System.Collections.Generic;

namespace Remote.Data
{
    public class SessionManager
    {
        public static readonly SessionManager Instance = new SessionManager();
        private readonly List<Session> _sessions = new List<Session>();
        private readonly List<SessionProvider> _providers = new List<SessionProvider>();

        public IEnumerable<Session> Sessions
        {
            get { return _sessions.AsReadOnly(); }
        }

        public IEnumerable<SessionProvider> Providers
        {
            get { return _providers.AsReadOnly(); }
        }


        public void AddProvider(SessionProvider provider)
        {
            _providers.Add(provider);
        }

        public void Populate()
        {
            _sessions.Clear();
            foreach (var provider in _providers)
            {
                var sessions = provider.GetSessions();
                if (sessions != null) _sessions.AddRange(sessions);
            }
        }
    }
}