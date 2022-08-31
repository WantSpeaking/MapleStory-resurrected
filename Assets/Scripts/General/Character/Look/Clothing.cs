#define USE_NX

using System;
using System.Collections.Generic;
using System.Linq;
using ms.Helper;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;





namespace ms
{
	public class Clothing
	{
		public enum Layer
		{
			CAPE,
			SHOES,
			PANTS,
			TOP,
			MAIL,
			MAILARM,
			EARRINGS,
			FACEACC,
			EYEACC,
			PENDANT,
			BELT,
			MEDAL,
			RING,
			CAP,
			CAP_BELOW_BODY,
			CAP_OVER_HAIR,
			GLOVE,
			WRIST,
			GLOVE_OVER_HAIR,
			WRIST_OVER_HAIR,
			GLOVE_OVER_BODY,
			WRIST_OVER_BODY,
			SHIELD,
			BACKSHIELD,
			SHIELD_BELOW_BODY,
			SHIELD_OVER_HAIR,
			WEAPON,
			BACKWEAPON,
			WEAPON_BELOW_ARM,
			WEAPON_BELOW_BODY,
			WEAPON_OVER_HAND,
			WEAPON_OVER_BODY,
			WEAPON_OVER_GLOVE,
			//WEAPON_OVER_ARM,
			mailArmOverHair,
			NUM_LAYERS
		}

		private readonly Clothing.Layer[] layers = new[] {Clothing.Layer.CAP, Clothing.Layer.FACEACC, Clothing.Layer.EYEACC, Clothing.Layer.EARRINGS, Clothing.Layer.TOP, Clothing.Layer.MAIL, Clothing.Layer.PANTS, Clothing.Layer.SHOES, Clothing.Layer.GLOVE, Clothing.Layer.SHIELD, Clothing.Layer.CAPE, Clothing.Layer.RING, Clothing.Layer.PENDANT, Clothing.Layer.BELT, Clothing.Layer.MEDAL};

