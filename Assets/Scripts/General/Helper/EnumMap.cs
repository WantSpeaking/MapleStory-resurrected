using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helper;
using ms.Helper;

public class EnumMap<T>
{
	private Dictionary<T, object> values { get; set; }

	public EnumMap ()
	{
		values = new Dictionary<T, object> ();
		Enum.GetValues (typeof (T)).Cast<T> ().ToList ().ForEach (x => values.Add (x, new object ()));
	}

	public object this [T index]
	{
		get { return values[index]; }
		set { values[index] = value; }
	}
}

public class EnumMap<T, V> : IEnumerable<KeyValuePair<T, V>> /*where V : new()*/
{
	public List<T> keys = new List<T> ();
	public Dictionary<T, V> dict { get; set; }

	public EnumMap ()
	{
		dict = new Dictionary<T, V> ();
		Enum.GetValues (typeof (T)).Cast<T> ().ToList ().ForEach (x => dict.Add (x, default)); //todo 2 EnumMap V maybe null
		keys.AddRange (dict.Keys);
	}

	public EnumMap (Dictionary<T, V> dict)
	{
		this.dict = dict;
	}

	public V this [T index]
	{
		get { return dict[index]; }
		set { dict[index] = value; }
	}

	public IEnumerator<KeyValuePair<T, V>> GetEnumerator ()
	{
		return dict.GetEnumerator ();
	}

	IEnumerator IEnumerable.GetEnumerator ()
	{
		throw new NotImplementedException ();
	}

	public void Clear ()
	{
		dict.Clear ();
	}

	public int Count => dict.Count;
	/*public static implicit operator EnumMapNew<T, V> (EnumMap<T, V> map)
	{
		return new EnumMapNew<T, V> (map.values);
	}*/


	public void SetValue (Func<V> method)
	{
		foreach (var key in keys)
		{
			dict[key] = method.Invoke ();
		}
	}
/*
	public void AddValue (Func<V> method)
	{
		foreach (var key in keys)
		{
			dict[key] = GenericArithmetic.Add (dict[key], method.Invoke ());
		}
	}*/

	public bool TryGetValue (T key, out V value)
	{
		return dict.TryGetValue (key, out value);
	}

	public V TryGetValue (T key)
	{
		dict.TryGetValue (key, out var value);
		return value;
	}

	public void TryAdd (T key, V value, bool refreshValue = false)
	{
		dict.TryAdd (key, value, refreshValue);
	}
}

public class EnumMapNew<T, V> : IEnumerable<KeyValuePair<T, V>> where V : new ()
{
	public Dictionary<T, V> values { get; set; }

	public EnumMapNew ()
	{
		values = new Dictionary<T, V> ();
		Enum.GetValues (typeof (T)).Cast<T> ().ToList ().ForEach (x => values.Add (x, new V ())); //todo 2 EnumMap V maybe null
	}

	public EnumMapNew (Dictionary<T, V> values)
	{
		this.values = values;
	}

	public V this [T index]
	{
		get { return values[index]; }
		set { values[index] = value; }
	}

	public IEnumerator<KeyValuePair<T, V>> GetEnumerator ()
	{
		return values.GetEnumerator ();
	}

	IEnumerator IEnumerable.GetEnumerator ()
	{
		throw new NotImplementedException ();
	}

	public void Clear ()
	{
		values.Clear ();
	}

	public static implicit operator EnumMap<T, V> (EnumMapNew<T, V> map)
	{
		return new EnumMap<T, V> (map.values);
	}
}