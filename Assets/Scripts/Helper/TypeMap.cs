using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeMap<D> where D : new ()
{
	private readonly Dictionary<System.Type, D> container;

	public TypeMap ()
	{
		container = new Dictionary<Type, D> ();
	}

	public D this [Type type]
	{
		get => container.TryGetValue (type);
		set => container.TryAdd (type, value);
	}

	public void erase ()
	{
		container.Remove (typeof (D));
	}

	public void clear ()
	{
		container.Clear ();
	}

	public void add (D d)
	{
		container.TryAdd<Type, D> (typeof (D), d);
	}

	public void emplace<F> () where F : D, new ()
	{
		container.TryAdd (typeof (F), new F ());
	}

	public D get<T> ()
	{
		container.TryGetValue (typeof (T), out var d);
		return d;
	}


	public Dictionary<Type, D>.Enumerator GetEnumerator ()
	{
		return container.GetEnumerator ();
	}
}