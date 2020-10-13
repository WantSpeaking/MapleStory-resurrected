#define USE_NX

using System;
using System.Collections.Generic;
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
	public class Hair
	{
		public enum Layer
		{
			NONE,
			DEFAULT,
			BELOWBODY,
			OVERHEAD,
			SHADE,
			BACK,
			BELOWCAP,
			BELOWCAPNARROW,
			BELOWCAPWIDE,
			NUM_LAYERS
		}

		public Hair (int hairid, BodyDrawInfo drawinfo)
		{
			init_Dict ();
			var node_Hair_00030000img = nl.nx.wzFile_character["Hair"]["000" + Convert.ToString (hairid) + ".img"];

			foreach (var stance_iter in Stance.names)
			{
				Stance.Id stance = stance_iter.Key;
				string stancename = stance_iter.Value;

				var node_Hair_00030000img_alert = node_Hair_00030000img?[stancename];

				if (node_Hair_00030000img_alert == null)
				{
					continue;
				}

				if (node_Hair_00030000img_alert is WzImageProperty property_Hair_00030000img_alert)
				{
					foreach (var property_Hair_00030000img_alert_0 in property_Hair_00030000img_alert.WzProperties)
					{
						var frame = byte.Parse (property_Hair_00030000img_alert_0.Name);
						string layername = property_Hair_00030000img_alert_0.Name;

						layers_by_name.TryGetValue (layername, out var layer);

						Point<short> brow = property_Hair_00030000img_alert_0["map"]?["brow"]?.GetPoint ().ToMSPoint ()??Point<short>.zero;
						Point<short> shift = drawinfo.gethairpos (stance, frame)??Point<short>.zero - brow;
						var texture = new Texture (property_Hair_00030000img_alert_0);
						texture.shift (shift);
						stances[(int)stance, (int)layer]?.Add (frame, texture);
					}
				}

				/*for (byte frame = 0; nl.node framenode = node_Hair_00030000img_alert[frame]; ++frame)
				{
					foreach (nl  in :node layernode : framenode)
					{
						string layername = layernode.name();
						var layer_iter = GlobalMembers.layers_by_name.find(layername);

						if (layer_iter == GlobalMembers.layers_by_name.end())
						{
							Console.Write("Unknown Hair::Layer name: [");
							Console.Write(layername);
							Console.Write("]\tLocation: [");
							Console.Write(node_Hair_00030000img.name());
							Console.Write("][");
							Console.Write(stancename);
							Console.Write("][");
							Console.Write(frame);
							Console.Write("]");
							Console.Write("\n");
							continue;
						}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
						Layer layer = layer_iter.second;

						Point<short> brow = layernode["map"]["brow"];
						Point<short> shift = drawinfo.gethairpos(stance, frame) - brow;

						stances[(int)stance, (int)layer].Add(frame, layernode).first.second.shift(shift);
					}
				}*/
			}

			name = nl.nx.wzFile_string["Eqp.img"]["Eqp"]["Hair"][Convert.ToString (hairid)]["name"].ToString ();

			const uint NUM_COLORS = 8;
			string[] haircolors = {"Black", "Red", "Orange", "Blonde", "Green", "Blue", "Violet", "Brown"};

			uint index = (uint)(hairid % 10);
			color = (index < NUM_COLORS) ? haircolors[index] : "";
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Stance::Id stance, Layer layer, byte frame, const DrawArgument& args) const
		public void draw (Stance.Id stance, Layer layer, byte frame, DrawArgument args)
		{
			Texture frameit = null;
			stances[(int)stance, (int)layer]?.TryGetValue (frame, out frameit);
			frameit?.draw (args);

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

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string& getcolor() const
		public string getcolor ()
		{
			return color;
		}

		private Dictionary<byte, Texture>[,] stances = new Dictionary<byte, Texture>[(int)Stance.Id.LENGTH, (int)Layer.NUM_LAYERS];

		private void init_Dict ()
		{
			for (int i = 0; i < (int)Stance.Id.LENGTH; i++)
			{
				for (int j = 0; j < (int)Layer.NUM_LAYERS; j++)
				{
					stances[i, j] = new Dictionary<byte, Texture> ();
				}
			}
		}

		private string name;
		private string color;

		private readonly Dictionary<string, Layer> layers_by_name = new Dictionary<string, Layer> ()
		{
			{"hair", Layer.DEFAULT},
			{"hairBelowBody", Layer.BELOWBODY},
			{"hairOverHead", Layer.OVERHEAD},
			{"hairShade", Layer.SHADE},
			{"backHair", Layer.BACK},
			{"backHairBelowCap", Layer.BELOWCAP},
			{"backHairBelowCapNarrow", Layer.BELOWCAPNARROW},
			{"backHairBelowCapWide", Layer.BELOWCAPWIDE}
		};
	}
}


#if USE_NX
#endif