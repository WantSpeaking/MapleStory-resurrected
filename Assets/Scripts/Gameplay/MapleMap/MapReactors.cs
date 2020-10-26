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
	// Collection of reactors on a map
	public class MapReactors
	{
		// Draw all reactors on a layer
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Layer::Id layer, double viewx, double viewy, float alpha) const
		public void draw (Layer.Id layer, double viewx, double viewy, float alpha)
		{
			reactors.draw (layer, viewx, viewy, alpha);
		}
		// Update all reactors

		// Spawns all reactors to map with proper footholds
		public void update (Physics physics)
		{
			for (; spawns.Count > 0; spawns.Dequeue ())
			{
				ReactorSpawn spawn = spawns.Peek ();

				int oid = spawn.get_oid ();

				var reactor = reactors.get (oid);
				if (reactor != null)
				{
					reactor.get ().makeactive ();
				}
				else
				{
					reactors.add (spawn.instantiate (physics));
				}
			}

			reactors.update (physics);
		}

		// Trigger a reactor
		public void trigger (int oid, sbyte state)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
			Optional<Reactor> reactor = (Reactor)reactors.get (oid);
			if (reactor != null)
			{
				reactor.get ().set_state (state);
			}
		}

		// Spawn a new reactor
		public void spawn (ReactorSpawn spawn)
		{
			spawns.Enqueue (spawn);
		}

		// Remove a reactor
		public void remove (int oid, sbyte state, Point<short> position)
		{
			Optional<Reactor> reactor = (Reactor)reactors.get (oid);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
			if (reactor!=null)
			{
				reactor.get ().destroy (state, position);
			}
		}

		// Remove all reactors
		public void clear ()
		{
			reactors.clear ();
		}

		public MapObjects get_reactors ()
		{
			return reactors;
		}

		private MapObjects reactors = new MapObjects ();

		private Queue<ReactorSpawn> spawns = new Queue<ReactorSpawn> ();
	}
}