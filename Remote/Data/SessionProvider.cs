using System.Collections.Generic;
using Remote.Util;

namespace Remote.Data {
	internal abstract class SessionProvider {
		internal virtual IEnumerable<Session> GetSessions() {
			return null;
		}

		internal virtual IEnumerable<SessionAction> GetSessionActions(Session session) {
			return null;
		}
	}
}