using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SOLIDplate.Common.Portable
{
	public static class StringExtensions
	{
		public static string SplitCamelCase(this string str)
		{
			return Regex.Replace(Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2").Replace("  ", " ").Trim();
		}

		public static string ToCamelCase(this string str)
		{
			return string.Format("{0}{1}", str.Substring(0, 1).ToLower(), str.Substring(1)).Replace("  ", " ").Trim();
		}

		public static void RemoveAll<T>(this ICollection<T> collection, Func<T, bool> predicate)
		{
			for (var i = 0; i < collection.Count; i++)
			{
				var element = collection.ElementAt(i);
				if (predicate(element))
				{
					collection.Remove(element);
					if (i >= 0)
					{
						i--;
					}
				}
			}
		}
	}
}
