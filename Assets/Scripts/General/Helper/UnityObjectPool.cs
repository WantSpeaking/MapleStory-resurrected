using System;
using System.Collections.Concurrent;

namespace ms
{
	public class UnityObjectPool<T>
	{
		private ConcurrentDictionary<int, T> _dictionary = new ConcurrentDictionary<int, T> ();

		public T Spawn (int hashCode, Func<T> create)
		{
			if (!_dictionary.TryGetValue (hashCode, out var result))
			{
				result = create.Invoke ();
				_dictionary.TryAdd (hashCode, result);
			}

			return result;
		}

		public void DeSpawn (T unityObject)
		{
			
		}
	}
}