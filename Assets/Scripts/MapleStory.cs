using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HaCreator.Wz;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using ms;
using ms_Unity;
using server.quest;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utility.Inspector;
using Char = ms.Char;
using Color = UnityEngine.Color;
using Timer = ms.Timer;

public class MapleStory : SingletonMono<MapleStory>
{
	#region Placeholder

	[Button("main", "Connect")] public string placeholder0;


	[Button("LoginStartPacket", "LoginStartPacket 35")]
	public string placeholder1;

	void LoginStartPacket()
	{
		new LoginStartPacket().dispatch();
	}

	[Button("LoginPacket", "LoginPacket 1")]
	public string placeholder2;

	void LoginPacket()
	{
		new LoginPacket(account, password).dispatch();
	}

	[Button("ServerStatusRequestPacket", "ServerStatusRequestPacket 6")]
	public string placeholder3;

	void ServerStatusRequestPacket()
	{
		new ServerStatusRequestPacket(0).dispatch();
	}

	[Button("CharlistRequestPacket", "CharlistRequestPacket 5")]
	public string placeholder4;

	void CharlistRequestPacket()
	{
		new CharlistRequestPacket(0, 0).dispatch();
	}

	[Button("SelectCharPacket", "SelectCharPacket 19")]
	public string placeholder5;

	void SelectCharPacket()
	{
		new SelectCharPacket(characterIdToLoad).dispatch();
		if (disableDebugPacketAfterLogin)
			enableDebugPacket = false;
	}

	/*[Utility.Inspector. Button("PlayerLoginPacket","PlayerLoginPacket 20")]
	public string placeholder6;

	void PlayerLoginPacket ()
	{
		new PlayerLoginPacket (1).dispatch ();
	}*/

	[Button("loadAllQuest", "loadAllQuest")]
	public string placeholder6;

	[Button("stopLoadAllQuest", "stopLoadAllQuest")]
	public string placeholder7;

	public string expressionString = "80+3*x";

	public string Argument_Name = "x";
	public double Argument_Value = 1;
	void loadAllQuest()
	{
		/*var e = new org.mariuszgromada.math.mxparser.Expression (expressionString);
		e.addArguments (new org.mariuszgromada.math.mxparser.Argument (Argument_Name, Argument_Value));
		Debug.Log (e.calculate ());

		var toReplace = expressionString.Contains ("y") ? "y"
							: expressionString.Contains ("X") ? "X"
							: "x";

		var je = new Jint.Engine ().Execute (expressionString.Replace (toReplace, Argument_Value + ""));
		Debug.Log (je.GetCompletionValue());*/

		maplestoryFolder = Constants.get().path_MapleStoryFolder;
		NxFiles.init(maplestoryFolder);
		MapleQuest.loadAllQuest();
		//StartCoroutine(MapleQuest.loadAllQuest());
	}
	void stopLoadAllQuest()
	{

		StopAllCoroutines();
	}
	#endregion

	string message;
	public FairyGUI.UIPanel panel_StateLogin;
	public FGUI_StateLogin fGUI_StateLogin;

