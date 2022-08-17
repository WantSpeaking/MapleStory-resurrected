#define USE_NX

using System.Collections.Generic;
using MapleLib.WzLib;




namespace ms
{
	// The Equip inventory
	public class UIEquipInventory : UIDragElement<PosEQINV>
	{
		public const Type TYPE = UIElement.Type.EQUIPINVENTORY;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public UIEquipInventory (params object[] args) : this ((Inventory)args[0])
		{
		}
		
		public UIEquipInventory (Inventory invent)
		{
			//this.UIDragElement<PosEQINV> = new <type missing>();
			this.inventory = invent;
			this.tab = (int)Buttons.BT_TAB1;
			this.hasPendantSlot = false;
			this.hasPocketSlot = false;
			// Column 1
			iconpositions[EquipSlot.Id.RING1] = new Point_short (14, 50);
			iconpositions[EquipSlot.Id.RING2] = new Point_short (14, 91);
			iconpositions[EquipSlot.Id.RING3] = new Point_short (14, 132);
			iconpositions[EquipSlot.Id.RING4] = new Point_short (14, 173);
			iconpositions[EquipSlot.Id.POCKET] = new Point_short (14, 214);
			iconpositions[EquipSlot.Id.BOOK] = new Point_short (14, 255);

			// Column 2
			//iconpositions[EquipSlot::Id::NONE] = Point<int16_t>(55, 50);
			iconpositions[EquipSlot.Id.PENDANT2] = new Point_short (55, 91);
			iconpositions[EquipSlot.Id.PENDANT1] = new Point_short (55, 132);
			iconpositions[EquipSlot.Id.WEAPON] = new Point_short (55, 173);
			iconpositions[EquipSlot.Id.BELT] = new Point_short (55, 214);
			//iconpositions[EquipSlot::Id::NONE] = Point<int16_t>(55, 255);

			// Column 3
			iconpositions[EquipSlot.Id.HAT] = new Point_short (96, 50);
			iconpositions[EquipSlot.Id.FACE] = new Point_short (96, 91);
			iconpositions[EquipSlot.Id.EYEACC] = new Point_short (96, 132);
			iconpositions[EquipSlot.Id.TOP] = new Point_short (96, 173);
			iconpositions[EquipSlot.Id.BOTTOM] = new Point_short (96, 214);
			iconpositions[EquipSlot.Id.SHOES] = new Point_short (96, 255);

			// Column 4
			//iconpositions[EquipSlot::Id::NONE] = Point<int16_t>(137, 50);
			//iconpositions[EquipSlot::Id::NONE] = Point<int16_t>(137, 91);
			iconpositions[EquipSlot.Id.EARACC] = new Point_short (137, 132);
			iconpositions[EquipSlot.Id.SHOULDER] = new Point_short (137, 173);
			iconpositions[EquipSlot.Id.GLOVES] = new Point_short (137, 214);
			iconpositions[EquipSlot.Id.ANDROID] = new Point_short (137, 255);

			// Column 5
			iconpositions[EquipSlot.Id.EMBLEM] = new Point_short (178, 50);
			iconpositions[EquipSlot.Id.BADGE] = new Point_short (178, 91);
			iconpositions[EquipSlot.Id.MEDAL] = new Point_short (178, 132);
			iconpositions[EquipSlot.Id.SUBWEAPON] = new Point_short (178, 173);
			iconpositions[EquipSlot.Id.CAPE] = new Point_short (178, 214);
			iconpositions[EquipSlot.Id.HEART] = new Point_short (178, 255);

			//iconpositions[EquipSlot::Id::SHIELD] = Point<int16_t>(142, 124);
			//iconpositions[EquipSlot::Id::TAMEDMOB] = Point<int16_t>(142, 91);
			//iconpositions[EquipSlot::Id::SADDLE] = Point<int16_t>(76, 124);

			tab_source[(int)Buttons.BT_TAB0] = "Equip";
			tab_source[(int)Buttons.BT_TAB1] = "Cash";
			tab_source[(int)Buttons.BT_TAB2] = "Pet";
			tab_source[(int)Buttons.BT_TAB3] = "Android";

			WzObject close = ms.wz.wzFile_ui["Basic.img"]["BtClose3"];
			WzObject Equip = ms.wz.wzFile_ui["UIWindow4.img"]["Equip"];

			background[(int)Buttons.BT_TAB0] = Equip[tab_source[(int)Buttons.BT_TAB0]]["backgrnd"];
			background[(int)Buttons.BT_TAB1] = Equip[tab_source[(int)Buttons.BT_TAB1]]["backgrnd"];
			background[(int)Buttons.BT_TAB2] = Equip[tab_source[(int)Buttons.BT_TAB2]]["backgrnd"];
			background[(int)Buttons.BT_TAB3] = Equip[tab_source[(int)Buttons.BT_TAB3]]["backgrnd"];

			for (ushort i = (int)Buttons.BT_TAB0; i < (int)Buttons.BT_TABE; i++)
			{
				foreach (var slot in Equip[tab_source[i]]["Slots"])
				{
					if (slot.Name.IndexOf ("_") == -1)
					{
						var fd = Slots[i];
						var ff = Slots.TryGetNew (i);
						Slots.TryGetNew (i).Add (slot);
					}
				}
			}

			WzObject EquipGL = ms.wz.wzFile_ui["UIWindowGL.img"]["Equip"];
			WzObject backgrnd = Equip["backgrnd"];
			WzObject totem_backgrnd = EquipGL["Totem"]["backgrnd"];

			Point_short bg_dimensions = new Texture (backgrnd).get_dimensions ();
			totem_dimensions = new Texture (totem_backgrnd).get_dimensions ();
			totem_adj = new Point_short ((short)(-totem_dimensions.x () + 4), 0);

			sprites.Add (new Sprite (totem_backgrnd, totem_adj));
			sprites.Add (backgrnd);
			sprites.Add (Equip["backgrnd2"]);

			tabbar = Equip["tabbar"];
			disabled = Equip[tab_source[(int)Buttons.BT_TAB0]]["disabled"];
			disabled2 = Equip[tab_source[(int)Buttons.BT_TAB0]]["disabled2"];

			buttons[(int)Buttons.BT_CLOSE] = new MapleButton (close, new Point_short ((short)(bg_dimensions.x () - 19), 5));
			buttons[(int)Buttons.BT_SLOT] = new MapleButton (Equip[tab_source[(int)Buttons.BT_TAB0]]["BtSlot"]);
			buttons[(int)Buttons.BT_EFFECT] = new MapleButton (EquipGL["Equip"]["btEffect"]);
			buttons[(int)Buttons.BT_SALON] = new MapleButton (EquipGL["Equip"]["btSalon"]);
			buttons[(int)Buttons.BT_CONSUMESETTING] = new MapleButton (Equip[tab_source[(int)Buttons.BT_TAB2]]["BtConsumeSetting"]);
			buttons[(int)Buttons.BT_EXCEPTION] = new MapleButton (Equip[tab_source[(int)Buttons.BT_TAB2]]["BtException"]);
			buttons[(int)Buttons.BT_SHOP] = new MapleButton (Equip[tab_source[(int)Buttons.BT_TAB3]]["BtShop"]);

			buttons[(int)Buttons.BT_CONSUMESETTING].set_state (Button.State.DISABLED);
			buttons[(int)Buttons.BT_EXCEPTION].set_state (Button.State.DISABLED);
			buttons[(int)Buttons.BT_SHOP].set_state (Button.State.DISABLED);

			WzObject Tab = Equip["Tab"];

			for (ushort i = (int)Buttons.BT_TAB0; i < (int)Buttons.BT_TABE; i++)
			{
				buttons[(uint)(Buttons.BT_TAB0 + i)] = new TwoSpriteButton (Tab["disabled"][i.ToString ()], Tab["enabled"][i.ToString ()], new Point_short (0, 3));
			}

			dimension = new Point_short (bg_dimensions);
			dragarea = new Point_short (bg_dimensions.x (), 20);

			load_icons ();
			change_tab ((ushort)Buttons.BT_TAB0);
		}