		// Construct a new equip
		public Clothing (int id, BodyDrawInfo drawinfo)
		{
			this.itemid = id;
			EquipData equipdata = EquipData.get (itemid);

			eqslot = equipdata.get_eqslot ();

			if (eqslot == EquipSlot.Id.WEAPON)
			{
				twohanded = WeaponData.get (itemid).is_twohanded ();
			}
			else
			{
				twohanded = false;
			}

			const uint NON_WEAPON_TYPES = 15;
			const uint WEAPON_OFFSET = NON_WEAPON_TYPES + 15;
			const uint WEAPON_TYPES = 20;


			Clothing.Layer chlayer;
			uint index = (uint)((itemid / 10000) - 100);
			string indexTest = "";
			if (index < NON_WEAPON_TYPES)
			{
				chlayer = layers[index];
				indexTest = "NON_WEAPON_TYPES";
			}
			else if (index >= WEAPON_OFFSET && index < WEAPON_OFFSET + WEAPON_TYPES)
			{
				chlayer = Clothing.Layer.WEAPON;
				indexTest = "WEAPON";
			}
			else
			{
				chlayer = Clothing.Layer.CAPE;
				indexTest = "CAPE";
			}

			string strid = "0" + Convert.ToString (itemid);
			string category = equipdata.get_itemdata ().get_category ();
			//AppDebug.Log ($"strid:{strid}\t category:{category}\t eqslot:{eqslot}\t chlayer:{chlayer}");
			if (category == null) return;

			var src = ms.wz.wzFile_character[category]?[strid + ".img"];
			if (src == null) return; //todo 2 why cloth src is it null
			var info = src["info"];

			vslot = info["vslot"].ToString ();

			var temp_stand_value = (int)info["stand"];
			switch (temp_stand_value)
			{
				case 1:
					stand = Stance.Id.STAND1;
					break;
				case 2:
					stand = Stance.Id.STAND2;
					break;
				default:
					stand = twohanded ? Stance.Id.STAND2 : Stance.Id.STAND1;
					break;
			}

			int temp_walk_value = info["walk"];
			switch (temp_walk_value)
			{
				case 1:
					walk = Stance.Id.WALK1;
					break;
				case 2:
					walk = Stance.Id.WALK2;
					break;
				default:
					walk = twohanded ? Stance.Id.WALK2 : Stance.Id.WALK1;
					break;
			}

			foreach (var iter in Stance.names)
			{
				Stance.Id stance = iter.Key;
				string stancename = iter.Value;

				var node_00002000img_walk1 = src[stancename];

				if (node_00002000img_walk1 is WzImageProperty property_00002000img_walk1)
				{
					if (stance.ToString ().Contains ("STABTF")&& itemid == 1052202)
					{
						var fdsfg = 0;
					}
					foreach (var property_00002000img_walk1_0 in property_00002000img_walk1.WzProperties)
					{
						byte.TryParse (property_00002000img_walk1_0.Name, out byte frame);

						foreach (var property_00002000img_walk1_0_arm in property_00002000img_walk1_0.WzProperties)
						{
							string part = property_00002000img_walk1_0_arm.Name;

							if (!property_00002000img_walk1_0_arm.IsTexture())
							{
								continue;
							}

							Clothing.Layer z = chlayer;
							string zs = property_00002000img_walk1_0_arm["z"].ToString ();

							if (part == "mailArm")
							{
								z = Clothing.Layer.MAILARM;
							}
							else
							{
								if (sublayernames.TryGetValue (zs, out var layer))
								{
									z = layer;
								}
							}

							string parent = string.Empty;
							Point_short parentpos = new Point_short ();

							if (property_00002000img_walk1_0_arm["map"] is WzImageProperty property_property_00002000img_walk1_0_arm_map)
							{
								foreach (var property_property_00002000img_walk1_0_arm_map_hand in property_property_00002000img_walk1_0_arm_map.WzProperties)
								{
									if (property_property_00002000img_walk1_0_arm_map_hand.PropertyType == WzPropertyType.Vector)
									{
										parent = property_property_00002000img_walk1_0_arm_map_hand.Name;
										parentpos = property_property_00002000img_walk1_0_arm_map_hand.GetPoint ().ToMSPoint ();
									}
								}
							}

							var mapnode = property_00002000img_walk1_0_arm["map"];
							Point_short shift = new Point_short ();

							switch (eqslot)
							{
								case EquipSlot.Id.FACE:
									shift -= parentpos;
									break;
								case EquipSlot.Id.SHOES:
								case EquipSlot.Id.GLOVES:
								case EquipSlot.Id.TOP:
								case EquipSlot.Id.BOTTOM:
								case EquipSlot.Id.CAPE:
									shift = drawinfo.get_body_position (stance, frame) - parentpos;
									break;
								case EquipSlot.Id.HAT:
								case EquipSlot.Id.EARACC:
								case EquipSlot.Id.EYEACC:
									shift = drawinfo.getfacepos (stance, frame) - parentpos;
									break;
								case EquipSlot.Id.SHIELD:
								case EquipSlot.Id.WEAPON:
									if (parent == "handMove")
									{
										shift += drawinfo.get_hand_position (stance, frame);
									}
									else if (parent == "hand")
									{
										shift += drawinfo.get_arm_position (stance, frame);
									}
									else if (parent == "navel")
									{
										shift += drawinfo.get_body_position (stance, frame);
									}

									shift -= parentpos;
									break;
							}

							var tempTexture = new Texture (property_00002000img_walk1_0_arm, "Player");
							tempTexture.shift (shift);
							//if (stance.ToString ().Contains ("STAND"))
							{
								//AppDebug.Log ($"stances.Add: stance:{stance}\t z:{z}\t frame:{frame} \t tempTexture:{tempTexture.fullPath}\t itemid:{itemid}\t index:{index}\t indexTest:{indexTest}\t eqslot:{eqslot}\t chlayer:{chlayer}\t zs:{zs}\t category:{category}");
							}

							stances[stance][z].Add (frame, tempTexture);
						}
					}
				}
			}


			transparent = transparents.Any (x => x == itemid);
		}

		private readonly HashSet<int> transparents = new HashSet<int> {1002186};

