using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using System;

namespace Utility.PoolSystem
{
	public class PoolManager : SingletonMono<PoolManager>
	{
		static Dictionary<GameObject, UnityObjectPool> poolDict = new Dictionary<GameObject, UnityObjectPool> ();

		public static UnityObjectPool GetPool (GameObject prefab)
		{
			UnityObjectPool pool;
			if (!poolDict.TryGetValue (prefab, out pool))
			{
				var go = new GameObject ("Pool_" + prefab.name);
				pool = go.AddComponent<UnityObjectPool> ();
				pool.transform.SetParent (Instance.transform);
				pool.prefab = prefab;

				poolDict.Add (prefab, pool);
			}

			return pool;
		}

		public static UPRef Spawn (GameObject prefab, Action<UnityPoolItem> initFunc = null)
		{
			if (prefab == null)
			{
				return UPRef.Null;
			}

			var pool = GetPool (prefab);
			var objRef = pool.Spawn (initFunc);
			return objRef;
		}

		public static void Despawn (GameObject go)
		{
			var item = go.GetComponent<UnityPoolItem> ();
			if (!item)
			{
				Debug.LogError ("this gameobject is not a pool item: " + go.name);
			}

			item.Pool.Despawn (item);
		}

		public static void Despawn (UnityPoolItem item, Action<UnityPoolItem> finalFunc = null)
		{
			if (!item)
			{
				Debug.LogError ($"Despawn item is null");
			}

			item.Pool.Despawn (item, finalFunc);
		}
	}
}