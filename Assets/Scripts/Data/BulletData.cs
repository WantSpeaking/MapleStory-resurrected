#define USE_NX

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
	// Information about a bullet type item.
	public class BulletData : Cache<BulletData>
	{
		// Returns whether the bullet was loaded correctly.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_valid() const
		public bool is_valid()
		{
			return itemdata.is_valid();
		}
		// Returns whether the bullet was loaded correctly.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator bool() const
		public static implicit operator bool(BulletData ImpliedObject)
		{
			return ImpliedObject.is_valid();
		}

		// Returns the weapon attack increase when using this bullet.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short get_watk() const
		public short get_watk()
		{
			return watk;
		}
		// Returns the bullet animation.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const Animation& get_animation() const
		public Animation get_animation()
		{
			return bullet;
		}
		// Returns the general item data.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const ItemData& get_itemdata() const
		public ItemData get_itemdata()
		{
			return itemdata;
		}

		// Allow the cache to use the constructor.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# has no concept of a 'friend' class:
//		friend Cache<BulletData>;
		// Load a bullet from the game files.
		private BulletData(int itemid)
		{
			this.itemdata = ItemData.get(itemid);
			string prefix = "0" + Convert.ToString(itemid / 10000);
			string strid = "0" + Convert.ToString(itemid);
			var src  = nl.nx.wzFile_item["Consume"][prefix + ".img"][strid];

			bullet =new Animation(src["bullet"]); 
			watk = src["info"]["incPAD"].GetShort ();
		}

		private readonly ItemData itemdata;

		private Animation bullet = new Animation();
		private short watk;
	}
}

#if USE_NX
#endif
