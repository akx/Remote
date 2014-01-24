using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Win32;
using Remote.Util;

namespace Remote.Data
{

	class LaunchPuTTYAction : SessionAction
	{
		public LaunchPuTTYAction()
			: base("Launch PuTTY") {
		}

		public override void Dispatch(Session session) {
			PuTTYSessionProvider.LaunchPuTTY(session);
		}
	}


	class PuTTYSessionProvider: SessionProvider
	{
		private static string _puttyExecutable;

		internal static Process LaunchPuTTY(Session session) {
			if(session is PuTTYSession) {
				return Process.Start(new ProcessStartInfo {
					FileName = _puttyExecutable,
					Arguments = String.Format("-load \"{0}\"", session.Name)
				});
			}
			throw new NotImplementedException("Launching non-native sessions with PuTTY is not supported yet");
		}

		internal override IEnumerable<SessionAction> GetSessionActions(Session session) {
			if (_puttyExecutable != null) {
				return new[] { new LaunchPuTTYAction() };
			}
			return null;
		}

		internal override IEnumerable<Session> GetSessions() {
			foreach (var keyName in RegistryUtil.EnumerateSubkeys(Registry.CurrentUser, @"Software\SimonTatham\PuTTY\Sessions")) {
				var session = PuTTYSession.LoadFromRegistry(Registry.CurrentUser, keyName);
				if (session != null) {
					if (session.Name == "Default%20Settings") continue;
					if (session.Name == "Default Settings") continue;
					yield return session;
				}
			}
		}

		public PuTTYSessionProvider() {
			_puttyExecutable = Locator.LocateExecutable("PuTTY");
		}
	}
}
