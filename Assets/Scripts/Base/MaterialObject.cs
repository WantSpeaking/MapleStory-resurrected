using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace ms_Unity
{
	public class MaterialObject : ObjectBase
	{
		public static MaterialObject Create(string name,Material target)
		{
			var drawObject = ReferencePool.Acquire<MaterialObject>();
			drawObject.Initialize(name,target);
			return drawObject;
		}

		protected override void Release(bool isShutdown)
		{
			var hpBarItem = (Material)Target;
			Object.Destroy(hpBarItem);
		}
	}
}
