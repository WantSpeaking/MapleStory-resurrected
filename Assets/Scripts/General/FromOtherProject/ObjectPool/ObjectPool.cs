using System.Collections;
using System.Collections.Generic;
using System;

namespace Utility.PoolSystem
{
	public static class ObjectPool
	{
		static Dictionary<Type, object> typePoolDict = new Dictionary<Type, object> ();

		public static ObjectPool<T> GetPool<T> ()
			where T : class, new()
		{
			Type type = typeof (T);

			object pool;
			if (!typePoolDict.TryGetValue (type, out pool))
			{
				pool = new ObjectPool<T> ();
				typePoolDict.Add (type, pool);
			}
			return (ObjectPool<T>)pool;
		}

		public static PRef<T> Spawn<T> ()
			where T : class, new()
		{
			return GetPool<T> ().Spawn ().ObjRef;
		}


	}

	public class ObjectPool<T> : BasePool<ObjectItem<T>>
		where T : class, new()
	{
		protected override ObjectItem<T> CreatItem ()
		{
			var item = new ObjectItem<T> ();
			return item;
		}

		protected override void DestroyItem (ObjectItem<T> item)
		{
		}

		protected override void OnDespawnItem (ObjectItem<T> item)
		{
		}

		protected override void OnSpawnItem (ObjectItem<T> item)
		{
		}
	}

	public class ObjectItem<T> : IPoolItem<ObjectItem<T>>
		where T : class, new()
	{
		public T Obj { get; private set; }
		public bool IsDespawned { get; private set; } = false;
		public uint DespawnedCount { get; private set; } = 0;
		public ObjectPool<T> Pool { get; private set; }

		public PRef<T> ObjRef
		{
			get { return new PRef<T> (this, DespawnedCount); }
		}

		public void Despawn ()
		{
			Pool.Despawn (this);
		}

		void IPoolItem<ObjectItem<T>>.OnCreateItem (BasePool<ObjectItem<T>> pool)
		{
			Pool = pool as ObjectPool<T>;
			Obj = new T ();
		}

		void IPoolItem<ObjectItem<T>>.OnDestroyItem (BasePool<ObjectItem<T>> pool)
		{
			Obj = null;
		}

		void IPoolItem<ObjectItem<T>>.OnSpawnItem (BasePool<ObjectItem<T>> pool)
		{
			IsDespawned = false;
			if (Obj is IPoolCallback)
			{
				((IPoolCallback)Obj).OnSpawn ();
			}
		}

		void IPoolItem<ObjectItem<T>>.OnDespawnItem (BasePool<ObjectItem<T>> pool)
		{
			if (Obj is IPoolCallback)
			{
				((IPoolCallback)Obj).OnDespawn ();
			}
			IsDespawned = true;
			DespawnedCount++;
		}
	}

	public struct PRef<T> : IEquatable<PRef<T>> where T : class, new()
	{
		ObjectItem<T> obj;
		uint despawnedCount;

		public PRef (ObjectItem<T> obj, uint spawnedCount)
		{
			this.obj = obj;
			this.despawnedCount = spawnedCount;
		}

		public bool IsValid
		{
			get { return obj != null && !obj.IsDespawned && despawnedCount == obj.DespawnedCount; }
		}

		public T Object
		{
			get { return IsValid ? obj.Obj : null; }
		}

		public void Despawn ()
		{
			if (IsValid)
			{
				obj.Despawn ();
			}
		}

		public override bool Equals (object obj)
		{
			return obj is PRef<T> && Equals ((PRef<T>)obj);
		}

		public bool Equals (PRef<T> other)
		{
			if (IsValid)
			{
				return EqualityComparer<ObjectItem<T>>.Default.Equals (obj, other.obj) &&
					   despawnedCount == other.despawnedCount;
			}
			else
			{
				return !other.IsValid;
			}
		}

		public override int GetHashCode ()
		{
			var hashCode = -2060628903;
			hashCode = hashCode * -1521134295 + EqualityComparer<ObjectItem<T>>.Default.GetHashCode (obj);
			hashCode = hashCode * -1521134295 + despawnedCount.GetHashCode ();
			return hashCode;
		}

		public static implicit operator bool (PRef<T> pref)
		{
			return pref.IsValid;
		}

		public static bool operator == (PRef<T> ref1, PRef<T> ref2)
		{
			return ref1.Equals (ref2);
		}

		public static bool operator != (PRef<T> ref1, PRef<T> ref2)
		{
			return !(ref1 == ref2);
		}

		public static implicit operator T (PRef<T> pref)
		{
			return pref.Object;
		}

		public static implicit operator PRef<T>(Null nullObj)
		{
			return default;
		}
	}
}