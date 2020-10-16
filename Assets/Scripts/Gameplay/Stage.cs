using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ms;

namespace ms
{
	class Stage : Singleton<Stage>
	{
		enum State
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
		int mapid;

		private MapInfo mapinfo;
		MapTilesObjs tilesobjs;
		MapBackgrounds backgrounds;

		//private  map

		public void init ()
		{
			//drops.init();
		}

		public void load (int mapid, sbyte portalid)
		{
			state = State.INACTIVE;
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

			//start = ContinuousTimer.get().start();

			//CharStats stats = player.get_stats();
			//levelBefore = stats.get_stat(MapleStat.Id.LEVEL);
			//expBefore = stats.get_exp();
		}

		public void load_map (int mapid) //mapid:100000000
		{
			this.mapid = mapid;

			//string strid = string_format.extend_id(mapid, 9);
			string strid = mapid.ToString (); //strid:100000000
			string prefix = Convert.ToString (mapid / 100000000); //prefix:1

			//nl.node src = mapid == -1 ? nl.nx.ui["CashShopPreview.img"] : nl.nx.map["Map"]["Map" + prefix][strid + ".img"];
			//var src = nl.nx.mapWz.ResolvePath("Map").ResolvePath($"Map{prefix}").ResolvePath($"{strid}.img"); //srcNodePath:Map/Map1/100000000.img

			var node_100000000img = nl.nx.wzFile_map["Map"]["Map" + prefix][strid + ".img"]; //srcNodePath:Map/Map1/100000000.img

			tilesobjs = new MapTilesObjs (node_100000000img);
			backgrounds = new MapBackgrounds (node_100000000img["back"]); //back2NodePath:Map/Map1/100000000.img/back
			physics = new Physics (node_100000000img["foothold"]);
			mapinfo = new MapInfo (node_100000000img, physics.get_fht ().get_walls (), physics.get_fht ().get_borders ());
		}

		void respawn (sbyte portalid)
		{
			//Music(mapinfo.get_bgm()).play();

			//Point<short> spawnpoint = portals.get_portal_by_id(portalid);
			//Point<short> startpos = physics.get_y_below(spawnpoint);
			Point<short> startpos = Point<short>.zero;
			//player.respawn(startpos, mapinfo.is_underwater());
			player.respawn (new Point<short> (), mapinfo?.is_underwater () ?? false);

			camera.set_position(startpos);
			camera.set_view(mapinfo?.get_walls(), mapinfo?.get_borders());
		}

		public void draw (float alpha)
		{
			Point<short> viewpos = camera.position(alpha);
			Point<double> viewrpos = camera.realposition(alpha);
			double viewx = viewrpos.x();
			double viewy = viewrpos.y();
			
			/*double viewx = 0;
			double viewy = 0;
			Point<short> viewpos = new Point<short> ();*/

			backgrounds.drawbackgrounds (viewx, viewy, alpha);

			foreach (var id in Enum.GetValues (typeof (Layer.Id)))
			{
				tilesobjs.draw ((Layer.Id)id, viewpos, alpha);
				player.draw ((Layer.Id)id, viewx, viewy, alpha);
				//chars.draw((global.ms.Layer.Id)id, viewx, viewy, alpha);

				/* reactors.draw(id, viewx, viewy, alpha);
				 npcs.draw(id, viewx, viewy, alpha);
				 mobs.draw(id, viewx, viewy, alpha);
				
				 drops.draw(id, viewx, viewy, alpha);*/
			}

			backgrounds.drawforegrounds (viewx, viewy, alpha);
		}

		public void update ()
		{
			if (player == null) return;
			player.update (physics);

			
			camera.update(player.get_position());

			if (!player.is_climbing () && !player.is_sitting () && !player.is_attacking ())
			{
				if (player.is_key_down (KeyAction.Id.UP) && !player.is_key_down (KeyAction.Id.DOWN))
					check_ladders (true);

				/*if (player.is_key_down (KeyAction.Id.UP))
					check_portals ();*/

				if (player.is_key_down (KeyAction.Id.DOWN))
					check_ladders (false);

				/*if (player.is_key_down (KeyAction.Id.SIT))
					check_seats ();

				if (player.is_key_down (KeyAction.Id.ATTACK))
				{
				}
				//combat.use_move(0);

				if (player.is_key_down (KeyAction.Id.PICKUP))
					check_drops ();*/
			}
		}

		void check_portals ()
		{
			/*if (player.is_attacking())
				return;

			Point<int16_t> playerpos = player.get_position();
			Portal.WarpInfo warpinfo = portals.find_warp_at(playerpos);

			if (warpinfo.intramap)
			{
				Point<int16_t> spawnpoint = portals.get_portal_by_name(warpinfo.toname);
				Point<int16_t> startpos = physics.get_y_below(spawnpoint);

				player.respawn(startpos, mapinfo.is_underwater());
			}
			else if (warpinfo.valid)
			{
				ChangeMapPacket(false, -1, warpinfo.name, false).dispatch();

				CharStats& stats = get().get_player().get_stats();

				stats.set_mapid(warpinfo.mapid);

				Sound(Sound.Name.PORTAL).play();
			}*/
		}

		void check_seats ()
		{
			/*if (player.is_sitting() || player.is_attacking())
				return;

			Optional<const Seat> seat = mapinfo.findseat(player.get_position());
			player.set_seat(seat);*/
		}

		void check_ladders (bool up)
		{
			if (!player.can_climb() || player.is_climbing() || player.is_attacking())
				return;

			Optional<Ladder> ladder = mapinfo.findladder(player.get_position(), up);
			player.set_ladder(ladder);
		}

		void check_drops ()
		{
			/*Point<int16_t> playerpos = player.get_position();
			MapDrops.Loot loot = drops.find_loot_at(playerpos);

			if (loot.first)
				PickupItemPacket(loot.first, loot.second).dispatch();*/
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
					//combat.use_move (action);
					break;
				case KeyType.Id.ITEM:
					player.use_item (action);
					break;
				case KeyType.Id.FACE:
					player.set_expression (action);
					break;
			}
		}
		
		public Player get_player()
		{
			return player;
		}
	}
}