    public float timeScale = 4.0f;
	public Dictionary<string, UnityEngine.Sprite> tileSprites = new Dictionary<string, UnityEngine.Sprite>();
    protected override void OnAwake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start ()
	{
        Application.memoryUsageChanged += Application_memoryUsageChanged;
		Time.timeScale = timeScale;
        Application.targetFrameRate = 60;
		DebugManager.instance.enableRuntimeUI = false;
		/* wzFileManager = new WzFileManager();
		 var wzFile = wzFileManager.LoadWzFile(path);
		 var wzObject = wzFile.GetObjectFromPath(subPath);
		 Debug.LogFormat("Width:{0}\t Height:{1}", wzObject?.GetBitmap()?.Width, wzObject?.GetBitmap()?.Height);
		 spriteRenderer.sprite = TextureToSprite(GetTexrture2DFromPath(wzObject));*/
		button_load?.onClick.AddListener (OnButtonLoadClick);

		//UnityEngine.SceneManagement.SceneManager.LoadScene (GameSceneName);
		//UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
		var sprites = Resources.LoadAll<UnityEngine.Sprite>("WzPng/Map.wz/Tile");
		foreach (var sprite in sprites)
		{
			tileSprites.Add (sprite.name, sprite);
		}

		fGUI_StateLogin = (FGUI_StateLogin)panel_StateLogin.ui;
        fGUI_StateLogin.StartCheck();
#if UNITY_EDITOR
        /*CopyStreamingAssetToPersistent.CopyResourcesFile ("WZ/", "Map", ".wz");
		CopyStreamingAssetToPersistent.CopyResourcesFile ("WZ/", "Sound", ".wz");
		CopyStreamingAssetToPersistent.CopyResourcesFile ("WZ/", "UI_New", ".wz");

		CopyStreamingAssetToPersistent.CopyFile ("Settings");
		CopyStreamingAssetToPersistent.CopyFile ("Base.wz");
		CopyStreamingAssetToPersistent.CopyFile ("Character1.wz");
		CopyStreamingAssetToPersistent.CopyFile ("Effect.wz");
		CopyStreamingAssetToPersistent.CopyFile ("Etc.wz");


		CopyStreamingAssetToPersistent.CopyFile ("Item.wz");
		CopyStreamingAssetToPersistent.CopyFile ("List.wz");
		//CopyStreamingAssetToPersistent.CopyFile ("Map.wz");
		CopyStreamingAssetToPersistent.CopyFile ("Map001.wz");
		CopyStreamingAssetToPersistent.CopyFile ("MapLatest.wz");
		CopyStreamingAssetToPersistent.CopyFile ("Mob.wz");
		CopyStreamingAssetToPersistent.CopyFile ("Morph.wz");
		CopyStreamingAssetToPersistent.CopyFile ("Npc.wz");
		CopyStreamingAssetToPersistent.CopyFile ("Quest.wz");
		CopyStreamingAssetToPersistent.CopyFile ("Reactor.wz");
		CopyStreamingAssetToPersistent.CopyFile ("Skill.wz");
		//CopyStreamingAssetToPersistent.CopyFile ("Sound.wz");
		CopyStreamingAssetToPersistent.CopyFile ("String.wz");
		//CopyStreamingAssetToPersistent.CopyFile ("UI_New.wz");
		CopyStreamingAssetToPersistent.CopyFile ("UI_Endless.wz");*/
#elif UNITY_ANDROID

		
#endif
        //Debug.Log ($"{System.DateTime.Now.ToString("yyyyMMddHH")}");
        //System.DateTime.Parse ("2009010100");
        //Debug.Log (System.DateTime.Parse ("2009010100"));

        /*var str = $"abc\r\ndef\ngh";
		var replaceStr = str.Replace ("\r\n", "\n").Replace ("\n", "\r\n");
		// replaceStr = replaceStr.Replace ("\n", "\r\n");
		Debug.Log (replaceStr);
		Debug.Log (@replaceStr);*/
    }

	int memoryUsage = 0;
    private void Application_memoryUsageChanged(in ApplicationMemoryUsageChange usage)
    {
        if (usage.memoryUsage == ApplicationMemoryUsage.Critical)
        {
            // release resources here
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }
    }

    private void OnSceneLoaded (Scene arg0, LoadSceneMode arg1)
	{
		canStart = true;
	}

