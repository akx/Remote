using System;
using System.Diagnostics;
using System.Windows.Forms;
using Remote.Data;
using Remote.Providers.FileZilla;
using Remote.Providers.PuTTY;
using Remote.Providers.RDP;
using Remote.Providers.WinSCP;
using Remote.UI;
using Remote.Util;

namespace Remote
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            if (!Debugger.IsAttached)
            {
                AppDomain.CurrentDomain.UnhandledException += DumpHandler.UnhandledException;
                Application.ThreadException += DumpHandler.UnhandledException;
            }
            Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static void Initialize()
        {
            var sm = SessionManager.Instance;
            sm.AddProvider(new PuTTYSessionProvider());
            sm.AddProvider(new WinSCPSessionProvider());
            sm.AddProvider(new FileZillaSessionProvider());
            sm.AddProvider(new RDPSessionProvider());
            SettingsManager.Instance.LoadSettings();
        }
    }
}