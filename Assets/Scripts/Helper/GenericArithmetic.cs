using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ms.Helper;

namespace Helper
{
	public static class GenericArithmetic
	{
		private static readonly Dictionary<(Type Type, string Op), Delegate> Cache =
			new Dictionary<(Type Type, string Op), Delegate> ();

		public static T Add<T> (T left, T right) where T : unmanaged
		{
			var t = typeof (T);
			// If op is cached by type and function name, use cached version
			if (Cache.TryGetValue ((t, nameof (Add)), out var del))
				return del is Func<T, T, T> specificFunc
					? specificFunc (left, right)
					: throw new InvalidOperationException (nameof (Add));

			var leftPar = Expression.Parameter (t, nameof (left));
			var rightPar = Expression.Parameter (t, nameof (right));
			var body = Expression.Add (leftPar, rightPar);
			var func = Expression.Lambda<Func<T, T, T>> (body, leftPar, rightPar).Compile ();

			Cache[(t, nameof (Add))] = func;

			return func (left, right);
		}

		public static T Subtract<T> (T left, T right) where T : unmanaged
		{
			var t = typeof (T);
			// If op is cached by type and function name, use cached version
			if (Cache.TryGetValue ((t, nameof (Subtract)), out var del))
				return del is Func<T, T, T> specificFunc
					? specificFunc (left, right)
					: throw new InvalidOperationException (nameof (Subtract));

			var leftPar = Expression.Parameter (t, nameof (left));
			var rightPar = Expression.Parameter (t, nameof (right));
			var body = Expression.Subtract (leftPar, rightPar);

			var func = Expression.Lambda<Func<T, T, T>> (body, leftPar, rightPar).Compile ();

			Cache[(t, nameof (Subtract))] = func;

			return func (left, right);
		}

		/*public static T SubtractSpecific<T>(T left, float right) where T : unmanaged
		{
			var t = typeof(T);
			// If op is cached by type and function name, use cached version
			if (Cache.TryGetValue((t, nameof(DivideSpecific)), out var del))
				return del is Func<T, float, T> specificFunc
					? specificFunc(left, right)
					: throw new InvalidOperationException(nameof(DivideSpecific));

			var leftPar = System.Linq.Expressions.Expression.Parameter(t, nameof(left));
			var rightPar = System.Linq.Expressions.Expression.Parameter(t, nameof(right));
			var body = System.Linq.Expressions.Expression.Divide(leftPar, rightPar);

			var func = System.Linq.Expressions.Expression.Lambda<Func<T, float, T>>(body, leftPar, rightPar).Compile();

			Cache[(t, nameof(DivideSpecific))] = func;

			return func(left, right);
		}*/

		public static T Multiply<T> (T left, T right) where T : unmanaged
		{
			var t = typeof (T);
			// If op is cached by type and function name, use cached version
			if (Cache.TryGetValue ((t, nameof (Divide)), out var del))
				return del is Func<T, T, T> specificFunc
					? specificFunc (left, right)
					: throw new InvalidOperationException (nameof (Multiply));

			var leftPar = Expression.Parameter (t, nameof (left));
			var rightPar = Expression.Parameter (t, nameof (right));
			var body = Expression.Multiply (leftPar, rightPar);

			var func = Expression.Lambda<Func<T, T, T>> (body, leftPar, rightPar).Compile ();

			Cache[(t, nameof (Multiply))] = func;

			return func (left, right);
		}

		public static T MultiplySpecific<T> (T left, float right) where T : unmanaged
		{
			var type_left = typeof (T);
			var type_Right = typeof (float);
			// If op is cached by type and function name, use cached version
			if (Cache.TryGetValue ((type_left, nameof (MultiplySpecific)), out var del))
				return del is Func<T, float, T> specificFunc
					? specificFunc (left, right)
					: throw new InvalidOperationException (nameof (MultiplySpecific));

			var leftPar = Expression.Parameter (type_left, nameof (left));
			var rightPar = Expression.Parameter (type_Right, nameof (right));
			var body = Expression.Multiply (leftPar, rightPar);

			var func = Expression.Lambda<Func<T, float, T>> (body, leftPar, rightPar).Compile ();

			Cache[(type_left, nameof (MultiplySpecific))] = func;

			return func (left, right);
		}

		public static T Divide<T> (T left, T right) where T : unmanaged
		{
			var t = typeof (T);
			// If op is cached by type and function name, use cached version
			if (Cache.TryGetValue ((t, nameof (Divide)), out var del))
				return del is Func<T, T, T> specificFunc
					? specificFunc (left, right)
					: throw new InvalidOperationException (nameof (Divide));

			var leftPar = Expression.Parameter (t, nameof (left));
			var rightPar = Expression.Parameter (t, nameof (right));
			var body = Expression.Divide (leftPar, rightPar);

			var func = Expression.Lambda<Func<T, T, T>> (body, leftPar, rightPar).Compile ();

			Cache[(t, nameof (Divide))] = func;

			return func (left, right);
		}

