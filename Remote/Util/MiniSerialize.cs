using System;
using System.Reflection;
using System.Xml.Linq;

namespace Remote.Util
{
    internal static class MiniSerialize
    {
        private const BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance;

        public static XElement Serialize(object source)
        {
            var type = source.GetType();
            var dest = new XElement(type.Name);
            foreach (var prop in type.GetProperties(Flags))
            {
                var value = prop.GetValue(source, Flags, null, null, null);
                var valEl = new XElement(prop.Name);
                valEl.SetAttributeValue("type", prop.PropertyType.Name);
                valEl.SetAttributeValue("value", Convert.ToString(value));
                dest.Add(valEl);
            }
            return dest;
        }

        public static void Unserialize(XElement source, object dest)
        {
            var type = dest.GetType();
            foreach (var prop in type.GetProperties(Flags))
            {
                var valEl = source.Element(prop.Name);
                if (valEl != null)
                {
                    var valAttr = valEl.Attribute("value");
                    if (valAttr != null)
                    {
                        var valStr = valAttr.Value;
                        var value = Convert.ChangeType(valStr, prop.PropertyType);
                        prop.SetValue(dest, value, Flags, null, null, null);
                    }
                }
            }
        }
    }
}