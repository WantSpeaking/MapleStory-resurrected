using System;
using System.Collections.Generic;
using MapleLib.WzLib;
using SD.Tools.Algorithmia.GeneralDataStructures;
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
	// The collection of platforms in a maple map
	// Used for collision-detection
	public class FootholdTree
	{
		public FootholdTree (WzObject src)
		{
			short leftw = 30000;
			short rightw = -30000;
			short botb = -30000;
			short topb = 30000;
			if (src is WzImageProperty node_100000000img_foothold)
			{
				foreach (var basef in node_100000000img_foothold.WzProperties)
				{
					byte layer;

					try
					{
						layer = (byte)Convert.ToInt32 (basef.Name);
					}
					catch (System.Exception ex)
					{
						//Console.Write(__func__);
						Console.Write (": ");
						Console.Write (ex.Message);
						Console.Write ("\n");
						continue;
					}

					foreach (var midf in basef.WzProperties)
					{
						foreach (var lastf in midf.WzProperties)
						{
							ushort id;

							try
							{
								id = (ushort)Convert.ToInt32 (lastf.Name);
							}
							catch (System.Exception ex)
							{
								//Console.Write(__func__);
								Console.Write (": ");
								Console.Write (ex.Message);
								Console.Write ("\n");
								continue;
							}

							Foothold foothold = new Foothold (lastf, id, layer);
							footholds[id] = foothold;
							//Foothold foothold = footholds.emplace(std::piecewise_construct, std::forward_as_tuple(id), std::forward_as_tuple(lastf, id, layer)).first.second;

							if (foothold.l () < leftw)
							{
								leftw = foothold.l ();
							}

							if (foothold.r () > rightw)
							{
								rightw = foothold.r ();
							}

							if (foothold.b () > botb)
							{
								botb = foothold.b ();
							}

							if (foothold.t () < topb)
							{
								topb = foothold.t ();
							}

							if (foothold.is_wall ())
							{
								continue;
							}

							short start = foothold.l ();
							short end = foothold.r ();

							for (short i = start; i <= end; i++)
							{
								footholdsbyx.Add (i, id);
							}
						}
					}
				}
			}


			walls = new Range<short> ((short)(leftw + 25), (short)(rightw - 25));
			borders = new Range<short> ((short)(topb - 300), (short)(botb + 100));
		}

		public FootholdTree ()
		{
		}

		// Takes an accelerated PhysicsObject and limits its movement based on the platforms in this tree
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void limit_movement(PhysicsObject& phobj) const
		public void limit_movement (PhysicsObject phobj)
		{
			if (phobj.hmobile ())
			{
				double crnt_x = phobj.crnt_x ();
				double next_x = phobj.next_x ();

				bool left = phobj.hspeed < 0.0f;
				double wall = get_wall (phobj.fhid, left, phobj.next_y ());
				bool collision = left ? crnt_x >= wall && next_x <= wall : crnt_x <= wall  && next_x >= wall;

				if (!collision && phobj.is_flag_set (PhysicsObject.Flag.TURNATEDGES))
				{
					wall = get_edge (phobj.fhid, left);
					collision = left ? crnt_x >= wall  && next_x <= wall : crnt_x <= wall && next_x >= wall;
				}

				if (collision)
				{
					phobj.limitx (wall);
					phobj.clear_flag (PhysicsObject.Flag.TURNATEDGES);
				}
			}

			if (phobj.vmobile ())
			{
				double crnt_y = phobj.crnt_y ();
				double next_y = phobj.next_y ();

				var ground = new Range<double> (get_fh (phobj.fhid).ground_below (phobj.crnt_x ()), get_fh (phobj.fhid).ground_below (phobj.next_x ()));

				bool collision = crnt_y <= ground.first () && next_y >= ground.second ();

				if (collision)
				{
					phobj.limity (ground.second ());

					limit_movement (phobj);
				}
				else
				{
					if (next_y < borders.first ())
					{
						phobj.limity (borders.first ());
					}
					else if (next_y > borders.second ())
					{
						phobj.limity (borders.second ());
					}
				}
			}
		}

		// Updates a PhysicsObject's fhid based on it's position
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void update_fh(PhysicsObject& phobj) const
		public void update_fh (PhysicsObject phobj)
		{
			if (phobj.type == PhysicsObject.Type.FIXATED && phobj.fhid > 0)
			{
				return;
			}

			Foothold curfh = get_fh (phobj.fhid);
			bool checkslope = false;

			double x = phobj.crnt_x ();
			double y = phobj.crnt_y ();

			if (phobj.onground)
			{
				if (Math.Floor (x) > curfh.r ())
				{
					phobj.fhid = curfh.next ();
				}
				else if (Math.Ceiling (x) < curfh.l ())
				{
					phobj.fhid = curfh.prev ();
				}

				if (phobj.fhid == 0)
				{
					phobj.fhid = get_fhid_below (x, y);
				}
				else
				{
					checkslope = true;
				}
			}
			else
			{
				phobj.fhid = get_fhid_below (x, y);

				if (phobj.fhid == 0)
				{
					return;
				}
			}

			Foothold nextfh = get_fh (phobj.fhid);
			phobj.fhslope = nextfh.slope ();

			double ground = nextfh.ground_below (x);

			if (phobj.vspeed == 0.0 && checkslope)
			{
				double vdelta = Math.Abs (phobj.fhslope);

				if (phobj.fhslope < 0.0)
				{
					vdelta *= (ground - y);
				}
				else if (phobj.fhslope > 0.0)
				{
					vdelta *= (y - ground);
				}

				if (curfh.slope () != 0.0 || nextfh.slope () != 0.0)
				{
					if (phobj.hspeed > 0.0 && vdelta <= phobj.hspeed)
					{
						phobj.y = ground;
					}
					else if (phobj.hspeed < 0.0 && vdelta >= phobj.hspeed)
					{
						phobj.y = ground;
					}
				}
			}

			phobj.onground = phobj.y == ground;

			if (phobj.enablejd || phobj.is_flag_set (PhysicsObject.Flag.CHECKBELOW))
			{
				ushort belowid = get_fhid_below (x, nextfh.ground_below (x) + 1.0);

				if (belowid > 0)
				{
					double nextground = get_fh (belowid).ground_below (x);
					phobj.enablejd = (nextground - ground) < 600.0;
					phobj.groundbelow = ground + 1.0;
				}
				else
				{
					phobj.enablejd = false;
				}

				phobj.clear_flag (PhysicsObject.Flag.CHECKBELOW);
			}

			if (phobj.fhlayer == 0 || phobj.onground)
			{
				phobj.fhlayer = (byte)nextfh.layer ();
			}

			if (phobj.fhid == 0)
			{
				phobj.fhid = curfh.id ();
				phobj.limitx (curfh.x1 ());
			}
		}

		// Determine the point on the ground below the specified position
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short get_y_below(Point<short> position) const
		public short get_y_below (Point<short> position)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
			ushort fhid = get_fhid_below (position.x (), position.y ());
			if (fhid != 0)
			{
				Foothold fh = get_fh (fhid);

				return (short)fh.ground_below (position.x ());
			}
			else
			{
				return borders.second ();
			}
		}

		// Returns the leftmost and rightmost platform positions of the map
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Range<short> get_walls() const
		public Range<short> get_walls ()
		{
			return walls;
		}

		// Returns the topmost and bottommost platform positions of the map
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Range<short> get_borders() const
		public Range<short> get_borders ()
		{
			return borders;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ushort get_fhid_below(double fx, double fy) const
		private ushort get_fhid_below (double fx, double fy)
		{
			ushort ret = 0;
			double comp = borders.second ();

			short x = (short)fx;
			footholdsbyx.TryGetValue (x, out var range);
			//var range = new Range<short> (x,y);	
			//var range = footholdsbyx.equal_range (x);

			foreach (var iter in range)
			{
				Foothold fh = footholds[(ushort)iter];
				double ycomp = fh.ground_below (fx);

				if (comp >= ycomp && ycomp >= fy)
				{
					comp = ycomp;
					ret = fh.id ();
				}
			}
			/*for (var iter = range.first(); iter != range.second(); ++iter)
			{
				Foothold fh = footholds[(ushort)iter];
				double ycomp = fh.ground_below (fx);

				if (comp >= ycomp && ycomp >= fy)
				{
					comp = ycomp;
					ret = fh.id ();
				}
			}*/

			return ret;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: double get_wall(ushort curid, bool left, double fy) const
		private double get_wall (ushort curid, bool left, double fy)
		{
			var shorty = (short)fy;
			Range<short> vertical = new Range<short> ((short)(shorty - 50), (short)(shorty - 1));
			Foothold cur = get_fh (curid);

			if (left)
			{
				Foothold prev = get_fh (cur.prev ());

				if (prev.is_blocking (vertical))
				{
					return cur.l ();
				}

				Foothold prev_prev = get_fh (prev.prev ());

				if (prev_prev.is_blocking (vertical))
				{
					return prev.l ();
				}

				return walls.first ();
			}
			else
			{
				Foothold next = get_fh (cur.next ());

				if (next.is_blocking (vertical))
				{
					return cur.r ();
				}

				Foothold next_next = get_fh (next.next ());

				if (next_next.is_blocking (vertical))
				{
					return next.r ();
				}

				return walls.second ();
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: double get_edge(ushort curid, bool left) const
		private double get_edge (ushort curid, bool left)
		{
			Foothold fh = get_fh (curid);

			if (left)
			{
				ushort previd = fh.prev ();

				if (previd == 0)
				{
					return fh.l ();
				}

				Foothold prev = get_fh (previd);
				ushort prev_previd = prev.prev ();

				if (prev_previd == 0)
				{
					return prev.l ();
				}

				return walls.first ();
			}
			else
			{
				ushort nextid = fh.next ();

				if (nextid == 0)
				{
					return fh.r ();
				}

				Foothold next = get_fh (nextid);
				ushort next_nextid = next.next ();

				if (next_nextid == 0)
				{
					return next.r ();
				}

				return walls.second ();
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const Foothold& get_fh(ushort fhid) const
		private Foothold get_fh (ushort fhid)
		{
			Foothold foothold;
			if (!footholds.TryGetValue (fhid, out foothold))
			{
				foothold = new Foothold ();
				Debug.LogWarning ($"can't find Foothold by fhid={fhid}");
			}
			
			return foothold;
		}

		private Dictionary<ushort, Foothold> footholds = new Dictionary<ushort, Foothold> ();
		private MultiValueDictionary<short, ushort> footholdsbyx = new MultiValueDictionary<short, ushort> ();//todo unordered_multimap

		private Foothold nullfh = new Foothold ();
		private Range<short> walls = new Range<short> ();
		private Range<short> borders = new Range<short> ();
	}
}