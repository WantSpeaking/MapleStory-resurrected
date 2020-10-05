using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
	public class Singleton<T> : System.IDisposable where T : new ()
	{
		public virtual void Dispose()
		{
		}

		public static T get()
		{
			return instance;
		}

		private static T instance = new T();
	}
}
