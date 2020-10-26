#define USE_NX

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
	public class UILogin : UIElement
	{
		public const Type TYPE = UIElement.Type.LOGIN;
		public const bool FOCUSED = false;
		public const bool TOGGLED = false;

		public UILogin() : base(new Point<short>(0, 0), new Point<short>(800, 600))
		{
			this.signboard_pos = new Point<short>(389, 333);
			new LoginStartPacket().dispatch();

			string LoginMusicNewtro = Configuration.get().get_login_music_newtro();

			new Music(LoginMusicNewtro).play();

			string version_text = Configuration.get().get_version();
			version = new Text(Text.Font.A11B, Text.Alignment.LEFT, Color.Name.LEMONGRASS, "Ver. " + version_text);

			WzObject map001 = nl.nx.wzFile_map001["Back"]["login.img"];
			WzObject back = map001["back"];
			WzObject ani = map001["ani"];

			WzObject Login = nl.nx.wzFile_ui["Login.img"];
			WzObject Title = Login["Title"];
			WzObject Common = Login["Common"];

			WzObject prettyLogo = nl.nx.wzFile_wzFile_mapPretty["Back"]["login.img"]["ani"]["16"];
			WzObject frame = nl.nx.wzFile_mapLatest["Obj"]["login.img"]["Common"]["frame"]["2"]["0"];

			sprites.Add(new Sprite(back["11"], new Point<short>(400, 300)));
			sprites.Add(new Sprite(ani["17"], new Point<short>(165, 276)));
			sprites.Add(new Sprite(ani["18"], new Point<short>(301, 245)));
			sprites.Add(new Sprite(ani["19"], new Point<short>(374, 200)));
			sprites.Add(new Sprite(ani["19"], new Point<short>(348, 161)));
			sprites.Add(new Sprite(back["35"], new Point<short>(399, 260)));
			sprites.Add(new Sprite(prettyLogo, new Point<short>(409, 144)));
			sprites.Add(new Sprite(Title["signboard"], signboard_pos));
			sprites.Add(new Sprite(frame, new Point<short>(400, 300)));
			sprites.Add(new Sprite(Common["frame"], new Point<short>(400, 300)));

			buttons[(int)Buttons.BT_LOGIN] = new MapleButton(Title["BtLogin"], signboard_pos + new Point<short>(62, -51));
			buttons[(int)Buttons.BT_SAVEID] = new MapleButton(Title["BtLoginIDSave"], signboard_pos + new Point<short>(-89, 5));
			buttons[(int)Buttons.BT_IDLOST] = new MapleButton(Title["BtLoginIDLost"], signboard_pos + new Point<short>(-17, 5));
			buttons[(int)Buttons.BT_PASSLOST] = new MapleButton(Title["BtPasswdLost"], signboard_pos + new Point<short>(55, 5));
			buttons[(int)Buttons.BT_REGISTER] = new MapleButton(Title["BtNew"], signboard_pos + new Point<short>(-101, 25));
			buttons[(int)Buttons.BT_HOMEPAGE] = new MapleButton(Title["BtHomePage"], signboard_pos + new Point<short>(-29, 25));
			buttons[(int)Buttons.BT_QUIT] = new MapleButton(Title["BtQuit"], signboard_pos + new Point<short>(43, 25));

			checkbox[false] = Title["check"]["0"];
			checkbox[true] = Title["check"]["1"];

			background = new ColorBox(dimension.x(), dimension.y(), Color.Name.BLACK, 1.0f);

			Point<short> textbox_pos = signboard_pos + new Point<short>(-96, -51);
			Point<short> textbox_dim = new Point<short>(150, 24);
			short textbox_limit = 12;

#region Account
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: account = Textfield(Text::Font::A13M, Text::Alignment::LEFT, Color::Name::JAMBALAYA, Color::Name::SMALT, 0.75f, Rectangle<short>(textbox_pos, textbox_pos + textbox_dim), textbox_limit);
			account = new Textfield(Text.Font.A13M, Text.Alignment.LEFT, Color.Name.JAMBALAYA, Color.Name.SMALT, 0.75f, new Rectangle<short>(textbox_pos, textbox_pos + textbox_dim), (uint)textbox_limit);

			account.set_key_callback(KeyAction.Id.TAB, () =>
			{
					account.set_state(Textfield.State.NORMAL);
					password.set_state(Textfield.State.FOCUSED);
			});

			account.set_enter_callback((string msg) =>
			{
					login();
			});

			accountbg = Title["ID"];
#endregion

#region Password
			textbox_pos.shift_y(26);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: password = Textfield(Text::Font::A13M, Text::Alignment::LEFT, Color::Name::JAMBALAYA, Color::Name::PRUSSIANBLUE, 0.85f, Rectangle<short>(textbox_pos, textbox_pos + textbox_dim), textbox_limit);
			password = new Textfield(Text.Font.A13M, Text.Alignment.LEFT, Color.Name.JAMBALAYA, Color.Name.PRUSSIANBLUE, 0.85f, new Rectangle<short>(textbox_pos, textbox_pos + textbox_dim), (uint)textbox_limit);

			password.set_key_callback(KeyAction.Id.TAB, () =>
			{
					account.set_state(Textfield.State.FOCUSED);
					password.set_state(Textfield.State.NORMAL);
			});

			password.set_enter_callback((string msg) =>
			{
					login();
			});

			password.set_cryptchar((sbyte)'*');
			passwordbg = Title["PW"];
#endregion

			saveid = Setting<SaveLogin>.get().load();

			if (saveid)
			{
				account.change_text(Setting<DefaultAccount>.get().load());
				password.set_state(Textfield.State.FOCUSED);
			}
			else
			{
				account.set_state(Textfield.State.FOCUSED);
			}

			if (Configuration.get().get_var_login())
			{
				UI.get().emplace<UILoginWait>();

				var loginwait = UI.get().get_element<UILoginWait>();

				if (loginwait != null && loginwait.Dereference().is_active())
				{
					new LoginPacket(Configuration.get().get_var_acc(), Configuration.get().get_var_pass()).dispatch();
				}
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float alpha) const override
		public override void draw(float alpha)
		{
			background.draw(position + new Point<short>(0, 7));

			base.draw(alpha);

			version.draw(position + new Point<short>(707, 4));
			account.draw(position + new Point<short>(1, 0));
			password.draw(position + new Point<short>(1, 3));

			if (account.get_state() == Textfield.State.NORMAL && account.empty())
			{
				accountbg.draw(position + signboard_pos + new Point<short>(-101, -51));
			}

			if (password.get_state() == Textfield.State.NORMAL && password.empty())
			{
				passwordbg.draw(position + signboard_pos + new Point<short>(-101, -25));
			}

			checkbox[saveid].draw(position + signboard_pos + new Point<short>(-101, 7));
		}
		public override void update()
		{
			base.update();

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: account.update(position);
			account.update(position);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: password.update(position);
			password.update(position);
		}

		public override Cursor.State send_cursor(bool clicked, Point<short> cursorpos)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: if (Cursor::State new_state = account.send_cursor(cursorpos, clicked))
			Cursor.State new_state = account.send_cursor (cursorpos, clicked);
			if (new_state != Cursor.State.IDLE)
			{
				return new_state;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: if (Cursor::State new_state = password.send_cursor(cursorpos, clicked))
			Cursor.State new_state1 = password.send_cursor (cursorpos, clicked);
			if (new_state1 != Cursor.State.IDLE)
			{
				return new_state1;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return UIElement::send_cursor(clicked, cursorpos);
			return base.send_cursor(clicked, cursorpos);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type()
		{
			return TYPE;
		}

		public override Button.State button_pressed(ushort id)
		{
			switch ((Buttons)id)
			{
				case Buttons.BT_LOGIN:
				{
					login();

					return Button.State.NORMAL;
				}
				case Buttons.BT_REGISTER:
				case Buttons.BT_HOMEPAGE:
				case Buttons.BT_PASSLOST:
				case Buttons.BT_IDLOST:
				{
					open_url(id);

					return Button.State.NORMAL;
				}
				case Buttons.BT_SAVEID:
				{
					saveid = !saveid;

					Setting<SaveLogin>.get().save(saveid);

					return Button.State.MOUSEOVER;
				}
				case Buttons.BT_QUIT:
				{
					UI.get().quit();

					return Button.State.PRESSED;
				}
				default:
				{
					return Button.State.DISABLED;
				}
			}
		}

		private void login()
		{
			account.set_state(Textfield.State.DISABLED);
			password.set_state(Textfield.State.DISABLED);

			string account_text = account.get_text();
			string password_text = password.get_text();

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Only lambda expressions having all locals passed by reference can be converted to C#:
//ORIGINAL LINE: System.Action okhandler = [&, password_text]()
			System.Action okhandler = () =>
			{
				account.set_state(Textfield.State.NORMAL);
				password.set_state(Textfield.State.NORMAL);

				if (!string.IsNullOrEmpty(password_text))
				{
					password.set_state(Textfield.State.FOCUSED);
				}
				else
				{
					account.set_state(Textfield.State.FOCUSED);
				}
			};

			if (string.IsNullOrEmpty(account_text))
			{
				UI.get().emplace<UILoginNotice>(UILoginNotice.Message.NOT_REGISTERED, okhandler);
				return;
			}

			if (password_text.Length <= 4)
			{
				UI.get().emplace<UILoginNotice>(UILoginNotice.Message.WRONG_PASSWORD, okhandler);
				return;
			}

			UI.get().emplace<UILoginWait>(okhandler);

			var loginwait = UI.get().get_element<UILoginWait>();

			if (loginwait != null && loginwait.Dereference().is_active())
			{
				new LoginPacket(account_text, password_text).dispatch();
			}
		}
		private void open_url(ushort id)
		{
			string url;

			switch ((Buttons)id)
			{
				case Buttons.BT_REGISTER:
					url = Configuration.get().get_joinlink();
					break;
				case Buttons.BT_HOMEPAGE:
					url = Configuration.get().get_website();
					break;
				case Buttons.BT_PASSLOST:
					url = Configuration.get().get_findpass();
					break;
				case Buttons.BT_IDLOST:
					url = Configuration.get().get_findid();
					break;
				default:
					return;
			}

			//todo ShellExecuteA(null, "open", url, null, null, SW_SHOWNORMAL);
		}

		private enum Buttons
		{
			BT_LOGIN,
			BT_REGISTER,
			BT_HOMEPAGE,
			BT_PASSLOST,
			BT_IDLOST,
			BT_SAVEID,
			BT_QUIT,
			NUM_BUTTONS
		}

		private Text version = new Text();
		private Textfield account = new Textfield();
		private Textfield password = new Textfield();
		private Texture accountbg = new Texture();
		private Texture passwordbg = new Texture();
		private BoolPair<Texture> checkbox = new BoolPair<Texture>();
		private ColorBox background = new ColorBox();
		private Point<short> signboard_pos = new Point<short>();

		private bool saveid;
	}
}







#if USE_NX
#endif
