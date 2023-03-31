using System;
using System.Collections.Generic;
using System.Linq;
using Beebyte.Obfuscator;
using Helper;
using Loxodon.Framework.Observables;
using MapleLib.WzLib;

namespace ms
{
	[Skip]
	// The Item inventory
	public class UIItemInventory : UIDragElement<PosINV>
	{
		public const Type TYPE = UIElement.Type.ITEMINVENTORY;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public UIItemInventory (params object[] args) : this ((Inventory)args[0])
		{
		}

		public UIItemInventory (Inventory invent)
		{
			this.inventory = invent;
			this.ignore_tooltip = false;
			this.tab = InventoryType.Id.EQUIP;
			this.sort_enabled = false;
			WzObject Item = ms.wz.wzFile_ui["UIWindow2.img"]["Item"];

			//backgrnd = Item["productionBackgrnd"];//todo 2 Width height 1,1, it's outlink fault
			backgrnd = Item["backgrnd"];
			backgrnd2 = Item["productionBackgrnd2"];
			backgrnd3 = Item["productionBackgrnd3"];

			full_backgrnd = Item["FullBackgrnd"];
			full_backgrnd2 = Item["FullBackgrnd2"];
			full_backgrnd3 = Item["FullBackgrnd3"];

			bg_dimensions = new Point_short (backgrnd.get_dimensions ());
			bg_full_dimensions = new Point_short (full_backgrnd.get_dimensions ());

			WzObject New = Item["New"];
			newitemslot = New["inventory"];
			newitemtab = New["Tab0"];

			projectile = Item["activeIcon"];
			disabled = Item["disabled"];

			WzObject Tab = Item["Tab"];
			WzObject taben = Tab["enabled"];
			WzObject tabdis = Tab["disabled"];

			WzObject close = ms.wz.wzFile_ui["Basic.img"]["BtClose3"];
			buttons[(int)Buttons.BT_CLOSE] = new MapleButton (close);

			buttons[(int)Buttons.BT_TAB_EQUIP] = new TwoSpriteButton (tabdis["0"], taben["0"]);
			buttons[(int)Buttons.BT_TAB_USE] = new TwoSpriteButton (tabdis["1"], taben["1"]);
			buttons[(int)Buttons.BT_TAB_ETC] = new TwoSpriteButton (tabdis["2"], taben["2"]);
			buttons[(int)Buttons.BT_TAB_SETUP] = new TwoSpriteButton (tabdis["3"], taben["3"]);
			buttons[(int)Buttons.BT_TAB_CASH] = new TwoSpriteButton (tabdis["4"], taben["4"]);

			buttons[(int)Buttons.BT_COIN] = new MapleButton (Item["BtCoin3"]);
			buttons[(int)Buttons.BT_POINT] = new MapleButton (Item["BtPoint0"]);
			buttons[(int)Buttons.BT_GATHER] = new MapleButton (Item["BtGather3"]);
			buttons[(int)Buttons.BT_SORT] = new MapleButton (Item["BtSort3"]);
			buttons[(int)Buttons.BT_FULL] = new MapleButton (Item["BtFull3"]);
			buttons[(int)Buttons.BT_SMALL] = new MapleButton (Item["BtSmall3"]);
			buttons[(int)Buttons.BT_POT] = new MapleButton (Item["BtPot3"]);
			buttons[(int)Buttons.BT_UPGRADE] = new MapleButton (Item["BtUpgrade3"]);
			buttons[(int)Buttons.BT_APPRAISE] = new MapleButton (Item["BtAppraise3"]);
			buttons[(int)Buttons.BT_EXTRACT] = new MapleButton (Item["BtExtract3"]);
			buttons[(int)Buttons.BT_DISASSEMBLE] = new MapleButton (Item["BtDisassemble3"]);
			buttons[(int)Buttons.BT_TOAD] = new MapleButton (Item["BtToad3"]);

			buttons[(int)Buttons.BT_COIN_SM] = new MapleButton (Item["BtCoin4"]);
			buttons[(int)Buttons.BT_POINT_SM] = new MapleButton (Item["BtPoint1"]);
			buttons[(int)Buttons.BT_GATHER_SM] = new MapleButton (Item["BtGather4"]);
			buttons[(int)Buttons.BT_SORT_SM] = new MapleButton (Item["BtSort4"]);
			buttons[(int)Buttons.BT_FULL_SM] = new MapleButton (Item["BtFull4"]);
			buttons[(int)Buttons.BT_SMALL_SM] = new MapleButton (Item["BtSmall4"]);
			buttons[(int)Buttons.BT_POT_SM] = new MapleButton (Item["BtPot4"]);
			buttons[(int)Buttons.BT_UPGRADE_SM] = new MapleButton (Item["BtUpgrade4"]);
			buttons[(int)Buttons.BT_APPRAISE_SM] = new MapleButton (Item["BtAppraise4"]);
			buttons[(int)Buttons.BT_EXTRACT_SM] = new MapleButton (Item["BtExtract4"]);
			buttons[(int)Buttons.BT_DISASSEMBLE_SM] = new MapleButton (Item["BtDisassemble4"]);
			buttons[(int)Buttons.BT_TOAD_SM] = new MapleButton (Item["BtToad4"]);
			buttons[(int)Buttons.BT_CASHSHOP] = new MapleButton (Item["BtCashshop"]);

			buttons[(int)Buttons.BT_POT].set_state (Button.State.DISABLED);
			buttons[(int)Buttons.BT_POT_SM].set_state (Button.State.DISABLED);
			buttons[(int)Buttons.BT_EXTRACT].set_state (Button.State.DISABLED);
			buttons[(int)Buttons.BT_EXTRACT_SM].set_state (Button.State.DISABLED);
			buttons[(int)Buttons.BT_DISASSEMBLE].set_state (Button.State.DISABLED);
			buttons[(int)Buttons.BT_DISASSEMBLE_SM].set_state (Button.State.DISABLED);
			buttons[button_by_tab (tab)].set_state (Button.State.PRESSED);

			mesolabel = new Text (Text.Font.A11M, Text.Alignment.RIGHT, Color.Name.BLACK);
			maplepointslabel = new Text (Text.Font.A11M, Text.Alignment.RIGHT, Color.Name.BLACK);
			maplepointslabel.change_text ("0"); // TODO: Implement

			slotrange[InventoryType.Id.EQUIPPED] = new ValueTuple<short, short> (1, 24);
			slotrange[InventoryType.Id.EQUIP] = new ValueTuple<short, short> (1, 24);
			slotrange[InventoryType.Id.USE] = new ValueTuple<short, short> (1, 24);
			slotrange[InventoryType.Id.SETUP] = new ValueTuple<short, short> (1, 24);
			slotrange[InventoryType.Id.ETC] = new ValueTuple<short, short> (1, 24);
			slotrange[InventoryType.Id.CASH] = new ValueTuple<short, short> (1, 24);

			slider = new Slider ((int)Slider.Type.DEFAULT_SILVER, new Range_short (50, 245), 152, 6, rm: (short)(1 + inventory.get_slotmax (tab) / COLUMNS), (bool upwards) =>
			{
				short shift = (short)(upwards ? -COLUMNS : COLUMNS);
				bool above = slotrange[tab].Item1 + shift > 0;
				bool below = slotrange[tab].Item2 + shift < inventory.get_slotmax (tab) + 1 + COLUMNS;

				if (above && below)
				{
					var oldValue = slotrange[tab];
					oldValue.Item1 += shift;
					oldValue.Item2 += shift;
					slotrange[tab] = oldValue;
				}
			});

			set_full (false);
			clear_new ();
			load_icons ();


		}

