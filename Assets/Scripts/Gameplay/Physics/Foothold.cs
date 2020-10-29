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
		public Foothold()
		{
			this.m_id = 0;
			this.m_layer = 0;
			this.m_next = 0;
			this.m_prev = 0;
		}
		public Foothold(WzImageProperty src, ushort id, byte ly)
		{
			this.m_prev = (ushort)src["prev"];
			this.m_next = (ushort)src["next"];
			this.m_horizontal = new ms.Range<short>(src["x1"], src["x2"]);
			this.m_vertical = new ms.Range<short>(src["y1"], src["y2"]);
			this.m_id = id;
			this.m_layer = ly;
		}

		// Returns the foothold id aka the identifier in game data of this platform
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ushort id() const
		public ushort id()
		{
			return m_id;
		}
		// Returns the platform left to this
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ushort prev() const
		public ushort prev()
		{
			return m_prev;
		}
		// Returns the platform right to this
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ushort next() const
		public ushort next()
		{
			return m_next;
		}
		// Returns the platform's layer
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: byte layer() const
		public byte layer()
		{
			return m_layer;
		}
		// Returns the horizontal component
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const Range<short>& horizontal() const
		public Range<short> horizontal()
		{
			return m_horizontal;
		}
		// Returns the vertical component
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const Range<short>& vertical() const
		public Range<short> vertical()
		{
			return m_vertical;
		}

		// Return the left edge
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short l() const
		public short l()
		{
			return m_horizontal.smaller();
		}
		// Return the right edge
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short r() const
		public short r()
		{
			return m_horizontal.greater();
		}
		// Return the top edge
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short t() const
		public short t()
		{
			return m_vertical.smaller();
		}
		// Return the bottom edge
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short b() const
		public short b()
		{
			return m_vertical.greater();
		}
		// Return the first horizontal component
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short x1() const
		public short x1()
		{
			return m_horizontal.first();
		}
		// Return the second horizontal component
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short x2() const
		public short x2()
		{
			return m_horizontal.second();
		}
		// Return the first vertical component
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short y1() const
		public short y1()
		{
			return m_vertical.first();
		}
		// Return the second vertical component
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short y2() const
		public short y2()
		{
			return m_vertical.second();
		}
		// Return if the platform is a wall (x1 == x2)
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_wall() const
		public bool is_wall()
		{
			return m_id!=0 && m_horizontal.empty();
		}
		// Return if the platform is a floor (y1 == y2)
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_floor() const
		public bool is_floor()
		{
			return m_id!=0  && m_vertical.empty();
		}
		// Return if this platform is a left edge
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_left_edge() const
		public bool is_left_edge()
		{
			return m_id!=0  && m_prev == 0;
		}
		// Return if this platform is a right edge
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_right_edge() const
		public bool is_right_edge()
		{
			return m_id !=0 && m_next == 0;
		}
		// Returns if a x-coordinate is above or below this platform
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool hcontains(short x) const
		public bool hcontains(short x)
		{
			return m_id!=0  && m_horizontal.contains(x);
		}
		// Returns if a y-coordinate is right or left of this platform
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool vcontains(short y) const
		public bool vcontains(short y)
		{
			return m_id!=0  && m_vertical.contains(y);
		}
		// Check whether this foothold blocks an object
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_blocking(const Range<short>& vertical) const
		public bool is_blocking(Range<short> vertical)
		{
			return is_wall() && m_vertical.overlaps(vertical);
		}
		// Returns the width
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short hdelta() const
		public short hdelta()
		{
			return m_horizontal.delta();
		}
		// Returns the height
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short vdelta() const
		public short vdelta()
		{
			return m_vertical.delta();
		}
		// Returns the slope as a ratio of vertical/horizontal
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: double slope() const
		public double slope()
		{
			return is_wall() ? 0.0f : (double)vdelta() / hdelta();
		}
		// Returns a y-coordinate right below the given x-coordinate
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: double ground_below(double x) const
		public double ground_below(double x)
		{
			return is_floor() ? y1() : slope() * (x - x1()) + y1();
		}

		private ushort m_id;
		private ushort m_prev;
		private ushort m_next;
		private byte m_layer;
		private Range<short> m_horizontal = new Range<short>();
		private Range<short> m_vertical = new Range<short>();
	}
}
