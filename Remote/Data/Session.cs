using System;
using System.Collections.Generic;
using System.Text;

namespace Remote.Data
{
    public class SessionDataBag : Dictionary<String, object>
    {
        public void Update(Dictionary<String, object> other)
        {
            foreach (var item in other)
            {
                this[item.Key] = item.Value;
            }
        }
    }


    public abstract class Session
    {
        public virtual string ProgramName
        {
            get { return ""; }
        }

        public virtual string ProgramAbbrev
        {
            get { return "*"; }
        }

        protected readonly SessionDataBag DataBag = new SessionDataBag();

        public virtual string HostName
        {
            get
            {
                if (DataBag.ContainsKey("HostName")) return DataBag["HostName"] as string;
                if (DataBag.ContainsKey("Host")) return DataBag["Host"] as string;
                throw new KeyNotFoundException("HostName missing");
            }
        }

        public virtual int Port
        {
            get
            {
                if (DataBag.ContainsKey("PortNumber")) return Convert.ToInt32(DataBag["PortNumber"]);
                if (DataBag.ContainsKey("Port")) return Convert.ToInt32(DataBag["Port"]);
                throw new KeyNotFoundException("Port missing");
            }
        }

        public virtual string Name
        {
            get
            {
                if (DataBag.ContainsKey("Name"))
                {
                    return DataBag["Name"] as string;
                }
                return "Unnamed " + GetType();
            }
        }

        public virtual string DisplayName
        {
            get { return Uri.UnescapeDataString(Name); }
        }

        public virtual string UserName
        {
            get
            {
                if (DataBag.ContainsKey("UserName")) return DataBag["UserName"] as string;
                if (DataBag.ContainsKey("User")) return DataBag["User"] as string;
                throw new KeyNotFoundException("UserName missing");
            }
        }

        public virtual bool ShowInList
        {
            get { return Name != "Default Settings"; }
        }

        public virtual string ToolTipText
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendFormat("{0} Session\n", ProgramName);
                sb.AppendFormat("Host: {0}\n", HostName);
                sb.AppendFormat("Port: {0}\n", Port);
                sb.AppendFormat("User: {0}\n", UserName);
                return sb.ToString();
            }
        }

        public virtual UriBuilder UriBuilder
        {
            get
            {
                var ub = new UriBuilder
                {
                    Host = HostName,
                    Port = Port
                };
                if (!String.IsNullOrEmpty(UserName))
                {
                    ub.UserName = UserName;
                }
                return ub;
            }
        }


        public virtual void Launch()
        {
            throw new NotImplementedException(String.Format("{0}.Launch() not implemented.", GetType()));
        }

        public static T FromDataBag<T>(Dictionary<string, object> dataBag) where T : Session, new()
        {
            var session = new T();
            session.DataBag.Update(dataBag);
            return session;
        }
    }
}