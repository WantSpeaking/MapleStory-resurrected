#define USE_NX

using System;
using System.Collections.Generic;
using MapleLib.WzLib;
//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////


namespace ms
{
	public class UICashShop : UIElement
	{
		public const Type TYPE = UIElement.Type.CASHSHOP;
		public const bool FOCUSED = true;
		public const bool TOGGLED = false;

		public UICashShop ()
		{
			this.preview_index = 0;
			this.menu_index = 1;
			this.promotion_index = 0;
			this.mvp_grade = 1;
			this.mvp_exp = 0.07f;
			this.list_offset = 0;
			WzObject CashShop = nl.nx.wzFile_ui["CashShop.img"];
			WzObject Base = CashShop["Base"];
			WzObject backgrnd = Base["backgrnd"];
			WzObject BestNew = Base["BestNew"];
			WzObject Preview = Base["Preview"];
			WzObject CSTab = CashShop["CSTab"];
			WzObject CSGLChargeNX = CSTab["CSGLChargeNX"];
			WzObject CSStatus = CashShop["CSStatus"];
			WzObject CSPromotionBanner = CashShop["CSPromotionBanner"];
			WzObject CSMVPBanner = CashShop["CSMVPBanner"];
			WzObject CSItemSearch = CashShop["CSItemSearch"];
			WzObject CSChar = CashShop["CSChar"];
			WzObject CSList = CashShop["CSList"];
			WzObject CSEffect = CashShop["CSEffect"];

			sprites.Add (backgrnd);
			sprites.Add (new Sprite (BestNew, new Point<short> (139, 346)));

			BestNew_dim = new Texture (BestNew).get_dimensions ();

			for (uint i = 0; i < 3; i++)
			{
				preview_sprites[i] = Preview[i.ToString ()];
			}

			for (ushort i = 0; i < 3; i++)
			{
				buttons[(ushort)(Buttons.BtPreview1 + i)] = new TwoSpriteButton (Base["Tab"]["Disable"][i.ToString ()], Base["Tab"]["Enable"][i.ToString ()], new Point<short> ((short)(957 + (i * 17)), 46));
			}

			buttons[(int)Buttons.BtPreview1].set_state (Button.State.PRESSED);

			buttons[(int)Buttons.BtExit] = new MapleButton (CSTab["BtExit"], new Point<short> (5, 728));
			buttons[(int)Buttons.BtChargeNX] = new MapleButton (CSGLChargeNX["BtChargeNX"], new Point<short> (5, 554));
			buttons[(int)Buttons.BtChargeRefresh] = new MapleButton (CSGLChargeNX["BtChargeRefresh"], new Point<short> (92, 554));

			for (uint i = 0; i < 9; i++)
			{
				menu_tabs[i] = CSTab["Tab"][i.ToString ()];
			}

			buttons[(int)Buttons.BtChargeRefresh] = new MapleButton (CSGLChargeNX["BtChargeRefresh"], new Point<short> (92, 554));
			buttons[(int)Buttons.BtWish] = new MapleButton (CSStatus["BtWish"], new Point<short> (226, 6));
			buttons[(int)Buttons.BtMileage] = new MapleButton (CSStatus["BtMileage"], new Point<short> (869, 4));
			buttons[(int)Buttons.BtHelp] = new MapleButton (CSStatus["BtHelp"], new Point<short> (997, 4));
			buttons[(int)Buttons.BtCoupon] = new MapleButton (CSStatus["BtCoupon"], new Point<short> (950, 4));

			Charset tab = new Charset ();

			job_label = new Text (Text.Font.A11B, Text.Alignment.LEFT, Color.Name.SUPERNOVA, "Illium");
			name_label = new Text (Text.Font.A11B, Text.Alignment.LEFT, Color.Name.WHITE, "ShomeiZekkou");

			promotion_pos = new Point<short> (138, 40);
			sprites.Add (new Sprite (CSPromotionBanner["shadow"], promotion_pos));

			promotion_sprites.Add (CSPromotionBanner["basic"]);

			buttons[(int)Buttons.BtNext] = new MapleButton (CSPromotionBanner["BtNext"], promotion_pos);
			buttons[(int)Buttons.BtPrev] = new MapleButton (CSPromotionBanner["BtPrev"], promotion_pos);

			for (uint i = 0; i < 7; i++)
			{
				mvp_sprites[i] = CSMVPBanner["grade"][i.ToString ()];
			}

			mvp_pos = new Point<short> (63, 681);
			buttons[(int)Buttons.BtDetailPackage] = new MapleButton (CSMVPBanner["BtDetailPackage"], mvp_pos);
			buttons[(int)Buttons.BtNonGrade] = new MapleButton (CSMVPBanner["BtNonGrade"], mvp_pos);

			buttons[(int)Buttons.BtDetailPackage].set_active (mvp_grade == 1);
			buttons[(int)Buttons.BtNonGrade].set_active (mvp_grade == 0);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: mvp_gauge = Gauge(Gauge::Type::CASHSHOP, CSMVPBanner["gage"][0], CSMVPBanner["gage"][2], CSMVPBanner["gage"][1], 84, 0.0f);
			mvp_gauge = new Gauge (Gauge.Type.CASHSHOP, CSMVPBanner["gage"][0.ToString ()], CSMVPBanner["gage"][2.ToString ()], CSMVPBanner["gage"][1.ToString ()], 84, 0.0f);

			Point<short> search_pos = new Point<short> (0, 36);
			sprites.Add (new Sprite (CSItemSearch["backgrnd"], search_pos));
			sprites.Add (new Sprite (CSItemSearch["search"], search_pos + new Point<short> (35, 8)));

			buttons[(int)Buttons.BtBuyAvatar] = new MapleButton (CSChar["BtBuyAvatar"], new Point<short> (642, 305));
			buttons[(int)Buttons.BtDefaultAvatar] = new MapleButton (CSChar["BtDefaultAvatar"], new Point<short> (716, 305));
			buttons[(int)Buttons.BtInventory] = new MapleButton (CSChar["BtInventory"], new Point<short> (938, 305));
			buttons[(int)Buttons.BtSaveAvatar] = new MapleButton (CSChar["BtSaveAvatar"], new Point<short> (864, 305));
			buttons[(int)Buttons.BtTakeoffAvatar] = new MapleButton (CSChar["BtTakeoffAvatar"], new Point<short> (790, 305));

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: charge_charset = Charset(CSGLChargeNX["Number"], Charset::Alignment::RIGHT);
			charge_charset = new Charset (CSGLChargeNX["Number"], Charset.Alignment.RIGHT);

			item_base = CSList["Base"];
			item_line = Base["line"];
			item_none = Base["noItem"];

			foreach (var item_label in CSEffect)
			{
				item_labels.Add (item_label);
			}

			items.Add (new Item (5220000, Item.Label.HOT, 34000, 11));
			items.Add (new Item (5220000, Item.Label.HOT, 34000, 11));
			items.Add (new Item (5220000, Item.Label.HOT, 0, 0));
			items.Add (new Item (5220000, Item.Label.HOT, 0, 0));
			items.Add (new Item (5220000, Item.Label.HOT, 10000, 11));
			items.Add (new Item (5220000, Item.Label.NEW, 0, 0));
			items.Add (new Item (5220000, Item.Label.SALE, 7000, 0));
			items.Add (new Item (5220000, Item.Label.NEW, 13440, 0));
			items.Add (new Item (5220000, Item.Label.NEW, 7480, 0));
			items.Add (new Item (5220000, Item.Label.NEW, 7480, 0));
			items.Add (new Item (5220000, Item.Label.NEW, 7480, 0));
			items.Add (new Item (5220000, Item.Label.NONE, 12000, 11));
			items.Add (new Item (5220000, Item.Label.NONE, 22000, 11));
			items.Add (new Item (5220000, Item.Label.NONE, 0, 0));
			items.Add (new Item (5220000, Item.Label.NONE, 0, 0));
			items.Add (new Item (5220000, Item.Label.MASTER, 0, 15));

			for (int i = 0; i < MAX_ITEMS; i++)
			{
				var quot = i / 7;
				var rem = i % 7;
				buttons[(ushort)(Buttons.BtBuy + (ushort)i)] = new MapleButton (CSList["BtBuy"], new Point<short> (146, 523) + new Point<short> ((short)(124 * rem), (short)(205 * quot)));

				item_name[i] = new Text (Text.Font.A11B, Text.Alignment.CENTER, Color.Name.MINESHAFT);
				item_price[i] = new Text (Text.Font.A11M, Text.Alignment.CENTER, Color.Name.GRAY);
				item_discount[i] = new Text (Text.Font.A11M, Text.Alignment.CENTER, Color.Name.SILVERCHALICE);
				item_percent[i] = new Text (Text.Font.A11M, Text.Alignment.CENTER, Color.Name.TORCHRED);
			}

			Point<short> slider_pos = new Point<short> (1007, 372);

			list_slider = new Slider ((int)Slider.Type.THIN_MINESHAFT, new Range<short> (slider_pos.y (), (short)(slider_pos.y () + 381)), slider_pos.x (), 2, 7, (bool upwards) =>
			{
				short shift = (short)(upwards ? -7 : 7);
				bool above = list_offset >= 0;
				bool below = list_offset + shift < items.Count;

				if (above && below)
				{
					list_offset += shift;

					update_items ();
				}
			});

			update_items ();

			dimension = new Texture (backgrnd).get_dimensions ();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float inter) const
		public new void draw (float inter)
		{
			preview_sprites[preview_index].draw (position + new Point<short> (644, 65), inter);

			base.draw_sprites (inter);

			menu_tabs[menu_index].draw (position + new Point<short> (0, 63), inter);

			Point<short> label_pos = position + new Point<short> (4, 3);
			job_label.draw (label_pos);

			var length = job_label.width ();
			name_label.draw (label_pos + new Point<short> ((short)(length + 10), 0));

			promotion_sprites[promotion_index].draw (position + promotion_pos, inter);

			mvp_sprites[mvp_grade].draw (position + mvp_pos, inter);
			mvp_gauge.draw (position + mvp_pos);

			Point<short> charge_pos = position + new Point<short> (107, 388);

			charge_charset.draw ("0", charge_pos + new Point<short> (0, 30 * 1));
			charge_charset.draw ("3,300", charge_pos + new Point<short> (0, 30 * 2));
			charge_charset.draw ("0", charge_pos + new Point<short> (0, 30 * 3));
			charge_charset.draw ("8,698,565", charge_pos + new Point<short> (0, 30 * 4));
			charge_charset.draw ("0", charge_pos + new Point<short> (0, 30 * 5));

			if (items.Count > 0)
			{
				item_line.draw (position + new Point<short> (139, 566), inter);
			}
			else
			{
				item_none.draw (position + new Point<short> (137, 372) + new Point<short> ((short)(BestNew_dim.x () / 2), (short)(list_slider.getvertical ().length () / 2)) - item_none.get_dimensions () / (short)2, inter);
			}

			for (uint i = 0; i < MAX_ITEMS; i++)
			{
				short index = (short)(i + list_offset);

				if (index < items.Count)
				{
					var quot = i / 7;
					var rem = i % 7;
					Item item = items[index];

					item_base.draw (position + new Point<short> (137, 372) + new Point<short> ((short)(124 * rem), (short)(205 * quot)), inter);
					item.draw (new DrawArgument (position + new Point<short> (164, 473) + new Point<short> ((short)(124 * rem), (short)(205 * quot)), 2.0f, 2.0f));

					if (item.label != Item.Label.NONE)
					{
						item_labels[(int)(item.label + 1)].draw (position + new Point<short> (152, 372) + new Point<short> ((short)(124 * rem), (short)(205 * quot)), inter);
					}

					item_name[i].draw (position + new Point<short> (192, 480) + new Point<short> ((short)(124 * rem), (short)(205 * quot)));

					if (item_discount[i].get_text () == "")
					{
						item_price[i].draw (position + new Point<short> (195, 499) + new Point<short> ((short)(124 * rem), (short)(205 * quot)));
					}
					else
					{
						item_price[i].draw (position + new Point<short> (196, 506) + new Point<short> ((short)(124 * rem), (short)(205 * quot)));

						item_discount[i].draw (position + new Point<short> (185, 495) + new Point<short> ((short)(124 * rem), (short)(205 * quot)));
						item_percent[i].draw (position + new Point<short> ((short)(198 + (item_discount[i].width () / 2)), 495) + new Point<short> ((short)(124 * rem), (short)(205 * quot)));
					}
				}
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: list_slider.draw(position);
			list_slider.draw (position);

			base.draw_buttons (inter);
		}

		public override void update ()
		{
			base.update ();

			mvp_gauge.update (mvp_exp);
		}

		public new Button.State button_pressed (ushort buttonid)
		{
			switch ((Buttons)buttonid)
			{
				case Buttons.BtPreview1:
				case Buttons.BtPreview2:
				case Buttons.BtPreview3:
					buttons[preview_index].set_state (Button.State.NORMAL);

					preview_index = (byte)buttonid;
					return Button.State.PRESSED;
				case Buttons.BtExit:
				{
					ushort width = Setting<Width>.get ().load ();
					ushort height = Setting<Height>.get ().load ();

					Constants.get ().set_viewwidth ((short)width);
					Constants.get ().set_viewheight ((short)height);

					float fadestep = 0.025f;

					/*todo Window.get ().fadeout (fadestep, () =>
					{
						GraphicsGL.get ().clear ();
						ChangeMapPacket ().dispatch ();
					});

					GraphicsGL.get ().@lock ();*/
					Stage.get ().clear ();
					Timer.get ().start ();

					return Button.State.NORMAL;
				}
				case Buttons.BtNext:
				{
					uint size = (uint)(promotion_sprites.Count - 1);

					promotion_index++;

					if (promotion_index > size)
					{
						promotion_index = 0;
					}

					return Button.State.NORMAL;
				}
				case Buttons.BtPrev:
				{
					uint size = (uint)(promotion_sprites.Count - 1);

					promotion_index--;

					if (promotion_index < 0)
					{
						promotion_index = (sbyte)size;
					}

					return Button.State.NORMAL;
				}
				case Buttons.BtChargeNX:
				{
					string url = Configuration.get ().get_chargenx ();

					//todo ShellExecuteA (null, "open", url, null, null, SW_SHOWNORMAL);

					return Button.State.NORMAL;
				}
				default:
					break;
			}

			if (buttonid >= (int)Buttons.BtBuy)
			{
				short index = (short)(buttonid - Buttons.BtBuy + (ushort)list_offset);

				Item item = items[index];

				// TODO: Purchase item

				return Button.State.NORMAL;
			}

			return Button.State.DISABLED;
		}

		public override Cursor.State send_cursor (bool clicked, Point<short> cursorpos)
		{
			Point<short> cursor_relative = cursorpos - position;

			if (list_slider.isenabled ())
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: Cursor::State state = list_slider.send_cursor(cursor_relative, clicked);
				Cursor.State state = list_slider.send_cursor (cursor_relative, clicked);

				if (state != Cursor.State.IDLE)
				{
					return state;
				}
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return UIElement::send_cursor(clicked, cursorpos);
			return base.send_cursor (clicked, cursorpos);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public void exit_cashshop ()
		{
			UI ui = UI.get ();
			ui.change_state (UI.State.GAME);

			Stage stage = Stage.get ();
			Player player = stage.get_player ();

			new PlayerLoginPacket (player.get_oid ()).dispatch ();

			int mapid = player.get_stats ().get_mapid ();
			byte portalid = player.get_stats ().get_portal ();

			stage.load (mapid, (sbyte)portalid);
			stage.transfer_player ();

			ui.enable ();
			Timer.get ().start ();
			//todo GraphicsGL.get ().unlock ();
		}

		private void update_items ()
		{
			for (uint i = 0; i < MAX_ITEMS; i++)
			{
				short index = (short)(i + list_offset);
				bool found_item = index < items.Count;

				buttons[(ushort)((int)Buttons.BtBuy + i)].set_active (found_item);

				string name = "";
				string price_text = "";
				string discount_text = "";
				string percent_text = "";

				if (found_item)
				{
					Item item = items[index];

					name = item.get_name ();

					int price = item.get_price ();
					price_text = Convert.ToString (price);

					if (item.discount_price > 0 && price > 0)
					{
						discount_text = price_text;

						uint discount = (uint)item.discount_price;
						price_text = Convert.ToString (discount);

						var percent = (float)discount / price;
						string percent_str = Convert.ToString (percent);
						percent_text = "(" + percent_str.Substring (2, 1) + "%)";
					}

					string_format.split_number (price_text);
					string_format.split_number (discount_text);

					price_text += " NX";

					if (discount_text != "")
					{
						discount_text += " NX";
					}

					if (item.count > 0)
					{
						price_text += "(" + Convert.ToString (item.count) + ")";
					}
				}

				item_name[i].change_text (name);
				item_price[i].change_text (price_text);
				item_discount[i].change_text (discount_text);
				item_percent[i].change_text (percent_text);

				string_format.format_with_ellipsis (item_name[i], 92);
			}
		}

		private const byte MAX_ITEMS = (byte)(7u * 2u + 1u);

		private class Item
		{
			public enum Label : byte
			{
				ACTION,
				BOMB_SALE,
				BONUS,
				EVENT = 4,
				HOT,
				LIMITED,
				LIMITED_BRONZE,
				LIMITED_GOLD,
				LIMITED_SILVER,
				LUNA_CRYSTAL,
				MASTER = 12,
				MUST,
				NEW,
				SALE = 17,
				SPEICAL,
				SPECIAL_PRICE,
				TIME,
				TODAY,
				WEEKLY,
				WONDER_BERRY,
				WORLD_SALE,
				NONE
			}

			public Item (int itemid, Label label, int discount, ushort count)
			{
				this.label = label;
				this.discount_price = discount;
				this.count = count;
				this.data = ItemData.get (itemid);
			}

			public Label label;
			public int discount_price;
			public ushort count;

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(const DrawArgument& args) const
			public void draw (DrawArgument args)
			{
				data.get_icon (false).draw (args);
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string get_name() const
			public string get_name ()
			{
				return data.get_name ();
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const int get_price() const
			public int get_price ()
			{
				return data.get_price ();
			}

			private readonly ItemData data;
		}

		private enum Buttons : ushort
		{
			BtPreview1,
			BtPreview2,
			BtPreview3,
			BtExit,
			BtChargeNX,
			BtChargeRefresh,
			BtWish,
			BtMileage,
			BtHelp,
			BtCoupon,
			BtNext,
			BtPrev,
			BtDetailPackage,
			BtNonGrade,
			BtBuyAvatar,
			BtDefaultAvatar,
			BtInventory,
			BtSaveAvatar,
			BtTakeoffAvatar,
			BtBuy
		}

		private Point<short> BestNew_dim = new Point<short> ();

		private Sprite[] preview_sprites = new Sprite[3];
		private byte preview_index;

		private Sprite[] menu_tabs = new Sprite[9];
		private byte menu_index;

		private Text job_label = new Text ();
		private Text name_label = new Text ();

		private List<Sprite> promotion_sprites = new List<Sprite> ();
		private Point<short> promotion_pos = new Point<short> ();
		private sbyte promotion_index;

		private Sprite[] mvp_sprites = new Sprite[7];
		private Point<short> mvp_pos = new Point<short> ();
		private byte mvp_grade;
		private Gauge mvp_gauge = new Gauge ();
		private float mvp_exp;

		private Charset charge_charset = new Charset ();

		private Sprite item_base = new Sprite ();
		private Sprite item_line = new Sprite ();
		private Sprite item_none = new Sprite ();
		private List<Sprite> item_labels = new List<Sprite> ();
		private List<Item> items = new List<Item> ();
		private Text[] item_name = new Text[MAX_ITEMS];
		private Text[] item_price = new Text[MAX_ITEMS];
		private Text[] item_discount = new Text[MAX_ITEMS];
		private Text[] item_percent = new Text[MAX_ITEMS];

		private Slider list_slider = new Slider ();
		private short list_offset;
	}
}


#if USE_NX
#endif