#define USE_NX

using System;
using System.Collections.Generic;
using Helper;
using MapleLib.WzLib;

//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////


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
			WzObject src = nl.nx.wzFile_item["Special"]["0900.img"];

			mesoicons[(int)MesoIcon.BRONZE] = src["09000000"]["iconRaw"];
			mesoicons[(int)MesoIcon.GOLD] = src["09000001"]["iconRaw"];
			mesoicons[(int)MesoIcon.BUNDLE] = src["09000002"]["iconRaw"];
			mesoicons[(int)MesoIcon.BAG] = src["09000003"]["iconRaw"];
		}

		// Draw all drops on a layer
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Layer::Id layer, double viewx, double viewy, float alpha) const
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
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
				if (drop != null)
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
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
			Optional<Drop> drop = (Drop)drops.get (oid);
			if (drop != null)
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
		public System.Tuple<int, Point<short>> find_loot_at (Point<short> playerpos)
		{
			if (!lootenabled)
			{
				return new Tuple<int, Point<short>> (0, Point<short>.zero);
			}

			foreach (var mmo in drops)
			{
				Optional<Drop> drop = (Drop)mmo.Value;

				if (drop != null && drop.Dereference ().bounds ().contains (playerpos))
				{
					lootenabled = false;

					int oid = mmo.Key;
					Point<short> position = drop.Dereference ().get_position ();

					return new Tuple<int, Point<short>> (oid, position);
				}
			}

			return new Tuple<int, Point<short>> (0, Point<short>.zero);
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