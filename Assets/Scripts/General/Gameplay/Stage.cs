using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaCreator;
using HaCreator.Wz;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using ms;

namespace ms
{
	internal class Stage : Singleton<Stage>
	{
		private enum State
		{
			INACTIVE,
			TRANSITION,
			ACTIVE
		}

		public Camera camera = new Camera ();

		private Physics physics;

		private Player player;

		private Optional<Playable> playable;

		private State state;

		private int mapid;

		private MapInfo mapinfo;

		private MapTilesObjs tilesobjs;

		private MapBackgrounds backgrounds;

		private MapPortals portals;

		private readonly MapReactors reactors = new MapReactors ();

		private readonly MapNpcs npcs = new MapNpcs ();

		private readonly MapChars chars = new MapChars ();

		private readonly MapMobs mobs = new MapMobs ();

		private readonly MapDrops drops = new MapDrops ();

		private MapEffect effect;

		private readonly Combat combat;

		private DateTime start;

		private ushort levelBefore;

		private long expBefore;


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
			start = Singleton<ContinuousTimer>.get ().start ();
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
			TestURPBatcher.Instance.Clear ();
			ms_Unity.FGUI_Manager.Instance.CloseAll ();
		}

		private void load_map (int mapid)
		{
			//MapleStory.Instance.canStart = false;

			//UnityEngine.SceneManagement.SceneManager.LoadScene ("Game");

			//TestURPBatcher.Instance.Clear ();

			//TextManager.Instance.Clear ();

			this.mapid = mapid;
			string strid = string_format.extend_id (mapid, 9);
			string prefix = Convert.ToString (mapid / 100000000);
			WzObject node_100000000img = ((mapid == -1) ? wz.wzFile_ui["CashShopPreview.img"] : wz.wzFile_map["Map"]["Map" + prefix][strid + ".img"]);
			tilesobjs = new MapTilesObjs (node_100000000img);
			backgrounds = new MapBackgrounds (node_100000000img["back"]);
			physics = new Physics (node_100000000img["foothold"]);
			mapinfo = new MapInfo (node_100000000img, physics.get_fht ().get_walls (), physics.get_fht ().get_borders ());
			portals = new MapPortals (node_100000000img["portal"], mapid);
		}

		public byte just_Entered_portalid;
		public string just_Entered_portalName;
		private void respawn (sbyte portalid)
		{
			just_Entered_portalid = (byte)portalid;
			portals.get_portal_by_id (just_Entered_portalid)?.get_name ();
			Singleton<Music>.get ().play (mapinfo.get_bgm ());
			Point_short spawnpoint = portals.get_portalPos_by_id ((byte)portalid);
			Point_short startpos = physics.get_y_below (new Point_short (spawnpoint));
			player.respawn (new Point_short (startpos), mapinfo?.is_underwater () ?? false);
			camera.set_position (new Point_short (startpos));
			camera.set_view (mapinfo?.get_walls (), mapinfo?.get_borders ());
		}

		public double viewx;
		public double viewy;
		public float alpha;
		public void draw (float alpha)
		{
			if (state != State.ACTIVE)
			{
				return;
			}
			Point_short viewpos = camera.position (alpha);
			Point_double viewrpos = camera.realposition (alpha);
			viewx = viewrpos.x ();
			viewy = viewrpos.y ();
			this.alpha = alpha;
			MapleStory.Instance.viewx = viewx;
			MapleStory.Instance.viewy = viewy;
			MapleStory.Instance.alpha = alpha;

			backgrounds?.drawbackgrounds (viewx, viewy, alpha);
			foreach (ms.Layer.Id id in Enum.GetValues (typeof (ms.Layer.Id)))
			{
				tilesobjs?.draw (id, new Point_short (viewpos), alpha);
				reactors?.draw (id, viewx, viewy, alpha);
				npcs?.draw (id, viewx, viewy, alpha);
				mobs?.draw (id, viewx, viewy, alpha);
				chars?.draw (id, viewx, viewy, alpha);
				player?.draw (id, viewx, viewy, alpha);
				drops?.draw (id, viewx, viewy, alpha);
			}
			combat?.draw (viewx, viewy, alpha);
			portals?.draw (new Point_short (viewpos), alpha);
			backgrounds?.drawforegrounds (viewx, viewy, alpha);
			effect?.draw ();
		}

		public void update ()
		{
			if (state != State.ACTIVE)
			{
				return;
			}
			combat.update ();
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
			//AppDebug.Log ($"player pos: {player.get_position ()} camera.position:{camera.position (1f)}");
			if (!player.is_climbing () && !player.is_sitting () && !player.is_attacking ())
			{
				if (player.is_key_down (KeyAction.Id.UP) && !player.is_key_down (KeyAction.Id.DOWN))
				{
					check_ladders (up: true);
				}
				if (player.is_key_down (KeyAction.Id.UP))
				{
					check_portals ();
				}
				if (player.is_key_down (KeyAction.Id.DOWN))
				{
					check_ladders (up: false);
				}
				if (player.is_key_down (KeyAction.Id.ATTACK))
				{
					combat.use_move (0, true);
				}
				if (player.is_key_down (KeyAction.Id.MUTE))
				{
					combat.use_move (1, true);
				}
				if (player.is_key_down (KeyAction.Id.SIT))
				{
					check_seats ();
				}
				if (player.is_key_down (KeyAction.Id.PICKUP))
				{
					check_drops ();
				}
			}
			if (!player.is_invincible ())
				return;

			if (player.is_died ())
				return;

			var oid_id = mobs.find_colliding (player.get_phobj ());
			if (oid_id != 0)
			{
				MobAttack attack = mobs.create_attack (oid_id);
				if (attack)
				{
					MobAttackResult result = player.damage (attack);
					new TakeDamagePacket (result, TakeDamagePacket.From.TOUCH).dispatch ();
				}
			}

		}

