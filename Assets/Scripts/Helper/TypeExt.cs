using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ms;
using System.Linq.Expressions;
using Expression = System.Linq.Expressions.Expression;

namespace Assets.ms.Helper
{
	public static class TypeExt
	{
		public static bool ToBool (this int src)
		{
			return src == 1;
		}

		public static Byte ToByte<T> (this T src)
		{
			Byte.TryParse (src.ToString (), out var result);
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

		private static readonly Dictionary<(Type Type, string Op), Delegate> Cache =
			new Dictionary<(Type Type, string Op), Delegate>();

		public static T Add<T>(T left, T right) where T : unmanaged
		{
			var t = typeof(T);
			// If op is cached by type and function name, use cached version
			if (Cache.TryGetValue((t, nameof(Add)), out var del))
				return del is Func<T, T, T> specificFunc
					? specificFunc(left, right)
					: throw new InvalidOperationException(nameof(Add));

			var leftPar = Expression.Parameter(t, nameof(left));
			var rightPar = Expression.Parameter(t, nameof(right));
			var body = Expression.Add(leftPar, rightPar);

			var func = Expression.Lambda<Func<T, T, T>>(body, leftPar, rightPar).Compile();

			Cache[(t, nameof(Add))] = func;

			return func(left, right);
		}
		
		public static T Subtract<T>(T left, T right) where T : unmanaged
		{
			var t = typeof(T);
			// If op is cached by type and function name, use cached version
			if (Cache.TryGetValue((t, nameof(Subtract)), out var del))
				return del is Func<T, T, T> specificFunc
					? specificFunc(left, right)
					: throw new InvalidOperationException(nameof(Subtract));

			var leftPar = Expression.Parameter(t, nameof(left));
			var rightPar = Expression.Parameter(t, nameof(right));
			var body = Expression.Subtract(leftPar, rightPar);

			var func = Expression.Lambda<Func<T, T, T>>(body, leftPar, rightPar).Compile();

			Cache[(t, nameof(Subtract))] = func;

			return func(left, right);
		}
		
		/*public static T Add<T>(this T left, T right) where T : unmanaged
		{
			var t = typeof(T);
			// If op is cached by type and function name, use cached version
			if (Cache.TryGetValue((t, nameof(Add)), out var del))
				return del is Func<T, T, T> specificFunc
					? specificFunc(left, right)
					: throw new InvalidOperationException(nameof(Add));

			var leftPar = Expression.Parameter(t, nameof(left));
			var rightPar = Expression.Parameter(t, nameof(right));
			var body = Expression.Add(leftPar, rightPar);

			var func = Expression.Lambda<Func<T, T, T>>(body, leftPar, rightPar).Compile();

			Cache[(t, nameof(Add))] = func;

			return func(left, right);
		}*/
	}
}