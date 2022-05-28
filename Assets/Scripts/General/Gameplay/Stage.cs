using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaCreator;
using HaCreator.MapEditor;
using HaCreator.MapSimulator;
using HaCreator.Wz;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using ms;

namespace ms
{
	internal class Stage : Singleton<Stage>
	{
		//private  map
		public Stage ()
		{
			state = State.INACTIVE;
			combat = new Combat (player, chars, mobs, reactors);
		}

		public void init ()
		{
			drops.init ();
		}

		public void load (int mapid, sbyte portalid)
		{
			AppDebug.Log ($"load mapid:{mapid}\t portalid:{portalid}");
			switch (state)
			{
				case State.INACTIVE:
					load_map (mapid);
					respawn (portalid);
					break;
				case State.TRANSITION:
					respawn (portalid);
					break;
			}

			state = State.ACTIVE;
		}

		public void loadplayer (CharEntry entry)
		{
			player = new Player (entry);
			playable = new Optional<Playable> (player);

			start = ContinuousTimer.get ().start ();

			CharStats stats = player.get_stats ();
			levelBefore = stats.get_stat (MapleStat.Id.LEVEL);
			expBefore = stats.get_exp ();
		}

		public void clear ()
		{
			state = State.INACTIVE;

			chars.clear ();
			npcs.clear ();
			mobs.clear ();
			drops.clear ();
			reactors.clear ();
		}

		private void load_map (int mapid) //mapid:100000000
		{
			//SceneManager.LoadScene ("Game", LoadSceneMode.Single);
			//SpriteBatch.Instance.ClearSpriteRendererDict ();

			this.mapid = mapid;

			//string strid = mapid.ToString (); //strid:100000000
			string strid = string_format.extend_id (mapid, 9);
			string prefix = Convert.ToString (mapid / 100000000); //prefix:1

			//var src = nl.nx.mapWz.ResolvePath("Map").ResolvePath($"Map{prefix}").ResolvePath($"{strid}.img"); //srcNodePath:Map/Map1/100000000.img
			var node_100000000img = mapid == -1 ? ms.wz.wzFile_ui["CashShopPreview.img"] : ms.wz.wzFile_map["Map"][$"Map{prefix}"][$"{strid}.img"];

			//var node_100000000img = nl.nx.wzFile_map["Map"]["Map" + prefix][strid + ".img"]; //srcNodePath:Map/Map1/100000000.img

			tilesobjs = new MapTilesObjs (node_100000000img);
			backgrounds = new MapBackgrounds (node_100000000img["back"]); //back2NodePath:Map/Map1/100000000.img/back
			physics = new Physics (node_100000000img["foothold"]);
			mapinfo = new MapInfo (node_100000000img, physics.get_fht ().get_walls (), physics.get_fht ().get_borders ());
			portals = new MapPortals (node_100000000img["portal"], mapid);

			#region HaCreater
			WzImage mapImage = null;
			//int mapid = -1;
			string mapName = null, streetName = "", categoryName = "";
			WzSubProperty strMapProp = null;
			var SelectedItem = "";
			string mapid_str = strid;
			int.TryParse (mapid_str, out mapid);

			string mapcat = "Map" + mapid_str.Substring (0, 1);

			WzDirectory directory = wz.WzManager.FindMapWz (mapcat);
			mapImage = (WzImage)(mapid == -1 ? ms.wz.wzFile_ui["CashShopPreview.img"] : (WzImage)directory[mapid_str + ".img"]);

			strMapProp = WzInfoTools.GetMapStringProp (mapid_str);
			mapName = WzInfoTools.GetMapName (strMapProp);
			streetName = WzInfoTools.GetMapStreetName (strMapProp);
			categoryName = WzInfoTools.GetMapCategoryName (strMapProp);
			MapLoader.CreateMapFromImage (mapid, mapImage, mapName, streetName, categoryName, strMapProp, multiBoard);

			maplesimulator = new HaCreator.MapSimulator.MapSimulator (multiBoard.SelectedBoard, "");
			maplesimulator.spriteBatch = GameUtil.Instance.Batch;
			maplesimulator._DxDeviceManager = GameUtil.Instance.GraphicsManager;
			maplesimulator.Initialize();
			maplesimulator.LoadContent();
			#endregion
		}

