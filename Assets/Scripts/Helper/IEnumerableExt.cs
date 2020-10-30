using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class IEnumerableExt
{
	public static T TryGet<T> (this List<T> list, int index)
	{
		if (index >= 0 && index < list.Count - 1)
		{
			return list[index];
		}
		else
		{
			return default;
		}
	}

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

	public static V TryGetValue<T, V> (this IDictionary<T, V> dictionary, T key)
	{
		dictionary.TryGetValue (key, out var value);
		return value;
	}

	public static int count<T, V> (this IDictionary<T, V> dictionary, T key)
	{
		int result = 0;
		foreach (var pair in dictionary)
		{
			if (Equals (pair.Key, key))
			{
				result++;
			}
		}

		return result;
	}

	/*public static T RemoveWhere<T,V> (this Dictionary<T,V> list, Func<bool> predict)
	{
		if (index >= 0 && index < list.Count - 1)
		{
			return list[index];
		}
		else
		{
			return default;
		}
	}*/

	#endregion

	#region LinkedList

	public static void remove_if<T> (this LinkedList<T> list, Func<T, bool> predict)
	{
		var results = list.Where (predict).ToArray ();
		foreach (var result in results)
		{
			list.Remove (result);
		}
	}

	#endregion

	private static StringBuilder _stringBuilder = new StringBuilder();
	public static string ToDebugLog (this IEnumerable enumerable)
	{
		_stringBuilder.Clear ();
		_stringBuilder.Append ("[");
		foreach (var e in enumerable)
		{
			_stringBuilder.Append (e);
			_stringBuilder.Append (",");
		}
		_stringBuilder.Append ("]");
		return _stringBuilder.ToString ();
	}
	public static string ToDebugLog<T,V> (this IDictionary<T,V> enumerable)
	{
		_stringBuilder.Clear ();
		_stringBuilder.Append ("[");
		foreach (var e in enumerable)
		{
			_stringBuilder.Append ("(");

			_stringBuilder.Append (e.Key);
			_stringBuilder.Append (",");
			_stringBuilder.Append (e.Value);
			
			_stringBuilder.Append (")");

			_stringBuilder.Append (",");
		}
		_stringBuilder.Append ("]");
		return _stringBuilder.ToString ();
	}
}