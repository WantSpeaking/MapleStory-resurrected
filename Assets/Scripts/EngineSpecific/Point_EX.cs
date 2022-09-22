using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ms;
using UnityEngine;

namespace ms_Unity
{
	public static class Point_EX
	{
		public static Vector2 ToUnityVector2 (this Point_short point_short, bool flipX = false, bool flipY = false)
		{
			if (point_short == null)
			{
				return Vector2.zero;
			}
			return new Vector2 (flipX ? -point_short.x () : point_short.x (), flipY ? -point_short.y () : point_short.y ());
		}
	}
}