		public override void draw (float alpha)
		{
			base.draw (alpha);

			background[tab].draw (position);
			tabbar.draw (position);

			foreach (var slot in Slots[tab])
			{
				slot.draw (position);
			}

			if (tab == (int)Buttons.BT_TAB0)
			{
				if (!hasPendantSlot)
				{
					disabled.draw (position + iconpositions[EquipSlot.Id.PENDANT2]);
				}

				if (!hasPocketSlot)
				{
					disabled.draw (position + iconpositions[EquipSlot.Id.POCKET]);
				}

				foreach (var iter in icons)
				{
					if (iter.Value != null)
					{
						iter.Value.draw (position + iconpositions[iter.Key] + new Point_short (4, 4));
					}
				}
			}
			else if (tab == (int)Buttons.BT_TAB2)
			{
				disabled2.draw (position + new Point_short (113, 57));
				disabled2.draw (position + new Point_short (113, 106));
				disabled2.draw (position + new Point_short (113, 155));
			}
		}

		public override void toggle_active ()
		{
			clear_tooltip ();

			base.toggle_active ();
		}

		public override bool send_icon (Icon icon, Point_short cursorpos)
		{
			EquipSlot.Id slot = slot_by_position (new Point_short (cursorpos));
			if (slot != EquipSlot.Id.NONE)
			{
				icon.drop_on_equips (slot);
			}

			return true;
		}

