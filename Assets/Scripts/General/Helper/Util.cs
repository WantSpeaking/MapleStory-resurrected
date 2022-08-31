using System;
using System.Reflection.Emit;

namespace ms
{
	public static class Utils
	{
/*		public static T lerp<T> (T first, T second, float alpha)
		{
			return alpha <= 0.0f ? first
				: alpha >= 1.0f ? second
				: (dynamic)first == second ? first
				: (dynamic)(1.0f - alpha) * first + (dynamic)alpha * second;
		}*/

		//public static class Utils
		public static int SizeOf<T> (T obj)
		{
			return SizeOfCache<T>.SizeOf;
		}

		private static class SizeOfCache<T>
		{
			public static readonly int SizeOf;

			static SizeOfCache ()
			{
				var dm = new DynamicMethod ("func", typeof (int),
					Type.EmptyTypes, typeof (Utils));

				ILGenerator il = dm.GetILGenerator ();
				il.Emit (OpCodes.Sizeof, typeof (T));
				il.Emit (OpCodes.Ret);

				var func = (Func<int>)dm.CreateDelegate (typeof (Func<int>));
				SizeOf = func ();
			}
		}
	}
}

