using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ms_Unity
{
	public class UnityPool<T>
	{
		/// <summary>
		/// Callback function when a new object is creating.
		/// </summary>
		/// <param name="obj"></param>
		public delegate void InitCallbackDelegate (T obj);

		/// <summary>
		/// Callback function when a new object is creating.
		/// </summary>
		public InitCallbackDelegate initCallback;

		Dictionary<string, Queue<T>> _pool;
		Transform _manager;

		public UnityPool ()
		{
			//_manager = manager;
			_pool = new Dictionary<string, Queue<T>> ();
		}
		/// <summary>
		/// 需要设置一个manager，加入池里的对象都成为这个manager的孩子
		/// </summary>
		/// <param name="manager"></param>
		public UnityPool (Transform manager)
		{
			_manager = manager;
			_pool = new Dictionary<string, Queue<T>> ();
		}

		/// <summary>
		/// Dispose all objects in the pool.
		/// </summary>
		public void Clear (Action<T> destory)
		{
			foreach (KeyValuePair<string, Queue<T>> kv in _pool)
			{
				Queue<T> list = kv.Value;
				foreach (T obj in list)
				{
					if (destory != null)
					{
						destory.Invoke (obj);
					}
				}
				//obj.Dispose ();
			}
			_pool.Clear ();
		}

		/// <summary>
		/// 
		/// </summary>
		public int count
		{
			get { return _pool.Count; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public T GetObject (string url, Func<T> create)
		{
			if (url == null)
				return default (T);

			Queue<T> arr;
			T obj = default (T);
			if (_pool.TryGetValue (url, out arr) && arr.Count > 0)
			{
				obj = arr.Dequeue ();
			}
			else
			{
				if (create != null)
				{
					obj = create.Invoke ();
				}

				if (obj != null)
				{
					if (initCallback != null)
						initCallback (obj);
				}
			}

			return obj;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		public void ReturnObject (string url, T obj)
		{
			if (obj == null)
				return;

			Queue<T> arr;
			if (!_pool.TryGetValue (url, out arr))
			{
				arr = new Queue<T> ();
				_pool.Add (url, arr);
			}

			/*if (_manager != null)
				obj.displayObject.cachedTransform.SetParent (_manager, false);*/
			arr.Enqueue (obj);
		}
	}

}


