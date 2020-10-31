using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Helper;

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
	// A collection of generic MapObjects
	public class MapObjects : IEnumerable<KeyValuePair<int, MapObject>>
	{
		// Draw all MapObjects that are on the specified layer
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Layer::Id layer, double viewx, double viewy, float alpha) const
		public MapObject this [int index]
		{
			get
			{
				Objects.TryGetValue (index, out var result);
				return result;
			}
		}

		public MapObjects ()
		{
			Objects = new ReadOnlyDictionary<int, MapObject> (objects);
			initArray ();
		}

		public void draw (Layer.Id layer, double viewx, double viewy, float alpha)
		{
			foreach (var oid in layers[(int)layer])
			{
				var mmo = get (oid);

				if (mmo != null && mmo.Dereference ().is_active ())
				{
					mmo.Dereference ().draw (viewx, viewy, alpha);
				}
			}
		}

		HashSet<int> cache = new HashSet<int> ();

		// Update all MapObjects of this type
		// Also updates layers (E.g. drawing order)
		public void update (Physics physics)
		{
			cache.Clear ();
			foreach (var iter in objects)
			{
				bool remove_mob = false;
				var mmo = iter.Value;
				if (mmo != null)
				{
					sbyte oldlayer = mmo.get_layer ();
					sbyte newlayer = mmo.update (physics);

					if (newlayer == -1)
					{
						remove_mob = true;
					}
					else if (newlayer != oldlayer)
					{
						int oid = iter.Key;
						layers[oldlayer].RemoveWhere (l => l == oid);
						layers[newlayer].Add (oid);
					}
				}
				else
				{
					remove_mob = true;
				}

				if (remove_mob)
				{
					cache.Add (iter.Key);
					//iter = objects.Remove (iter);
				}

				/*else
				{
					iter++;
				}*/
			}

			foreach (var id in cache)
			{
				objects.Remove (id);
			}
		}

		// Adds a MapObjects of this type
		public void add (MapObject toadd)
		{
			int oid = toadd.get_oid ();
			sbyte layer = toadd.get_layer ();
			objects[oid] = toadd;
			layers[layer].Add (oid);
		}

		// Removes the MapObjects with the given oid
		public void remove (int oid)
		{
			if (objects.TryGetValue (oid, out var mapObject))
			{
				sbyte layer = mapObject.get_layer ();
				objects.Remove (oid);
				layers[layer].RemoveWhere (l => l == oid);
			}
		}

		// Removes all MapObjects of this type
		public void clear ()
		{
			objects.Clear ();

			foreach (var layer in layers)
			{
				layer.Clear ();
			}
		}

		// Check if a map object with the specified id exists on the map
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool contains(int oid) const
		public bool contains (int oid)
		{
			return objects.Any (pair => pair.Key == oid);
		}

		// Obtains a pointer to the MapObject with the given oid
		public Optional<MapObject> get (int oid)
		{
			objects.TryGetValue (oid, out var mapObject);
			return mapObject;
		}

		/*// Return a begin iterator
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The typedef 'underlying_t' was defined in multiple preprocessor conditionals and cannot be replaced in-line:
		public MapObjects.underlying_t.iterator begin()
		{
			return objects.GetEnumerator();
		}
		// Return an end iterator
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The typedef 'underlying_t' was defined in multiple preprocessor conditionals and cannot be replaced in-line:
		public MapObjects.underlying_t.iterator end()
		{
			return objects.end();
		}
		// Return a begin iterator
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The typedef 'underlying_t' was defined in multiple preprocessor conditionals and cannot be replaced in-line:
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: MapObjects::underlying_t::const_iterator begin() const
		public MapObjects.underlying_t.const_iterator begin()
		{
			return objects.GetEnumerator();
		}
		// Return an end iterator
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The typedef 'underlying_t' was defined in multiple preprocessor conditionals and cannot be replaced in-line:
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: MapObjects::underlying_t::const_iterator end() const
		public MapObjects.underlying_t.const_iterator end()
		{
			return objects.end();
		}*/
		// Return the size of the iterator
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The typedef 'underlying_t' was defined in multiple preprocessor conditionals and cannot be replaced in-line:
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: MapObjects::underlying_t::size_type size() const
		public int size ()
		{
			return objects.Count;
		}

		public ReadOnlyDictionary<int, MapObject> Objects;
		private Dictionary<int, MapObject> objects = new Dictionary<int, MapObject> ();
		private HashSet<int>[] layers = new HashSet<int>[EnumUtil.GetEnumLength<Layer.Id> ()];

		private void initArray ()
		{
			for (int i = 0; i < EnumUtil.GetEnumLength<Layer.Id> (); i++)
			{
				layers[i] = new HashSet<int> ();
			}
		}

		public IEnumerator<KeyValuePair<int, MapObject>> GetEnumerator ()
		{
			return Objects.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return Objects.GetEnumerator ();
		}
	}
}