using System;
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
	// View on stage which follows the player object.
	public class Camera
	{
		// Initialize everything to 0, we need the player's spawnpoint first to properly set the position.
		public Camera ()
		{
			Constants.get().set_viewwidth(800);//todo Constants.get().set_viewwidth(800) remove later
			Constants.get().set_viewheight(600);
			
			x.set (0.0);
			y.set (0.0);

			VWIDTH = Constants.get ().get_viewwidth ();
			VHEIGHT = Constants.get ().get_viewheight ();
		}

		// Update the view with the current player position. (Or any other target)
		public void update (Point<short> position)
		{
			//Debug.Log ($"player position: {position}");
			var new_width = Constants.get ().get_viewwidth ();
			var new_height = Constants.get ().get_viewheight ();

			if (VWIDTH != new_width || VHEIGHT != new_height)
			{
				VWIDTH = new_width;
				VHEIGHT = new_height;
			}

			double next_x = x.get ();
			double hdelta = VWIDTH / 2 - position.x () - next_x;

			if (Math.Abs (hdelta) >= 5.0)
			{
				next_x += hdelta * (12.0 / VWIDTH);
			}

			double next_y = y.get ();
			double vdelta = VHEIGHT / 2 - position.y () - next_y;

			if (Math.Abs (vdelta) >= 5.0)
			{
				next_y += vdelta * (12.0 / VHEIGHT);
			}

			if (next_x > hbounds.first () || hbounds.length () < VWIDTH)
			{
				next_x = hbounds.first ();
			}
			else if (next_x < hbounds.second () + VWIDTH)
			{
				next_x = hbounds.second () + VWIDTH;
			}

			if (next_y > vbounds.first () || vbounds.length () < VHEIGHT)
			{
				next_y = vbounds.first ();
			}
			else if (next_y < vbounds.second () + VHEIGHT)
			{
				next_y = vbounds.second () + VHEIGHT;
			}

			x = next_x;
			y = next_y;
		}

		// Set the position, changing the view immediately.
		public void set_position (Point<short> position)
		{
			var new_width = Constants.get ().get_viewwidth ();
			var new_height = Constants.get ().get_viewheight ();

			if (VWIDTH != new_width || VHEIGHT != new_height)
			{
				VWIDTH = new_width;
				VHEIGHT = new_height;
			}

			x.set (VWIDTH / 2 - position.x ());
			y.set (VHEIGHT / 2 - position.y ());
		}

		// Updates the view's boundaries. Determined by mapinfo or footholds.
		public void set_view (Range<short> mapwalls, Range<short> mapborders)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: hbounds = -mapwalls;
			hbounds = -mapwalls;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: vbounds = -mapborders;
			vbounds = -mapborders;
		}

		// Return the current position.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> position() const
		public Point<short> position ()
		{
			var shortx = (short)Math.Round (x.get ());
			var shorty = (short)Math.Round (y.get ());

			return new Point<short> (shortx, shorty);
		}

		// Return the interpolated position.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> position(float alpha) const
		public Point<short> position (float alpha)
		{
			var interx = (short)Math.Round (x.get (alpha));
			var intery = (short)Math.Round (y.get (alpha));

			return new Point<short> (interx, intery);
		}

		// Return the interpolated position.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<double> realposition(float alpha) const
		public Point<double> realposition (float alpha)
		{
			return new Point<double> (x.get (alpha), y.get (alpha));
		}

	
		
		// Movement variables.
		public Linear<double> x = new Linear<double> ();
		public Linear<double> y = new Linear<double> ();

		// View limits.
		public Range<short> hbounds = new Range<short> ();
		public Range<short> vbounds = new Range<short> ();

		private short VWIDTH;
		private short VHEIGHT;
	}
}