	protected override void OnUpdate ()
	{
		/*clearBuffer.Clear ();
		clearBuffer.ClearRenderTarget (true, true, clearColor);
		Graphics.ExecuteCommandBuffer (clearBuffer);*/
/*
		Constants.get ().walkSpeed = walkSpeed;
		Constants.get ().jumpSpeed = jumpSpeed;
		Constants.get ().climbSpeed = climbSpeed;
		Constants.get ().flySpeed = flySpeed;
		Constants.get ().fallSpeed = fallSpeed;
		Constants.get ().animSpeed = animSpeed;*/

		Constants.get ().frameDelay = frameDelay;

		Constants.get ().multiplier_timeStep = multiplier_timeStep;
		//loop ();

		if (running ())
		{

            var elapsed = Timer.get().stop();

            var accumulator_before = accumulator;
            var accumulator_plus_elapsed = accumulator + elapsed;
            // Update game with constant timestep as many times as possible.
            for (accumulator += elapsed; accumulator >= timestep; accumulator -= timestep)
            {
                //update ();
                //Debug.Log ($"{DateTime.Now.Millisecond}");

            }

            // Draw the game. Interpolate to account for remaining time.
            float alpha = Mathf.Clamp01(accumulator * multiplier_elapsed / timestep);
            //Debug.Log ($"elapsed:{elapsed} \t timestep:{timestep} \t accumulator:{accumulator} \t alpha:{alpha}");
            ms_draw(alpha);

            //Debug.Log ($"deltaTime:{Time.deltaTime * 1000}\t accumulator_before:{accumulator_before}\t elapsed:{elapsed}\t  accumulator_plus_elapsed:{accumulator_plus_elapsed}\t accumulator:{accumulator}\t  alpha:{alpha}");
        }
#if BackgroundStatic
		var playerPos = Stage.get ().get_player ()?.get_position ();
		if (playerPos != null)
			UnityEngine.Camera.main.transform.position = new Vector3 (playerPos.x (), -playerPos.y (), -1);
#endif

	}

    protected override void OnFixedUpdate()
	{
		if (running ())
		{
			ms_update ();

            
        }
	}
	public GameObject RuntimeHierarchyInspector;
	bool initError;

	private bool showNotice = false;
	public System.Action okhandler;
	
	public void ShowGUINotice (string message, Action okHandler = null)
	{
		showNotice = true;
		this.message = message;
		this.okhandler = okHandler;
	}
	
