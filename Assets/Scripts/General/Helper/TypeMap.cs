using System;
using System.Collections;
using System.Collections.Generic;


public class TypeMap<D> where D : new ()
{
	private readonly Dictionary<string, D> container;

	public TypeMap ()
	{
		container = new Dictionary<string, D> ();
	}

	public D this [string type]
	{
		get => container.TryGetValue (type);
		set => container.TryAdd (type, value);
	}

	public void erase ()
	{
		container.Remove (typeof(D).Name);
	}

	public void clear ()
	{
		container.Clear ();
	}

	public void add (D d)
	{
		container.TryAdd<string, D> (typeof(D).Name, d);
	}

	public void emplace<F> () where F : D, new ()
	{
		container.TryAdd (typeof(F).Name, new F ());
	}

	public D get<T> ()
	{
		container.TryGetValue (typeof(T).Name, out var d);
		return d;
	}

	public bool TryGetValue<T>(out D d)
	{
		return container.TryGetValue (typeof(T).Name, out d);
	}

	public bool TryGetValue(string key, out D d)
	{
		return container.TryGetValue(key, out d);
	}
	public Dictionary<string, D>.Enumerator GetEnumerator ()
	{
		return container.GetEnumerator ();
	}
}