using System;

namespace Helper
{
	public class EnumUtil
	{
		public static int GetEnumLength<T> () where T : struct, IConvertible
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("T must be an enum type");
			}

			return Enum.GetNames (typeof (T)).Length;
		}
	}
}