	private void OnGUI ()
	{
		
		GUI.skin.label.fontSize = fontSize;
		GUI.skin.button.fontSize = fontSize;

		//GUI.color = Color.yellow;
		GUILayout.BeginVertical ();

		if (initError)
		{
			if (GUILayout.Button (message))
			{
				initError = false;
			}
		}

		GUILayout.EndVertical ();

		if (running ())
		{
			Window.get ().HandleGUIEvents (Event.current);
		}

		if (showNotice)
		{
			// 在屏幕中央创建一个组
			GUI.BeginGroup (new Rect (Screen.width / 2 - 250, Screen.height / 2 - 150, 500, 300));
			// 现在所有矩形都调整到该组。(0,0) 是该组的左上角。
    
			
			//我们将创建一个框形，以便能看到该组在屏幕上的位置。
			GUI.Box (new Rect (0,0,500,300),"");
			GUI.Label (new Rect (50,0,400,150), message);
			if (GUI.Button (new Rect (150,150,200,100), "确定"))
			{
				BackToLogin ();
				showNotice = false;
				FGUI_Manager.Instance.PanelOpening = false;
				this.okhandler?.Invoke ();
			}
    
			// 结束我们上面开始的组。记住这一点非常重要！
			GUI.EndGroup ();
		}

		var oldColor = GUI.color;
		GUI.color = Color.black;
		GUI.BeginGroup (new Rect (Screen.width / 2, 0, 500, 300));
		GUI.Label (new Rect (0,0,250,300), $"游戏版本：{GameUtil.Instance.Version}");
		//GUI.Label (new Rect (250,0,250,300), $"测试信息：{GameUtil.Instance.debugMessage}");
        GUI.EndGroup ();
		GUI.color = oldColor;
		//Debug.Log ($"{FairyGUI.Stage.inst.touchTarget} {FairyGUI.Stage.inst.touchTarget?.name}", FairyGUI.Stage.inst.touchTarget?.gameObject);

		/*if (GUI.Button (new Rect (200, 0, 200, 100), "开始"))
		{
			main ();
			//UnityEngine.SceneManagement.SceneManager.LoadScene (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name);
		}
		if (GUI.Button (new Rect (400, 0, 200, 100), "显示监视面板"))
		{
			RuntimeHierarchyInspector.SetActive (true);
		}
		if (GUI.Button (new Rect (600, 0, 200, 100), "隐藏监视面板"))
		{
			RuntimeHierarchyInspector.SetActive (false);
		}

		if (GUI.Button (new Rect (400, 100, 200, 100), "Item"))
		{
			CopyStreamingAssetToPersistent.CopyFile ("Item.wz");
		}
		if (GUI.Button (new Rect (600, 100, 200, 100), "List"))
		{
			CopyStreamingAssetToPersistent.CopyFile ("List.wz");
		}
		if (GUI.Button (new Rect (800, 100, 200, 100), "Map"))
		{
			//CopyStreamingAssetToPersistent.CopyFile ("Map.wz");
			CopyStreamingAssetToPersistent.CopyResourcesFile ("WZ/", "Map", ".wz");

		}
		if (GUI.Button (new Rect (1000, 100, 200, 100), "Map001"))
		{
			CopyStreamingAssetToPersistent.CopyFile ("Map001.wz");
		}
		if (GUI.Button (new Rect (1200, 100, 200, 100), "MapLatest"))
		{
			CopyStreamingAssetToPersistent.CopyFile ("MapLatest.wz");
		}

		if (GUI.Button (new Rect (400, 200, 200, 100), "Mob"))
		{
			CopyStreamingAssetToPersistent.CopyFile ("Mob.wz");
		}
		if (GUI.Button (new Rect (600, 200, 200, 100), "Morph"))
		{
			CopyStreamingAssetToPersistent.CopyFile ("Morph.wz");
		}
		if (GUI.Button (new Rect (800, 200, 200, 100), "Npc"))
		{
			CopyStreamingAssetToPersistent.CopyFile ("Npc.wz");
		}
		if (GUI.Button (new Rect (1000, 200, 200, 100), "Quest"))
		{
			CopyStreamingAssetToPersistent.CopyFile ("Quest.wz");
		}
		if (GUI.Button (new Rect (1200, 200, 200, 100), "Reactor"))
		{
			CopyStreamingAssetToPersistent.CopyFile ("Reactor.wz");
		}

		if (GUI.Button (new Rect (400, 300, 200, 100), "Skill"))
		{
			CopyStreamingAssetToPersistent.CopyFile ("Skill.wz");
		}
		if (GUI.Button (new Rect (600, 300, 200, 100), "Sound"))
		{
			//CopyStreamingAssetToPersistent.CopyFile ("Sound.wz");
			CopyStreamingAssetToPersistent.CopyResourcesFile ("WZ/", "Sound", ".wz");

		}
		if (GUI.Button (new Rect (800, 300, 200, 100), "String"))
		{
			CopyStreamingAssetToPersistent.CopyFile ("String.wz");
		}
		if (GUI.Button (new Rect (1000, 300, 200, 100), "TamingMob"))
		{
			CopyStreamingAssetToPersistent.CopyFile ("TamingMob.wz");
		}
		if (GUI.Button (new Rect (1200, 300, 200, 100), "UI_New"))
		{
			//CopyStreamingAssetToPersistent.CopyFile ("UI_New.wz");
			CopyStreamingAssetToPersistent.CopyResourcesFile ("WZ/", "UI_New", ".wz");
		}
		if (GUI.Button (new Rect (1400, 300, 200, 100), "UI_Endless"))
		{
			CopyStreamingAssetToPersistent.CopyFile ("UI_Endless.wz");
		}

		if (GUI.Button (new Rect (400, 400, 200, 100), "Settings"))
		{
			CopyStreamingAssetToPersistent.CopyFile ("Settings");
		}
		if (GUI.Button (new Rect (600, 400, 200, 100), "Base"))
		{
			CopyStreamingAssetToPersistent.CopyFile ("Base.wz");
		}
		if (GUI.Button (new Rect (800, 400, 200, 100), "Character1"))
		{
			CopyStreamingAssetToPersistent.CopyFile ("Character1.wz");
		}
		if (GUI.Button (new Rect (1000, 400, 200, 100), "Effect"))
		{
			CopyStreamingAssetToPersistent.CopyFile ("Effect.wz");
		}
		if (GUI.Button (new Rect (1200, 400, 200, 100), "Etc"))
		{
			CopyStreamingAssetToPersistent.CopyFile ("Etc.wz");
		}*/



	}

