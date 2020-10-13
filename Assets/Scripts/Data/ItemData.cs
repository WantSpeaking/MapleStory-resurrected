#define USE_NX

using System;
using Assets.ms.Helper;
using MapleLib.WzLib;

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
	// Class that represents an item loaded from the game's files
	// Contains all shared data between concrete items
	public class ItemData : Cache<ItemData>
	{
		// Returns whether the item was loaded correctly
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_valid() const
		public bool is_valid()
		{
			return valid;
		}
		// Returns whether the item is tradable or not
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_untradable() const
		public bool is_untradable()
		{
			return untradable;
		}
		// Returns whether the item is a one-of-a-kind item or not
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_unique() const
		public bool is_unique()
		{
			return unique;
		}
		// Returns whether the item is able to be sold or not
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_unsellable() const
		public bool is_unsellable()
		{
			return unsellable;
		}
		// Returns whether the item is a cash item or not
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_cashitem() const
		public bool is_cashitem()
		{
			return cashitem;
		}
		// Returns whether the item was loaded correctly
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator bool() const
		public static implicit operator bool(ItemData ImpliedObject)
		{
			return ImpliedObject.is_valid();
		}

		// Returns the item id
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_id() const
		public int get_id()
		{
			return itemid;
		}
		// Returns the item price
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_price() const
		public int get_price()
		{
			return price;
		}
		// Returns the item's gender based on item id
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: sbyte get_gender() const
		public sbyte get_gender()
		{
			return gender;
		}
		// Returns the item's name loaded from the String file
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string& get_name() const
		public string get_name()
		{
			return name;
		}
		// Returns the item's description loaded from the String file
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string& get_desc() const
		public string get_desc()
		{
			return desc;
		}
		// Return the item category (Also the node name)
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string& get_category() const
		public string get_category()
		{
			return category;
		}
		// Returns one of the item's icons
		// For each item there is a 'raw' icon and an icon with a drop shadow
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const Texture& get_icon(bool raw) const
		public Texture get_icon(bool raw)
		{
			return icons[raw];
		}

		// Allow the cache to use the constructor
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# has no concept of a 'friend' class:
//		friend Cache<ItemData>;
		// Creates an item from the game's Item file with the specified id
		public ItemData(int id)
		{
			this.itemid = id;
			untradable = false;
			unique = false;
			unsellable = false;
			cashitem = false;
			gender = 0;

			WzObject src = null;
			WzObject strsrc = null;

			string strprefix = "0" + Convert.ToString(get_item_prefix(itemid));
			string strid = "0" + Convert.ToString(itemid);
			int prefix = get_prefix(itemid);

			switch (prefix)
			{
			case 1:
				category = get_eqcategory(itemid);
				src = nl.nx.wzFile_character[category][strid + ".img"]["info"];
				strsrc = nl.nx.wzFile_string["Eqp.img"]["Eqp"][category][Convert.ToString(itemid)];
				break;
			case 2:
				category = "Consume";
				src = nl.nx.wzFile_item["Consume"][strprefix + ".img"][strid]["info"];
				strsrc = nl.nx.wzFile_string["Consume.img"][Convert.ToString(itemid)];
				break;
			case 3:
				category = "Install";
				src = nl.nx.wzFile_item["Install"][strprefix + ".img"][strid]["info"];
				strsrc = nl.nx.wzFile_string["Ins.img"][Convert.ToString(itemid)];
				break;
			case 4:
				category = "Etc";
				src = nl.nx.wzFile_item["Etc"][strprefix + ".img"][strid]["info"];
				strsrc = nl.nx.wzFile_string["Etc.img"]["Etc"][Convert.ToString(itemid)];
				break;
			case 5:
				category = "Cash";
				src = nl.nx.wzFile_item["Cash"][strprefix + ".img"][strid]["info"];
				strsrc = nl.nx.wzFile_string["Cash.img"][Convert.ToString(itemid)];
				break;
			}

			if (src != null)
			{
				icons[false] =new Texture (src["icon"]);
				icons[true] = new Texture (src["iconRaw"]);
				price = src["price"].GetInt ();
				untradable = src["tradeBlock"].GetInt ().ToBool();
				unique = src["only"].GetInt ().ToBool();
				unsellable = src["notSale"].GetInt ().ToBool();
				cashitem = src["cash"].GetInt ().ToBool();
				gender = get_item_gender(itemid);

				name = strsrc["name"].ToString ();
				desc = strsrc["desc"].ToString ();

				valid = true;
			}
			else
			{
				valid = false;
			}
		}

		string[] categorynames = {"Cap", "Accessory", "Accessory", "Accessory", "Coat", "Longcoat", "Pants", "Shoes", "Glove", "Shield", "Cape", "Ring", "Accessory", "Accessory", "Accessory"};

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string get_eqcategory(int id) const
		private string get_eqcategory(int id)
		{

			int index = get_item_prefix(id) - 100;

			if (index < 15)
			{
				return categorynames[index];
			}
			else if (index >= 30 && index <= 70)
			{
				return "Weapon";
			}
			else
			{
				return "";
			}
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_prefix(int id) const
		private int get_prefix(int id)
		{
			return id / 1000000;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_item_prefix(int id) const
		private int get_item_prefix(int id)
		{
			return id / 10000;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: sbyte get_item_gender(int id) const
		private sbyte get_item_gender(int id)
		{
			int item_prefix = get_item_prefix(id);

			if ((get_prefix(id) != 1 && item_prefix != 254) || item_prefix == 119 || item_prefix == 168)
			{
				return 2;
			}

			int gender_digit = id / 1000 % 10;

			return (sbyte)((gender_digit > 1) ? 2 : gender_digit);
		}

		private BoolPair<Texture> icons = new BoolPair<Texture>();
		private int itemid;
		private int price;
		private sbyte gender;
		private string name;
		private string desc;
		private string category;

		private bool valid;
		private bool untradable;
		private bool unique;
		private bool unsellable;
		private bool cashitem;
	}
}

#if USE_NX
#endif
