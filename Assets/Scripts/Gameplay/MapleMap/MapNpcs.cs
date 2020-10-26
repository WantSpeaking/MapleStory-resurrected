using System.Collections.Generic;

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
	public class MapNpcs
	{
		// Draw all NPCs on a layer
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Layer::Id layer, double viewx, double viewy, float alpha) const
		public void draw (Layer.Id layer, double viewx, double viewy, float alpha)
		{
			npcs.draw (layer, viewx, viewy, alpha);
		}

		// Update all NPCs
		public void update (Physics physics)
		{
			for (; spawns.Count > 0; spawns.Dequeue ())
			{
				NpcSpawn spawn = spawns.Peek ();

				int oid = spawn.get_oid ();
				Optional<MapObject> npc = npcs.get (oid);

				if (npc != null)
				{
					npc.Dereference ().makeactive ();
				}
				else
				{
					npcs.add (spawn.instantiate (physics));
				}
			}

			npcs.update (physics);
		}

		// Add an NPC to the spawn queue
		public void spawn (NpcSpawn spawn)
		{
			spawns.Enqueue ((spawn));
		}

		// Remove the NPC with the specified oid
		public void remove (int oid)
		{
			var npc = (Npc)npcs.get (oid);
			if (npc != null)
			{
				npc.deactivate ();
			}
		}

		// Remove all NPCs
		public void clear ()
		{
			npcs.clear ();
		}

		// Returns a reference to the MapObject's object
		public MapObjects get_npcs ()
		{
			return npcs;
		}

		// Send mouse input to clickable NPCs
		public Cursor.State send_cursor (bool pressed, Point<short> position, Point<short> viewpos)
		{
			foreach (var map_object in npcs)
			{
				Npc npc = (Npc)map_object.Value;

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: if (npc &npc->is_active() &npc->inrange(position, viewpos))
				if (npc != null && npc.is_active () && npc.inrange (position, viewpos))
				{
					if (pressed)
					{
						// TODO: Try finding dialog first
						new TalkToNPCPacket (npc.get_oid ()).dispatch ();

						return Cursor.State.IDLE;
					}
					else
					{
						return Cursor.State.CANCLICK;
					}
				}
			}

			return Cursor.State.IDLE;
		}

		private MapObjects npcs = new MapObjects ();

		private Queue<NpcSpawn> spawns = new Queue<NpcSpawn> ();
	}
}