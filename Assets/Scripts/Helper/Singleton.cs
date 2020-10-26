using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ms
{
	public class Singleton<T> : IDisposable where T :new()
	{
		private static T instance;

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new T();
				}
				return instance;
			}
		}

		public static T get () => Instance;
		
		public virtual void Dispose()
		{
        
		}
	}
}
