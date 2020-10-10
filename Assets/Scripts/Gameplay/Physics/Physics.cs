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


using MapleLib.WzLib;

namespace ms
{
	// Class that uses physics engines and the collection of platforms to determine object movement
	public class Physics
	{
		public Physics(WzObject node_100000000img_foothold)
		{
			fht = new FootholdTree(node_100000000img_foothold);
			//fht = src;
		}
		public Physics()
		{
		}

		// Move the specified object over the specified game-time
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void move_object(PhysicsObject& phobj) const
		public void move_object(PhysicsObject phobj)
		{
			// Determine which platform the object is currently on
			fht.update_fh(phobj);

			// Use the appropriate physics for the terrain the object is on
			switch (phobj.type)
			{
			case PhysicsObject.Type.NORMAL:
				move_normal(phobj);
				fht.limit_movement(phobj);
				break;
			case PhysicsObject.Type.FLYING:
				move_flying(phobj);
				fht.limit_movement(phobj);
				break;
			case PhysicsObject.Type.SWIMMING:
				move_swimming(phobj);
				fht.limit_movement(phobj);
				break;
			case PhysicsObject.Type.FIXATED:
			default:
				break;
			}

			// Move the object forward
			phobj.move();
		}
		// Determine the point on the ground below the specified position
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> get_y_below(Point<short> position) const
		public Point<short> get_y_below(Point<short> position)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: short ground = fht.get_y_below(position);
			short ground = fht.get_y_below(position);

			return new Point<short>(position.x(), (short)(ground - 1));
		}
		// Return a reference to the collection of platforms
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const FootholdTree& get_fht() const
		public FootholdTree get_fht()
		{
			return fht;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void move_normal(PhysicsObject& phobj) const
		private void move_normal(PhysicsObject phobj)
		{
			phobj.vacc = 0.0;
			phobj.hacc = 0.0;

			if (phobj.onground)
			{
				phobj.vacc += phobj.vforce;
				phobj.hacc += phobj.hforce;

				if (phobj.hacc == 0.0 && phobj.hspeed < 0.1 && phobj.hspeed > -0.1)
				{
					phobj.hspeed = 0.0;
				}
				else
				{
					double inertia = phobj.hspeed / GlobalMembers.GROUNDSLIP;
					double slopef = phobj.fhslope;

					if (slopef > 0.5)
					{
						slopef = 0.5;
					}
					else if (slopef < -0.5)
					{
						slopef = -0.5;
					}

					phobj.hacc -= (GlobalMembers.FRICTION + GlobalMembers.SLOPEFACTOR * (1.0 + slopef * -inertia)) * inertia;
				}
			}
			else if (phobj.is_flag_not_set(PhysicsObject.Flag.NOGRAVITY))
			{
				phobj.vacc += GlobalMembers.GRAVFORCE;
			}

			phobj.hforce = 0.0;
			phobj.vforce = 0.0;

			phobj.hspeed += phobj.hacc;
			phobj.vspeed += phobj.vacc;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void move_flying(PhysicsObject& phobj) const
		private void move_flying(PhysicsObject phobj)
		{
			phobj.hacc = phobj.hforce;
			phobj.vacc = phobj.vforce;
			phobj.hforce = 0.0;
			phobj.vforce = 0.0;

			phobj.hacc -= GlobalMembers.FLYFRICTION * phobj.hspeed;
			phobj.vacc -= GlobalMembers.FLYFRICTION * phobj.vspeed;

			phobj.hspeed += phobj.hacc;
			phobj.vspeed += phobj.vacc;

			if (phobj.hacc == 0.0 && phobj.hspeed < 0.1 && phobj.hspeed > -0.1)
			{
				phobj.hspeed = 0.0;
			}

			if (phobj.vacc == 0.0 && phobj.vspeed < 0.1 && phobj.vspeed > -0.1)
			{
				phobj.vspeed = 0.0;
			}
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void move_swimming(PhysicsObject& phobj) const
		private void move_swimming(PhysicsObject phobj)
		{
			phobj.hacc = phobj.hforce;
			phobj.vacc = phobj.vforce;
			phobj.hforce = 0.0;
			phobj.vforce = 0.0;

			phobj.hacc -= GlobalMembers.SWIMFRICTION * phobj.hspeed;
			phobj.vacc -= GlobalMembers.SWIMFRICTION * phobj.vspeed;

			if (phobj.is_flag_not_set(PhysicsObject.Flag.NOGRAVITY))
			{
				phobj.vacc += GlobalMembers.SWIMGRAVFORCE;
			}

			phobj.hspeed += phobj.hacc;
			phobj.vspeed += phobj.vacc;

			if (phobj.hacc == 0.0 && phobj.hspeed < 0.1 && phobj.hspeed > -0.1)
			{
				phobj.hspeed = 0.0;
			}

			if (phobj.vacc == 0.0 && phobj.vspeed < 0.1 && phobj.vspeed > -0.1)
			{
				phobj.vspeed = 0.0f;
			}
		}

		private FootholdTree fht = new FootholdTree();
	}
}
