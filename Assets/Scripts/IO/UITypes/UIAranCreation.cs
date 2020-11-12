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
	public class UIAranCreation : UIElement
	{
		public const Type TYPE = UIElement.Type.CLASSCREATION;
		public const bool FOCUSED = false;
		public const bool TOGGLED = false;

		public UIAranCreation() : base(new Point<short>(0, 0), new Point<short>(800, 600))
		{
			gender = false;
			charSet = false;
			named = false;

			string version_text = Configuration.get().get_version();
			version = new Text(Text.Font.A11B, Text.Alignment.LEFT, Color.Name.LEMONGRASS, "Ver. " + version_text);

			WzObject Login = nl.nx.wzFile_ui["Login.img"];
			WzObject Common = Login["Common"];
			WzObject CustomizeChar = Login["CustomizeChar"]["2000"];
			WzObject back = nl.nx.wzFile_map001["Back"]["login.img"]["back"];
			WzObject board = CustomizeChar["board"];
			WzObject genderSelect = CustomizeChar["genderSelect"];
			WzObject frame = nl.nx.wzFile_mapLatest["Obj"]["login.img"]["Common"]["frame"]["2"]["0"];

			sky = back["2"];
			cloud = back["27"];

			sprites.Add(new Sprite(back["33"], new Point<short>(256, 299)));
			sprites.Add(new Sprite (back["34"], new Point<short> (587, 157)));
			sprites_gender_select.Add(new Sprite (board["genderTop"], new Point<short> (491, 168)));
			sprites_gender_select.Add(new Sprite (board["boardMid"], new Point<short> (491, 220)));
			sprites_gender_select.Add(new Sprite (board["boardBottom"], new Point<short> (491, 313)));
			sprites_lookboard.Add(new Sprite (CustomizeChar["charSet"], new Point<short> (473, 103)));

			for (int i = 0; i <= 6; i++)
			{
				sprites_lookboard.Add(new Sprite (CustomizeChar["avatarSel"][i.ToString ()]["normal"], new Point<short> (504, (short)(187 + (i * 18)))));
			}

			buttons[(int)Buttons.BT_CHARC_GENDER_M] =new MapleButton(genderSelect["male"], new Point<short>(439, 106));
			buttons[(int)Buttons.BT_CHARC_GEMDER_F] =new MapleButton(genderSelect["female"], new Point<short>(437, 106));
			buttons[(int)Buttons.BT_CHARC_SKINL] = new MapleButton(CustomizeChar["BtLeft"], new Point<short>(562, 187 + (2 * 18)));
			buttons[(int)Buttons.BT_CHARC_SKINR] =new MapleButton(CustomizeChar["BtRight"], new Point<short>(699, 187 + (2 * 18)));
			buttons[(int)Buttons.BT_CHARC_OK] = new MapleButton(CustomizeChar["BtYes"], new Point<short>(520, 397));
			buttons[(int)Buttons.BT_CHARC_CANCEL] = new MapleButton(CustomizeChar["BtNo"], new Point<short>(594, 397));

			buttons[(int)Buttons.BT_CHARC_SKINL].set_active(false);
			buttons[(int)Buttons.BT_CHARC_SKINR].set_active(false);

			nameboard = CustomizeChar["charName"];
			namechar = new Textfield(Text.Font.A13M, Text.Alignment.LEFT, Color.Name.WHITE, new Rectangle<short>(new Point<short>(524, 196), new Point<short>(630, 253)), 12);

			sprites.Add(new Sprite (frame, new Point<short> (400, 300)));
			sprites.Add(new Sprite (Common["frame"], new Point<short> (400, 300)));
			sprites.Add(new Sprite (Common["step"]["3"], new Point<short> (40, 0)));

			buttons[(int)Buttons.BT_BACK] =new MapleButton(Login["Common"]["BtStart"], new Point<short>(0, 515));

			namechar.set_state(Textfield.State.DISABLED);

			namechar.set_enter_callback((string UnnamedParameter1) =>
			{
					button_pressed((ushort)Buttons.BT_CHARC_OK);
			});

			namechar.set_key_callback(KeyAction.Id.ESCAPE, () =>
			{
					button_pressed((ushort)Buttons.BT_CHARC_CANCEL);
			});

			facename = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.BLACK);
			hairname = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.BLACK);
			bodyname = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.BLACK);
			topname = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.BLACK);
			botname = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.BLACK);
			shoename = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.BLACK);
			wepname = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.BLACK);

			WzObject mkinfo = nl.nx.wzFile_etc["MakeCharInfo.img"]["Info"];

			for (uint i = 0; i < 2; i++)
			{
				bool f;
				WzObject CharGender;

				if (i == 0)
				{
					f = true;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: CharGender = mkinfo["CharFemale"];
					CharGender=(mkinfo["CharFemale"]);
				}
				else
				{
					f = false;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: CharGender = mkinfo["CharMale"];
					CharGender=(mkinfo["CharMale"]);
				}

				foreach (var node in CharGender)
				{
					int num = Convert.ToInt32(node.Name);

					foreach (var idnode in node)
					{
						int value = idnode;

						switch (num)
						{
							case 0:
								faces[f].Add(value);
								break;
							case 1:
								hairs[f].Add(value);
								break;
							case 2:
								haircolors[f].Add((byte)value);
								break;
							case 3:
								skins[f].Add((byte)value);
								break;
							case 4:
								tops[f].Add(value);
								break;
							case 5:
								bots[f].Add(value);
								break;
							case 6:
								shoes[f].Add(value);
								break;
							case 7:
								weapons[f].Add(value);
								break;
						}
					}
				}
			}

			female = false;
			randomize_look();

			newchar.set_direction(true);

			cloudfx = 200.0f;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float inter) const override
		public override void draw(float inter)
		{
			for (uint i = 0; i < 2; i++)
			{
				for (short k = 0; k < 800; k += sky.width())
				{
					sky.draw(new Point<short>(k, (short)((400 * i) - 100)));
				}
			}

			short cloudx = (short)((short)cloudfx % 800);
			cloud.draw(new Point<short>((short)(cloudx - cloud.width()), 310));
			cloud.draw(new Point<short>(cloudx, 310));
			cloud.draw(new Point<short>((short)(cloudx + cloud.width()), 310));

			if (!gender)
			{
				for (int i = 0; i < sprites_gender_select.Count; i++)
				{
					if (i == 1)
					{
						for (int f = 0; f <= 4; f++)
						{
							sprites_gender_select[i].draw(position + new Point<short>(0, (short)(24 * f)), inter);
						}
					}
					else
					{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: sprites_gender_select[i].draw(position, inter);
						sprites_gender_select[i].draw(position, inter);
					}
				}

				base.draw(inter);

				newchar.draw(new Point<short>(394, 339), inter);
			}
			else
			{
				if (!charSet)
				{
					base.draw_sprites(inter);

					foreach (var sprite in sprites_lookboard)
					{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: sprite.draw(position, inter);
						sprite.draw(position, inter);
					}

					facename.draw(new Point<short>(647, 183 + (0 * 18)));
					hairname.draw(new Point<short>(647, 183 + (1 * 18)));
					bodyname.draw(new Point<short>(647, 183 + (2 * 18)));
					topname.draw(new Point<short>(647, 183 + (3 * 18)));
					botname.draw(new Point<short>(647, 183 + (4 * 18)));
					shoename.draw(new Point<short>(647, 183 + (5 * 18)));
					wepname.draw(new Point<short>(647, 183 + (6 * 18)));

					newchar.draw(new Point<short>(394, 339), inter);

					base.draw_buttons(inter);
				}
				else
				{
					if (!named)
					{
						base.draw_sprites(inter);

						nameboard.draw(new Point<short>(489, 106));

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: namechar.draw(position);
						namechar.draw(position);
						newchar.draw(new Point<short>(394, 339), inter);

						base.draw_buttons(inter);
					}
					else
					{
						base.draw_sprites(inter);

						nameboard.draw(new Point<short>(489, 106));

						base.draw_buttons(inter);

						foreach (var sprite in sprites_keytype)
						{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: sprite.draw(position, inter);
							sprite.draw(position, inter);
						}
					}
				}
			}

			version.draw(position + new Point<short>(707, 4));
		}
		public override void update()
		{
			if (!gender)
			{
				foreach (var sprite in sprites_gender_select)
				{
					sprite.update();
				}

				newchar.update(Constants.TIMESTEP);
			}
			else
			{
				if (!charSet)
				{
					foreach (var sprite in sprites_lookboard)
					{
						sprite.update();
					}

					newchar.update(Constants.TIMESTEP);
				}
				else
				{
					if (!named)
					{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: namechar.update(position);
						namechar.update(position);
						newchar.update(Constants.TIMESTEP);
					}
					else
					{
						foreach (var sprite in sprites_keytype)
						{
							sprite.update();
						}

						namechar.set_state(Textfield.State.DISABLED);
					}
				}
			}

			base.update();

			cloudfx += 0.25f;
		}

		public override Cursor.State send_cursor(bool clicked, Point<short> cursorpos)
		{
			if (namechar.get_state() == Textfield.State.NORMAL)
			{
				if (namechar.get_bounds().contains(cursorpos))
				{
					if (clicked)
					{
						namechar.set_state(Textfield.State.FOCUSED);

						return Cursor.State.CLICKING;
					}
					else
					{
						return Cursor.State.IDLE;
					}
				}
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return UIElement::send_cursor(clicked, cursorpos);
			return base.send_cursor(clicked, cursorpos);
		}
		public override void send_key(int keycode, bool pressed, bool escape)
		{
			if (pressed)
			{
				if (escape)
				{
					button_pressed((ushort)Buttons.BT_CHARC_CANCEL);
				}
				else if (keycode == (int)KeyAction.Id.RETURN)
				{
					button_pressed((ushort)Buttons.BT_CHARC_OK);
				}
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type()
		{
			return TYPE;
		}

		public void send_naming_result(bool nameused)
		{
			if (!named)
			{
				if (!nameused)
				{
					named = true;

					string cname = namechar.get_text();
					int cface = faces[female][face];
					int chair = hairs[female][hair];
					byte chairc = haircolors[female][haircolor];
					byte cskin = skins[female][skin];
					int ctop = tops[female][top];
					int cbot = bots[female][bot];
					int cshoe = shoes[female][shoe];
					int cwep = weapons[female][weapon];

					new CreateCharPacket(cname, 2, cface, chair, chairc, cskin, ctop, cbot, cshoe, cwep, female).dispatch();

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Lambda expressions cannot be assigned to 'var':
					Action<bool> onok = (bool alternate) =>
					{
						new Sound(Sound.Name.SCROLLUP).play();

						UI.get().remove(UIElement.Type.LOGINNOTICE_CONFIRM);
						UI.get().remove(UIElement.Type.LOGINNOTICE);
						UI.get().remove(UIElement.Type.CLASSCREATION);
						UI.get().remove(UIElement.Type.RACESELECT);
						var charselect = UI.get ().get_element<UICharSelect> ();
						if (charselect)
						{
							charselect.get ().post_add_character();
						}
					};

					UI.get().emplace<UIKeySelect>(onok, true);
				}
				else
				{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Lambda expressions cannot be assigned to 'var':
					Action onok = () =>
					{
						namechar.set_state(Textfield.State.FOCUSED);

						buttons[(int)Buttons.BT_CHARC_OK].set_state(Button.State.NORMAL);
						buttons[(int)Buttons.BT_CHARC_CANCEL].set_state(Button.State.NORMAL);
					};

					UI.get().emplace<UILoginNotice>(UILoginNotice.Message.NAME_IN_USE, onok);
				}
			}
		}

		public override Button.State button_pressed(ushort buttonid)
		{
			switch ((Buttons)buttonid)
			{
				case Buttons.BT_CHARC_OK:
				{
					if (!gender)
					{
						gender = true;

						buttons[(int)Buttons.BT_CHARC_GENDER_M].set_active(false);
						buttons[(int)Buttons.BT_CHARC_GEMDER_F].set_active(false);

						buttons[(int)Buttons.BT_CHARC_SKINL].set_active(true);
						buttons[(int)Buttons.BT_CHARC_SKINR].set_active(true);

						buttons[(int)Buttons.BT_CHARC_OK].set_position(new Point<short>(533, 368));
						buttons[(int)Buttons.BT_CHARC_CANCEL].set_position(new Point<short>(607, 368));

						return Button.State.NORMAL;
					}
					else
					{
						if (!charSet)
						{
							charSet = true;

							buttons[(int)Buttons.BT_CHARC_SKINL].set_active(false);
							buttons[(int)Buttons.BT_CHARC_SKINR].set_active(false);

							buttons[(int)Buttons.BT_CHARC_OK].set_position(new Point<short>(523, 243));
							buttons[(int)Buttons.BT_CHARC_CANCEL].set_position(new Point<short>(597, 243));

							namechar.set_state(Textfield.State.FOCUSED);

							return Button.State.NORMAL;
						}
						else
						{
							if (!named)
							{
								string name = namechar.get_text();

								if (name.Length <= 0)
								{
									return Button.State.NORMAL;
								}
								else if (name.Length >= 4)
								{
									namechar.set_state(Textfield.State.DISABLED);

									buttons[(int)Buttons.BT_CHARC_OK].set_state(Button.State.DISABLED);
									buttons[(int)Buttons.BT_CHARC_CANCEL].set_state(Button.State.DISABLED);
									var raceselect = UI.get ().get_element<UIRaceSelect> ();
									if (raceselect)
									{
										if (raceselect.get ().check_name(name))
										{
											new NameCharPacket(name).dispatch();

											return Button.State.IDENTITY;
										}
									}

									System.Action okhandler = () =>
									{
										namechar.set_state(Textfield.State.FOCUSED);

										buttons[(int)Buttons.BT_CHARC_OK].set_state(Button.State.NORMAL);
										buttons[(int)Buttons.BT_CHARC_CANCEL].set_state(Button.State.NORMAL);
									};

									UI.get().emplace<UILoginNotice>(UILoginNotice.Message.ILLEGAL_NAME, okhandler);

									return Button.State.NORMAL;
								}
								else
								{
									namechar.set_state(Textfield.State.DISABLED);

									buttons[(int)Buttons.BT_CHARC_OK].set_state(Button.State.DISABLED);
									buttons[(int)Buttons.BT_CHARC_CANCEL].set_state(Button.State.DISABLED);

									System.Action okhandler = () =>
									{
										namechar.set_state(Textfield.State.FOCUSED);

										buttons[(int)Buttons.BT_CHARC_OK].set_state(Button.State.NORMAL);
										buttons[(int)Buttons.BT_CHARC_CANCEL].set_state(Button.State.NORMAL);
									};

									UI.get().emplace<UILoginNotice>(UILoginNotice.Message.ILLEGAL_NAME, okhandler);

									return Button.State.IDENTITY;
								}
							}
							else
							{
								return Button.State.NORMAL;
							}
						}
					}
				}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# does not allow fall-through from a non-empty 'case':
				case Buttons.BT_BACK:
				{
					new Sound(Sound.Name.SCROLLUP).play();

					UI.get().remove(UIElement.Type.CLASSCREATION);
					UI.get().emplace<UIRaceSelect>();

					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_CANCEL:
				{
					if (charSet)
					{
						charSet = false;

						buttons[(int)Buttons.BT_CHARC_SKINL].set_active(true);
						buttons[(int)Buttons.BT_CHARC_SKINR].set_active(true);

						buttons[(int)Buttons.BT_CHARC_OK].set_position(new Point<short>(533, 368));
						buttons[(int)Buttons.BT_CHARC_CANCEL].set_position(new Point<short>(607, 368));

						namechar.set_state(Textfield.State.DISABLED);

						return Button.State.NORMAL;
					}
					else
					{
						if (gender)
						{
							gender = false;

							buttons[(int)Buttons.BT_CHARC_GENDER_M].set_active(true);
							buttons[(int)Buttons.BT_CHARC_GEMDER_F].set_active(true);

							buttons[(int)Buttons.BT_CHARC_SKINL].set_active(false);
							buttons[(int)Buttons.BT_CHARC_SKINR].set_active(false);

							buttons[(int)Buttons.BT_CHARC_OK].set_position(new Point<short>(520, 397));
							buttons[(int)Buttons.BT_CHARC_CANCEL].set_position(new Point<short>(594, 397));

							return Button.State.NORMAL;
						}
						else
						{
							button_pressed((ushort)Buttons.BT_BACK);

							return Button.State.NORMAL;
						}
					}
				}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# does not allow fall-through from a non-empty 'case':
				case Buttons.BT_CHARC_SKINL:
				{
					skin = (skin > 0) ? skin - 1 : (skins[female].Count - 1);
					newchar.set_body(skins[female][skin]);
					bodyname.change_text(newchar.get_body().get_name());

					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_SKINR:
				{
					skin = (skin < skins[female].Count - 1) ? skin + 1 : 0;
					newchar.set_body(skins[female][skin]);
					bodyname.change_text(newchar.get_body().get_name());

					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_GENDER_M:
				{
					if (female)
					{
						female = false;
						randomize_look();
					}

					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_GEMDER_F:
				{
					if (!female)
					{
						female = true;
						randomize_look();
					}

					return Button.State.NORMAL;
				}
			}

			return Button.State.PRESSED;
		}

		private void randomize_look()
		{
			hair = 0;
			face = 0;
			skin = randomizer.next_int(skins[female].Count);
			haircolor = 0;
			top = 0;
			bot = 0;
			shoe = 0;
			weapon = 0;

			newchar.set_body(skins[female][skin]);
			newchar.set_face(faces[female][face]);
			newchar.set_hair(hairs[female][hair] + haircolors[female][haircolor]);
			newchar.add_equip(tops[female][top]);
			newchar.add_equip(bots[female][bot]);
			newchar.add_equip(shoes[female][shoe]);
			newchar.add_equip(weapons[female][weapon]);

			bodyname.change_text(newchar.get_body().get_name());
			facename.change_text(newchar.get_face().get_name());
			hairname.change_text(newchar.get_hair().get_name());
			topname.change_text(get_equipname(EquipSlot.Id.TOP));
			botname.change_text(get_equipname(EquipSlot.Id.BOTTOM));
			shoename.change_text(get_equipname(EquipSlot.Id.SHOES));
			wepname.change_text(get_equipname(EquipSlot.Id.WEAPON));
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string& get_equipname(EquipSlot::Id slot) const
		private string get_equipname(EquipSlot.Id slot)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
			int item_id = newchar.get_equips ().get_equip (slot);
			if (item_id!=0)
			{
				return ItemData.get(item_id).get_name();
			}
			else
			{
				const string nullstr = "Missing name.";

				return nullstr;
			}
		}

		private enum Buttons : ushort
		{
			BT_BACK,
			BT_CHARC_OK,
			BT_CHARC_CANCEL,
			BT_CHARC_SKINL,
			BT_CHARC_SKINR,
			BT_CHARC_GENDER_M,
			BT_CHARC_GEMDER_F
		}

		private enum GenderButtons : byte
		{
			GENDER_BACKGROUND,
			GENDER_HEAD,
			GENDER_TOP,
			GENDER_MID,
			GENDER_BOTTOM
		}

		private List<Sprite> sprites_lookboard = new List<Sprite>();
		private List<Sprite> sprites_gender_select = new List<Sprite>();
		private List<Sprite> sprites_keytype = new List<Sprite>();
		private Texture sky = new Texture();
		private Texture cloud = new Texture();
		private float cloudfx;
		private Texture nameboard = new Texture();
		private Textfield namechar = new Textfield();
		private CharLook newchar = new CharLook();
		private Randomizer randomizer = new Randomizer();

		private BoolPair<List<byte>> skins = new BoolPair<List<byte>>();
		private BoolPair<List<byte>> haircolors = new BoolPair<List<byte>>();
		private BoolPair<List<int>> faces = new BoolPair<List<int>>();
		private BoolPair<List<int>> hairs = new BoolPair<List<int>>();
		private BoolPair<List<int>> tops = new BoolPair<List<int>>();
		private BoolPair<List<int>> bots = new BoolPair<List<int>>();
		private BoolPair<List<int>> shoes = new BoolPair<List<int>>();
		private BoolPair<List<int>> weapons = new BoolPair<List<int>>();

		private bool gender;
		private bool charSet;
		private bool named;
		private bool female;
		private int skin;
		private int haircolor;
		private int face;
		private int hair;
		private int top;
		private int bot;
		private int shoe;
		private int weapon;
		private Text facename = new Text();
		private Text hairname = new Text();
		private Text bodyname = new Text();
		private Text topname = new Text();
		private Text botname = new Text();
		private Text shoename = new Text();
		private Text wepname = new Text();
		private Text version = new Text();
	}
}







#if USE_NX
#endif
