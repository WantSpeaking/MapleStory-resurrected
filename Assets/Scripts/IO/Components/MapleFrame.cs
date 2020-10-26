#define USE_NX

using System;
using MapleLib.WzLib;
using UnityEngine.SceneManagement;

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
	public class MapleFrame
	{
		public MapleFrame()
		{
		}
		public MapleFrame(WzObject src)
		{
			center = src["c"];
			east = src["e"];
			northeast = src["ne"];
			north = src["n"];
			northwest = src["nw"];
			west = src["w"];
			southwest = src["sw"];
			south = src["s"];
			southeast = src["se"];

			xtile = Math.Max(north.width(), (short)1);
			ytile = Math.Max(west.height(), (short)1);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point<short> position, short rwidth, short rheight) const
		public void draw(Point<short> position, short rwidth, short rheight)
		{
			short numhor = (short)(rwidth / xtile + 2);
			short numver = (short)(rheight / ytile);
			int width = numhor * xtile;
			int height = numver * ytile;
			short left = (short)(position.x() - width / 2);
			short top = (short)(position.y() - height);
			short right = (short)(left + width);
			int bottom = top + height;

			northwest.draw(new DrawArgument(left, top));
			southwest.draw(new DrawArgument(left, bottom));

			for (short y = top; y < bottom; y += ytile)
			{
				west.draw(new DrawArgument(left, y));
				east.draw(new DrawArgument(right, y));
			}

			center.draw(new DrawArgument(new Point<short>(left, top), new Point<short>((short)width, (short)height)));

			for (short x = left; x < right; x += xtile)
			{
				north.draw(new DrawArgument(x, top));
				south.draw(new DrawArgument(x, bottom));
			}

			northeast.draw(new DrawArgument(right, top));
			southeast.draw(new DrawArgument(right, bottom));
		}

		private Texture center = new Texture();
		private Texture east = new Texture();
		private Texture northeast = new Texture();
		private Texture north = new Texture();
		private Texture northwest = new Texture();
		private Texture west = new Texture();
		private Texture southwest = new Texture();
		private Texture south = new Texture();
		private Texture southeast = new Texture();
		private short xtile;
		private short ytile;
	}
}

#if USE_NX
#endif
