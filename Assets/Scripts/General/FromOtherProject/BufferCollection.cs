using System.Collections;
using System.Collections.Generic;
using System;
using Utility.PoolSystem;

namespace Utility
{
	public class BufferList<T> : List<T>, IDisposable
	{
		static Dictionary<Type, object> pools = new Dictionary<Type, object> ();

		static SimplePool<BufferList<T>> GetPool ()
		{
			return (SimplePool<BufferList<T>>)pools.GetOrNew (typeof (BufferList<T>), CreatePool);

			object CreatePool ()
			{
				var pool = new SimplePool<BufferList<T>> ();
				pool.createObj = () => new BufferList<T> ();
				pool.onSpawn = list => list.Clear ();
				pool.onDespawn = list => list.Clear ();
				return pool;
			}
		}

		public static BufferList<T> Spawn ()
		{
			return GetPool ().Spawn ();
		}

		public static BufferList<T> Spawn (params T[] source)
		{
			var list = Spawn ();
			list.AddRange (source);
			return list;
		}

		public static BufferList<T> Spawn (IEnumerable<T> source)
		{
			var list = Spawn ();
			list.AddRange (source);
			return list;
		}

		public static void Despawn (BufferList<T> list)
		{
			GetPool ().Despawn (list);
		}

		public void Dispose ()
		{
			Despawn (this);
		}

		protected BufferList () { }
	}

	public class BufferList2D<T> : List<BufferList<T>>, IDisposable
	{
		static Dictionary<Type, object> pools = new Dictionary<Type, object> ();

		static SimplePool<BufferList2D<T>> GetPool ()
		{
			return (SimplePool<BufferList2D<T>>)pools.GetOrNew (typeof (BufferList2D<T>), CreatePool);

			object CreatePool ()
			{
				var pool = new SimplePool<BufferList2D<T>> ();
				pool.createObj = () => new BufferList2D<T> ();
				pool.onSpawn = list => list.Clear ();
				pool.onDespawn = list => list.Clear ();
				return pool;
			}
		}

		public static BufferList2D<T> Spawn ()
		{
			return GetPool ().Spawn ();
		}

		public static BufferList2D<T> Spawn (int count)
		{
			var list = Spawn ();
			for (int i = 0; i < count; i++)
			{
				list.AddList ();
			}
			return list;
		}

		public static void Despawn (BufferList2D<T> list)
		{
			foreach (var list_i in list)
			{
				list_i.Dispose ();
			}
			GetPool ().Despawn (list);
		}

		public void Dispose ()
		{
			Despawn (this);
		}

		public BufferList<T> AddList ()
		{
			var list = BufferList<T>.Spawn ();
			Add (list);
			return list;
		}

		public BufferList<T> AddList (IEnumerable<T> items)
		{
			var list = BufferList<T>.Spawn (items);
			Add (list);
			return list;
		}

		protected BufferList2D () { }
	}


	public class BufferSet<T> : HashSet<T>, IDisposable
	{
		static Dictionary<Type, object> pools = new Dictionary<Type, object> ();

		static SimplePool<BufferSet<T>> GetPool ()
		{
			return (SimplePool<BufferSet<T>>)pools.GetOrNew (typeof (BufferSet<T>), CreatePool);

			object CreatePool ()
			{
				var pool = new SimplePool<BufferSet<T>> ();
				pool.createObj = () => new BufferSet<T> ();
				pool.onSpawn = set => set.Clear ();
				pool.onDespawn = set => set.Clear ();
				return pool;
			}
		}

		public static BufferSet<T> Spawn ()
		{
			return GetPool ().Spawn ();
		}

		public static BufferSet<T> Spawn (IEnumerable<T> source)
		{
			var set = Spawn ();
			set.AddRange (source);
			return set;
		}

		public static void Despawn (BufferSet<T> set)
		{
			GetPool ().Despawn (set);
		}

		public void Dispose ()
		{
			Despawn (this);
		}

		protected BufferSet () { }
	}

	public class BufferDict<TK, TV> : Dictionary<TK, TV>, IDisposable
	{
		static Dictionary<Type, object> pools = new Dictionary<Type, object> ();

		static SimplePool<BufferDict<TK, TV>> GetPool ()
		{
			return (SimplePool<BufferDict<TK, TV>>)pools.GetOrNew (typeof (BufferDict<TK, TV>), CreatePool);

			object CreatePool ()
			{
				var pool = new SimplePool<BufferDict<TK, TV>> ();
				pool.createObj = () => new BufferDict<TK, TV> ();
				pool.onSpawn = dict => dict.Clear ();
				pool.onDespawn = dict => dict.Clear ();
				return pool;
			}
		}

		public static BufferDict<TK, TV> Spawn ()
		{
			return GetPool ().Spawn ();
		}

		public static void Despawn (BufferDict<TK, TV> dict)
		{
			GetPool ().Despawn (dict);
		}

		public void Dispose ()
		{
			Despawn (this);
		}

		protected BufferDict () { }
	}

	public class BufferStack<T> : Stack<T>, IDisposable
	{
		static Dictionary<Type, object> pools = new Dictionary<Type, object> ();

		static SimplePool<BufferStack<T>> GetPool ()
		{
			return (SimplePool<BufferStack<T>>)pools.GetOrNew (typeof (BufferStack<T>), CreatePool);

			object CreatePool ()
			{
				var pool = new SimplePool<BufferStack<T>> ();
				pool.createObj = () => new BufferStack<T> ();
				pool.onSpawn = set => set.Clear ();
				pool.onDespawn = set => set.Clear ();
				return pool;
			}
		}

		public static BufferStack<T> Spawn ()
		{
			return GetPool ().Spawn ();
		}

		public static void Despawn (BufferStack<T> set)
		{
			GetPool ().Despawn (set);
		}

		public void Dispose ()
		{
			Despawn (this);
		}

		protected BufferStack () { }
	}

	public class BufferQueue<T> : Queue<T>, IDisposable
	{
		static Dictionary<Type, object> pools = new Dictionary<Type, object> ();

		static SimplePool<BufferQueue<T>> GetPool ()
		{
			return (SimplePool<BufferQueue<T>>)pools.GetOrNew (typeof (BufferQueue<T>), CreatePool);

			object CreatePool ()
			{
				var pool = new SimplePool<BufferQueue<T>> ();
				pool.createObj = () => new BufferQueue<T> ();
				pool.onSpawn = set => set.Clear ();
				pool.onDespawn = set => set.Clear ();
				return pool;
			}
		}

		public static BufferQueue<T> Spawn ()
		{
			return GetPool ().Spawn ();
		}

		public static void Despawn (BufferQueue<T> set)
		{
			GetPool ().Despawn (set);
		}

		public void Dispose ()
		{
			Despawn (this);
		}

		protected BufferQueue () { }
	}
}