		public void show_character_effect (int cid, CharEffect.Id effect)
		{
			Optional<ms.Char> character = get_character (cid);
			if ((bool)character)
			{
				character.get ().show_effect_id (effect);
			}
		}

		private void check_portals ()
		{
			if (!player.is_attacking ())
			{
				Point_short playerpos = player.get_position ();
				Portal.WarpInfo warpinfo = portals.find_warp_at (playerpos);
				if (warpinfo.intramap)
				{
					Point_short spawnpoint = portals.get_portal_by_name (warpinfo.toname);
					Point_short startpos = physics.get_y_below (spawnpoint);
					player.respawn (new Point_short (startpos), mapinfo.is_underwater ());
				}
				else if (warpinfo.valid)
				{
					new ChangeMapPacket (died: false, -1, warpinfo.name, usewheel: false).dispatch ();
					CharStats stats = Singleton<Stage>.get ().get_player ().get_stats ();
					stats.set_mapid (warpinfo.mapid);
					new Sound (Sound.Name.PORTAL).play ();
				}
			}
		}

		private void check_seats ()
		{
			if (!player.is_sitting () && !player.is_attacking ())
			{
				Optional<Seat> seat = mapinfo.findseat (player.get_position ());
				player.set_seat (new Optional<Seat> (seat?.get ()));
			}
		}

		private void check_ladders (bool up)
		{
			if (player.can_climb () && !player.is_climbing () && !player.is_attacking ())
			{
				Optional<Ladder> ladder = mapinfo.findladder (player.get_position (), up);
				player.set_ladder (ladder);
			}
		}

		private void check_drops ()
		{
			Point_short playerpos = player.get_position ();
			Tuple<int, Point_short> loot = drops.find_loot_at (new Point_short (playerpos));
			if (loot.Item1 != 0)
			{
				new PickupItemPacket (loot.Item1, loot.Item2).dispatch ();
			}
		}

		public void send_key (KeyType.Id type, int action, bool down, bool pressing = false)
		{
			if (state != State.ACTIVE || !playable)
			{
				return;
			}
			switch (type)
			{
				case KeyType.Id.ACTION:
					playable.get ().send_action (KeyAction.actionbyid (action), down);
					break;
				case KeyType.Id.SKILL:
					//if (down)
					{
						combat.use_move (action, down, pressing);
					}
					break;
				case KeyType.Id.ITEM:
					if (down)
					{
						player.use_item (action);
					}
					break;
				case KeyType.Id.FACE:
					if (down)
					{
						player.set_expression (action);
					}
					break;
				case KeyType.Id.CASH:
				case KeyType.Id.MENU:
					break;
			}
		}

		public void send_keyDown (int keycode)
		{
			Keyboard.Mapping mapping = UI.get ().get_keyboard ().get_maple_mapping (keycode);
			send_key (mapping.type, mapping.action, true);
		}
		public void send_keyUp (int keycode)
		{
			Keyboard.Mapping mapping = UI.get ().get_keyboard ().get_maple_mapping (keycode);
			send_key (mapping.type, mapping.action, false);
		}
		public Cursor.State send_cursor (bool pressed, Point_short position)
		{
			if (ms_Unity.FGUI_Manager.Get().PanelOpening) return Cursor.State.GAME;

			Optional<UIStatusBar> statusbar = Singleton<UI>.get ().get_element<UIStatusBar> ();
			if ((bool)statusbar && statusbar.get ().is_menu_active ())
			{
				if (pressed)
				{
					statusbar.get ().remove_menus ();
				}
				if (statusbar.get ().is_in_range (position))
				{
					return statusbar.get ().send_cursor (pressed, position);
				}
			}
			Optional<UIContextMenu> contextMenu = Singleton<UI>.get ().get_element<UIContextMenu> ();
			if ((bool)contextMenu && pressed)
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

		public Optional<ms.Char> get_character (int cid)
		{
			if (is_player (cid))
			{
				return player;
			}
			return (OtherChar)chars.get_char (cid);
		}

		public Optional<ms.Char> get_character (string cName)
		{
			if (is_player (cName))
			{
				return player;
			}
			return (OtherChar)chars.get_char (cName);
		}

		public int get_mapid ()
		{
			return mapid;
		}

		public MapPortals get_portals()
		{
			return portals;
		}
		public void add_effect (string path)
		{
			effect = new MapEffect (path);
		}

		public long get_uptime ()
		{
			return Singleton<ContinuousTimer>.get ().stop (start);
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
			new ChangeMapSpecialPacket ().dispatch ();
			
			if (Singleton<Configuration>.get ().get_admin ())
			{
				new AdminEnterMapPacket (AdminEnterMapPacket.Operation.ALERT_ADMINS).dispatch ();
			}
		}

		public FootholdTree GetFootholdTree ()
		{
			return physics?.get_fht ();
		}

		public void UpdateQuest ()
		{
			ms.Stage.get ().get_npcs ().UpdateQuest ();
			ms_Unity.FGUI_Manager.Instance.GetFGUI<ms_Unity.FGUI_QuestLog> ().UpdateQuest ();

		}
	}
}