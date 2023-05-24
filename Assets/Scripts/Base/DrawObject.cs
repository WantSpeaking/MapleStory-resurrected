using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace ms_Unity
{
	public class DrawObject : ObjectBase
	{
		public static DrawObject Create(string name,GameObject target)
		{
			DrawObject drawObject = ReferencePool.Acquire<DrawObject>();
			drawObject.Initialize(name,target);
			return drawObject;
		}

		protected override void Release(bool isShutdown)
		{
			var hpBarItem = (GameObject)Target;
			Object.Destroy(hpBarItem);
		}
	}
}
