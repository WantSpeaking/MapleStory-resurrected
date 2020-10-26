#define USE_NX

using System;
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
	public abstract class UINotice : UIDragElement<PosNOTICE>
	{
		public const Type TYPE = UIElement.Type.NOTICE;
		public const bool FOCUSED = true;
		public const bool TOGGLED = false;

		protected enum NoticeType : byte
		{
			YESNO,
			ENTERNUMBER,
			OK
		}

		protected UINotice(string message, NoticeType t, Text.Alignment a)
		{
			//this.UIDragElement<PosNOTICE> = new <type missing>();
			this.type = t;
			this.alignment = a;
			WzObject src = nl.nx.wzFile_ui["Basic.img"]["Notice6"];

			top = src["t"];
			center = src["c"];
			centerbox = src["c_box"];
			box = src["box"];
			box2 = src["box2"];
			bottom = src["s"];
			bottombox = src["s_box"];

			if (type == NoticeType.YESNO)
			{
				position.shift_y(-8);
				question = new Text(Text.Font.A11M, alignment, Color.Name.WHITE, message, 200);
			}
			else if (type == NoticeType.ENTERNUMBER)
			{
				position.shift_y(-16);
				question = new Text(Text.Font.A12M, Text.Alignment.LEFT, Color.Name.WHITE, message, 200);
			}
			else if (type == NoticeType.OK)
			{
				ushort maxwidth = (ushort)(top.width() - 6);

				position.shift_y(-8);
				question = new Text(Text.Font.A11M, Text.Alignment.CENTER, Color.Name.WHITE, message, maxwidth);
			}

			height = question.height();
			dimension = new Point<short>(top.width(), (short)(top.height() + height + bottom.height()));
			position = new Point<short>((short)(position.x() - dimension.x() / 2), (short)(position.y() - dimension.y() / 2));
			dragarea = new Point<short>(dimension.x(), 20);

			if (type != NoticeType.ENTERNUMBER)
			{
				new Sound(Sound.Name.DLGNOTICE).play();
			}
		}
		protected UINotice(string message, NoticeType t) : this(message, t, Text.Alignment.CENTER)
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(bool textfield) const
		protected void draw(bool textfield)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: Point<short> start = position;
			Point<short> start = position;

			top.draw(start);
			start.shift_y(top.height());

			if (textfield)
			{
				center.draw(start);
				start.shift_y(center.height());
				centerbox.draw(start);
				start.shift_y((short)(centerbox.height() - 1));
				box2.draw(start);
				start.shift_y(box2.height());
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: box.draw(DrawArgument(start, Point<short>(0, 29)));
				box.draw(new DrawArgument(start, new Point<short>(0, 29)));
				start.shift_y(29);

				question.draw(position + new Point<short>(13, 13));
			}
			else
			{
				short pos_y = (short)(height >= 32 ? height : 32);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: center.draw(DrawArgument(start, Point<short>(0, pos_y)));
				center.draw(new DrawArgument(start, new Point<short>(0, pos_y)));
				start.shift_y(pos_y);
				centerbox.draw(start);
				start.shift_y(centerbox.height());
				box.draw(start);
				start.shift_y(box.height());

				if (type == NoticeType.YESNO && alignment == Text.Alignment.LEFT)
				{
					question.draw(position + new Point<short>(31, 14));
				}
				else
				{
					question.draw(position + new Point<short>(131, 14));
				}
			}

			bottombox.draw(start);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short box2offset(bool textfield) const
		protected short box2offset(bool textfield)
		{
			short offset = (short)(top.height() + centerbox.height() + box.height() + height - (textfield ? 0 : 16));

			if (type == NoticeType.OK)
			{
				if (height < 34)
				{
					offset += 15;
				}
			}

			return offset;
		}

		private Texture top = new Texture();
		private Texture center = new Texture();
		private Texture centerbox = new Texture();
		private Texture box = new Texture();
		private Texture box2 = new Texture();
		private Texture bottom = new Texture();
		private Texture bottombox = new Texture();
		private Text question = new Text();
		private short height;
		private NoticeType type;
		private Text.Alignment alignment;
	}

	public class UIYesNo : UINotice
	{
		public UIYesNo(string message, System.Action<bool> yh, Text.Alignment alignment) : base(message, NoticeType.YESNO, alignment)
		{
			yesnohandler = yh;

			short belowtext = box2offset(false);

			WzObject src = nl.nx.wzFile_ui["Basic.img"];

			buttons[(int)Buttons.YES] = new MapleButton(src["BtOK4"], new Point<short>(156, belowtext));
			buttons[(int)Buttons.NO] = new MapleButton(src["BtCancel4"], new Point<short>(198, belowtext));
		}
		public UIYesNo(string message, System.Action<bool> yesnohandler) : this(message, yesnohandler, Text.Alignment.CENTER)
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float alpha) const override
		public override void draw(float alpha)
		{
			base.draw(false);
			base.draw(alpha);
		}

		public override void send_key(int keycode, bool pressed, bool escape)
		{
			if (keycode == (int)KeyAction.Id.RETURN)
			{
				yesnohandler(true);
				deactivate();
			}
			else if (escape)
			{
				yesnohandler(false);
				deactivate();
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type()
		{
			return TYPE;
		}

		public override Button.State button_pressed(ushort buttonid)
		{
			deactivate();

			switch ((Buttons)buttonid)
			{
			case Buttons.YES:
				yesnohandler(true);
				break;
			case Buttons.NO:
				yesnohandler(false);
				break;
			}

			return Button.State.PRESSED;
		}

		private enum Buttons : short
		{
			YES,
			NO
		}

		private System.Action<bool> yesnohandler;
	}

	public class UIEnterNumber : UINotice
	{
		public UIEnterNumber(string message, System.Action<int> nh, int m, int quantity) : base(message, NoticeType.ENTERNUMBER)
		{
			numhandler = nh;
			max = m;

			short belowtext = (short)(box2offset(true) - 21);
			short pos_y = (short)(belowtext + 35);

			WzObject src = nl.nx.wzFile_ui["Basic.img"];

			buttons[(int)Buttons.OK] = new MapleButton(src["BtOK4"], 156, pos_y);
			buttons[(int)Buttons.CANCEL] = new MapleButton(src["BtCancel4"], 198, pos_y);

			numfield = new Textfield(Text.Font.A11M, Text.Alignment.LEFT, Color.Name.LIGHTGREY, new Rectangle<short>(24, 232, belowtext, (short)(belowtext + 20)), 10);
			numfield.change_text(Convert.ToString(quantity));

			numfield.set_enter_callback((string numstr) =>
			{
					handlestring(numstr);
			});

			numfield.set_key_callback(KeyAction.Id.ESCAPE, () =>
			{
					deactivate();
			});

			numfield.set_state(Textfield.State.FOCUSED);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float alpha) const override
		public override void draw(float alpha)
		{
			base.draw(true);
			base.draw(alpha);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: numfield.draw(position);
			numfield.draw(position);
		}
		public override void update()
		{
			base.update();

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: numfield.update(position);
			numfield.update(position);
		}

		public override Cursor.State send_cursor(bool clicked, Point<short> cursorpos)
		{
			if (numfield.get_state() == Textfield.State.NORMAL)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: Cursor::State nstate = numfield.send_cursor(cursorpos, clicked);
				Cursor.State nstate = numfield.send_cursor(cursorpos, clicked);

				if (nstate != Cursor.State.IDLE)
				{
					return nstate;
				}
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return UIElement::send_cursor(clicked, cursorpos);
			return base.send_cursor(clicked, cursorpos);
		}
		public override void send_key(int keycode, bool pressed, bool escape)
		{
			if (keycode == (int)KeyAction.Id.RETURN)
			{
				handlestring(numfield.get_text());
				deactivate();
			}
			else if (escape)
			{
				deactivate();
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type()
		{
			return TYPE;
		}

		public override Button.State button_pressed(ushort buttonid)
		{
			switch ((Buttons)buttonid)
			{
			case Buttons.OK:
				handlestring(numfield.get_text());
				break;
			case Buttons.CANCEL:
				deactivate();
				break;
			}

			return Button.State.NORMAL;
		}

		private void handlestring(string numstr)
		{
			int num = -1;
			bool has_only_digits = (numstr.find_first_not_of("0123456789") == -1);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Lambda expressions cannot be assigned to 'var':
			Action<bool> okhandler = (bool UnnamedParameter1) =>
			{
				numfield.set_state(Textfield.State.FOCUSED);
				buttons[(int)Buttons.OK].set_state(Button.State.NORMAL);
			};

			if (!has_only_digits)
			{
				numfield.set_state(Textfield.State.DISABLED);
				UI.get().emplace<UIOk>("Only numbers are allowed.", okhandler);
				return;
			}
			else
			{
				num = Convert.ToInt32(numstr);
			}

			if (num < 1)
			{
				numfield.set_state(Textfield.State.DISABLED);
				UI.get().emplace<UIOk>("You may only enter a number equal to or higher than 1.", okhandler);
				return;
			}
			else if (num > max)
			{
				numfield.set_state(Textfield.State.DISABLED);
				UI.get().emplace<UIOk>("You may only enter a number equal to or lower than " + Convert.ToString(max) + ".", okhandler);
				return;
			}
			else
			{
				numhandler(num);
				deactivate();
			}

			buttons[(int)Buttons.OK].set_state(Button.State.NORMAL);
		}

		private enum Buttons : short
		{
			OK,
			CANCEL
		}

		private System.Action<int> numhandler;
		private Textfield numfield = new Textfield();
		private int max;
	}

	public class UIOk : UINotice
	{
		public UIOk(string message, System.Action<bool> oh) : base(message, NoticeType.OK)
		{
			okhandler = oh;

			WzObject src = nl.nx.wzFile_ui["Basic.img"];

			buttons[(int)Buttons.OK] = new MapleButton(src["BtOK4"], 197, box2offset(false));
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float alpha) const override
		public override void draw(float alpha)
		{
			base.draw(false);
			base.draw(alpha);
		}

		public override void send_key(int keycode, bool pressed, bool escape)
		{
			if (pressed)
			{
				if (keycode == (int)KeyAction.Id.RETURN)
				{
					okhandler(true);
					deactivate();
				}
				else if (escape)
				{
					okhandler(false);
					deactivate();
				}
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type()
		{
			return TYPE;
		}

		public override Button.State button_pressed(ushort buttonid)
		{
			deactivate();

			switch ((Buttons)buttonid)
			{
			case Buttons.OK:
				okhandler(true);
				break;
			}

			return Button.State.NORMAL;
		}

		private enum Buttons : short
		{
			OK
		}

		private System.Action<bool> okhandler;
	}
}




#if USE_NX
#endif
