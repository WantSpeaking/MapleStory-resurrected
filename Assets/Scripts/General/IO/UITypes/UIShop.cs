#define USE_NX

using System;
using System.Collections.Generic;
using Beebyte.Obfuscator;
using constants.skills;
using Loxodon.Framework.Observables;
using MapleLib.WzLib;
using ms.Helper;




namespace ms
{
	[Skip]
	public class UIShop : UIDragElement<PosSHOP>
	{
		public const Type TYPE = UIElement.Type.SHOP;
		public const bool FOCUSED = true;
		public const bool TOGGLED = true;

		public UIShop (params object[] args) : this ((CharLook)args[0], (Inventory)args[1])
		{
		}

		public UIShop (CharLook in_charlook, Inventory in_inventory)
		{
			/*this.charlook = in_charlook;
			this.inventory = in_inventory;
			WzObject src = ms.wz.wzFile_ui["UIWindow.img"]["Shop2"];

			WzObject background = src["backgrnd"];
			Texture bg = background;

			var bg_dimensions = bg.get_dimensions ();

			sprites.Add (background);
			sprites.Add (src["backgrnd2"]);
			sprites.Add (src["backgrnd3"]);
			sprites.Add (src["backgrnd4"]);

			buttons[(int)Buttons.BUY_ITEM] = new MapleButton (src["BtBuy"]);
			buttons[(int)Buttons.SELL_ITEM] = new MapleButton (src["BtSell"]);
			buttons[(int)Buttons.EXIT] = new MapleButton (src["BtExit"]);

			Texture cben = src["checkBox"][0.ToString ()];
			Texture cbdis = src["checkBox"][1.ToString ()];

			Point_short cb_origin = cben.get_origin ();
			short cb_x = cb_origin.x ();
			short cb_y = cb_origin.y ();

			checkBox[0.ToBool ()] = cbdis;
			checkBox[1.ToBool ()] = cben;

			buttons[(int)Buttons.CHECKBOX] = new AreaButton (new Point_short (Math.Abs (cb_x), Math.Abs (cb_y)), cben.get_dimensions ());

			WzObject buyen = src["TabBuy"]["enabled"];
			WzObject buydis = src["TabBuy"]["disabled"];

			buttons[(int)Buttons.OVERALL] = new TwoSpriteButton (buydis[0.ToString ()], buyen[0.ToString ()]);

			WzObject sellen = src["TabSell"]["enabled"];
			WzObject selldis = src["TabSell"]["disabled"];

			for (ushort i = (int)Buttons.EQUIP; i <= (int)Buttons.CASH; i++)
			{
				string tabnum = Convert.ToString (i - (int)Buttons.EQUIP);
				buttons[i] = new TwoSpriteButton (selldis[tabnum], sellen[tabnum]);
			}

			short item_y = 124;
			short item_height = 36;

			buy_x = 8;
			buy_width = 257;

			for (ushort i = (int)Buttons.BUY0; i <= (int)Buttons.BUY8; i++)
			{
				Point_short pos = new Point_short (buy_x, (short)(item_y + 42 * (i - (int)Buttons.BUY0)));
				Point_short dim = new Point_short (buy_width, item_height);
				buttons[i] = new AreaButton (pos, dim);
			}

			sell_x = 284;
			sell_width = 200;

			for (ushort i = (int)Buttons.SELL0; i <= (int)Buttons.SELL8; i++)
			{
				Point_short pos = new Point_short (sell_x, (short)(item_y + 42 * (i - (int)Buttons.SELL0)));
				Point_short dim = new Point_short (sell_width, item_height);
				buttons[i] = new AreaButton (pos, dim);
			}

			buy_selection = src["select"];
			sell_selection = src["select2"];
			meso = src["meso"];

			mesolabel = new Text (Text.Font.A11M, Text.Alignment.RIGHT, Color.Name.MINESHAFT);

			buyslider = new Slider ((int)Slider.Type.DEFAULT_SILVER, new Range_short (123, 484), 257, 5, 1, (bool upwards) =>
			{
				short shift = (short)(upwards ? -1 : 1);
				bool above = buystate.offset + shift >= 0;
				bool below = buystate.offset + shift <= buystate.lastslot - 5;

				if (above && below)
				{
					buystate.offset += shift;
				}
			});

			sellslider = new Slider ((int)Slider.Type.DEFAULT_SILVER, new Range_short (123, 484), 488, 5, 1, (bool upwards) =>
			{
				short shift = (short)(upwards ? -1 : 1);
				bool above = sellstate.offset + shift >= 0;
				bool below = sellstate.offset + shift <= sellstate.lastslot - 5;

				if (above && below)
				{
					sellstate.offset += shift;
				}
			});

			active = false;
			dimension = new Point_short (bg_dimensions);
			dragarea = new Point_short (bg_dimensions.x (), 10);*/
			active = false;
		}

