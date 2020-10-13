using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ms;

namespace Assets.ms
{
	class Stage : Singleton<Stage>
	{
		enum State
		{
			INACTIVE,
			TRANSITION,
			ACTIVE
		};
		
		int mapid;
		MapBackgrounds backgrounds;
		MapTilesObjs tilesobjs;
		private Physics physics;
		private MapInfo mapinfo;
		private Player player;
		private Playable playable;
		private State state;
		//private  map
		
		public void init()
		{
			//drops.init();
		}
		
		public void load (int mapid, sbyte portalid)
		{
			state = State.INACTIVE;
			switch (state)
			{
				case State.INACTIVE:
					//load_map (mapid);
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
			playable = player;

			//start = ContinuousTimer::get().start();

			//CharStats stats = player.get_stats();
			//levelBefore = stats.get_stat(MapleStat::Id::LEVEL);
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

			//player.respawn(startpos, mapinfo.is_underwater());
			player.respawn (new Point<short> (), mapinfo?.is_underwater ()??false);

			//camera.set_position(startpos);
			//camera.set_view(mapinfo.get_walls(), mapinfo.get_borders());
		}

		public void draw (float alpha)
		{
			double viewx = 0;
			double viewy = 0;
			Point<short> viewpos = new Point<short> ();

			//backgrounds.drawbackgrounds (1, 1, 1);

			foreach (var id in Enum.GetValues (typeof (global::ms.Layer.Id)))
			{
				//tilesobjs.draw ((global::ms.Layer.Id)id, viewpos, alpha);
				player.draw((global::ms.Layer.Id)id, viewx, viewy, alpha);
				//chars.draw((global::ms.Layer.Id)id, viewx, viewy, alpha);

				/* reactors.draw(id, viewx, viewy, alpha);
				 npcs.draw(id, viewx, viewy, alpha);
				 mobs.draw(id, viewx, viewy, alpha);
				
				 drops.draw(id, viewx, viewy, alpha);*/
			}

			//backgrounds.drawforegrounds (1, 1, 1);
		}
	}
}