#define USE_NX

using System;
using System.Collections.Generic;
using ms.Helper;
using MapleLib.WzLib;
using UnityEngine;

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
	public class Body
	{
		public enum Layer
		{
			NONE,
			BODY,
			ARM,
			ARM_BELOW_HEAD,
			ARM_BELOW_HEAD_OVER_MAIL,
			ARM_OVER_HAIR,
			ARM_OVER_HAIR_BELOW_WEAPON,
			HAND_BELOW_WEAPON,
			HAND_OVER_HAIR,
			HAND_OVER_WEAPON,
			HEAD,
			NUM_LAYERS
		}

		public Body (int skin, BodyDrawInfo drawinfo)
		{
			init_Dict ();

			string strid = string_format.extend_id (skin, 2);
			var bodynode = nl.nx.wzFile_character["000020" + strid + ".img"];
			var headnode = nl.nx.wzFile_character["000120" + strid + ".img"];

			foreach (var iter in Stance.names)
			{
				Stance.Id stance = iter.Key;
				string stancename = iter.Value;

				var node_00002000img_alert = bodynode?[stancename];

				if (node_00002000img_alert == null)
				{
					continue;
				}

				if (node_00002000img_alert is WzImageProperty property_00002000img_alert)
				{
					foreach (var property_00002000img_alert_0 in property_00002000img_alert.WzProperties)
					{
						var frame = byte.Parse (property_00002000img_alert_0.Name);
						foreach (var property_00002000img_alert_0_arm in property_00002000img_alert_0.WzProperties)
						{
							string part = property_00002000img_alert_0_arm.Name;

							if (part != "delay" && part != "face")
							{
								string z = property_00002000img_alert_0_arm["z"]?.ToString ();
								Body.Layer layer = layer_by_name (z);

								if (layer == Body.Layer.NONE)
								{
									continue;
								}

								Point<short> shift = new Point<short> ();

								switch (layer)
								{
									case Body.Layer.HAND_BELOW_WEAPON:
										shift = (drawinfo.get_hand_position (stance, frame)) ?? new Point<short> ();
										var tempPoint = property_00002000img_alert_0_arm["map"]?["handMove"]?.GetPoint ().ToMSPoint () ?? new Point<short> ();
										shift = shift - tempPoint;
										break;
									default:
										shift = (drawinfo.get_body_position (stance, frame)) ?? new Point<short> ();
										var tempPoint1 = property_00002000img_alert_0_arm["map"]["navel"]?.GetPoint ().ToMSPoint () ?? new Point<short> ();
										//Debug.Log ($"shift:{shift} tempPoint:{tempPoint1}");
										shift = shift - tempPoint1;
										break;
								}

								var texture = new Texture (property_00002000img_alert_0_arm);
								texture.shift (shift);
								//Debug.Log (texture.get_origin ());
								stances[(int)stance, (int)layer][frame] = texture; //todo might repeat add
								/*stances[stance][layer]
								.emplace(frame, partnode)
								.first->second.shift(shift);
								*/
							}
						}

						if (headnode[stancename]?[frame.ToString ()]?["head"] is WzObject headsfnode)
						{
							Point<short> shift = drawinfo.get_head_position (stance, frame) ?? new Point<short> ();

							var texture = new Texture (headsfnode);
							texture.shift (shift);
							stances[(int)stance, (int)Body.Layer.HEAD]?.Add (frame, texture);
						}
					}
				}
			}

			const uint NUM_SKINTYPES = 12;

			string[] skintypes = new[] {"Light", "Tan", "Dark", "Pale", "Blue", "Green", "", "", "", "Grey", "Pink", "Red"};

			uint index = (uint)skin;
			name = (index < NUM_SKINTYPES) ? skintypes[index] : "";
		}


		public void draw (Stance.Id stance, Layer layer, byte frame, DrawArgument args, bool drawOrErase = true)
		{
			Texture frameit = null;
			stances[(int)stance, (int)layer]?.TryGetValue (frame, out frameit);
			if (drawOrErase)
			{
				frameit?.draw (args);
			}
			else
			{
				frameit?.erase ();
			}

			//var frameit = expressions[(int)expression].find(frame);
			/*var frameit = stances[(int)stance, (int)layer].find(frame);

			if (frameit == stances[(int)stance, (int)layer].end())
			{
				return;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			frameit.second.draw(args);*/
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string& get_name() const
		public string get_name ()
		{
			return name;
		}

		public static Body.Layer layer_by_name (string name)
		{
			if (string.IsNullOrEmpty (name)) return Layer.NONE;
			layers_by_name.TryGetValue (name, out var layer);
			return layer;
			/*var layer_iter = layers_by_name.find(name);

			if (layer_iter == layers_by_name.end())
			{
				if (name != "")
				{
					Console.Write("Unknown Body::Layer name: [");
					Console.Write(name);
					Console.Write("]");
					Console.Write("\n");
				}

				return Body.Layer.NONE;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			return layer_iter.second;*/
		}

		private Dictionary<byte, Texture>[,] stances = new Dictionary<byte, Texture>[(int)Enum.GetNames (typeof (Stance.Id)).Length, (int)Layer.NUM_LAYERS];

		private void init_Dict ()
		{
			for (int i = 0; i < (int)Enum.GetNames (typeof (Stance.Id)).Length; i++)
			{
				for (int j = 0; j < (int)Layer.NUM_LAYERS; j++)
				{
					stances[i, j] = new Dictionary<byte, Texture> ();
				}
			}
		}

		private string name;

		private static Dictionary<string, Layer> layers_by_name = new Dictionary<string, Layer> ()
		{
			{"body", Layer.BODY},
			{"backBody", Layer.BODY},
			{"arm", Layer.ARM},
			{"armBelowHead", Layer.ARM_BELOW_HEAD},
			{"armBelowHeadOverMailChest", Layer.ARM_BELOW_HEAD_OVER_MAIL},
			{"armOverHair", Layer.ARM_OVER_HAIR},
			{"armOverHairBelowWeapon", Layer.ARM_OVER_HAIR_BELOW_WEAPON},
			{"handBelowWeapon", Layer.HAND_BELOW_WEAPON},
			{"handOverHair", Layer.HAND_OVER_HAIR},
			{"handOverWeapon", Layer.HAND_OVER_WEAPON},
			{"head", Layer.HEAD}
		};
	}
}


#if USE_NX
#endif