		/*public override void draw (float alpha)
		{
			base.draw(alpha);

			npc.draw(new DrawArgument(position + new Point_short(58, 85), true));
			charlook.draw(position + new Point_short(338, 85), false, Stance.Id.STAND1, Expression.Id.DEFAULT);

			mesolabel.draw(position + new Point_short(493, 51));

			buystate.draw(new Point_short(position), buy_selection);
			sellstate.draw(new Point_short(position), sell_selection);

			buyslider.draw(new Point_short(position));
			sellslider.draw(new Point_short(position));

			checkBox[rightclicksell].draw(position);
		}*/

		public override void update ()
		{
			/*long num_mesos = inventory.get_meso ();
			string mesostr = Convert.ToString (num_mesos);
			string_format.split_number (mesostr);
			mesolabel.change_text (mesostr);*/
		}

		public override void remove_cursor ()
		{
			base.remove_cursor ();

			buyslider.remove_cursor ();
			sellslider.remove_cursor ();
		}

		/*public override Cursor.State send_cursor (bool clicked, Point_short cursorpos)
		{
			Point_short cursoroffset = cursorpos - position;
			lastcursorpos = new Point_short (cursoroffset);

			if (buyslider.isenabled ())
			{
				Cursor.State bstate = buyslider.send_cursor (new Point_short (cursoroffset), clicked);

				if (bstate != Cursor.State.IDLE)
				{
					clear_tooltip ();

					return bstate;
				}
			}

			if (sellslider.isenabled ())
			{
				Cursor.State sstate = sellslider.send_cursor (new Point_short (cursoroffset), clicked);

				if (sstate != Cursor.State.IDLE)
				{
					clear_tooltip ();

					return sstate;
				}
			}

			short xoff = cursoroffset.x ();
			short yoff = cursoroffset.y ();
			short slot = slot_by_position (yoff);

			if (slot >= 0 && slot <= 8)
			{
				if (xoff >= buy_x && xoff <= buy_width)
				{
					show_item (slot, true);
				}
				else if (xoff >= sell_x && xoff <= sell_x + sell_width)
				{
					show_item (slot, false);
				}
				else
				{
					clear_tooltip ();
				}
			}
			else
			{
				clear_tooltip ();
			}

			Cursor.State ret = clicked ? Cursor.State.CLICKING : Cursor.State.IDLE;

			for (uint i = 0; i < (ulong)Buttons.NUM_BUTTONS; i++)
			{
				if (buttons[i].is_active () && buttons[i].bounds (position).contains (cursorpos))
				{
					if (buttons[i].get_state () == Button.State.NORMAL)
					{
						if (i >= ((int)Buttons.BUY_ITEM) && i <= (ulong)Buttons.EXIT)
						{
							new Sound (Sound.Name.BUTTONOVER).play ();

							buttons[i].set_state (Button.State.MOUSEOVER);
							ret = Cursor.State.CANCLICK;
						}
						else
						{
							buttons[i].set_state (Button.State.MOUSEOVER);
							ret = Cursor.State.IDLE;
						}
					}
					else if (buttons[i].get_state () == Button.State.MOUSEOVER)
					{
						if (clicked)
						{
							if (i >= ((int)Buttons.BUY_ITEM) && i <= (ulong)Buttons.CASH)
							{
								if (i >= ((int)Buttons.OVERALL) && i <= (ulong)Buttons.CASH)
								{
									new Sound (Sound.Name.TAB).play ();
								}
								else
								{
									if (i != (int)Buttons.CHECKBOX)
									{
										new Sound (Sound.Name.BUTTONCLICK).play ();
									}
								}

								buttons[i].set_state (button_pressed ((ushort)i));

								ret = Cursor.State.IDLE;
							}
							else
							{
								buttons[i].set_state (button_pressed ((ushort)i));

								ret = Cursor.State.IDLE;
							}
						}
						else
						{
							if (i >= ((int)Buttons.BUY_ITEM) && i <= (ulong)Buttons.EXIT)
							{
								ret = Cursor.State.CANCLICK;
							}
							else
							{
								ret = Cursor.State.IDLE;
							}
						}
					}
					else if (buttons[i].get_state () == Button.State.PRESSED)
					{
						if (clicked)
						{
							if (i >= ((int)Buttons.OVERALL) && i <= (ulong)Buttons.CASH)
							{
								new Sound (Sound.Name.TAB).play ();

								ret = Cursor.State.IDLE;
							}
						}
					}
				}
				else if (buttons[i].get_state () == Button.State.MOUSEOVER)
				{
					buttons[i].set_state (Button.State.NORMAL);
				}
			}

			return ret;
		}*/

