using System;
using System.Collections.Generic;
using HaCreator.Wz;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using ms;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utility.Inspector;
using Char = ms.Char;
using Color = UnityEngine.Color;
using Timer = ms.Timer;

public class MapleStory : SingletonMono<MapleStory>
{
	private void Start ()
	{
		/* wzFileManager = new WzFileManager();
		 var wzFile = wzFileManager.LoadWzFile(path);
		 var wzObject = wzFile.GetObjectFromPath(subPath);
		 Debug.LogFormat("Width:{0}\t Height:{1}", wzObject?.GetBitmap()?.Width, wzObject?.GetBitmap()?.Height);
		 spriteRenderer.sprite = TextureToSprite(GetTexrture2DFromPath(wzObject));*/
		button_load.onClick.AddListener (OnButtonLoadClick);
		DontDestroyOnLoad (this);
		clearBuffer = new CommandBuffer () { name = "Clear Buffer" };
		//Debug.Log ($"{System.DateTime.Now.ToString("yyyyMMddHH")}");
		//System.DateTime.Parse ("2009010100");
		//Debug.Log (System.DateTime.Parse ("2009010100"));

		/*var str = $"abc\r\ndef\ngh";
		var replaceStr = str.Replace ("\r\n", "\n").Replace ("\n", "\r\n");
		// replaceStr = replaceStr.Replace ("\n", "\r\n");
		Debug.Log (replaceStr);
		Debug.Log (@replaceStr);*/

	}

	private void Update ()
	{
		/*clearBuffer.Clear ();
		clearBuffer.ClearRenderTarget (true, true, clearColor);
		Graphics.ExecuteCommandBuffer (clearBuffer);*/

		Constants.get ().walkSpeed = walkSpeed;
		Constants.get ().jumpSpeed = jumpSpeed;
		Constants.get ().climbSpeed = climbSpeed;
		Constants.get ().flySpeed = flySpeed;
		Constants.get ().fallSpeed = fallSpeed;
		Constants.get ().animSpeed = animSpeed;

		Constants.get ().frameDelay = frameDelay;

		Constants.get ().multiplier_timeStep = multiplier_timeStep;
		loop ();


#if BackgroundStatic
		var playerPos = Stage.get ().get_player ()?.get_position ();
		if (playerPos != null)
			UnityEngine.Camera.main.transform.position = new Vector3 (playerPos.x (), -playerPos.y (), -1);
#endif

	}

	private void OnGUI ()
	{
		if (running ())
		{
			Window.get ().HandleGUIEvents (Event.current);
		}
	}

	private void FixedUpdate ()
	{
		if (running ())
		{
			//update ();
		}
	}

	CommandBuffer clearBuffer;
	public UnityEngine.Color clearColor = UnityEngine.Color.magenta;

	private void OnPostRender ()
	{
	}

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
		maplestoryFolder = Constants.get ().path_MapleStoryFolder;
		Session.get ().init ();
		NxFiles.init (maplestoryFolder);
		Window.get ().init ();

		Char.init ();
		MapPortals.init ();
		Stage.get ().init ();
		UI.get ().init ();
		canStart = true;
		//Stage.get ().load_map(100000000);

		dictionary = DictionaryPool<string, string>.Get ();
		foreach (var item in wz.wzFile_quest["QuestInfo.img"])
		{
		}
		
		//FindChild (wz.wzFile_quest["QuestInfo.img"]);
		FindChild (wz.wzFile_quest["Say.img"]);
		Debug.Log (dictionary.ToDebugLog ());
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
	public void update ()
	{
		Stage.get ().update ();
		UI.get ().update ();
		Session.get ().read ();
		Window.get ().check_events ();
		Window.get ().update ();

	}

	public void draw (float alpha)
	{
		//Window.get().begin();
		GameUtil.Instance.DrawOrder = 0;
		TestURPBatcher.Instance.HideAll ();
		Stage.get ().draw (alpha);
		UI.get ().draw (alpha);
		//Window.get().end();
	}

	private bool canStart = false;

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

