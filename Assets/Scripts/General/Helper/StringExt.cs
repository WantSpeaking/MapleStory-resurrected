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
			return $"{src}{appendString}";
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
				bool findInCompare = false;
				foreach (var compareChar in compareString)
				{
					if (srcChar == compareChar)
					{
						findInCompare = true;
						break;
					}
				}

				if (findInCompare == false)
				{
					result = src.IndexOf (srcChar);
				}
			}

			return result;
		}

		public static int end (this string src)
		{
			return src.Length - 1;
		}
		
		/// <summary>
		/// 转成Base64字符串进行比较
		/// </summary>
		/// <param name="b1"></param>
		/// <param name="b2"></param>
		/// <returns></returns>
		public static bool BytesCompare_Base64(byte[] b1, byte[] b2)
		{
			var miniLength = Math.Min (b1.Length, b2.Length);
			for (int i = 0; i < miniLength; i++)
			{
				if (b1[i] != b2[i])
				{
					return false;
				}
			}
			return true;
			AppDebug.Log ($"b1:{Convert.ToBase64String(b1).TrimEnd ('A')}\t b2:{Convert.ToBase64String(b2).TrimEnd ('A')}");
			if (b1 == null || b2 == null) return false;
			if(b1.Length !=b2.Length ) return false;
			return string.Compare(Convert.ToBase64String(b1).TrimEnd ('A'), Convert.ToBase64String(b2).TrimEnd ('M','A','=','='), false) == 0 ? true : false;
		}

		public static bool isInt(this string src)
		{
			return int.TryParse (src, out var result);
		}

		public static bool isFloot (this string src)
		{
			return float.TryParse (src, out var result);
		}

		public static bool isDouble (this string src)
		{
			return double.TryParse (src, out var result);
		}

		public static bool isNumber (this string src)
		{
			return isInt(src) || isFloot (src) || isDouble (src);
		}

	}
}