using System;
using MapleLib.WzLib;




namespace ms
{
	public class SkillTooltip : Tooltip
	{
		public SkillTooltip ()
		{
			WzObject Frame = ms.wz.wzFile_ui["UIToolTip.img"]["Item"]["Frame2"];

			frame = new MapleFrame (Frame);
			cover = Frame["cover"];

			skill_id = 0;
		}

		public override void draw (Point_short pos)
		{
			if (skill_id == 0)
			{
				return;
			}

			short max_width = Constants.get ().get_viewwidth ();
			short max_height = Constants.get ().get_viewheight ();
			short cur_width = (short)(pos.x () + width + 45);
			short cur_height = (short)(pos.y () + height + 35);

			short adj_x = (short)(cur_width - max_width);
			short adj_y = (short)(cur_height - max_height);

			if (adj_x > 0)
			{
				pos.shift_x ((short)(adj_x * -1));
			}

			if (adj_y > 0)
			{
				pos.shift_y ((short)(adj_y * -1));
			}

			frame.draw (pos + new Point_short (176, (short)(height + 11)), width, (short)(height - 1));
			name.draw (pos + new Point_short (33, 3));
			cover.draw (pos + new Point_short (16, -1));

			pos.shift_y (icon_offset);

			box.draw (new DrawArgument (pos + new Point_short (26, 21)));
			icon.draw (new DrawArgument (pos + new Point_short (28, 87), 2.0f, 2.0f));
			desc.draw (pos + new Point_short (102, 15));

			pos.shift_y (level_offset);

			line.draw (pos + new Point_short (22, 10));
			leveldesc.draw (pos + new Point_short (25, 11));
		}

		const string mltag = "Master Level";
		const string exptag = "#cAvailable until";

		public void set_skill (int id, int level, int mlevel, long expiration)
		{
			if (skill_id == id)
			{
				return;
			}

			skill_id = id;

			if (skill_id == 0)
			{
				return;
			}

			SkillData data = SkillData.get (id);

			int masterlevel;

			if (mlevel > 0)
			{
				masterlevel = mlevel;
			}
			else
			{
				masterlevel = data.get_masterlevel ();
			}

			string descstr = data.get_desc ();

			if (masterlevel > 0)
			{
				string mlstr = Convert.ToString (masterlevel);
				int mlstart = descstr.IndexOf (mltag);
				if (mlstart >= 0 && mlstart < descstr.Length)
				{
					int mlpos = descstr.IndexOf (':', mlstart) + 2; //todo 2 maybe skill descstr doesn't contains :
					int mlend = descstr.IndexOf ("]", mlstart);

					if (mlpos < mlend && mlend != -1)
					{
						int mlsize = mlend - mlpos;
						descstr = descstr.Remove (mlpos, mlsize);
						descstr = descstr.Insert (mlpos, mlstr);

						// Fixing errors in the files...
						if (mlstart == 0)
						{
							descstr = descstr.Insert (0, "[");
							mlend++;
						}

						int linebreak = descstr.IndexOf ("]\\n", mlstart);

						if (linebreak != mlend)
						{
							descstr = descstr.Insert (mlend + 1, "\\n");
						}
					}
					else
					{
						descstr = descstr.Insert (0, "[" + mltag + ": " + mlstr + "]\\n");
					}
				}
				else
				{
					AppDebug.Log ("skill descstr doesn't contain Master Level");
				}
			}


			if (expiration > 0)
			{
				// TODO: Blank
			}
			else
			{
				int expstart = descstr.IndexOf (exptag);
				int expend = descstr.IndexOf ('#', expstart + 1);

				if (expstart < expend && expend != -1 && expstart != -1)
				{
					int expsize = expend - expstart + 1;
					descstr = descstr.Remove (expstart, expsize);
				}
			}

			string levelstr = String.Empty;
			bool current = level > 0;
			bool next = level < masterlevel;

			if (current)
			{
				levelstr += "[Current Level: " + Convert.ToString (level) + "]\\n" + data.get_level_desc (level);
			}

			if (current && next)
			{
				levelstr += "\\n";
			}

			if (next)
			{
				levelstr += "[Next Level: " + Convert.ToString (level + 1) + "]\\n" + data.get_level_desc (level + 1);
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: icon = data.get_icon(SkillData::Icon::NORMAL);
			icon = new Texture (data.get_icon (SkillData.Icon.NORMAL));
			name = new Text (Text.Font.A12B, Text.Alignment.LEFT, Color.Name.WHITE, data.get_name (), 320);
			desc = new Text (Text.Font.A12M, Text.Alignment.LEFT, Color.Name.WHITE, descstr, 210);
			leveldesc = new Text (Text.Font.A12M, Text.Alignment.LEFT, Color.Name.WHITE, levelstr, 290);

			var desc_height = desc.height () + 11;

			icon_offset = name.height ();
			level_offset = (short)Math.Max (desc_height, 85);
			height = (short)(icon_offset + level_offset + leveldesc.height ());

			short icon_width = (short)((icon.get_dimensions ().x () * 2) + 4);
			width = 292;

			line = new ColorLine ((short)(width + 16), Color.Name.WHITE, 1.0f, false);
			box = new ColorBox (icon_width, icon_width, Color.Name.WHITE, 0.65f);
		}

		private int skill_id;
		private short height;
		private short width;
		private short icon_offset;
		private short level_offset;
		private Texture icon = new Texture ();
		private Texture required_icon = new Texture ();

		private Text name = new Text ();
		private Text desc = new Text ();
		private Text leveldesc = new Text ();
		private MapleFrame frame = new MapleFrame ();
		private ColorLine line = new ColorLine ();
		private ColorBox box = new ColorBox ();
		private Texture cover = new Texture ();
	}
}