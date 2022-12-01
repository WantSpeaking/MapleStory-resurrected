using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FairyGUI;
using MapleLib.WzLib;
using ms;
using UnityEngine;
using Camera = UnityEngine.Camera;
using ms_Unity;

public class TestURPBatcher : SingletonMono<TestURPBatcher>
{
	public GameObject prefeb;
	public GameObject prefab_Sprite;
	public UnityEngine.U2D.SpriteAtlas SpriteAtlas;

	public Material presetMaterial;

	public List<Texture2D> presetTextureList = new List<Texture2D> ();

	public ConcurrentDictionary<ms.Texture, Renderer> texture_GObj_Dict = new ConcurrentDictionary<ms.Texture, Renderer> ();

	public ConcurrentDictionary<string, Material> path_Material_Dict = new ConcurrentDictionary<string, Material> ();

	public UnityPool<GameObject> _GObj_Pool = new UnityPool<GameObject> ();
	public UnityPool<Material> _Material_Pool = new UnityPool<Material> ();

	public GameObject test;

	private GRichTextField gRichTextField;

	private GTextInput gTextInput;

	UnityObjectPool<GameObject> pool = new UnityObjectPool<GameObject> ();
	public Renderer TryGetGObj (ms.Texture hashCode, Func<GameObject> create = null)
	{
		if (!texture_GObj_Dict.TryGetValue (hashCode, out var result))
		{
			//result = create?.Invoke ();
			result = Create (hashCode).GetComponent<Renderer> ();
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
	private Dictionary<string, int> textureName_RenderOrder = new Dictionary<string, int> ();
	private int renderOrder = -100;
	public void TryDraw (ms.Texture texture, Vector3 pos, Vector3 scale, GameObject drawParent)
	{
		var renderer = TryGetGObj (texture);
		if (renderer == null)
		{
			throw new NullReferenceException ();
		}
		var gobj = renderer.gameObject;

		if ((bool)gobj)
		{
			if (drawParent != null && !gobj.transform.IsChildOf (drawParent.transform))
			{
				gobj.SetParent (drawParent.transform);
			}
			//gobj.SetActive (true);
			renderer.enabled = true;
			gobj.transform.position = pos;
			var scalFactorX = texture.sprite == null ? texture.get_dimensions ().x () : 100;
			var scalFactorY = texture.sprite == null ? texture.get_dimensions ().y () : 100;

			gobj.transform.localScale = new Vector3 ((float)scalFactorX * scale.x, (float)scalFactorY * scale.y, 1f);
			SingletonMono<GameUtil>.Instance.DrawOrder--;
		}
	}
	List<KeyValuePair<ms.Texture, GameObject>> cache_List = new List<KeyValuePair<ms.Texture, GameObject>> ();
	int cache_Count = 0;
	public void HideAll ()
	{
		//SpriteAtlas.
		//cache_List.Sort((x,y)=>x.sprite.)
		//AppDebug.LogError ($"texture_GObj_Dict.Count:{texture_GObj_Dict.Count}\t _GObj_Pool.count:{_GObj_Pool.count}\t _Material_Pool.count:{_Material_Pool.count}");
		//Clear ();

		/*	if (texture_GObj_Dict.Count != cache_List.Count)
			{
				cache_List.Clear ();
				cache_List.AddRange(texture_GObj_Dict);
				cache_List.Sort ((pair1, pair2) => pair1.Value..)
			}*/
		/*		if (cache_Count != texture_GObj_Dict.Count)
				{
					cache_Count = texture_GObj_Dict.Count;
					foreach (var pair in texture_GObj_Dict)
					{
						if (pair.Key.sprite == null)
						{
							continue;
						}
						GameObject gobj = pair.Value;
						var spriteRender = gobj.GetComponent<SpriteRenderer> ();
						spriteRender.sortingOrder = textureName_RenderOrder.TryGetValue (spriteRender.sprite.texture.name);
						Debug.Log ($"spriteRender:{spriteRender.sprite.texture.name}={spriteRender.sortingOrder}");
					}
				}*/

		foreach (var  gobj in texture_GObj_Dict.Values)
		{
			/*Vector3 originalPos = gobj.transform.position;
			gobj.transform.position = new Vector3 (originalPos.x, originalPos.y, 1f);*/

			gobj.enabled = false;
		}
		//Debug.Log ($"_GObj_Pool.count:{_GObj_Pool.count} texture_GObj_Dict.Count:{texture_GObj_Dict.Count} targetFrameRate:{Application.targetFrameRate}");
	}

	List<ms.Texture> textures = new List<ms.Texture> ();
	public void Clear ()
	{
		/*textures.Clear ();
		

		foreach (var key in textures)
		{
			texture_GObj_Dict.Remove (key, out var gameObject);
		}*/

		foreach (var pair in texture_GObj_Dict)
		{
			if (pair.Key.sprite == null)
				_Material_Pool.ReturnObject (pair.Key.fullPath, pair.Value.GetComponent<MeshRenderer> ().material);
			_GObj_Pool.ReturnObject (pair.Key.fullPath, pair.Value.gameObject);
			pair.Value.enabled = false;
			//pair.Value.SetActive (false);
		}
		texture_GObj_Dict.Clear ();
	}

	private GameObject Parent;
	private GameObject Create (ms.Texture tex)
	{
		if (Parent == null)
		{
			Parent = new GameObject ("Parent");
		}
		if (tex.sprite == null)
		{
			Material tempMaterial = _Material_Pool.GetObject (tex.fullPath, CreateMaterial);
			tempMaterial.mainTexture = tex.texture2D;

			GameObject tempObj = _GObj_Pool.GetObject (tex.fullPath, CreateGObj);
			tempObj.SetActive (true);
			tempObj.name = tex.fullPath.Substring (tex.fullPath.IndexOf (".wz\\"));
			tempObj.GetComponent<MeshRenderer> ().material = tempMaterial;
			tempObj.SetParent (Parent.transform);
			tempObj.layer = tex.layerMask;
			return tempObj;
		}
		else
		{
			GameObject tempObj = _GObj_Pool.GetObject (tex.fullPath, CreateSpriteObj);
			tempObj.SetActive (true);
			tempObj.name = tex.fullPath;

			tempObj.SetParent (Parent.transform);
			tempObj.layer = tex.layerMask;

			var spriteRender = tempObj.GetComponent<SpriteRenderer> ();
			spriteRender.sprite = tex.sprite;
			if (!textureName_RenderOrder.TryGetValue (spriteRender.sprite.texture.name, out var _))
			{
				textureName_RenderOrder.Add (spriteRender.sprite.texture.name, renderOrder++);
			}
			spriteRender.sortingOrder = textureName_RenderOrder.TryGetValue (spriteRender.sprite.texture.name);

			return tempObj;
		}
	}

	private GameObject CreateGObj ()
	{
		return UnityEngine.Object.Instantiate (prefeb);
	}
	private GameObject CreateSpriteObj ()
	{
		return UnityEngine.Object.Instantiate (prefab_Sprite);
	}
	private Material CreateMaterial ()
	{
		return new Material (presetMaterial);
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
