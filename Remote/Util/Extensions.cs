using System.Collections.Generic;

namespace Remote.Util
{
	static class Extensions
	{
		public static void Update<TKey, TValue>(this Dictionary<TKey, TValue> dict, Dictionary<TKey, TValue> other) {
			foreach(var item in other) {
				dict[item.Key] = item.Value;
			}
		}

	}
}