		private void respawn (sbyte portalid)
		{
			Music.get ().play (mapinfo.get_bgm ());

			Point_short spawnpoint = portals.get_portal_by_id ((byte)portalid);
			Point_short startpos = physics.get_y_below (new Point_short (spawnpoint));
			player.respawn (new Point_short (startpos), mapinfo?.is_underwater () ?? false);
			camera.set_position (new Point_short (startpos));
			camera.set_view (mapinfo?.get_walls (), mapinfo?.get_borders ());
		}

		public void draw (float alpha)
		{
			if (state != State.ACTIVE)
				return;
#if BackgroundStatic
			Point_short viewpos = Point_short.zero;
#else
			Point_short viewpos = camera.position (alpha);
#endif

			Point_double viewrpos = camera.realposition (alpha);
			double viewx = viewrpos.x ();
			double viewy = viewrpos.y ();

			//backgrounds?.drawbackgrounds (viewx, viewy, alpha);
			maplesimulator?.Draw (GameUtil.Instance.Game.gameTime);

			foreach (Layer.Id id in Enum.GetValues (typeof (Layer.Id)))
			{
				//tilesobjs?.draw (id, new Point_short (viewpos), alpha);
				//reactors?.draw (id, viewx, viewy, alpha);
				//npcs?.draw (id, viewx, viewy, alpha);
				mobs?.draw (id, viewx, viewy, alpha);
				chars?.draw (id, viewx, viewy, alpha);
				player?.draw (id, viewx, viewy, alpha);
				drops?.draw (id, viewx, viewy, alpha);
			}

			combat?.draw (viewx, viewy, alpha);
			//portals?.draw (new Point_short (viewpos), alpha);
			//backgrounds?.drawforegrounds (viewx, viewy, alpha);
			effect?.draw ();
		}

		public void update ()
		{
			if (state != State.ACTIVE)
				return;

			//if (player == null) return;

			combat.update ();
			//backgrounds.update ();
			maplesimulator?.Update (GameUtil.Instance.Game.gameTime, camera);
			effect?.update ();
			tilesobjs.update ();

			reactors.update (physics);
			npcs.update (physics);
			mobs.update (physics);
			chars.update (physics);
			drops.update (physics);
			player.update (physics);

			portals.update (player.get_position ());
			camera.update (player.get_position ());
			AppDebug.Log($"player pos: {player.get_position()} camera.position:{camera.position(1)}");
			if (!player.is_climbing () && !player.is_sitting () && !player.is_attacking ())
			{
				if (player.is_key_down (KeyAction.Id.UP) && !player.is_key_down (KeyAction.Id.DOWN))
					check_ladders (true);

				if (player.is_key_down (KeyAction.Id.UP))
					check_portals ();

				if (player.is_key_down (KeyAction.Id.DOWN))
					check_ladders (false);

				if (player.is_key_down (KeyAction.Id.ATTACK))
				{
					combat.use_move (0);
				}

				if (player.is_key_down (KeyAction.Id.SIT))
					check_seats ();

				if (player.is_key_down (KeyAction.Id.PICKUP))
					check_drops ();
			}

			if (player.is_invincible ())
				return;

			/*int oid_id = mobs.find_colliding (player.get_phobj ());
			if (oid_id != 0)
			{
				MobAttack attack = mobs.create_attack (oid_id);
				if (attack != null)
				{
					MobAttackResult result = player.damage (attack);
					new TakeDamagePacket(result, TakeDamagePacket.From.TOUCH).dispatch();
				}
			}*/
		}

		public void show_character_effect (int cid, CharEffect.Id effect)
		{
			var character = get_character (cid);
			if (character)
				character.get ().show_effect_id (effect);
		}

		private void check_portals ()
		{
			if (player.is_attacking ())
				return;

			Point_short playerpos = player.get_position ();
			Portal.WarpInfo warpinfo = portals.find_warp_at (new Point_short (playerpos));

			if (warpinfo.intramap)
			{
				Point_short spawnpoint = portals.get_portal_by_name (warpinfo.toname);
				Point_short startpos = physics.get_y_below (new Point_short (spawnpoint));

				player.respawn (new Point_short (startpos), mapinfo.is_underwater ());
			}
			else if (warpinfo.valid)
			{
				new ChangeMapPacket (false, -1, warpinfo.name, false).dispatch ();

				CharStats stats = get ().get_player ().get_stats ();

				stats.set_mapid (warpinfo.mapid);

				new Sound (Sound.Name.PORTAL).play ();
			}
		}

		private void check_seats ()
		{
			if (player.is_sitting () || player.is_attacking ())
				return;

			Optional<Seat> seat = mapinfo.findseat (player.get_position ());//todo 2 might simplify
			player.set_seat (new Optional<Seat> (seat?.get ()));
		}