		// Draw the equip
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Stance.Id stance, Layer layer, byte frame, const DrawArgument& args) const
		public void draw (Stance.Id stance, Layer layer, byte frame, DrawArgument args, bool drawOrErase = true)
		{
		
			var range = stances[stance][layer].Where (pair => pair.Key == frame).Select (pair => pair.Value);

			foreach (var texture in range)
			{
				if (drawOrErase)
				{
					texture.draw (args);
				}
				else
				{
					texture.erase ();
				}
			}

			/*for (var iter = range.first; iter != range.second; ++iter)
			{
				iter.second.draw (args);
			}*/
		}

		// Check if a part of the equip lies on the specified layer while in the specified stance
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool contains_layer(Stance.Id stance, Layer layer) const
		public bool contains_layer (Stance.Id stance, Layer layer)
		{
			return stances[stance][layer].Count != 0;
		}

		// Return whether the equip is invisible
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_transparent() const
		public bool is_transparent ()
		{
			return transparent;
		}

		// Return whether this equip uses twohanded stances
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_twohanded() const
		public bool is_twohanded ()
		{
			return twohanded;
		}

		// Return the item id
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_id() const
		public int get_id ()
		{
			return itemid;
		}

		// Return the equip slot for this cloth
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: EquipSlot.Id get_eqslot() const
		public EquipSlot.Id get_eqslot ()
		{
			return eqslot;
		}

		// Return the standing stance to use while equipped
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Stance.Id get_stand() const
		public Stance.Id get_stand ()
		{
			return stand;
		}

		// Return the walking stance to use while equipped
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Stance.Id get_walk() const
		public Stance.Id get_walk ()
		{
			return walk;
		}

		// Return the vslot, used to distinguish some layering types.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string& get_vslot() const
		public string get_vslot ()
		{
			return vslot;
		}

		private EnumMapNew<Stance.Id, EnumMapNew<Layer, Dictionary<byte, Texture>>> stances = new EnumMapNew<Stance.Id, EnumMapNew<Layer, Dictionary<byte, Texture>>> ();
		private int itemid;
		private EquipSlot.Id eqslot;
		private Stance.Id walk;
		private Stance.Id stand;
		private string vslot;
		private bool twohanded;
		private bool transparent;

		private readonly Dictionary<string, Layer> sublayernames = new Dictionary<string, Layer> ()
		{
			// WEAPON
			{"weaponOverHand", Clothing.Layer.WEAPON_OVER_HAND},
			{"weaponOverGlove", Clothing.Layer.WEAPON_OVER_GLOVE},
			{"weaponOverBody", Clothing.Layer.WEAPON_OVER_BODY},
			//{"weaponOverArm", Clothing.Layer.WEAPON_OVER_ARM},
			{"weaponBelowArm", Clothing.Layer.WEAPON_BELOW_ARM},
			{"weaponBelowBody", Clothing.Layer.WEAPON_BELOW_BODY},
			{"backWeaponOverShield", Clothing.Layer.BACKWEAPON},
			// SHIELD
			{"shieldOverHair", Clothing.Layer.SHIELD_OVER_HAIR},
			{"shieldBelowBody", Clothing.Layer.SHIELD_BELOW_BODY},
			{"backShield", Clothing.Layer.BACKSHIELD},
			// GLOVE
			{"gloveWrist", Clothing.Layer.WRIST},
			{"gloveOverHair", Clothing.Layer.GLOVE_OVER_HAIR},
			{"gloveOverBody", Clothing.Layer.GLOVE_OVER_BODY},
			{"gloveWristOverHair", Clothing.Layer.WRIST_OVER_HAIR},
			{"gloveWristOverBody", Clothing.Layer.WRIST_OVER_BODY},
			// CAP
			{"capOverHair", Clothing.Layer.CAP_OVER_HAIR},
			{"capBelowBody", Clothing.Layer.CAP_BELOW_BODY},
			//mail
			{"mailArmOverHair", Clothing.Layer.mailArmOverHair},
			
		};
	}
}


#if USE_NX
#endif