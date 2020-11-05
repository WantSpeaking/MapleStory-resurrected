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
	// A collection of remote controlled characters on a map
	public class MapChars
	{
		// Draw all characters on a layer
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Layer::Id layer, double viewx, double viewy, float alpha) const
		public void draw(Layer.Id layer, double viewx, double viewy, float alpha)
		{
			chars.draw(layer, viewx, viewy, alpha);
		}
		// Update all characters
		public void update(Physics physics)
		{
			for (; spawns.Count > 0; spawns.Dequeue())
			{
				CharSpawn spawn = spawns.Peek();

				int cid = spawn.get_cid();
				Optional<OtherChar> ochar = get_char(cid);

				if (ochar != null)
				{
					// TODO: Blank
				}
				else
				{
					chars.add(spawn.instantiate());
				}
			}

			chars.update(physics);
		}

		// Spawn a new character, if it has not been spawned yet.
		public void spawn(CharSpawn spawn)
		{
			spawns.Enqueue(spawn);
		}
		// Remove a character
		public void remove(int cid)
		{
			chars.remove(cid);
		}
		// Remove all characters
		public void clear()
		{
			chars.clear();
		}

		// Returns a reference to the MapObjects` object
		public MapObjects get_chars()
		{
			return chars;
		}

		// Update a character's movement
		public void send_movement(int cid, List<Movement> movements)
		{
			Optional<OtherChar> otherchar = get_char (cid);
			if (otherchar != null)
			{
				otherchar.get ().send_movement(movements);
			}
		}
		// Update a character's look
		public void update_look(int cid, LookEntry look)
		{
			Optional<OtherChar> otherchar = get_char (cid);
			if (otherchar != null)
			{
				otherchar.get ().update_look(look);
			}
		}

		public Optional<OtherChar> get_char(int cid)
		{
			return (OtherChar)chars.get(cid);
		}

		private MapObjects chars = new MapObjects();

		private Queue<CharSpawn> spawns = new Queue<CharSpawn>();
	}
}
