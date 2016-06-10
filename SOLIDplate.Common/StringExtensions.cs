using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SOLIDplate.Common
{
	public static class StringExtensions
	{
		public static string ToTitleCase(this string str)
		{
			return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str).Replace("  ", " ").Trim();
		}

		public static IEnumerable<int> ConvertAllToInt(this IEnumerable<string> collection)
		{
			return collection.ToList().ConvertAll(int.Parse);
		}

		public static IEnumerable<DateTime> ConvertAllToDateTime(this IEnumerable<string> collection)
		{
			return collection.ToList().ConvertAll(DateTime.Parse);
		}

		public static IEnumerable<Double> ConvertAllToDouble(this IEnumerable<string> collection)
		{
			return collection.ToList().ConvertAll(Double.Parse);
		}
	}
}