		public override void draw (float alpha)
		{
			base.draw_sprites (alpha);

			Point_short mesolabel_pos = position + new Point_short (127, 262);
			Point_short maplepointslabel_pos = position + new Point_short (159, 279);

			if (full_enabled)
			{
				full_backgrnd.draw (position);
				full_backgrnd2.draw (position);
				full_backgrnd3.draw (position);

				mesolabel.draw (mesolabel_pos + new Point_short (3, 70));
				maplepointslabel.draw (maplepointslabel_pos + new Point_short (181, 53));
			}
			else
			{
				backgrnd.draw (position);
				backgrnd2.draw (position);
				backgrnd3.draw (position);

				slider.draw (position + new Point_short (0, 1));

				mesolabel.draw (mesolabel_pos);
				maplepointslabel.draw (maplepointslabel_pos);
			}

			var range = slotrange[tab];

			uint numslots = inventory.get_slotmax (tab);
			uint firstslot = (uint)(full_enabled ? 1 : range.Item1);
			uint lastslot = full_enabled ? MAXFULLSLOTS : (uint)range.Item2;

			for (short i = 0; i <= MAXFULLSLOTS; i++)//position:300,160
			{
				Point_short slotpos = full_enabled ? get_full_slotpos ((short)i) : get_slotpos (i);

				if (icons.ContainsKey (i))
				{
					var icon = icons[i];

					if (icon != null && i >= firstslot && i <= lastslot)
					{
						icon.draw (position + slotpos);
					}
				}
				else
				{
					if (i > numslots && i <= lastslot)
					{
						disabled.draw (position + slotpos);
					}
				}
			}

			short bulletslot = inventory.get_bulletslot ();

			if (tab == InventoryType.Id.USE && is_visible (bulletslot))
			{
				projectile.draw (position + get_slotpos (bulletslot));
			}

			if (tab == newtab)
			{
				newitemtab.draw (position + get_tabpos (newtab), alpha);

				if (is_visible (newslot))
				{
					newitemslot.draw (position + get_slotpos (newslot) + new Point_short (1, 1), alpha);
				}
			}

			base.draw_buttons (alpha);
		}

