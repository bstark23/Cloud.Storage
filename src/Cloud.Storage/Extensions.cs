using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Storage
{
	public static class Extensions
	{
		public static string ToStringComparableValue(this Int64 integer)
		{
			var sb = new StringBuilder();
			var integerString = integer.ToString();
			var integerStringLength = integerString.Length;
			for (int i = 0; i < (MaxChars - integerStringLength); ++i)
			{
				sb.Append("0");
			}
			sb.Append(integerString);
			return sb.ToString();
		}

		private static int MaxChars = Int64.MaxValue.ToString().Length;
	}
}
