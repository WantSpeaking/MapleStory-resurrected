using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Utility.PoolSystem
{
	public interface ISimplePool
	{
		object Spawn ();
		bool Despawn (object obj);
	}

	public interface ISimplePool<T> : ISimplePool
		where T : class
	{
		new T Spawn ();
		bool Despawn (T obj);
	}

	public static class SimplePoolUtility
	{
		public static ISimplePool CreatePool (Type itemType)
		{
			Type poolType = typeof (SimplePool<>).MakeGenericType (itemType);
			var pool = Activator.CreateInstance (poolType);
			return pool as ISimplePool;
		}
	}

	public class SimplePool<T> : ISimplePool<T>
		where T : class
	{
		Queue<T> queue = new Queue<T> ();
		HashSet<T> spawnedObjs = new HashSet<T> ();

		public Func<T> createObj;
		public Action<T> onSpawn;
		public Action<T> onDespawn;

		public SimplePool ()
		{
		}

		public SimplePool (Func<T> creator)
		{
			createObj = creator;
		}

		public T Spawn ()
		{
			var obj = queue.Count > 0 ? queue.Dequeue () : CreateObj ();
			spawnedObjs.Add (obj);
			OnSpawn (obj);
			return obj;
		}

		public bool Despawn (T obj)
		{
			if (obj != null && spawnedObjs.Contains (obj))
			{
				spawnedObjs.Remove (obj);
				queue.Enqueue (obj);
				OnDespawn (obj);
				return true;
			}
			return false;
		}

		protected virtual T CreateObj ()
		{
			return createObj?.Invoke () ?? Activator.CreateInstance<T> ();
		}

		protected virtual void OnSpawn (T obj)
		{
			onSpawn?.Invoke (obj);
			(obj as IPoolCallback)?.OnSpawn ();
		}

		protected virtual void OnDespawn (T obj)
		{
			onDespawn?.Invoke (obj);
			(obj as IPoolCallback)?.OnDespawn ();
		}

		object ISimplePool.Spawn ()
		{
			return Spawn ();
		}

		bool ISimplePool.Despawn (object obj)
		{
			return obj is T t ? Despawn (t) : false;
		}
	}

}