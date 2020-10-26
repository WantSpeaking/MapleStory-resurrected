using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ms;
using System.Linq.Expressions;
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

		public static Point<short> ToMSPoint (this System.Drawing.Point src)
		{
			return new Point<short> ((short)src.X, (short)src.Y);
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
	}
}