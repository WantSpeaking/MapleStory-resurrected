#define USE_NX

using System;
using Beebyte.Obfuscator;

namespace ms
{
	[Skip]
	public class UILoginWait : UIElement
	{
		public const Type TYPE = UIElement.Type.LOGINWAIT;
		public const bool FOCUSED = true;
		public const bool TOGGLED = false;

		public UILoginWait () : this ((Action)null)
		{
		}

		public UILoginWait (params object[] args) : this ((System.Action)args[0])
		{
		}

		public UILoginWait (System.Action okhandler)
		{
			this.okhandler = okhandler;
			var Loading = ms.wz.wzProvider_ui["Login.img"]["Notice"]["Loading"];
			var backgrnd = Loading["backgrnd"];

			sprites.Add (backgrnd);
			sprites.Add (new Sprite(Loading["circle"], new Point_short (127, 70)));

			buttons[(uint)Buttons.CANCEL] = new MapleButton (Loading["BtCancel"], new Point_short (101, 106));

			position = new Point_short (276, 229);
			dimension = new Texture (backgrnd).get_dimensions ();
		}

		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public void close ()
		{
			deactivate ();
			okhandler ();
		}

		public System.Action get_handler ()
		{
			return okhandler;
		}

		public override Button.State button_pressed (ushort id)
		{
			Session.get ().reconnect ();

			close ();

			return Button.State.NORMAL;
		}

		private enum Buttons : ushort
		{
			CANCEL
		}

		private System.Action okhandler;
	}
}