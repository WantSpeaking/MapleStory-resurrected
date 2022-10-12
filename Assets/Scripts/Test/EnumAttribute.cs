using ParadoxNotion.Design;
using UnityEngine;

namespace ms_Unity
{
	public class MyRangeAttribute : DrawerAttribute
	{
		public float min = 0;
		public float max = 1;
		public KeyCode keyCode;
		public MyRangeAttribute (float min, float max, KeyCode keyCode)
		{
			this.min = min;
			this.max = max;
			this.keyCode = keyCode;
		}
	}

	public class CharActionIdSelectorAttribute : DrawerAttribute
	{

	}

	public class StanceIdSelectorAttribute : DrawerAttribute
	{

	}

	public class AfterimgIdSelectorAttribute : DrawerAttribute
	{

	}
}