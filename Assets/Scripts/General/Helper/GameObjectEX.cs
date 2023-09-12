using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
	public static class GameObjectEX
	{
		public static T GetOrAddComponent<T> (this GameObject go)
			where T : Component
		{
			var c = go.GetComponent<T> ();
			if (c == null)
			{
				c = go.AddComponent<T> ();
			}
			return c;
		}

		public static void SetLayer (this GameObject go, int layer, bool includeChildren = true)
		{
			if (go == null) return;
			if (includeChildren)
			{
				foreach (var transform in go.GetComponentsInChildren<Transform>())
				{
					transform.gameObject.layer = layer;
				}
			}
			else
			{
				go.layer = layer;
			}
		}

        public static void SetSortingLayer(this GameObject go, string sortingLayerName, bool includeChildren = true)
        {
            if (go == null) return;
            if (includeChildren)
			{
                foreach (var transform in go.GetComponentsInChildren<Transform>())
                {
					SetSortingLayer(transform.gameObject, sortingLayerName, false);
                }
            }
			else
			{
				
                var render = go.GetComponent<Renderer>();
                if (render != null)
                {
                    //Debug.Log($"SetSortingLayer:{go.name} {sortingLayerName}");
                    render.sortingLayerName = sortingLayerName;
                }
            }
        }
    }
}