using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

		/// <summary>
		///  把字符串按照指定长度分割
		/// </summary>
		/// <param name="txtString">字符串</param>
		/// <param name="charNumber">长度</param>
		/// <returns></returns>
		public static List<string> GetSeparateSubString (this string txtString, int charNumber)
		{
			var arrlist = new List<string>();
			string tempStr = txtString;
			for (int i = 0; i < tempStr.Length; i += charNumber)
			{
				if ((tempStr.Length - i) > charNumber)//如果是，就截取
				{
					arrlist.Add (tempStr.Substring (i, charNumber));
				}
				else
				{
					arrlist.Add (tempStr.Substring (i));//如果不是，就截取最后剩下的那部分
				}
			}
			return arrlist;
			//假设txtString为"我的未来不是梦",charNumber为2
			//那么返回的是["我的"，"未来","不是"，"梦"]
		}

        /// <summary>
        /// String 扩充方法（用正则表达式分割字符串）
        /// </summary>
        /// <param name="target">目标字符串</param>
        /// <param name="regex">正则表达式</param>
        /// <param name="isIncludeMatch">是否包括匹配结果</param>
        /// <returns>数组</returns>
        public static (string[],string[]) Split(this string target, Regex regex, bool isIncludeMatch = true)
        {
            List<string> list = new List<string>();
            List<string> matchList = new List<string>();
            MatchCollection mc = regex.Matches(target);
            int curPostion = 0;
            foreach (Match match in mc)
            {
                if (match.Index != curPostion)
                {
                    list.Add(target.Substring(curPostion, match.Index - curPostion));
                }
                curPostion = match.Index + match.Length;
                if (isIncludeMatch)
                {
                    list.Add(match.Value);
                }
                matchList.Add(match.Value);
            }
            if (target.Length > curPostion)
            {
                list.Add(target.Substring(curPostion));
            }
            return (list.ToArray(), matchList.ToArray());
        }

    }
}