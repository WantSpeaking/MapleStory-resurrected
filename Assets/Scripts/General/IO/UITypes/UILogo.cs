#define USE_NX

using Beebyte.Obfuscator;
using MapleLib.WzLib;




namespace ms
{
	[Skip]
	public class UILogo : UIElement
	{
		public const Type TYPE = UIElement.Type.START;
		public const bool FOCUSED = false;
		public const bool TOGGLED = false;

		public UILogo() : base(new Point_short(0, 0), new Point_short(800, 600))
		{
			Music.get().play_once("BgmUI.img/NxLogo");

			nexon_ended = false;
			wizet_ended = false;
			user_clicked = false;

			WzObject Logo = ms.wz.wzFile_ui["Logo.img"];

			Nexon = Logo["Nexon"];
			Wizet = Logo["Wizet"];
			WizetEnd = Logo["Wizet"]["40"];
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float inter) const override
		public override void draw(float inter)
		{
			if (!user_clicked)
			{
				if (!nexon_ended)
				{
					Nexon.draw(position + new Point_short(440, 360), inter);
				}
				else
				{
					if (!wizet_ended)
					{
						Wizet.draw(position + new Point_short(263, 195), inter);
					}
					else
					{
						WizetEnd.draw(position + new Point_short(263, 195));
					}
				}
			}
			else
			{
				WizetEnd.draw(position + new Point_short(263, 195));
			}
		}
		public override void update()
		{
			if (!nexon_ended)
			{
				nexon_ended = Nexon.update();
			}
			else
			{
				if (!wizet_ended)
				{
					wizet_ended = Wizet.update();
				}
				else
				{
					Configuration.get().set_start_shown(true);

					UI.get().remove(UIElement.Type.START);
					UI.get().emplace<UILogin>();
				}
			}
		}

		public override Cursor.State send_cursor(bool clicked, Point_short cursorpos)
		{
			Cursor.State ret = clicked ? Cursor.State.CLICKING : Cursor.State.IDLE;

			if (clicked && !user_clicked)
			{
				user_clicked = true;
			}

			return ret;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type()
		{
			return TYPE;
		}

		private Animation Nexon = new Animation();
		private Animation Wizet = new Animation();
		private Texture WizetEnd = new Texture();

		private bool nexon_ended;
		private bool wizet_ended;
		private bool user_clicked;
	}
}




#if USE_NX
#endif
