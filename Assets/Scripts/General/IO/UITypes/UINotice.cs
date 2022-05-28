#define USE_NX

using System;
using MapleLib.WzLib;




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

		protected UINotice (string message, NoticeType t, Text.Alignment a)
		{
			//this.UIDragElement<PosNOTICE> = new <type missing>();
			this.type = t;
			this.alignment = a;
			WzObject src = ms.wz.wzFile_ui["Basic.img"]["Notice6"];

			top = src["t"];
			center = src["c"];
			centerbox = src["c_box"];
			box = src["box"];
			box2 = src["box2"];
			bottom = src["s"];
			bottombox = src["s_box"];

			if (type == NoticeType.YESNO)
			{
				position.shift_y (-8);
				question = new Text (Text.Font.A11M, alignment, Color.Name.WHITE, message, 200);
			}
			else if (type == NoticeType.ENTERNUMBER)
			{
				position.shift_y (-16);
				question = new Text (Text.Font.A12M, Text.Alignment.LEFT, Color.Name.WHITE, message, 200);
			}
			else if (type == NoticeType.OK)
			{
				ushort maxwidth = (ushort)(top.width () - 6);

				position.shift_y (-8);
				question = new Text (Text.Font.A11M, Text.Alignment.CENTER, Color.Name.WHITE, message, maxwidth);
			}

			height = question.height ();
			dimension = new Point_short (top.width (), (short)(top.height () + height + bottom.height ()));
			position = new Point_short ((short)(position.x () - dimension.x () / 2), (short)(position.y () - dimension.y () / 2));
			dragarea = new Point_short (dimension.x (), 20);

			if (type != NoticeType.ENTERNUMBER)
			{
				new Sound (Sound.Name.DLGNOTICE).play ();
			}
		}

		protected UINotice (string message, NoticeType t) : this (message, t, Text.Alignment.CENTER)
		{
		}

		protected void draw (bool textfield)
		{
			Point_short start = new Point_short (position);

			top.draw (start);
			start.shift_y (top.height ());

			if (textfield)
			{
				/*center.draw (start);
				start.shift_y (center.height ());*/
				centerbox.draw (start);
				start.shift_y ((short)(centerbox.height () - 1));
				box2.draw (start);
				start.shift_y (box2.height ());
				box.draw (new DrawArgument (new Point_short (start), new Point_short (0, 29)));
				start.shift_y (29);

				question.draw (position + new Point_short (13, 13));
			}
			else
			{
				short pos_y = (short)(height >= 32 ? height : 32);

				center.draw (new DrawArgument (new Point_short (start), new Point_short (0, pos_y)));
				start.shift_y (pos_y);
				centerbox.draw (start);
				start.shift_y (centerbox.height ());
				box.draw (start);
				start.shift_y (box.height ());

				if (type == NoticeType.YESNO && alignment == Text.Alignment.LEFT)
				{
					question.draw (position + new Point_short (31, 14));
				}
				else
				{
					question.draw (position + new Point_short (10, 14));
				}
			}

			bottombox.draw (start);
		}

		protected short box2offset (bool textfield)
		{
			short offset = (short)(top.height () + centerbox.height () + box.height () + height - (textfield ? 0 : 16));

			if (type == NoticeType.OK)
			{
				if (height < 34)
				{
					offset += 15;
				}
			}

			return offset;
		}

		protected short numfieldTop ()
		{
			return (short)(top.height () + centerbox.height ());
		}
		private Texture top = new Texture ();
		private Texture center = new Texture ();
		private Texture centerbox = new Texture ();
		private Texture box = new Texture ();
		private Texture box2 = new Texture ();
		private Texture bottom = new Texture ();
		private Texture bottombox = new Texture ();
		private Text question = new Text ();
		private short height;
		private NoticeType type;
		private Text.Alignment alignment;
	}

	public class UIYesNo : UINotice
	{
		public UIYesNo (params object[] args) : this ((string)args[0], (Action<bool>)args[1], (Text.Alignment)args[2])
		{
		}

		public UIYesNo (string message, System.Action<bool> yh, Text.Alignment alignment) : base (message, NoticeType.YESNO, alignment)
		{
			yesnohandler = yh;

			short belowtext = box2offset (false);

			WzObject src = ms.wz.wzFile_ui["Basic.img"];

			buttons[(int)Buttons.YES] = new MapleButton (src["BtOK4"], new Point_short (156, belowtext));
			buttons[(int)Buttons.NO] = new MapleButton (src["BtCancel4"], new Point_short (198, belowtext));
		}

		public UIYesNo (string message, System.Action<bool> yesnohandler) : this (message, yesnohandler, Text.Alignment.CENTER)
		{
		}

		public override void draw (float alpha)
		{
			base.draw (false);
			base.draw (alpha);
		}

		public override void send_key (int keycode, bool pressed, bool escape)
		{
			if (keycode == (int)KeyAction.Id.RETURN)
			{
				yesnohandler?.Invoke (true);
				deactivate ();
			}
			else if (escape)
			{
				yesnohandler?.Invoke (false);
				deactivate ();
			}
		}

		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public override Button.State button_pressed (ushort buttonid)
		{
			deactivate ();

			switch ((Buttons)buttonid)
			{
				case Buttons.YES:
					yesnohandler?.Invoke (true);
					break;
				case Buttons.NO:
					yesnohandler?.Invoke (false);
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
		public UIEnterNumber (params object[] args) : this ((string)args[0], (Action<int>)args[1], (int)args[2], (int)args[3])
		{
		}

		public UIEnterNumber (string message, System.Action<int> nh, int m, int quantity) : base (message, NoticeType.ENTERNUMBER)
		{
			numhandler = nh;
			max = m;

			short belowtext = (short)(box2offset (true) - 21);
			short pos_y = (short)(belowtext + 35);

			WzObject src = ms.wz.wzFile_ui["Basic.img"];

			buttons[(int)Buttons.OK] = new MapleButton (src["BtOK4"], 156, pos_y);
			buttons[(int)Buttons.CANCEL] = new MapleButton (src["BtCancel4"], 198, pos_y);

			numfield = new Textfield (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.BLACK, new Rectangle_short (24, 232, numfieldTop(), (short)(belowtext + 20)), 10);
			numfield.change_text (Convert.ToString (quantity));

			numfield.set_enter_callback ((string numstr) => { handlestring (numstr); });

			numfield.set_key_callback (KeyAction.Id.ESCAPE, () => { deactivate (); });

			numfield.set_state (Textfield.State.FOCUSED);
		}

		public override void draw (float alpha)
		{
			base.draw (true);
			base.draw (alpha);

			numfield.draw (new Point_short (position));
		}

		public override void update ()
		{
			base.update ();

			numfield.update (new Point_short (position));
		}

		public override Cursor.State send_cursor (bool clicked, Point_short cursorpos)
		{
			if (numfield.get_state () == Textfield.State.NORMAL)
			{
				Cursor.State nstate = numfield.send_cursor (new Point_short (cursorpos), clicked);

				if (nstate != Cursor.State.IDLE)
				{
					return nstate;
				}
			}

			return base.send_cursor (clicked, new Point_short (cursorpos));
		}

		public override void send_key (int keycode, bool pressed, bool escape)
		{
			if (keycode == (int)KeyAction.Id.RETURN)
			{
				handlestring (numfield.get_text ());
				deactivate ();
			}
			else if (escape)
			{
				deactivate ();
			}
		}

		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public override Button.State button_pressed (ushort buttonid)
		{
			switch ((Buttons)buttonid)
			{
				case Buttons.OK:
					handlestring (numfield.get_text ());
					break;
				case Buttons.CANCEL:
					deactivate ();
					break;
			}

			return Button.State.NORMAL;
		}

		private void handlestring (string numstr)
		{
			int num = -1;
			bool has_only_digits = (numstr.find_first_not_of ("0123456789") == -1);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Lambda expressions cannot be assigned to 'var':
			Action<bool> okhandler = (bool UnnamedParameter1) =>
			{
				numfield.set_state (Textfield.State.FOCUSED);
				buttons[(int)Buttons.OK].set_state (Button.State.NORMAL);
			};

			if (!has_only_digits)
			{
				numfield.set_state (Textfield.State.DISABLED);
				UI.get ().emplace<UIOk> ("Only numbers are allowed.", okhandler);
				return;
			}
			else
			{
				num = Convert.ToInt32 (numstr);
			}

			if (num < 1)
			{
				numfield.set_state (Textfield.State.DISABLED);
				UI.get ().emplace<UIOk> ("You may only enter a number equal to or higher than 1.", okhandler);
				return;
			}
			else if (num > max)
			{
				numfield.set_state (Textfield.State.DISABLED);
				UI.get ().emplace<UIOk> ("You may only enter a number equal to or lower than " + Convert.ToString (max) + ".", okhandler);
				return;
			}
			else
			{
				numhandler (num);
				deactivate ();
			}

			buttons[(int)Buttons.OK].set_state (Button.State.NORMAL);
		}

		private enum Buttons : short
		{
			OK,
			CANCEL
		}

		private System.Action<int> numhandler;
		private Textfield numfield = new Textfield ();
		private int max;
	}

	public class UIOk : UINotice
	{
		public UIOk (params object[] args) : this ((string)args[0], (Action<bool>)args[1])
		{
		}

		public UIOk (string message, System.Action<bool> oh) : base (message, NoticeType.OK)
		{
			okhandler = oh;

			WzObject src = ms.wz.wzFile_ui["Basic.img"];

			buttons[(int)Buttons.OK] = new MapleButton (src["BtOK4"], 197, box2offset (false));
		}

		public override void draw (float alpha)
		{
			base.draw (false);
			base.draw (alpha);
		}

		public override void send_key (int keycode, bool pressed, bool escape)
		{
			if (pressed)
			{
				if (keycode == (int)KeyAction.Id.RETURN)
				{
					okhandler (true);
					deactivate ();
				}
				else if (escape)
				{
					okhandler (false);
					deactivate ();
				}
			}
		}

		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public override Button.State button_pressed (ushort buttonid)
		{
			deactivate ();

			switch ((Buttons)buttonid)
			{
				case Buttons.OK:
					okhandler?.Invoke (true);
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