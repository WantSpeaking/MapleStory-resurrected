using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System.Linq;
using System;

namespace Utility
{
	public static class ListEX
	{
		public enum ClampOption
		{
			None,
			LowerBound,
			UpperBound,
			Both,
		}

		public static bool TryGet<T> (this IList<T> list, int index, out T value, ClampOption clampOption = ClampOption.None)
		{
			value = default;
			if (list.Count == 0)
			{
				return false;
			}
			if (index < 0)
			{
				if (clampOption == ClampOption.Both || clampOption == ClampOption.LowerBound)
				{
					index = 0;
				}
				else
				{
					return false;
				}
			}
			else if (index >= list.Count)
			{
				if (clampOption == ClampOption.Both || clampOption == ClampOption.UpperBound)
				{
					index = list.Count - 1;
				}
				else
				{
					return false;
				}
			}

			value = list[index];
			return true;
		}

		public static T TryGet<T> (this IList<T> list, int index, ClampOption clampOption = ClampOption.None)
		{
			return list.TryGet (index, out T val, clampOption) ? val : default;
		}

		public static bool TryGet<T> (this IEnumerable<T> items, int index, out T value, ClampOption clampOption = ClampOption.None)
		{
			if (items is IList<T> list)
			{
				return list.TryGet (index, out value, clampOption);
			}
			else
			{
				using (var buffer = BufferList<T>.Spawn (items))
				{
					var canGet = buffer.TryGet (index, out value, clampOption);
					return canGet;
				}
			}
		}

		public static T TryGet<T> (this IEnumerable<T> items, int index, ClampOption clampOption = ClampOption.None)
		{
			return items.TryGet (index, out var value, clampOption) ? value : default;
		}

		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue> (this IEnumerable<ValueTuple<TKey, TValue>> list)
		{
			return list?.ToDictionary (g => g.Item1, g => g.Item2);
		}

		public static ReadOnlyCollection<T> ToReadOnly<T> (this IEnumerable<T> list)
		{
			return new ReadOnlyCollection<T> ((list as IList<T>) ?? list.ToArray ());
		}

		public static ReadOnlyDictionary<TKey, ReadOnlyCollection<TValue>> ToReadOnlyDictionary<TKey, TValue> (this IEnumerable<IGrouping<TKey, TValue>> list)
		{
			return list?.ToDictionary (g => g.Key, g => g.ToArray ().ToReadOnly ()).ToReadOnly ();
		}

		public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TItem, TKey, TValue> (this IEnumerable<TItem> list, Func<TItem, TKey> keySelector, Func<TItem, TValue> valueSelector)
		{
			return list?.ToDictionary (g => keySelector (g), g => valueSelector (g)).ToReadOnly ();
		}

		public static IEnumerable<T> NotNull<T> (this IEnumerable<T> list)
			where T : class
		{
			bool _NotNull (T item) => item != null;
			return list.Where (_NotNull);
		}

		public static void ForEach<T> (this IEnumerable<T> source, Action<T> action)
		{
			if (action == null)
			{
				return;
			}

			foreach (var item in source)
			{
				action (item);
			}
		}

		public static int FindIndex<T> (this IList<T> list, T item, bool findFirst = true)
		{
			var comparer = EqualityComparer<T>.Default;
			if (findFirst)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (comparer.Equals (item, list[i]))
					{
						return i;
					}
				}
			}
			else
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (comparer.Equals (item, list[i]))
					{
						return i;
					}
				}
			}
			return -1;
		}

		public static void AddRange<T> (this IList<T> list, IEnumerable<T> items)
		{
			foreach (var item in items)
			{
				list.Add (item);
			}
		}

		public static void Sort<T> (this IList<T> list, IComparer<T> comparer)
		{
			if (list is List<T> _list)
			{
				_list.Sort (comparer);
			}
			else
			{
				using (var buffer = BufferList<T>.Spawn ())
				{
					buffer.Sort (comparer);
					list.Clear ();
					list.AddRange (buffer);
				}
			}
		}

		public static void Sort<T> (this IList<T> list, Comparison<T> comparison)
		{
			if (list is List<T> _list)
			{
				_list.Sort (comparison);
			}
			else
			{
				using (var buffer = BufferList<T>.Spawn ())
				{
					buffer.Sort (comparison);
					list.Clear ();
					list.AddRange (buffer);
				}
			}
		}

		public static void Sort<T> (this IList<T> list)
		{
			if (list is List<T> _list)
			{
				_list.Sort ();
			}
			else
			{
				using (var buffer = BufferList<T>.Spawn ())
				{
					buffer.Sort ();
					list.Clear ();
					list.AddRange (buffer);
				}
			}
		}
	}
}