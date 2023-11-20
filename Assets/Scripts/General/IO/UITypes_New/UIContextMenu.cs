#define USE_NX

using System;
using MapleLib.WzLib;
using provider;

namespace ms
{
	[Beebyte.Obfuscator.Skip]
	public class UIContextMenu : UIElement
	{
		public const Type TYPE = UIElement.Type.ContextMenu;
		public const bool FOCUSED = true;
		public const bool TOGGLED = false;


		public UIContextMenu ()
		{
			MapleData src = ms.wz.wzProvider_ui["UIWindow.img"]["ContextMenu"];

			top = src["t"];
			center = src["c"];
			bottom = src["s"];

			short shiftPosY = top.height ();
			buttons.Add ((uint)Buttons.BtParty, new MapleButton (src["BtParty"], 3 + 8, shiftPosY));
			buttons.Add ((uint)Buttons.BtTrade, new MapleButton (src["BtTrade"], 11, (short)(shiftPosY + buttonHeight)));

			dimension = new Point_short (top.width (), (short)(top.height () + centerHeight + bottom.height ()));
		}


		public void SetDisplayInfo (Point_short displayPos, OtherChar clickedChar)
		{
			//height = question.height ();
			//dimension = new Point_short (top.Width (), (short)(top.height () + height + bottom.height ()));
			position = new Point_short (displayPos); /*+ new Point_short ((short)(position.x () - dimension.x () / 2), (short)(position.y () - dimension.y () / 2))*/
			position.shift_y ((short)-(top.height () - 4));
			rightClickedChar = clickedChar;

			//if (type != NoticeType.ENTERNUMBER)
			{
				new Sound (Sound.Name.DLGNOTICE).play ();
			}
		}

		public override void draw (float alpha)
		{
			Point_short start = new Point_short (position);

			top.draw (start);
			start.shift_x (3);
			start.shift_y (top.height ());
			center.draw (new DrawArgument (new Point_short (start), new Point_short (0, centerHeight)));
			start.shift_y (centerHeight);
			bottom.draw (start);


			base.draw (alpha);
		}

		public override Button.State button_pressed (ushort buttonid)
		{
			switch ((Buttons)buttonid)
			{
				case Buttons.BtParty:
					OnBtPartyClick ();
					break;
				case Buttons.BtTrade:
					OnBtTradeClick ();
					break;
			}

			return Button.State.NORMAL;
		}

		public override Cursor.State send_cursor (bool down, Point_short pos)
		{
			var state = base.send_cursor (down, pos);
			if (down) deactivate ();
			return state;
		}
		
		public void remove_menus ()
		{
			deactivate ();
		}

		public override Type get_type ()
		{
			return TYPE;
		}

		private Char rightClickedChar;

		private void OnBtPartyClick ()
		{
			if (rightClickedChar != null)
			{
				//new CreatePartyPacket ().dispatch ();
				new InviteToPartyPacket (rightClickedChar.get_name ()).dispatch ();
			}
		}

		private void OnBtTradeClick ()
		{
		}

		private const short buttonHeight = 16;

		private short centerHeight => (short)(buttons.Count * buttonHeight);

		private Texture top = new Texture ();
		private Texture center = new Texture ();
		private Texture bottom = new Texture ();
		private Text question = new Text ();
		private Text.Alignment alignment;

		private enum Buttons : uint
		{
			BtParty,
			BtTrade,
		}

		public override void Dispose ()
		{
			base.Dispose ();
			top?.Dispose ();
			center?.Dispose ();
			bottom?.Dispose ();
		}
	}
}