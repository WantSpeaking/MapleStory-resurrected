using System.Collections;
using System.Collections.Generic;
//
//using Utility.DataStructure;

namespace Utility
{
	public static class SetEX
	{
		public static HashSet<T> ToSet<T> (this IEnumerable<T> items)
		{
			return new HashSet<T> (items);
		}

		/*public static ReadOnlySet<T> ToReadOnlySet<T> (this IEnumerable<T> items)
		{
			return items.ToSet ().ToReadOnly ();
		}

		public static ReadOnlySet<T> ToReadOnly<T> (this ISet<T> set)
		{
			return new ReadOnlySet<T> (set);
		}*/

		public static void AddRange<T> (this ISet<T> set, IEnumerable<T> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					set.Add (item);
				}
			}
		}
		
		public static void RemoveRange<T> (this ISet<T> set, IEnumerable<T> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					set.Remove (item);
				}
			}
		}
	}
}