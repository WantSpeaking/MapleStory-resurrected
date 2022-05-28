using System.Collections.Generic;
using MapleLib.WzLib;
using ms;




namespace ms
{
	public abstract class FadeYesNoInfo_Base : UIElement
	{
		private enum Buttons : uint
		{
			BtOK,
			BtCancel
		}

		public FadeYesNoInfo_Base ()
		{
			short height = Constants.get ().get_viewheight ();
			short width = Constants.get ().get_viewwidth ();

			update_screen (width, height);

			wzObject_FadeYesNo = wz.wzFile_ui["UIWindow2.img"]["FadeYesNo"];

			BtOK = new MapleButton (wzObject_FadeYesNo["BtOK"]);
			BtCancel = new MapleButton (wzObject_FadeYesNo["BtCancel"]);

			buttons.Add ((uint)Buttons.BtOK, BtOK);
			buttons.Add ((uint)Buttons.BtCancel, BtCancel);

			text_line1 = new Text (Text.Font.A12M, Text.Alignment.LEFT, Color.Name.WHITE, "");
			text_line2 = new Text (Text.Font.A12M, Text.Alignment.LEFT, Color.Name.WHITE, "");
		}

		public override void update_screen (short new_width, short new_height)
		{
			short y_adj = (short)((new_width > 800) ? 37 : 0);

			//position = new Point_short ((short)(new_width - 200), (short)(new_height - 145 + y_adj));
		}

		public override Button.State button_pressed (ushort id)
		{
			switch ((Buttons)id)
			{
				case Buttons.BtOK:
					OnBtnOKPressed ();
					break;
				case Buttons.BtCancel:
					OnBtnCancelPressed ();
					break;
			}

			deactivate ();
			return Button.State.NORMAL;
		}

		public override void draw (float alpha)
		{
			base.draw (alpha);

			float interopc = opacity.get (alpha);

			text_line1.draw (position + new Point_short (25, 10));
			text_line2.draw (position + new Point_short (25, 30));
		}

		/*

		public virtual bool update ()
		{
			const float FADE_STEP = Constants.TIMESTEP * 1.0f / FADE_DURATION;
			opacity -= FADE_STEP;

			return opacity.last () < FADE_STEP;
		}*/


		protected virtual void OnBtnOKPressed ()
		{
		}

		protected virtual void OnBtnCancelPressed ()
		{
		}

		protected Text text_line1 = new Text ();
		protected Text text_line2 = new Text ();
		protected Linear_float opacity = new Linear_float ();

		protected Sprite backgrnd;
		protected Sprite icon;
		protected MapleButton BtOK;
		protected MapleButton BtCancel;

		protected WzObject wzObject_FadeYesNo;

		// 8 seconds
		private const long FADE_DURATION = 8_000;
	}

	public class UIFadeYesNo_PartyInvite : FadeYesNoInfo_Base
	{
		public const Type TYPE = UIElement.Type.FadeYesNo_PartyInvite;
		public const bool FOCUSED = true;
		public const bool TOGGLED = false;

		public UIFadeYesNo_PartyInvite ()
		{
			/*text = new Text (Text.Font.A12M, Text.Alignment.RIGHT, color, str);
			shadow = new Text (Text.Font.A12M, Text.Alignment.RIGHT, Color.Name.BLACK, str);

			opacity.set (1.0f);*/
			backgrnd = wzObject_FadeYesNo["backgrnd"];
			icon = new Sprite (wzObject_FadeYesNo["icon5"], new Point_short (5, 16));

			sprites.Add (backgrnd);
			sprites.Add (icon);

			BtOK.set_position (new Point_short (185, 10));
			BtCancel.set_position (new Point_short (185, 30));


			dimension = new Point_short (backgrnd.get_dimensions ());
			position = new Point_short ((short)(Constants.get ().get_viewwidth () - 214), (short)(Constants.get ().get_viewheight () - 110));

			text_line1.change_text ("Party invite");
		}

		private int _partyId;
		private Char _inviterChar;

		public void SetInviteInfo (int partyId, Char inviter)
		{
			_inviterChar = inviter;
			_partyId = partyId;

			text_line2.change_text ($"from '{_inviterChar.get_name ()}'");
			//if (type != NoticeType.ENTERNUMBER)
			{
				new Sound (Sound.Name.DLGNOTICE).play ();
			}
		}

		protected override void OnBtnOKPressed ()
		{
			base.OnBtnOKPressed ();
			new JoinPartyPacket (_partyId).dispatch ();
		}

		protected override void OnBtnCancelPressed ()
		{
			base.OnBtnCancelPressed ();
			new DENY_PARTY_REQUEST_Packet ($"{Stage.get ().get_player ().get_name ()}PS: {_inviterChar?.get_name ()}").dispatch ();
		}

		public override Cursor.State send_cursor (bool down, Point_short pos)
		{
			var state = base.send_cursor (down, pos);

			return state;
		}

		public override Type get_type ()
		{
			return TYPE;
		}
	}
}