using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Remote.Data
{
    public abstract class SettingsObject
    {
        [Browsable(false)]
        public virtual string Name { get { return GetType().Name.Replace("Settings", ""); } }
        public virtual bool Validate()
        {
            return true;
        }

        public virtual bool ValidateChange(string name, object oldValue, object newValue)
        {
            return true;
        }
    }
}
