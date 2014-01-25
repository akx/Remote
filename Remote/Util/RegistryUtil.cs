using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace Remote.Util
{
    public static class RegistryUtil
	{
	    public static IEnumerable<string> EnumerateSubkeys(RegistryKey rootKey, string subkeyPath) {
			var subkey = rootKey.OpenSubKey(subkeyPath);
			if (subkey != null) {
				using (subkey) {
					foreach (var keyName in subkey.GetSubKeyNames()) {
						yield return subkeyPath + "\\" + keyName;
					}
				}
			}
		}

	    public static Dictionary<string, object> GetDataBag(RegistryKey rootKey, string subkeyPath) {
			var key = rootKey.OpenSubKey(subkeyPath);
			if (key == null) {
				return null;
			}
			var bag = new Dictionary<string, object>();
			using (key) {
				bag.Clear();
				bag["Name"] = key.Name.Split('\\').Last();
				foreach (var valueName in key.GetValueNames()) {
					var value = key.GetValue(valueName);
					if(valueName.Equals("password", StringComparison.InvariantCultureIgnoreCase)) {
						// XXX: Don't store passwords in memory
					}
					bag[valueName] = value;
				}
			}
			return bag;
		}
	}
}
