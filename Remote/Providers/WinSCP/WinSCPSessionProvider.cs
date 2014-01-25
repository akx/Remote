using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Win32;
using Remote.Data;
using Remote.Util;

namespace Remote.Providers.WinSCP
{
    internal class WinSCPSessionProvider : SessionProvider
    {
        private class LaunchWinSCPAction : SessionAction
        {
            public LaunchWinSCPAction()
                : base("Launch WinSCP")
            {
            }

            public override void Dispatch(Session session)
            {
                LaunchWinSCP(session);
            }
        }

        private static string _winScpExecutable;

        public static Process LaunchWinSCP(Session session)
        {
            if (_winScpExecutable == null) throw new Problem("WinSCP executable not found");
            var ub = session.UriBuilder;
            ub.Scheme = "sftp";
            return Process.Start(new ProcessStartInfo
            {
                FileName = _winScpExecutable,
                Arguments = ub.Uri.ToString()
            });
        }

        public override IEnumerable<SessionAction> GetSessionActions(Session session)
        {
            if (_winScpExecutable != null)
            {
                return new[] {new LaunchWinSCPAction()};
            }
            return null;
        }

        public override IEnumerable<Session> GetSessions()
        {
            foreach (
                var keyName in
                    RegistryUtil.EnumerateSubkeys(Registry.CurrentUser, @"Software\Martin Prikryl\WinSCP 2\Sessions"))
            {
                var databag = RegistryUtil.GetDataBag(Registry.CurrentUser, keyName);
                var session = Session.FromDataBag<WinSCPSession>(databag);
                if (session != null)
                {
                    if (session.Name == "Default%20Settings") continue;
                    if (session.Name == "Default Settings") continue;
                    yield return session;
                }
            }
        }

        public WinSCPSessionProvider()
        {
            _winScpExecutable = Locator.LocateExecutable("WinSCP");
        }
    }
}