		public override void send_scroll (double yoffset)
		{
			/*short xoff = lastcursorpos.x ();
			short slider_width = 10;

			if (buyslider.isenabled ())
			{
				if (xoff >= buy_x && xoff <= buy_width + slider_width)
				{
					buyslider.send_scroll (yoffset);
				}
			}

			if (sellslider.isenabled ())
			{
				if (xoff >= sell_x && xoff <= sell_x + sell_width + slider_width)
				{
					sellslider.send_scroll (yoffset);
				}
			}*/
		}

		public override void rightclick (Point_short cursorpos)
		{
			if (rightclicksell)
			{
				Point_short cursoroffset = cursorpos - position;

				short xoff = cursoroffset.x ();
				short yoff = cursoroffset.y ();
				short slot = slot_by_position (yoff);

				if (slot >= 0 && slot <= 8)
				{
					if (xoff >= sell_x && xoff <= sell_x + sell_width)
					{
						clear_tooltip ();

						sellstate.selection = slot;
						sellstate.sell (true);
						buystate.selection = -1;
					}
				}
			}
		}

		public override void send_key (int keycode, bool pressed, bool escape)
		{
			if (pressed && escape)
			{
				exit_shop ();
			}
		}

		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public void reset (int npcid)
		{
			string strid = string_format.extend_id (npcid, 7);
			npc = ms.wz.wzFile_npc[strid + ".img"]["stand"]["0"];

			foreach (var button in buttons)
			{
				button.Value.set_state (Button.State.NORMAL);
			}

			buttons[(int)Buttons.OVERALL].set_state (Button.State.PRESSED);
			buttons[(int)Buttons.EQUIP].set_state (Button.State.PRESSED);

			buystate.reset ();
			sellstate.reset ();

			changeselltab (InventoryType.Id.EQUIP);

			makeactive ();
			rightclicksell = Configuration.get ().get_rightclicksell ();
		}

		public void add_item (int id, int price, int pitch, int time, short buyable)
		{
			add_rechargable (id, price, pitch, time, 0, buyable);
		}

		public void add_rechargable (int id, int price, int pitch, int time, short chargeprice, short buyable)
		{
			var buyitem = new BuyItem (new Texture (meso), id, price, pitch, time, chargeprice, buyable);
			buystate.add (buyitem);

			buyslider.setrows (5, buystate.lastslot);
		}

		public void modify (InventoryType.Id type)
		{
			if (type == sellstate.tab)
			{
				changeselltab (type);
			}
		}

		static readonly Range_ushort buy = new Range_ushort ((ushort)Buttons.BUY0, (ushort)Buttons.BUY8);
		static readonly Range_ushort sell = new Range_ushort ((ushort)Buttons.SELL0, (ushort)Buttons.SELL8);