		public override void doubleclick (Point_short cursorpos)
		{
			EquipSlot.Id slot = slot_by_position (new Point_short (cursorpos));

			if (icons.TryGetValue(slot) != null)
			{
				short freeslot = inventory.find_free_slot (InventoryType.Id.EQUIP);
				if (freeslot != 0)
				{
					new UnequipItemPacket ((short)slot, freeslot).dispatch ();
				}
			}
		}

		public override bool is_in_range (Point_short cursorpos)
		{
			Rectangle_short bounds = new Rectangle_short (new Point_short (position), position + dimension);

			Rectangle_short totem_bounds = new Rectangle_short (new Point_short (position), position + totem_dimensions);
			totem_bounds.shift (totem_adj);

			return bounds.contains (cursorpos) || totem_bounds.contains (cursorpos);
		}

		public override Cursor.State send_cursor (bool pressed, Point_short cursorpos)
		{
			Cursor.State dstate = base.send_cursor (pressed, new Point_short (cursorpos));

			if (dragged)
			{
				clear_tooltip ();

				return dstate;
			}

			EquipSlot.Id slot = slot_by_position (new Point_short (cursorpos));

			var icon = icons.TryGetValue(slot);
			if (icon != null)
			{
				if (pressed)
				{
					icon.start_drag (cursorpos - position - iconpositions[slot]);

					UI.get ().drag_icon (icon);

					clear_tooltip ();

					return Cursor.State.GRABBING;
				}
				else
				{
					show_equip (slot);

					return Cursor.State.CANGRAB;
				}
			}
			else
			{
				clear_tooltip ();

				return Cursor.State.IDLE;
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
					ushort newtab = (ushort)(tab + 1);

					if (newtab >= (int)Buttons.BT_TABE)
					{
						newtab = (int)Buttons.BT_TAB0;
					}

					change_tab (newtab);
				}
			}
		}

		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public void modify (short pos, sbyte mode, short arg)
		{
			EquipSlot.Id eqpos = EquipSlot.by_id (pos);
			EquipSlot.Id eqarg = EquipSlot.by_id (arg);

			switch (mode)
			{
				case 0:
				case 3:
					update_slot (eqpos);
					break;
				case 2:
					update_slot (eqpos);
					update_slot (eqarg);
					break;
			}
		}

		public override Button.State button_pressed (ushort id)
		{
			switch ((Buttons)id)
			{
				case Buttons.BT_CLOSE:
					toggle_active ();
					break;
				case Buttons.BT_TAB0:
				case Buttons.BT_TAB1:
				case Buttons.BT_TAB2:
				case Buttons.BT_TAB3:
					change_tab (id);

					return Button.State.IDENTITY;
				default:
					break;
			}

			return Button.State.NORMAL;
		}

		private void show_equip (EquipSlot.Id slot)
		{
			UI.get ().show_equip (Tooltip.Parent.EQUIPINVENTORY, (short)slot);
		}

		private void clear_tooltip ()
		{
			UI.get ().clear_tooltip (Tooltip.Parent.EQUIPINVENTORY);
		}

		private void load_icons ()
		{
			icons.Clear ();

			foreach (var iter in EquipSlot.values)
			{
				update_slot (iter);
			}
		}

		private void update_slot (EquipSlot.Id slot)
		{
			int item_id = inventory.get_item_id (InventoryType.Id.EQUIPPED, (short)slot);
			if (item_id != 0)
			{
				Texture texture = new Texture(ItemData.get (item_id).get_icon (false));

				icons[slot] = new Icon (new EquipIcon ((short)slot), texture, -1);
			}
			else/* if (icons[slot] != null)*/
			{
				icons[slot] = null;
				//icons.TryGetValue(slot)?.Dispose ();
			}

			clear_tooltip ();
		}

