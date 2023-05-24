#define USE_NX

using System;
using MapleLib.WzLib;

namespace ms
{
	[Beebyte.Obfuscator.Skip]
	public class UIPartySideMenu : UIElement
	{
		public const Type TYPE = UIElement.Type.PartySideMenu;
		public const bool FOCUSED = true;
		public const bool TOGGLED = false;


		public UIPartySideMenu ()
		{
			WzObject src = ms.wz.wzFile_ui["UIWindow.img"]["UserList"]["Main"]["Party"]["SideMenu"];

			top = src["top"];
			center = src["center"];
			bottom = src["bottom"];

			short shiftPosY = top.height ();
			buttons.Add ((uint)Buttons.BtFriend, new MapleButton (src["BtFriend"], 3 + 8, shiftPosY));
			buttons.Add ((uint)Buttons.BtMaster, new MapleButton (src["BtMaster"], 11, (short)(shiftPosY + buttonHeight)));
			buttons.Add ((uint)Buttons.BtKickOut, new MapleButton (src["BtKickOut"], 11, (short)(shiftPosY + buttonHeight)));

			dimension = new Point_short (top.width (), (short)(top.height () + centerHeight + bottom.height ()));
		}

		private bool isPartyLeader;

		public void SetDisplayInfo (Point_short displayPos, MaplePartyCharacter clickedChar, int partyLeaderId)
		{
			//height = question.height ();
			//dimension = new Point_short (top.Width (), (short)(top.height () + height + bottom.height ()));
			position = new Point_short (displayPos); /*+ new Point_short ((short)(position.x () - dimension.x () / 2), (short)(position.y () - dimension.y () / 2))*/
			position.shift_y ((short)-(top.height () - 4));
			rightClickedChar = clickedChar;
			isPartyLeader = Stage.get ().is_player (partyLeaderId);

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

		protected override void draw_buttons (float alpha)
		{
			Point_short startRelativePos = new Point_short (7, top.height ());

			foreach (var iter in buttons)
			{
				if (iter.Value is MapleButton mapleButton)
				{
					if (iter.Key == (uint)Buttons.BtMaster || iter.Key == (uint)Buttons.BtKickOut) continue;
					mapleButton.set_position (startRelativePos);
					mapleButton.draw (position);

					startRelativePos.shift_y (buttonHeight);

				}
			}

			if (isPartyLeader)
			{
				buttons[(uint)Buttons.BtMaster].set_position (startRelativePos);
				buttons[(uint)Buttons.BtMaster].draw (position);
				startRelativePos.shift_y (buttonHeight);

				buttons[(uint)Buttons.BtKickOut].set_position (startRelativePos);
				buttons[(uint)Buttons.BtKickOut].draw (position);
				startRelativePos.shift_y (buttonHeight);
			}
		}

		public override Button.State button_pressed (ushort buttonid)
		{
			switch ((Buttons)buttonid)
			{
				case Buttons.BtFriend:
					OnBtFriendClick ();
					break;
				case Buttons.BtMaster:
					OnBtMasterClick ();
					break;
				case Buttons.BtKickOut:
					OnBtKickOutClick ();
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

		private MaplePartyCharacter rightClickedChar;

		private void OnBtFriendClick ()
		{
			if (rightClickedChar != null)
			{
				//new CreatePartyPacket ().dispatch ();
				//new InviteToPartyPacket (rightClickedChar.get_name ()).dispatch ();
			}
		}

		private void OnBtMasterClick ()
		{
			if (rightClickedChar == null) return;
			new ChangePartyLeaderPacket (rightClickedChar.id).dispatch ();
		}

		private void OnBtKickOutClick ()
		{
			if (rightClickedChar == null) return;
			new ExpelFromPartyPacket(rightClickedChar.id).dispatch ();
			//new ChangePartyLeaderPacket (rightClickedChar.id).dispatch ();
		}

		private const short buttonHeight = 14;

		private short centerHeight => (short)(buttons.Count * buttonHeight);

		private Texture top = new Texture ();
		private Texture center = new Texture ();
		private Texture bottom = new Texture ();
		private Text question = new Text ();
		private Text.Alignment alignment;

		private enum Buttons : uint
		{
			BtFriend,
			BtMaster,
			BtKickOut,
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