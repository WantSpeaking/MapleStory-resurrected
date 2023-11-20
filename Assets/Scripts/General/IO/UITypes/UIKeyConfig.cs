#define USE_NX

using System;
using System.Collections.Generic;
using System.Linq;
using Beebyte.Obfuscator;
using Helper;
using MapleLib.WzLib;
using provider;

namespace ms
{
	[Skip]
	public class UIKeyConfig : UIDragElement<PosKEYCONFIG>
	{
		public const Type TYPE = UIElement.Type.KEYCONFIG;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public UIKeyConfig (params object[] args) : this ((Inventory)args[0], (SkillBook)args[1])
		{
		}

		public UIKeyConfig (Inventory in_inventory, SkillBook in_skillbook)
		{
			//this.UIDragElement<PosKEYCONFIG> = new <type missing>();
			this.inventory = in_inventory;
			this.skillbook = in_skillbook;
			this.dirty = false;
			keyboard = UI.get ().get_keyboard ();
			staged_mappings = new SortedDictionary<int, Keyboard.Mapping> (keyboard.get_maplekeys ());

			MapleData KeyConfig = ms.wz.wzProvider_ui["UIWindow.img"]["KeyConfig"];

			icon = (KeyConfig["icon"]);
			key = (KeyConfig["key"]);

			MapleData backgrnd = KeyConfig["backgrnd"];
			Texture bg = backgrnd;
			Point_short bg_dimensions = bg.get_dimensions ();

			sprites.Add (backgrnd);
			//sprites.Add (KeyConfig["backgrnd2"]);
			//sprites.Add (KeyConfig["backgrnd3"]);

			MapleData BtClose3 = ms.wz.wzProvider_ui["Basic.img"]["BtClose2"];
			buttons[(int)Buttons.CLOSE] = new MapleButton (BtClose3, new Point_short ((short)(bg_dimensions.x () - 18), 3));
			buttons[(int)Buttons.CANCEL] = new MapleButton (KeyConfig["BtCancel"]);
			buttons[(int)Buttons.DEFAULT] = new MapleButton (KeyConfig["BtDefault"]);
			buttons[(int)Buttons.DELETE] = new MapleButton (KeyConfig["BtDelete"]);
			//buttons[(int)Buttons.KEYSETTING] = new MapleButton (KeyConfig["button:keySetting"]);
			buttons[(int)Buttons.OK] = new MapleButton (KeyConfig["BtOK"]);

			dimension = new Point_short (bg_dimensions);
			dragarea = new Point_short (bg_dimensions.x (), 20);

			load_keys_pos ();
			load_unbound_actions_pos ();
			load_key_textures ();
			load_action_mappings ();
			load_action_icons ();
			load_item_icons ();
			load_skill_icons ();

			bind_staged_action_keys ();
		}


		/// UI: General
		public override void draw (float inter)
		{
			base.draw (inter);

			foreach (var iter in staged_mappings)
			{
				int maplekey = iter.Key;
				Keyboard.Mapping mapping = iter.Value;

				Icon ficon = null;

				if (mapping.type == KeyType.Id.ITEM)
				{
					int item_id = mapping.action;
					ficon = item_icons[item_id];
				}
				else if (mapping.type == KeyType.Id.SKILL)
				{
					int skill_id = mapping.action;
					ficon = skill_icons[skill_id];
				}
				else if (is_action_mapping (mapping))
				{
					KeyAction.Id action = KeyAction.actionbyid (mapping.action);

					if ((int)action != 0)
					{
						foreach (var it in action_icons)
						{
							if (it.Key == action)
							{
								ficon = it.Value;
								break;
							}
						}
					}
				}
				else
				{
					Console.Write ("Invalid key mapping: (");
					Console.Write (mapping.type);
					Console.Write (", ");
					Console.Write (mapping.action);
					Console.Write (")");
					Console.Write ("\n");
				}

				if (ficon != null)
				{
					if (maplekey != -1)
					{
						KeyConfig.Key fkey = KeyConfig.actionbyid (maplekey);

						if (maplekey == (int)KeyConfig.Key.SPACE)
						{
							ficon.draw (position + keys_pos[fkey] - new Point_short (0, 3));
						}
						else
						{
							if (fkey == KeyConfig.Key.LEFT_CONTROL || fkey == KeyConfig.Key.RIGHT_CONTROL)
							{
								ficon.draw (position + keys_pos[KeyConfig.Key.LEFT_CONTROL] - new Point_short (2, 3));
								ficon.draw (position + keys_pos[KeyConfig.Key.RIGHT_CONTROL] - new Point_short (2, 3));
							}
							else if (fkey == KeyConfig.Key.LEFT_ALT || fkey == KeyConfig.Key.RIGHT_ALT)
							{
								ficon.draw (position + keys_pos[KeyConfig.Key.LEFT_ALT] - new Point_short (2, 3));
								ficon.draw (position + keys_pos[KeyConfig.Key.RIGHT_ALT] - new Point_short (2, 3));
							}
							else if (fkey == KeyConfig.Key.LEFT_SHIFT || fkey == KeyConfig.Key.RIGHT_SHIFT)
							{
								ficon.draw (position + keys_pos[KeyConfig.Key.LEFT_SHIFT] - new Point_short (2, 3));
								ficon.draw (position + keys_pos[KeyConfig.Key.RIGHT_SHIFT] - new Point_short (2, 3));
							}
							else
							{
								ficon.draw (position + keys_pos[fkey] - new Point_short (2, 3));
							}
						}
					}
				}
			}

			foreach (var ubicon in action_icons)
			{
				if (ubicon.Value != null)
				{
					if (!bound_actions.Contains (ubicon.Key))
					{
						ubicon.Value.draw (position + unbound_actions_pos[ubicon.Key]);
					}
				}
			}

			foreach (var fkey in key_textures)
			{
				KeyConfig.Key key = fkey.Key;
				Texture tx = fkey.Value;

				tx?.draw (position + keys_pos[key]);
			}
		}

		/*public override Cursor.State send_cursor (bool clicked, Point_short cursorpos)
		{
			Cursor.State dstate = base.send_cursor (clicked, new Point_short (cursorpos));

			if (dragged)
			{
				return dstate;
			}

			KeyAction.Id icon_slot = unbound_action_by_position (new Point_short (cursorpos));

			if (icon_slot != (KeyAction.Id.NONE)) //todo 2 any question
			{
				var icon = action_icons[icon_slot];
				if (icon != null)
				{
					if (clicked)
					{
						icon.start_drag (cursorpos - position - unbound_actions_pos[icon_slot]);
						UI.get ().drag_icon (new Icon (icon)); //todo 2 question need new icon?

						return Cursor.State.GRABBING;
					}
					else
					{
						return Cursor.State.CANGRAB;
					}
				}
			}

			clear_tooltip ();

			KeyConfig.Key key_slot = key_by_position (new Point_short (cursorpos));

			if (key_slot != KeyConfig.Key.NONE)
			{
				Keyboard.Mapping mapping = get_staged_mapping ((int)key_slot);

				if (mapping.type != KeyType.Id.NONE)
				{
					Icon ficon = null;

					if (mapping.type == KeyType.Id.ITEM)
					{
						int item_id = mapping.action;
						ficon = item_icons[item_id];

						show_item (item_id);
					}
					else if (mapping.type == KeyType.Id.SKILL)
					{
						int skill_id = mapping.action;
						ficon = skill_icons[skill_id];

						show_skill (skill_id);
					}
					else if (is_action_mapping (mapping))
					{
						KeyAction.Id action = KeyAction.actionbyid (mapping.action);
						ficon = action_icons[action];
					}
					else
					{
						Console.Write ("Invalid icon type for key mapping: (");
						Console.Write (mapping.type);
						Console.Write (", ");
						Console.Write (mapping.action);
						Console.Write (")");
						Console.Write ("\n");
					}

					if (ficon != null)
					{
						if (clicked)
						{
							clear_tooltip ();

							ficon.start_drag (cursorpos - position - keys_pos[key_slot]);
							UI.get ().drag_icon (ficon);

							return Cursor.State.GRABBING;
						}
						else
						{
							return Cursor.State.CANGRAB;
						}
					}
				}
			}

			return base.send_cursor (clicked, new Point_short (cursorpos));
		}*/

		public override bool send_icon (Icon icon, Point_short cursorpos)
		{
			foreach (var iter in unbound_actions_pos)
			{
				Rectangle_short icon_rect = new Rectangle_short (position + iter.Value, position + iter.Value + new Point_short (32, 32));

				if (icon_rect.contains (cursorpos))
				{
					icon.drop_on_bindings (new Point_short (cursorpos), true);
				}
			}

			KeyConfig.Key fkey = key_by_position (new Point_short (cursorpos));

			if (fkey != KeyConfig.Key.NONE) //todo 2 original line is if (fkey != KeyConfig.Key.LENGTH),however Key.LENGTH was deleted
			{
				icon.drop_on_bindings (new Point_short (cursorpos), false);
			}

			return true;
		}

		public override void send_key (int keycode, bool pressed, bool escape)
		{
			if (pressed && escape)
			{
				safe_close ();
			}
		}

		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public void close ()
		{
			clear_tooltip ();
			deactivate ();
			reset ();
		}

		public void SaveAndClose()
        {
			save_staged_mappings();
			close();
		}

		/// Keymap Staging
		public void stage_mapping (Point_short cursorposition, Keyboard.Mapping mapping)
		{
			KeyConfig.Key key = key_by_position (new Point_short (cursorposition));
			stage_mapping (key, mapping);
		}

