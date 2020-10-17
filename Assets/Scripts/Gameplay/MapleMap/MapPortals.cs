#define USE_NX

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
	// Collection of portals on a map
	// Draws and updates portals
	// Also contains methods for using portals and obtaining spawn points
	public class MapPortals
	{
		public static void init ()
		{
			var src = nl.nx.wzFile_map["MapHelper.img"]["portal"]["game"];

			/*animations[Portal.Type.HIDDEN] = new Animation (src["ph"]["default"]["portalContinue"]);
			animations[Portal.Type.REGULAR] = new Animation (src["pv"]);

			animations[Portal.Type.SPAWN] = new Animation ();
			animations[Portal.Type.INVISIBLE] = new Animation ();

			animations[Portal.Type.TOUCH] = new Animation ();
			animations[Portal.Type.TYPE5] = new Animation ();
			animations[Portal.Type.WARP] = new Animation ();
			animations[Portal.Type.SPAWN] = new Animation ();
			animations[Portal.Type.SCRIPTED] = new Animation ();
			animations[Portal.Type.SCRIPTED_INVISIBLE] = new Animation ();
			animations[Portal.Type.SCRIPTED_TOUCH] = new Animation ();

			animations[Portal.Type.SCRIPTED_HIDDEN] = new Animation ();
			animations[Portal.Type.SPRING1] = new Animation ();
			animations[Portal.Type.SPRING2] = new Animation ();
			animations[Portal.Type.TYPE14] = new Animation ();*/
		}

		public MapPortals (WzObject node_100000000img_portal, int mapid)
		{
			if (node_100000000img_portal is WzImageProperty property_100000000img_portal)
			{
				foreach (var property_100000000img_portal_0 in property_100000000img_portal.WzProperties)
				{
					sbyte portal_id = string_conversion<sbyte>.or_default (property_100000000img_portal_0.Name, (sbyte)-1);

					if (portal_id < 0)
					{
						continue;
					}


					Portal.Type type = Portal.typebyid (property_100000000img_portal_0["pt"]);
					string name = property_100000000img_portal_0["pn"].ToString ();

					//if (name.Contains ("west00"))
					{
						string target_name = property_100000000img_portal_0["tn"].ToString ();
						int target_id = property_100000000img_portal_0["tm"];
						Point<short> position = new Point<short> (property_100000000img_portal_0["x"], property_100000000img_portal_0["y"]);

						Animation animation = Get_AnimationByType (type);
						bool intramap = target_id == mapid;
						portals_by_id.Add ((byte)portal_id, new Portal (animation, type, name, intramap, position, target_id, target_name));
						//portals_by_id.emplace(std::piecewise_construct, std::forward_as_tuple(portal_id), std::forward_as_tuple(animation, type, name, intramap, position, target_id, target_name));

						portal_ids_by_name.TryAdd (name, (byte)portal_id); //todo add repeatedly
					}
				}
			}


			cooldown = WARPCD;
		}

		public MapPortals ()
		{
			cooldown = WARPCD;
		}

		public void update (Point<short> playerpos)
		{
			//animations[Portal.Type.REGULAR].update ((ushort)Constants.TIMESTEP);
			//animations[Portal.Type.HIDDEN].update ((ushort)Constants.TIMESTEP);

			foreach (var iter in portals_by_id)
			{
				Portal portal = iter.Value;
				switch (portal.get_type ())
				{
					case Portal.Type.HIDDEN:
					case Portal.Type.TOUCH:
						portal.update (playerpos);
						break;
				}
			}

			if (cooldown > 0)
			{
				cooldown--;
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point<short> viewpos, float inter) const
		public void draw (Point<short> viewpos, float inter)
		{
			foreach (var ptit in portals_by_id)
			{
				ptit.Value.draw (viewpos, inter);
			}
		}

		public Portal.WarpInfo find_warp_at (Point<short> playerpos)
		{
			if (cooldown == 0)
			{
				cooldown = WARPCD;

				foreach (var iter in portals_by_id)
				{
					Portal portal = iter.Value;

					if (portal.bounds ().contains (playerpos))
					{
						return portal.getwarpinfo ();
					}
				}
			}

			return default;
			/*return
			{
			};*/
		}

		readonly Point<short> ABOVE = new Point<short> (0, 30);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> get_portal_by_id(byte portal_id) const
		public Point<short> get_portal_by_id (byte portal_id)
		{
			if (portals_by_id.TryGetValue (portal_id, out var portalPos))
			{
				return portalPos.get_position () - ABOVE;
			}

			return Point<short>.zero;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> get_portal_by_name(const string& portal_name) const
		public Point<short> get_portal_by_name (string portal_name)
		{
			if (portal_ids_by_name.TryGetValue (portal_name, out var portalId))
			{
				return get_portal_by_id (portalId);
			}

			return Point<short>.zero;
		}

		private Animation Get_AnimationByType (Portal.Type portalType)
		{
			Animation ani;
			var src = nl.nx.wzFile_map["MapHelper.img"]["portal"]["game"];

			if (portalType == Portal.Type.HIDDEN)
			{
				ani = new Animation (src["ph"]["default"]["portalContinue"]);
			}
			else if (portalType == Portal.Type.REGULAR)
			{
				ani = new Animation (src["pv"]);
			}
			else
			{
				ani = new Animation ();
			}

			return ani;
		}

		private static Dictionary<Portal.Type, Animation> animations = new Dictionary<Portal.Type, Animation> ();

		private Dictionary<byte, Portal> portals_by_id = new Dictionary<byte, Portal> ();
		private Dictionary<string, byte> portal_ids_by_name = new Dictionary<string, byte> ();

		private const short WARPCD = 48;
		private short cooldown;
	}
}


#if USE_NX
#endif