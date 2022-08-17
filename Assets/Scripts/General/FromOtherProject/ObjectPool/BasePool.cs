using System.Collections;
using System.Collections.Generic;

using System;

namespace Utility.PoolSystem
{
	public interface IPoolItem<T> where T : class, IPoolItem<T>
	{
		void OnSpawnItem (BasePool<T> pool);
		void OnDespawnItem (BasePool<T> pool);

		void OnCreateItem (BasePool<T> pool);
		void OnDestroyItem (BasePool<T> pool);
	}

	public interface IPoolCallback
	{
		void OnSpawn ();
		void OnDespawn ();
	}

	public interface IPoolCallbackFull : IPoolCallback
	{
		void OnCreate ();
		void OnDestroy ();
	}

	public abstract class BasePool<T> where T : class, IPoolItem<T>
	{
		Queue<T> poolItemQueue = new Queue<T> ();

		public int PoolSize { get; set; } = -1;

		public int PooledCount { get { return poolItemQueue.Count; } }

		public bool IsFull
		{
			get
			{
				return PooledCount < 0 ? false : PooledCount < PoolSize;
			}
		}

		public virtual T Spawn (Action<T> initFunc = null)
		{
			T item;
			if (PooledCount > 0)
			{
				//dequeue
				item = poolItemQueue.Dequeue ();
			}
			else
			{
				//create new
				item = CreatItem ();
				item.OnCreateItem (this);
			}
			initFunc?.Invoke (item);
			OnSpawnItem (item);
			item.OnSpawnItem (this);
			return item;
		}

		public virtual void Despawn (T item)
		{
			if (IsFull)
			{
				//destroy
				item.OnDespawnItem (this);
				item.OnDestroyItem (this);
				DestroyItem (item);
			}
			else
			{
				//pooled				
				item.OnDespawnItem (this);
				OnDespawnItem (item);
				poolItemQueue.Enqueue (item);
			}
		}

		protected abstract T CreatItem ();
		protected abstract void DestroyItem (T item);
		protected abstract void OnSpawnItem (T item);
		protected abstract void OnDespawnItem (T item);
	}

}