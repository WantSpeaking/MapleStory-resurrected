using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FairyGUI;
using MapleLib.WzLib;
using ms;
using UnityEngine;
using Camera = UnityEngine.Camera;

public class TestURPBatcher : SingletonMono<TestURPBatcher>
{
	public GameObject prefeb;

	public Material presetMaterial;

	public List<Texture2D> presetTextureList = new List<Texture2D> ();

	public ConcurrentDictionary<ms.Texture, GameObject> texture_GObj_Dict = new ConcurrentDictionary<ms.Texture, GameObject> ();

	public GameObject test;

	private GRichTextField gRichTextField;

	private GTextInput gTextInput;

	UnityObjectPool<GameObject> pool = new UnityObjectPool<GameObject> ();
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
		else if (result == null)
		{
			texture_GObj_Dict.TryRemove (hashCode, out var _);
		}
		return result;
	}

	public void TryDraw (ms.Texture texture, Bitmap pnginfo, Vector3 pos, Vector3 scale, GameObject drawParent)
	{
		GameObject gobj = TryGetGObj (texture);
		if (gobj == null || pnginfo == null)
		{
			throw new NullReferenceException ();
		}
		if ((bool)gobj)
		{
			if (drawParent != null && !gobj.transform.IsChildOf (drawParent.transform))
			{
				gobj.SetParent (drawParent.transform);
			}
			gobj.SetActive (true);
			gobj.transform.position = pos;
			gobj.transform.localScale = new Vector3 ((float)pnginfo.Width * scale.x, (float)pnginfo.Height * scale.y, 1f);
			Singleton<GameUtil>.Instance.DrawOrder--;
			//AppDebug.Log ($"fullPath:{texture.fullPath} pnginfo:{pnginfo.ToString ()}  scale:{scale}");
		}
	}

	public void HideAll ()
	{
		AppDebug.LogError ($"texture_GObj_Dict.Count:{texture_GObj_Dict.Count}");
		//Clear ();
		foreach (GameObject gobj in texture_GObj_Dict.Values)
		{
			/*Vector3 originalPos = gobj.transform.position;
			gobj.transform.position = new Vector3 (originalPos.x, originalPos.y, 1f);*/

			gobj.SetActive (false);
		}
	}

	List<ms.Texture> textures = new List<ms.Texture> ();
	public void Clear()
	{
		textures.Clear ();
		foreach (var pair in texture_GObj_Dict)
		{
			if (pair.Value == null)//if gameobj is destoried
			{
				textures.Add (pair.Key);
			}
		}

		foreach (var key in textures)
		{
			texture_GObj_Dict.Remove (key, out var gameObject);
		}
		//texture_GObj_Dict.Clear ();
	}

	private GameObject Parent;
	private GameObject Create (ms.Texture tex)
	{
		if (Parent == null)
		{
			Parent = new GameObject ("Parent");
		}

		Material tempMaterial = new Material (presetMaterial);
		tempMaterial.mainTexture = tex.texture2D;
		GameObject tempObj = UnityEngine.Object.Instantiate (prefeb);
		tempObj.name = tex.fullPath;
		tempObj.GetComponent<MeshRenderer> ().material = tempMaterial;
		tempObj.SetParent (Parent.transform);
		tempObj.layer = tex.layerMask;
		return tempObj;
	}

	private void Awake ()
	{
		//GRoot.inst.container.renderMode = RenderMode.WorldSpace;
	}

	private void Start ()
	{
		/*gTextInput = new GTextInput ();
		gTextInput.text = "11111111111111111111111111111111";
		gTextInput.border = 5;
		GRoot.inst.AddChild (gTextInput);*/
	}

	private new void Update ()
	{
		/*Vector3 screenPos = StageCamera.main.WorldToScreenPoint (test.transform.position);
		screenPos.y = (float)Screen.height - screenPos.y;
		Vector2 localPos = GRoot.inst.GlobalToLocal (screenPos);
		gTextInput.SetPosition (localPos.x, localPos.y, -99f);
		gTextInput.SetSize (200f, 100f);
		Vector3 position = GRoot.inst.touchTarget?.position ?? Vector3.zero;*/
	}
}
