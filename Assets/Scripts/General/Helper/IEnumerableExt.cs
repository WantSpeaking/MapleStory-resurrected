using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class IEnumerableExt
{
    public static T TryGet<T>(this IList<T> list, int index)
    {
        if (index >= 0 && index < list.Count)
        {
            return list[index];
        }
        else
        {
            return default;
        }
    }

    public static T TryGetNew<T>(this IList<T> list, int index) where T : new()
    {
        if (index >= 0 && index < list.Count)
        {
            if (list[index] == null)
            {
                list[index] = new T();
            }

            return list[index];
        }
        else
        {
            return default;
        }
    }
    public static T TryAdd<T>(this IList<T> list, int index) where T : new()
    {
        while (index >= list.Count)
        {
            list.Add(new T());
        }
        return list[index];
    }
    public static T TryAdd<T>(this IList<T> list, int index, T value) where T : new()
    {
        while (index >= list.Count)
        {
            list.Add(new T());
        }
        list[index] = value;
        return value;
    }

    #region Dictionary

    public static IDictionary<T, V> TryAdd<T, V>(this IDictionary<T, V> dictionary, T key, bool refreshValue = false) where V : new()
    {
        V value;
        if (dictionary.ContainsKey(key))
        {
            if (refreshValue)
            {
                value = new V();
                dictionary[key] = value;
            }
        }
        else
        {
            value = new V();
            dictionary.Add(key, value);
        }

        return dictionary;
    }

    public static void TryAdd<T, V>(this IDictionary<T, V> dictionary, T key, V value, bool refreshValue = false)
    {
        if (dictionary.ContainsKey(key))
        {
            if (refreshValue)
            {
                dictionary[key] = value;
            }
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

    public static void TryAdd<T, V> (this SortedDictionary<T, V> dictionary, T key, V value, bool refreshValue = false)
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

    public static V TryGetValue<T, V>(this IDictionary<T, V> dictionary, T key)
    {
        dictionary.TryGetValue(key, out var value);
        return value;
    }

    public static int count<T, V>(this IDictionary<T, V> dictionary, T key)
    {
        int result = 0;
        foreach (var pair in dictionary)
        {
            if (Equals(pair.Key, key))
            {
                result++;
            }
        }

        return result;
    }

    public static int count<T>(this IEnumerable<T> source, Func<T, bool> predict)
    {
        int result = 0;
        foreach (var s in source)
        {
            if (predict?.Invoke(s) ?? false)
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

    public static void remove_if<T>(this LinkedList<T> list, Func<T, bool> predict)
    {
        var results = list.Where(predict).ToArray();
        foreach (var result in results)
        {
            list.Remove(result);
        }
    }

    #endregion

    private static readonly StringBuilder _stringBuilder = new StringBuilder();

    public static string ToDebugLog(this IEnumerable enumerable)
    {
        _stringBuilder.Clear();
        _stringBuilder.Append('[');
        //_stringBuilder.AppendLine ("");

        foreach (var e in enumerable)
        {
            _stringBuilder.Append(e);
            _stringBuilder.Append(',');
            //_stringBuilder.AppendLine ("");
        }

        _stringBuilder.Append(']');
        return _stringBuilder.ToString();
    }

    public static string ToDebugLog<T, V>(this IDictionary<T, V> enumerable)
    {
        _stringBuilder.Clear();
        _stringBuilder.Append("[");
        foreach (var e in enumerable)
        {
            _stringBuilder.Append("(");

            _stringBuilder.Append(e.Key);
            _stringBuilder.Append(",");
            _stringBuilder.Append(e.Value);

            _stringBuilder.Append(")");

            _stringBuilder.Append(",");
            _stringBuilder.AppendLine ("");
        }

        _stringBuilder.Append("]");
        return _stringBuilder.ToString();
    }
}