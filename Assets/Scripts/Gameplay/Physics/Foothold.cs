#define USE_NX

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


#if USE_NX
#else
#endif

namespace ms
{
	// Represents a platform part on a maple map
	public class Foothold
	{
		public Foothold ()
		{
			this.m_id = 0;
			this.m_layer = 0;
			this.m_next = 0;
			this.m_prev = 0;
		}

		public Foothold (WzImageProperty src, ushort id, byte ly)
		{
			this.m_prev = (ushort)src["prev"];
			this.m_next = (ushort)src["next"];
			this.m_horizontal = new ms.Range<short> (src["x1"], src["x2"]);
			this.m_vertical = new ms.Range<short> (src["y1"], src["y2"]);
			this.m_id = id;
			this.m_layer = ly;
		}

		// Returns the foothold id aka the identifier in game data of this platform
		public ushort id ()
		{
			return m_id;
		}

		// Returns the platform left to this
		public ushort prev ()
		{
			return m_prev;
		}

		// Returns the platform right to this
		public ushort next ()
		{
			return m_next;
		}

		// Returns the platform's layer
		public byte layer ()
		{
			return m_layer;
		}

		// Returns the horizontal component
		public Range<short> horizontal ()
		{
			return m_horizontal;
		}

		// Returns the vertical component
		public Range<short> vertical ()
		{
			return m_vertical;
		}

		// Return the left edge
		public short l ()
		{
			return m_horizontal.smaller ();
		}

		// Return the right edge
		public short r ()
		{
			return m_horizontal.greater ();
		}

		// Return the top edge
		public short t ()
		{
			return m_vertical.smaller ();
		}

		// Return the bottom edge
		public short b ()
		{
			return m_vertical.greater ();
		}

		// Return the first horizontal component
		public short x1 ()
		{
			return m_horizontal.first ();
		}

		// Return the second horizontal component
		public short x2 ()
		{
			return m_horizontal.second ();
		}

		// Return the first vertical component
		public short y1 ()
		{
			return m_vertical.first ();
		}

		// Return the second vertical component
		public short y2 ()
		{
			return m_vertical.second ();
		}

		// Return if the platform is a wall (x1 == x2)
		public bool is_wall ()
		{
			return m_id != 0 && m_horizontal.empty ();
		}

		// Return if the platform is a floor (y1 == y2)
		public bool is_floor ()
		{
			return m_id != 0 && m_vertical.empty ();
		}

		// Return if this platform is a left edge
		public bool is_left_edge ()
		{
			return m_id != 0 && m_prev == 0;
		}

		// Return if this platform is a right edge
		public bool is_right_edge ()
		{
			return m_id != 0 && m_next == 0;
		}

		// Returns if a x-coordinate is above or below this platform
		public bool hcontains (short x)
		{
			return m_id != 0 && m_horizontal.contains (x);
		}

		// Returns if a y-coordinate is right or left of this platform
		public bool vcontains (short y)
		{
			return m_id != 0 && m_vertical.contains (y);
		}

		// Check whether this foothold blocks an object
		public bool is_blocking (Range<short> vertical)
		{
			return is_wall () && m_vertical.overlaps (vertical);
		}

		// Returns the width
		public short hdelta ()
		{
			return m_horizontal.delta ();
		}

		// Returns the height
		public short vdelta ()
		{
			return m_vertical.delta ();
		}

		// Returns the slope as a ratio of vertical/horizontal
		public double slope ()
		{
			return is_wall () ? 0.0f : (double)vdelta () / hdelta ();
		}

		// Returns a y-coordinate right below the given x-coordinate
		public double ground_below (double x)
		{
			return is_floor () ? y1 () : slope () * (x - x1 ()) + y1 ();
		}

		private ushort m_id;
		private ushort m_prev;
		private ushort m_next;
		private byte m_layer;
		private Range<short> m_horizontal = new Range<short> ();
		private Range<short> m_vertical = new Range<short> ();
	}
}