		public void stage_mapping (KeyConfig.Key key, Keyboard.Mapping mapping)
		{
			Keyboard.Mapping prior_staged = staged_mappings.TryGetValue ((int)key);

			//AppDebug.Log ($"stage_mapping: key:{key} type:{mapping.type} action:{mapping.action}| prior_staged: type:{mapping.type} action:{prior_staged.action}");

			if (prior_staged == mapping)
			{
				return;
			}

			unstage_mapping (prior_staged);

			if (is_action_mapping (mapping))
			{
				KeyAction.Id action = KeyAction.actionbyid (mapping.action);
				if (!bound_actions.Contains (action))
				{
					bound_actions.Add (action);
				}
			}

			foreach (var it in staged_mappings)
			{
				Keyboard.Mapping staged_mapping = it.Value;

				if (staged_mapping == mapping)
				{
					if (it.Key == (int)KeyConfig.Key.LEFT_CONTROL || it.Key == (int)KeyConfig.Key.RIGHT_CONTROL)
					{
						staged_mappings.Remove ((int)KeyConfig.Key.LEFT_CONTROL);
						staged_mappings.Remove ((int)KeyConfig.Key.RIGHT_CONTROL);
					}
					else if (it.Key == (int)KeyConfig.Key.LEFT_ALT || it.Key == (int)KeyConfig.Key.RIGHT_ALT)
					{
						staged_mappings.Remove ((int)KeyConfig.Key.LEFT_ALT);
						staged_mappings.Remove ((int)KeyConfig.Key.RIGHT_ALT);
					}
					else if (it.Key == (int)KeyConfig.Key.LEFT_SHIFT || it.Key == (int)KeyConfig.Key.RIGHT_SHIFT)
					{
						staged_mappings.Remove ((int)KeyConfig.Key.LEFT_SHIFT);
						staged_mappings.Remove ((int)KeyConfig.Key.RIGHT_SHIFT);
					}
					else
					{
						staged_mappings.Remove (it.Key);
					}

					break;
				}
			}

			if (key == KeyConfig.Key.LEFT_CONTROL || key == KeyConfig.Key.RIGHT_CONTROL)
			{
				staged_mappings[(int)KeyConfig.Key.LEFT_CONTROL] = mapping;
				staged_mappings[(int)KeyConfig.Key.RIGHT_CONTROL] = mapping;
			}
			else if (key == KeyConfig.Key.LEFT_ALT || key == KeyConfig.Key.RIGHT_ALT)
			{
				staged_mappings[(int)KeyConfig.Key.LEFT_ALT] = mapping;
				staged_mappings[(int)KeyConfig.Key.RIGHT_ALT] = mapping;
			}
			else if (key == KeyConfig.Key.LEFT_SHIFT || key == KeyConfig.Key.RIGHT_SHIFT)
			{
				staged_mappings[(int)KeyConfig.Key.LEFT_SHIFT] = mapping;
				staged_mappings[(int)KeyConfig.Key.RIGHT_SHIFT] = mapping;
			}
			else
			{
				staged_mappings[(int)key] = mapping;
			}

			if (mapping.type == KeyType.Id.ITEM)
			{
				int item_id = mapping.action;

				if (!item_icons.ContainsKey (item_id))
				{
					short count = inventory.get_total_item_count (item_id);
					Texture tx = get_item_texture (item_id);
					item_icons[item_id] = new Icon (new CountableMappingIcon (mapping, count), tx, count);
				}
			}
			else if (mapping.type == KeyType.Id.SKILL)
			{
				int skill_id = mapping.action;

				if (!skill_icons.ContainsKey (skill_id))
				{
					Texture tx = get_skill_texture (skill_id);
					skill_icons[skill_id] = new Icon (new MappingIcon (mapping), tx, -1);
				}
			}

			dirty = true;
		}
		public void unstage_mapping (Keyboard.Mapping mapping)
		{
			if (is_action_mapping (mapping))
			{
				KeyAction.Id action = KeyAction.actionbyid (mapping.action);
				if (bound_actions.Contains (action))
				{
					bound_actions.Remove (action);
				}
			}

			foreach (var it in staged_mappings)
			{
				Keyboard.Mapping staged_mapping = it.Value;

				if (staged_mapping == mapping)
				{
					if (it.Key == (int)KeyConfig.Key.LEFT_CONTROL || it.Key == (int)KeyConfig.Key.RIGHT_CONTROL)
					{
						staged_mappings.Remove ((int)KeyConfig.Key.LEFT_CONTROL);
						staged_mappings.Remove ((int)KeyConfig.Key.RIGHT_CONTROL);
					}
					else if (it.Key == (int)KeyConfig.Key.LEFT_ALT || it.Key == (int)KeyConfig.Key.RIGHT_ALT)
					{
						staged_mappings.Remove ((int)KeyConfig.Key.LEFT_ALT);
						staged_mappings.Remove ((int)KeyConfig.Key.RIGHT_ALT);
					}
					else if (it.Key == (int)KeyConfig.Key.LEFT_SHIFT || it.Key == (int)KeyConfig.Key.RIGHT_SHIFT)
					{
						staged_mappings.Remove ((int)KeyConfig.Key.LEFT_SHIFT);
						staged_mappings.Remove ((int)KeyConfig.Key.RIGHT_SHIFT);
					}
					else
					{
						staged_mappings.Remove (it.Key);
					}

					if (staged_mapping.type == KeyType.Id.ITEM)
					{
						int item_id = staged_mapping.action;
						item_icons.Remove (item_id);
					}
					else if (staged_mapping.type == KeyType.Id.SKILL)
					{
						int skill_id = staged_mapping.action;
						skill_icons.Remove (skill_id);
					}

					dirty = true;

					break;
				}
			}
		}


		/// Item count
		public void update_item_count (InventoryType.Id type, short slot, short change)
		{
			int item_id = inventory.get_item_id (type, slot);

			if (!item_icons.ContainsKey (item_id))
			{
				return;
			}

			short item_count = item_icons[item_id].get_count ();
			item_icons[item_id].set_count ((short)(item_count + change));
		}

		public override Button.State button_pressed (ushort buttonid)
		{
			switch ((Buttons)buttonid)
			{
				case Buttons.CLOSE:
				case Buttons.CANCEL:
					close ();
					break;
				case Buttons.DEFAULT:
				{
					const string message = "Would you like to revert to default settings?";

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Lambda expressions cannot be assigned to 'var':
					Action<bool> onok = (bool ok) =>
					{
						if (ok)
						{
							Action<bool> keysel_onok = (bool alternate) =>
							{
								clear ();

								if (alternate)
								{
									staged_mappings = new SortedDictionary<int, Keyboard.Mapping> (alternate_keys);
								}
								else
								{
									staged_mappings = new SortedDictionary<int, Keyboard.Mapping> (basic_keys);
								}

								bind_staged_action_keys ();
							};

							UI.get ().emplace<UIKeySelect> (keysel_onok, false);
						}
					};

						ms_Unity.FGUI_OK.ShowNotice (message, onok);
					//UI.get ().emplace<UIOk> (message, onok);
						break;
				}
				case Buttons.DELETE:
				{
					const string message = "Would you like to clear all key bindings?";

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Lambda expressions cannot be assigned to 'var':
					Action<bool> onok = (bool ok) =>
					{
						if (ok)
						{
							clear ();
						}
					};
					ms_Unity.FGUI_OK.ShowNotice (message, onok);

					//UI.get ().emplace<UIOk> (message, onok);
					break;
				}
				case Buttons.KEYSETTING:
					break;
				case Buttons.OK:
				{
					save_staged_mappings ();
					close ();
					break;
				}
				default:
					break;
			}

			return Button.State.NORMAL;
		}


