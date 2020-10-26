using System;

namespace ms
{
	public static class StringExt
	{
		public static string insert (this string src, int _Off, int _Count, char _Ch)
		{
			string insertString = String.Empty;
			for (int i = 0; i < _Count; i++)
			{
				insertString += _Ch;
			}

			return src.Insert (_Off, insertString);
		}

		public static string pop_back (this string src)
		{
			return src.Substring (0, src.Length - 1);
			;
		}

		public static string append (this string src, string appendString)
		{
			return src + appendString;
		}

		public static char back (this string src)
		{
			return src[src.Length - 1];
		}

		public static int find_first_not_of (this string src, string compareString)
		{
			int result = -1;
			foreach (var srcChar in src)
			{
				foreach (var compareChar in compareString)
				{
					if (srcChar == compareChar)
					{
						break;
					}
				}

				result = src.IndexOf (srcChar);
			}

			return result;
		}

		public static int end (this string src)
		{
			return src.Length - 1;
		}
	}
}