		public override Button.State button_pressed (ushort buttonid)
		{
			clear_tooltip ();

			if (buy.contains (buttonid))
			{
				short selected = (short)(buttonid - (int)Buttons.BUY0);
				buystate.select (selected);
				sellstate.selection = -1;

				return Button.State.NORMAL;
			}
			else if (sell.contains (buttonid))
			{
				short selected = (short)(buttonid - (int)Buttons.SELL0);
				sellstate.select (selected);
				buystate.selection = -1;

				return Button.State.NORMAL;
			}
			else
			{
				switch ((Buttons)buttonid)
				{
					case Buttons.BUY_ITEM:
						buystate.buy ();

						return Button.State.NORMAL;
					case Buttons.SELL_ITEM:
						sellstate.sell (false);

						return Button.State.NORMAL;
					case Buttons.EXIT:
						exit_shop ();

						return Button.State.PRESSED;
					case Buttons.CHECKBOX:
						rightclicksell = !rightclicksell;
						Configuration.get ().set_rightclicksell (rightclicksell);

						return Button.State.NORMAL;
					case Buttons.EQUIP:
						changeselltab (InventoryType.Id.EQUIP);

						return Button.State.IDENTITY;
					case Buttons.USE:
						changeselltab (InventoryType.Id.USE);

						return Button.State.IDENTITY;
					case Buttons.ETC:
						changeselltab (InventoryType.Id.ETC);

						return Button.State.IDENTITY;
					case Buttons.SETUP:
						changeselltab (InventoryType.Id.SETUP);

						return Button.State.IDENTITY;
					case Buttons.CASH:
						changeselltab (InventoryType.Id.CASH);

						return Button.State.IDENTITY;
				}
			}

			return Button.State.PRESSED;
		}

		private void clear_tooltip ()
		{
			UI.get ().clear_tooltip (Tooltip.Parent.SHOP);
		}

		private void show_item (short slot, bool buy)
		{
			if (buy)
			{
				buystate.show_item (slot);
			}
			else
			{
				sellstate.show_item (slot);
			}
		}

		public void changeselltab (InventoryType.Id type)
		{
			ushort oldtab = tabbyinventory (sellstate.tab);

			if (oldtab > 0)
			{
				buttons[oldtab].set_state (Button.State.NORMAL);
			}

			ushort newtab = tabbyinventory (type);

			if (newtab > 0)
			{
				buttons[newtab].set_state (Button.State.PRESSED);
			}

			sellstate.change_tab (inventory, type, new Texture (meso));

			sellslider.setrows (5, sellstate.lastslot);

			for (uint i = (int)Buttons.SELL0; i < (ulong)Buttons.SELL8; i++)
			{
				if (i - (ulong)Buttons.SELL0 < (ulong)sellstate.lastslot)
				{
					buttons[i].set_state (Button.State.NORMAL);
				}
				else
				{
					buttons[i].set_state (Button.State.DISABLED);
				}
			}
		}

		private short slot_by_position (short y)
		{
			short yoff = (short)(y - 123);

			if (yoff > 0 && yoff < 38)
			{
				return 0;
			}
			else if (yoff > 42 && yoff < 80)
			{
				return 1;
			}
			else if (yoff > 84 && yoff < 122)
			{
				return 2;
			}
			else if (yoff > 126 && yoff < 164)
			{
				return 3;
			}
			else if (yoff > 168 && yoff < 206)
			{
				return 4;
			}
			else if (yoff > 210 && yoff < 248)
			{
				return 5;
			}
			else if (yoff > 252 && yoff < 290)
			{
				return 6;
			}
			else if (yoff > 294 && yoff < 332)
			{
				return 7;
			}
			else if (yoff > 336 && yoff < 374)
			{
				return 8;
			}
			else
			{
				return -1;
			}
		}

		private ushort tabbyinventory (InventoryType.Id type)
		{
			switch (type)
			{
				case InventoryType.Id.EQUIP:
					return (ushort)Buttons.EQUIP;
				case InventoryType.Id.USE:
					return (ushort)Buttons.USE;
				case InventoryType.Id.ETC:
					return (ushort)Buttons.ETC;
				case InventoryType.Id.SETUP:
					return (ushort)Buttons.SETUP;
				case InventoryType.Id.CASH:
					return (ushort)Buttons.CASH;
				default:
					return 0;
			}
		}

		public void exit_shop ()
		{
			clear_tooltip ();

			deactivate ();
			new NpcShopActionPacket ().dispatch ();
		}

