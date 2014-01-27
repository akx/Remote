using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Microsoft.Win32;
using Remote.Data;
using Remote.UI;
using Remote.Util;

// ReSharper disable once UnusedAutoPropertyAccessor.Local

namespace Remote.Providers.PuTTY
{
    public class PuTTYSessionProvider : SessionProvider
    {
        #region classes

        private class PuTTYSessionProviderSettings : SettingsObject
        {
            [DisplayName("Colorize Session Background By Host")]
            [Description("Before starting a PuTTY session, change the default background color to one based on the session's host name.")]
            public bool ColorizeSessionBackgroundByHost { get; set; }

        	private float _colorizeSessionBrightness = 0.1f;

        	[DisplayName("Colorization Brightness")]
			[DefaultValue(0.1f)]
			public float ColorizeSessionBrightness {
        		get { return _colorizeSessionBrightness; }
        		set { _colorizeSessionBrightness = value; }
        	}

        	[DisplayName("Additional Command Line Parameters (foreign sessions)")]
			public string ForeignSessionCommandLineParameters { get; set; }

        }

        private class LaunchPuTTYAction : SessionAction
        {
            public LaunchPuTTYAction()
                : base("Launch PuTTY")
            {
            }

            public override void Dispatch(Session session)
            {
                LaunchPuTTY(session);
            }
        }

        #endregion

        private static readonly PuTTYSessionProviderSettings Settings = new PuTTYSessionProviderSettings();
        private static string _puttyExecutable;


        public static Process LaunchPuTTY(Session session)
        {
            if (_puttyExecutable == null)
            {
                throw new Problem("PuTTY could not be located.");
            }
            if (session is PuTTYSession)
            {
                if (Settings.ColorizeSessionBackgroundByHost) {
                	const int N_HUES = 30;
                    var hue = (Hash.Djb2(session.HostName) % N_HUES) * (360.0 / N_HUES);
                    var color = UiUtil.ColorFromHsv(hue, 0.8, Settings.ColorizeSessionBrightness);
                    SetSessionAttribute(session.Name, "Colour2", string.Format("{0},{1},{2}", color.R, color.G, color.B));
                }
                return Process.Start(new ProcessStartInfo
                {
                    FileName = _puttyExecutable,
                    Arguments = String.Format("-load \"{0}\"", session.Name.Replace("%20", " "))
                });
            }
            var args = new StringBuilder();
            args.Append("-ssh ");
            if (session.Port != 22) args.AppendFormat("-P {0} ", session.Port);
			if (!string.IsNullOrEmpty(Settings.ForeignSessionCommandLineParameters)) args.Append(Settings.ForeignSessionCommandLineParameters + " ");
            if (!String.IsNullOrEmpty(session.UserName)) args.AppendFormat("{0}@", session.UserName);
            args.AppendFormat(session.HostName);
            return Process.Start(new ProcessStartInfo
            {
                FileName = _puttyExecutable,
                Arguments = args.ToString()
            });
        }

        public override IEnumerable<SessionAction> GetSessionActions(Session session)
        {
            if (_puttyExecutable != null)
            {
                return new[] {new LaunchPuTTYAction()};
            }
            return null;
        }

        public override IEnumerable<Session> GetSessions()
        {
            foreach (
                var keyName in
                    RegistryUtil.EnumerateSubkeys(Registry.CurrentUser, @"Software\SimonTatham\PuTTY\Sessions"))
            {
                var dataBag = RegistryUtil.GetDataBag(Registry.CurrentUser, keyName);
                var session = Session.FromDataBag<PuTTYSession>(dataBag);
                if (session != null)
                {
                    if (session.Name == "Default%20Settings") continue;
                    if (session.Name == "Default Settings") continue;
                    yield return session;
                }
            }
        }

        private static void SetSessionAttribute(string sessionName, string attribute, object value)
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Software\SimonTatham\PuTTY\Sessions\" + sessionName, true);
            if (key == null) throw new NullReferenceException("Session " + sessionName + " not found in registry or unable to write");
            using (key)
            {
                key.SetValue(attribute, value);
                key.Close();
            }
        }

        public PuTTYSessionProvider()
        {
            _puttyExecutable = Locator.LocateExecutable("PuTTY");
        }

        public override SettingsObject GetSettingsObject()
        {
            return Settings;
        }
    }
}