	public string GameSceneName = "Game";
	public void init ()
	
	{
		/*if (Error error = Session.get().init())
		return error;

#ifdef USE_NX
		if (Error error = NxFiles.init())
		return error;
#else
		if (Error error = WzFiles.init())
		return error;
#endif

		if (Error error = Window.get().init())
		return error;

		if (Error error = Sound.init())
		return error;

		if (Error error = Music.init())
		return error;

		Char.init();
		DamageNumber.init();
		MapPortals.init();
		*/
		//try
		{
			AssetBundleLoaderMgr.Instance.Init();
            Application.targetFrameRate = 60;
            maplestoryFolder = Constants.get ().path_MapleStoryFolder;
			ms.Setting<ServerIP>.get().save(inputField_IP.text);
			ms.Setting<ServerPort>.get().save(inputField_Port.text);
            Session.get ().init (ms.Setting<ServerIP>.get().load(), ms.Setting<ServerPort>.get().load());
			NxFiles.init (maplestoryFolder);
			Window.get ().init ();
			Sound.init ();
			Music.init ();

			Char.init ();
			DamageNumber.init ();
			MapPortals.init ();
			Stage.get ().init ();
			UI.get ().init ();
			MapleQuest.loadAllQuest ();

			canStart = true;
			button_load.transform.parent.gameObject.SetActive (false);

		}
		/*catch (Exception ex)
		{
			initError = true;
			message = "初始化错误，数据包不存在，或则权限不足无法读取，请开启文件读写权限：";
			message += ex.Message;
			button_load.transform.parent.gameObject.SetActive (true);
		}*/

		//Stage.get ().load_map(100000000);

		/*dictionary = DictionaryPool<string, string>.Get ();
		List<string> strings = new List<string> ();

		foreach (var item in wz.wzFile_quest["QuestInfo.img"])
		{

		}
		foreach (var wzchar_00012000_img in wz.wzFile_character.WzDirectory.WzImages)
		{
			if (wzchar_00012000_img.Name.Contains (".img"))
			{
				foreach (var wzchar_00012000_img_fly in wzchar_00012000_img)
				{
					if (wzchar_00012000_img.Name.Contains ("info"))
					{

					}
					else
					{
						foreach (var wzchar_00012000_img_fly_0 in wzchar_00012000_img_fly)
						{
							bool isaction = wzchar_00012000_img_fly_0["action"] != null;
							if (isaction)
							{
								dictionary.TryAdd (wzchar_00012000_img_fly.Name, wzchar_00012000_img_fly_0["action"].ToString ());
							}
						}
					}
				}
			}
		}*/
		//FindChild (wz.wzFile_quest["QuestInfo.img"]);
		//FindChild (wz.wzFile_quest["Say.img"]);
		//Debug.Log (dictionary.ToDebugLog ());
		//Debug.Log (dictionary.Keys.ToDebugLog ());

	}
	Dictionary<string, string> dictionary;
	private void FindChild (WzObject wzObj)
	{
		if (wzObj == null)
			return;
		foreach (var item in wzObj)
		{
			if (!dictionary.ContainsKey (item.Name)/* && item is not WzSubProperty*/)
			{
				if (!int.TryParse (item.Name, out var result))
				{
					dictionary.Add (item.Name, item.FullPath);

				}

			}
			FindChild (item);
		}
	}

