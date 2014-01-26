using System;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Remote.Logging
{
    public static class StringInject
    {
        private static readonly Regex AttributeRegex = new Regex("{([a-z0-9_]+)(?:}|(?::(.[^}]*)}))", RegexOptions.IgnoreCase);

        public static string Inject(string formatString, object injectionObject)
        {
            var objType = injectionObject.GetType();
            return formatString.Inject(name => objType.GetProperty(name).GetValue(injectionObject, null));
        }

        public static string Inject(string formatString, IDictionary dictionary)
        {
            return formatString.Inject(name => dictionary[name]);
        }

        public static string Inject(this string formatString, Func<string, object> keyGetter)
        {
            return AttributeRegex.Replace(formatString, delegate(Match match)
            {
                var value = keyGetter(match.Groups[1].ToString());
                var format = match.Groups[2];
                if (format.Length > 0)
                {
                    return string.Format(CultureInfo.CurrentCulture, "{0:" + format + "}", value);
                }
                else
                {
                    return (value ?? string.Empty).ToString();
                }
            });
        }
    }
}