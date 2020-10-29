#define USE_NX

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
#endif

using System.Diagnostics;
using ms.Helper;
using MapleLib.WzLib;

namespace ms
{
	// Represents a tile on a map
	public class Tile
	{
		private int orderInLayer;

		public Tile (WzImageProperty node_100000000img_0_Tile_0, string ts)
		{
			int.TryParse (node_100000000img_0_Tile_0.Name, out orderInLayer);
			//UnityEngine. Debug.Log(node_100000000img_0_Tile_0["u"].WzValue.ToString()+"\t"+ts + "\t" + node_100000000img_0_Tile_0["no"].ToString());
			var node_Tile_allblackTileimg_bsc_0 = nl.nx.wzFile_map["Tile"][ts][node_100000000img_0_Tile_0["u"].ToString ()][node_100000000img_0_Tile_0["no"].ToString ()];
			texture = new Texture (node_Tile_allblackTileimg_bsc_0);
			//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
			//ORIGINAL LINE: pos = Point<short>(src["x"], src["y"]);
			pos = new Point<short> (node_100000000img_0_Tile_0["x"], node_100000000img_0_Tile_0["y"]);
			//z = (byte)(255- node_100000000img_0_Tile_0["zM"].GetShort ().ToByte ());//todo orderInLayer wz和unity正好相反
			//z = (node_100000000img_0_Tile_0["zM"].GetShort ().ToByte ());//todo orderInLayer wz和unity正好相反
			/*if (z == 0)
			{
			    z = node_Tile_allblackTileimg_bsc_0["zM"].GetShort().ToByte();
			}*/
		}

		// Draw the tile
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: void draw(Point<short> viewpos) const
		public void draw (Point<short> viewpos,int layerId)
		{
			var tempPoint = new Point<short> ((short)(pos.x () + viewpos.x ()), (short)(pos.y () + viewpos.y ()));
			texture.draw (new DrawArgument (tempPoint, layerId, orderInLayer));
		}

		// Returns the depth of the tile
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: byte getz() const
		public byte getz ()
		{
			return z;
		}

		private Texture texture = new Texture ();
		private Point<short> pos = new Point<short> ();
		private byte z;
	}
}

#if USE_NX
#endif