		public override void update ()
		{
			base.update ();

			newitemtab.update (6);
			newitemslot.update (6);

			string meso_str = Convert.ToString (inventory.get_meso ());
			string_format.split_number (meso_str);

			mesolabel.change_text (meso_str);
		}

		public override void doubleclick (Point_short cursorpos)
		{
			short slot = slot_by_position (cursorpos - position);

			if (icons.Any (pair => pair.Key == slot) && is_visible (slot))
			{
				int item_id = inventory.get_item_id (tab, slot);
				if (item_id != 0)
				{
					switch (tab)
					{
						case InventoryType.Id.EQUIP:
							{
								if (can_wear_equip (slot))
								{
									new EquipItemPacket (slot, inventory.find_equipslot (item_id)).dispatch ();
								}

								break;
							}
						case InventoryType.Id.USE:
							{
								new UseItemPacket (slot, item_id).dispatch ();
								break;
							}
					}
				}
			}
		}

		public override bool send_icon (Icon icon, Point_short cursorpos)
		{
			short slot = slot_by_position (cursorpos - position);

			if (slot > 0)
			{
				int item_id = inventory.get_item_id (tab, slot);
				EquipSlot.Id eqslot;
				bool equip;

				if (item_id != 0 && tab == InventoryType.Id.EQUIP)
				{
					eqslot = inventory.find_equipslot (item_id);
					equip = true;
				}
				else
				{
					eqslot = EquipSlot.Id.NONE;
					equip = false;
				}

				ignore_tooltip = true;

				return icon.drop_on_items (tab, eqslot, slot, equip);
			}

			return true;
		}

		public override void toggle_active ()
		{
			base.toggle_active ();

			if (!active)
			{
				clear_new ();
				clear_tooltip ();
			}
		}

		public override void remove_cursor ()
		{
			base.remove_cursor ();

			slider.remove_cursor ();
		}

		public override Cursor.State send_cursor (bool pressed, Point_short cursorpos)
		{
			Cursor.State dstate = base.send_cursor (pressed, new Point_short (cursorpos));

			if (dragged)
			{
				clear_tooltip ();

				return dstate;
			}

			Point_short cursor_relative = cursorpos - position;

			if (!full_enabled && slider.isenabled ())
			{
				Cursor.State sstate = slider.send_cursor (new Point_short (cursor_relative), pressed);

				if (sstate != Cursor.State.IDLE)
				{
					clear_tooltip ();

					return sstate;
				}
			}

			short slot = slot_by_position (new Point_short (cursor_relative));
			Icon icon = get_icon (slot);
			bool is_icon = icon != null && is_visible (slot);

			if (is_icon)
			{
				if (pressed)
				{
					Point_short slotpos = get_slotpos (slot);
					icon.start_drag (cursor_relative - slotpos);
					UI.get ().drag_icon (icon);

					clear_tooltip ();

					AppDebug.Log ($"drag_icon:{icon.get_texture ().fullPath}");
					return Cursor.State.GRABBING;
				}
				else if (!ignore_tooltip)
				{
					show_item (slot);

					return Cursor.State.CANGRAB;
				}
				else
				{
					ignore_tooltip = false;

					return Cursor.State.CANGRAB;
				}
			}
			else
			{
				clear_tooltip ();

				return base.send_cursor (pressed, new Point_short (cursorpos));
			}
		}