		private void check_ladders (bool up)
		{
			if (!player.can_climb () || player.is_climbing () || player.is_attacking ())
				return;

			Optional<Ladder> ladder = mapinfo.findladder (player.get_position (), up);
			player.set_ladder (ladder);
		}

		private void check_drops ()
		{
			Point_short playerpos = player.get_position ();
			var loot = drops.find_loot_at (new Point_short (playerpos));

			if (loot.Item1 != 0)
				new PickupItemPacket (loot.Item1, loot.Item2).dispatch ();
		}

		public void send_key (KeyType.Id type, int action, bool down)
		{
			if (state != State.ACTIVE || !(bool)playable)
				return;

			switch (type)
			{
				case KeyType.Id.ACTION:
					playable.get ().send_action (KeyAction.actionbyid (action), down);
					break;
				case KeyType.Id.SKILL:
					if (down)
						combat.use_move (action);//todo judge is down?
					break;
				case KeyType.Id.ITEM:
					if (down)
						player.use_item (action);
					break;
				case KeyType.Id.FACE:
					if (down)
						player.set_expression (action);
					break;
			}
		}

		public Cursor.State send_cursor (bool pressed, Point_short position)
		{
			var statusbar = UI.get ().get_element<UIStatusBar> ();

			if (statusbar && statusbar.get ().is_menu_active ())
			{
				if (pressed)
					statusbar.get ().remove_menus ();

				if (statusbar.get ().is_in_range (position))
					return statusbar.get ().send_cursor (pressed, position);
			}

			var contextMenu = UI.get ().get_element<UIContextMenu> ();
			if (contextMenu && pressed)
			{
				contextMenu.get ().remove_menus ();
			}

			return npcs.send_cursor (pressed, new Point_short (position), camera.position ());
		}

		public bool is_player (int cid)
		{
			return cid == player.get_oid ();
		}
		public bool is_player (string cName)
		{
			return cName == player.get_name ();
		}
		public MapNpcs get_npcs ()
		{
			return npcs;
		}

		public MapChars get_chars ()
		{
			return chars;
		}

		public MapMobs get_mobs ()
		{
			return mobs;
		}

		public MapReactors get_reactors ()
		{
			return reactors;
		}

		public MapDrops get_drops ()
		{
			return drops;
		}

		public Player get_player ()
		{
			return player;
		}

		public Combat get_combat ()
		{
			return combat;
		}

		public Optional<Char> get_character (int cid)
		{
			if (is_player (cid))
				return player;
			else
				return (Char)chars.get_char (cid);
		}

		public Optional<Char> get_character (string cName)
		{
			if (is_player (cName))
				return player;
			else
				return (Char)chars.get_char (cName);
		}
		public int get_mapid ()
		{
			return mapid;
		}

		public void add_effect (string path)
		{
			effect = new MapEffect (path);
		}

		public long get_uptime ()
		{
			return ContinuousTimer.get ().stop (start);
		}

		public ushort get_uplevel ()
		{
			return levelBefore;
		}

		public long get_upexp ()
		{
			return expBefore;
		}

		public void transfer_player ()
		{
			new PlayerMapTransferPacket ().dispatch ();

			if (Configuration.get ().get_admin ())
				new AdminEnterMapPacket (AdminEnterMapPacket.Operation.ALERT_ADMINS).dispatch ();
		}

		public FootholdTree GetFootholdTree ()
		{
			return physics?.get_fht ();
		}

		private enum State
		{
			INACTIVE,
			TRANSITION,
			ACTIVE
		};

		public Camera camera = new Camera ();
		private Physics physics;
		private Player player;

		private Optional<Playable> playable;
		private State state;
		private int mapid;

		MapInfo mapinfo;
		MapTilesObjs tilesobjs;
		MapBackgrounds backgrounds;
		MapPortals portals;
		readonly MapReactors reactors = new MapReactors ();
		readonly MapNpcs npcs = new MapNpcs ();
		readonly MapChars chars = new MapChars ();
		readonly MapMobs mobs = new MapMobs ();
		readonly MapDrops drops = new MapDrops ();
		MapEffect effect;

		readonly Combat combat;

		DateTime start;
		ushort levelBefore;
		long expBefore;

		#region HaCreater
		public MapSimulator maplesimulator;
		public TexturePool texturePool = new TexturePool ();
		public MultiBoard multiBoard = new MultiBoard ();
		#endregion
	}
}