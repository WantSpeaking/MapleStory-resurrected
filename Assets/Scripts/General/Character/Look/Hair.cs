#define USE_NX

using System;
using System.Collections.Generic;
using Helper;
using ms.Helper;
using MapleLib.WzLib;
using provider;

namespace ms
{
	public class Hair : IDisposable
	{
		public enum Layer
		{
			NONE = 0,
			DEFAULT = 1,
			BELOWBODY = 2,
			OVERHEAD = 3,
			SHADE = 4,
			BACK = 5,
			BELOWCAP = 6,
			BELOWCAPNARROW = 7,
			BELOWCAPWIDE = 8,
		}

		public Hair(int hairid, BodyDrawInfo drawinfo)
		{
			init_Dict();
			var node_Hair_00030000img = ms.wz.wzProvider_character[$"Hair/000{hairid}.img"];

			if (node_Hair_00030000img == null)
			{
				AppDebug.Log($"hair is null,id:{hairid}");
				node_Hair_00030000img = ms.wz.wzProvider_character[$"Hair/00030000.img"];
				hairid = 30000;
			}
			foreach (var stance_iter in Stance.names)
			{
				Stance.Id stance = stance_iter.Key;
				string stancename = stance_iter.Value;

				var node_Hair_00030000img_alert = node_Hair_00030000img?[stancename];

				if (node_Hair_00030000img_alert == null)
				{
					continue;
				}

				if (node_Hair_00030000img_alert is MapleData property_Hair_00030000img_alert)
				{
					foreach (var property_Hair_00030000img_alert_0 in property_Hair_00030000img_alert)
					{
						var frame = byte.Parse(property_Hair_00030000img_alert_0.Name);

						foreach (var property_Hair_00030000img_alert_0_hair in property_Hair_00030000img_alert_0)
						{
							string layername = property_Hair_00030000img_alert_0_hair.Name;

							if (!layers_by_name.TryGetValue(layername, out var layer))
							{
								AppDebug.Log($"Unknown Hair::Layer name: [{layername}]\tLocation: [{node_Hair_00030000img.Name}][{stancename}][{frame}]");
							}


							if (stance == Stance.Id.WALK1)
							{
								var fds = 2;
							}


							Point_short brow = property_Hair_00030000img_alert_0_hair["map"]?["brow"];
							//var hairPosFromDrawInfo = drawinfo.gethairpos (stance, frame);

							//Point_short shift = hairPosFromDrawInfo - brow;
							Point_short shift = (drawinfo.gethairpos(stance, frame) ?? Point_short.zero) - brow;
							var texture = new Texture(property_Hair_00030000img_alert_0_hair, "Player", GameUtil.Instance.sortingLayerName_DefaultPlayer);
							texture.shift(shift);
							stances[(int)stance, (int)layer]?.Add(frame, texture);
						}

					}
				}

				/*for (byte frame = 0; WzObject framenode = node_Hair_00030000img_alert[frame]; ++frame)
				{
					foreach (nl  in :node layernode : framenode)
					{
						string layername = layernode.name();
						var layer_iter = layers_by_name.find(layername);

						if (layer_iter == layers_by_name.end())
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

						Point_short brow = layernode["map"]["brow"];
						Point_short shift = drawinfo.gethairpos(stance, frame) - brow;

						stances[(int)stance, (int)layer].Add(frame, layernode).first.second.shift(shift);
					}
				}*/
			}

			name = ms.wz.wzProvider_string["Eqp.img"]?["Eqp"]?["Hair"]?[Convert.ToString(hairid)]?["name"]?.ToString();

			const uint NUM_COLORS = 8;
			//string[] haircolors = {"Black", "Red", "Orange", "Blonde", "Green", "Blue", "Violet", "Brown"};
			string[] haircolors = { "黑", "红", "橙", "金", "绿", "蓝", "紫", "棕" };
			uint index = (uint)(hairid % 10);
			color = (index < NUM_COLORS) ? haircolors[index] : "";
		}


		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: void draw(Stance::Id stance, Layer layer, byte frame, const DrawArgument& args) const
		public void draw(Stance.Id stance, Layer layer, byte frame, DrawArgument args, bool drawOrErase = true)
		{
			Texture frameit = null;
			stances[(int)stance, (int)layer]?.TryGetValue(frame, out frameit);

			if (drawOrErase)
			{
				frameit?.draw(args);
			}
			else
			{
				frameit?.erase();
			}

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
		public string get_name()
		{
			return name;
		}

		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: const string& getcolor() const
		public string getcolor()
		{
			return color;
		}

		private Dictionary<byte, Texture>[,] stances = new Dictionary<byte, Texture>[EnumUtil.GetEnumLength<Stance.Id>(), EnumUtil.GetEnumLength<Layer>()];

		private void init_Dict()
		{
			for (int i = 0; i < EnumUtil.GetEnumLength<Stance.Id>(); i++)
			{
				for (int j = 0; j < EnumUtil.GetEnumLength<Layer>(); j++)
				{
					stances[i, j] = new Dictionary<byte, Texture>();
				}
			}
		}

		private string name;
		private string color;

		private readonly Dictionary<string, Layer> layers_by_name = new Dictionary<string, Layer>()
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

		public void Dispose()
		{
			foreach (var pair1 in stances)
			{
				foreach (var pair2 in pair1)
				{
					pair2.Value.Dispose();
				}
				//pair1.Clear ();
			}

			//stances = null;
		}
	}
}


#if USE_NX
#endif