using System;

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
	// Represents a projectile on a map
	public class Bullet
	{
		public Bullet(Animation a, Point<short> origin, bool toleft)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: animation = a;
			animation=a;

			moveobj.set_x(origin.x() + (toleft ? -30.0 : 30.0));
			moveobj.set_y(origin.y() - 26.0);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(double viewx, double viewy, float alpha) const
		public void draw(double viewx, double viewy, float alpha)
		{
			Point<short> bulletpos = moveobj.get_absolute(viewx, viewy, alpha);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: DrawArgument args(bulletpos, flip);
			DrawArgument args = new DrawArgument(bulletpos, flip);
			animation.draw(args, alpha);
		}
		public bool settarget(Point<short> target)
		{
			double xdelta = target.x() - moveobj.crnt_x();
			double ydelta = target.y() - moveobj.crnt_y();

			if (Math.Abs(xdelta) < 10.0)
			{
				return true;
			}

			flip = xdelta > 0.0;

			moveobj.hspeed = xdelta / 32;

			if (xdelta > 0.0)
			{
				if (moveobj.hspeed < 3.0)
				{
					moveobj.hspeed = 3.0;
				}
				else if (moveobj.hspeed > 6.0)
				{
					moveobj.hspeed = 6.0;
				}
			}
			else if (xdelta < 0.0)
			{
				if (moveobj.hspeed > -3.0)
				{
					moveobj.hspeed = -3.0;
				}
				else if (moveobj.hspeed < -6.0)
				{
					moveobj.hspeed = -6.0;
				}
			}

			moveobj.vspeed = moveobj.hspeed * ydelta / xdelta;

			return false;
		}
		public bool update(Point<short> target)
		{
			animation.update();
			moveobj.move();

			short xdelta = (short)(target.x() - moveobj.get_x());
			return moveobj.hspeed > 0.0 ? xdelta < 10 : xdelta > 10;
		}

		private Animation animation = new Animation();
		private MovingObject moveobj = new MovingObject();
		private bool flip;
	}
}
