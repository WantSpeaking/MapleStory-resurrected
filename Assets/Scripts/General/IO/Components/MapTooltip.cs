#define USE_NX

using MapleLib.WzLib;





namespace ms
{
	public class MapTooltip : Tooltip
	{
		public MapTooltip()
		{
			/*this.name = "";
			this.description = "";
			this.fillwidth = MIN_WIDTH;
			this.fillheight = 0;
			WzObject Frame = ms.wz.wzProvider_ui["UIToolTip.img"]["Item"]["Frame2"];
			WzObject WorldMap = ms.wz.wzProvider_ui["UIWindow2.img"]["ToolTip"]["WorldMap"];

			frame =new MapleFrame (Frame);
			cover = Frame["cover"];
			Mob = WorldMap["Mob"];
			Npc = WorldMap["Npc"];
			Party = WorldMap["Party"];*/
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point_short pos) const override
		public override void draw(Point_short pos)
		{
			/*if (name_label.empty())
			{
				return;
			}

			short max_width = Constants.get().get_viewwidth();
			short max_height = Constants.get().get_viewheight();

			if (parent == Tooltip.Parent.MINIMAP && mob_labels.Length == 0 && npc_labels.Length == 0)
			{
				if (desc_label.empty())
				{
					short new_width = name_simple.width();
					short new_height = name_simple.height();

					short cur_width = (short)(pos.x() + new_width + 21);
					short cur_height = (short)(pos.y() + new_height + 40);

					short adj_x = (short)(cur_width - max_width);
					short adj_y = (short)(cur_height - max_height);

					if (adj_x > 0)
					{
						pos.shift_x((short)(adj_x * -1));
					}

					if (adj_y > 0)
					{
						pos.shift_y((short)(adj_y * -1));
					}

					frame.draw(pos + new Point_short((short)(new_width / 2 + 2), (short)(new_height - 7)), (short)(new_width - 14), (short)(new_height - 18));

					if (new_height > 18)
					{
						name_simple.draw(pos);
					}
					else
					{
						name_simple.draw(pos + new Point_short(1, -3));
					}
				}
				else
				{
					short name_width = name_label.width();
					short name_height = name_label.height();

					short desc_width = desc_simple.width();

					short new_width = (name_width > desc_width) ? name_width : desc_width;
					short new_height = (short)(name_height + desc_simple.height() - 11);

					short cur_width = (short)(pos.x() + new_width + 0x15);
					short cur_height = (short)(pos.y() + new_height + 40);

					short adj_x = (short)(cur_width - max_width);
					int adj_y = cur_height - max_height;

					if (adj_x > 0)
					{
						pos.shift_x((short)(adj_x * -1));
					}

					if (adj_y > 0)
					{
						pos.shift_y((short)(adj_y * -1));
					}

					short half_width = (short)(new_width / 2);

					frame.draw(pos + new Point_short((short)(half_width + 2), (short)(new_height - 7 + BOTTOM_PADDING)), (short)(new_width - 14), (short)(new_height - 18 + BOTTOM_PADDING));
					cover.draw(pos + new Point_short(-5, -2));
					name_label.draw(pos + new Point_short(half_width, -2));

					pos.shift_y(name_height);

					separator.draw(pos + SEPARATOR_ADJ);
					desc_simple.draw(pos + new Point_short(half_width, -3));
				}
			}
			else
			{
				short cur_width = (short)(pos.x() + fillwidth + 21);
				short cur_height = (short)(pos.y() + fillheight + 40);

				short adj_x = (short)(cur_width - max_width);
				short adj_y = (short)(cur_height - max_height);

				if (adj_x > 0)
				{
					pos.shift_x((short)(adj_x * -1));
				}

				if (adj_y > 0)
				{
					pos.shift_y((short)(adj_y * -1));
				}

				short half_width = (short)(fillwidth / 2);

				frame.draw(pos + new Point_short((short)(half_width + 2), (short)(fillheight - 7 + BOTTOM_PADDING)), (short)(fillwidth - 14), (short)(fillheight - 18 + BOTTOM_PADDING));
				cover.draw(pos + new Point_short(-5, -2));
				name_label.draw(pos + new Point_short(half_width, 0));

				short name_height = name_label.height();

				if (!desc_label.empty())
				{
					pos.shift_y((short)(name_height + 4));

					desc_label.draw(pos + new Point_short(4, 0));

					pos.shift_y( (short)(desc_label.height() + BOTTOM_PADDING));
				}
				else
				{
					pos.shift_y((short)(name_height + BOTTOM_PADDING));
				}

				if (mob_labels.Length > 0)
				{
					separator.draw(pos + SEPARATOR_ADJ);

					for (uint i = 0; i < MAX_LIFE; i++)
					{
						if(mob_labels[i] == null) continue;
						if (!mob_labels[i].empty())
						{
							mob_labels[i].draw(pos + LIFE_LABEL_ADJ);

							if (i == 0)
							{
								Mob.draw(pos + LIFE_ICON_ADJ);
							}

							pos.shift_y(mob_labels[i].height());
						}
					}

					pos.shift_y(8);
				}

				if (npc_labels.Length > 0)
				{
					separator.draw(pos + SEPARATOR_ADJ);

					for (uint i = 0; i < MAX_LIFE; i++)
					{
						if(npc_labels[i] == null) continue;
						if (!npc_labels[i].empty())
						{
							npc_labels[i].draw(pos + LIFE_LABEL_ADJ);

							if (i == 0)
							{
								Npc.draw(pos + LIFE_ICON_ADJ);
							}

							pos.shift_y(npc_labels[i].height());
						}
					}
				}
			}*/
		}