		public override void send_key (int keycode, bool pressed, bool escape)
		{
			if (pressed)
			{
				if (escape)
				{
					toggle_active ();
				}
				else if (keycode == (int)KeyAction.Id.TAB)
				{
					clear_tooltip ();

					InventoryType.Id newtab = InventoryType.Id.NONE;

					switch (tab)
					{
						case InventoryType.Id.EQUIP:
							newtab = InventoryType.Id.USE;
							break;
						case InventoryType.Id.USE:
							newtab = InventoryType.Id.ETC;
							break;
						case InventoryType.Id.ETC:
							newtab = InventoryType.Id.SETUP;
							break;
						case InventoryType.Id.SETUP:
							newtab = InventoryType.Id.CASH;
							break;
						case InventoryType.Id.CASH:
							newtab = InventoryType.Id.EQUIP;
							break;
					}

					button_pressed (button_by_tab (newtab));
				}
			}
		}

		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public void modify (InventoryType.Id type, short slot, sbyte mode, short arg)
		{
			if (slot <= 0)
			{
				return;
			}

			if (type == tab)
			{
				switch ((Inventory.Modification)mode)
				{
					case Inventory.Modification.ADD:
						{
							update_slot (slot);

							newtab = type;
							newslot = slot;
							break;
						}
					case Inventory.Modification.CHANGECOUNT:
					case Inventory.Modification.ADDCOUNT:
						{
							var icon = get_icon (slot);
							if (icon != null)
							{
								icon.set_count (arg);
							}

							break;
						}
					case Inventory.Modification.SWAP:
						{
							if (arg != slot)
							{
								update_slot (slot);
								update_slot (arg);
							}

							break;
						}
					case Inventory.Modification.REMOVE:
						{
							update_slot (slot);
							break;
						}
				}
			}

			switch ((Inventory.Modification)mode)
			{
				case Inventory.Modification.ADD:
				case Inventory.Modification.ADDCOUNT:
					{
						newtab = type;
						newslot = slot;
						break;
					}
				case Inventory.Modification.CHANGECOUNT:
				case Inventory.Modification.SWAP:
				case Inventory.Modification.REMOVE:
					{
						if (newslot == slot && newtab == type)
						{
							clear_new ();
						}

						break;
					}
			}
		}

		public void set_sort (bool enabled)
		{
			sort_enabled = enabled;

			if (full_enabled)
			{
				if (sort_enabled)
				{
					buttons[(int)Buttons.BT_SORT].set_active (false);
					buttons[(int)Buttons.BT_SORT_SM].set_active (true);
					buttons[(int)Buttons.BT_GATHER].set_active (false);
					buttons[(int)Buttons.BT_GATHER_SM].set_active (false);
				}
				else
				{
					buttons[(int)Buttons.BT_SORT].set_active (false);
					buttons[(int)Buttons.BT_SORT_SM].set_active (false);
					buttons[(int)Buttons.BT_GATHER].set_active (false);
					buttons[(int)Buttons.BT_GATHER_SM].set_active (true);
				}
			}
			else
			{
				if (sort_enabled)
				{
					buttons[(int)Buttons.BT_SORT].set_active (true);
					buttons[(int)Buttons.BT_SORT_SM].set_active (false);
					buttons[(int)Buttons.BT_GATHER].set_active (false);
					buttons[(int)Buttons.BT_GATHER_SM].set_active (false);
				}
				else
				{
					buttons[(int)Buttons.BT_SORT].set_active (false);
					buttons[(int)Buttons.BT_SORT_SM].set_active (false);
					buttons[(int)Buttons.BT_GATHER].set_active (true);
					buttons[(int)Buttons.BT_GATHER_SM].set_active (false);
				}
			}
		}

		public void change_tab (InventoryType.Id type)
		{
			button_pressed (button_by_tab (type));
		}

		public void clear_new ()
		{
			newtab = InventoryType.Id.NONE;
			newslot = 0;
		}

		public override Button.State button_pressed (ushort buttonid)
		{
			InventoryType.Id oldtab = tab;

			switch ((Buttons)buttonid)
			{
				case Buttons.BT_CLOSE:
					{
						toggle_active ();

						return Button.State.NORMAL;
					}
				case Buttons.BT_TAB_EQUIP:
					{
						tab = InventoryType.Id.EQUIP;
						break;
					}
				case Buttons.BT_TAB_USE:
					{
						tab = InventoryType.Id.USE;
						break;
					}
				case Buttons.BT_TAB_SETUP:
					{
						tab = InventoryType.Id.SETUP;
						break;
					}
				case Buttons.BT_TAB_ETC:
					{
						tab = InventoryType.Id.ETC;
						break;
					}
				case Buttons.BT_TAB_CASH:
					{
						tab = InventoryType.Id.CASH;
						break;
					}
				case Buttons.BT_GATHER:
				case Buttons.BT_GATHER_SM:
					{
						new GatherItemsPacket (tab).dispatch ();
						break;
					}
				case Buttons.BT_SORT:
				case Buttons.BT_SORT_SM:
					{
						new SortItemsPacket (tab).dispatch ();
						break;
					}
				case Buttons.BT_FULL:
				case Buttons.BT_FULL_SM:
					{
						set_full (true);

						return Button.State.NORMAL;
					}
				case Buttons.BT_SMALL:
				case Buttons.BT_SMALL_SM:
					{
						set_full (false);

						return Button.State.NORMAL;
					}
				case Buttons.BT_COIN:
				case Buttons.BT_COIN_SM:
				case Buttons.BT_POINT:
				case Buttons.BT_POINT_SM:
				case Buttons.BT_POT:
				case Buttons.BT_POT_SM:
				case Buttons.BT_UPGRADE:
				case Buttons.BT_UPGRADE_SM:
				case Buttons.BT_APPRAISE:
				case Buttons.BT_APPRAISE_SM:
				case Buttons.BT_EXTRACT:
				case Buttons.BT_EXTRACT_SM:
				case Buttons.BT_DISASSEMBLE:
				case Buttons.BT_DISASSEMBLE_SM:
				case Buttons.BT_TOAD:
				case Buttons.BT_TOAD_SM:
				case Buttons.BT_CASHSHOP:
					{
						return Button.State.NORMAL;
					}
			}

			if (tab != oldtab)
			{
				ushort row = (ushort)(slotrange[tab].Item1 / COLUMNS);
				slider.setrows ((short)row, 6, (short)(1 + inventory.get_slotmax (tab) / COLUMNS));

				buttons[button_by_tab (oldtab)].set_state (Button.State.NORMAL);
				buttons[button_by_tab (tab)].set_state (Button.State.PRESSED);

				load_icons ();
				set_sort (false);
			}

			return Button.State.IDENTITY;
		}

