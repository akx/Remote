using System;
using System.Collections.Generic;
using System.Text;

namespace Remote.Data {
	internal abstract class Session {
		public virtual string ProgramName {
			get { return ""; }
		}

		protected readonly Dictionary<String, object> DataBag = new Dictionary<string, object>();

		public virtual string HostName {
			get { return DataBag["HostName"] as string; }
		}

		public virtual int Port {
			get { return (int) DataBag["PortNumber"]; }
		}

		public virtual string Name {
			get {
				if (DataBag.ContainsKey("Name")) {
					return DataBag["Name"] as string;
				}
				return "Unnamed " + GetType();
			}
		}

		public virtual string UserName {
			get { return DataBag["UserName"] as string; }
		}

		public virtual bool ShowInList {
			get { return Name != "Default Settings"; }
		}

		public virtual string ToolTipText {
			get {
				var sb = new StringBuilder();
				sb.AppendFormat("Host: {0}\n", HostName);
				sb.AppendFormat("Port: {0}\n", Port);
				sb.AppendFormat("User: {0}\n", UserName);
				return sb.ToString();
			}
		}

		public virtual void Launch() {
			throw new NotImplementedException(String.Format("{0}.Launch() not implemented.", GetType()));
		}
	}
}