		public void set_name(Tooltip.Parent p, string n, bool bolded)
		{
			if (name == n || parent == p)
			{
				return;
			}

			name = n;
			parent = p;

			if (string.IsNullOrEmpty(name) || (parent != Tooltip.Parent.WORLDMAP && parent != Tooltip.Parent.MINIMAP))
			{
				return;
			}

			name_label = new Text(bolded ? Text.Font.A12B : Text.Font.A12M, Text.Alignment.CENTER, Color.Name.WHITE, name);
			name_simple = new Text(bolded ? Text.Font.A12B : Text.Font.A12M, Text.Alignment.LEFT, Color.Name.WHITE, name);

			short width = name_label.width();
			short height = name_label.height();

			if (width > fillwidth)
			{
				fillwidth = width;
			}

			separator = new ColorLine((short)(fillwidth - 6), Color.Name.WHITE, 0.40f, false);

			if (height > fillheight)
			{
				fillheight = height;
			}
		}
		public void set_desc(string d)
		{
			if (description == d)
			{
				return;
			}

			description = d;

			if (string.IsNullOrEmpty(description))
			{
				return;
			}

			desc_label = new Text(Text.Font.A12M, Text.Alignment.LEFT, Color.Name.WHITE, description,0,true, fillwidth);
			desc_simple = new Text(Text.Font.A12M, Text.Alignment.CENTER, Color.Name.WHITE, description,0,true, fillwidth);

			fillwidth += 17;

			if (parent == Tooltip.Parent.MINIMAP)
			{
				short name_width = name_label.width();
				short desc_width = desc_simple.width();
				short new_width = (name_width > desc_width) ? name_width : desc_width;

				separator = new ColorLine((short)(new_width - 6), Color.Name.WHITE, 0.40f, false);
			}
			else
			{
				separator = new ColorLine((short)(fillwidth - 6), Color.Name.WHITE, 0.40f, false);
			}

			fillheight += (short)(desc_label.height () + 4);
		}
		public void set_mapid(int mapid)
		{
			uint m = 0;
			uint n = 0;
			var life = NxHelper.Map.get_life_on_map(mapid);

			foreach (var l in life)
			{
				var life_object = l.Value;

				if (life_object.Item1 == "m" && m < MAX_LIFE)
				{
					mob_labels[m] = new Text(Text.Font.A12M, Text.Alignment.LEFT, Color.Name.CHARTREUSE, life_object.Item2);
					fillheight += (short)(mob_labels[m].height () + 2);
					m++;
				}
				else if (life_object.Item1 == "n" && n < MAX_LIFE)
				{
					npc_labels[n] = new Text(Text.Font.A12M, Text.Alignment.LEFT, Color.Name.MALIBU, life_object.Item2);
					fillheight += (short)(npc_labels[n].height () + 2);
					n++;
				}
			}

			if (desc_label.empty())
			{
				if (mob_labels.Length > 0 || npc_labels.Length > 0)
				{
					fillheight += BOTTOM_PADDING;
				}
			}
		}

		public void reset()
		{
			set_name(Tooltip.Parent.NONE, "", false);
			set_desc("");

			desc_label.change_text("");
			name_label.change_text("");

			for (uint i = 0; i < MAX_LIFE; i++)
			{
				mob_labels[i]?.change_text("");
				npc_labels[i]?.change_text("");
			}

			fillwidth = MIN_WIDTH;
			fillheight = 0;
		}

		private  const byte MAX_LIFE = (byte)10u;
		private  const byte MIN_WIDTH = (byte)166u;
		private  const byte BOTTOM_PADDING = (byte)8u;
		private static  Point_short SEPARATOR_ADJ = new Point_short(5, 0);
		private static  Point_short LIFE_LABEL_ADJ = new Point_short(20, 3);
		private static  Point_short LIFE_ICON_ADJ = new Point_short(5, 9);

		private MapleFrame frame = new MapleFrame();

		private Texture cover = new Texture();
		private Texture Mob = new Texture();
		private Texture Npc = new Texture();
		private Texture Party = new Texture();

		private Tooltip.Parent parent;

		private string name;
		private string description;

		private Text name_label = new Text();
		private Text name_simple = new Text();
		private Text desc_label = new Text();
		private Text desc_simple = new Text();
		private Text[] mob_labels = new Text[MAX_LIFE];
		private Text[] npc_labels = new Text[MAX_LIFE];

		private short fillwidth;
		private short fillheight;

		private ColorLine separator = new ColorLine();
	}
}


#if USE_NX
#endif
