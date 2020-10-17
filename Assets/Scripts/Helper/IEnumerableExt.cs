using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IEnumerableExt
{
	#region Dictionary

	public static void TryAdd<T, V> (this Dictionary<T, V> dictionary, T key, bool refreshValue = false) where V : new ()
	{
		var value = new V ();
		if (dictionary.ContainsKey (key))
		{
			if (refreshValue)
			{
				dictionary[key] = value;
			}
		}
		else
		{
			dictionary.Add (key, value);
		}
	}

	public static void TryAdd<T, V> (this Dictionary<T, V> dictionary, T key, V value, bool refreshValue = false)
	{
		if (dictionary.ContainsKey (key))
		{
			if (refreshValue)
			{
				dictionary[key] = value;
			}
		}
		else
		{
			dictionary.Add (key, value);
		}
	}

	public static T TryGet<T> (this List<T> list, int index)
	{
		if (index>=0&&index<list.Count-1)
		{
			return list[index];
		}
		else
		{
			return default;
		}
	}
	#endregion
}