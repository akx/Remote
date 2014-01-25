using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Remote.Util;

namespace Remote.Data
{
    internal class SettingsManager
    {
        public static SettingsManager Instance = new SettingsManager();
        private string _settingsPath;

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
        }

        public void LoadSettings()
        {
            if (!File.Exists(_settingsPath)) return;
            XElement document;
            using (var sr = new StreamReader(_settingsPath))
            {
                document = XDocument.Load(sr).Root;
            }
            foreach (var settings in GetSettingsObjects())
            {
                var settEl = document.Element(settings.GetType().Name);
                if (settEl != null)
                {
                    MiniSerialize.Unserialize(settEl, settings);
                }
            }
        }
    }
}