	public bool stage_update = true;
    public bool UI_update = true;
    public bool Session_update = true;
    public bool Window_check_events = true;
    public bool Window_update = true;
    public bool TimerManager_update = true;


    public void ms_update ()
	{
		if (GameUtil.Instance.enableUpdate)
		{
            if (TestURPBatcherHideAll)
                Stage.get ().update ();
            if (TestURPBatcherHideAll)
                UI.get ().update ();
            if (TestURPBatcherHideAll)
                Session.get().read();
            if (TestURPBatcherHideAll)
                Window.get().check_events();
            if (TestURPBatcherHideAll)
                Window.get().update();
            if (TestURPBatcherHideAll)
                TimerManager.get().update();
        }
		
		
	}

    public bool TestURPBatcheraddBefore = true;
    public bool TestURPBatcherHideAll = true;
    public bool TextManagerHideAll = true;
    public bool TextInputManagerHideAll = true;
    public bool Stagedraw = true;
    public bool UIdraw = true;

    public void ms_draw (float alpha)
	{
		//TestURPBatcher.Instance.Log ("before"); 
		if(TestURPBatcheraddBefore)
		TestURPBatcher.Instance.addBefore ();
		//Window.get().begin();
		GameUtil.Instance.DrawOrder = 0;
        if (TestURPBatcherHideAll)
            TestURPBatcher.Instance.HideAll ();
        if (TextManagerHideAll)
            TextManager.Instance.HideAll ();
        if (TextInputManagerHideAll)
            TextInputManager.Instance.HideAll ();
        if (Stagedraw)
            Stage.get ().draw (alpha);
        if (UIdraw)
            UI.get ().draw (alpha);

		if (enable_DebugPlayerPos)
			AppDebug.Log (Stage.get ().get_player ()?.get_position ());
		//Window.get().end();
		//TestURPBatcher.Instance.LogDiff ();
		//TestURPBatcher.Instance.Log ("After");
	}

	public bool canStart = false;

	public bool running ()
	{
		/*return Session.get().is_connected()
		       && UI.get().not_quitted()
		       && Window.get().not_closed();*/
		return canStart && Session.get ().is_connected ()
			/*&& UI.get ().not_quitted ()*/;
	}

	private long timestep => (long)(Constants.TIMESTEP * multiplier_timeStep);

	//private static long timestep = (long)(8 * 1000 * 1);
	private long accumulator = 0;

	/*private void loop ()
	{
		if (running ())
		{
			var elapsed = Timer.get ().stop ();

			var accumulator_before = accumulator;
			var accumulator_plus_elapsed = accumulator + elapsed;
			// Update game with constant timestep as many times as possible.
			for (accumulator += elapsed; accumulator >= timestep; accumulator -= timestep)
			{
				update ();
				Debug.Log ($"{DateTime.Now.Millisecond}");

			}

			// Draw the game. Interpolate to account for remaining time.
			float alpha = Mathf.Clamp01 (accumulator * multiplier_elapsed / timestep);
			//Debug.Log ($"elapsed:{elapsed} \t timestep:{timestep} \t accumulator:{accumulator} \t alpha:{alpha}");
			//draw (alpha);
			//Debug.Log ($"deltaTime:{Time.deltaTime * 1000}\t accumulator_before:{accumulator_before}\t elapsed:{elapsed}\t  accumulator_plus_elapsed:{accumulator_plus_elapsed}\t accumulator:{accumulator}\t  alpha:{alpha}");
		}
	}*/

