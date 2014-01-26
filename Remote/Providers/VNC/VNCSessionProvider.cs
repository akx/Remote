using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using Remote.Data;
using Remote.Providers.PuTTY;
using Remote.Util;

namespace Remote.Providers.VNC
{
    internal class VNCSessionProvider : SessionProvider
    {
        private static string _tightVncViewerExecutable;
        private static string _vncViewerExecutable;

        public override IEnumerable<Session> GetSessions()
        {
            foreach (var session in GetTightVNCSessions()) yield return session;
        }

        private static IEnumerable<Session> GetTightVNCSessions()
        {
            foreach (var keyName in RegistryUtil.EnumerateSubkeys(Registry.CurrentUser, @"Software\TightVNC\Viewer\History"))
            {
                if (keyName.EndsWith(".listen", StringComparison.InvariantCultureIgnoreCase)) continue;
                var dataBag = RegistryUtil.GetDataBag(Registry.CurrentUser, keyName);
                var nameParts = keyName.Split('\\');
                var session = VNCSession.FromNameAndDataBag<TightVNCSession>(nameParts[nameParts.Length - 1], dataBag);
                if (session != null)
                {
                    yield return session;
                }
            }
        }

        public static Process LaunchVNC(VNCSession vncSession)
        {
            if (String.IsNullOrEmpty(_vncViewerExecutable)) throw new Problem("No VNC viewer could be found.");

            return Process.Start(new ProcessStartInfo
            {
                FileName = _vncViewerExecutable,
                Arguments = (vncSession.Port != 5900 ? string.Format("{0}::{1}", vncSession.HostName, vncSession.Port) : vncSession.HostName)
            });
        }

        public VNCSessionProvider()
        {
            _tightVncViewerExecutable = Locator.LocateExecutable("tvnviewer", new[] {"TightVNC"});
            // XXX: Support for other VNCen.
            _vncViewerExecutable = _tightVncViewerExecutable ?? null;
        }
    }
}