using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Remote.Logging;
using Remote.Util;

namespace Remote.Data
{
    internal class SettingsManager
    {
        public static SettingsManager Instance = new SettingsManager();
        private readonly string _settingsPath;
        private static Logger _log = Logger.Get("SettingsManager");

        public SettingsManager()
        {
            _settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RemoteSettings.xml");
        }

        public IEnumerable<object> GetSettingsObjects()
        {
            foreach (var provider in SessionManager.Instance.Providers)
            {
                var settings = provider.GetSettingsObject();
                if (settings == null) continue;
                yield return settings;
            }
        }

        public void SaveSettings()
        {
            var document = new XDocument();
            var root = new XElement("RemoteSettings");
            document.Add(root);
            foreach (var settings in GetSettingsObjects())
            {
                root.Add(MiniSerialize.Serialize(settings));
            }
            var ms = new MemoryStream();
            using (var xw = XmlWriter.Create(ms, new XmlWriterSettings{Encoding = Encoding.UTF8, Indent = true, IndentChars = "\t"}))
            {
                document.WriteTo(xw);
            }
            File.WriteAllBytes(_settingsPath, ms.ToArray());
            _log.Success("Settings saved to {0}", _settingsPath);
        }

        public void LoadSettings()
        {
            if (!File.Exists(_settingsPath)) return;
            XElement document;
            using (var sr = new StreamReader(_settingsPath))
            {
                document = XDocument.Load(sr).Root;
            }
            if (document == null) return;
            foreach (var settings in GetSettingsObjects())
            {
                var settEl = document.Element(settings.GetType().Name);
                if (settEl != null)
                {
                    MiniSerialize.Unserialize(settEl, settings);
                }
            }
            _log.Success("Settings loaded from {0}", _settingsPath);
        }
    }
}
