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
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using Texture = ms.Texture;

public class TestURPBatcher : SingletonMono<TestURPBatcher>
{
	public GameObject prefeb;

	public Material presetMaterial;

	public List<Texture2D> presetTextureList = new List<Texture2D> ();

	public List<string> urlList_before = new List<string> ();
	public List<string> urlList_after = new List<string> ();

	public ConcurrentDictionary<Texture, GameObject> texture_GObj_Dict_before = new ConcurrentDictionary<Texture, GameObject> ();
	public ConcurrentDictionary<Texture, GameObject> texture_GObj_Dict_after = new ConcurrentDictionary<Texture, GameObject> ();

	public ConcurrentDictionary<Texture, GameObject> texture_GObj_Dict = new ConcurrentDictionary<Texture, GameObject> ();

	public ConcurrentDictionary<string, Material> path_Material_Dict = new ConcurrentDictionary<string, Material> ();

	public UnityPool<GameObject> _GObj_Pool = new UnityPool<GameObject> ();
	public UnityPool<Material> _Material_Pool = new UnityPool<Material> ();

	public GameObject test;

	private GRichTextField gRichTextField;

	private GTextInput gTextInput;

	UnityObjectPool<GameObject> pool = new UnityObjectPool<GameObject> ();
	public GameObject TryGetGObj (ms.Texture hashCode, Func<GameObject> create = null)
	{
		if (!texture_GObj_Dict.TryGetValue (hashCode, out var result))
		{
			//result = create?.Invoke ();
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

			if (name_gobj_ParentDict.Count == 0)
			{
				FillParentDict ();
			}
			if (!name_gobj_ParentDict.TryGetValue (GetWzName (texture.fullPath),out var p))
			{
				p = Parent;
			}
			if (drawParent != null && !drawParent.transform.IsChildOf (p.transform))
			{
				drawParent.SetParent (p.transform);
			}

			if (drawParent == null && !gobj.transform.IsChildOf (p.transform))
			{
				gobj.SetParent (p.transform);
			}
			
			gobj.SetActive (true);
			gobj.transform.position = pos;
			gobj.transform.localScale = new Vector3 ((float)pnginfo.Width * scale.x, (float)pnginfo.Height * scale.y, 1f);
			SingletonMono<GameUtil>.Instance.DrawOrder--;
			//AppDebug.Log ($"fullPath:{texture.fullPath} pnginfo:{pnginfo.ToString ()}  scale:{scale}");
		}
	}

	public void HideAll ()
	{

		//AppDebug.LogError ($"texture_GObj_Dict.Count:{texture_GObj_Dict.Count}\t _GObj_Pool.count:{_GObj_Pool.count}\t _Material_Pool.count:{_Material_Pool.count}");
		//Clear ();
		foreach (GameObject gobj in texture_GObj_Dict.Values)
		{
			/*Vector3 originalPos = gobj.transform.position;
			gobj.transform.position = new Vector3 (originalPos.x, originalPos.y, 1f);*/

			gobj.SetActive (false);
		}
	}

	public void addBefore ()
	{
		texture_GObj_Dict_before.Clear ();
		urlList_before.Clear ();
		urlList_before.AddRange<string> (_GObj_Pool.GetKeys ());
	}
	public void Log (string message)
	{
		Debug.Log ($"{message} texture_GObj_Dict.Count:{texture_GObj_Dict.Count} _GObj_Pool.count: {_GObj_Pool.count} _Material_Pool.count:{_Material_Pool.count}");

	}

	public void LogDiff ()
	{
		/*texture_GObj_Dict_after.Clear ();
		foreach (var pair in _GObj_Pool)
		{
			if (!texture_GObj_Dict_before.ContainsKey (pair.Key))
			{
				texture_GObj_Dict_after.TryAdd (pair.Key, pair.Value);
			}
		}

		AppDebug.Log (texture_GObj_Dict_after.ToDebugLog ());*/
		urlList_after.Clear ();
		foreach (var pair in _GObj_Pool)
		{
			if (!urlList_before.Contains (pair.Key))
			{
				urlList_after.Add (pair.Key);
			}
		}
		AppDebug.Log (urlList_after.ToDebugLog ());
	}
	
	List<ms.Texture> textures = new List<ms.Texture> ();
	public void Clear ()
	{
		/*textures.Clear ();
		

		foreach (var key in textures)
		{
			texture_GObj_Dict.Remove (key, out var gameObject);
		}*/
		
		Debug.Log ($"Clear：texture_GObj_Dict.Count:{texture_GObj_Dict.Count} _GObj_Pool.count: {_GObj_Pool.count} _Material_Pool.count:{_Material_Pool.count}");

		foreach (var pair in texture_GObj_Dict)
		{
			_Material_Pool.ReturnObject (pair.Key.fullPath, pair.Value.GetComponent<MeshRenderer> ().material);
			_GObj_Pool.ReturnObject (pair.Key.fullPath, pair.Value);
			pair.Value.SetActive (false);
		}
		texture_GObj_Dict.Clear ();
	}

