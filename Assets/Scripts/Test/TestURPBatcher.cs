using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FairyGUI;
using GameFramework.ObjectPool;
using MapleLib.WzLib;
using ms;
using UnityEngine;
using Camera = UnityEngine.Camera;
using ms_Unity;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using Sprite = UnityEngine.Sprite;
using Texture = ms.Texture;
using System.Linq;
using tools;
using Utility;

//namespace ms_Unity
//{



public class TestURPBatcher : SingletonMono<TestURPBatcher>
{
    public GameObject prefeb;
    public GameObject prefeb_Sprite;

    public Material unlit_presetMaterial;
    public Material pixelate_presetMaterial;

    public List<Texture2D> presetTextureList = new List<Texture2D>();

    public List<string> urlList_before = new List<string>();
    public List<string> urlList_after = new List<string>();

    public ConcurrentDictionary<Texture, GameObject> texture_GObj_Dict_before = new ConcurrentDictionary<Texture, GameObject>();
    public ConcurrentDictionary<Texture, GameObject> texture_GObj_Dict_after = new ConcurrentDictionary<Texture, GameObject>();

    public ConcurrentDictionary<Texture, GameObject> texture_GObj_Dict = new ConcurrentDictionary<Texture, GameObject>();
    public ConcurrentDictionary<Texture, Material> texture_Material_Dict = new ConcurrentDictionary<Texture, Material>();

    public ConcurrentDictionary<string, Material> path_Material_Dict = new ConcurrentDictionary<string, Material>();

    //public UnityPool<GameObject> _GObj_Pool = new UnityPool<GameObject> ();
    //public UnityPool<Material> _Material_Pool = new UnityPool<Material> ();

    public GameObject test;

    private GRichTextField gRichTextField;

    private GTextInput gTextInput;

    UnityObjectPool<GameObject> pool = new UnityObjectPool<GameObject>();
    private static List<string> drawImmediateList = new List<string>() { "Character", "BasicEff.img", "icon", "skill", "No" };

    public GameObject TryGetGObj(ms.Texture hashCode, Func<GameObject> create = null)
    {
        if (!texture_GObj_Dict.TryGetValue(hashCode, out var result))
        {
            if (drawImmediateList.Where(k => hashCode.fullPath.Contains(k)).Any())
                //if (hashCode.fullPath.Contains("No"))
            {
                result = Create (hashCode);
                if (result != null)
                {
                    texture_GObj_Dict.TryAdd (hashCode, result);
                }
            }
           else
            {
                var spTask = new SpawnTask(hashCode);
                spawnTasks.Enqueue(spTask);
                texture_GObj_Dict.TryAdd(hashCode, null);
            }
        }
        /*else if (result == null)
        {
            texture_GObj_Dict.TryRemove (hashCode, out var _);
        }*/
        return result;
    }