		private void show_item (short slot)
		{
			if (tab == InventoryType.Id.EQUIP)
			{
				UI.get ().show_equip (Tooltip.Parent.ITEMINVENTORY, slot);
			}
			else
			{
				int item_id = inventory.get_item_id (tab, slot);
				UI.get ().show_item (Tooltip.Parent.ITEMINVENTORY, item_id);
			}
		}

		private void clear_tooltip ()
		{
			UI.get ().clear_tooltip (Tooltip.Parent.ITEMINVENTORY);
		}

		private void load_icons ()
		{
			icons.Clear ();

			byte numslots = inventory.get_slotmax (tab);

			for (short i = 0; i <= MAXFULLSLOTS; i++)
			{
				if (i <= numslots)
				{
					update_slot (i);
				}
			}
		}

		private void update_slot (short slot)
		{
			int item_id = inventory.get_item_id (tab, slot);
			if (item_id != 0)
			{
				short count;

				if (tab == InventoryType.Id.EQUIP)
				{
					count = -1;
				}
				else
				{
					count = inventory.get_item_count (tab, slot);
				}

				bool untradable = ItemData.get (item_id).is_untradable ();
				bool cashitem = ItemData.get (item_id).is_cashitem ();
				Texture texture = new Texture (ItemData.get (item_id).get_icon (false));
				EquipSlot.Id eqslot = inventory.find_equipslot (item_id);

				icons[slot] = new Icon (new ItemIcon (this, tab, eqslot, slot, item_id, count, untradable, cashitem), texture, count);
			}
			else if (icons.count (slot) > 0)
			{
				icons.Remove (slot);
			}
		}

		private bool is_visible (short slot)
		{
			return !is_not_visible (slot);
		}

		private bool is_not_visible (short slot)
		{
			var range = slotrange[tab];

			if (full_enabled)
			{
				return slot < 1 || slot > 24;
			}
			else
			{
				return slot < range.Item1 || slot > range.Item2;
			}
		}

		public bool can_wear_equip (short slot)
		{
			Player player = Stage.get ().get_player ();
			CharStats stats = player.get_stats ();
			CharLook look = player.get_look ();
			bool alerted = look.get_alerted ();

			if (alerted)
			{
				ms_Unity.FGUI_OK.ShowNotice ("You cannot complete this action right now.\\nEvade the attack and try again.", null);
				//UI.get ().emplace<UIOk> ("You cannot complete this action right now.\\nEvade the attack and try again.", null);
				return false;
			}

			int item_id = inventory.get_item_id (InventoryType.Id.EQUIP, slot);
			EquipData equipdata = EquipData.get (item_id);
			ItemData itemdata = equipdata.get_itemdata ();

			sbyte reqGender = itemdata.get_gender ();
			bool female = stats.get_female ();

			switch (reqGender)
			{
				// Male
				case 0:
					{
						if (female)
						{
							return false;
						}

						break;
					}
				// Female
				case 1:
					{
						if (!female)
						{
							return false;
						}

						break;
					}
				// Unisex
				case 2:
				default:
					{
						break;
					}
			}

			string jobname = stats.get_jobname ();

			if (jobname == "GM" || jobname == "SuperGM")
			{
				return true;
			}

			short reqJOB = equipdata.get_reqstat (MapleStat.Id.JOB);

			if (!stats.get_job ().is_EquipRequiredJob ((ushort)reqJOB))
			{
				ms_Unity.FGUI_OK.ShowNotice ("Your current job\\ncannot equip the selected item.", null);
				//UI.get ().emplace<UIOk> ("Your current job\\ncannot equip the selected item.", null);
				return false;
			}

			short reqLevel = equipdata.get_reqstat (MapleStat.Id.LEVEL);
			short reqDEX = equipdata.get_reqstat (MapleStat.Id.DEX);
			short reqSTR = equipdata.get_reqstat (MapleStat.Id.STR);
			short reqLUK = equipdata.get_reqstat (MapleStat.Id.LUK);
			short reqINT = equipdata.get_reqstat (MapleStat.Id.INT);
			short reqFAME = equipdata.get_reqstat (MapleStat.Id.FAME);

			sbyte i = 0;

			if (reqLevel > stats.get_stat (MapleStat.Id.LEVEL))
			{
				i++;
			}
			else if (reqDEX > stats.get_total (EquipStat.Id.DEX))
			{
				i++;
			}
			else if (reqSTR > stats.get_total (EquipStat.Id.STR))
			{
				i++;
			}
			else if (reqLUK > stats.get_total (EquipStat.Id.LUK))
			{
				i++;
			}
			else if (reqINT > stats.get_total (EquipStat.Id.INT))
			{
				i++;
			}
			else if (reqFAME > stats.get_honor ())
			{
				i++;
			}

			if (i > 0)
			{
				ms_Unity.FGUI_OK.ShowNotice ("Your stats are too low to equip this item\\nor you do not meet the job requirement.");
				//UI.get ().emplace<UIOk> ("Your stats are too low to equip this item\\nor you do not meet the job requirement.", null);
				return false;
			}

			return true;
		}