	public void BackToLogin ()
	{
		//Window.get().ChangeResloution((short)800, (short)600);
		float fadestep = 0.025f;

		Window.get().fadeout(fadestep, () =>
		{
			ms.GraphicsGL.get().clear();

			UI.get().change_state(UI.State.LOGIN);
            TestURPBatcher.Instance.Clear();

            UI.get().set_scrollnotice("");
			Session.get().reconnect();

			UI.get().enable();
			Timer.get().start();
			ms.GraphicsGL.get().unlock();
        });

		ms.GraphicsGL.get().enlock();
		Stage.get().clear();
		Timer.get().start();
		ms_Unity.FGUI_Manager.Instance.CloseAll ();
	}
	private void OnApplicationQuit ()
	{
		Sound.close ();
		Configuration.get ().save ();
		//Music.get ().Quit ();
	}
	#region Will be removed later

	public RenderTexture target;

	private GameObject map_Parent;
	private GameObject mob_Parent;
	private GameObject character_Parent;
	private GameObject effect_Parent;

	public GameObject Map_Parent => map_Parent ?? (map_Parent = new GameObject ("map_Parent"));
	public GameObject Mob_Parent => mob_Parent ?? (mob_Parent = new GameObject ("mob_Parent"));
	public GameObject Character_Parent => character_Parent ?? (character_Parent = new GameObject ("character_Parent"));
	public GameObject Effect_Parent => effect_Parent ?? (effect_Parent = new GameObject ("effect_Parent"));

	public GameObject prefab_SpriteDrawer;

	public bool enableDebugPacket = true;
	public bool disableDebugPacketAfterLogin = true;
	public UnityEngine.UI.Button button_load;
	public TMP_InputField inputField_IP;
	public TMP_InputField inputField_Port;
	
    public InputField inpuField_MapleFolder;

	[FormerlySerializedAs ("inpuField_MapId")]
	public InputField inpuField_MapAccount;

	public InputField inpuField_MapPassword;
	public InputField inpuField_MapCharacter;

	public string maplestoryFolder = @"F:\Program Files (x86)\MapleStory\"; //F:\Program Files (x86)\MapleStory\ ;F:/BaiduYunDownload/079mg5/
	public string account = "admin";
	public string password = "admin";
	public int characterIdToLoad = 1;

	private WzFileManager wzFileManager;

	public float walkSpeed = 1;
	public float jumpSpeed = 1;
	public float climbSpeed = 1;
	public float flySpeed = 1;
	public float fallSpeed = 1;
	public float animSpeed = 1;

	public float frameDelay = 0.5f;

	public float multiplier_timeStep = 1f;
	public float multiplier_elapsed = 1f;

	private string fds;

	void TempLogin ()
	{
	}

	private void OnButtonLoadClick ()
	{
        //Configuration.get ().set_hwid ("2EFDB98799DD_CB4F4F88", ref fds);
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Game", UnityEngine.SceneManagement.LoadSceneMode.Single);
        init ();
	}

	private GUIStyle labelStyle = new GUIStyle ();

	public int fontSize = 1;
	public Color fontColor = Color.black;
	public bool enable_DrawFootHolder = false;
	public bool enable_DrawAttackRange = false;
	public bool enable_DrawMobRange = false;
	public bool enable_DebugPlayerPos = false;
	public bool AddToParent = true;
	public Rectangle_short attackRange;
	public Rectangle_short attackRangeAfter;
	public double viewx;
	public double viewy;
	public float alpha;

#if UNITY_EDITOR


	private void DrawAttackRange ()
	{
		if (attackRange != null && attackRangeAfter != null && !attackRangeAfter.empty ())
		{
/*			Vector3 center = new Vector3 (attackRange.center ().x (), -attackRange.center ().y ());
			Vector3 size = new Vector3 (attackRange.width (), attackRange.height ());
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube (center, size);
			Debug.Log ($"center:{center}\t size:{size}\t attackRange:{attackRange}");*/

			Vector3 centerAfter = new Vector3 ((float)(attackRangeAfter.center ().x () + viewx), -(attackRangeAfter.center ().y () + (float)viewy));
			Vector3 sizeAfter = new Vector3 (attackRangeAfter.width (), attackRangeAfter.height ());
			Gizmos.color = Color.black;
			Gizmos.DrawWireCube (centerAfter, sizeAfter);
			//Debug.Log ($"centerAfter:{centerAfter}\t sizeAfter:{sizeAfter}\t attackRangeAfter:{attackRangeAfter}");
		}
	}