    public void TryDraw(ms.Texture texture, Bitmap pnginfo, Vector3 pos, Vector3 scale, GameObject drawParent)
    {
        GameObject gobj = TryGetGObj(texture);
        if (gobj == null || pnginfo == null)
        {
            //throw new NullReferenceException ();
            return;
        }
        if (gobj != null)
        {
            if (drawParent != null && !gobj.transform.IsChildOf(drawParent.transform))
            {
                gobj.SetParent(drawParent.transform);
            }

            /*if (name_gobj_ParentDict.Count == 0)
            {
                FillParentDict();
            }
            if (!name_gobj_ParentDict.TryGetValue(GetWzName(texture.fullPath), out var p))
            {
                p = Parent;
            }
            if (drawParent != null && p!= null &&  !drawParent.transform.IsChildOf(p.transform))
            {
                drawParent.SetParent(p.transform);
            }

            if (drawParent == null && p != null && gobj!=null && !gobj.transform.IsChildOf(p.transform))
            {
                gobj.SetParent(p.transform);
            }*/

            gobj.SetActive(true);
            gobj.transform.position = pos;
            if (texture.isSprite)
            {
                gobj.transform.localScale = new Vector3(1, 1, -1f);
            }
            else
            {
                gobj.transform.localScale = new Vector3((float)pnginfo.Width * scale.x, (float)pnginfo.Height * scale.y, 1f);

            }
            SingletonMono<GameUtil>.Instance.DrawOrder--;
            //AppDebug.Log ($"fullPath:{texture.fullPath} pnginfo:{pnginfo.ToString ()}  scale:{scale}");
        }
    }
    public void HideOne(ms.Texture texture)
    {
        if (texture_GObj_Dict.TryGetValue(texture,out var gobj))
        {
            gobj?.SetActive(false);
        }
    }
    public void HideAll()
    {

        //AppDebug.LogError ($"texture_GObj_Dict.Count:{texture_GObj_Dict.Count}\t _GObj_Pool.count:{_GObj_Pool.count}\t _Material_Pool.count:{_Material_Pool.count}");
        //Clear ();
        foreach (GameObject gobj in texture_GObj_Dict.Values)
        {
            if (gobj != null)
            {
                gobj.SetActive(false);
            }
        }

        /*foreach (GameObject gobj in texture_GObj_Dict.Values)
        {
            *//*Vector3 originalPos = gobj.transform.position;
            gobj.transform.position = new Vector3 (originalPos.x, originalPos.y, 1f);*//*

            if (gobj != null)
            {
                m_DrawObjectPool.Unspawn(gobj);
            }
        }*/
        //texture_GObj_Dict.Clear();
    }

    public void addBefore()
    {
        texture_GObj_Dict_before.Clear();
        urlList_before.Clear();
        //urlList_before.AddRange<string> (_GObj_Pool.GetKeys ());
    }
    public void Log(string message)
    {
        //Debug.Log ($"{message} texture_GObj_Dict.Count:{texture_GObj_Dict.Count} _GObj_Pool.count: {_GObj_Pool.count} _Material_Pool.count:{_Material_Pool.count}");

    }

