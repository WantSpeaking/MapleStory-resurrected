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


using Assets.ms;
using Assets.ms.Helper;
using MapleLib.WzLib;

namespace ms
{
	// Represents a map decoration (object) on a map
	public class Obj
	{
		private int orderInLayer;

		public Obj (WzImageProperty node_100000000img_0_obj_0)
		{
			int.TryParse (node_100000000img_0_obj_0.Name, out orderInLayer);

			//UnityEngine.Debug.Log(node_100000000img_0_obj_0["oS"] + ".img" + "\t" + node_100000000img_0_obj_0["l0"] + "\t" + node_100000000img_0_obj_0["l1"] + "\t" + node_100000000img_0_obj_0["l2"]);

			animation = new Animation (nl.nx.map["Obj"][node_100000000img_0_obj_0["oS"] + ".img"][node_100000000img_0_obj_0["l0"].ToString ()][node_100000000img_0_obj_0["l1"].ToString ()][node_100000000img_0_obj_0["l2"].ToString ()]);
			//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
			//ORIGINAL LINE: pos = Point<short>(src["x"], src["y"]);

			pos = new Point<short> (node_100000000img_0_obj_0["x"].GetShort (), node_100000000img_0_obj_0["y"].GetShort ());
			flip = node_100000000img_0_obj_0["f"].GetInt ().ToBool ();
			//z = (byte)(255- node_100000000img_0_obj_0["z"].GetShort ().ToByte ());//orderInLayer wz和unity正好相反
			z = (node_100000000img_0_obj_0["z"].GetShort ().ToByte ());//orderInLayer wz和unity正好相反
			if (z==0)
			{
				z = (node_100000000img_0_obj_0["zM"].GetShort ().ToByte ());
			}
		}

		// Update animation
		public void update ()
		{
			animation.update ();
		}

		// Draw the object at the specified position
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: void draw(Point<short> viewpos, float inter) const
		public void draw (Point<short> viewpos, float inter,int layerId)
		{
			var tempPoint = new Point<short> ((short)(pos.x () + viewpos.x ()), (short)(pos.y () + viewpos.y ()));
			animation.draw (new DrawArgument (tempPoint, layerId, z), inter);
		}

		// Return the depth of the object
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: byte getz() const
		public byte getz ()
		{
			return z;
		}

		private Animation animation = new Animation ();
		private Point<short> pos = new Point<short> ();
		private byte z;
		private bool flip;
	}
}

#if USE_NX
#endif