	private GameObject _parent;
	private GameObject _parent_Character;
	private GameObject _parent_Effect;
	private GameObject _parent_Item;
	private GameObject _parent_Map;
	private GameObject _parent_Mob;
	private GameObject _parent_Npc;
	private GameObject _parent_Skill;
	private GameObject _parent_UI_083;
	
	private GameObject Parent;
	private GameObject Parent_Character=> _parent_Character??=new GameObject("Character1");
	private GameObject Parent_Effect=>    _parent_Effect??=new GameObject("Effect");
	private GameObject Parent_Item=>      _parent_Item??=new GameObject(  "Item");
	private GameObject Parent_Map=>       _parent_Map??(_parent_Map=new GameObject(   "Map")) ;
	private GameObject Parent_Mob=>       _parent_Mob??=new GameObject(   "Mob");
	private GameObject Parent_Npc=>       _parent_Npc??=new GameObject(   "Npc");
	private GameObject Parent_Skill=>     _parent_Skill??=new GameObject( "Skill");
	private GameObject Parent_UI_083=>    _parent_UI_083??(_parent_UI_083=new GameObject("UI_083"));

	private GameObject Create (ms.Texture tex)
	{
		//Material tempMaterial = new Material (presetMaterial);
		Material tempMaterial = _Material_Pool.GetObject (tex.fullPath, CreateMaterial);
		tempMaterial.mainTexture = tex.texture2D;

		//GameObject tempObj = UnityEngine.Object.Instantiate (prefeb);

		GameObject tempObj = _GObj_Pool.GetObject (tex.fullPath, CreateGObj);
		tempObj.SetActive (true);
		tempObj.name = tex.fullPath;
		tempObj.GetComponent<MeshRenderer> ().material = tempMaterial;
		//empObj.SetParent (Parent.transform);
		var n = LayerMask.NameToLayer (GetWzName (tex.fullPath));
		if (n is < 0 or > 31)
		{
			AppDebug.Log ($"layer：{GetWzName (tex.fullPath)}  不存在");
		}	
		
		/*tempObj.layer = LayerMask.NameToLayer (GetWzName(tex.fullPath));
		if (tex.layerMask.value == LayerMask.NameToLayer("Player"))
		{
			tempObj.layer = LayerMask.NameToLayer ("Player");
		}*/
		tempObj.layer = tex.layerMask;
		return tempObj;
	}

	private string GetWzName (string path)
	{
		var arr = path.Split (".wz");
		if (arr.Length>=2)
		{
			return arr[0];
		}

		return "Default";
	}

	private GameObject CreateGObj ()
	{
		return UnityEngine.Object.Instantiate (prefeb);
	}

	private Material CreateMaterial ()
	{
		return new Material (presetMaterial);
	}

	protected override void OnAwake ()
	{
		base.OnAwake ();
		
	}

	private void Awake ()
	{
		//GRoot.inst.container.renderMode = RenderMode.WorldSpace;
		
	}

	private void FillParentDict ()
	{
		Parent = new GameObject ("Parent");
		name_gobj_ParentDict.Add("Character1",new GameObject("Character1"));
		name_gobj_ParentDict.Add("Effect",new GameObject("Effect"));
		name_gobj_ParentDict.Add("Item",new GameObject("Item"));
		name_gobj_ParentDict.Add("Map",new GameObject ("Map"));
		name_gobj_ParentDict.Add("Mob",new GameObject ("Mob"));
		name_gobj_ParentDict.Add("Npc",new GameObject ("Npc"));
		name_gobj_ParentDict.Add("Skill",new GameObject("Skill"));
		name_gobj_ParentDict.Add("UI_083",new GameObject("UI_083"));
		name_gobj_ParentDict.Add("UI_Endless",new GameObject("UI_Endless"));
		name_gobj_ParentDict.Add("Reactor",new GameObject("Reactor"));

	}
	public Dictionary<string, GameObject> name_gobj_ParentDict = new Dictionary<string, GameObject> ();
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
		//Debug.Log ($"texture_GObj_Dict.Count:{texture_GObj_Dict.Count} _GObj_Pool.count: {_GObj_Pool.count} _Material_Pool.count:{_Material_Pool.count}");
	}
}