    public void LogDiff()
    {

        /*urlList_after.Clear ();
        foreach (var pair in _GObj_Pool)
        {
            if (!urlList_before.Contains (pair.Key))
            {
                urlList_after.Add (pair.Key);
            }
        }
        AppDebug.Log (urlList_after.ToDebugLog ());*/
    }
    public void CLearDestoryObjs()
    {
        spawnTasks.Clear();
        ms.Stage.get().get_mobs().clearSpawns();
        ms.Stage.get().get_npcs().clearSpawns();

        StopAllCoroutines();
        StartCoroutine(SpawnGObj());
        StartCoroutine(ms.Stage.get().get_mobs().SpawnMob());
        StartCoroutine(ms.Stage.get().get_npcs().SpawnNPC());

       /* textures.Clear();
        foreach (var pair in texture_GObj_Dict)
        {
            //AppDebug.Log($"pair.Key.isDontDestoryOnLoad:{pair.Key.isDontDestoryOnLoad}");
            if (!pair.Key.isDontDestoryOnLoad)
            {
                textures.Add(pair.Key);
            }
        }
        foreach (var t in textures)
        {
            if (ms.Texture.dontDestoryList.Where(k => t.fullPath.Contains(k)).Any()) continue;
            texture_GObj_Dict.TryRemove(t, out var g);
            *//*if (g != null)
            m_DrawObjectPool.Unspawn(g);*//*
        }*/

        foreach (var gobj in texture_GObj_Dict)
        {
            if (gobj.Key.DrawObject != null)
            {
                m_DrawObjectPool.Unspawn(gobj.Key.DrawObject);
            }
        }
        texture_GObj_Dict.Clear();
        //AppDebug.Log($"texture_GObj_Dict count:{texture_GObj_Dict.Count}\tCharacter1 tex count:{texture_GObj_Dict.Where(p=>p.Key.fullPath.Contains("Character1.wz")).Count()}");

    }
    List<ms.Texture> textures = new List<ms.Texture>();
    /// <summary>
    /// 要在UI（change_state）和Stage（CLear）都Dispose完了再Clear
    /// </summary>
    public void Clear()
    {
       
        /*textures.Clear ();


        foreach (var key in textures)
        {
            texture_GObj_Dict.Remove (key, out var gameObject);
        }*//*

        //Debug.Log ($"Clear：texture_GObj_Dict.Count:{texture_GObj_Dict.Count} _GObj_Pool.count: {_GObj_Pool.count} _Material_Pool.count:{_Material_Pool.count}");

        foreach (var pair in texture_GObj_Dict)
        {
            if (pair.Value == null) continue;
            if (!pair.Key.isSprite)
            {
                //m_MaterialObjectPool.Unspawn (pair.Value.GetComponent<MeshRenderer> ().material);
                {
                    if (pair.Value?.TryGetComponent<MeshRenderer>(out var meshRenderer)??false)
                    {
                        Destroy(meshRenderer.material);
                    }

                }
                //_Material_Pool.ReturnObject (pair.Key.fullPath, pair.Value.GetComponent<MeshRenderer> ().material);
            }
            //m_DrawObjectPool.Unspawn (pair.Value);
            //_GObj_Pool.ReturnObject (pair.Key.fullPath, pair.Value);
            pair.Value.SetActive (false);
        }

        foreach (var material in _materials)
        {
            Destroy (material);
        }
        _materials.Clear ();
        texture_GObj_Dict.Clear ();*/
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
    private GameObject Parent_Character => _parent_Character ??= new GameObject("Character1");
    private GameObject Parent_Effect => _parent_Effect ??= new GameObject("Effect");
    private GameObject Parent_Item => _parent_Item ??= new GameObject("Item");
    private GameObject Parent_Map => _parent_Map ?? (_parent_Map = new GameObject("Map"));
    private GameObject Parent_Mob => _parent_Mob ??= new GameObject("Mob");
    private GameObject Parent_Npc => _parent_Npc ??= new GameObject("Npc");
    private GameObject Parent_Skill => _parent_Skill ??= new GameObject("Skill");
    private GameObject Parent_UI_083 => _parent_UI_083 ?? (_parent_UI_083 = new GameObject("UI_083"));

    private List<Material> _materials = new List<Material>();
    private GameObject Create(ms.Texture tex)
    {
        if (tex.fullPath.Contains("Character1.wz"))
        {
            //Debug.Log($"Create:{tex.GetHashCode()}|{tex.fullPath}");
        }
        GameObject tempObj;
        if (tex.isSprite)
        {
            var drawObj = m_DrawObjectPool.Spawn(tex.fullPath);
            if (drawObj != null)
            {
                tempObj = (GameObject)drawObj.Target;
            }
            else
            {
                tempObj = CreateSpriteGObj();
                drawObj = DrawObject.Create(tex.fullPath, tempObj);

                m_DrawObjectPool.Register(drawObj, true);
            }
            tex.DrawObject = drawObj;
            //tempObj = CreateSpriteGObj();

            tempObj.SetActive(true);
            tempObj.name = tex.fullPath;
            tempObj.layer = tex.layerMask;
            tempObj.SetSortingLayer(tex.sortingLayerName);

            if (tempObj.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.sprite = tex.sprite;
            }
        }
        else
        {
            //Material tempMaterial = new Material (presetMaterial);
            /*Material tempMaterial = null;
            var materialObjectbj = m_MaterialObjectPool.Spawn (tex.fullPath);
            if (materialObjectbj!=null)
            {
                tempMaterial = (Material)materialObjectbj.Target;
            }
            else
            {
                tempMaterial = CreateMaterial ();
                m_MaterialObjectPool.Register (MaterialObject.Create (tex.fullPath,tempMaterial),true);
            }*/

            var tempMaterial = CreateMaterial();
            _materials.Add(tempMaterial);
            texture_Material_Dict.TryAdd(tex, tempMaterial);

            //Material tempMaterial = _Material_Pool.GetObject (tex.fullPath, CreateMaterial);
            //Debug.Log ($"before:{tempMaterial.GetHashCode ()}");

            tempMaterial.name = tex.fullPath;
            tempMaterial.mainTexture = tex.texture2D;
            //Debug.Log ($"after:{tempMaterial.GetHashCode ()}");
            if (!GameUtil.Instance.Use_Unlit_or_Pixelate_Material)
            {
                tempMaterial.SetVector("_PixelCount", new Vector4(tex.width() /3, tex.height() /3, 0, 0));
            }
            var drawObj = m_DrawObjectPool.Spawn(tex.fullPath);
            if (drawObj?.Target is GameObject targetGobj)
            {
                tempObj = targetGobj;
            }
            else
            {
                tempObj = CreateGObj();
                drawObj = DrawObject.Create(tex.fullPath, tempObj);

                m_DrawObjectPool.Register(drawObj, true);
            }
            tex.DrawObject = drawObj;

            //tempObj = CreateGObj();
            if (tempObj == null) return null;
            tempObj.SetActive(true);
            tempObj.name = tex.fullPath;
            if (tempObj.TryGetComponent<MeshRenderer>(out var meshRenderer))
            {
                meshRenderer.material = tempMaterial;
            }
            //empObj.SetParent (Parent.transform);
            var n = LayerMask.NameToLayer(GetWzName(tex.fullPath));
            if (n is < 0 or > 31)
            {
                AppDebug.Log($"layer：{GetWzName(tex.fullPath)}  不存在");
            }

            /*tempObj.layer = LayerMask.NameToLayer (GetWzName(tex.fullPath));
            if (tex.layerMask.value == LayerMask.NameToLayer("Player"))
            {
                tempObj.layer = LayerMask.NameToLayer ("Player");
            }*/
            tempObj.layer = tex.layerMask;
            tempObj.SetSortingLayer(tex.sortingLayerName);

        }

        return tempObj;
    }

    private string GetWzName(string path)
    {
        var arr = path.Split(".wz");
        if (arr.Length >= 2)
        {
            return arr[0];
        }

        return "Default";
    }

    private GameObject CreateGObj()
    {
        return UnityEngine.Object.Instantiate(prefeb);
    }
    private GameObject CreateSpriteGObj()
    {
        return UnityEngine.Object.Instantiate(prefeb_Sprite);
    }
    private Material CreateMaterial()
    {
        if (GameUtil.Instance.Use_Unlit_or_Pixelate_Material)
        {
            return new Material(unlit_presetMaterial);
        }
        else
        {
            return new Material(pixelate_presetMaterial);
        }
    }

    public void UnSpawn(ms.Texture tex)
    {
        if (texture_Material_Dict.TryRemove(tex, out var material))
        {
            Destroy(material);

        }
        if (texture_GObj_Dict.TryRemove(tex,out var gameObject) && tex.DrawObject?.Target != null)
        {
            /*if (tex.fullPath == null) return;
            if (ms.Texture.dontDestoryList.Where(k => tex.fullPath.Contains(k)).Any()) return;*/

            m_DrawObjectPool.Unspawn(tex.DrawObject);
            //Destroy(gameObject);
        }
        //AppDebug.Log($"UnSpawn:{tex.fullPath}\t ContainsKey:{texture_GObj_Dict.ContainsKey(tex)}");

        if (!tex.isDontDestoryOnLoad)
        {
            //AppDebug.Log($"UnSpawn:tex.isDestoryOnLoad:{!tex.isDontDestoryOnLoad}\t{tex.fullPath}");
            //texture_GObj_Dict.TryRemove(tex, out var gameObject);
            if (tex.DrawObject != null)
            {
                //m_DrawObjectPool.Unspawn(tex.DrawObject);
            }
        }
        

        /*if (texture_GObj_Dict.TryGetValue (tex, out var gobj))
        {
            m_DrawObjectPool.Unspawn (gobj);
            AppDebug.Log($"Unspawn:{tex.fullPath}");
        }*/
    }
    protected override void OnAwake()
    {
        base.OnAwake();

    }

    [SerializeField]
    private int m_InstanceDrawObjectPoolCapacity = 200;
    [SerializeField]
    private float m_InstanceDrawObjectPoolExpireTime = 60f;

    private IObjectPool<DrawObject> m_DrawObjectPool = null;
    //private IObjectPool<MaterialObject> m_MaterialObjectPool = null;


    public int dospawnTaskPerTime = 20;
    public int domsTexInitTaskPerTime = 20;


    private ConcurrentQueue<SpawnTask> spawnTasks = new ConcurrentQueue<SpawnTask>();
    //public ConcurrentQueue<MsTexInitTask> msTexInitTasks = new ConcurrentQueue<MsTexInitTask>();

    IEnumerator SpawnGObj()
    {
        while (true)
        {
            yield return null;
            //Debug.Log("SpawnGObj");
            for (int i = 0; i < dospawnTaskPerTime; i++)
            {
                if (spawnTasks.Count > 0)
                {
                    if (spawnTasks.TryDequeue(out var spawnTask))
                    {
                        var msTex = spawnTask.msTex;
                        var gobj = Create(msTex);
                        if (msTex.isDontDestoryOnLoad)
                        {
                            DontDestroyOnLoad(gobj);
                        }
                        texture_GObj_Dict.TryAdd(spawnTask.msTex, gobj, true);
                        //Instantiate(spawnTask.prefab);
                    }

                }
            }

        }

    }

    /*IEnumerator MsTexInit()
    {
        while (true)
        {
            yield return null;
            //Debug.Log("SpawnGObj");
            for (int i = 0; i < domsTexInitTaskPerTime; i++)
            {
                if (msTexInitTasks.Count > 0)
                {
                    if (msTexInitTasks.TryDequeue(out var spawnTask))
                    {
                        spawnTask.msTex?.InitCostTIme();
                    }

                }
            }

        }

    }*/
    private void Awake()
    {


    }

    public void FillParentDict()
    {
       /* foreach (var pair in name_gobj_ParentDict)
        {
            Destroy(pair.Value);
        }*/
        /*name_gobj_ParentDict.Clear();

        //Parent = new GameObject("Parent");
        name_gobj_ParentDict.Add("Character1", new GameObject("Character1"));
        name_gobj_ParentDict.Add("Effect", new GameObject("Effect"));
        name_gobj_ParentDict.Add("Item", new GameObject("Item"));
        name_gobj_ParentDict.Add("Map", new GameObject("Map"));
        name_gobj_ParentDict.Add("Mob", new GameObject("Mob"));
        name_gobj_ParentDict.Add("Npc", new GameObject("Npc"));
        name_gobj_ParentDict.Add("Skill", new GameObject("Skill"));
        name_gobj_ParentDict.Add("UI_083", new GameObject("UI_083"));
        name_gobj_ParentDict.Add("UI_Endless", new GameObject("UI_Endless"));
        name_gobj_ParentDict.Add("Reactor", new GameObject("Reactor"));*/

        

    }
    public Dictionary<string, GameObject> name_gobj_ParentDict = new Dictionary<string, GameObject>();
    private void Start()
    {
        /*gTextInput = new GTextInput ();
        gTextInput.text = "11111111111111111111111111111111";
        gTextInput.border = 5;
        GRoot.inst.AddChild (gTextInput);*/
        //GRoot.inst.container.renderMode = RenderMode.WorldSpace;
        m_DrawObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<DrawObject>("DrawObjectPool", m_InstanceDrawObjectPoolCapacity, m_InstanceDrawObjectPoolExpireTime);
        //m_MaterialObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<MaterialObject>("MaterialObjectPool", m_InstanceMaterialObjectPoolCapacity);

        StartCoroutine(SpawnGObj());
        //StartCoroutine(MsTexInit());

        //GameEntry.Resource.InitResources(OnInitResourcesComplete);
    }

    private bool m_InitResourcesComplete = false;
    private void OnInitResourcesComplete()
    {
        m_InitResourcesComplete = true;
        AppDebug.Log("Init resources complete.");
    }
    private new void Update()
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

public struct SpawnTask
{
    public ms.Texture msTex;

    public SpawnTask(ms.Texture t)
    {
        msTex = t;
    }
}

public struct MsTexInitTask
{
    public ms.Texture msTex;

    public MsTexInitTask(ms.Texture t)
    {
        msTex = t;
    }
}
//}