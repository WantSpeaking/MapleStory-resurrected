using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
	// A collection of mobs on a map.
	public class MapMobs
	{
		// Draw all mobs on a layer.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Layer::Id layer, double viewx, double viewy, float alpha) const
		public void draw (Layer.Id layer, double viewx, double viewy, float alpha)
		{
			mobs.draw (layer, viewx, viewy, alpha);
		}

		// Update all mobs.
		public void update (Physics physics)
		{
			for (; spawns.Count > 0; spawns.Dequeue ())
			{
				MobSpawn spawn = spawns.Peek ();

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
				var mobRef = mobs.get (spawn.get_oid ());
				if (mobRef.get () is Mob mob)
				{
					sbyte mode = spawn.get_mode ();

					if (mode > 0)
					{
						mob.set_control (mode);
					}

					mob.makeactive ();
				}
				else
				{
					mobs.add (spawn.instantiate ());
				}
			}

			mobs.update (physics);
		}

		// Spawn a new mob.
		public void spawn (MobSpawn spawn)
		{
			//Debug.Log ($"MapMobs spawn : oid:{spawn.get_oid ()}\t mode:{spawn.get_mode ()}");
			spawns.Enqueue (spawn);
		}

		// Kill a mob.
		public void remove (int oid, sbyte animation)
		{
			if (mobs.get (oid).get () is Mob mob)
			{
				mob.kill (animation);
			}
		}

		// Remove all mobs.
		public void clear ()
		{
			mobs.clear ();
		}

		// Update who a mob is controlled by.
		public void set_control (int oid, bool control)
		{
			sbyte mode = (sbyte)(control ? 1 : 0);

			if (mobs.get (oid).get () is Mob mob)
			{
				mob.set_control (mode);
			}
		}

		// Update a mob's hp display.
		public void send_mobhp (int oid, sbyte percent, ushort playerlevel)
		{
			if (mobs.get (oid).get () is Mob mob)
			{
				mob.show_hp (percent, playerlevel);
			}
		}

		// Update a mob's movements.
		public void send_movement (int oid, Point<short> start, List<Movement> movements)
		{
			if (mobs.get (oid).get () is Mob mob)
			{
				mob.send_movement (start, movements);
			}
		}

		// Calculate the results of an attack.
		public void send_attack (AttackResult result, Attack attack, List<int> targets, byte mobcount)
		{
			foreach (var target in targets)
			{
				if (mobs.get (target).get () is Mob mob)
				{
					result.damagelines[target] = mob.calculate_damage (attack);
					result.mobcount++;

					if (result.mobcount == 1)
					{
						result.first_oid = target;
					}

					if (result.mobcount == mobcount)
					{
						result.last_oid = target;
					}
				}
			}
		}

		// Applies damage to a mob.
		public void apply_damage (int oid, int damage, bool toleft, AttackUser user, SpecialMove move)
		{
			if (mobs.get (oid).get () is Mob mob)
			{
				mob.apply_damage (damage, toleft);

				// TODO: Maybe move this into the method above too?
				move.apply_hiteffects (user, mob);
			}
		}

		// Check if the mob with the specified oid exists.
		public bool contains (int oid)
		{
			return mobs.contains (oid);
		}

		Range<short> horizontal = new Range<short> ();
		Range<short> vertical = new Range<short> ();

		Rectangle<short> player_rect = new Rectangle<short> ();

		// Return the id of the first mob who collides with the object.
		public int find_colliding (MovingObject moveobj)
		{
			horizontal.Set (moveobj.get_last_x (), moveobj.get_x ());
			vertical.Set (moveobj.get_last_y (), moveobj.get_y ());
			player_rect.Set (horizontal.smaller (), horizontal.greater (), (short)(vertical.smaller () - 50), vertical.greater ());
			
			return mobs.Objects.FirstOrDefault (pair => pair.Value != null && ((pair.Value as Mob)?.is_alive () ?? false) && ((pair.Value as Mob)?.is_in_range (player_rect) ?? false)).Key;
		}

		// Create an attack by the specified mob.
		public MobAttack create_attack (int oid)
		{
			if (mobs.get (oid).get () is Mob mob)
			{
				return mob.create_touch_attack ();
			}

			return null;
		}

		// Return the position of a mob.
		public Point<short> get_mob_position (int oid)
		{
			if (mobs.get (oid).get () is Mob mob)
			{
				return mob.get_position ();
			}
			else
			{
				return new Point<short> (0, 0);
			}
		}

		// Return the head position of a mob.
		public Point<short> get_mob_head_position (int oid)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
			if (mobs.get (oid).get () is Mob mob)
			{
				return mob.get_head_position ();
			}
			else
			{
				return new Point<short> (0, 0);
			}
		}

		// Return all mob map objects
		public MapObjects get_mobs ()
		{
			return mobs;
		}

		private MapObjects mobs = new MapObjects ();

		private Queue<MobSpawn> spawns = new Queue<MobSpawn> ();
	}
}