		/// Load
		private void load_keys_pos ()
		{
			short slot_width = 33;
			short slot_width_lg = 98;
			short slot_height = 33;

			short row_y = 126;
			short row_special_y = (short)(row_y - slot_height - 5);

			short row_quickslot_x = 535;

			short row_one_x = 31;
			short row_two_x = 80;
			short row_three_x = 96;
			short row_four_x = 55;
			short row_five_x = 39;

			short row_special_x = row_one_x;

			keys_pos[KeyConfig.Key.ESCAPE] = new Point_short (row_one_x, row_special_y);

			row_special_x += (short)(slot_width * 2);

			for (int i = (int)KeyConfig.Key.F1; i <= (int)KeyConfig.Key.F12; i++)
			{
				KeyConfig.Key id = KeyConfig.actionbyid (i);

				keys_pos[id] = new Point_short (row_special_x, row_special_y);

				row_special_x += slot_width;

				if (id == KeyConfig.Key.F4 || id == KeyConfig.Key.F8)
				{
					row_special_x += 17;
				}
			}

			keys_pos[KeyConfig.Key.SCROLL_LOCK] = new Point_short ((short)(row_quickslot_x + (slot_width * 1)), row_special_y);

			keys_pos[KeyConfig.Key.GRAVE_ACCENT] = new Point_short ((short)(row_one_x + (slot_width * 0)), (short)(row_y + (slot_height * 0)));
			keys_pos[KeyConfig.Key.NUM1] = new Point_short ((short)(row_one_x + (slot_width * 1)), (short)(row_y + (slot_height * 0)));
			keys_pos[KeyConfig.Key.NUM2] = new Point_short ((short)(row_one_x + (slot_width * 2)), (short)(row_y + (slot_height * 0)));
			keys_pos[KeyConfig.Key.NUM3] = new Point_short ((short)(row_one_x + (slot_width * 3)), (short)(row_y + (slot_height * 0)));
			keys_pos[KeyConfig.Key.NUM4] = new Point_short ((short)(row_one_x + (slot_width * 4)), (short)(row_y + (slot_height * 0)));
			keys_pos[KeyConfig.Key.NUM5] = new Point_short ((short)(row_one_x + (slot_width * 5)), (short)(row_y + (slot_height * 0)));
			keys_pos[KeyConfig.Key.NUM6] = new Point_short ((short)(row_one_x + (slot_width * 6)), (short)(row_y + (slot_height * 0)));
			keys_pos[KeyConfig.Key.NUM7] = new Point_short ((short)(row_one_x + (slot_width * 7)), (short)(row_y + (slot_height * 0)));
			keys_pos[KeyConfig.Key.NUM8] = new Point_short ((short)(row_one_x + (slot_width * 8)), (short)(row_y + (slot_height * 0)));
			keys_pos[KeyConfig.Key.NUM9] = new Point_short ((short)(row_one_x + (slot_width * 9)), (short)(row_y + (slot_height * 0)));
			keys_pos[KeyConfig.Key.NUM0] = new Point_short ((short)(row_one_x + (slot_width * 10)), (short)(row_y + (slot_height * 0)));
			keys_pos[KeyConfig.Key.MINUS] = new Point_short ((short)(row_one_x + (slot_width * 11)), (short)(row_y + (slot_height * 0)));
			keys_pos[KeyConfig.Key.EQUAL] = new Point_short ((short)(row_one_x + (slot_width * 12)), (short)(row_y + (slot_height * 0)));

			for (int i = (int)KeyConfig.Key.Q; i <= (int)KeyConfig.Key.RIGHT_BRACKET; i++)
			{
				KeyConfig.Key id = KeyConfig.actionbyid (i);

				keys_pos[id] = new Point_short ((short)(row_two_x + (slot_width * (int)(i - (int)KeyConfig.Key.Q))), (short)(row_y + (slot_height * 1)));
			}

			row_two_x += 9;

			keys_pos[KeyConfig.Key.BACKSLASH] = new Point_short ((short)(row_two_x + (slot_width * 12)), (short)(row_y + (slot_height * 1)));

			for (int i = (int)KeyConfig.Key.A; i <= (int)KeyConfig.Key.APOSTROPHE; i++)
			{
				KeyConfig.Key id = KeyConfig.actionbyid (i);

				keys_pos[id] = new Point_short ((short)(row_three_x + (slot_width * (int)(i - (int)KeyConfig.Key.A))), (short)(row_y + (slot_height * 2)));
			}

			keys_pos[KeyConfig.Key.LEFT_SHIFT] = new Point_short ((short)(row_four_x + (slot_width * 0)), (short)(row_y + (slot_height * 3)));

			row_four_x += 24;

			for (int i = (int)KeyConfig.Key.Z; i <= (int)KeyConfig.Key.PERIOD; i++)
			{
				KeyConfig.Key id = KeyConfig.actionbyid (i);

				keys_pos[id] = new Point_short ((short)(row_four_x + (slot_width * (int)(i - (int)KeyConfig.Key.Z + 1))), (short)(row_y + (slot_height * 3)));
			}

			row_four_x += 24;

			keys_pos[KeyConfig.Key.RIGHT_SHIFT] = new Point_short ((short)(row_four_x + (slot_width * 11)), (short)(row_y + (slot_height * 3)));

			keys_pos[KeyConfig.Key.LEFT_CONTROL] = new Point_short ((short)(row_five_x + (slot_width_lg * 0)), (short)(row_y + (slot_height * 4)));
			keys_pos[KeyConfig.Key.LEFT_ALT] = new Point_short ((short)(row_five_x + (slot_width_lg * 1)), (short)(row_y + (slot_height * 4)));

			row_five_x += 24;

			keys_pos[KeyConfig.Key.SPACE] = new Point_short ((short)(row_five_x + (slot_width_lg * 2)), (short)(row_y + (slot_height * 4)));

			row_five_x += 27;

			keys_pos[KeyConfig.Key.RIGHT_ALT] = new Point_short ((short)(row_five_x + (slot_width_lg * 3)), (short)(row_y + (slot_height * 4)));

			row_five_x += 2;

			keys_pos[KeyConfig.Key.RIGHT_CONTROL] = new Point_short ((short)(row_five_x + (slot_width_lg * 4)), (short)(row_y + (slot_height * 4)));

			keys_pos[KeyConfig.Key.INSERT] = new Point_short ((short)(row_quickslot_x + (slot_width * 0)), (short)(row_y + (slot_height * 0)));
			keys_pos[KeyConfig.Key.HOME] = new Point_short ((short)(row_quickslot_x + (slot_width * 1)), (short)(row_y + (slot_height * 0)));
			keys_pos[KeyConfig.Key.PAGE_UP] = new Point_short ((short)(row_quickslot_x + (slot_width * 2)), (short)(row_y + (slot_height * 0)));
			keys_pos[KeyConfig.Key.DELETE] = new Point_short ((short)(row_quickslot_x + (slot_width * 0)), (short)(row_y + (slot_height * 1)));
			keys_pos[KeyConfig.Key.END] = new Point_short ((short)(row_quickslot_x + (slot_width * 1)), (short)(row_y + (slot_height * 1)));
			keys_pos[KeyConfig.Key.PAGE_DOWN] = new Point_short ((short)(row_quickslot_x + (slot_width * 2)), (short)(row_y + (slot_height * 1)));
		}

		private void load_unbound_actions_pos ()
		{
			short row_x = 26;
			short row_y = 307;

			short slot_width = 36;
			short slot_height = 36;

			unbound_actions_pos[KeyAction.Id.MAPLECHAT] = new Point_short ((short)(row_x + (slot_width * 0)), (short)(row_y + (slot_height * 0)));
			unbound_actions_pos[KeyAction.Id.TOGGLECHAT] = new Point_short ((short)(row_x + (slot_width * 1)), (short)(row_y + (slot_height * 0)));
			unbound_actions_pos[KeyAction.Id.WHISPER] = new Point_short ((short)(row_x + (slot_width * 2)), (short)(row_y + (slot_height * 0)));
			unbound_actions_pos[KeyAction.Id.MEDALS] = new Point_short ((short)(row_x + (slot_width * 3)), (short)(row_y + (slot_height * 0)));
			unbound_actions_pos[KeyAction.Id.BOSSPARTY] = new Point_short ((short)(row_x + (slot_width * 4)), (short)(row_y + (slot_height * 0)));
			unbound_actions_pos[KeyAction.Id.PROFESSION] = new Point_short ((short)(row_x + (slot_width * 5)), (short)(row_y + (slot_height * 0)));
			unbound_actions_pos[KeyAction.Id.EQUIPMENT] = new Point_short ((short)(row_x + (slot_width * 6)), (short)(row_y + (slot_height * 0)));
			unbound_actions_pos[KeyAction.Id.ITEMS] = new Point_short ((short)(row_x + (slot_width * 7)), (short)(row_y + (slot_height * 0)));
			unbound_actions_pos[KeyAction.Id.CHARINFO] = new Point_short ((short)(row_x + (slot_width * 8)), (short)(row_y + (slot_height * 0)));
			unbound_actions_pos[KeyAction.Id.MENU] = new Point_short ((short)(row_x + (slot_width * 9)), (short)(row_y + (slot_height * 0)));
			unbound_actions_pos[KeyAction.Id.QUICKSLOTS] = new Point_short ((short)(row_x + (slot_width * 10)), (short)(row_y + (slot_height * 0)));
			unbound_actions_pos[KeyAction.Id.PICKUP] = new Point_short ((short)(row_x + (slot_width * 11)), (short)(row_y + (slot_height * 0)));
			unbound_actions_pos[KeyAction.Id.SIT] = new Point_short ((short)(row_x + (slot_width * 12)), (short)(row_y + (slot_height * 0)));
			unbound_actions_pos[KeyAction.Id.ATTACK] = new Point_short ((short)(row_x + (slot_width * 13)), (short)(row_y + (slot_height * 0)));
			unbound_actions_pos[KeyAction.Id.JUMP] = new Point_short ((short)(row_x + (slot_width * 14)), (short)(row_y + (slot_height * 0)));
			unbound_actions_pos[KeyAction.Id.INTERACT_HARVEST] = new Point_short ((short)(row_x + (slot_width * 15)), (short)(row_y + (slot_height * 0)));
			unbound_actions_pos[KeyAction.Id.SOULWEAPON] = new Point_short ((short)(row_x + (slot_width * 16)), (short)(row_y + (slot_height * 0)));

			unbound_actions_pos[KeyAction.Id.SAY] = new Point_short ((short)(row_x + (slot_width * 0)), (short)(row_y + (slot_height * 1)));
			unbound_actions_pos[KeyAction.Id.PARTYCHAT] = new Point_short ((short)(row_x + (slot_width * 1)), (short)(row_y + (slot_height * 1)));
			unbound_actions_pos[KeyAction.Id.FRIENDSCHAT] = new Point_short ((short)(row_x + (slot_width * 2)), (short)(row_y + (slot_height * 1)));
			unbound_actions_pos[KeyAction.Id.ITEMPOT] = new Point_short ((short)(row_x + (slot_width * 3)), (short)(row_y + (slot_height * 1)));
			unbound_actions_pos[KeyAction.Id.EVENT] = new Point_short ((short)(row_x + (slot_width * 4)), (short)(row_y + (slot_height * 1)));
			unbound_actions_pos[KeyAction.Id.SILENTCRUSADE] = new Point_short ((short)(row_x + (slot_width * 5)), (short)(row_y + (slot_height * 1)));
			unbound_actions_pos[KeyAction.Id.STATS] = new Point_short ((short)(row_x + (slot_width * 6)), (short)(row_y + (slot_height * 1)));
			unbound_actions_pos[KeyAction.Id.SKILLS] = new Point_short ((short)(row_x + (slot_width * 7)), (short)(row_y + (slot_height * 1)));
			unbound_actions_pos[KeyAction.Id.QUESTLOG] = new Point_short ((short)(row_x + (slot_width * 8)), (short)(row_y + (slot_height * 1)));
			unbound_actions_pos[KeyAction.Id.CHANGECHANNEL] = new Point_short ((short)(row_x + (slot_width * 9)), (short)(row_y + (slot_height * 1)));
			unbound_actions_pos[KeyAction.Id.GUILD] = new Point_short ((short)(row_x + (slot_width * 10)), (short)(row_y + (slot_height * 1)));
			unbound_actions_pos[KeyAction.Id.PARTY] = new Point_short ((short)(row_x + (slot_width * 11)), (short)(row_y + (slot_height * 1)));
			unbound_actions_pos[KeyAction.Id.NOTIFIER] = new Point_short ((short)(row_x + (slot_width * 12)), (short)(row_y + (slot_height * 1)));
			unbound_actions_pos[KeyAction.Id.FRIENDS] = new Point_short ((short)(row_x + (slot_width * 13)), (short)(row_y + (slot_height * 1)));
			unbound_actions_pos[KeyAction.Id.WORLDMAP] = new Point_short ((short)(row_x + (slot_width * 14)), (short)(row_y + (slot_height * 1)));
			unbound_actions_pos[KeyAction.Id.MINIMAP] = new Point_short ((short)(row_x + (slot_width * 15)), (short)(row_y + (slot_height * 1)));
			unbound_actions_pos[KeyAction.Id.KEYBINDINGS] = new Point_short ((short)(row_x + (slot_width * 16)), (short)(row_y + (slot_height * 1)));

			unbound_actions_pos[KeyAction.Id.GUILDCHAT] = new Point_short ((short)(row_x + (slot_width * 0)), (short)(row_y + (slot_height * 2)));
			unbound_actions_pos[KeyAction.Id.ALLIANCECHAT] = new Point_short ((short)(row_x + (slot_width * 1)), (short)(row_y + (slot_height * 2)));
			unbound_actions_pos[KeyAction.Id.BATTLEANALYSIS] = new Point_short ((short)(row_x + (slot_width * 2)), (short)(row_y + (slot_height * 2)));
			unbound_actions_pos[KeyAction.Id.GUIDE] = new Point_short ((short)(row_x + (slot_width * 3)), (short)(row_y + (slot_height * 2)));
			unbound_actions_pos[KeyAction.Id.ENHANCEEQUIP] = new Point_short ((short)(row_x + (slot_width * 4)), (short)(row_y + (slot_height * 2)));
			unbound_actions_pos[KeyAction.Id.MONSTERCOLLECTION] = new Point_short ((short)(row_x + (slot_width * 5)), (short)(row_y + (slot_height * 2)));
			unbound_actions_pos[KeyAction.Id.MANAGELEGION] = new Point_short ((short)(row_x + (slot_width * 6)), (short)(row_y + (slot_height * 2)));
			//unbound_actions_pos[KeyAction::Id::LENGTH] = Point<int16_t>((short)(row_x + (slot_width * 7), (short)(row_y + (slot_height * 2)));
			unbound_actions_pos[KeyAction.Id.MAPLENEWS] = new Point_short ((short)(row_x + (slot_width * 8)), (short)(row_y + (slot_height * 2)));
			unbound_actions_pos[KeyAction.Id.CASHSHOP] = new Point_short ((short)(row_x + (slot_width * 9)), (short)(row_y + (slot_height * 2)));
			unbound_actions_pos[KeyAction.Id.MAINMENU] = new Point_short ((short)(row_x + (slot_width * 10)), (short)(row_y + (slot_height * 2)));
			unbound_actions_pos[KeyAction.Id.SCREENSHOT] = new Point_short ((short)(row_x + (slot_width * 11)), (short)(row_y + (slot_height * 2)));
			unbound_actions_pos[KeyAction.Id.PICTUREMODE] = new Point_short ((short)(row_x + (slot_width * 12)), (short)(row_y + (slot_height * 2)));
			//unbound_actions_pos[KeyAction::Id::LENGTH] = Point<int16_t>((short)(row_x + (slot_width * 13), (short)(row_y + (slot_height * 2)));
			unbound_actions_pos[KeyAction.Id.MUTE] = new Point_short ((short)(row_x + (slot_width * 14)), (short)(row_y + (slot_height * 2)));
			//unbound_actions_pos[KeyAction::Id::LENGTH] = Point<int16_t>((short)(row_x + (slot_width * 15), (short)(row_y + (slot_height * 2)));
			//unbound_actions_pos[KeyAction::Id::LENGTH] = Point<int16_t>((short)(row_x + (slot_width * 16), (short)(row_y + (slot_height * 2)));

			unbound_actions_pos[KeyAction.Id.FACE1] = new Point_short ((short)(row_x + (slot_width * 0)), (short)(row_y + (slot_height * 3)));
			unbound_actions_pos[KeyAction.Id.FACE2] = new Point_short ((short)(row_x + (slot_width * 1)), (short)(row_y + (slot_height * 3)));
			unbound_actions_pos[KeyAction.Id.FACE3] = new Point_short ((short)(row_x + (slot_width * 2)), (short)(row_y + (slot_height * 3)));
			unbound_actions_pos[KeyAction.Id.FACE4] = new Point_short ((short)(row_x + (slot_width * 3)), (short)(row_y + (slot_height * 3)));
			unbound_actions_pos[KeyAction.Id.FACE5] = new Point_short ((short)(row_x + (slot_width * 4)), (short)(row_y + (slot_height * 3)));
			unbound_actions_pos[KeyAction.Id.FACE6] = new Point_short ((short)(row_x + (slot_width * 5)), (short)(row_y + (slot_height * 3)));
			unbound_actions_pos[KeyAction.Id.FACE7] = new Point_short ((short)(row_x + (slot_width * 6)), (short)(row_y + (slot_height * 3)));
			unbound_actions_pos[KeyAction.Id.MAPLEACHIEVEMENT] = new Point_short ((short)(row_x + (slot_width * 7)), (short)(row_y + (slot_height * 3)));
			unbound_actions_pos[KeyAction.Id.MONSTERBOOK] = new Point_short ((short)(row_x + (slot_width * 8)), (short)(row_y + (slot_height * 3)));
			unbound_actions_pos[KeyAction.Id.TOSPOUSE] = new Point_short ((short)(row_x + (slot_width * 9)), (short)(row_y + (slot_height * 3)));
			//unbound_actions_pos[KeyAction::Id::LENGTH] = Point<int16_t>(row_x + (slot_width * 10), row_y + (slot_height * 3));
			//unbound_actions_pos[KeyAction::Id::LENGTH] = Point<int16_t>(row_x + (slot_width * 11), row_y + (slot_height * 3));
			//unbound_actions_pos[KeyAction::Id::LENGTH] = Point<int16_t>(row_x + (slot_width * 12), row_y + (slot_height * 3));
			//unbound_actions_pos[KeyAction::Id::LENGTH] = Point<int16_t>(row_x + (slot_width * 13), row_y + (slot_height * 3));
			//unbound_actions_pos[KeyAction::Id::LENGTH] = Point<int16_t>(row_x + (slot_width * 14), row_y + (slot_height * 3));
			//unbound_actions_pos[KeyAction::Id::LENGTH] = Point<int16_t>(row_x + (slot_width * 15), row_y + (slot_height * 3));
			//unbound_actions_pos[KeyAction::Id::LENGTH] = Point<int16_t>(row_x + (slot_width * 16), row_y + (slot_height * 3));
		}

