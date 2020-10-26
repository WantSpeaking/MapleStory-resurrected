#define USE_NX

using System.Collections.Generic;
using System.Linq;
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
	public class UIChatBar : UIDragElement<PosCHAT>
	{
		public const Type TYPE = UIElement.Type.CHATBAR;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public enum LineType
		{
			UNK0,
			WHITE,
			RED,
			BLUE,
			YELLOW
		}

		public UIChatBar ()
		{
			//this.UIDragElement<PosCHAT> = new Point<short>(410, -5);
			chatopen = Setting<Chatopen>.get ().load ();
			chatopen_persist = chatopen;
			chatfieldopen = false;
			chatrows = 5;
			lastpos = 0;
			rowpos = 0;
			rowmax = -1;

			WzObject chat = nl.nx.wzFile_ui["StatusBar3.img"]["chat"];
			WzObject ingame = chat["ingame"];
			WzObject view = ingame["view"];
			WzObject input = ingame["input"];
			WzObject chatTarget = chat["common"]["chatTarget"];

			chatspace[0] = view["min"]["top"];
			chatspace[1] = view["min"]["center"];
			chatspace[2] = view["min"]["bottom"];
			chatspace[3] = view["drag"];

			short chattop_y = (short)(getchattop (true) - 33);
			closechat = new Point<short> (387, 21);

			buttons[(int)Buttons.BT_OPENCHAT] = new MapleButton (view["btMax"], new Point<short> (391, -7));
			buttons[(int)Buttons.BT_CLOSECHAT] = new MapleButton (view["btMin"], closechat + new Point<short> (0, chattop_y));
			buttons[(int)Buttons.BT_CHAT] = new MapleButton (input["button:chat"], new Point<short> (344, -8));
			buttons[(int)Buttons.BT_LINK] = new MapleButton (input["button:itemLink"], new Point<short> (365, -8));
			buttons[(int)Buttons.BT_HELP] = new MapleButton (input["button:help"], new Point<short> (386, -8));

			buttons[(ushort)(chatopen ? Buttons.BT_OPENCHAT : Buttons.BT_CLOSECHAT)].set_active (false);
			buttons[(int)Buttons.BT_CHAT].set_active (chatopen ? true : false);
			buttons[(int)Buttons.BT_LINK].set_active (chatopen ? true : false);
			buttons[(int)Buttons.BT_HELP].set_active (chatopen ? true : false);

			chattab_x = 6;
			chattab_y = chattop_y;
			chattab_span = 54;

			for (uint i = 0; i < (ulong)ChatTab.NUM_CHATTAB; i++)
			{
				buttons[(int)Buttons.BT_TAB_0 + i] = new MapleButton (view["tab"], new Point<short> ((short)(chattab_x + (i * chattab_span)), chattab_y));
				buttons[(int)Buttons.BT_TAB_0 + i].set_active (chatopen ? true : false);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: chattab_text[ChatTab::CHT_ALL + i] = Text(Text::Font::A12M, Text::Alignment::CENTER, Color::Name::DUSTYGRAY, ChatTabText[i]);
				chattab_text[(int)ChatTab.CHT_ALL + i] = new Text (Text.Font.A12M, Text.Alignment.CENTER, Color.Name.DUSTYGRAY, ChatTabText[(int)i]);
			}

			chattab_text[(int)ChatTab.CHT_ALL].change_color (Color.Name.WHITE);

			buttons[(uint)((int)Buttons.BT_TAB_0 + ChatTab.NUM_CHATTAB)] = new MapleButton (view["btAddTab"], new Point<short> ((short)(chattab_x + ((short)ChatTab.NUM_CHATTAB * chattab_span)), chattab_y));
			buttons[(uint)((int)Buttons.BT_TAB_0 + ChatTab.NUM_CHATTAB)].set_active (chatopen ? true : false);

			buttons[(int)Buttons.BT_CHAT_TARGET] = new MapleButton (chatTarget["all"], new Point<short> (5, -8));
			buttons[(int)Buttons.BT_CHAT_TARGET].set_active (chatopen ? true : false);

			chatenter = input["layer:chatEnter"];
			chatcover = input["layer:backgrnd"];

			chatfield = new Textfield (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.WHITE, new Rectangle<short> (new Point<short> (62, -9), new Point<short> (330, 8)), 0);
			chatfield.set_state (chatopen ? Textfield.State.NORMAL : Textfield.State.DISABLED);

			chatfield.set_enter_callback ((string msg) =>
			{
				if (msg.Length > 0)
				{
					int last = msg.LastIndexOf (' ');

					if (last != -1)
					{
						//msg = msg.erase(last + 1);
						msg = msg.Substring (0, last);

						new GeneralChatPacket (msg, true).dispatch ();

						lastentered.Add (msg);
						lastpos = (uint)lastentered.Count;
					}
					else
					{
						toggle_chatfield ();
					}

					chatfield.change_text ("");
				}
				else
				{
					toggle_chatfield ();
				}
			});

			chatfield.set_key_callback (KeyAction.Id.UP, () =>
			{
				if (lastpos > 0)
				{
					lastpos--;
					chatfield.change_text (lastentered[(int)lastpos]);
				}
			});

			chatfield.set_key_callback (KeyAction.Id.DOWN, () =>
			{
				if (lastentered.Count > 0 && lastpos < lastentered.Count - 1)
				{
					lastpos++;
					chatfield.change_text (lastentered[(int)lastpos]);
				}
			});

			chatfield.set_key_callback (KeyAction.Id.ESCAPE, () => { toggle_chatfield (false); });

			//int16_t slider_x = 394;
			//int16_t slider_y = -80;
			//int16_t slider_height = slider_y + 56;
			//int16_t slider_unitrows = chatrows;
			//int16_t slider_rowmax = 1;
			//slider = Slider(Slider::Type::CHATBAR, Range<int16_t>(slider_y, slider_height), slider_x, slider_unitrows, slider_rowmax, [&](bool upwards) {});

			send_chatline ("[Welcome] Welcome to MapleStory!!", LineType.YELLOW);

			dimension = new Point<short> (410, DIMENSION_Y);

			//if (chatopen)
			//	dimension.shift_y(getchatbarheight());
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float inter) const override
		public override void draw (float inter)
		{
			base.draw_sprites (inter);

			if (chatopen)
			{
				short chattop = getchattop (chatopen);

				var pos_adj = chatfieldopen ? new Point<short> (0, 0) : new Point<short> (0, 28);

				chatspace[0].draw (position + new Point<short> (0, chattop) + pos_adj);

				if (chatrows > 1)
				{
					chatspace[1].draw (new DrawArgument (position + new Point<short> (0, -28) + pos_adj, new Point<short> (0, (short)(28 + chattop))));
				}

				chatspace[2].draw (position + new Point<short> (0, -28) + pos_adj);
				chatspace[3].draw (position + new Point<short> (0, (short)(-15 + chattop)) + pos_adj);

				//slider.draw(position);

				short yshift = chattop;

				for (uint i = 0; i < chatrows; i++)
				{
					short rowid = (short)(rowpos - i);

					if (rowtexts.All (pair => pair.Key != rowid))
					{
						break;
					}

					short textheight = (short)(rowtexts[rowid].height () / CHATROWHEIGHT);

					while (textheight > 0)
					{
						yshift += CHATROWHEIGHT;
						textheight--;
					}

					rowtexts[rowid].draw (position + new Point<short> (9, (short)(getchattop (chatopen) - yshift - 21)) + pos_adj);
				}
			}
			else
			{
				var pos_adj = chatfieldopen ? new Point<short> (0, -28) : new Point<short> (0, 0);

				chatspace[0].draw (position + new Point<short> (0, -1) + pos_adj);
				chatspace[1].draw (position + new Point<short> (0, -1) + pos_adj);
				chatspace[2].draw (position + pos_adj);
				chatspace[3].draw (position + new Point<short> (0, -16) + pos_adj);

				if (rowtexts.Any (pair => pair.Key == rowmax))
				{
					rowtexts[rowmax].draw (position + new Point<short> (9, -6) + pos_adj);
				}
			}

			if (chatfieldopen)
			{
				chatcover.draw (new DrawArgument (position + new Point<short> (0, -13), new Point<short> (409, 0)));
				chatenter.draw (new DrawArgument (position + new Point<short> (0, -13), new Point<short> (285, 0)));
				chatfield.draw (position + new Point<short> (-4, -4));
			}

			base.draw_buttons (inter);

			if (chatopen)
			{
				var pos_adj = chatopen && !chatfieldopen ? new Point<short> (0, 28) : new Point<short> (0, 0);

				for (uint i = 0; i < (ulong)ChatTab.NUM_CHATTAB; i++)
				{
					chattab_text[(int)ChatTab.CHT_ALL + i].draw (position + new Point<short> ((short)(chattab_x + (i * chattab_span) + 25), (short)(chattab_y - 3)) + pos_adj);
				}
			}
		}

		public override void update ()
		{
			base.update ();

			var pos_adj = chatopen && !chatfieldopen ? new Point<short> (0, 28) : new Point<short> (0, 0);

			for (uint i = 0; i < (ulong)ChatTab.NUM_CHATTAB; i++)
			{
				buttons[(int)Buttons.BT_TAB_0 + i].set_position (new Point<short> ((short)(chattab_x + (i * chattab_span)), chattab_y) + pos_adj);
			}

			buttons[(uint)((int)Buttons.BT_TAB_0 + ChatTab.NUM_CHATTAB)].set_position (new Point<short> ((short)(chattab_x + ((short)ChatTab.NUM_CHATTAB * chattab_span)), chattab_y) + pos_adj);
			buttons[(int)Buttons.BT_CLOSECHAT].set_position (closechat + new Point<short> (0, chattab_y) + pos_adj);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: chatfield.update(position);
			chatfield.update (position);

			foreach (var iter in message_cooldowns)
			{
				message_cooldowns[iter.Key]-= Constants.TIMESTEP;
				//iter.Value -= Constants.TIMESTEP;
			}
		}

		public override void send_key (int keycode, bool pressed, bool escape)
		{
			if (pressed)
			{
				if (keycode == (int)KeyAction.Id.RETURN)
				{
					toggle_chatfield ();
				}
				else if (escape)
				{
					toggle_chatfield (false);
				}
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_in_range(Point<short> cursorpos) const override
		public override bool is_in_range (Point<short> cursorpos)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: var bounds = getbounds(dimension);
			var bounds = getbounds (dimension);
			return bounds.contains (cursorpos);
		}

		public override Cursor.State send_cursor (bool clicking, Point<short> cursorpos)
		{
			if (chatopen)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: if (Cursor::State new_state = chatfield.send_cursor(cursorpos, clicking))
				Cursor.State new_state = chatfield.send_cursor (cursorpos, clicking);
				if (new_state != Cursor.State.IDLE)
				{
					return new_state;
				}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return check_dragtop(clicking, cursorpos);
				return check_dragtop (clicking, cursorpos);
			}
			else
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return UIDragElement::send_cursor(clicking, cursorpos);
				return base.send_cursor (clicking, cursorpos);
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public Cursor.State check_dragtop (bool clicking, Point<short> cursorpos)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: Rectangle<short> bounds = getbounds(dimension);
			Rectangle<short> bounds = getbounds (dimension);
			Point<short> bounds_lt = bounds.get_left_top ();
			Point<short> bounds_rb = bounds.get_right_bottom ();

			short chattab_height = 20;
			short bounds_rb_y = bounds_rb.y ();
			short bounds_lt_y = (short)(bounds_lt.y () + chattab_height);

			var chattop_rb = new Point<short> ((short)(bounds_rb.x () - 1), (short)(bounds_rb_y - 27));
			var chattop = new Rectangle<short> (new Point<short> ((short)(bounds_lt.x () + 1), bounds_lt_y), chattop_rb);

			var chattopleft = new Rectangle<short> (new Point<short> (bounds_lt.x (), bounds_lt_y), new Point<short> (bounds_lt.x (), chattop_rb.y ()));
			var chattopright = new Rectangle<short> (new Point<short> ((short)(chattop_rb.x () + 1), bounds_lt_y), new Point<short> ((short)(chattop_rb.x () + 1), chattop_rb.y ()));
			var chatleft = new Rectangle<short> (new Point<short> (bounds_lt.x (), bounds_lt_y), new Point<short> (bounds_lt.x (), (short)(bounds_lt_y + bounds_rb_y)));
			var chatright = new Rectangle<short> (new Point<short> ((short)(chattop_rb.x () + 1), bounds_lt_y), new Point<short> ((short)(chattop_rb.x () + 1), (short)(bounds_lt_y + bounds_rb_y)));

			bool in_chattop = chattop.contains (cursorpos);
			bool in_chattopleft = chattopleft.contains (cursorpos);
			bool in_chattopright = chattopright.contains (cursorpos);
			bool in_chatleft = chatleft.contains (cursorpos);
			bool in_chatright = chatright.contains (cursorpos);

			if (dragchattop)
			{
				if (clicking)
				{
					short ydelta = (short)(cursorpos.y () - bounds_rb_y + 10);

					while (ydelta > 0 && chatrows > MINCHATROWS)
					{
						chatrows--;
						ydelta -= CHATROWHEIGHT;
					}

					while (ydelta < 0 && chatrows < MAXCHATROWS)
					{
						chatrows++;
						ydelta += CHATROWHEIGHT;
					}

					//slider.setrows(rowpos, chatrows, rowmax);
					//slider.setvertical(Range<int16_t>(0, CHATROWHEIGHT * chatrows - 14));

					chattab_y = (short)(getchattop (chatopen) - 33);
					//dimension.set_y(getchatbarheight());

					return Cursor.State.CLICKING;
				}
				else
				{
					dragchattop = false;
				}
			}
			else if (in_chattop)
			{
				if (clicking)
				{
					dragchattop = true;

					return Cursor.State.CLICKING;
				}
				else
				{
					return Cursor.State.CHATBARVDRAG;
				}
			}
			else if (in_chattopleft)
			{
				if (clicking)
				{
					//dragchattopleft = true;

					return Cursor.State.CLICKING;
				}
				else
				{
					return Cursor.State.CHATBARBRTLDRAG;
				}
			}
			else if (in_chattopright)
			{
				if (clicking)
				{
					//dragchattopright = true;

					return Cursor.State.CLICKING;
				}
				else
				{
					return Cursor.State.CHATBARBLTRDRAG;
				}
			}
			else if (in_chatleft)
			{
				if (clicking)
				{
					//dragchatleft = true;

					return Cursor.State.CLICKING;
				}
				else
				{
					return Cursor.State.CHATBARHDRAG;
				}
			}
			else if (in_chatright)
			{
				if (clicking)
				{
					//dragchatright = true;

					return Cursor.State.CLICKING;
				}
				else
				{
					return Cursor.State.CHATBARHDRAG;
				}
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return UIDragElement::send_cursor(clicking, cursorpos);
			return base.send_cursor (clicking, cursorpos);
		}

		public void send_chatline (string line, LineType type)
		{
			rowmax++;
			rowpos = rowmax;

			//slider.setrows(rowpos, chatrows, rowmax);

			Color.Name color;

			switch (type)
			{
				case LineType.RED:
					color = Color.Name.DARKRED;
					break;
				case LineType.BLUE:
					color = Color.Name.MEDIUMBLUE;
					break;
				case LineType.YELLOW:
					color = Color.Name.YELLOW;
					break;
				default:
					color = Color.Name.WHITE;
					break;
			}

			rowtexts.Add (rowmax, new Text(Text.Font.A11M, Text.Alignment.LEFT, color, line, 480));
		}

		public void display_message (Messages.Type line, UIChatBar.LineType type)
		{
			if (message_cooldowns[line] > 0)
			{
				return;
			}

			string message = Messages.messages[line];
			send_chatline (message, type);

			message_cooldowns[line] = MESSAGE_COOLDOWN;
		}

		public void toggle_chat ()
		{
			chatopen_persist = !chatopen_persist;
			toggle_chat (chatopen_persist);
		}

		public void toggle_chat (bool chat_open)
		{
			if (!chat_open && chatopen_persist)
			{
				return;
			}

			chatopen = chat_open;

			if (!chatopen && chatfieldopen)
			{
				toggle_chatfield ();
			}

			buttons[(int)Buttons.BT_OPENCHAT].set_active (!chat_open);
			buttons[(int)Buttons.BT_CLOSECHAT].set_active (chat_open);

			for (uint i = 0; i < (ulong)ChatTab.NUM_CHATTAB; i++)
			{
				buttons[(int)Buttons.BT_TAB_0 + i].set_active (chat_open);
			}

			buttons[(uint)((int)Buttons.BT_TAB_0 + ChatTab.NUM_CHATTAB)].set_active (chat_open);
		}

		public void toggle_chatfield ()
		{
			chatfieldopen = !chatfieldopen;
			toggle_chatfield (chatfieldopen);
		}

		public void toggle_chatfield (bool chatfield_open)
		{
			chatfieldopen = chatfield_open;

			toggle_chat (chatfieldopen);

			if (chatfieldopen)
			{
				buttons[(int)Buttons.BT_CHAT].set_active (true);
				buttons[(int)Buttons.BT_HELP].set_active (true);
				buttons[(int)Buttons.BT_LINK].set_active (true);
				buttons[(int)Buttons.BT_CHAT_TARGET].set_active (true);

				chatfield.set_state (Textfield.State.FOCUSED);

				//dimension.shift_y(getchatbarheight());
			}
			else
			{
				buttons[(int)Buttons.BT_CHAT].set_active (false);
				buttons[(int)Buttons.BT_HELP].set_active (false);
				buttons[(int)Buttons.BT_LINK].set_active (false);
				buttons[(int)Buttons.BT_CHAT_TARGET].set_active (false);

				chatfield.set_state (Textfield.State.DISABLED);
				chatfield.change_text ("");

				//dimension.set_y(DIMENSION_Y);
			}
		}

		public bool is_chatopen ()
		{
			return chatopen;
		}

		public bool is_chatfieldopen ()
		{
			return chatfieldopen;
		}

		public override Button.State button_pressed (ushort buttonid)
		{
			switch ((Buttons)buttonid)
			{
				case Buttons.BT_OPENCHAT:
				case Buttons.BT_CLOSECHAT:
					toggle_chat ();
					break;
				case Buttons.BT_TAB_0:
				case Buttons.BT_TAB_1:
				case Buttons.BT_TAB_2:
				case Buttons.BT_TAB_3:
				case Buttons.BT_TAB_4:
				case Buttons.BT_TAB_5:
					for (uint i = 0; i < (ulong)ChatTab.NUM_CHATTAB; i++)
					{
						buttons[(int)Buttons.BT_TAB_0 + i].set_state (Button.State.NORMAL);
						chattab_text[(int)ChatTab.CHT_ALL + i].change_color (Color.Name.DUSTYGRAY);
					}

					chattab_text[(int)(buttonid - Buttons.BT_TAB_0)].change_color (Color.Name.WHITE);

					return Button.State.PRESSED;
			}

			Setting<Chatopen>.get ().save (chatopen);

			return Button.State.NORMAL;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool indragrange(Point<short> cursorpos) const override
public override bool indragrange (Point<short> cursorpos)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: var bounds = getbounds(dragarea);
			var bounds = getbounds (dragarea);

			return bounds.contains (cursorpos);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short getchattop(bool chat_open) const
		private short getchattop (bool chat_open)
		{
			if (chat_open)
			{
				return (short)(getchatbarheight () * -1);
			}
			else
			{
				return -1;
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short getchatbarheight() const
		private short getchatbarheight ()
		{
			return (short)(15 + chatrows * CHATROWHEIGHT);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Rectangle<short> getbounds(Point<short> additional_area) const
		private Rectangle<short> getbounds (Point<short> additional_area)
		{
			short screen_adj = (short)((chatopen) ? 35 : 16);

			var absp = position + new Point<short> (0, getchattop (chatopen));
			var da = absp + additional_area;

			absp = new Point<short> (absp.x (), (short)(absp.y () - screen_adj));
			da = new Point<short> (da.x (), da.y ());

			return new Rectangle<short> (absp, da);
		}

		private const short CHATROWHEIGHT = 13;
		private const short MINCHATROWS = 1;
		private const short MAXCHATROWS = 16;
		private const short DIMENSION_Y = 17;
		private const long MESSAGE_COOLDOWN = 1_000;

		private enum Buttons : ushort
		{
			BT_OPENCHAT,
			BT_CLOSECHAT,
			BT_CHAT,
			BT_HELP,
			BT_LINK,
			BT_TAB_0,
			BT_TAB_1,
			BT_TAB_2,
			BT_TAB_3,
			BT_TAB_4,
			BT_TAB_5,
			BT_TAB_ADD,
			BT_CHAT_TARGET
		}

		private enum ChatTab
		{
			CHT_ALL,
			CHT_BATTLE,
			CHT_PARTY,
			CHT_FRIEND,
			CHT_GUILD,
			CHT_ALLIANCE,
			NUM_CHATTAB
		}

		private List<string> ChatTabText = new List<string> () {"All", "Battle", "Party", "Friend", "Guild", "Alliance"};

		private bool chatopen;
		private bool chatopen_persist;
		private bool chatfieldopen;
		private Texture[] chatspace =new Texture[4];
		private Texture chatenter = new Texture ();
		private Texture chatcover = new Texture ();
		private Textfield chatfield = new Textfield ();
		private Point<short> closechat = new Point<short> ();

		private Text[] chattab_text =new Text[(int)UIChatBar.ChatTab.NUM_CHATTAB];
		private short chattab_x;
		private short chattab_y;
		private short chattab_span;

		private Slider slider = new Slider ();

		private EnumMap<Messages.Type, long> message_cooldowns = new EnumMap<Messages.Type, long> ();
		private List<string> lastentered = new List<string> ();
		private uint lastpos;

		private short chatrows;
		private short rowpos;
		private short rowmax;
		private Dictionary<short, Text> rowtexts = new Dictionary<short, Text> ();

		private bool dragchattop;
	}
}


#if USE_NX
#endif