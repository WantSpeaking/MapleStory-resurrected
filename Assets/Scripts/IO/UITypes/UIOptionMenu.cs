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
	public class UIOptionMenu : UIDragElement<PosOPTIONMENU>
	{
		public const Type TYPE = UIElement.Type.OPTIONMENU;
		public const bool FOCUSED = true;
		public const bool TOGGLED = false;

		public UIOptionMenu ()
		{
			//this.UIDragElement<PosOPTIONMENU> = new <type missing>();
			this.selected_tab = 0;
			WzObject OptionMenu = nl.nx.wzFile_ui["StatusBar3.img"]["OptionMenu"];
			WzObject backgrnd = OptionMenu["backgrnd"];

			sprites.Add (backgrnd);
			sprites.Add (OptionMenu["backgrnd2"]);

			WzObject graphic = OptionMenu["graphic"];

			tab_background[(int)Buttons.TAB0] = graphic["layer:backgrnd"];
			tab_background[(int)Buttons.TAB1] = OptionMenu["sound"]["layer:backgrnd"];
			tab_background[(int)Buttons.TAB2] = OptionMenu["game"]["layer:backgrnd"];
			tab_background[(int)Buttons.TAB3] = OptionMenu["invite"]["layer:backgrnd"];
			tab_background[(int)Buttons.TAB4] = OptionMenu["screenshot"]["layer:backgrnd"];

			buttons[(int)Buttons.CANCEL] = new MapleButton (OptionMenu["button:Cancel"]);
			buttons[(int)Buttons.OK] = new MapleButton (OptionMenu["button:OK"]);
			buttons[(int)Buttons.UIRESET] = new MapleButton (OptionMenu["button:UIReset"]);

			WzObject tab = OptionMenu["tab"];
			WzObject tab_disabled = tab["disabled"];
			WzObject tab_enabled = tab["enabled"];

			for (uint i = (int)Buttons.TAB0; i < (ulong)Buttons.CANCEL; i++)
			{
				buttons[i] = new TwoSpriteButton (tab_disabled[i.ToString ()], tab_enabled[i.ToString ()]);
			}

			string sButtonUOL = graphic["combo:resolution"]["sButtonUOL"].ToString ();
			string ctype = new string (sButtonUOL.back (),1);
			MapleComboBox.Type type = (MapleComboBox.Type)Convert.ToInt32 (ctype);

			List<string> resolutions = new List<string> () {"800 x 600 ( 4 : 3 )", "1024 x 768 ( 4 : 3 )", "1280 x 720 ( 16 : 9 )", "1366 x 768 ( 16 : 9 )", "1920 x 1080 ( 16 : 9 ) - Beta"};

			short max_width = Configuration.get ().get_max_width ();
			short max_height = Configuration.get ().get_max_height ();

			if (max_width >= 1920 && max_height >= 1200)
			{
				resolutions.Add ("1920 x 1200 ( 16 : 10 ) - Beta");
			}

			ushort default_option = 0;
			short screen_width = Constants.get ().get_viewwidth ();
			short screen_height = Constants.get ().get_viewheight ();

			switch (screen_width)
			{
				case 800:
					default_option = 0;
					break;
				case 1024:
					default_option = 1;
					break;
				case 1280:
					default_option = 2;
					break;
				case 1366:
					default_option = 3;
					break;
				case 1920:
					switch (screen_height)
					{
						case 1080:
							default_option = 4;
							break;
						case 1200:
							default_option = 5;
							break;
					}

					break;
			}

			long combobox_width = graphic["combo:resolution"]["boxWidth"];
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: Point<short> lt = Point<short>(graphic["combo:resolution"]["lt"]);
			Point<short> lt = graphic["combo:resolution"]["lt"];

			buttons[(int)Buttons.SELECT_RES] = new MapleComboBox (type, resolutions, default_option, position, lt, combobox_width);

			Point<short> bg_dimensions = new Texture (backgrnd).get_dimensions ();

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: dimension = bg_dimensions;
			dimension = (bg_dimensions);
			dragarea = new Point<short> (bg_dimensions.x (), 20);

			change_tab ((ushort)Buttons.TAB2);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float inter) const override
		public override void draw (float inter)
		{
			base.draw_sprites (inter);

			tab_background[selected_tab].draw (position);

			base.draw_buttons (inter);
		}

		public override Cursor.State send_cursor (bool clicked, Point<short> cursorpos)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: Cursor::State dstate = UIDragElement::send_cursor(clicked, cursorpos);
			Cursor.State dstate = base.send_cursor (clicked, cursorpos);

			if (dragged)
			{
				return dstate;
			}

			var button = buttons[(int)Buttons.SELECT_RES];

			if (button.is_pressed ())
			{
				if (button.in_combobox (cursorpos))
				{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
					Cursor.State new_state = button.send_cursor (clicked, cursorpos);
					if (new_state != Cursor.State.IDLE)
					{
						return new_state;
					}
				}
				else
				{
					remove_cursor ();
				}
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return UIElement::send_cursor(clicked, cursorpos);
			return base.send_cursor (clicked, cursorpos);
		}

		public override void send_key (int keycode, bool pressed, bool escape)
		{
			if (pressed)
			{
				if (escape)
				{
					deactivate ();
				}
				else if (keycode == (int)KeyAction.Id.RETURN)
				{
					button_pressed ((ushort)Buttons.OK);
				}
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public override Button.State button_pressed (ushort buttonid)
		{
			switch ((Buttons)buttonid)
			{
				case Buttons.TAB0:
				case Buttons.TAB1:
				case Buttons.TAB2:
				case Buttons.TAB3:
				case Buttons.TAB4:
					change_tab (buttonid);
					return Button.State.IDENTITY;
				case Buttons.CANCEL:
					deactivate ();
					return Button.State.NORMAL;
				case Buttons.OK:
					switch ((Buttons)selected_tab)
					{
						case Buttons.TAB0:
						{
							ushort selected_value = buttons[(int)Buttons.SELECT_RES].get_selected ();

							short width = Constants.get ().get_viewwidth ();
							short height = Constants.get ().get_viewheight ();

							switch (selected_value)
							{
								case 0:
									width = 800;
									height = 600;
									break;
								case 1:
									width = 1024;
									height = 768;
									break;
								case 2:
									width = 1280;
									height = 720;
									break;
								case 3:
									width = 1366;
									height = 768;
									break;
								case 4:
									width = 1920;
									height = 1080;
									break;
								case 5:
									width = 1920;
									height = 1200;
									break;
							}

							Setting<Width>.get ().save ((ushort)width);
							Setting<Height>.get ().save ((ushort)height);

							Constants.get ().set_viewwidth (width);
							Constants.get ().set_viewheight (height);
						}
							break;
						case Buttons.TAB1:
						case Buttons.TAB2:
						case Buttons.TAB3:
						case Buttons.TAB4:
						default:
							break;
					}

					deactivate ();
					return Button.State.NORMAL;
				case Buttons.UIRESET:
					return Button.State.DISABLED;
				case Buttons.SELECT_RES:
					buttons[(int)Buttons.SELECT_RES].toggle_pressed ();
					return Button.State.NORMAL;
				default:
					return Button.State.DISABLED;
			}
		}

		private void change_tab (ushort tabid)
		{
			buttons[selected_tab].set_state (Button.State.NORMAL);
			buttons[tabid].set_state (Button.State.PRESSED);

			selected_tab = tabid;

			switch ((Buttons)tabid)
			{
				case Buttons.TAB0:
					buttons[(int)Buttons.SELECT_RES].set_active (true);
					break;
				case Buttons.TAB1:
				case Buttons.TAB2:
				case Buttons.TAB3:
				case Buttons.TAB4:
					buttons[(int)Buttons.SELECT_RES].set_active (false);
					break;
				default:
					break;
			}
		}

		private enum Buttons : ushort
		{
			TAB0,
			TAB1,
			TAB2,
			TAB3,
			TAB4,
			CANCEL,
			OK,
			UIRESET,
			SELECT_RES
		}

		private ushort selected_tab;
		private Texture[] tab_background = new Texture[(int)Buttons.CANCEL];
	}
}


#if USE_NX
#endif