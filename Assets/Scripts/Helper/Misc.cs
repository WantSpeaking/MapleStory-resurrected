#define USE_NX

using System;
using System.Collections.Generic;
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
	public static class string_format
	{
		public static void split_number (string input)
		{
			for (int i = input.Length; i > 3; i -= 3)
				input.insert(i - 3, 1, ',');
		}

		public static string extend_id (int id, int length)
		{
			string strid = id.ToString ();
			//if (strid.Length < length)

			for (int i = 0; i < length - strid.Length; i++)
			{
				strid = strid.Insert (0, "0");
			}

			return strid;
		}

		public static void format_with_ellipsis (Text input, int length)
		{
			string text = input.get_text ();

			while (input.width () > length)
			{
				text = text.Substring (0, text.Length - 1);
				input.change_text (text + "..");
			}
		}

		public static void bytecode ()
		{
		}

		public static string tolower (string input)
		{
			return input.ToLower ();
		}
	}

	public static class string_conversion
	{
		public static T or_default<T> (string str, T defaultVaule)
		{
			T ret = (T)Convert.ChangeType (defaultVaule, typeof (T));
			try
			{
				ret = (T)Convert.ChangeType (str, typeof (T));
			}
			catch (Exception exception)
			{
			}

			return ret;
		}

		public static T or_zero<T> (string str)
		{
			return or_default<T> (str, default (T));
		}
	}

	namespace NxHelper
	{
		public class Map
		{
			public struct MapInfo
			{
				public string description;
				public string name;
				public string street_name;
				public string full_name;

				public MapInfo (string description, string name, string streetName, string fullName)
				{
					this.description = description;
					this.name = name;
					street_name = streetName;
					full_name = fullName;
				}
			};

			// Returns all relative map info
			public static MapInfo get_map_info_by_id (int mapid)
			{
				string map_category = get_map_category (mapid);
				WzObject map_info = nl.nx.wzFile_string["Map.img"][map_category][mapid.ToString ()];

				return new MapInfo (map_info["mapDesc"].ToString (), map_info["mapName"].ToString (), map_info["streetName"].ToString (), map_info["streetName"] + " : " + map_info["mapName"]);
			}

			// Returns the category of a map
			public static string get_map_category (int mapid)
			{
				if (mapid < 100000000)
					return "maple";

				if (mapid < 200000000)
					return "victoria";

				if (mapid < 300000000)
					return "ossyria";

				if (mapid < 540000000)
					return "elin";

				if (mapid < 600000000)
					return "singapore";

				if (mapid < 670000000)
					return "MasteriaGL";

				if (mapid < 682000000)
				{
					int prefix3 = (mapid / 1000000) * 1000000;
					int prefix4 = (mapid / 100000) * 100000;

					if (prefix3 == 674000000 || prefix4 == 680100000 || prefix4 == 889100000)
						return "etc";

					if (prefix3 == 677000000)
						return "Episode1GL";

					return "weddingGL";
				}

				if (mapid < 683000000)
					return "HalloweenGL";

				if (mapid < 800000000)
					return "event";

				if (mapid < 900000000)
					return "jp";

				return "etc";
			}

			public static Dictionary<long, Tuple<string, string>> map_life = new Dictionary<long, Tuple<string, string>> ();

			// Returns a list of all life on a map (Mobs and NPCs)
			public static Dictionary<long, Tuple<string, string>> get_life_on_map (int mapid)
			{
				map_life.Clear ();
				WzObject portal = get_map_node_name (mapid);

				foreach (var life in portal["life"])
				{
					long life_id = life["id"];
					string life_type = life["type"].ToString ();

					if (life_type == "m")
					{
						// Mob
						WzObject life_name = nl.nx.wzFile_string["Mob.img"][life_id.ToString ()]["name"];

						string life_id_str = string_format.extend_id ((int)life_id, 7);
						WzObject life_level = nl.nx.wzFile_mob[life_id_str + ".img"]["info"]["level"];

						if (life_name && life_level)
							map_life[life_id] = new Tuple<string, string> (life_type, life_name + "(Lv. " + life_level + ")");
					}
					else if (life_type == "n")
					{
						// NPC
						WzObject life_name = nl.nx.wzFile_string["Npc.img"][life_id.ToString ()]["name"];
						if (life_name != null)
							map_life[life_id] = new Tuple<string, string> (life_type, life_name.ToString ());
					}
				}

				return map_life;
			}

			// Returns the name of the node, under which the argument map id is in
			public static WzObject get_map_node_name (int mapid)
			{
				string prefix = (mapid / 100000000).ToString ();
				string mapid_str = string_format.extend_id (mapid, 9);

				return nl.nx.wzFile_map["Map"]["Map" + prefix][mapid_str + ".img"];
			}
		}
	}
}


#if USE_NX
#endif