		private EquipSlot.Id slot_by_position (Point_short cursorpos)
		{
			if (tab != (int)Buttons.BT_TAB0)
			{
				return EquipSlot.Id.NONE;
			}

			foreach (var iter in iconpositions)
			{
				Rectangle_short iconrect = new Rectangle_short (
					position + iter.Value, 
					position + iter.Value + new Point_short (32, 32)
					);

				if (iconrect.contains (cursorpos))
				{
					return iter.Key;
				}
			}

			return EquipSlot.Id.NONE;
		}

		private void change_tab (ushort tabid)
		{
			byte oldtab = (byte)tab;
			tab = tabid;

			if (oldtab != tab)
			{
				clear_tooltip ();

				buttons[oldtab].set_state (Button.State.NORMAL);
				buttons[tab].set_state (Button.State.PRESSED);

				if (tab == (int)Buttons.BT_TAB0)
				{
					buttons[(int)Buttons.BT_SLOT].set_active (true);
				}
				else
				{
					buttons[(int)Buttons.BT_SLOT].set_active (false);
				}

				if (tab == (int)Buttons.BT_TAB2)
				{
					buttons[(int)Buttons.BT_CONSUMESETTING].set_active (true);
					buttons[(int)Buttons.BT_EXCEPTION].set_active (true);
				}
				else
				{
					buttons[(int)Buttons.BT_CONSUMESETTING].set_active (false);
					buttons[(int)Buttons.BT_EXCEPTION].set_active (false);
				}

				if (tab == (int)Buttons.BT_TAB3)
				{
					buttons[(int)Buttons.BT_SHOP].set_active (true);
				}
				else
				{
					buttons[(int)Buttons.BT_SHOP].set_active (false);
				}
			}
		}

		private class EquipIcon : Icon.Type
		{
			public EquipIcon (short s)
			{
				source = s;
			}

			public override void drop_on_stage ()
			{
				new Sound (Sound.Name.DRAGEND).play ();
			}

			public override void drop_on_equips (EquipSlot.Id slot)
			{
				if (source == (int)slot)
				{
					new Sound (Sound.Name.DRAGEND).play ();
				}
			}

			public override bool drop_on_items (InventoryType.Id tab, EquipSlot.Id eqslot, short slot, bool equip)
			{
				if (tab != InventoryType.Id.EQUIP)
				{
					var iteminventory = UI.get ().get_element<UIItemInventory> ();
					if (iteminventory)
					{
						if (iteminventory.get ().is_active ())
						{
							iteminventory.get ().change_tab (InventoryType.Id.EQUIP);
							return false;
						}
					}
				}

				if (equip)
				{
					if ((int)eqslot == source)
					{
						new EquipItemPacket (slot, eqslot).dispatch ();
					}
				}
				else
				{
					new UnequipItemPacket (source, slot).dispatch ();
				}

				return true;
			}

			public override void drop_on_bindings (Point_short UnnamedParameter1, bool UnnamedParameter2)
			{
			}

			public override void set_count (short UnnamedParameter1)
			{
			}

			public override Icon.IconType get_type ()
			{
				return Icon.IconType.EQUIP;
			}

			private short source;
		}

		private enum Buttons : ushort
		{
			BT_TAB0,
			BT_TAB1,
			BT_TAB2,
			BT_TAB3,
			BT_TABE,
			BT_CLOSE,
			BT_SLOT,
			BT_EFFECT,
			BT_SALON,
			BT_CONSUMESETTING,
			BT_EXCEPTION,
			BT_SHOP
		}

		private readonly Inventory inventory;

		private EnumMapNew<EquipSlot.Id, Point_short> iconpositions = new EnumMapNew<EquipSlot.Id, Point_short> ();
		private EnumMap<EquipSlot.Id, Icon> icons = new EnumMap<EquipSlot.Id, Icon> ();

		private ushort tab;
		private string[] tab_source = new string[(int)Buttons.BT_TABE];
		private Texture tabbar = new Texture ();
		private Texture[] background = new Texture[(int)Buttons.BT_TABE];
		private Texture disabled = new Texture ();
		private Texture disabled2 = new Texture ();
		private List<Texture>[] Slots = new List<Texture>[(int)Buttons.BT_TABE];

		private Point_short totem_dimensions = new Point_short ();
		private Point_short totem_adj = new Point_short ();

		private bool hasPendantSlot;
		private bool hasPocketSlot;
	}
}


#if USE_NX
#endif