		public override void OnActivityChange (bool isActiveAfterChange)
		{
			if (isActiveAfterChange)
			{
				_fgui_Shop = ms_Unity.FGUI_Manager.Instance.OpenFGUI<ms_Unity.FGUI_Shop>()?.OnVisiblityChanged(true, this);
            }
			else
			{
                _fgui_Shop = ms_Unity.FGUI_Manager.Instance.CloseFGUI<ms_Unity.FGUI_Shop> ()?.OnVisiblityChanged (false,this);
			}
		}

		public Texture get_npc_texture () => npc;
		private enum Buttons : short
		{
			BUY_ITEM,
			SELL_ITEM,
			EXIT,
			CHECKBOX,
			OVERALL,
			EQUIP,
			USE,
			ETC,
			SETUP,
			CASH,
			BUY0,
			BUY1,
			BUY2,
			BUY3,
			BUY4,
			BUY5,
			BUY6,
			BUY7,
			BUY8,
			SELL0,
			SELL1,
			SELL2,
			SELL3,
			SELL4,
			SELL5,
			SELL6,
			SELL7,
			SELL8,
			NUM_BUTTONS
		}

		private readonly CharLook charlook;
		private readonly Inventory inventory;

		private Texture npc = new Texture ();
		private Texture buy_selection = new Texture ();
		private Texture sell_selection = new Texture ();
		private Texture meso = new Texture ();
		private Text mesolabel = new Text ();

		private Slider buyslider = new Slider ();
		private Slider sellslider = new Slider ();

		private short buy_x;
		private short buy_width;
		private short sell_x;
		private short sell_width;

		private BoolPairNew<Texture> checkBox = new BoolPairNew<Texture> ();

		private bool rightclicksell;

		private Point_short lastcursorpos = new Point_short ();

		public static ms_Unity.FGUI_Shop _fgui_Shop;

		public class BuyItem
		{
			public BuyItem (Texture cur, int i, int p, int pt, int t, short cp, short b)
			{
				this.currency = new Texture (cur);
				this.id = i;
				this.price = p;
				this.pitch = pt;
				this.time = t;
				this.chargeprice = cp;
				this.buyable = b;
				namelabel = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.MINESHAFT);
				pricelabel = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.MINESHAFT);

				ItemData item = ItemData.get (id);

				if (item.is_valid ())
				{
					icon = new Texture (item.get_icon (false));
					namelabel.change_text (item.get_name ());
				}

				string mesostr = Convert.ToString (price);
				string_format.split_number (mesostr);
				pricelabel.change_text (mesostr + "meso");
			}

			public void draw (Point_short pos)
			{
				icon.draw (pos + new Point_short (0, 42));
				namelabel.draw (pos + new Point_short (40, 6));
				currency.draw (pos + new Point_short (38, 29));
				pricelabel.draw (pos + new Point_short (55, 24));
			}

			public int get_id ()
			{
				return id;
			}

			public short get_buyable ()
			{
				return buyable;
			}
			
			public Texture get_icon ()
			{
				return icon;
			}
            public int get_chargePrice()
            {
                return chargeprice;
            }

