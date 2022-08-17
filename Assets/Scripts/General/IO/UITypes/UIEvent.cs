using System.Collections.Generic;
using MapleLib.WzLib;
using ms.Helper;

namespace ms
{
	public class UIEvent : UIDragElement<PosEVENT>
	{
		public const Type TYPE = UIElement.Type.EVENT;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public UIEvent (params object[] args) : this ()
		{
		}
		
		public UIEvent ()
		{
			offset = 0;
			event_count = 16;

			WzObject main = ms.wz.wzFile_ui["UIWindow2.img"]["EventList"]["main"];
			WzObject close = ms.wz.wzFile_ui["Basic.img"]["BtClose3"];

			WzObject backgrnd = main["backgrnd"];
			Point_short bg_dimensions = new Texture (backgrnd).get_dimensions ();

			sprites.Add (backgrnd);
			sprites.Add (new Sprite (main["backgrnd2"], new Point_short (1, 0)));

			buttons[(int)Buttons.CLOSE] = new MapleButton (close, new Point_short ((short)(bg_dimensions.x () - 19), 6));

			bool in_progress = false;
			bool item_rewards = false;

			for (uint i = 0; i < 5; i++)
			{
				events.Add (new BoolPairNew<bool> (true, true));
			}

			for (uint i = 0; i < 10; i++)
			{
				events.Add (new BoolPairNew<bool> (false, true));
			}

			events.Add (new BoolPairNew<bool> (false, false));

			for (uint i = 0; i < 3; i++)
			{
				event_title[i] = new ShadowText (Text.Font.A18M, Text.Alignment.LEFT, Color.Name.HALFANDHALF, Color.Name.ENDEAVOUR);
			}

			for (uint i = 0; i < 3; i++)
			{
				event_date[i] = new Text (Text.Font.A12B, Text.Alignment.LEFT, Color.Name.WHITE);
			}

			item_reward = main["event"]["normal"];
			text_reward = main["liveEvent"]["normal"];
			next = main["liveEvent"]["next"];
			label_on = main["label_on"]["0"];
			label_next = main["label_next"]["0"];

			slider = new Slider ((int)Slider.Type.DEFAULT_SILVER, new Range_short (86, 449), 396, 3, event_count, (bool upwards) =>
			{
				short shift = (short)(upwards ? -1 : 1);
				bool above = offset + shift >= 0;
				bool below = offset + shift <= event_count - 3;

				if (above && below)
				{
					offset += shift;
				}
			});

			dimension = new Point_short (bg_dimensions);
			dragarea = new Point_short (dimension.x (), 20);
		}

		public override void draw (float inter)
		{
			base.draw (inter);

			slider.draw (new Point_short (position));

			for (uint i = 0; i < 3; i++)
			{
				short slot = (short)(i + offset);

				if (slot >= event_count)
				{
					break;
				}

				var event_pos = new Point_short (12, (short)(87 + 125 * i));

				var evnt = events[slot];
				var in_progress = evnt[1.ToBool ()];
				var itm_reward = evnt[0.ToBool ()];

				if (itm_reward != null)
				{
					item_reward.draw (position + event_pos);

					short x_adj = 0;

					for (uint f = 0; f < 5; f++)
					{
						ItemData item_data = ItemData.get ((int)(2000000 + f));
						Texture icon = item_data.get_icon (true);

						if (f == 2)
						{
							x_adj = 2;
						}
						else if (f == 3)
						{
							x_adj = 6;
						}
						else if (f == 4)
						{
							x_adj = 9;
						}

						icon.draw (position + new Point_short ((short)(33 + x_adj + 46 * f), (short)(191 + 125 * i)));
					}
				}
				else
				{
					text_reward.draw (position + event_pos);

					if (in_progress == null)
					{
						next.draw (position + event_pos);
					}
				}

				if (in_progress != null)
				{
					label_on.draw (position + event_pos);
				}
				else
				{
					label_next.draw (position + event_pos);
				}

				var title_pos = new Point_short (28, (short)(95 + 125 * i));
				var date_pos = new Point_short (28, (short)(123 + 125 * i));

				event_title[i].draw (position + title_pos);
				event_date[i].draw (position + date_pos);
			}
		}

		public override void update ()
		{
			base.update ();

			for (uint i = 0; i < 3; i++)
			{
				short slot = (short)(i + offset);

				if (slot >= event_count)
				{
					break;
				}

				string title = get_event_title ((byte)slot);

				if (title.Length > 35)
				{
					title = title.Substring (0, 35) + "..";
				}

				event_title[i].change_text (title);
				event_date[i].change_text (get_event_date ((byte)slot));
			}
		}

		public override void remove_cursor ()
		{
			base.remove_cursor ();

			UI.get ().clear_tooltip (Tooltip.Parent.EVENT);

			slider.remove_cursor ();
		}