	private void DrawFootHolder ()
	{
		var footholdtree = Stage.get ().GetFootholdTree ();
		if (footholdtree != null)
		{
			foreach (var pair in footholdtree.Footholds)
			{
				var id = pair.Key;
				var footHolder = pair.Value;
				//Handles.RectangleHandleCap ();

				var left = new Vector3 ((float)(footHolder.horizontal ().smaller ()+viewx), (float)(-footHolder.vertical ().smaller ()-viewy));
				var center = new Vector3 ((float)(footHolder.horizontal ().center ()+viewx), (float)(-footHolder.vertical ().center ()-viewy));
				var right = new Vector3 ((float)(footHolder.horizontal ().greater ()+viewx), (float)(-footHolder.vertical ().greater ()-viewy));
				var footHoldSize = new Vector3 (footHolder.horizontal ().length (), footHolder.vertical ().length ());
				//Debug.Log ($"Bound.Center:{center}\tBound.size:{size}");

				Handles.BeginGUI ();

				var text_Size = new Vector2 (40, 20);
				var center_ScreenPosition = HandleUtility.WorldToGUIPoint (center);
				var center_ScreenPosition_MinusWidth = new Vector2 (center_ScreenPosition.x - text_Size.x / 2, center_ScreenPosition.y);
				var left_ScreenPosition = HandleUtility.WorldToGUIPoint (left);
				var Right_ScreenPosition = HandleUtility.WorldToGUIPoint (right);
				labelStyle.fontSize = fontSize;
				labelStyle.normal.textColor = fontColor;
				labelStyle.fontStyle = FontStyle.Bold;
				//GUI.backgroundColor = Color.black;
				//GUI.color = Color.red;
				GUI.Label (new Rect (center_ScreenPosition_MinusWidth, text_Size), id.ToString (), labelStyle);
				//GUI.backgroundColor = Color.white;
				//GUI.Label (new Rect (left_ScreenPosition, 5 * Vector2.one), "");
				//GUI.Label (new Rect (Right_ScreenPosition, 5 * Vector2.one), "");
				Handles.EndGUI ();

				//Gizmos.color = new Color (id % 2, id % 2, id % 2, id % 2);
				Gizmos.color = Color.blue ;
				Gizmos.DrawWireCube (center, footHoldSize);
				Gizmos.DrawCube (left, Vector3.one * 10);
				//Gizmos.DrawCube (center, Vector3.one * 10);
				Gizmos.DrawCube (right, Vector3.one * 10);
			}
		}

		//Handles.dr
	}

	private void DrawMobRange ()
	{
		var mobs = ms.Stage.get ()?.get_mobs ()?.get_mobs ();
		if (mobs == null)
			return;
		foreach (var pair in mobs)
		{
			if (pair.Value is Mob mob)
			{
				var mobRange = mob.get_Range ();
				Vector3 center = new Vector3 (mobRange.center ().x (), -mobRange.center ().y ());
				Vector3 size = new Vector3 (mobRange.width (), mobRange.height ());
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireCube (center, size);
				//Debug.Log ($"center:{center}");
			}
		}
	}

	private void OnDrawGizmos ()
	{
		if (UnityEngine.Camera.current != UnityEngine.Camera.main)
		{
			return;
		}

		if (enable_DrawFootHolder)
			DrawFootHolder ();

		if (enable_DrawAttackRange)
			DrawAttackRange ();

		if (enable_DrawMobRange)
		{
			DrawMobRange ();
		}
	}
#endif

	

	#endregion
}