using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System;

namespace Utility
{
	public static class DictionaryEX
	{
		public static ReadOnlyDictionary<TK, TV> ToReadOnly<TK, TV> (this IDictionary<TK, TV> dictionary)
		{
			return new ReadOnlyDictionary<TK, TV> (dictionary);
		}

		public static void RemoveAll<TK, TV> (this IDictionary<TK, TV> dict, Func<TK, TV, bool> predicate)
		{
			using (var buffer = BufferSet<TK>.Spawn ())
			{
				foreach (var pair in dict)
				{
					if (predicate (pair.Key, pair.Value))
					{
						buffer.Add (pair.Key);
					}
				}
				foreach (var key in buffer)
				{
					dict.Remove (key);
				}
			}
		}

		public static TV GetOrNew<TK, TV> (this IDictionary<TK, TV> dict, TK key)
			where TV : new()
		{
			if (!dict.TryGetValue (key, out var val))
			{
				val = new TV ();
				dict[key] = val;
			}
			return val;
		}

		public static TV GetOrNew<TK, TV> (this IDictionary<TK, TV> dict, TK key, Func<TV> itemCreator)
		{
			if (!dict.TryGetValue (key, out var val))
			{
				val = itemCreator ();
				dict[key] = val;
			}
			return val;
		}

		public static TV GetOrNew<TK, TV> (this IDictionary<TK, TV> dict, TK key, Func<TK, TV> itemCreator)
		{
			if (!dict.TryGetValue (key, out var val))
			{
				val = itemCreator (key);
				dict[key] = val;
			}
			return val;
		}

		public static TV TryGetValue<TK, TV> (this IDictionary<TK, TV> dict, TK key)
		{
			return dict.TryGetValue (key, out var val) ? val : default;
		}
	}


}