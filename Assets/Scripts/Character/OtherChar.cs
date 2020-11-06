using System.Collections.Generic;
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
	// Other client's players.
	public class OtherChar : Char
	{
		public OtherChar (int id, CharLook lk, byte lvl, short jb, string nm, sbyte st, Point<short> pos) : base (id, lk, nm)
		{
			//this.Char = new < type missing > (id, lk, nm);
			level = lvl;
			job = jb;
			set_position (pos);

			lastmove.xpos = pos.x ();
			lastmove.ypos = pos.y ();
			lastmove.newstate = (byte)st;
			timer = 0;

			attackspeed = 6;
			attacking = false;
		}

		// Update the character.
		public override sbyte update (Physics physics)
		{
			//Debug.Log ($"OtherChar update lastmove.xpos: {lastmove.xpos}\t lastmove.ypos: {lastmove.ypos}\t phobj.crnt_x ():{phobj.crnt_x ()}\t phobj.crnt_y ():{phobj.crnt_y ()}");

			if (timer > 1)
			{
				timer--;
			}
			else if (timer == 1)
			{
				if (movements.Count > 0)
				{
					lastmove = movements.Peek ();
					movements.Dequeue ();
				}
				else
				{
					timer = 0;
				}
			}

			if (!attacking)
			{
				byte laststate = lastmove.newstate;
				set_state (laststate);
			}

			phobj.hspeed = lastmove.xpos - phobj.crnt_x ();
			phobj.vspeed = lastmove.ypos - phobj.crnt_y ();
			phobj.move ();
			
			physics.get_fht ().update_fh (phobj);

			bool aniend = base.update (physics, get_stancespeed ());

			if (aniend && attacking)
			{
				attacking = false;
			}

			return get_layer ();
		}

		// Add the movements which this character will go through next.
		public void send_movement (List<Movement> newmoves)
		{
			movements.Enqueue (newmoves[newmoves.Count - 1]);

			if (timer == 0)
			{
				const ushort DELAY = 50;
				timer = DELAY;
			}
		}

		// Update a skill level.
		public void update_skill (int skillid, byte skilllevel)
		{
			skilllevels[skillid] = skilllevel;
		}

		// Update the attack speed.
		public void update_speed (byte @as)
		{
			attackspeed = @as;
		}

		// Update the character look.
		public void update_look (LookEntry newlook)
		{
			look =new CharLook(newlook);

			byte laststate = lastmove.newstate;
			set_state (laststate);
		}

		// Return the character's attacking speed.
		public override sbyte get_integer_attackspeed ()
		{
			return (sbyte)attackspeed;
		}

		// Return the character's level.
		public override ushort get_level ()
		{
			return level;
		}

		// Return the character's level of a skill.
		public override int get_skilllevel (int skillid)
		{
			if (!skilllevels.ContainsKey (skillid))
			{
				return 0;
			}
			

			return skilllevels[skillid];
		}

		private ushort level;
		private short job;
		private Queue<Movement> movements = new Queue<Movement> ();
		private Movement lastmove = new Movement ();
		private ushort timer;

		private Dictionary<int, byte> skilllevels = new Dictionary<int, byte> ();
		private byte attackspeed;
	}
}