		private short slot_by_position (Point_short cursorpos)
		{
			short xoff = (short)(cursorpos.x () - 11);
			short yoff = (short)(cursorpos.y () - 51);

			if (xoff < 1 || xoff > 143 || yoff < 1)
			{
				return 0;
			}

			short slot = (short)((full_enabled ? 1 : slotrange[tab].Item1) + (xoff / ICON_WIDTH) + COLUMNS * (yoff / ICON_HEIGHT));

			return (short)(is_visible (slot) ? slot : 0);
		}

		private ushort button_by_tab (InventoryType.Id tb)
		{
			switch ((InventoryType.Id)tb)
			{
				case InventoryType.Id.EQUIP:
					return (ushort)Buttons.BT_TAB_EQUIP;
				case InventoryType.Id.USE:
					return (ushort)Buttons.BT_TAB_USE;
				case InventoryType.Id.SETUP:
					return (ushort)Buttons.BT_TAB_SETUP;
				case InventoryType.Id.ETC:
					return (ushort)Buttons.BT_TAB_ETC;
				default:
					return (ushort)Buttons.BT_TAB_CASH;
			}
		}

		private Point_short get_slotpos (short slot)
		{
			short absslot = (short)(slot - slotrange[tab].Item1);

			var x = (short)(10 + (absslot % COLUMNS) * ICON_WIDTH);//-26
			var y = (short)(51 + (absslot / COLUMNS) * ICON_HEIGHT);//51
			return new Point_short ((short)(10 + (absslot % COLUMNS) * ICON_WIDTH), (short)(51 + (absslot / COLUMNS) * ICON_HEIGHT));
		}

		private Point_short get_full_slotpos (short slot)
		{
			short absslot = (short)(slot - 1);
			std.div_t div = std.div (absslot, MAXSLOTS);
			short new_slot = (short)(absslot - (div.quot * MAXSLOTS));
			short adj_x = (short)(div.quot * COLUMNS * ICON_WIDTH);

			return new Point_short ((short)(10 + adj_x + (new_slot % COLUMNS) * ICON_WIDTH), (short)(51 + (new_slot / COLUMNS) * ICON_HEIGHT));
		}

		private Point_short get_tabpos (InventoryType.Id tb)
		{
			short fixed_tab = (short)tb;

			switch ((InventoryType.Id)tb)
			{
				case InventoryType.Id.ETC:
					fixed_tab = 3;
					break;
				case InventoryType.Id.SETUP:
					fixed_tab = 4;
					break;
			}

			return new Point_short ((short)(10 + ((fixed_tab - 1) * 31)), 29);
		}

		private Icon get_icon (short slot)
		{
			return icons.TryGetValue (slot);
		}

