using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using System;

namespace Utility.PoolSystem
{
	public class UnityObjectPool : MonoBehaviour
	{
		public class UnityItemPool : BasePool<UnityPoolItem>
		{
			public UnityObjectPool unityPool;

			public UnityItemPool (UnityObjectPool pool)
			{
				unityPool = pool;
			}

			public override void Despawn (UnityPoolItem item)
			{
				if (item.IsDespawned)
				{
					return;
				}

				base.Despawn (item);
			}

			protected override UnityPoolItem CreatItem ()
			{
				var prefab = unityPool.prefab;
				if (prefab == null)
				{
					Debug.LogError ("Prefab is null");
				}

				var obj = Instantiate (prefab);
				var item = obj.GetOrAddComponent<UnityPoolItem> ();
				return item;
			}

			protected override void DestroyItem (UnityPoolItem item)
			{
				Destroy (item);
			}

			protected override void OnDespawnItem (UnityPoolItem item)
			{
			}

			protected override void OnSpawnItem (UnityPoolItem item)
			{
			}
		}

		public GameObject prefab;

		UnityItemPool _pool;

		public UnityItemPool Pool
		{
			get
			{
				if (_pool == null)
				{
					_pool = new UnityItemPool (this);
				}

				return _pool;
			}
		}

		public UPRef Spawn (Action<UnityPoolItem> initFunc = null)
		{
			var item = Pool.Spawn (initFunc);
			return item.PooledRef;
		}

		public void Despawn (UnityPoolItem item, Action<UnityPoolItem> finalFunc = null)
		{
			if (item.Pool != this)
			{
				Debug.LogError ("item does not belong to this pool");
			}

			//item.gameObject.SetActive (false);
			//item.transform.SetParent (transform, false);
			Pool.Despawn (item);
		}

		public void Despawn (GameObject go)
		{
			var item = go.GetComponent<UnityPoolItem> ();
			if (!item)
			{
				Debug.LogError ("this is not a pooled item");
			}

			Despawn (item);
		}
	}
}