		private void load_key_textures ()
		{
			key_textures[KeyConfig.Key.ESCAPE] = key[1.ToString ()];
			key_textures[KeyConfig.Key.NUM1] = key[2.ToString ()];
			key_textures[KeyConfig.Key.NUM2] = key[3.ToString ()];
			key_textures[KeyConfig.Key.NUM3] = key[4.ToString ()];
			key_textures[KeyConfig.Key.NUM4] = key[5.ToString ()];
			key_textures[KeyConfig.Key.NUM5] = key[6.ToString ()];
			key_textures[KeyConfig.Key.NUM6] = key[7.ToString ()];
			key_textures[KeyConfig.Key.NUM7] = key[8.ToString ()];
			key_textures[KeyConfig.Key.NUM8] = key[9.ToString ()];
			key_textures[KeyConfig.Key.NUM9] = key[10.ToString ()];
			key_textures[KeyConfig.Key.NUM0] = key[11.ToString ()];
			key_textures[KeyConfig.Key.MINUS] = key[12.ToString ()];
			key_textures[KeyConfig.Key.EQUAL] = key[13.ToString ()];

			key_textures[KeyConfig.Key.Q] = key[16.ToString ()];
			key_textures[KeyConfig.Key.W] = key[17.ToString ()];
			key_textures[KeyConfig.Key.E] = key[18.ToString ()];
			key_textures[KeyConfig.Key.R] = key[19.ToString ()];
			key_textures[KeyConfig.Key.T] = key[20.ToString ()];
			key_textures[KeyConfig.Key.Y] = key[21.ToString ()];
			key_textures[KeyConfig.Key.U] = key[22.ToString ()];
			key_textures[KeyConfig.Key.I] = key[23.ToString ()];
			key_textures[KeyConfig.Key.O] = key[24.ToString ()];
			key_textures[KeyConfig.Key.P] = key[25.ToString ()];
			key_textures[KeyConfig.Key.LEFT_BRACKET] = key[26.ToString ()];
			key_textures[KeyConfig.Key.RIGHT_BRACKET] = key[27.ToString ()];

			key_textures[KeyConfig.Key.LEFT_CONTROL] = key[29.ToString ()];
			key_textures[KeyConfig.Key.RIGHT_CONTROL] = key[29.ToString ()];

			key_textures[KeyConfig.Key.A] = key[30.ToString ()];
			key_textures[KeyConfig.Key.S] = key[31.ToString ()];
			key_textures[KeyConfig.Key.D] = key[32.ToString ()];
			key_textures[KeyConfig.Key.F] = key[33.ToString ()];
			key_textures[KeyConfig.Key.G] = key[34.ToString ()];
			key_textures[KeyConfig.Key.H] = key[35.ToString ()];
			key_textures[KeyConfig.Key.J] = key[36.ToString ()];
			key_textures[KeyConfig.Key.K] = key[37.ToString ()];
			key_textures[KeyConfig.Key.L] = key[38.ToString ()];
			key_textures[KeyConfig.Key.SEMICOLON] = key[39.ToString ()];
			key_textures[KeyConfig.Key.APOSTROPHE] = key[40.ToString ()];
			key_textures[KeyConfig.Key.GRAVE_ACCENT] = key[41.ToString ()];

			key_textures[KeyConfig.Key.LEFT_SHIFT] = key[42.ToString ()];
			key_textures[KeyConfig.Key.RIGHT_SHIFT] = key[42.ToString ()];

			key_textures[KeyConfig.Key.BACKSLASH] = key[43.ToString ()];
			key_textures[KeyConfig.Key.Z] = key[44.ToString ()];
			key_textures[KeyConfig.Key.X] = key[45.ToString ()];
			key_textures[KeyConfig.Key.C] = key[46.ToString ()];
			key_textures[KeyConfig.Key.V] = key[47.ToString ()];
			key_textures[KeyConfig.Key.B] = key[48.ToString ()];
			key_textures[KeyConfig.Key.N] = key[49.ToString ()];
			key_textures[KeyConfig.Key.M] = key[50.ToString ()];
			key_textures[KeyConfig.Key.COMMA] = key[51.ToString ()];
			key_textures[KeyConfig.Key.PERIOD] = key[52.ToString ()];

			key_textures[KeyConfig.Key.LEFT_ALT] = key[56.ToString ()];
			key_textures[KeyConfig.Key.RIGHT_ALT] = key[56.ToString ()];

			key_textures[KeyConfig.Key.SPACE] = key[57.ToString ()];

			key_textures[KeyConfig.Key.F1] = key[59.ToString ()];
			key_textures[KeyConfig.Key.F2] = key[60.ToString ()];
			key_textures[KeyConfig.Key.F3] = key[61.ToString ()];
			key_textures[KeyConfig.Key.F4] = key[62.ToString ()];
			key_textures[KeyConfig.Key.F5] = key[63.ToString ()];
			key_textures[KeyConfig.Key.F6] = key[64.ToString ()];
			key_textures[KeyConfig.Key.F7] = key[65.ToString ()];
			key_textures[KeyConfig.Key.F8] = key[66.ToString ()];
			key_textures[KeyConfig.Key.F9] = key[67.ToString ()];
			key_textures[KeyConfig.Key.F10] = key[68.ToString ()];

			key_textures[KeyConfig.Key.SCROLL_LOCK] = key[70.ToString ()];
			key_textures[KeyConfig.Key.HOME] = key[71.ToString ()];

			key_textures[KeyConfig.Key.PAGE_UP] = key[73.ToString ()];

			key_textures[KeyConfig.Key.END] = key[79.ToString ()];

			key_textures[KeyConfig.Key.PAGE_DOWN] = key[81.ToString ()];
			key_textures[KeyConfig.Key.INSERT] = key[82.ToString ()];
			key_textures[KeyConfig.Key.DELETE] = key[83.ToString ()];

			key_textures[KeyConfig.Key.F11] = key[87.ToString ()];
			key_textures[KeyConfig.Key.F12] = key[88.ToString ()];
		}