		private void set_full (bool enabled)
		{
			full_enabled = enabled;

			if (full_enabled)
			{
				dimension = new Point_short (bg_full_dimensions);

				buttons[(int)Buttons.BT_FULL].set_active (false);
				buttons[(int)Buttons.BT_FULL_SM].set_active (false);
				buttons[(int)Buttons.BT_SMALL].set_active (false);
				buttons[(int)Buttons.BT_SMALL_SM].set_active (true);
			}
			else
			{
				dimension = new Point_short (bg_dimensions);

				buttons[(int)Buttons.BT_FULL].set_active (true);
				buttons[(int)Buttons.BT_FULL_SM].set_active (false);
				buttons[(int)Buttons.BT_SMALL].set_active (false);
				buttons[(int)Buttons.BT_SMALL_SM].set_active (false);
			}

			dragarea = new Point_short (dimension.x (), 20);

			short adj_x = (short)(full_enabled ? 20 : 22);
			buttons[(int)Buttons.BT_CLOSE].set_position (new Point_short ((short)(dimension.x () - adj_x), 6));

			buttons[(int)Buttons.BT_COIN].set_active (!enabled);
			buttons[(int)Buttons.BT_POINT].set_active (!enabled);
			buttons[(int)Buttons.BT_POT].set_active (!enabled);
			buttons[(int)Buttons.BT_UPGRADE].set_active (!enabled);
			buttons[(int)Buttons.BT_APPRAISE].set_active (!enabled);
			buttons[(int)Buttons.BT_EXTRACT].set_active (!enabled);
			buttons[(int)Buttons.BT_DISASSEMBLE].set_active (!enabled);
			buttons[(int)Buttons.BT_TOAD].set_active (!enabled);
			buttons[(int)Buttons.BT_CASHSHOP].set_active (!enabled);

			buttons[(int)Buttons.BT_COIN_SM].set_active (enabled);
			buttons[(int)Buttons.BT_POINT_SM].set_active (enabled);
			buttons[(int)Buttons.BT_POT_SM].set_active (enabled);
			buttons[(int)Buttons.BT_UPGRADE_SM].set_active (enabled);
			buttons[(int)Buttons.BT_APPRAISE_SM].set_active (enabled);
			buttons[(int)Buttons.BT_EXTRACT_SM].set_active (enabled);
			buttons[(int)Buttons.BT_DISASSEMBLE_SM].set_active (enabled);
			buttons[(int)Buttons.BT_TOAD_SM].set_active (enabled);
			buttons[(int)Buttons.BT_CASHSHOP].set_active (enabled);

			set_sort (sort_enabled);
			load_icons ();
		}

		public override void OnAdd ()
		{

		}
		public override void OnRemove ()
		{


		}

		public override void OnActivityChange (bool isActiveAfterChange)
		{
            ms_Unity.FGUI_Manager.Instance.PanelOpening = isActiveAfterChange;
            if (isActiveAfterChange)
			{
				//ms_Unity.Launcher.Instance.Open<ms_Unity.InventoryWindow> ();
				//ms_Unity.Launcher.Instance.OpenNew<ms_Unity.InventoryWindow> ();
				ms_Unity.FGUI_Manager.Instance.OpenFGUI<ms_Unity.FGUI_Inventory> ();
				//AppDebug.Log ("OnAdd FGUI_Inventory");
			}
			else
			{
				//ms_Unity.Launcher.Instance.Open<ms_Unity.InventoryWindow> ();
				//ms_Unity.Launcher.Instance.OpenNew<ms_Unity.InventoryWindow> ();
				ms_Unity.FGUI_Manager.Instance.CloseFGUI<ms_Unity.FGUI_Inventory> ();
				//AppDebug.Log ("OnRemove FGUI_Inventory");

			}
		}


		private const ushort ROWS = 8;
		private const ushort COLUMNS = 4;
		private const ushort MAXSLOTS = ROWS * COLUMNS;
		private const ushort MAXFULLSLOTS = COLUMNS * MAXSLOTS;
		private const ushort ICON_WIDTH = 36;
		private const ushort ICON_HEIGHT = 35;

		private enum Buttons
		{
			BT_CLOSE,
			BT_TAB_EQUIP,
			BT_TAB_USE,
			BT_TAB_ETC,
			BT_TAB_SETUP,
			BT_TAB_CASH,
			BT_COIN,
			BT_POINT,
			BT_GATHER,
			BT_SORT,
			BT_FULL,
			BT_SMALL,
			BT_POT,
			BT_UPGRADE,
			BT_APPRAISE,
			BT_EXTRACT,
			BT_DISASSEMBLE,
			BT_TOAD,
			BT_COIN_SM,
			BT_POINT_SM,
			BT_GATHER_SM,
			BT_SORT_SM,
			BT_FULL_SM,
			BT_SMALL_SM,
			BT_POT_SM,
			BT_UPGRADE_SM,
			BT_APPRAISE_SM,
			BT_EXTRACT_SM,
			BT_DISASSEMBLE_SM,
			BT_TOAD_SM,
			BT_CASHSHOP
		}

		private readonly Inventory inventory;

		private Animation newitemslot = new Animation ();
		private Animation newitemtab = new Animation ();
		private Texture projectile = new Texture ();
		private Texture disabled = new Texture ();
		private Text mesolabel = new Text ();
		private Text maplepointslabel = new Text ();
		private Slider slider = new Slider ();

