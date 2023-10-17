using MapleLib.WzLib;
using provider;

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
			this.m_horizontal = new ms.Range_short (src["x1"], src["x2"]);
			this.m_vertical = new ms.Range_short (src["y1"], src["y2"]);
			this.m_id = id;
			this.m_layer = ly;
		}

        public Foothold(MapleData src, ushort id, byte ly)
        {
            this.m_prev = (ushort)src["prev"];
            this.m_next = (ushort)src["next"];
            this.m_horizontal = new ms.Range_short(src["x1"], src["x2"]);
            this.m_vertical = new ms.Range_short(src["y1"], src["y2"]);
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
		public Range_short horizontal ()
		{
			return m_horizontal;
		}

		// Returns the vertical component
		public Range_short vertical ()
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
		public bool is_blocking (Range_short vertical)
		{
			return is_wall () && m_vertical.overlaps (vertical);
		}

		// Returns the Width
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
		private Range_short m_horizontal = new Range_short ();
		private Range_short m_vertical = new Range_short ();
	}
}