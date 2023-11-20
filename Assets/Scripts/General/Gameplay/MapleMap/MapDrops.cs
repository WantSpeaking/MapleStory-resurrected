#define USE_NX

using System;
using System.Collections.Generic;
using Helper;
using MapleLib.WzLib;




namespace ms
{
	public class MapDrops
	{
		public MapDrops ()
		{
			lootenabled = false;
		}

		// Initialize the meso icons
		public void init ()
		{
			var src = ms.wz.wzProvider_item["Special/0900.img"];

			mesoicons[(int)MesoIcon.BRONZE] = src["09000000"]["iconRaw"];
			mesoicons[(int)MesoIcon.GOLD] = src["09000001"]["iconRaw"];
			mesoicons[(int)MesoIcon.BUNDLE] = src["09000002"]["iconRaw"];
			mesoicons[(int)MesoIcon.BAG] = src["09000003"]["iconRaw"];
		}

		// Draw all drops on a layer
		public void draw (Layer.Id layer, double viewx, double viewy, float alpha)
		{
			drops.draw (layer, viewx, viewy, alpha);
		}

		// Update all drops
		public void update (Physics physics)
		{
			for (; spawns.Count > 0; spawns.Dequeue ())
			{
				DropSpawn spawn = spawns.Peek ();

				int oid = spawn.get_oid ();

				Optional<MapObject> drop = drops.get (oid);
				if (drop)
				{
					drop.get ().makeactive ();
				}
				else
				{
					int itemid = spawn.get_itemid ();
					bool meso = spawn.is_meso ();
					ItemData itemdata = ItemData.get (itemid);
					if (meso)
					{
						MesoIcon mesotype = (itemid > 999) ? MesoIcon.BAG : (itemid > 99) ? MesoIcon.BUNDLE : (itemid > 49) ? MesoIcon.GOLD : MesoIcon.BRONZE;

						Animation icon = new Animation(mesoicons[(int)mesotype]);
						drops.add (spawn.instantiate (icon));
					}
					else if (itemdata != null)
					{
						Texture icon = new Texture(itemdata.get_icon (true));
						drops.add (spawn.instantiate (icon));
					}
				}
			}

			foreach (var mesoicon in mesoicons)
			{
				mesoicon.update ();
			}

			drops.update (physics);

			lootenabled = true;
		}

		// Spawn a new drop
		public void spawn (DropSpawn spawn)
		{
			spawns.Enqueue (spawn);
		}

		// Remove a drop
		public void remove (int oid, sbyte mode, PhysicsObject looter)
		{
			Optional<Drop> drop = (Drop)drops.get (oid);
			if (drop)
			{
				drop.get ().expire (mode, looter);
			}
		}

		// Remove all drops
		public void clear ()
		{
			drops.clear ();
		}

		// Find a drop which can be picked up at the specified position
		public System.Tuple<int, Point_short> find_loot_at (Point_short playerpos)
		{
			if (!lootenabled)
			{
				return new Tuple<int, Point_short> (0, Point_short.zero);
			}

			foreach (var mmo in drops)
			{
				Optional<Drop> drop = (Drop)mmo.Value;

				if (drop && drop.get ().bounds ().contains (playerpos))
				{
					lootenabled = false;

					int oid = mmo.Key;
					Point_short position = drop.get ().get_position ();

					return new Tuple<int, Point_short> (oid, position);
				}
			}

			return new Tuple<int, Point_short> (0, Point_short.zero);
		}
		List <int> ids = new List<int> ();
		/// <summary>
		/// 
		/// </summary>
		/// <param name="searchRange"></param>
		/// <param name="itemType">0 item;1 meso;2 all</param>
		/// <returns></returns>
        public List<int> find_loot_inRange(Rectangle_short searchRange, byte itemType)
        {
            ids.Clear ();
            
            foreach (var mmo in drops)
            {
                Optional<Drop> drop = (Drop)mmo.Value;

                if (drop && drop.get().bounds().overlaps(searchRange))
                {
                    lootenabled = false;

                    int oid = mmo.Key;
					if(itemType == 0)
					{
						if(drop.get() is ItemDrop i)
						{
                            ids.Add(oid);
                        }
					}
                    else if(itemType==1)
					{
                        if (drop.get() is MesoDrop i)
                        {
                            ids.Add(oid);
                        }
                    }
					else
					{
                        ids.Add(oid);
                    }
                }
            }

            return ids;
        }

        private MapObjects drops = new MapObjects ();

		private enum MesoIcon
		{
			BRONZE,
			GOLD,
			BUNDLE,
			BAG,
		}

		private Animation[] mesoicons = new Animation[EnumUtil.GetEnumLength<MesoIcon> ()];
		private bool lootenabled;

		private Queue<DropSpawn> spawns = new Queue<DropSpawn> ();
	}
}


#if USE_NX
#endif