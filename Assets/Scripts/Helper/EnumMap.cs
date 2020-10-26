using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
	public Dictionary<T, V> values { get; set; }

	public EnumMap ()
	{
		values = new Dictionary<T, V> ();
		Enum.GetValues (typeof (T)).Cast<T> ().ToList ().ForEach (x => values.Add (x, default)); //todo EnumMap V maybe null
	}
	public EnumMap (Dictionary<T, V> values)
	{
		this.values =values ;
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
	/*public static implicit operator EnumMapNew<T, V> (EnumMap<T, V> map)
	{
		return new EnumMapNew<T, V> (map.values);
	}*/
}

public class EnumMapNew<T, V> : IEnumerable<KeyValuePair<T, V>> where V : new ()
{
	public Dictionary<T, V> values { get; set; }

	public EnumMapNew ()
	{
		values = new Dictionary<T, V> ();
		Enum.GetValues (typeof (T)).Cast<T> ().ToList ().ForEach (x => values.Add (x, new V ())); //todo EnumMap V maybe null
	}

	public EnumMapNew (Dictionary<T, V> values)
	{
		this.values =values ;
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