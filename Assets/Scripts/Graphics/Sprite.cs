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


using System;
using MapleLib.WzLib;

namespace ms
{
	// Combines an Animation with additional state
	public class Sprite : IDisposable
	{
		public Sprite (Animation a, DrawArgument args)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.animation = new ms.Animation(a);
			this.animation = a;
			this.stateargs = args;
		}

		public Sprite (WzObject src, DrawArgument args)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.animation = new ms.Animation(src);
			this.animation = new ms.Animation (src);
			this.stateargs = args;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this(src, {});
		public Sprite (WzObject src) : this (src, new DrawArgument ())
		{
		}

		public Sprite ()
		{
		}

		public void Dispose ()
		{
			animation.Dispose ();
		}

		private static DrawArgument uiRenderOrderArgs = new DrawArgument(Constants.get ().sortingLayer_UI,0);

		public void draw (Point<short> parentpos, float alpha)
		{
			//var absargs = stateargs + parentpos + uiRenderOrderArgs;//todo uiRenderOrderArgs
			var absargs = stateargs + parentpos;
			animation.draw (absargs, alpha);
		}
		
		public void draw (Point<short> parentpos, float alpha, DrawArgument renderOrderArgs)
		{
			var absargs = renderOrderArgs != null ? stateargs + parentpos + renderOrderArgs : stateargs + parentpos;
			animation.draw (absargs, alpha);
		}

		public bool update (ushort timestep)
		{
			return animation.update (timestep);
		}

		public bool update ()
		{
			return animation.update ();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short width() const
		public short width ()
		{
			return get_dimensions ().x ();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short height() const
		public short height ()
		{
			return get_dimensions ().y ();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> get_origin() const
		public Point<short> get_origin ()
		{
			return animation.get_origin ();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> get_dimensions() const
		public Point<short> get_dimensions ()
		{
			return animation.get_dimensions ();
		}

		private Animation animation = new Animation ();
		private DrawArgument stateargs = new DrawArgument ();
	}
}