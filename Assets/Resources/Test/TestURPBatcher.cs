using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using MapleLib.WzLib;
using ms;
using UnityEngine;

public class TestURPBatcher : SingletonMono<TestURPBatcher>
{
	public GameObject prefeb;
	public Material presetMaterial;
	public List<Texture2D> presetTextureList = new List<Texture2D> ();
	public ConcurrentDictionary<ms.Texture, GameObject> texture_GObj_Dict = new ConcurrentDictionary<ms.Texture, GameObject> ();

	public GameObject TryGetGObj (ms.Texture hashCode, Func<GameObject> create = null)
	{
		if (!texture_GObj_Dict.TryGetValue (hashCode, out var result))
		{
			result = create?.Invoke ();
			result = Create (hashCode);
			if (result != null)
			{
				texture_GObj_Dict.TryAdd (hashCode, result);
			}
		}

		else
		{
			if (result == null)
			{
				texture_GObj_Dict.TryRemove (hashCode, out var removed);
			}
		}

		return result;
	}
	public void TryDraw(ms.Texture texture, Bitmap pnginfo, Vector3 pos, Vector3 scale)
	{
		/*if(texture.texture2D != null)
			Graphics.DrawTexture(new Rect(pos.x,pos.y, pnginfo.width, pnginfo.height), texture.texture2D, presetMaterial);*/
		var gobj = TryGetGObj (texture, null);
		if (gobj == null || pnginfo == null)
		{
			throw new NullReferenceException();
		}
		if (gobj)
		{
			gobj.transform.position = pos;
			gobj.transform.localScale = new Vector3(pnginfo.Width * scale.x, pnginfo.Height* scale.y, 1);
			GameUtil.Instance.DrawOrder--;
			AppDebug.Log($"DrawOrder:{GameUtil.Instance.DrawOrder} fullPath:{texture.fullPath} format:{pnginfo.format}");
		}
	}
	GameObject Create (ms.Texture tex)
	{
		var tempMaterial = new Material (presetMaterial);
		tempMaterial.mainTexture = tex.texture2D;

		var tempObj = Instantiate (prefeb);
		tempObj.name = tex.fullPath;
		tempObj.GetComponent<MeshRenderer> ().material = tempMaterial;
		return tempObj;
	}
	// Start is called before the first frame update
	void Start ()
	{
		/*foreach (var tex in presetTextureList)
		{
			var tempMaterial = new Material (presetMaterial);
			tempMaterial.mainTexture = tex;
			var tempObj = Instantiate (prefeb);
			tempObj.GetComponent<MeshRenderer> ().material = tempMaterial;
		}*/

	}

	// Update is called once per frame
	void Update ()
	{

	}
}