	private void loop ()
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
				//Debug.Log ("update");bt`
			}

			// Draw the game. Interpolate to account for remaining time.
			float alpha = Mathf.Clamp01 (accumulator * multiplier_elapsed / timestep);
			//Debug.Log ($"elapsed:{elapsed} \t timestep:{timestep} \t accumulator:{accumulator} \t alpha:{alpha}");
			draw (alpha);
			//Debug.Log ($"deltaTime:{Time.deltaTime * 1000}\t accumulator_before:{accumulator_before}\t elapsed:{elapsed}\t  accumulator_plus_elapsed:{accumulator_plus_elapsed}\t accumulator:{accumulator}\t  alpha:{alpha}");
		}
	}

	private void main ()
	{
#if !UNITY_EDITOR
		maplestoryFolder = inpuField_MapleFolder.text;
		account = inpuField_MapAccount.text;
		password = inpuField_MapPassword.text;
		characterIdToLoad = int.Parse (inpuField_MapCharacter.text);
#endif

		init ();
		/*LoginStartPacket();
		LoginPacket ();
		ServerStatusRequestPacket ();
		CharlistRequestPacket ();
		SelectCharPacket ();*/
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
		main ();
	}

	private GUIStyle labelStyle = new GUIStyle ();

	public int fontSize = 1;
	public Color fontColor = Color.black;
	public bool enable_DrawFootHolder = false;
	public bool enable_DrawAttackRange = false;
	public bool enable_DrawMobRange = false;
	public bool AddToParent = true;
	public Rectangle_short attackRange;
	public Rectangle_short attackRangeAfter;

#if UNITY_EDITOR

	public double viewx;
	public double viewy;
	private void DrawAttackRange ()
	{
		if (attackRange != null && attackRangeAfter != null && !attackRangeAfter.empty ())
		{
	/*		Vector3 center = new Vector3 (attackRange.center ().x (), -attackRange.center ().y ());
			Vector3 size = new Vector3 (attackRange.width (), attackRange.height ());
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube (center, size);*/
			//Debug.Log ($"center:{center}\t size:{size}\t attackRange:{attackRange}");

			Vector3 centerAfter = new Vector3 ((float)(attackRangeAfter.center ().x () + viewx), -(attackRangeAfter.center ().y ()+(float)viewy));
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

				var left = new Vector3 (footHolder.horizontal ().smaller (), -footHolder.vertical ().smaller ());
				var center = new Vector3 (footHolder.horizontal ().center (), -footHolder.vertical ().center ());
				var right = new Vector3 (footHolder.horizontal ().greater (), -footHolder.vertical ().greater ());
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
				Gizmos.color = Color.black;
				Gizmos.DrawWireCube (center, footHoldSize);
				Gizmos.DrawCube (left, Vector3.one * 10);
				//Gizmos.DrawCube (center, Vector3.one * 10);
				Gizmos.DrawCube (right, Vector3.one * 10);
			}
		}

		//Handles.dr
	}

	private void DrawMobRange()
	{
		var mobs = ms.Stage.get ()?.get_mobs ()?.get_mobs();
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
		if (enable_DrawFootHolder)
			DrawFootHolder ();

		if (enable_DrawAttackRange)
			DrawAttackRange ();

		if(enable_DrawMobRange)
		{
			DrawMobRange ();
		}
	}
#endif

	#region Placeholder

	[Button ("main", "Connect")] public string placeholder0;


	[Button ("LoginStartPacket", "LoginStartPacket 35")]
	public string placeholder1;

	void LoginStartPacket ()
	{
		new LoginStartPacket ().dispatch ();
	}

	[Button ("LoginPacket", "LoginPacket 1")]
	public string placeholder2;

	void LoginPacket ()
	{
		new LoginPacket (account, password).dispatch ();
	}

	[Button ("ServerStatusRequestPacket", "ServerStatusRequestPacket 6")]
	public string placeholder3;

	void ServerStatusRequestPacket ()
	{
		new ServerStatusRequestPacket (0).dispatch ();
	}

	[Button ("CharlistRequestPacket", "CharlistRequestPacket 5")]
	public string placeholder4;

	void CharlistRequestPacket ()
	{
		new CharlistRequestPacket (0, 0).dispatch ();
	}

	[Button ("SelectCharPacket", "SelectCharPacket 19")]
	public string placeholder5;

	void SelectCharPacket ()
	{
		new SelectCharPacket (characterIdToLoad).dispatch ();
		if (disableDebugPacketAfterLogin)
			enableDebugPacket = false;
	}

	/*[Utility.Inspector. Button("PlayerLoginPacket","PlayerLoginPacket 20")]
	public string placeholder6;

	void PlayerLoginPacket ()
	{
		new PlayerLoginPacket (1).dispatch ();
	}*/

	#endregion

	#endregion
}