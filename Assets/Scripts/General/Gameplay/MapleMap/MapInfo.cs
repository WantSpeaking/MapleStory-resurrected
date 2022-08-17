using System.Collections.Generic;
using ms.Helper;
using MapleLib.WzLib;




namespace ms
{
	public class Seat
	{
		public Seat (WzImageProperty node_100000000img_seat_0)
		{
			var tempPoint = node_100000000img_seat_0.GetPoint ();
			pos = new Point_short ((short)tempPoint.X, (short)tempPoint.Y);
		}

		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: bool inrange(Point_short position) const
		public bool inrange (Point_short position)
		{
			var hor = Range_short.symmetric (position.x (), 10);
			var ver = Range_short.symmetric (position.y (), 10);

			return hor.contains (pos.x ()) && ver.contains (pos.y ());
		}

		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: Point_short getpos() const
		public Point_short getpos ()
		{
			return pos;
		}

		private Point_short pos = new Point_short ();
	}

	public class Ladder
	{
		public Ladder (WzImageProperty node_100000000img_ladderRope_1)
		{
			x = node_100000000img_ladderRope_1["x"];
			y1 = node_100000000img_ladderRope_1["y1"];
			y2 = node_100000000img_ladderRope_1["y2"];
			ladder = node_100000000img_ladderRope_1["l"];
		}

		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: bool is_ladder() const
		public bool is_ladder ()
		{
			return ladder;
		}

		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: bool inrange(Point_short position, bool upwards) const
		public bool inrange (Point_short position, bool upwards)
		{
			var hor = Range_short.symmetric (position.x (), 10);
			var ver = new Range_short (y1, y2);

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
		public MapInfo (WzObject node_100000000img, Range_short walls, Range_short borders)
		{
			var node_node_100000000img_info = node_100000000img["info"];

			if (node_node_100000000img_info["VRLeft"] != null)//todo 2 MapInfo VRLeft
			{
				mapwalls = new Range_short(node_node_100000000img_info["VRLeft"], node_node_100000000img_info["VRRight"]);
				mapborders = new Range_short(node_node_100000000img_info["VRTop"], node_node_100000000img_info["VRBottom"]);
			}
			else
			{
				mapwalls = new Range_short (walls);
				mapborders = new Range_short (borders);
			}

			string bgmpath = node_node_100000000img_info["bgm"].ToString ();
			var split = bgmpath.IndexOf ('/');
			bgm = bgmpath.Substring (0, split) + ".img/" + bgmpath.Substring (split + 1);

			cloud = node_node_100000000img_info["cloud"];
			fieldlimit = node_node_100000000img_info["fieldLimit"];
			hideminimap = node_node_100000000img_info["hideMinimap"];
			mapmark = node_node_100000000img_info["mapMark"]?.ToString () ?? string.Empty; //todo 2 mapmark maybe empty string
			swim = node_node_100000000img_info["swim"];
			town = node_node_100000000img_info["town"];

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
		//ORIGINAL LINE: Range_short get_walls() const
		public Range_short get_walls ()
		{
			return mapwalls;
		}

		public Range_short get_borders ()
		{
			return mapborders;
		}

		// Find a seat the player's position
		public Optional<Seat> findseat (Point_short position)
		{
			foreach (var seat in seats)
			{
				if (seat.inrange (new Point_short (position)))
				{
					return seat;
				}
			}

			return null;
		}

		// Find a ladder at the player's position
		// !upwards - implies downwards
		public Optional<Ladder> findladder (Point_short position, bool upwards)
		{
			foreach (var ladder in ladders)
			{
				if (ladder.inrange (new Point_short (position), upwards))
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
		private Range_short mapwalls = new Range_short ();
		private Range_short mapborders = new Range_short ();
		private List<Seat> seats = new List<Seat> ();
		private List<Ladder> ladders = new List<Ladder> ();
	}
}