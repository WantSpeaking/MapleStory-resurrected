using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ms;
using System.Linq.Expressions;
using MapleLib.WzLib;
using Expression = System.Linq.Expressions.Expression;

namespace ms.Helper
{
	public static class TypeExt
	{
		public static bool ToBool (this int src)
		{
			return src == 1;
		}

		public static byte ToByte<T> (this T src)
		{
			Byte.TryParse (src.ToString (), out var result);
			return result;
		}

		public static sbyte ToSByte<T> (this T src)
		{
			SByte.TryParse (src.ToString (), out var result);
			return result;
		}
		
		public static ushort ToUshort<T> (this T src)
		{
			ushort.TryParse (src.ToString (), out var result);
			return result;
		}

		public static Point_short ToMSPoint (this Point src)
		{
			return new Point_short ((short)src.X, (short)src.Y);
		}


		private static readonly Dictionary<object, object> Cache =
			new Dictionary<object, object> ();

		public static T ToT<T> (this object srcValue)
		{
			if (!Cache.TryGetValue (srcValue, out var resultValue))
			{
				resultValue = Convert.ChangeType (srcValue, typeof (T));
			}

			return (T)resultValue;
		}

		public static sbyte[] ToSbyteArray (this byte[] src)
		{
			sbyte[] tar = new sbyte[src.Length];
			for (int i = 0; i < src.Length; i++)
			{
				if (src[i]>=128)
				{
					tar[i] = (sbyte)(src[i] - 256);
				}
				tar[i] = (sbyte)(src[i]);
			}

			return tar;
		}
		
		public static byte[] ToByteArray (this sbyte[] src)
		{
			byte[] tar = new byte[src.Length];
			for (int i = 0; i < src.Length; i++)
			{
				if (src[i]<0)
				{
					tar[i] = (byte)(src[i] + 256);
				}
				else
				{
					tar[i] = (byte)(src[i]);
				}
			}

			return tar;
		}
	}
}