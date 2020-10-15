using System.Collections.Generic;
using ms.Helper;
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
	public class Seat
	{
		public Seat (WzImageProperty node_100000000img_seat_0)
		{
			var tempPoint = node_100000000img_seat_0.GetPoint ();
			pos = new Point<short> ((short)tempPoint.X, (short)tempPoint.Y);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool inrange(Point<short> position) const
		public bool inrange (Point<short> position)
		{
			var hor = Range<short>.symmetric (position.x (), 10);
			var ver = Range<short>.symmetric (position.y (), 10);

			return hor.contains (pos.x ()) && ver.contains (pos.y ());
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> getpos() const
		public Point<short> getpos ()
		{
			return pos;
		}

		private Point<short> pos = new Point<short> ();
	}

	public class Ladder
	{
		public Ladder (WzImageProperty node_100000000img_ladderRope_1)
		{
			x = node_100000000img_ladderRope_1["x"].GetShort ();
			y1 = node_100000000img_ladderRope_1["y1"].GetShort ();
			y2 = node_100000000img_ladderRope_1["y2"].GetShort ();
			ladder = node_100000000img_ladderRope_1["l"].GetInt ().ToBool ();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_ladder() const
		public bool is_ladder ()
		{
			return ladder;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool inrange(Point<short> position, bool upwards) const
		public bool inrange (Point<short> position, bool upwards)
		{
			var hor = Range<short>.symmetric (position.x (), 10);
			var ver = new Range<short> (y1, y2);

			short y = (short)(upwards ? position.y () - 5 : position.y () + 5);

			return hor.contains (x) && ver.contains (y);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool felloff(short y, bool downwards) const
		public bool felloff (short y, bool downwards)
		{
			short dy = (short)(downwards ? y + 5 : y - 5);

			return dy > y2 || y + 5 < y1;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short get_x() const
		public short get_x ()
		{
			return x;
		}

		private short x;
		private short y1;
		private short y2;
		private bool ladder;
	}

	public class MapInfo
	{
		public MapInfo (WzObject node_100000000img, Range<short> walls, Range<short> borders)
		{
			var node_node_100000000img_info = node_100000000img["info"];

			/*if (node_node_100000000img_info["VRLeft"].data_type() == nl.node.type.integer)
			{
				mapwalls = new Range<short>(node_node_100000000img_info["VRLeft"], node_node_100000000img_info["VRRight"]);
				mapborders = new Range<short>(node_node_100000000img_info["VRTop"], node_node_100000000img_info["VRBottom"]);
			}
			else*/
			{
				mapwalls = walls;
				mapborders = borders;
			}

			string bgmpath = node_node_100000000img_info["bgm"].ToString ();
			var split = bgmpath.IndexOf ('/');
			bgm = bgmpath.Substring (0, split) + ".img/" + bgmpath.Substring (split + 1);

			cloud = node_node_100000000img_info["cloud"].GetInt ().ToBool ();
			fieldlimit = node_node_100000000img_info["fieldLimit"].GetInt ();
			hideminimap = node_node_100000000img_info["hideMinimap"].GetInt ().ToBool ();
			mapmark = node_node_100000000img_info["mapMark"].ToString ();
			swim = node_node_100000000img_info["swim"].GetInt ().ToBool ();
			town = node_node_100000000img_info["town"].GetInt ().ToBool ();

			if (node_100000000img["seat"] is WzImageProperty node_100000000img_seat)
			{
				foreach (var node_100000000img_seat_0 in node_100000000img_seat.WzProperties)
				{
					seats.Add (new Seat (node_100000000img_seat_0));
				}
			}

			if (node_100000000img["ladderRope"] is WzImageProperty node_100000000img_ladderRope)
			{
				foreach (var node_100000000img_ladderRope_1 in node_100000000img_ladderRope.WzProperties)
				{
					ladders.Add (new Ladder (node_100000000img_ladderRope_1));
				}
			}
		}

		public MapInfo ()
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_underwater() const
		public bool is_underwater ()
		{
			return swim;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string get_bgm() const
		public string get_bgm ()
		{
			return bgm;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Range<short> get_walls() const
		public Range<short> get_walls ()
		{
			return mapwalls;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Range<short> get_borders() const
		public Range<short> get_borders ()
		{
			return mapborders;
		}

		// Find a seat the player's position
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Optional<const Seat> findseat(Point<short> position) const
		public Seat findseat (Point<short> position)
		{
			foreach (var seat in seats)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: if (seat.inrange(position))
				if (seat.inrange (position))
				{
					return seat;
				}
			}

			return null;
		}

		// Find a ladder at the player's position
		// !upwards - implies downwards
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Optional<const Ladder> findladder(Point<short> position, bool upwards) const
		public Ladder findladder (Point<short> position, bool upwards)
		{
			foreach (var ladder in ladders)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: if (ladder.inrange(position, upwards))
				if (ladder.inrange (position, upwards))
				{
					return ladder;
				}
			}

			return null;
		}

		private int fieldlimit;
		private bool cloud;
		private string bgm;
		private string mapdesc;
		private string mapname;
		private string streetname;
		private string mapmark;
		private bool swim;
		private bool town;
		private bool hideminimap;
		private Range<short> mapwalls = new Range<short> ();
		private Range<short> mapborders = new Range<short> ();
		private List<Seat> seats = new List<Seat> ();
		private List<Ladder> ladders = new List<Ladder> ();
	}
}