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


using UnityEngine;

namespace ms
{
	public class Portal
	{
		public enum Type
		{
			SPAWN,
			INVISIBLE,
			REGULAR,
			TOUCH,
			TYPE4,
			TYPE5,
			WARP,
			SCRIPTED,
			SCRIPTED_INVISIBLE,
			SCRIPTED_TOUCH,
			HIDDEN,
			SCRIPTED_HIDDEN,
			SPRING1,
			SPRING2,
			TYPE14
		}

		public static Type typebyid (int id)
		{
			return (Type)id;
		}

		public struct WarpInfo
		{
			public int mapid;
			public string toname;
			public string name;
			public bool intramap;
			public bool valid;

			public WarpInfo (int m, bool i, string tn, string n)
			{
				this.mapid = m;
				this.intramap = i;
				this.toname = tn;
				this.name = n;
				valid = mapid < 999999999;
			}

			/*public WarpInfo() : this(999999999, false, string.Empty, string.Empty)
			{
			}*/
		}

		public Portal (Animation a, Type t, string nm, bool intramap, Point<short> p, int tid, string tnm)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: this.animation = a;
			this.animation = a;
			this.type = t;
			this.name = nm;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.position = new ms.Point<short>(p);
			this.position = p;
			this.warpinfo = new ms.Portal.WarpInfo (tid, intramap, tnm, nm);
			touched = false;
		}

		public Portal () : this (null, Type.SPAWN, "", false, new Point<short> (), 0, "")
		{
		}

		public void update (Point<short> playerpos)
		{
			touched = bounds ().contains (playerpos);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point<short> viewpos, float inter) const
		public void draw (Point<short> viewpos, float inter)
		{
			if (animation == null || (type == Type.HIDDEN && !touched))
			{
				return;
			}

			//Debug.Log ($"protal draw postion: {position}");
			animation.update ();
			animation.draw (new DrawArgument (position + viewpos, 8, 0), inter);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string get_name() const
		public string get_name ()
		{
			return name;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Portal::Type get_type() const
		public Portal.Type get_type ()
		{
			return type;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> get_position() const
		public Point<short> get_position ()
		{
			return position;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Rectangle<short> bounds() const
		public Rectangle<short> bounds ()
		{
			var lt = position + new Point<short> (-25, -100);
			var rb = position + new Point<short> (25, 25);

			return new Rectangle<short> (lt, rb);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Portal::WarpInfo getwarpinfo() const
		public Portal.WarpInfo getwarpinfo ()
		{
			return warpinfo;
		}

		private readonly Animation animation;
		private Type type;
		private string name;
		private Point<short> position = new Point<short> ();
		private WarpInfo warpinfo = new WarpInfo ();
		private bool touched;
	}
}