		private ObservableSortedDictionary<short, Icon> icons = new ObservableSortedDictionary<short, Icon> ();
		private SortedDictionary<InventoryType.Id, System.ValueTuple<short, short>> slotrange = new SortedDictionary<InventoryType.Id, System.ValueTuple<short, short>> ();

		private InventoryType.Id tab;
		private InventoryType.Id newtab;
		private short newslot;
		private bool ignore_tooltip;

		private bool sort_enabled;
		private bool full_enabled;
		private Texture backgrnd;
		private Texture backgrnd2;
		private Texture backgrnd3;
		private Texture full_backgrnd;
		private Texture full_backgrnd2;
		private Texture full_backgrnd3;
		private Point_short bg_dimensions;
		private Point_short bg_full_dimensions;
	}

	public class ItemIcon : Icon.Type
	{
		public ItemIcon (UIItemInventory parent, InventoryType.Id st, EquipSlot.Id eqs, short s, int iid, short c, bool u, bool cash)
		{
			this.parent = parent;
			sourcetab = st;
			eqsource = eqs;
			source = s;
			item_id = iid;
			count = c;
			untradable = u;
			cashitem = cash;
		}

		public override void drop_on_stage ()
		{
			const string dropmessage = "How many will you drop?";
			const string untradablemessage = "This item can't be taken back once thrown away.\\nWill you still drop it?";
			const string cashmessage = "You can't drop this item.";

			if (cashitem)
			{
				//UI.get ().emplace<UIOk> (cashmessage, null);
				ms_Unity.FGUI_OK.ShowNotice (cashmessage);
			}
			else
			{
				if (untradable)
				{
					Action<bool> onok = (bool ok) =>
					{
						if (ok)
						{
							if (count <= 1)
							{
								new MoveItemPacket (sourcetab, source, 0, 1).dispatch ();
							}
							else
							{
								Action<int> onenter = (int qty) => { new MoveItemPacket (sourcetab, source, 0, (short)qty).dispatch (); };

								//UI.get ().emplace<UIEnterNumber> (dropmessage, onenter, count, count);
								ms_Unity.FGUI_EnterNumber.ShowNotice (dropmessage, onenter, count, count);
							}
						}
					};

					//UI.get ().emplace<UIYesNo> (untradablemessage, onok);
					ms_Unity.FGUI_YesNo.ShowNotice (untradablemessage, onok);

				}
				else
				{
					if (count <= 1)
					{
						new MoveItemPacket (sourcetab, source, 0, 1).dispatch ();
					}
					else
					{
						Action<int> onenter = (int qty) => { new MoveItemPacket (sourcetab, source, 0, (short)qty).dispatch (); };

						//UI.get ().emplace<UIEnterNumber> (dropmessage, onenter, count, count);

						ms_Unity.FGUI_EnterNumber.ShowNotice (dropmessage, onenter, count,  count);
					}
				}
			}
		}

		public override void drop_on_equips (EquipSlot.Id eqslot)
		{
			switch (sourcetab)
			{
				case InventoryType.Id.EQUIP:
					{
						if (eqsource == eqslot)
						{
							if (parent.can_wear_equip (source))
							{
								new EquipItemPacket (source, eqslot).dispatch ();
							}
						}

						new Sound (Sound.Name.DRAGEND).play ();
						break;
					}
				case InventoryType.Id.USE:
					{
						new ScrollEquipPacket (source, eqslot).dispatch ();
						break;
					}
			}
		}

		public override bool drop_on_items (InventoryType.Id tab, EquipSlot.Id eqslot, short slot, bool equip)
		{
			if (tab != sourcetab || slot == source)
			{
				return true;
			}

			new MoveItemPacket (tab, source, slot, 1).dispatch ();

			return true;
		}

		public override void drop_on_bindings (Point_short cursorposition, bool remove)
		{
			if (sourcetab == InventoryType.Id.USE || sourcetab == InventoryType.Id.SETUP)
			{
				var keyconfig = UI.get ().get_element<UIKeyConfig> ();
				Keyboard.Mapping mapping = new Keyboard.Mapping (KeyType.Id.ITEM, item_id);

				if (remove)
				{
					keyconfig.get ().unstage_mapping (mapping);
				}
				else
				{
					keyconfig.get ().stage_mapping (cursorposition, mapping);
				}
			}
		}

		public override void set_count (short c)
		{
			count = c;
		}

		public override Icon.IconType get_type ()
		{
			return Icon.IconType.ITEM;
		}

		private InventoryType.Id sourcetab;
		private EquipSlot.Id eqsource;
		private short source;
		private int item_id;
		private short count;
		private bool untradable;
		private bool cashitem;
		private readonly UIItemInventory parent;
	}
}
