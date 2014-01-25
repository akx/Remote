using System;
using System.Collections.Generic;
using Microsoft.Win32;
using Remote.Data;

namespace Remote.Providers.RDP
{
    internal class RDPSessionProvider : SessionProvider
    {
        public override IEnumerable<Session> GetSessions()
        {
            foreach (var session in GetMRUSessions())
            {
                yield return session;
            }
        }

        private IEnumerable<Session> GetMRUSessions()
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Terminal Server Client\Default");
            if (key != null)
            {
                foreach (var valueName in key.GetValueNames())
                {
                    if (valueName.StartsWith("MRU"))
                    {
                        var address = key.GetValue(valueName).ToString();
                        var bits = address.Split(':');
                        int port = (bits.Length > 1 ? Convert.ToInt32(bits[1]) : 3389);
                        string host = bits[0];
                        yield return RDPSession.FromHostAndPort(host, port);
                    }
                }
            }
        }
    }
}