		public override Cursor.State send_cursor (bool clicked, Point_short cursorpos)
		{
			Point_short cursoroffset = cursorpos - position;

			if (slider.isenabled ())
			{
				Cursor.State new_state = slider.send_cursor (new Point_short (cursoroffset), clicked);
				if (new_state != Cursor.State.IDLE)
				{
					return new_state;
				}
			}

			short yoff = cursoroffset.y ();
			short xoff = cursoroffset.x ();
			short row = row_by_position (yoff);
			short col = col_by_position (xoff);

			if (row > 0 && row < 4 && col > 0 && col < 6)
			{
				show_item (row, col);
			}

			return base.send_cursor (clicked, new Point_short (cursorpos));
		}

		public override void send_key (int keycode, bool pressed, bool escape)
		{
			if (pressed && escape)
			{
				close ();
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
				case Buttons.CLOSE:
					close ();
					break;
				//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# does not allow fall-through from a non-empty 'case':
				default:
					break;
			}

			return Button.State.NORMAL;
		}

		private void close ()
		{
			deactivate ();
		}

		private string get_event_title (byte id)
		{
			switch (id)
			{
				case 0:
					return "LINE FRIENDS";
				case 1:
					return "LINE FRIENDS Coin Shop";
				case 2:
					return "[14th Street] Big Bang Store";
				case 3:
					return "[14th Street] Override Fashion Marketing";
				case 4:
					return "[14th Street] Dance Battle V";
				case 5:
					return "MapleStory 14th Anniversary Appre..";
				case 6:
					return "[14th Street] Big Bang Store Season..";
				case 7:
					return "[14th Street] Maplelin Star Grub!";
				case 8:
					return "[14th Street] Sub-Zero Hunt";
				case 9:
					return "[14th Street] The Legends Return!";
				case 10:
					return "[14th Street] Renegade Personal Training";
				case 11:
					return "[14th Street] Round-We-Go Cafe Rising Heroes!";
				case 12:
					return "[14th Street] Big Bang Attack!";
				case 13:
					return "[14th Street] Spiegelmann's Art Retrieval";
				case 14:
					return "[14th Street] 14th Street Sky";
				case 15:
					return "[Sunny Sunday] Perks Abound!";
				default:
					return "";
			}
		}

		private string get_event_date (byte id)
		{
			switch (id)
			{
				case 0:
				case 1:
				case 2:
					return "04/24/2019 - 05/21/2019, 23:59";
				case 3:
					return "04/24/2019 - 05/07/2019, 23:59";
				case 4:
					return "04/24/2019 - 06/11/2019, 23:59";
				case 5:
					return "05/11/2019 - 05/11/2019, 23:59";
				case 6:
				case 10:
				case 11:
				case 12:
					return "05/22/2019 - 06/11/2019, 23:59";
				case 7:
				case 8:
					return "05/08/2019 - 05/21/2019, 23:59";
				case 9:
					return "05/08/2019 - 06/11/2019, 23:59";
				case 13:
				case 14:
					return "05/29/2019 - 06/11/2019, 23:59";
				case 15:
					return "05/05/2019 - 05/05/2019, 23:59";
				default:
					return "";
			}
		}

		private short row_by_position (short y)
		{
			short item_height = 43;

			if (y >= 148 && y <= 148 + item_height)
			{
				return 1;
			}
			else if (y >= 273 && y <= 273 + item_height)
			{
				return 2;
			}
			else if (y >= 398 && y <= 398 + item_height)
			{
				return 3;
			}
			else
			{
				return -1;
			}
		}

		private short col_by_position (short x)
		{
			short item_width = 43;

			if (x >= 25 && x <= 25 + item_width)
			{
				return 1;
			}
			else if (x >= 71 && x <= 71 + item_width)
			{
				return 2;
			}
			else if (x >= 117 && x <= 117 + item_width)
			{
				return 3;
			}
			else if (x >= 163 && x <= 163 + item_width)
			{
				return 4;
			}
			else if (x >= 209 && x <= 209 + item_width)
			{
				return 5;
			}
			else
			{
				return -1;
			}
		}

		private void show_item (short row, short col)
		{
			UI.get ().show_item (Tooltip.Parent.EVENT, 2000000 + col - 1);
		}

		private enum Buttons : ushort
		{
			CLOSE
		}

		private short offset;
		private short event_count;
		private ShadowText[] event_title =new ShadowText[3];
		private Text[] event_date =new Text[3]; 
		private Slider slider = new Slider ();
		private Texture item_reward = new Texture ();
		private Texture text_reward = new Texture ();
		private Texture next = new Texture ();
		private Texture label_on = new Texture ();
		private Texture label_next = new Texture ();
		private List<BoolPairNew<bool>> events = new List<BoolPairNew<bool>> ();
	}
}


#if USE_NX
#endif