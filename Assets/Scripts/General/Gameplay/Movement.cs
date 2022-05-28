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
	public struct Movement
	{
		public enum Type
		{
			NONE,
			ABSOLUTE,
			RELATIVE,
			CHAIR,
			JUMPDOWN
		}

		public Movement(Type t, byte c, short x, short y, short lx, short ly, ushort f, byte s, short d)
		{
			this.type = t;
			this.command = c;
			this.xpos = x;
			this.ypos = y;
			this.lastx = lx;
			this.lasty = ly;
			this.fh = f;
			this.newstate = s;
			this.duration = d;
		}
		public Movement(short x, short y, short lx, short ly, byte s, short d) : this(Type.ABSOLUTE, 0, x, y, lx, ly, 0, s, d)
		{
		}
		public Movement(PhysicsObject phobj, byte s) : this(Type.ABSOLUTE, 0, phobj.get_x(), phobj.get_y(), phobj.get_last_x(), phobj.get_last_y(), phobj.fhid, s, 1)
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool hasmoved(const Movement& newmove) const
		public bool hasmoved(Movement newmove)
		{
			return newmove.newstate != newstate || newmove.xpos != xpos || newmove.ypos != ypos || newmove.lastx != lastx || newmove.lasty != lasty;
		}

		public Type type;
		public byte command;
		public short xpos;
		public short ypos;
		public short lastx;
		public short lasty;
		public ushort fh;
		public byte newstate;
		public short duration;
	}
}