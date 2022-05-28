using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.PoolSystem
{
	public class UnityPoolItem : MonoBehaviour, IPoolItem<UnityPoolItem>
	{
		[SerializeField] bool isPrespawn = false;
		[SerializeField] GameObject prefab;
		public GameObject Prefab => prefab;
		public UnityObjectPool Pool { get; protected set; }
		public uint DespawnedCount { get; private set; } = 0; //spawned times
		public bool IsDespawned { get; private set; } = true;

		List<IPoolCallback> listeners = new List<IPoolCallback> ();
		List<IPoolCallbackFull> listeners_full = new List<IPoolCallbackFull> ();

		private Dictionary<Type, Component> cache_Component = new Dictionary<Type, Component> ();

		public T GetCachedComponent<T> () where T : Component
		{
			if (!cache_Component.TryGetValue (typeof (T), out var cachedComponent))
			{
				cachedComponent = GetComponent<T> ();
			}

			return (T)cachedComponent;
		}

		public UPRef PooledRef
		{
			get { return UPRef.New (this, DespawnedCount); }
		}

		[ContextMenu ("Despawn")]
		public void Despawn ()
		{
			if (IsDespawned)
			{
				return;
			}

			Pool.Despawn (this);
		}

		private void Awake ()
		{
			if (isPrespawn)
			{
				isPrespawn = false;
				var pool = PoolManager.GetPool (prefab).Pool;
				OnCreateItem (pool);
				OnSpawnItem (pool);
			}
		}

		private void OnValidate ()
		{
#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				//prefab mode
				if (UnityEditor.PrefabUtility.GetPrefabAssetType (gameObject) == UnityEditor.PrefabAssetType.NotAPrefab)
				{
					prefab = null;
					isPrespawn = false;
				}
				else
				{
					//prefab asset
					if (gameObject.scene == null)
					{
						prefab = null;
						isPrespawn = false;
					}
					//scene object
					else
					{
						GameObject prefab = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource (gameObject);
						this.prefab = prefab;
						isPrespawn = true;
					}
				}
			}
#endif
		}

		#region override

		protected virtual void OnCreateItem (BasePool<UnityPoolItem> pool)
		{
			Pool = (pool as UnityObjectPool.UnityItemPool).unityPool;
			prefab = prefab ?? Pool.prefab;

			Debug.Assert (Pool != null);

			//gameObject.SetActive (false);

			GetComponents (listeners_full);
			foreach (var item in listeners_full)
			{
				item.OnCreate ();
			}

			foreach (var component in gameObject.GetComponents<Component> ())
			{
				cache_Component.TryAdd (component.GetType (), component);
			}
		}

		protected virtual void OnSpawnItem (BasePool<UnityPoolItem> pool)
		{
			IsDespawned = false;
			//GetCachedComponent<Renderer>()//gameObject.SetActive (true);

			GetComponents (listeners);
			foreach (var item in listeners)
			{
				item.OnSpawn ();
			}
		}

		protected virtual void OnDespawnItem (BasePool<UnityPoolItem> pool)
		{
			GetComponents (listeners);
			foreach (var item in listeners)
			{
				item.OnDespawn ();
			}

			IsDespawned = true;
			//gameObject.SetActive (false);
			listeners.Clear ();
			DespawnedCount++;
		}

		protected virtual void OnDestroyItem (BasePool<UnityPoolItem> pool)
		{
			Pool = null;

			GetComponents (listeners_full);
			foreach (var item in listeners_full)
			{
				item.OnDestroy ();
			}
		}

		#endregion

		#region interface

		void IPoolItem<UnityPoolItem>.OnCreateItem (BasePool<UnityPoolItem> pool)
		{
			OnCreateItem (pool);
		}

		void IPoolItem<UnityPoolItem>.OnDespawnItem (BasePool<UnityPoolItem> pool)
		{
			OnDespawnItem (pool);
		}

		void IPoolItem<UnityPoolItem>.OnDestroyItem (BasePool<UnityPoolItem> pool)
		{
			OnDestroyItem (pool);
		}

		void IPoolItem<UnityPoolItem>.OnSpawnItem (BasePool<UnityPoolItem> pool)
		{
			OnSpawnItem (pool);
		}

		#endregion
	}

	public struct UPRef : IEquatable<UPRef>
	{
		public static UPRef Null
		{
			get { return new UPRef (null, 0); }
		}

		public static UPRef New (UnityPoolItem item, uint spawnedCount)
		{
			return new UPRef (item, spawnedCount);
		}

		UnityPoolItem item;
		uint despawnedCount;

		public UPRef (UnityPoolItem item, uint despawnedCount)
		{
			this.item = item;
			this.despawnedCount = despawnedCount;
		}

		public bool IsValid
		{
			get { return item != null && !item.IsDespawned && despawnedCount == item.DespawnedCount; }
		}

		public GameObject GameObject
		{
			get { return IsValid ? item.gameObject : null; }
		}

		public void Despawn ()
		{
			if (IsValid)
			{
				item.Despawn ();
				item = null;
			}
		}

		public UPRef<T> GetComponent<T> () where T : Component
		{
			return new UPRef<T> (item, despawnedCount);
		}

		public override bool Equals (object obj)
		{
			return obj is UPRef && Equals ((UPRef)obj);
		}

		public bool Equals (UPRef other)
		{
			if (IsValid)
			{
				return EqualityComparer<UnityPoolItem>.Default.Equals (item, other.item) &&
				       despawnedCount == other.despawnedCount;
			}
			else
			{
				return !other.IsValid;
			}
		}

		public override int GetHashCode ()
		{
			var hashCode = 885369253;
			hashCode = hashCode * -1521134295 + EqualityComparer<UnityPoolItem>.Default.GetHashCode (item);
			hashCode = hashCode * -1521134295 + despawnedCount.GetHashCode ();
			return hashCode;
		}

		public static implicit operator GameObject (UPRef upref)
		{
			return upref.GameObject;
		}

		public static implicit operator UPRef (Null obj)
		{
			return default;
		}

		public static implicit operator bool (UPRef upref)
		{
			return upref.IsValid;
		}

		public static bool operator == (UPRef ref1, UPRef ref2)
		{
			return ref1.Equals (ref2);
		}

		public static bool operator != (UPRef ref1, UPRef ref2)
		{
			return !(ref1 == ref2);
		}
	}

	public struct UPRef<T> : IEquatable<UPRef<T>> where T : Component
	{
		public static UPRef<T> New (UnityPoolItem item, uint spawnedCount)
		{
			return new UPRef<T> (item, spawnedCount);
		}

		UnityPoolItem item;
		T component;
		uint despawnedCount;

		public UPRef (UnityPoolItem item, uint despawnedCount)
		{
			this.item = item;
			this.despawnedCount = despawnedCount;

			component = item.GetCachedComponent<T> ();
		}

		public bool IsValid
		{
			get { return item != null && !item.IsDespawned && despawnedCount == item.DespawnedCount; }
		}

		public GameObject GameObject
		{
			get { return IsValid ? item.gameObject : null; }
		}

		public T Component
		{
			get
			{
				if (IsValid)
				{
					if (component == null)
					{
						component = item.GetComponent<T> ();
					}

					return component;
				}
				else
				{
					return null;
				}
			}
		}

		public void Despawn ()
		{
			if (IsValid)
			{
				item.Despawn ();
				item = null;
			}
		}

		public override bool Equals (object obj)
		{
			return obj is UPRef<T> && Equals ((UPRef<T>)obj);
		}

		public bool Equals (UPRef<T> other)
		{
			if (IsValid)
			{
				return EqualityComparer<UnityPoolItem>.Default.Equals (item, other.item) &&
				       despawnedCount == other.despawnedCount;
			}
			else
			{
				return !other.IsValid;
			}
		}

		public override int GetHashCode ()
		{
			var hashCode = 885369253;
			hashCode = hashCode * -1521134295 + EqualityComparer<UnityPoolItem>.Default.GetHashCode (item);
			hashCode = hashCode * -1521134295 + despawnedCount.GetHashCode ();
			return hashCode;
		}

		public static implicit operator T (UPRef<T> upref)
		{
			return upref.Component;
		}

		public static implicit operator UPRef (UPRef<T> upref)
		{
			return UPRef.New (upref.item, upref.despawnedCount);
		}

		public static implicit operator UPRef<T> (Null obj)
		{
			return default;
		}

		public static implicit operator bool (UPRef<T> upref)
		{
			return upref.IsValid;
		}

		public static bool operator == (UPRef<T> ref1, UPRef<T> ref2)
		{
			return ref1.Equals (ref2);
		}

		public static bool operator != (UPRef<T> ref1, UPRef<T> ref2)
		{
			return !(ref1 == ref2);
		}
	}
}