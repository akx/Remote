using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using Remote.Data;
using Remote.Util;

namespace Remote.Providers.FileZilla
{

    class LaunchFileZillaAction : SessionAction
    {
        public LaunchFileZillaAction()
            : base("Launch FileZilla")
        {
        }

        public override void Dispatch(Session session)
        {
            FileZillaSessionProvider.LaunchFileZilla(session);
        }
    }

    class FileZillaSessionProvider: SessionProvider
    {
        private static string _filezillaExecutable;

        internal override IEnumerable<Session> GetSessions()
        {
            var fileZillaDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FileZilla");
            var siteManagerXmlPath = Path.Combine(fileZillaDataPath, "sitemanager.xml");
            XDocument siteManagerXml;
            using (var sr = new StreamReader(siteManagerXmlPath))
            {
                siteManagerXml = XDocument.Load(sr);
            }
            foreach (XElement serverNode in siteManagerXml.Descendants("Server"))
            {
                var parentChain = new List<string>();

                foreach (var ancestor in serverNode.Ancestors("Folder"))
                {
                    var xText = ancestor.FirstNode as XText;
                    if (xText == null) break;
                    parentChain.Add(xText.Value.Trim());
                }

                var dataBag = new Dictionary<string, object>();
                foreach (var node in serverNode.Elements())
                {
                    dataBag[node.Name.LocalName] = (string) node;
                }
                if (parentChain.Count > 0)
                {
                    parentChain.Reverse();
                    string path = String.Join("/", parentChain.ToArray());
                    dataBag["Name"] = path + "/" + dataBag["Name"];
                }
                yield return FileZillaSession.FromDataBag(dataBag);
            }
        }

        internal static Process LaunchFileZilla(Session session)
        {
            if (_filezillaExecutable == null)
            {
                throw new Problem("FileZilla could not be located.");
            }
            if (session is FileZillaSession)
            {
                return Process.Start(new ProcessStartInfo
                {
                    FileName = _filezillaExecutable,
                    Arguments = String.Format("--site=\"0/{0}\"", session.Name)
                });
            }
            var ub = session.UriBuilder;
            ub.Scheme = "sftp";
            return Process.Start(new ProcessStartInfo
            {
                FileName = _filezillaExecutable,
                Arguments = ub.Uri.ToString()
            });	
        }

        internal override IEnumerable<SessionAction> GetSessionActions(Session session)
        {
            if (_filezillaExecutable!= null)
            {
                return new[] { new LaunchFileZillaAction() };
            }
            return null;
        }


        public FileZillaSessionProvider()
        {
            _filezillaExecutable = Locator.LocateExecutable("filezilla", new String[]
            {
                "FileZilla FTP Client"
            });
        }
    }
}
