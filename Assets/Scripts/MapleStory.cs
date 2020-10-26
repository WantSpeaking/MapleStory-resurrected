using HaRepacker;
using ms;
using UnityEngine;
using UnityEngine.UI;
using Char = ms.Char;

public class MapleStory : MonoBehaviour
{
	public UnityEngine.UI.Button button_load;

	public InputField inpuField_MapleFolder;
	public InputField inpuField_MapId;

	public string maplestoryFolder = @"F:/BaiduYunDownload/079mg5/";
	public int mapIdToLoad = 100000000;

	private WzFileManager wzFileManager;

	private void Start ()
	{
		/* wzFileManager = new WzFileManager();
		 var wzFile = wzFileManager.LoadWzFile(path);
		 var wzObject = wzFile.GetObjectFromPath(subPath);
		 Debug.LogFormat("Width:{0}\t Height:{1}", wzObject?.GetBitmap()?.Width, wzObject?.GetBitmap()?.Height);
		 spriteRenderer.sprite = TextureToSprite(GetTexrture2DFromPath(wzObject));*/
		button_load.onClick.AddListener (main);
		DontDestroyOnLoad (this);
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
		//Session.get().init();
		//Stage.get().init();
		//UI.get().init();
		NxFiles.init (maplestoryFolder);
		Char.init ();
		MapPortals.init ();
		Stage.get ().init ();
	}

	public float walkSpeed = 1;
	public float jumpSpeed = 1;
	public float climbSpeed = 1;
	public float flySpeed = 1;
	public float fallSpeed = 1;
	public float animSpeed = 1;

	public float frameDelay = 0.5f;

	private void Update ()
	{
		Constants.get ().walkSpeed = walkSpeed;
		Constants.get ().jumpSpeed = jumpSpeed;
		Constants.get ().climbSpeed = climbSpeed;
		Constants.get ().flySpeed = flySpeed;
		Constants.get ().fallSpeed = fallSpeed;
		Constants.get ().animSpeed = animSpeed;

		Constants.get ().frameDelay = frameDelay;
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

	private void update ()
	{
		Stage.get ().update ();

		/*Window.get().check_events();
		Window.get().update();
		UI.get().update();
		Session.get().read();*/
	}

	private void draw (float alpha)
	{
		//Window.get().begin();
		Stage.get ().draw (alpha);
		//UI.get().draw(alpha);
		//Window.get().end();
	}

	private bool canStart = false;

	private bool running ()
	{
		return canStart;
	}

	private static long timestep = Constants.TIMESTEP * 1000;
	private long accumulator = timestep;

	private long period = 0;
	private int samples = 0;

	private void loop ()
	{
		if (running ())
		{
			var elapsed = Timer.get ().stop ();

			// Update game with constant timestep as many times as possible.
			for (accumulator += elapsed; accumulator >= timestep; accumulator -= timestep)
			{
			}

			update ();
			// Draw the game. Interpolate to account for remaining time.
			float alpha = (float)(accumulator) / timestep;
			//Debug.Log ($"elapsed:{elapsed} \t timestep:{timestep} \t accumulator:{accumulator} \t alpha:{alpha}");
			draw (alpha);
		}
	}

	private void main ()
	{
		maplestoryFolder = inpuField_MapleFolder.text;
		mapIdToLoad = int.Parse (inpuField_MapId.text);

		init ();
		var charEntry = parse_charentry (new InPacket (new byte[NetConstants.HEADER_LENGTH], 0));
		Stage.get ().loadplayer (charEntry);
		Stage.get ().load (mapIdToLoad, 0);
		canStart = true;

		Stage.get().get_mobs().spawn(new MobSpawn (1000000002,120100,0,5,135,false,-1,new ms.Point<short> (157,214)));
		//nx.load_all(maplestoryFolder);
		//Stage.get().load_map(mapIdToLoad);
		//Stage.get().draw(1);


		//Stage.get().draw(1);
	}
	#region Will be removed later

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


	private void DrawBound ()
	{
		var camer = Stage.get ().camera;
		var center = new Vector3 ((float)camer.x.get (1), (float)camer.y.get (1));
		var size = new Vector3 ((float)camer.hbounds.delta (), (float)camer.vbounds.delta ());
		//Debug.Log ($"Bound.Center:{center}\tBound.size:{size}");
		Gizmos.DrawWireCube (center, size);
	}

	private void OnDrawGizmos ()
	{
		DrawBound ();
	}

	#endregion
}