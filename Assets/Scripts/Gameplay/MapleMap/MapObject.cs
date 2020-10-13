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
	// Base for objects on a map, e.g., Mobs, NPCs, Characters, etc.
	public abstract class MapObject : System.IDisposable
	{
		public MapObject ()
		{
			
		}
		
		public virtual void Dispose()
		{
		}

		// Draws the object at the given position and with the specified interpolation.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void draw(double viewx, double viewy, float alpha) const = 0;
		public abstract void draw(double viewx, double viewy, float alpha);

		// Updates the object and returns the updated layer.
		public virtual sbyte update(Physics physics)
		{
			physics.move_object(phobj);

			return (sbyte)phobj.fhlayer;
		}
		// Reactivates the object.
		public virtual void makeactive()
		{
			active = true;
		}
		// Deactivates the object.
		public virtual void deactivate()
		{
			active = false;
		}
		// Checks whether this object is active.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual bool is_active() const
		public virtual bool is_active()
		{
			return active;
		}
		// Obtains the layer used to determine the drawing order on the map.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual sbyte get_layer() const
		public virtual byte get_layer()
		{
			return phobj.fhlayer;
		}

		// Changes the objects position.
		public void set_position(short x, short y)
		{
			phobj.set_x(x);
			phobj.set_y(y);
		}
		// Changes the objects position.
		public void set_position(Point<short> position)
		{
			short x = position.x();
			short y = position.y();
			set_position(x, y);
		}

		// Returns the object id unique to every object on one map.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_oid() const
		public int get_oid()
		{
			return oid;
		}
		// Returns the current position.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> get_position() const
		public Point<short> get_position()
		{
			return phobj.get_position();
		}

		protected MapObject(int o, Point<short> p )
		{
			this.oid = o;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: set_position(p);
			set_position (p);
			active = true;
		}

		protected PhysicsObject phobj = new PhysicsObject();
		protected int oid;
		protected bool active;
	}
}
