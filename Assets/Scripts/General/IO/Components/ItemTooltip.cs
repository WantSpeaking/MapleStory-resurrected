#define USE_NX

using MapleLib.WzLib;




namespace ms
{
	public class ItemTooltip : Tooltip
	{
		// TODO: Add blue dot next to name
		public ItemTooltip ()
		{
			/*WzObject Item = ms.wz.wzFile_ui["UIToolTip.img"]["Item"];
			WzObject Frame = Item["Frame2"];
			WzObject ItemIcon = Item["ItemIcon"];

			frame = new MapleFrame (Frame);
			cover = Frame["cover"];
			baseTexture = ItemIcon["base"];
			itemcover = ItemIcon["cover"];
			type[true] = ItemIcon["new"];
			type[false] = ItemIcon["old"];

			itemid = 0;*/
		}

		public override void draw (Point_short pos)
		{
			/*if (itemid == 0)
			{
				return;
			}

			short max_width = Constants.get ().get_viewwidth ();
			short max_height = Constants.get ().get_viewheight ();
			short cur_width = (short)(pos.x () + fillwidth + 32);
			short cur_height = (short)(pos.y () + fillheight + 40);

			int adj_x = cur_width - max_width;
			short adj_y = (short)(cur_height - max_height);

			short adj_d = (short)(descdelta > 0 ? descdelta : 0);
			short adj_t = (short)((untradable || unique) ? 19 : 0);

			if (adj_x > 0)
			{
				pos.shift_x ((short)(adj_x * -1));
			}

			if (adj_y > 0)
			{
				pos.shift_y ((short)(adj_y * -1));
			}

			frame.draw (pos + new Point_short (150, (short)(118 + adj_d + adj_t)), fillwidth, (short)(fillheight + adj_t));
			cover.draw (pos + new Point_short (4, 4));
			name.draw (pos + new Point_short (22, 8));

			if (untradable || unique)
			{
				qual.draw (pos + new Point_short (148, 27));
			}

			pos.shift (14, (short)(18 + name.height () + adj_t));

			baseTexture.draw (pos);
			type[true].draw (pos);
			itemicon.draw (new DrawArgument (pos + new Point_short (8, 72), 2.0f, 2.0f));
			itemcover.draw (pos);
			desc.draw (pos + new Point_short (90, -6));*/
		}

		public bool set_item (int iid)
		{
/*			if (itemid == iid)
			{
				return false;
			}*/

			itemid = iid;

			if (itemid == 0)
			{
				return false;
			}

			ItemData idata = ItemData.get (itemid);

			itemicon = new Texture (idata.get_icon (false));
			untradable = idata.is_untradable ();
			unique = idata.is_unique ();

			string quality = "";

			if (unique && untradable)
			{
				quality = TestLuban.Get().GetL10nText("One-of-a-kind Item, Untradable");
			}
			else if (unique && !untradable)
			{
				quality = TestLuban.Get().GetL10nText("One-of-a-kind Item");
			}
			else if (!unique && untradable)
			{
				quality = TestLuban.Get().GetL10nText("Untradable");
			}
			else
			{
				quality = "";
			}

			name = new Text (Text.Font.A12B, Text.Alignment.LEFT, Color.Name.WHITE, idata.get_name (), 240);
			desc = new Text (Text.Font.A12M, Text.Alignment.LEFT, Color.Name.WHITE, idata.get_desc (), 185);
			qual = new Text (Text.Font.A12M, Text.Alignment.CENTER, Color.Name.ORANGE, quality, 185);

			fillwidth = 264;
			fillheight = (short)(83 + name.height ());
			descdelta = (short)(desc.height () - 80);

			if (descdelta > 0)
			{
				fillheight += descdelta;
			}

			return true;
		}
		public void clear_set_item ()
		{
			itemid = 0;
		}
		public int itemid;
		public short fillwidth;
		public short fillheight;
		public short descdelta;
		public Texture itemicon = new Texture ();
		
		public Text name = new Text ();
		public Text desc = new Text ();
		public Text qual = new Text ();
		public MapleFrame frame = new MapleFrame ();
		public Texture cover = new Texture ();
		public Texture baseTexture = new Texture ();
		public Texture itemcover = new Texture ();
		public BoolPairNew<Texture> type = new BoolPairNew<Texture> ();
		public bool untradable;
		public bool unique;
	}
}


#if USE_NX
#endif