            private Texture icon = new Texture ();
			private Texture currency = new Texture ();
			private int id;
			private int price;
			private int pitch;
			private int time;
			private short chargeprice;
			private short buyable;
			private Text namelabel = new Text ();
			private Text pricelabel = new Text ();
		}

		public class SellItem
		{
			public SellItem (int item_id, short count, short s, bool sc, Texture cur)
			{
				ItemData idata = ItemData.get (item_id);

				icon = new Texture (idata.get_icon (false));
				id = item_id;
				sellable = count;
				slot = s;
				showcount = sc;

				namelabel = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.MINESHAFT);
				pricelabel = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.MINESHAFT);

				string name = idata.get_name ();

				if (name.Length >= 28)
				{
					name = name.Substring (0, 28) + "..";
				}

				namelabel.change_text (name);

				int price = idata.get_price ();
				string mesostr = Convert.ToString (price);
				string_format.split_number (mesostr);
				pricelabel.change_text (mesostr + "meso");
			}

			public void draw (Point_short pos)
			{
				icon.draw (pos + new Point_short (43, 42));

				if (showcount)
				{
					Charset countset = new Charset (ms.wz.wzFile_ui["Basic.img"]["ItemNo"], Charset.Alignment.LEFT);
					countset.draw (Convert.ToString (sellable), pos + new Point_short (41, 28));
				}

				namelabel.draw (pos + new Point_short (84, 6));
				pricelabel.draw (pos + new Point_short (84, 24));
			}

			public int get_id ()
			{
				return id;
			}

			public short get_slot ()
			{
				return slot;
			}

			public short get_sellable ()
			{
				return sellable;
			}

			public Texture get_icon ()
			{
				return icon;
			}
			
			private Texture icon = new Texture ();
			private int id;
			private short slot;
			private short sellable;
			private bool showcount;
			private Text namelabel = new Text ();
			private Text pricelabel = new Text ();
		}

		public class BuyState
		{
			public ObservableList<BuyItem> items = new ObservableList<BuyItem> ();
			public short offset;
			public short lastslot;
			public short selection;

			public void reset ()
			{
				items.Clear ();

				offset = 0;
				lastslot = 0;
				selection = -1;

                _fgui_Shop?.Refresh();
            }

			public void draw (Point_short parentpos, Texture selected)
			{
				for (short i = 0; i < 9; i++)
				{
					short slot = (short)(i + offset);

					if (slot >= lastslot)
					{
						break;
					}

					var itempos = new Point_short (12, (short)(116 + 42 * i));

					if (slot == selection)
					{
						selected.draw (parentpos + itempos + new Point_short (35, 8));
					}

					items[slot].draw (parentpos + itempos);
				}
			}

			public void show_item (short slot)
			{
				short absslot = (short)(slot + offset);

				if (absslot < 0 || absslot >= lastslot)
				{
					return;
				}

				int itemid = items[absslot].get_id ();
				UI.get ().show_item (Tooltip.Parent.SHOP, itemid);
			}

			public void add (BuyItem item)
			{
				items.Add (item);

				lastslot++;

                _fgui_Shop?.Refresh();

            }

            public void buy ()
			{
				if (selection < 0 || selection >= lastslot)
				{
					return;
				}

				BuyItem item = items[selection];
				short buyable = item.get_buyable ();
				short slot = selection;
				int itemid = item.get_id ();

				if (buyable > 1)
				{
					const string question = "How many are you willing to buy?";

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Lambda expressions cannot be assigned to 'var':
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Only lambda expressions having all locals passed by reference can be converted to C#:
//ORIGINAL LINE: var onenter = [slot, itemid](int qty)
					Action<int> onenter = (int qty) =>
					{
						var shortqty = (short)qty;

						new NpcShopActionPacket (slot, itemid, shortqty, true).dispatch ();
					};

					ms_Unity.FGUI_EnterNumber.ShowNotice (question, onenter, buyable, 1);
					//UI.get ().emplace<UIEnterNumber> (question, onenter, buyable, 1);
				}
				else if (buyable > 0)
				{
					const string question = "Are you sure you want to buy it?";

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Lambda expressions cannot be assigned to 'var':
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Only lambda expressions having all locals passed by reference can be converted to C#:
//ORIGINAL LINE: var ondecide = [slot, itemid](bool yes)
					Action<bool> ondecide = (bool yes) =>
					{
						if (yes)
						{
							new NpcShopActionPacket (slot, itemid, 1, true).dispatch ();
						}
					};
					ms_Unity.FGUI_YesNo.ShowNotice (question, ondecide);
					//UI.get ().emplace<UIYesNo> (question, ondecide);
				}
			}

			public void select (short selected)
			{
				short slot = (short)(selected + offset);

				if (slot == selection)
				{
					buy ();
				}
				else
				{
					selection = slot;
				}
			}
		}

		public BuyState buystate = new BuyState ();

		public class SellState
		{
			public ObservableList<SellItem> items = new ObservableList<SellItem>();
			//public List<SellItem> items = new List<SellItem> ();
			public short offset;
			public InventoryType.Id tab;
			public short lastslot;
			public short selection;

			public void reset ()
            {
                AppDebug.Log($"reset items:{items.GetHashCode()}");
                items.Clear ();
                offset = 0;
                lastslot = 0;
                selection = -1;
				tab = InventoryType.Id.NONE;

				_fgui_Shop?.Refresh();
			}

			public void change_tab (Inventory inventory, InventoryType.Id newtab, Texture meso)
			{
				tab = newtab;

				offset = 0;

				items.Clear ();

				short slots = inventory.get_slotmax (tab);

				for (short i = 1; i <= slots; i++)
				{
					int item_id = inventory.get_item_id (tab, i);
					if (item_id != 0)
					{
						short count = inventory.get_item_count (tab, i);
						items.Add (new SellItem (item_id, count, i, tab != InventoryType.Id.EQUIP, meso));
					}
				}

				lastslot = (short)items.Count;

                _fgui_Shop?.Refresh();
            }

			public void draw (Point_short parentpos, Texture selected)
			{
				for (short i = 0; i <= 8; i++)
				{
					short slot = (short)(i + offset);

					if (slot >= lastslot)
					{
						break;
					}

					Point_short itempos = new Point_short (243, (short)(116 + 42 * i));

					if (slot == selection)
					{
						selected.draw (parentpos + itempos + new Point_short (78, 8));
					}

					items[slot].draw (parentpos + itempos);
				}
			}

			public void show_item (short slot)
			{
				short absslot = (short)(slot + offset);

				if (absslot < 0 || absslot >= lastslot)
				{
					return;
				}

				if (tab == InventoryType.Id.EQUIP)
				{
					short realslot = items[absslot].get_slot ();
					UI.get ().show_equip (Tooltip.Parent.SHOP, realslot);
				}
				else
				{
					int itemid = items[absslot].get_id ();
					UI.get ().show_item (Tooltip.Parent.SHOP, itemid);
				}
			}

			public void sell (bool skip_confirmation)
			{
				if (selection < 0 || selection >= lastslot)
				{
					return;
				}

				SellItem item = items[selection];
				int itemid = item.get_id ();
				short sellable = item.get_sellable ();
				short slot = item.get_slot ();

				if (sellable > 1)
				{
					const string question = "How many are you willing to sell?";

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Lambda expressions cannot be assigned to 'var':
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Only lambda expressions having all locals passed by reference can be converted to C#:
//ORIGINAL LINE: var onenter = [itemid, slot](int qty)
					Action<int> onenter = (int qty) =>
					{
						var shortqty = (short)qty;

						new NpcShopActionPacket (slot, itemid, shortqty, false).dispatch ();
					};

					ms_Unity.FGUI_EnterNumber.ShowNotice (question, onenter, sellable, 1);
					//UI.get ().emplace<UIEnterNumber> (question, onenter, sellable, 1);
				}
				else if (sellable > 0)
				{
					if (skip_confirmation)
					{
						new NpcShopActionPacket (slot, itemid, 1, false).dispatch ();
						return;
					}

					const string question = "Are you sure you want to sell it?";

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Lambda expressions cannot be assigned to 'var':
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Only lambda expressions having all locals passed by reference can be converted to C#:
//ORIGINAL LINE: var ondecide = [itemid, slot](bool yes)
					Action<bool> ondecide = (bool yes) =>
					{
						if (yes)
						{
							new NpcShopActionPacket (slot, itemid, 1, false).dispatch ();
						}
					};
					ms_Unity.FGUI_YesNo.ShowNotice (question, ondecide);
					//UI.get ().emplace<UIYesNo> (question, ondecide);
				}
			}

            public void recharge()
            {
                if (selection < 0 || selection >= lastslot)
                {
                    return;
                }

                SellItem item = items[selection];
                int itemid = item.get_id();
                short sellable = item.get_sellable();
                short slot = item.get_slot();

                const string question = "确定要充值吗?";
                Action<bool> ondecide = (bool yes) =>
                {
                    if (yes)
                    {
                        new NpcShopActionPacket(slot).dispatch();
                    }
                };
                ms_Unity.FGUI_YesNo.ShowNotice(question, ondecide);
                
            }

            public void select (short selected)
			{
				short slot = (short)(selected + offset);

				if (slot == selection)
				{
					sell (false);
				}
				else
				{
					selection = slot;
				}
			}
		}

		public SellState sellstate = new SellState ();
	}
}


#if USE_NX
#endif