		public static T DivideSpecific<T> (T left, float right) where T : unmanaged
		{
			var type_left = typeof (T);
			var type_Right = typeof (float);
			// If op is cached by type and function name, use cached version
			if (Cache.TryGetValue ((type_left, nameof (DivideSpecific)), out var del))
				return del is Func<T, float, T> specificFunc
					? specificFunc (left, right)
					: throw new InvalidOperationException (nameof (DivideSpecific));

			var leftPar = Expression.Parameter (type_left, nameof (left));
			var rightPar = Expression.Parameter (type_Right, nameof (right));
			var body = Expression.Divide (leftPar, rightPar);

			var func = Expression.Lambda<Func<T, float, T>> (body, leftPar, rightPar).Compile ();

			Cache[(type_left, nameof (DivideSpecific))] = func;

			return func (left, right);
		}

		public static bool GreaterThan<T> (T left, T right) where T : unmanaged
		{
			var t = typeof (T);
			// If op is cached by type and function name, use cached version
			if (Cache.TryGetValue ((t, nameof (GreaterThan)), out var del))
				return del is Func<T, T, bool> specificFunc
					? specificFunc (left, right)
					: throw new InvalidOperationException (nameof (GreaterThan));

			var leftPar = Expression.Parameter (t, nameof (left));
			var rightPar = Expression.Parameter (t, nameof (right));
			var body = Expression.GreaterThan (leftPar, rightPar);

			var func = Expression.Lambda<Func<T, T, bool>> (body, leftPar, rightPar).Compile ();

			Cache[(t, nameof (GreaterThan))] = func;

			return func (left, right);
		}

		public static bool GreaterThanOrEqual<T> (T left, T right) where T : unmanaged
		{
			var t = typeof (T);
			// If op is cached by type and function name, use cached version
			if (Cache.TryGetValue ((t, nameof (GreaterThanOrEqual)), out var del))
				return del is Func<T, T, bool> specificFunc
					? specificFunc (left, right)
					: throw new InvalidOperationException (nameof (GreaterThanOrEqual));

			var leftPar = Expression.Parameter (t, nameof (left));
			var rightPar = Expression.Parameter (t, nameof (right));
			var body = Expression.GreaterThanOrEqual (leftPar, rightPar);

			var func = Expression.Lambda<Func<T, T, bool>> (body, leftPar, rightPar).Compile ();

			Cache[(t, nameof (GreaterThanOrEqual))] = func;

			return func (left, right);
		}

		public static bool Equal<T> (T left, T right) where T : unmanaged
		{
			var t = typeof (T);
			// If op is cached by type and function name, use cached version
			if (Cache.TryGetValue ((t, nameof (Equal)), out var del))
				return del is Func<T, T, bool> specificFunc
					? specificFunc (left, right)
					: throw new InvalidOperationException (nameof (Equal));

			var leftPar = Expression.Parameter (t, nameof (left));
			var rightPar = Expression.Parameter (t, nameof (right));
			var body = Expression.Equal (leftPar, rightPar);

			var func = Expression.Lambda<Func<T, T, bool>> (body, leftPar, rightPar).Compile ();

			Cache[(t, nameof (Equal))] = func;

			return func (left, right);
		}

		public static T Abs<T> (T a) where T : unmanaged
		{
			return !GreaterThanOrEqual (a, (0).ToT<T> ()) ? Multiply (a, (-1).ToT<T> ()) : a;
		}
		
		public static T Sqrt<T> (T left, T right) where T : unmanaged
		{
			var t = typeof (T);
			// If op is cached by type and function name, use cached version
			if (Cache.TryGetValue ((t, nameof (Sqrt)), out var del))
				return del is Func<T, T, T> specificFunc
					? specificFunc (left, right)
					: throw new InvalidOperationException (nameof (Sqrt));
			var xParameter = Expression.Parameter (t, nameof (left));
			var yParameter = Expression.Parameter (t, nameof (right));
			var xSquared = Expression.Multiply(xParameter, xParameter);
			var ySquared = Expression.Multiply(yParameter, yParameter);
			var sum = Expression.Add(xSquared, ySquared);
			var sqrtMethod = typeof(Math).GetMethod("Sqrt", new[] { typeof(double) });
			var distance = Expression.Call(sqrtMethod, sum);
			
			var func = Expression.Lambda<Func<T, T, T>> (distance, xParameter, yParameter).Compile ();

			Cache[(t, nameof (Sqrt))] = func;

			return func (left, right);
		}
	}
}