using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using HaRepacker;
using ms;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Utility.Inspector;
using Camera = ms.Camera;
using Char = ms.Char;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;
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
		//button_load.onClick.AddListener (main);
		DontDestroyOnLoad (this);
		clearBuffer = new CommandBuffer () {name = "Clear Buffer"};
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


		/*UI.get ().send_key (KeyType.Id.ACTION, (int)KeyAction.Id.LEFT, Input.GetKey (KeyCode.LeftArrow));

		UI.get ().send_key (KeyType.Id.ACTION, (int)KeyAction.Id.RIGHT, Input.GetKey (KeyCode.RightArrow));

		UI.get ().send_key (KeyType.Id.ACTION, (int)KeyAction.Id.UP, Input.GetKey (KeyCode.UpArrow));

		UI.get ().send_key (KeyType.Id.ACTION, (int)KeyAction.Id.DOWN, Input.GetKey (KeyCode.DownArrow));

		UI.get ().send_key (KeyType.Id.ACTION, (int)KeyAction.Id.JUMP, Input.GetKey (KeyCode.LeftAlt));
		UI.get ().send_key (KeyType.Id.ACTION, (int)KeyAction.Id.ATTACK, Input.GetKey (KeyCode.LeftControl));*/

		/*UI.get ().send_key ((int)KeyType.Id.ACTION,  Input.GetKey (KeyCode.LeftArrow));

		UI.get ().send_key ((int)KeyType.Id.ACTION,  Input.GetKey (KeyCode.RightArrow));

		UI.get ().send_key ((int)KeyType.Id.ACTION,  Input.GetKey (KeyCode.UpArrow));

		UI.get ().send_key ((int)KeyType.Id.ACTION,  Input.GetKey (KeyCode.DownArrow));

		UI.get ().send_key ((int)KeyType.Id.ACTION,  Input.GetKey (KeyCode.LeftAlt));
		UI.get ().send_key ((int)KeyType.Id.ACTION,  Input.GetKey (KeyCode.LeftControl));*/
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

	private void init ()
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
		Session.get ().init ();
		NxFiles.init (maplestoryFolder);
		Window.get ().init ();

		Char.init ();
		MapPortals.init ();
		Stage.get ().init ();
		//UI.get ().init ();
		TempLogin ();
	}

	private void update ()
	{
		Stage.get ().update ();
		//UI.get ().update ();
		Session.get ().read ();
		Window.get ().check_events ();
		Window.get ().update ();
		/*
		UI.get().update();
		Session.get().read();*/

		var playerPos = Stage.get ().get_player ()?.get_position ();
		if (playerPos != null)
			UnityEngine.Camera.main.transform.position = new Vector3 (playerPos.x (), -playerPos.y (), -1);
	}

	private void draw (float alpha)
	{
		//Window.get().begin();
		Stage.get ().draw (alpha);
		//UI.get ().draw (alpha);
		//Window.get().end();
	}

	private bool canStart = false;

	private bool running ()
	{
		/*return Session.get().is_connected()
		       && UI.get().not_quitted()
		       && Window.get().not_closed();*/
		return canStart && Session.get ().is_connected ()
			/*&& UI.get ().not_quitted ()*/;
	}

	private long timestep => (long)(Constants.TIMESTEP * Constants.get ().multiplier_timeStep);

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
		//maplestoryFolder = inpuField_MapleFolder.text;
		//mapIdToLoad = int.Parse (inpuField_MapId.text);

		init ();
		canStart = true;

		/*var charEntry = parse_charentry (new InPacket (new byte[NetConstants.HEADER_LENGTH], 0));
		Stage.get ().loadplayer (charEntry);
		Stage.get ().load (mapIdToLoad, 0);
		Stage.get().get_mobs().spawn(new MobSpawn (1000000002,120100,0,5,135,false,-1,new ms.Point<short> (157,214)));*/

		//nx.load_all(maplestoryFolder);
		//Stage.get().load_map(mapIdToLoad);
		//Stage.get().draw(1);
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


	public bool enableDebugPacket = true;
	public bool disableDebugPacketAfterLogin = true;
	public UnityEngine.UI.Button button_load;

	public InputField inpuField_MapleFolder;
	public InputField inpuField_MapId;

	public string maplestoryFolder = @"F:\Program Files (x86)\MapleStory\";//F:\Program Files (x86)\MapleStory\ ;F:/BaiduYunDownload/079mg5/
	public int mapIdToLoad = 100000000;

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
		//Configuration.get ().set_hwid ("2EFDB98799DD_CB4F4F88", ref fds);
	}

	private void OnButtonLoadClick ()
	{
	}

	private CharEntry parse_charentry (InPacket recv)
	{
		int cid = 0;
		StatsEntry stats = parse_stats (recv);
		LookEntry look = parse_look (recv);

		//recv.read_bool(); // 'rankinfo' bool

		/*if (recv.read_bool())
		{
		    int currank = recv.read_int();
		    int rankmv = recv.read_int();
		    int curjobrank = recv.read_int();
		    int jobrankmv = recv.read_int();
		    sbyte rankmc = (rankmv > 0) ? '+' : (rankmv < 0) ? '-' : '=';
		    sbyte jobrankmc = (jobrankmv > 0) ? '+' : (jobrankmv < 0) ? '-' : '=';

		    stats.rank =new Tuple<int, sbyte> (currank, rankmc);
		    stats.jobrank = new Tuple<int, sbyte> (curjobrank, jobrankmc);
		}*/

		return new CharEntry (stats, look, cid);
	}

	private LookEntry parse_look (InPacket recv)
	{
		LookEntry look = new LookEntry ();

		look.female = false;
		look.skin = (byte)0;
		look.faceid = 20000;

		//recv.read_bool(); // megaphone

		look.hairid = 30000;

		/*byte eqslot = (byte)recv.read_byte();

		while (eqslot != 0xFF)
		{
		    look.equips[eqslot] = recv.read_int();
		    eqslot = (byte)recv.read_byte();
		}*/

		/*byte mskeqslot = (byte)recv.read_byte();

		while (mskeqslot != 0xFF)
		{
		    look.maskedequips[(sbyte)mskeqslot] = recv.read_int();
		    mskeqslot = (byte)recv.read_byte();
		}*/

		look.maskedequips[-111] = 0;

		for (byte i = 0; i < 3; i++)
			look.petids.Add (i);

		return look;


		/*LookEntry look = new LookEntry ();

		look.female = recv.read_bool();
		look.skin = (byte)recv.read_byte();
		look.faceid = recv.read_int();

		recv.read_bool(); // megaphone

		look.hairid = recv.read_int();

		byte eqslot = (byte)recv.read_byte();

		while (eqslot != 0xFF)
		{
		    look.equips[eqslot] = recv.read_int();
		    eqslot = (byte)recv.read_byte();
		}

		byte mskeqslot = (byte)recv.read_byte();

		while (mskeqslot != 0xFF)
		{
		    look.maskedequips[(sbyte)mskeqslot] = recv.read_int();
		    mskeqslot = (byte)recv.read_byte();
		}

		look.maskedequips[-111] = recv.read_int();

		for (byte i = 0; i < 3; i++)
		    look.petids.Add(recv.read_int());

		return look;*/
	}

	private StatsEntry parse_stats (InPacket recv)
	{
		StatsEntry statsentry = new StatsEntry ();

		statsentry.name = "HelloWorld";
		statsentry.female = false;

		/*recv.read_byte();	// skin
		recv.read_int();	// face
		recv.read_int();	// hair*/

		for (var i = 0; i < 3; i++)
			statsentry.petids.Add (i);

		statsentry.stats[MapleStat.Id.LEVEL] = (ushort)1; // TODO: Change to recv.read_short(); to increase level cap
		statsentry.stats[MapleStat.Id.JOB] = (ushort)100;
		statsentry.stats[MapleStat.Id.STR] = (ushort)4;
		statsentry.stats[MapleStat.Id.DEX] = (ushort)4;
		statsentry.stats[MapleStat.Id.INT] = (ushort)4;
		statsentry.stats[MapleStat.Id.LUK] = (ushort)4;
		statsentry.stats[MapleStat.Id.HP] = (ushort)10;
		statsentry.stats[MapleStat.Id.MAXHP] = (ushort)10;
		statsentry.stats[MapleStat.Id.MP] = (ushort)10;
		statsentry.stats[MapleStat.Id.MAXMP] = (ushort)10;
		statsentry.stats[MapleStat.Id.AP] = (ushort)5;
		statsentry.stats[MapleStat.Id.SP] = (ushort)5;
		statsentry.exp = 0;
		statsentry.stats[MapleStat.Id.FAME] = (ushort)5;

		//recv.skip(4); // gachaexp

		statsentry.mapid = 100000000;
		statsentry.portal = (byte)0;

		//recv.skip(4); // timestamp

		return statsentry;

		/*// TODO: This is similar to CashShopParser.cpp, try and merge these.
		StatsEntry statsentry = new StatsEntry ();

		statsentry.name = recv.read_padded_string(13);
		statsentry.female = recv.read_bool();

		recv.read_byte();	// skin
		recv.read_int();	// face
		recv.read_int();	// hair

		for (var i = 0; i < 3; i++)
		    statsentry.petids.Add(recv.read_long());

		statsentry.stats[ms.MapleStat.Id.LEVEL] = (ushort)recv.read_byte(); // TODO: Change to recv.read_short(); to increase level cap
		statsentry.stats[ms.MapleStat.Id.JOB] = (ushort)recv.read_short();
		statsentry.stats[ms.MapleStat.Id.STR] = (ushort)recv.read_short();
		statsentry.stats[ms.MapleStat.Id.DEX] = (ushort)recv.read_short();
		statsentry.stats[ms.MapleStat.Id.INT] = (ushort)recv.read_short();
		statsentry.stats[ms.MapleStat.Id.LUK] = (ushort)recv.read_short();
		statsentry.stats[ms.MapleStat.Id.HP] = (ushort)recv.read_short();
		statsentry.stats[ms.MapleStat.Id.MAXHP] = (ushort)recv.read_short();
		statsentry.stats[ms.MapleStat.Id.MP] = (ushort)recv.read_short();
		statsentry.stats[ms.MapleStat.Id.MAXMP] = (ushort)recv.read_short();
		statsentry.stats[ms.MapleStat.Id.AP] = (ushort)recv.read_short();
		statsentry.stats[ms.MapleStat.Id.SP] = (ushort)recv.read_short();
		statsentry.exp = recv.read_int();
		statsentry.stats[ms.MapleStat.Id.FAME] = (ushort)recv.read_short();

		recv.skip(4); // gachaexp

		statsentry.mapid = recv.read_int();
		statsentry.portal = (byte)recv.read_byte();

		recv.skip(4); // timestamp

		return statsentry;*/
	}


	private GUIStyle labelStyle = new GUIStyle ();

	public int fontSize = 1;
	public Color fontColor = Color.black;
	public bool enable_DrawFootHolder = false;
	public bool enable_DrawAttackRange = false;
	public bool AddToParent = true;
	public Rectangle<short> attackRange;
	public Rectangle<short> attackRangeAfter;

	private void DrawAttackRange ()
	{
		if (attackRange != null && attackRangeAfter != null && !attackRangeAfter.empty ())
		{
			Vector3 center = new Vector3 (attackRange.center ().x (), -attackRange.center ().y ());
			Vector3 size = new Vector3 (attackRange.width (), attackRange.height ());
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube (center, size);
			//Debug.Log ($"center:{center}\t size:{size}\t attackRange:{attackRange}");

			Vector3 centerAfter = new Vector3 (attackRangeAfter.center ().x (), -attackRangeAfter.center ().y ());
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

	private void OnDrawGizmos ()
	{
		if (enable_DrawFootHolder)
			DrawFootHolder ();

		if (enable_DrawAttackRange)
			DrawAttackRange ();
	}

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
		new LoginPacket ("admin", "admin").dispatch ();
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
		new SelectCharPacket (4).dispatch ();
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