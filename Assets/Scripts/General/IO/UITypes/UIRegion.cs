#define USE_NX

using Beebyte.Obfuscator;
using MapleLib.WzLib;




namespace ms
{
	[Skip]
	public class UIRegion : UIElement
	{
		public const Type TYPE = UIElement.Type.REGION;
		public const bool FOCUSED = false;
		public const bool TOGGLED = false;

		public UIRegion() : base(new Point_short(0, 0), new Point_short(800, 600))
		{
			WzObject Common = ms.wz.wzFile_ui["Login.img"]["Common"];
			WzObject frame = ms.wz.wzFile_mapLatest["Obj"]["login.img"]["Common"]["frame"]["2"]["0"];
			WzObject Gateway = ms.wz.wzFile_ui["Gateway.img"]["WorldSelect"];
			WzObject na = Gateway["BtButton0"];
			WzObject eu = Gateway["BtButton1"];

			sprites.Add(Gateway["backgrnd2"]);
			sprites.Add(new Sprite (frame, new Point_short(400, 300)));
			sprites.Add(new Sprite (Common["frame"], new Point_short(400, 300)));

			short pos_y = 84;
			Point_short na_pos = new Point_short(155, pos_y);
			Point_short eu_pos = new Point_short(424, pos_y);

			buttons[(int)Buttons.NA] = new MapleButton(na, na_pos);
			buttons[(int)Buttons.EU] = new MapleButton(eu, eu_pos);
			buttons[(int)Buttons.EXIT] = new MapleButton(Common["BtExit"], new Point_short(0, 540));

			Point_short na_dim = new Texture(na["normal"]["0"]).get_dimensions();
			Point_short eu_dim = new Texture(eu["normal"]["0"]).get_dimensions();

			na_rect = new Rectangle_short(new Point_short (na_pos), na_pos + na_dim);
			eu_rect = new Rectangle_short(new Point_short (eu_pos), eu_pos + eu_dim);
		}

		public override Cursor.State send_cursor(bool clicked, Point_short cursorpos)
		{
			clear_tooltip();

			if (na_rect.contains(cursorpos))
			{
				UI.get().show_text(Tooltip.Parent.TEXT, "Warning: You may experience latency and connection issues when connecting to the NA server from Europe.");
			}

			if (eu_rect.contains(cursorpos))
			{
				UI.get().show_text(Tooltip.Parent.TEXT, "Warning: You may experience latency and connection issues when connecting to the EU server from North America.");
			}

			return base.send_cursor(clicked, new Point_short (cursorpos));
		}

		public override UIElement.Type get_type()
		{
			return TYPE;
		}

		public override Button.State button_pressed(ushort buttonid)
		{
			clear_tooltip();

			switch ((Buttons)buttonid)
			{
				case Buttons.NA:
				case Buttons.EU:
				{
					// TODO: Update UIWorldSelect after selecting new region
					//uint8_t region = (buttonid == Buttons::NA) ? 5 : 6;
					var worldselect = UI.get ().get_element<UIWorldSelect> ();
					if (worldselect)
					{
						UI.get().remove(UIElement.Type.REGION);

						//worldselect->set_region(region);
						worldselect.get ().makeactive();
					}

					break;
				}
				case Buttons.EXIT:
				{
					UI.get().quit();
					break;
				}
				default:
				{
					break;
				}
			}

			return Button.State.NORMAL;
		}

		private void clear_tooltip()
		{
			UI.get().clear_tooltip(Tooltip.Parent.TEXT);
		}

		private enum Buttons : ushort
		{
			NA,
			EU,
			EXIT
		}

		private Rectangle_short na_rect = new Rectangle_short();
		private Rectangle_short eu_rect = new Rectangle_short();
	}
}




#if USE_NX
#endif