		private void load_action_mappings ()
		{
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.EQUIPMENT), (int)KeyAction.Id.EQUIPMENT));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.ITEMS), (int)KeyAction.Id.ITEMS));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.STATS), (int)KeyAction.Id.STATS));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.SKILLS), (int)KeyAction.Id.SKILLS));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.FRIENDS), (int)KeyAction.Id.FRIENDS));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.WORLDMAP), (int)KeyAction.Id.WORLDMAP));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.MAPLECHAT), (int)KeyAction.Id.MAPLECHAT));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.MINIMAP), (int)KeyAction.Id.MINIMAP));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.QUESTLOG), (int)KeyAction.Id.QUESTLOG));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.KEYBINDINGS), (int)KeyAction.Id.KEYBINDINGS));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.SAY), (int)KeyAction.Id.SAY));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.WHISPER), (int)KeyAction.Id.WHISPER));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.PARTYCHAT), (int)KeyAction.Id.PARTYCHAT));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.FRIENDSCHAT), (int)KeyAction.Id.FRIENDSCHAT));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.MENU), (int)KeyAction.Id.MENU));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.QUICKSLOTS), (int)KeyAction.Id.QUICKSLOTS));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.TOGGLECHAT), (int)KeyAction.Id.TOGGLECHAT));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.GUILD), (int)KeyAction.Id.GUILD));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.GUILDCHAT), (int)KeyAction.Id.GUILDCHAT));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.PARTY), (int)KeyAction.Id.PARTY));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.NOTIFIER), (int)KeyAction.Id.NOTIFIER));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.MAPLENEWS), (int)KeyAction.Id.MAPLENEWS));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.CASHSHOP), (int)KeyAction.Id.CASHSHOP));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.ALLIANCECHAT), (int)KeyAction.Id.ALLIANCECHAT));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.MANAGELEGION), (int)KeyAction.Id.MANAGELEGION));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.MEDALS), (int)KeyAction.Id.MEDALS));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.BOSSPARTY), (int)KeyAction.Id.BOSSPARTY));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.PROFESSION), (int)KeyAction.Id.PROFESSION));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.ITEMPOT), (int)KeyAction.Id.ITEMPOT));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.EVENT), (int)KeyAction.Id.EVENT));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.SILENTCRUSADE), (int)KeyAction.Id.SILENTCRUSADE));
			//action_mappings.Add(Keyboard::Mapping(get_keytype(KeyAction::Id::BITS), (int)KeyAction::Id::BITS));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.BATTLEANALYSIS), (int)KeyAction.Id.BATTLEANALYSIS));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.GUIDE), (int)KeyAction.Id.GUIDE));
			//action_mappings.Add(Keyboard::Mapping(get_keytype(KeyAction::Id::VIEWERSCHAT), (int)KeyAction::Id::VIEWERSCHAT));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.ENHANCEEQUIP), (int)KeyAction.Id.ENHANCEEQUIP));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.MONSTERCOLLECTION), (int)KeyAction.Id.MONSTERCOLLECTION));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.SOULWEAPON), (int)KeyAction.Id.SOULWEAPON));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.CHARINFO), (int)KeyAction.Id.CHARINFO));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.CHANGECHANNEL), (int)KeyAction.Id.CHANGECHANNEL));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.MAINMENU), (int)KeyAction.Id.MAINMENU));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.SCREENSHOT), (int)KeyAction.Id.SCREENSHOT));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.PICTUREMODE), (int)KeyAction.Id.PICTUREMODE));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.MAPLEACHIEVEMENT), (int)KeyAction.Id.MAPLEACHIEVEMENT));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.PICKUP), (int)KeyAction.Id.PICKUP));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.SIT), (int)KeyAction.Id.SIT));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.ATTACK), (int)KeyAction.Id.ATTACK));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.JUMP), (int)KeyAction.Id.JUMP));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.INTERACT_HARVEST), (int)KeyAction.Id.INTERACT_HARVEST));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.FACE1), (int)KeyAction.Id.FACE1));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.FACE2), (int)KeyAction.Id.FACE2));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.FACE3), (int)KeyAction.Id.FACE3));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.FACE4), (int)KeyAction.Id.FACE4));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.FACE5), (int)KeyAction.Id.FACE5));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.FACE6), (int)KeyAction.Id.FACE6));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.FACE7), (int)KeyAction.Id.FACE7));
			//action_mappings.Add(Keyboard::Mapping(get_keytype(KeyAction::Id::MAPLESTORAGE), (int)KeyAction::Id::MAPLESTORAGE));
			//action_mappings.Add(Keyboard::Mapping(get_keytype(KeyAction::Id::SAFEMODE), (int)KeyAction::Id::SAFEMODE));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.MUTE), (int)KeyAction.Id.MUTE));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.MONSTERBOOK), (int)KeyAction.Id.MONSTERBOOK));
			action_mappings.Add (new Keyboard.Mapping (get_keytype (KeyAction.Id.TOSPOUSE), (int)KeyAction.Id.TOSPOUSE));
		}

		private void load_action_icons ()
		{
			action_icons[KeyAction.Id.EQUIPMENT] = new Icon (new MappingIcon (KeyAction.Id.EQUIPMENT), icon[0.ToString ()], -1);
			action_icons[KeyAction.Id.ITEMS] = new Icon (new MappingIcon (KeyAction.Id.ITEMS), icon[1.ToString ()], -1);
			action_icons[KeyAction.Id.STATS] = new Icon (new MappingIcon (KeyAction.Id.STATS), icon[2.ToString ()], -1);
			action_icons[KeyAction.Id.SKILLS] = new Icon (new MappingIcon (KeyAction.Id.SKILLS), icon[3.ToString ()], -1);
			action_icons[KeyAction.Id.FRIENDS] = new Icon (new MappingIcon (KeyAction.Id.FRIENDS), icon[4.ToString ()], -1);
			action_icons[KeyAction.Id.WORLDMAP] = new Icon (new MappingIcon (KeyAction.Id.WORLDMAP), icon[5.ToString ()], -1);
			action_icons[KeyAction.Id.MAPLECHAT] = new Icon (new MappingIcon (KeyAction.Id.MAPLECHAT), icon[6.ToString ()], -1);
			action_icons[KeyAction.Id.MINIMAP] = new Icon (new MappingIcon (KeyAction.Id.MINIMAP), icon[7.ToString ()], -1);
			action_icons[KeyAction.Id.QUESTLOG] = new Icon (new MappingIcon (KeyAction.Id.QUESTLOG), icon[8.ToString ()], -1);
			action_icons[KeyAction.Id.KEYBINDINGS] = new Icon (new MappingIcon (KeyAction.Id.KEYBINDINGS), icon[9.ToString ()], -1);
			action_icons[KeyAction.Id.SAY] = new Icon (new MappingIcon (KeyAction.Id.SAY), icon[10.ToString ()], -1);
			action_icons[KeyAction.Id.WHISPER] = new Icon (new MappingIcon (KeyAction.Id.WHISPER), icon[11.ToString ()], -1);
			action_icons[KeyAction.Id.PARTYCHAT] = new Icon (new MappingIcon (KeyAction.Id.PARTYCHAT), icon[12.ToString ()], -1);
			action_icons[KeyAction.Id.FRIENDSCHAT] = new Icon (new MappingIcon (KeyAction.Id.FRIENDSCHAT), icon[13.ToString ()], -1);
			action_icons[KeyAction.Id.MENU] = new Icon (new MappingIcon (KeyAction.Id.MENU), icon[14.ToString ()], -1);
			action_icons[KeyAction.Id.QUICKSLOTS] = new Icon (new MappingIcon (KeyAction.Id.QUICKSLOTS), icon[15.ToString ()], -1);
			action_icons[KeyAction.Id.TOGGLECHAT] = new Icon (new MappingIcon (KeyAction.Id.TOGGLECHAT), icon[16.ToString ()], -1);
			action_icons[KeyAction.Id.GUILD] = new Icon (new MappingIcon (KeyAction.Id.GUILD), icon[17.ToString ()], -1);
			action_icons[KeyAction.Id.GUILDCHAT] = new Icon (new MappingIcon (KeyAction.Id.GUILDCHAT), icon[18.ToString ()], -1);
			action_icons[KeyAction.Id.PARTY] = new Icon (new MappingIcon (KeyAction.Id.PARTY), icon[19.ToString ()], -1);
			action_icons[KeyAction.Id.NOTIFIER] = new Icon (new MappingIcon (KeyAction.Id.NOTIFIER), icon[20.ToString ()], -1);
			action_icons[KeyAction.Id.MAPLENEWS] = new Icon (new MappingIcon (KeyAction.Id.MAPLENEWS), icon[21.ToString ()], -1);
			action_icons[KeyAction.Id.CASHSHOP] = new Icon (new MappingIcon (KeyAction.Id.CASHSHOP), icon[22.ToString ()], -1);
			action_icons[KeyAction.Id.ALLIANCECHAT] = new Icon (new MappingIcon (KeyAction.Id.ALLIANCECHAT), icon[23.ToString ()], -1);
			action_icons[KeyAction.Id.MANAGELEGION] = new Icon (new MappingIcon (KeyAction.Id.MANAGELEGION), icon[25.ToString ()], -1);
			action_icons[KeyAction.Id.MEDALS] = new Icon (new MappingIcon (KeyAction.Id.MEDALS), icon[26.ToString ()], -1);
			action_icons[KeyAction.Id.BOSSPARTY] = new Icon (new MappingIcon (KeyAction.Id.BOSSPARTY), icon[27.ToString ()], -1);
			action_icons[KeyAction.Id.PROFESSION] = new Icon (new MappingIcon (KeyAction.Id.PROFESSION), icon[29.ToString ()], -1);
			action_icons[KeyAction.Id.ITEMPOT] = new Icon (new MappingIcon (KeyAction.Id.ITEMPOT), icon[30.ToString ()], -1);
			action_icons[KeyAction.Id.EVENT] = new Icon (new MappingIcon (KeyAction.Id.EVENT), icon[31.ToString ()], -1);
			action_icons[KeyAction.Id.SILENTCRUSADE] = new Icon (new MappingIcon (KeyAction.Id.SILENTCRUSADE), icon[33.ToString ()], -1);
			//action_icons[KeyAction::Id::BITS] = new Icon(<KeyMapIcon>(KeyAction::Id::BITS), icon[34.ToString()], -1);
			action_icons[KeyAction.Id.GUIDE] = new Icon (new MappingIcon (KeyAction.Id.GUIDE), icon[39.ToString ()], -1);
			//action_icons[KeyAction::Id::VIEWERSCHAT] = new Icon(<KeyMapIcon>(KeyAction::Id::VIEWERSCHAT), icon[40.ToString()], -1);
			action_icons[KeyAction.Id.ENHANCEEQUIP] = new Icon (new MappingIcon (KeyAction.Id.ENHANCEEQUIP), icon[41.ToString ()], -1);
			action_icons[KeyAction.Id.MONSTERCOLLECTION] = new Icon (new MappingIcon (KeyAction.Id.MONSTERCOLLECTION), icon[42.ToString ()], -1);
			action_icons[KeyAction.Id.SOULWEAPON] = new Icon (new MappingIcon (KeyAction.Id.SOULWEAPON), icon[43.ToString ()], -1);
			action_icons[KeyAction.Id.CHARINFO] = new Icon (new MappingIcon (KeyAction.Id.CHARINFO), icon[44.ToString ()], -1);
			action_icons[KeyAction.Id.CHANGECHANNEL] = new Icon (new MappingIcon (KeyAction.Id.CHANGECHANNEL), icon[45.ToString ()], -1);
			action_icons[KeyAction.Id.MAINMENU] = new Icon (new MappingIcon (KeyAction.Id.MAINMENU), icon[46.ToString ()], -1);
			action_icons[KeyAction.Id.SCREENSHOT] = new Icon (new MappingIcon (KeyAction.Id.SCREENSHOT), icon[47.ToString ()], -1);
			action_icons[KeyAction.Id.PICTUREMODE] = new Icon (new MappingIcon (KeyAction.Id.PICTUREMODE), icon[48.ToString ()], -1);
			action_icons[KeyAction.Id.MAPLEACHIEVEMENT] = new Icon (new MappingIcon (KeyAction.Id.MAPLEACHIEVEMENT), icon[49.ToString ()], -1);
			action_icons[KeyAction.Id.PICKUP] = new Icon (new MappingIcon (KeyAction.Id.PICKUP), icon[50.ToString ()], -1);
			action_icons[KeyAction.Id.SIT] = new Icon (new MappingIcon (KeyAction.Id.SIT), icon[51.ToString ()], -1);
			action_icons[KeyAction.Id.ATTACK] = new Icon (new MappingIcon (KeyAction.Id.ATTACK), icon[52.ToString ()], -1);
			action_icons[KeyAction.Id.JUMP] = new Icon (new MappingIcon (KeyAction.Id.JUMP), icon[53.ToString ()], -1);
			action_icons[KeyAction.Id.INTERACT_HARVEST] = new Icon (new MappingIcon (KeyAction.Id.INTERACT_HARVEST), icon[54.ToString ()], -1);
			action_icons[KeyAction.Id.FACE1] = new Icon (new MappingIcon (KeyAction.Id.FACE1), icon[100.ToString ()], -1);
			action_icons[KeyAction.Id.FACE2] = new Icon (new MappingIcon (KeyAction.Id.FACE2), icon[101.ToString ()], -1);
			action_icons[KeyAction.Id.FACE3] = new Icon (new MappingIcon (KeyAction.Id.FACE3), icon[102.ToString ()], -1);
			action_icons[KeyAction.Id.FACE4] = new Icon (new MappingIcon (KeyAction.Id.FACE4), icon[103.ToString ()], -1);
			action_icons[KeyAction.Id.FACE5] = new Icon (new MappingIcon (KeyAction.Id.FACE5), icon[104.ToString ()], -1);
			action_icons[KeyAction.Id.FACE6] = new Icon (new MappingIcon (KeyAction.Id.FACE6), icon[105.ToString ()], -1);
			action_icons[KeyAction.Id.FACE7] = new Icon (new MappingIcon (KeyAction.Id.FACE7), icon[106.ToString ()], -1);
			//action_icons[KeyAction::Id::MAPLESTORAGE] = new Icon(<KeyMapIcon>(KeyAction::Id::MAPLESTORAGE), icon[200.ToString()], -1);
			//action_icons[KeyAction::Id::SAFEMODE] = new Icon(<KeyMapIcon>(KeyAction::Id::SAFEMODE), icon[201.ToString()], -1);
			action_icons[KeyAction.Id.MUTE] = new Icon (new MappingIcon (KeyAction.Id.MUTE), icon[202.ToString ()], -1);
			action_icons[KeyAction.Id.MONSTERBOOK] = new Icon (new MappingIcon (KeyAction.Id.MONSTERBOOK), icon[1000.ToString ()], -1);
			action_icons[KeyAction.Id.TOSPOUSE] = new Icon (new MappingIcon (KeyAction.Id.TOSPOUSE), icon[1001.ToString ()], -1);
		}

		private void load_item_icons ()
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: 'var' variable declarations are not supported in C#:
//ORIGINAL LINE: for (var const& it : staged_mappings)
			foreach (var it in staged_mappings)
			{
				Keyboard.Mapping mapping = it.Value;

				if (mapping.type == KeyType.Id.ITEM)
				{
					int item_id = mapping.action;
					short count = inventory.get_total_item_count (item_id);
					Texture tx = get_item_texture (item_id);

					item_icons[item_id] = new Icon (new CountableMappingIcon (mapping, count), tx, count);
				}
			}
		}

		private void load_skill_icons ()
		{
			foreach (var it in staged_mappings)
			{
				Keyboard.Mapping mapping = it.Value;

				if (mapping.type == KeyType.Id.SKILL)
				{
					int skill_id = mapping.action;
					Texture tx = get_skill_texture (skill_id);

					skill_icons[skill_id] = new Icon (new MappingIcon (mapping), tx, -1);
				}
			}
		}

		private void safe_close ()
		{
			if (dirty)
			{
				const string message = "Do you want to save your changes?";

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Lambda expressions cannot be assigned to 'var':
				Action<bool> onok = (bool ok) =>
				{
					if (ok)
					{
						save_staged_mappings ();
						close ();
					}
					else
					{
						close ();
					}
				};
				ms_Unity.FGUI_OK.ShowNotice (message, onok);

				//UI.get ().emplace<UIOk> (message, onok);
			}
			else
			{
				close ();
			}
		}


		/// UI: Tooltip
		private void show_item (int item_id)
		{
			UI.get ().show_item (Tooltip.Parent.KEYCONFIG, item_id);
		}

		private void show_skill (int skill_id)
		{
			int level = skillbook.get_level (skill_id);
			int masterlevel = skillbook.get_masterlevel (skill_id);
			long expiration = skillbook.get_expiration (skill_id);

			UI.get ().show_skill (Tooltip.Parent.KEYCONFIG, skill_id, level, masterlevel, expiration);
		}

		private void clear_tooltip ()
		{
			UI.get ().clear_tooltip (Tooltip.Parent.KEYCONFIG);
		}

		public void save_staged_mappings ()
		{
			List<System.Tuple<KeyConfig.Key, KeyType.Id, int>> updated_actions = new List<System.Tuple<KeyConfig.Key, KeyType.Id, int>> ();

			foreach (var key in staged_mappings)
			{
				KeyConfig.Key k = KeyConfig.actionbyid (key.Key);
				Keyboard.Mapping mapping = key.Value;
				Keyboard.Mapping saved_mapping = keyboard.get_maple_mapping (key.Key);

				if (mapping != saved_mapping)
				{
					updated_actions.Add (System.Tuple.Create (k, mapping.type, mapping.action));
				}
			}

			var maplekeys = keyboard.get_maplekeys ();

			foreach (var key in maplekeys)
			{
				bool keyFound = false;
				KeyConfig.Key keyConfig = KeyConfig.actionbyid (key.Key);

				foreach (var tkey in staged_mappings)
				{
					KeyConfig.Key tKeyConfig = KeyConfig.actionbyid (tkey.Key);

					if (keyConfig == tKeyConfig)
					{
						keyFound = true;
						break;
					}
				}

				if (!keyFound)
				{
					updated_actions.Add (System.Tuple.Create (keyConfig, KeyType.Id.NONE, EnumUtil.GetEnumLength<KeyAction.Id> ()));
				}
			}

			if (updated_actions.Count > 0)
			{
				new ChangeKeyMapPacket (updated_actions).dispatch ();
			}

			foreach (var action in updated_actions)
			{
				KeyConfig.Key key = action.Item1;
				KeyType.Id type = action.Item2;
				int keyAction = action.Item3;

				if (type == KeyType.Id.NONE)
				{
					keyboard.remove ((byte)key);
				}
				else
				{
					keyboard.assign ((byte)key, (byte)type, keyAction);
				}
			}

			dirty = false;
		}

		private void bind_staged_action_keys ()
		{
			foreach (var fkey in key_textures)
			{
				Keyboard.Mapping mapping = get_staged_mapping ((int)fkey.Key);

				if (mapping.type != KeyType.Id.NONE)
				{
					KeyAction.Id action = KeyAction.actionbyid (mapping.action);

					if ((int)action != 0)
					{
						bound_actions.Add (action);
					}
				}
			}
		}

		private void clear ()
		{
			item_icons.Clear ();
			skill_icons.Clear ();
			bound_actions.Clear ();
			staged_mappings = new SortedDictionary<int, Keyboard.Mapping> ();
			dirty = true;
		}

		public void reset ()
		{
			clear ();
			staged_mappings = new SortedDictionary<int, Keyboard.Mapping> (keyboard.get_maplekeys ());
			load_item_icons ();
			load_skill_icons ();
			bind_staged_action_keys ();
			dirty = false;
		}


		/// Helpers
		private Texture get_item_texture (int item_id)
		{
			ItemData data = ItemData.get (item_id);
			return data.get_icon (false);
		}

		private Texture get_skill_texture (int skill_id)
		{
			SkillData data = SkillData.get (skill_id);
			return data.get_icon (SkillData.Icon.NORMAL);
		}

		private KeyConfig.Key key_by_position (Point_short cursorpos)
		{
			foreach (var iter in keys_pos)
			{
				Rectangle_short icon_rect = new Rectangle_short (position + iter.Value, position + iter.Value + new Point_short (32, 32));

				if (icon_rect.contains (cursorpos))
				{
					return iter.Key;
				}
			}

			return KeyConfig.Key.NONE;
		}

		private KeyAction.Id unbound_action_by_position (Point_short cursorpos)
		{
			foreach (var iter in unbound_actions_pos)
			{
				if (bound_actions.Contains (iter.Key))
				{
					continue;
				}

				Rectangle_short icon_rect = new Rectangle_short (position + iter.Value, position + iter.Value + new Point_short (32, 32));

				if (icon_rect.contains (cursorpos))
				{
					return iter.Key;
				}
			}

			return KeyAction.Id.NONE;
		}

		private Keyboard.Mapping get_staged_mapping (int keycode)
		{
			if (!staged_mappings.ContainsKey (keycode))
			{
				return new Keyboard.Mapping ();
			}

			return staged_mappings[keycode];
		}

		private bool is_action_mapping (Keyboard.Mapping mapping)
		{
			if (mapping == null) return false;
			return action_mappings.Any(sourceMapping => sourceMapping == mapping);
			return action_mappings.Contains (mapping);
		}

		private static KeyType.Id get_keytype (KeyAction.Id action)
		{
			switch (action)
			{
				case KeyAction.Id.EQUIPMENT:
				case KeyAction.Id.ITEMS:
				case KeyAction.Id.STATS:
				case KeyAction.Id.SKILLS:
				case KeyAction.Id.FRIENDS:
				case KeyAction.Id.WORLDMAP:
				case KeyAction.Id.MAPLECHAT:
				case KeyAction.Id.MINIMAP:
				case KeyAction.Id.QUESTLOG:
				case KeyAction.Id.KEYBINDINGS:
				case KeyAction.Id.TOGGLECHAT:
				case KeyAction.Id.WHISPER:
				case KeyAction.Id.SAY:
				case KeyAction.Id.PARTYCHAT:
				case KeyAction.Id.MENU:
				case KeyAction.Id.QUICKSLOTS:
				case KeyAction.Id.GUILD:
				case KeyAction.Id.FRIENDSCHAT:
				case KeyAction.Id.PARTY:
				case KeyAction.Id.NOTIFIER:
				case KeyAction.Id.CASHSHOP:
				case KeyAction.Id.GUILDCHAT:
				case KeyAction.Id.MEDALS:
				case KeyAction.Id.BITS:
				case KeyAction.Id.ALLIANCECHAT:
				case KeyAction.Id.MAPLENEWS:
				case KeyAction.Id.MANAGELEGION:
				case KeyAction.Id.PROFESSION:
				case KeyAction.Id.BOSSPARTY:
				case KeyAction.Id.ITEMPOT:
				case KeyAction.Id.EVENT:
				case KeyAction.Id.SILENTCRUSADE:
				case KeyAction.Id.BATTLEANALYSIS:
				case KeyAction.Id.GUIDE:
				case KeyAction.Id.VIEWERSCHAT:
				case KeyAction.Id.ENHANCEEQUIP:
				case KeyAction.Id.MONSTERCOLLECTION:
				case KeyAction.Id.SOULWEAPON:
				case KeyAction.Id.CHARINFO:
				case KeyAction.Id.CHANGECHANNEL:
				case KeyAction.Id.MAINMENU:
				case KeyAction.Id.SCREENSHOT:
				case KeyAction.Id.PICTUREMODE:
				case KeyAction.Id.MAPLEACHIEVEMENT:
					return KeyType.Id.MENU;
				case KeyAction.Id.PICKUP:
				case KeyAction.Id.SIT:
				case KeyAction.Id.ATTACK:
				case KeyAction.Id.JUMP:
					return KeyType.Id.ACTION;
				case KeyAction.Id.INTERACT_HARVEST:
				case KeyAction.Id.MAPLESTORAGE:
				case KeyAction.Id.SAFEMODE:
				case KeyAction.Id.MUTE:
				case KeyAction.Id.MONSTERBOOK:
				case KeyAction.Id.TOSPOUSE:
					return KeyType.Id.MENU;
				case KeyAction.Id.FACE1:
				case KeyAction.Id.FACE2:
				case KeyAction.Id.FACE3:
				case KeyAction.Id.FACE4:
				case KeyAction.Id.FACE5:
				case KeyAction.Id.FACE6:
				case KeyAction.Id.FACE7:
					return KeyType.Id.FACE;
				case KeyAction.Id.LEFT:
				case KeyAction.Id.RIGHT:
				case KeyAction.Id.UP:
				case KeyAction.Id.DOWN:
				case KeyAction.Id.BACK:
				case KeyAction.Id.TAB:
				case KeyAction.Id.RETURN:
				case KeyAction.Id.ESCAPE:
				case KeyAction.Id.SPACE:
				case KeyAction.Id.DELETE:
				case KeyAction.Id.HOME:
				case KeyAction.Id.END:
				case KeyAction.Id.COPY:
				case KeyAction.Id.PASTE:
				case KeyAction.Id.NONE:
				default:
					return KeyType.Id.NONE;
			}
		}

		private enum Buttons : ushort
		{
			CLOSE,
			CANCEL,
			DEFAULT,
			DELETE,
			KEYSETTING,
			OK
		}

		private class MappingIcon : Icon.Type
		{
			/// MappingIcon
			public MappingIcon (Keyboard.Mapping m)
			{
				this.mapping = m;
			}

			public MappingIcon (KeyAction.Id action)
			{
				KeyType.Id type = UIKeyConfig.get_keytype (action);
				mapping = new Keyboard.Mapping (type, (int)action);
			}

			public override void drop_on_stage ()
			{
				if (mapping.type == KeyType.Id.ITEM || mapping.type == KeyType.Id.SKILL)
				{
					var keyconfig = UI.get ().get_element<UIKeyConfig> ();
					keyconfig.get ().unstage_mapping (mapping);
				}
			}

			public override void drop_on_equips (EquipSlot.Id UnnamedParameter1)
			{
			}

			public override bool drop_on_items (InventoryType.Id UnnamedParameter1, EquipSlot.Id UnnamedParameter2, short UnnamedParameter3, bool UnnamedParameter4)
			{
				return true;
			}

			public override void drop_on_bindings (Point_short cursorposition, bool remove)
			{
				var keyconfig = UI.get ().get_element<UIKeyConfig> ();

				if (remove)
				{
					keyconfig.get ().unstage_mapping (mapping);
				}
				else
				{
					keyconfig.get ().stage_mapping (cursorposition, mapping);
				}
			}

			public override void set_count (short UnnamedParameter1)
			{
			}

			public override Icon.IconType get_type ()
			{
				return Icon.IconType.KEY;
			}

			private Keyboard.Mapping mapping = new Keyboard.Mapping ();
		}

		// Used for displaying item counts
		private class CountableMappingIcon : MappingIcon
		{
			public CountableMappingIcon (Keyboard.Mapping m, short c) : base (new Keyboard.Mapping (m))
			{
				this.count = c;
			}

			public override void set_count (short c)
			{
				count = c;
			}

			private short count;
		}

		private readonly Inventory inventory;
		private readonly SkillBook skillbook;

		private bool dirty;

		private Keyboard keyboard = null;

		private MapleData key;
		private MapleData icon;

		private EnumMap<KeyConfig.Key, Texture> key_textures = new EnumMap<KeyConfig.Key, Texture> ();
		private EnumMapNew<KeyConfig.Key, Point_short> keys_pos = new EnumMapNew<KeyConfig.Key, Point_short> ();

		private EnumMap<KeyAction.Id, Icon> action_icons = new EnumMap<KeyAction.Id, Icon> ();
		private EnumMapNew<KeyAction.Id, Point_short> unbound_actions_pos = new EnumMapNew<KeyAction.Id, Point_short> ();

		private SortedDictionary<int, Icon> item_icons = new SortedDictionary<int, Icon> ();
		private SortedDictionary<int, Icon> skill_icons = new SortedDictionary<int, Icon> ();

		// Used to determine if mapping belongs to predefined action, e.g. attack, pick up, faces, etc.
		private List<Keyboard.Mapping> action_mappings = new List<Keyboard.Mapping> ();

		private List<KeyAction.Id> bound_actions = new List<KeyAction.Id> ();
		private SortedDictionary<int, Keyboard.Mapping> staged_mappings = new SortedDictionary<int, Keyboard.Mapping> ();

		private SortedDictionary<int, Keyboard.Mapping> alternate_keys = new SortedDictionary<int, Keyboard.Mapping> ()
		{
			{(int)KeyConfig.Key.ESCAPE, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.MAINMENU)},
			{(int)KeyConfig.Key.F1, new Keyboard.Mapping (KeyType.Id.FACE, (int)KeyAction.Id.FACE1)},
			{(int)KeyConfig.Key.F2, new Keyboard.Mapping (KeyType.Id.FACE, (int)KeyAction.Id.FACE2)},
			{(int)KeyConfig.Key.F3, new Keyboard.Mapping (KeyType.Id.FACE, (int)KeyAction.Id.FACE3)},
			{(int)KeyConfig.Key.F5, new Keyboard.Mapping (KeyType.Id.FACE, (int)KeyAction.Id.FACE4)},
			{(int)KeyConfig.Key.F6, new Keyboard.Mapping (KeyType.Id.FACE, (int)KeyAction.Id.FACE5)},
			{(int)KeyConfig.Key.F7, new Keyboard.Mapping (KeyType.Id.FACE, (int)KeyAction.Id.FACE6)},
			{(int)KeyConfig.Key.F8, new Keyboard.Mapping (KeyType.Id.FACE, (int)KeyAction.Id.FACE7)},
			{(int)KeyConfig.Key.SCROLL_LOCK, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.SCREENSHOT)},
			{(int)KeyConfig.Key.GRAVE_ACCENT, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.CASHSHOP)},
			{(int)KeyConfig.Key.INSERT, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.SAY)},
			{(int)KeyConfig.Key.HOME, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.PARTYCHAT)},
			{(int)KeyConfig.Key.PAGE_UP, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.FRIENDSCHAT)},
			{(int)KeyConfig.Key.T, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.BOSSPARTY)},
			{(int)KeyConfig.Key.Y, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.ITEMPOT)},
			{(int)KeyConfig.Key.U, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.EQUIPMENT)},
			{(int)KeyConfig.Key.I, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.ITEMS)},
			{(int)KeyConfig.Key.P, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.PARTY)},
			{(int)KeyConfig.Key.LEFT_BRACKET, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.MENU)},
			{(int)KeyConfig.Key.RIGHT_BRACKET, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.QUICKSLOTS)},
			{(int)KeyConfig.Key.BACKSLASH, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.KEYBINDINGS)},
			{(int)KeyConfig.Key.DELETE, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.GUILDCHAT)},
			{(int)KeyConfig.Key.END, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.ALLIANCECHAT)},
			{(int)KeyConfig.Key.G, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.GUILD)},
			{(int)KeyConfig.Key.H, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.WHISPER)},
			{(int)KeyConfig.Key.J, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.QUESTLOG)},
			{(int)KeyConfig.Key.K, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.SKILLS)},
			{(int)KeyConfig.Key.L, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.NOTIFIER)},
			{(int)KeyConfig.Key.SEMICOLON, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.MEDALS)},
			{(int)KeyConfig.Key.APOSTROPHE, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.TOGGLECHAT)},
			{(int)KeyConfig.Key.Z, new Keyboard.Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.PICKUP)},
			{(int)KeyConfig.Key.X, new Keyboard.Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.SIT)},
			{(int)KeyConfig.Key.C, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.STATS)},
			{(int)KeyConfig.Key.V, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.EVENT)},
			{(int)KeyConfig.Key.B, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.PROFESSION)},
			{(int)KeyConfig.Key.N, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.WORLDMAP)},
			{(int)KeyConfig.Key.M, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.MINIMAP)},
			{(int)KeyConfig.Key.PERIOD, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.FRIENDS)},
			{(int)KeyConfig.Key.LEFT_CONTROL, new Keyboard.Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.ATTACK)},
			{(int)KeyConfig.Key.LEFT_ALT, new Keyboard.Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.JUMP)},
			{(int)KeyConfig.Key.SPACE, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.INTERACT_HARVEST)},
			{(int)KeyConfig.Key.RIGHT_ALT, new Keyboard.Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.JUMP)},
			{(int)KeyConfig.Key.RIGHT_CONTROL, new Keyboard.Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.ATTACK)}
		};

		private SortedDictionary<int, Keyboard.Mapping> basic_keys = new SortedDictionary<int, Keyboard.Mapping> ()
		{
			{(int)KeyConfig.Key.ESCAPE, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.MAINMENU)},
			{(int)KeyConfig.Key.F1, new Keyboard.Mapping (KeyType.Id.FACE, (int)KeyAction.Id.FACE1)},
			{(int)KeyConfig.Key.F2, new Keyboard.Mapping (KeyType.Id.FACE, (int)KeyAction.Id.FACE2)},
			{(int)KeyConfig.Key.F3, new Keyboard.Mapping (KeyType.Id.FACE, (int)KeyAction.Id.FACE3)},
			{(int)KeyConfig.Key.F5, new Keyboard.Mapping (KeyType.Id.FACE, (int)KeyAction.Id.FACE4)},
			{(int)KeyConfig.Key.F6, new Keyboard.Mapping (KeyType.Id.FACE, (int)KeyAction.Id.FACE5)},
			{(int)KeyConfig.Key.F7, new Keyboard.Mapping (KeyType.Id.FACE, (int)KeyAction.Id.FACE6)},
			{(int)KeyConfig.Key.F8, new Keyboard.Mapping (KeyType.Id.FACE, (int)KeyAction.Id.FACE7)},
			{(int)KeyConfig.Key.SCROLL_LOCK, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.SCREENSHOT)},
			{(int)KeyConfig.Key.GRAVE_ACCENT, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.CASHSHOP)},
			{(int)KeyConfig.Key.NUM1, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.SAY)},
			{(int)KeyConfig.Key.NUM2, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.PARTYCHAT)},
			{(int)KeyConfig.Key.NUM3, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.FRIENDSCHAT)},
			{(int)KeyConfig.Key.NUM4, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.GUILDCHAT)},
			{(int)KeyConfig.Key.NUM5, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.ALLIANCECHAT)},
			{(int)KeyConfig.Key.Q, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.QUESTLOG)},
			{(int)KeyConfig.Key.W, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.WORLDMAP)},
			{(int)KeyConfig.Key.E, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.EQUIPMENT)},
			{(int)KeyConfig.Key.R, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.FRIENDS)},
			{(int)KeyConfig.Key.T, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.BOSSPARTY)},
			{(int)KeyConfig.Key.Y, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.ITEMPOT)},
			{(int)KeyConfig.Key.U, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.GUIDE)},
			{(int)KeyConfig.Key.I, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.ITEMS)},
			{(int)KeyConfig.Key.O, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.ENHANCEEQUIP)},
			{(int)KeyConfig.Key.P, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.PARTY)},
			{(int)KeyConfig.Key.LEFT_BRACKET, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.MENU)},
			{(int)KeyConfig.Key.RIGHT_BRACKET, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.QUICKSLOTS)},
			{(int)KeyConfig.Key.BACKSLASH, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.KEYBINDINGS)},
			{(int)KeyConfig.Key.S, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.STATS)},
			{(int)KeyConfig.Key.G, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.GUILD)},
			{(int)KeyConfig.Key.H, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.WHISPER)},
			{(int)KeyConfig.Key.K, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.SKILLS)},
			{(int)KeyConfig.Key.L, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.NOTIFIER)},
			{(int)KeyConfig.Key.SEMICOLON, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.MEDALS)},
			{(int)KeyConfig.Key.APOSTROPHE, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.TOGGLECHAT)},
			{(int)KeyConfig.Key.Z, new Keyboard.Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.PICKUP)},
			{(int)KeyConfig.Key.X, new Keyboard.Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.SIT)},
			{(int)KeyConfig.Key.C, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.MAPLECHAT)},
			{(int)KeyConfig.Key.V, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.EVENT)},
			{(int)KeyConfig.Key.B, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.PROFESSION)},
			{(int)KeyConfig.Key.M, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.MINIMAP)},
			{(int)KeyConfig.Key.LEFT_CONTROL, new Keyboard.Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.ATTACK)},
			{(int)KeyConfig.Key.LEFT_ALT, new Keyboard.Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.JUMP)},
			{(int)KeyConfig.Key.SPACE, new Keyboard.Mapping (KeyType.Id.MENU, (int)KeyAction.Id.INTERACT_HARVEST)},
			{(int)KeyConfig.Key.RIGHT_ALT, new Keyboard.Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.JUMP)},
			{(int)KeyConfig.Key.RIGHT_CONTROL, new Keyboard.Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.ATTACK)}
		};

		public Point_short GetKeyPos(KeyConfig.Key key)
        {
            if (!keys_pos.values.TryGetValue(key,out var pos))
            {
				pos = Point_short.zero;
				pos.shift_x(16);
				pos.shift_y(16);
			}
			return pos;
		}

		public Point_short GetAbsKeyPos(KeyConfig.Key key)
		{
			return GetKeyPos(key) + position;
		}

		public Icon GetStagedIcon(int mapleKey)
        {
			Icon ficon = null;
			if (staged_mappings.TryGetValue(mapleKey, out var mapping))
			{
				if (mapping.type == KeyType.Id.ITEM)
				{
					int item_id = mapping.action;
					ficon = item_icons[item_id];
				}
				else if (mapping.type == KeyType.Id.SKILL)
				{
					int skill_id = mapping.action;
					ficon = skill_icons[skill_id];
				}
				else if (is_action_mapping(mapping))
				{
					KeyAction.Id action = KeyAction.actionbyid(mapping.action);

					if ((int)action != 0)
					{
						foreach (var it in action_icons)
						{
							if (it.Key == action)
							{
								ficon = it.Value;
								break;
							}
						}
					}
				}
				else
				{
					/*Console.Write("Invalid key mapping: (");//todo maplekey 44(z) type:action ;action:50 doesnot exist
					Console.Write(mapping.type);
					Console.Write(", ");
					Console.Write(mapping.action);
					Console.Write(")");
					Console.Write("\n");*/
				}
			}
			return ficon;

		}

		public override void Dispose ()
		{
			base.Dispose ();
			//key?.Dispose ();
			//icon?.Dispose ();
			foreach (var pair in key_textures)
			{
				pair.Value?.Dispose ();
			}
			key_textures.Clear ();
			
			foreach (var pair in action_icons)
			{
				pair.Value?.Dispose ();
			}
			action_icons.Clear ();
			
			foreach (var pair in item_icons)
			{
				pair.Value?.Dispose ();
			}
			item_icons.Clear ();
			
			foreach (var pair in skill_icons)
			{
				pair.Value?.Dispose ();
			}
			skill_icons.Clear ();
			
			
		}
	}
}