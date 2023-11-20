using System;
using System.Collections.Generic;
using Beebyte.Obfuscator;
using MapleLib.WzLib;
using ms_Unity;
using ms.Util;
using provider;

namespace ms
{
	[Skip]
	public class UIExplorerCreation : UIElement
	{
		public const Type TYPE = UIElement.Type.CLASSCREATION;
		public const bool FOCUSED = false;
		public const bool TOGGLED = false;
		public static int SelectedJobId = 112;

		public UIExplorerCreation() : base(new Point_short(0, 0), new Point_short(800, 600))
		{
			gender = true;
			charSet = false;
			named = false;

			string version_text = Configuration.get().get_version();
			version = new Text(Text.Font.A11B, Text.Alignment.LEFT, Color.Name.LEMONGRASS, "Ver. " + version_text);

			var Login = ms.wz.wzProvider_ui["Login.img"];
			var Common = Login["Common"];
			//WzObject CustomizeChar = Login["CustomizeChar"]["000"];
			var NewChar = Login["NewChar"];

			var back = ms.wz.wzProvider_map["Back/login.img"]["back"];
			//WzObject signboard = ms.wz.wzFile_map["Obj"]["login.img"]["NewChar"]["signboard"];
			//WzObject board = NewChar["board"];
			//WzObject genderSelect = NewChar["genderSelect"];
			var frame = ms.wz.wzProvider_map["Obj/login.img"]["Common"]["frame"]["0"]["0"];

			sky = back["2"];
			cloud = back["27"];

			sprites.Add(new Sprite (back["14"], new Point_short (250, 302)));
			//sprites.Add(new Sprite (signboard["2"], new DrawArgument(new Point_short(234, 235), 2.0f)));
			/*sprites_gender_select.Add(new Sprite (board["genderTop"], new Point_short(486, 95)));
			sprites_gender_select.Add(new Sprite (board["boardMid"], new Point_short(486, 209)));
			sprites_gender_select.Add(new Sprite (board["boardBottom"], new Point_short(486, 329)));*/
			sprites_lookboard.Add(new Sprite (NewChar["charSet"], new Point_short(486, 95)));

			/*for (int i = 0; i <= 5; i++)
			{
				int f = i;

				if (i >= 2)
				{
					f++;
				}

				int tmp = i;
				switch (i)
				{
                    case 3:
                        tmp = 2;
                        break;
                    case 4:
                        tmp = 3;
                        break;
                    case 5:
						tmp = 4;
                        break;
				}
				sprites_lookboard.Add(new Sprite (NewChar["avatarSel"][tmp.ToString()]["normal"], new Point_short(497, (short)(197 + (f * 18)))));
			}*/
			for (int i = 0; i <= 8; i++)
			{
				sprites_lookboard.Add(new Sprite (NewChar["avatarSel"][i.ToString()]["normal"], new Point_short(497, (short)(197 + (i * 18)))));
				
				/*int f = i;

				if (i >= 2)
				{
					f++;
				}

				int tmp = i;
				switch (i)
				{
					case 3:
						tmp = 2;
						break;
					case 4:
						tmp = 3;
						break;
					case 5:
						tmp = 4;
						break;
				}
				sprites_lookboard.Add(new Sprite (NewChar["avatarSel"][tmp.ToString()]["normal"], new Point_short(497, (short)(197 + (f * 18)))));*/
			}
			
			
			buttons[(int)Buttons.BT_CHARC_FACEL] = new MapleButton(NewChar["BtLeft"], new Point_short(552, 198 + (0 * 18)));
			buttons[(int)Buttons.BT_CHARC_FACER] = new MapleButton(NewChar["BtRight"], new Point_short(684, 198 + (0 * 18)));
			buttons[(int)Buttons.BT_CHARC_HAIRL] = new MapleButton(NewChar["BtLeft"], new Point_short(552, 198 + (1 * 18)));
			buttons[(int)Buttons.BT_CHARC_HAIRR] = new MapleButton(NewChar["BtRight"], new Point_short(684, 198 + (1 * 18)));
			buttons[(int)Buttons.BT_CHARC_HAIRC0] = new MapleButton(NewChar["BtLeft"], new Point_short(552, 198 + (2 * 18)));
			buttons[(int)Buttons.BT_CHARC_HAIRC1] = new MapleButton(NewChar["BtRight"], new Point_short(684, 198 + (2 * 18)));
			buttons[(int)Buttons.BT_CHARC_SKINL] = new MapleButton(NewChar["BtLeft"], new Point_short(552, 198 + (3 * 18)));
			buttons[(int)Buttons.BT_CHARC_SKINR] = new MapleButton(NewChar["BtRight"], new Point_short(684, 198 + (3 * 18)));
			buttons[(int)Buttons.BT_CHARC_TOPL] = new MapleButton(NewChar["BtLeft"], new Point_short(552, 198 + (4 * 18)));
			buttons[(int)Buttons.BT_CHARC_TOPR] = new MapleButton(NewChar["BtRight"], new Point_short(684, 198 + (4 * 18)));
			buttons[(int)Buttons.BT_CHARC_BOTL] = new MapleButton(NewChar["BtLeft"], new Point_short(552, 198 + (5 * 18)));
			buttons[(int)Buttons.BT_CHARC_BOTR] = new MapleButton(NewChar["BtRight"], new Point_short(684, 198 + (5 * 18)));
			buttons[(int)Buttons.BT_CHARC_SHOESL] = new MapleButton(NewChar["BtLeft"], new Point_short(552, 198 + (6 * 18)));
			buttons[(int)Buttons.BT_CHARC_SHOESR] = new MapleButton(NewChar["BtRight"], new Point_short(684, 198 + (6 * 18)));
			buttons[(int)Buttons.BT_CHARC_WEPL] = new MapleButton(NewChar["BtLeft"], new Point_short(552, 198 + (7 * 18)));
			buttons[(int)Buttons.BT_CHARC_WEPR] = new MapleButton(NewChar["BtRight"], new Point_short(684, 198 + (7 * 18)));
			buttons[(int)Buttons.BT_CHARC_GENDER_M] = new MapleButton(NewChar["BtLeft"], new Point_short(552, 198 + (8 * 18)));
			buttons[(int)Buttons.BT_CHARC_GEMDER_F] = new MapleButton(NewChar["BtRight"], new Point_short(684, 198 + (8 * 18)));
			
			


			/*for (uint i = 0; i <= 7; i++)
			{
				buttons[(int)Buttons.BT_CHARC_HAIRC0 + i] = new MapleButton(NewChar["hairSelect"][i.ToString ()], new Point_short((short)(549 + (i * 15)), 234));
				buttons[(int)Buttons.BT_CHARC_HAIRC0 + i].set_active(false);
			}*/

			/*buttons[(int)Buttons.BT_CHARC_FACEL].set_active(false);
			buttons[(int)Buttons.BT_CHARC_FACER].set_active(false);
			buttons[(int)Buttons.BT_CHARC_HAIRL].set_active(false);
			buttons[(int)Buttons.BT_CHARC_HAIRR].set_active(false);
			buttons[(int)Buttons.BT_CHARC_SKINL].set_active(false);
			buttons[(int)Buttons.BT_CHARC_SKINR].set_active(false);
			buttons[(int)Buttons.BT_CHARC_TOPL].set_active(false);
			buttons[(int)Buttons.BT_CHARC_TOPR].set_active(false);
			buttons[(int)Buttons.BT_CHARC_SHOESL].set_active(false);
			buttons[(int)Buttons.BT_CHARC_SHOESR].set_active(false);
			buttons[(int)Buttons.BT_CHARC_WEPL].set_active(false);
			buttons[(int)Buttons.BT_CHARC_WEPR].set_active(false);
			
			buttons[(int)Buttons.BT_CHARC_HAIRC0].set_active(false);
			buttons[(int)Buttons.BT_CHARC_HAIRC1].set_active(false);
			buttons[(int)Buttons.BT_CHARC_GENDER_M].set_active(false);
			buttons[(int)Buttons.BT_CHARC_GEMDER_F].set_active(false);*/
			
			buttons[(int)Buttons.BT_CHARC_OK] = new MapleButton(NewChar["BtYes"], new Point_short(514, 427));
			buttons[(int)Buttons.BT_CHARC_CANCEL] = new MapleButton(NewChar["BtNo"], new Point_short(590, 427));

			nameboard = NewChar["charName"];
			namechar = new Textfield(Text.Font.A13M, Text.Alignment.LEFT, Color.Name.WHITE, new Rectangle_short(new Point_short(522, 195), new Point_short(630, 253)), 12);

			sprites.Add(new Sprite ( frame, new Point_short(400, 300)));
			sprites.Add(new Sprite ( Common["frame"], new Point_short(400, 300)));
			sprites.Add(new Sprite ( Common["step"]["3"], new Point_short(40, 0)));

			buttons[(int)Buttons.BT_BACK] = new MapleButton(Login["Common"]["BtStart"], new Point_short(0, 515));

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
			hairColorName = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.BLACK);
			bodyname = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.BLACK);
			topname = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.BLACK);
			botname = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.BLACK);
			shoename = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.BLACK);
			wepname = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.BLACK);
			gendername = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.BLACK);

			var mkinfo = ms.wz.wzProvider_etc["MakeCharInfo.img"]["Info"];

			for (uint i = 0; i < 2; i++)
			{
				bool f;
				MapleData CharGender;

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

		public override void draw(float inter)
		{
			for (uint i = 0; i < 2; i++)
			{
				for (short k = 0; k < 800; k += sky.width())
				{
					sky.draw(new Point_short(k, (short)((400 * i) - 100)));
				}
			}

			short cloudx = (short)((short)cloudfx % 800);
			cloud.draw(new Point_short((short)(cloudx - cloud.width()), 310));
			cloud.draw(new Point_short(cloudx, 310));
			cloud.draw(new Point_short((short)(cloudx + cloud.width()), 310));

			if (!gender)
			{
				for (int i = 0; i < sprites_gender_select.Count; i++)
				{
					if (i == 1)
					{
						for (short f = 0; f <= 4; f++)
						{
							sprites_gender_select[i].draw(position + new Point_short(0, (short)(24 * f)), inter);
						}
					}
					else
					{
						sprites_gender_select[i].draw(new Point_short (position), inter);
					}
				}

				base.draw(inter);

				newchar.draw(new Point_short(394, 339), inter);
			}
			else
			{
				if (!charSet)
				{
					base.draw_sprites(inter);

					foreach (var sprite in sprites_lookboard)
					{
						sprite.draw(new Point_short (position), inter);
					}

					facename.draw(new Point_short(640, 193 + (0 * 18)));
					hairname.draw(new Point_short(640, 193 + (1 * 18)));
					hairColorName.draw(new Point_short(640, 193 + (2 * 18)));
					bodyname.draw(new Point_short(640, 193 + (3 * 18)));
					topname.draw(new Point_short(640, 193 + (4 * 18)));
					botname.draw(new Point_short(640, 193 + (5 * 18)));
					shoename.draw(new Point_short(640, 193 + (6 * 18)));
					wepname.draw(new Point_short(640, 193 + (7 * 18)));
					gendername.draw(new Point_short(640, 193 + (8 * 18)));
					
					newchar.draw(new Point_short(394, 339), inter);

					base.draw_buttons(inter);
				}
				else
				{
					if (!named)
					{
						base.draw_sprites(inter);

						nameboard.draw(new Point_short(486, 95));
						namechar.draw(new Point_short (position));
						newchar.draw(new Point_short(394, 339), inter);

						base.draw_buttons(inter);
					}
					else
					{
						base.draw_sprites(inter);

						nameboard.draw(new Point_short(486, 95));

						base.draw_buttons(inter);

						foreach (var sprite in sprites_keytype)
						{
							sprite.draw(new Point_short (position), inter);
						}
					}
				}
			}

			version.draw(position + new Point_short(707, 4));
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
						namechar.update(new Point_short (position));
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

		public override Cursor.State send_cursor(bool clicked, Point_short cursorpos)
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

			return base.send_cursor(clicked, new Point_short (cursorpos));
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

					string cname = text_CharName ?? namechar.get_text();
					int cface = faces[female][(int)face];
					int chair = hairs[female][(int)hair];
					byte chairc = haircolors[female][(int)haircolor];
					byte cskin = skins[female][(int)skin];
					int ctop = tops[female][(int)top];
					int cbot = bots[female][(int)bot];
					int cshoe = shoes[female][(int)shoe];
					int cwep = weapons[female][(int)weapon];

					new CreateCharPacket(cname, 1, cface, chair, chairc, cskin, ctop, cbot, cshoe, cwep, female, SelectedJobId).dispatch();
				}
				else
				{
					Action onok = () =>
					{
						namechar.set_state(Textfield.State.FOCUSED);

						buttons[(int)Buttons.BT_CHARC_OK].set_state(Button.State.NORMAL);
						buttons[(int)Buttons.BT_CHARC_CANCEL].set_state(Button.State.NORMAL);
					};

					UI.get().emplace<UILoginNotice>(UILoginNotice.Message.NAME_IN_USE, onok, null);
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

						buttons[(int)Buttons.BT_CHARC_FACEL].set_active(true);
						buttons[(int)Buttons.BT_CHARC_FACER].set_active(true);
						buttons[(int)Buttons.BT_CHARC_HAIRL].set_active(true);
						buttons[(int)Buttons.BT_CHARC_HAIRR].set_active(true);
						buttons[(int)Buttons.BT_CHARC_TOPL].set_active(true);
						buttons[(int)Buttons.BT_CHARC_TOPR].set_active(true);
						buttons[(int)Buttons.BT_CHARC_SHOESL].set_active(true);
						buttons[(int)Buttons.BT_CHARC_SHOESR].set_active(true);
						buttons[(int)Buttons.BT_CHARC_WEPL].set_active(true);
						buttons[(int)Buttons.BT_CHARC_WEPR].set_active(true);

						for (uint i = 0; i <= 1; i++)
						{
							buttons[(int)Buttons.BT_CHARC_HAIRC0 + i].set_active(true);
						}

						buttons[(int)Buttons.BT_CHARC_OK].set_position(new Point_short(523, 425));
						buttons[(int)Buttons.BT_CHARC_CANCEL].set_position(new Point_short(597, 425));

						return Button.State.NORMAL;
					}
					else
					{
						if (!charSet)
						{
							charSet = true;

							

							buttons[(int)Buttons.BT_CHARC_FACEL].set_active(false);
							buttons[(int)Buttons.BT_CHARC_FACER].set_active(false);
							buttons[(int)Buttons.BT_CHARC_HAIRL].set_active(false);
							buttons[(int)Buttons.BT_CHARC_HAIRR].set_active(false);
							buttons[(int)Buttons.BT_CHARC_HAIRC0].set_active(false);
							buttons[(int)Buttons.BT_CHARC_HAIRC1].set_active(false);
							buttons[(int)Buttons.BT_CHARC_SKINL].set_active(false);
							buttons[(int)Buttons.BT_CHARC_SKINR].set_active(false);
							buttons[(int)Buttons.BT_CHARC_TOPL].set_active(false);
							buttons[(int)Buttons.BT_CHARC_TOPR].set_active(false);
							buttons[(int)Buttons.BT_CHARC_BOTL].set_active(false);
							buttons[(int)Buttons.BT_CHARC_BOTR].set_active(false);
							buttons[(int)Buttons.BT_CHARC_SHOESL].set_active(false);
							buttons[(int)Buttons.BT_CHARC_SHOESR].set_active(false);
							buttons[(int)Buttons.BT_CHARC_WEPL].set_active(false);
							buttons[(int)Buttons.BT_CHARC_WEPR].set_active(false);
							buttons[(int)Buttons.BT_CHARC_GEMDER_F].set_active(false);
							buttons[(int)Buttons.BT_CHARC_GENDER_M].set_active(false);

							for (uint i = 0; i <= 1; i++)
							{
								buttons[(int)Buttons.BT_CHARC_HAIRC0 + i].set_active(false);
							}

							buttons[(int)Buttons.BT_CHARC_OK].set_position(new Point_short(513, 273));
							buttons[(int)Buttons.BT_CHARC_CANCEL].set_position(new Point_short(587, 273));

							namechar.set_state(Textfield.State.FOCUSED);

							return Button.State.NORMAL;
						}
						else
						{
							if (!named)
							{
								string name = text_CharName ?? namechar.get_text();

								if (name.Length <= 0)
								{
									return Button.State.NORMAL;
								}
								else if (name.Length >= 4)
								{
									namechar.set_state(Textfield.State.DISABLED);

									buttons[(int)Buttons.BT_CHARC_OK].set_state(Button.State.DISABLED);
									buttons[(int)Buttons.BT_CHARC_CANCEL].set_state(Button.State.DISABLED);
                                    if (check_name(name))
                                    {
										new NameCharPacket(name).dispatch();

										return Button.State.IDENTITY;
									}
									/*var raceselect = UI.get ().get_element<UIRaceSelect> ();
									if (raceselect)
									{
										if (raceselect.get ().check_name(name))
										{
											new NameCharPacket(name).dispatch();

											return Button.State.IDENTITY;
										}
									}*/

									System.Action okhandler = () =>
									{
										namechar.set_state(Textfield.State.FOCUSED);

										buttons[(int)Buttons.BT_CHARC_OK].set_state(Button.State.NORMAL);
										buttons[(int)Buttons.BT_CHARC_CANCEL].set_state(Button.State.NORMAL);
									};

									UI.get().emplace<UILoginNotice>(UILoginNotice.Message.ILLEGAL_NAME, okhandler, null);

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

									UI.get().emplace<UILoginNotice>(UILoginNotice.Message.ILLEGAL_NAME, okhandler, null);

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
					//UI.get().emplace<UIRaceSelect>();

					var charselect = UI.get().get_element<UICharSelect>();
					if (charselect)
					{
						charselect.get().makeactive();
					}
					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_CANCEL:
				{
					if (charSet)
					{
						charSet = false;

						buttons[(int)Buttons.BT_CHARC_SKINL].set_active(true);
						buttons[(int)Buttons.BT_CHARC_SKINR].set_active(true);

						buttons[(int)Buttons.BT_CHARC_FACEL].set_active(true);
						buttons[(int)Buttons.BT_CHARC_FACER].set_active(true);
						buttons[(int)Buttons.BT_CHARC_HAIRL].set_active(true);
						buttons[(int)Buttons.BT_CHARC_HAIRR].set_active(true);
						buttons[(int)Buttons.BT_CHARC_TOPL].set_active(true);
						buttons[(int)Buttons.BT_CHARC_TOPR].set_active(true);
						buttons[(int)Buttons.BT_CHARC_SHOESL].set_active(true);
						buttons[(int)Buttons.BT_CHARC_SHOESR].set_active(true);
						buttons[(int)Buttons.BT_CHARC_WEPL].set_active(true);
						buttons[(int)Buttons.BT_CHARC_WEPR].set_active(true);

						for (uint i = 0; i <= 7; i++)
						{
							buttons[(int)Buttons.BT_CHARC_HAIRC0 + i].set_active(true);
						}

						buttons[(int)Buttons.BT_CHARC_OK].set_position(new Point_short(523, 425));
						buttons[(int)Buttons.BT_CHARC_CANCEL].set_position(new Point_short(597, 425));

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

							buttons[(int)Buttons.BT_CHARC_FACEL].set_active(false);
							buttons[(int)Buttons.BT_CHARC_FACER].set_active(false);
							buttons[(int)Buttons.BT_CHARC_HAIRL].set_active(false);
							buttons[(int)Buttons.BT_CHARC_HAIRR].set_active(false);
							buttons[(int)Buttons.BT_CHARC_TOPL].set_active(false);
							buttons[(int)Buttons.BT_CHARC_TOPR].set_active(false);
							buttons[(int)Buttons.BT_CHARC_SHOESL].set_active(false);
							buttons[(int)Buttons.BT_CHARC_SHOESR].set_active(false);
							buttons[(int)Buttons.BT_CHARC_WEPL].set_active(false);
							buttons[(int)Buttons.BT_CHARC_WEPR].set_active(false);

							/*for (uint i = 0; i <= 7; i++)
							{
								buttons[(int)Buttons.BT_CHARC_HAIRC0 + i].set_active(false);
							}*/

							buttons[(int)Buttons.BT_CHARC_OK].set_position(new Point_short(514, 394));
							buttons[(int)Buttons.BT_CHARC_CANCEL].set_position(new Point_short(590, 394));

							return Button.State.NORMAL;
						}
						else
						{
							button_pressed((ushort)Buttons.BT_BACK);

							return Button.State.NORMAL;
						}
					}
				}
				case Buttons.BT_CHARC_FACEL:
				{
					face = (face > 0) ? face - 1 : (int)(faces[female].Count - 1);
					newchar.set_face(faces[female][(int)face]);
					facename.change_text(newchar.get_face().get_name());

					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_FACER:
				{
					face = (face < faces[female].Count - 1) ? face + 1 : 0;
					newchar.set_face(faces[female][(int)face]);
					facename.change_text(newchar.get_face().get_name());

					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_HAIRL:
				{
					hair = (hair > 0) ? hair - 1 : (int)(hairs[female].Count - 1);
					newchar.set_hair(hairs[female][(int)hair] + haircolors[female][(int)haircolor]);
					hairname.change_text(newchar.get_hair().get_name());

					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_HAIRR:
				{
					hair = (hair < hairs[female].Count - 1) ? hair + 1 : 0;
					newchar.set_hair(hairs[female][(int)hair] + haircolors[female][(int)haircolor]);
					hairname.change_text(newchar.get_hair().get_name());

					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_HAIRC0:
				case Buttons.BT_CHARC_HAIRC1:
				case Buttons.BT_CHARC_HAIRC2:
				case Buttons.BT_CHARC_HAIRC3:
				case Buttons.BT_CHARC_HAIRC4:
				case Buttons.BT_CHARC_HAIRC5:
				case Buttons.BT_CHARC_HAIRC6:
				case Buttons.BT_CHARC_HAIRC7:
				{
					// TODO: These need to be changed so when you click the color it only assigns the color, not the next in the series.
					haircolor = (haircolor > 0) ? haircolor - 1 : haircolors[female].Count - 1;
					newchar.set_hair(hairs[female][hair] + haircolors[female][haircolor]);
					hairname.change_text(newchar.get_hair().getcolor());
					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_SKINL:
				{
					skin = (skin > 0) ? skin - 1 : skins[female].Count - 1;
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
				case Buttons.BT_CHARC_TOPL:
				{
					top = (top > 0) ? top - 1 : tops[female].Count - 1;
					newchar.add_equip(tops[female][top]);
					topname.change_text(get_equipname(EquipSlot.Id.TOP));

					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_TOPR:
				{
					top = (top < tops[female].Count - 1) ? top + 1 : 0;
					newchar.add_equip(tops[female][top]);
					topname.change_text(get_equipname(EquipSlot.Id.TOP));

					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_BOTL:
				{
					bot = (bot > 0) ? bot - 1 : bots[female].Count - 1;
					newchar.add_equip(bots[female][bot]);
					botname.change_text(get_equipname(EquipSlot.Id.BOTTOM));
					
					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_BOTR:
				{
					bot = (bot < bots[female].Count - 1) ? bot + 1 : 0;
					newchar.add_equip(bots[female][bot]);
					botname.change_text(get_equipname(EquipSlot.Id.BOTTOM));

					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_SHOESL:
				{
					shoe = (shoe > 0) ? shoe - 1 : shoes[female].Count - 1;
					newchar.add_equip(shoes[female][shoe]);
					shoename.change_text(get_equipname(EquipSlot.Id.SHOES));

					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_SHOESR:
				{
					shoe = (shoe < shoes[female].Count - 1) ? shoe + 1 : 0;
					newchar.add_equip(shoes[female][shoe]);
					shoename.change_text(get_equipname(EquipSlot.Id.SHOES));

					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_WEPL:
				{
					weapon = (weapon > 0) ? weapon - 1 : weapons[female].Count - 1;
					newchar.add_equip(weapons[female][weapon]);
					wepname.change_text(get_equipname(EquipSlot.Id.WEAPON));

					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_WEPR:
				{
					weapon = (weapon < weapons[female].Count - 1) ? weapon + 1 : 0;
					newchar.add_equip(weapons[female][weapon]);
					wepname.change_text(get_equipname(EquipSlot.Id.WEAPON));

					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_GENDER_M:
				{
					if (female)
					{
						female = false;
						randomize_look();
					}
					gendername.change_text (get_gendername(female));
					return Button.State.NORMAL;
				}
				case Buttons.BT_CHARC_GEMDER_F:
				{
					if (!female)
					{
						female = true;
						randomize_look();
					}
					gendername.change_text (get_gendername(female));
					return Button.State.NORMAL;
				}
			}

			return Button.State.PRESSED;
		}
		public bool check_name(string name)
		{
			var ForbiddenName = ms.wz.wzProvider_etc["ForbiddenName.img"];

			foreach (var forbiddenName in ForbiddenName)
			{
				string lName = name.ToLower();
				string fName = forbiddenName.ToString().ToLower();

				if (lName.IndexOf(fName) != -1)
				{
					return false;
				}
			}

			return true;
		}
		private void randomize_look()
		{
			hair = randomizer.next_int(hairs[female].Count);
			face = randomizer.next_int(faces[female].Count);
			skin = randomizer.next_int(skins[female].Count);
			haircolor = randomizer.next_int(haircolors[female].Count);
			top = randomizer.next_int(tops[female].Count);
			bot = randomizer.next_int(bots[female].Count);
			shoe = randomizer.next_int(shoes[female].Count);
			weapon = randomizer.next_int(weapons[female].Count);

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
			hairColorName.change_text(newchar.get_hair().getcolor());
			topname.change_text(get_equipname(EquipSlot.Id.TOP));
			botname.change_text(get_equipname(EquipSlot.Id.BOTTOM));
			shoename.change_text(get_equipname(EquipSlot.Id.SHOES));
			wepname.change_text(get_equipname(EquipSlot.Id.WEAPON));
			gendername.change_text (get_gendername(female));
		}
		private string get_equipname(EquipSlot.Id slot)
		{
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

		private string get_gendername (bool isFemale)
		{
			return isFemale ? "女" : "男";
		}
		public string text_CharName;
		public override void OnAdd()
		{
			var bound = namechar.get_bounds();
			var pos = position + bound.get_left_top();
			EditTextInfo editTextInfo = new EditTextInfo(pos.x(), pos.y() - bound.height(), bound.width(), bound.height());
			MessageCenter.get().ShowCharName(this, editTextInfo);
		}

		public override void OnRemove()
		{
			MessageCenter.get().HideCharName(this);
		}

		public void HideTextfield_CharName ()
        {
			namechar.isForbid = true;
        }

		private enum Buttons : ushort
		{
			BT_BACK,
			BT_CHARC_OK,
			BT_CHARC_CANCEL,
			BT_CHARC_FACEL,
			BT_CHARC_FACER,
			BT_CHARC_HAIRL,
			BT_CHARC_HAIRR,
			BT_CHARC_SKINL,
			BT_CHARC_SKINR,
			BT_CHARC_TOPL,
			BT_CHARC_TOPR,
			BT_CHARC_BOTL,
			BT_CHARC_BOTR,
			BT_CHARC_SHOESL,
			BT_CHARC_SHOESR,
			BT_CHARC_WEPL,
			BT_CHARC_WEPR,
			BT_CHARC_GENDER_M,
			BT_CHARC_GEMDER_F,
			BT_CHARC_HAIRC0,
			BT_CHARC_HAIRC1,
			BT_CHARC_HAIRC2,
			BT_CHARC_HAIRC3,
			BT_CHARC_HAIRC4,
			BT_CHARC_HAIRC5,
			BT_CHARC_HAIRC6,
			BT_CHARC_HAIRC7
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

		private BoolPairNew<List<byte>> skins = new BoolPairNew<List<byte>>();
		private BoolPairNew<List<byte>> haircolors = new BoolPairNew<List<byte>>();
		private BoolPairNew<List<int>> faces = new BoolPairNew<List<int>>();
		private BoolPairNew<List<int>> hairs = new BoolPairNew<List<int>>();
		private BoolPairNew<List<int>> tops = new BoolPairNew<List<int>>();
		private BoolPairNew<List<int>> bots = new BoolPairNew<List<int>>();
		private BoolPairNew<List<int>> shoes = new BoolPairNew<List<int>>();
		private BoolPairNew<List<int>> weapons = new BoolPairNew<List<int>>();

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
		private Text hairColorName = new Text();
		private Text bodyname = new Text();
		private Text topname = new Text();
		private Text botname = new Text();
		private Text shoename = new Text();
		private Text wepname = new Text();
		private Text gendername = new Text();
		
		private Text version = new Text();
	}
}