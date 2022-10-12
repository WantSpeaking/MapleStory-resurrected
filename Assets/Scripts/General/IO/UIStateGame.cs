using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Beebyte.Obfuscator;
using ms_Unity;




namespace ms
{
	[Skip]
	public class UIStateGame : UIState
	{
		FGUI_StateGame fgui_StateGame;

		public UIStateGame ()
		{
			ushort width = Setting<Width>.get().load();
			ushort height = Setting<Height>.get().load();
			//Window.get().ChangeResloution((short)width, (short)height);

			this.stats = Stage.get ().get_player ().get_stats ();
			this.dragged = null;
			focused = UIElement.Type.NONE;
			tooltipparent = Tooltip.Parent.NONE;

			CharLook look = Stage.get ().get_player ().get_look ();
			Inventory inventory = Stage.get ().get_player ().get_inventory ();

			emplace<UIStatusMessenger> ();
			emplace<UIStatusBar> (stats);
			emplace<UIChatBar> ();
			emplace<UIMiniMap> (stats);
			emplace<UIBuffList> ();
			emplace<UIShop> (look, inventory);
	

			emplace<UIKeyConfig>(Stage.get().get_player().get_inventory(), Stage.get().get_player().get_skills());
			get(UIElement.Type.KEYCONFIG).deactivate();

			emplace<UIJoystick> ();
			emplace<UIActionButton> ();

			VWIDTH = Constants.get ().get_viewwidth ();
			VHEIGHT = Constants.get ().get_viewheight ();

			fgui_StateGame = FGUI_StateGame.CreateInstance ();
		}

		public override void draw (float inter, Point_short cursor)
		{
			foreach (var type in elementorder)
			{
				var element = elements[type];

				if (element != null && element.is_active ())
				{
					element.draw (inter);
				}
			}

			if (tooltip)
			{
				tooltip.get ().draw (cursor + new Point_short (0, 22));
			}

			if (draggedicon)
			{
				draggedicon.get ().dragdraw (cursor);
			}
		}

		public override void update ()
		{
			bool update_screen = false;
			short new_width = Constants.get ().get_viewwidth ();
			short new_height = Constants.get ().get_viewheight ();

			if (VWIDTH != new_width || VHEIGHT != new_height)
			{
				update_screen = true;
				VWIDTH = new_width;
				VHEIGHT = new_height;

				UI.get ().remove (UIElement.Type.STATUSBAR);

				CharStats stats = Stage.get ().get_player ().get_stats ();
				emplace<UIStatusBar> (stats);
			}

			foreach (var type in elementorder)
			{
				var element = elements[type];

				if (element != null && element.is_active ())
				{
					element.update ();

					if (update_screen)
					{
						element.update_screen (new_width, new_height);
					}
				}
			}
		}

		public override void doubleclick (Point_short pos)
		{
			UIElement front = get_front (new Point_short (pos));
			if (front != null)
			{
				front.doubleclick (pos);
			}
		}

		public override void rightclick (Point_short pos)
		{
			UIElement front = get_front (new Point_short (pos));
			if (front != null)
			{
				front.rightclick (pos);
			}
			else
			{
				var rightClickedChar = Stage.get ().get_chars ().findFirstRightClickedChar (pos);
				if (rightClickedChar)
				{
					var contextmenu = UI.get ().emplace<UIContextMenu> ();
					if (contextmenu)
					{
						contextmenu.get ().SetDisplayInfo (pos, rightClickedChar.get ());
					}
				}
			}
		}

		public override void send_key (KeyType.Id type, int action, bool pressed, bool escape, bool pressing = false)
		{
			UIElement focusedelement = get (focused);
			if (focusedelement != null)
			{
				if (focusedelement.is_active ())
				{
					focusedelement.send_key (action, pressed, escape);
					return;
				}
				else
				{
					focused = UIElement.Type.NONE;

					return;
				}
			}
			else
			{
				switch (type)
				{
					case KeyType.Id.MENU:
					{
						if (pressed)
						{
							switch ((KeyAction.Id)action)
							{
								case KeyAction.Id.EQUIPMENT:
								{
									emplace<UIEquipInventory> (Stage.get ().get_player ().get_inventory ());

									break;
								}
								case KeyAction.Id.ITEMS:
								{
									emplace<UIItemInventory> (Stage.get ().get_player ().get_inventory ());

									break;
								}
								case KeyAction.Id.STATS:
								{
									emplace<UIStatsInfo> (Stage.get ().get_player ().get_stats ());

									break;
								}
								case KeyAction.Id.SKILLS:
								{
									emplace<UISkillBook> (Stage.get ().get_player ().get_stats (), Stage.get ().get_player ().get_skills ());

									break;
								}
								case KeyAction.Id.FRIENDS:
								case KeyAction.Id.PARTY:
								case KeyAction.Id.BOSSPARTY:
								{
									UIUserList.Tab tab = 0;

									switch ((KeyAction.Id)action)
									{
										case KeyAction.Id.FRIENDS:
											tab = UIUserList.Tab.FRIEND;
											break;
										case KeyAction.Id.PARTY:
											tab = UIUserList.Tab.PARTY;
											break;
										case KeyAction.Id.BOSSPARTY:
											tab = UIUserList.Tab.BOSS;
											break;
									}

									var userlist = UI.get ().get_element<UIUserList> ();

									if (userlist && userlist.get ().get_tab () != (int)tab && userlist.get ().is_active ())
									{
										userlist.get ().change_tab ((byte)tab);
									}
									else
									{
										UI.get ().emplace<UIUserList> ((ushort)tab);

										if (userlist && userlist.get ().get_tab () != (int)tab && userlist.get ().is_active ())
										{
											userlist.get ().change_tab ((byte)tab);
										}
									}

									break;
								}
								case KeyAction.Id.WORLDMAP:
								{
									UI.get ().emplace<UIWorldMap> ();
									break;
								}
								case KeyAction.Id.MAPLECHAT:
								{
									var chat = UI.get ().get_element<UIChat> ();

									if (chat == false || !chat.get ().is_active ())
									{
										UI.get ().emplace<UIChat> ();
									}

									break;
								}
								case KeyAction.Id.MINIMAP:
								{
									var minimap = UI.get ().get_element<UIMiniMap> ();
									if (minimap)
									{
										minimap.get ().send_key (action, pressed, escape);
									}

									break;
								}
								case KeyAction.Id.QUESTLOG:
								{
									UI.get ().emplace<UIQuestLog> (Stage.get ().get_player ().get_questlog ());

									break;
								}
								case KeyAction.Id.KEYBINDINGS:
								{
									var keyconfig = UI.get ().get_element<UIKeyConfig> ();

									if (keyconfig == false || !keyconfig.get ().is_active ())
									{
										UI.get ().emplace<UIKeyConfig> (Stage.get ().get_player ().get_inventory (), Stage.get ().get_player ().get_skills ());
									}
									else if (keyconfig && keyconfig.get ().is_active ())
									{
										keyconfig.get ().close ();
									}

									break;
								}
								case KeyAction.Id.TOGGLECHAT:
								{
									var chatbar = UI.get ().get_element<UIChatBar> ();
									if (chatbar)
									{
										if (!chatbar.get ().is_chatfieldopen ())
										{
											chatbar.get ().toggle_chat ();
										}
									}

									break;
								}
								case KeyAction.Id.MENU:
								{
									var statusbar = UI.get ().get_element<UIStatusBar> ();
									if (statusbar)
									{
										statusbar.get ().toggle_menu ();
									}

									break;
								}
								case KeyAction.Id.QUICKSLOTS:
								{
									var statusbar = UI.get ().get_element<UIStatusBar> ();
									if (statusbar)
									{
										statusbar.get ().toggle_qs ();
									}

									break;
								}
								case KeyAction.Id.CASHSHOP:
								{
									new EnterCashShopPacket ().dispatch ();
									break;
								}
								case KeyAction.Id.EVENT:
								{
									UI.get ().emplace<UIEvent> ();
									break;
								}
								case KeyAction.Id.CHARINFO:
								{
									UI.get ().emplace<UICharInfo> (Stage.get ().get_player ().get_oid ());

									break;
								}
								case KeyAction.Id.CHANGECHANNEL:
								{
									UI.get ().emplace<UIChannel> ();
									break;
								}
								case KeyAction.Id.MAINMENU:
								{
									var statusbar = UI.get ().get_element<UIStatusBar> ();
									if (statusbar)
									{
										statusbar.get ().send_key (action, pressed, escape);
									}

									break;
								}
								default:
								{
									Console.Write ("Unknown KeyAction::Id action: [");
									Console.Write (action);
									Console.Write ("]");
									Console.Write ("\n");
									break;
								}
							}
						}

						break;
					}
					case KeyType.Id.ACTION:
					case KeyType.Id.FACE:
					case KeyType.Id.ITEM:
					case KeyType.Id.SKILL:
					{
						Stage.get ().send_key (type, action, pressed, pressing);
						break;
					}
				}
			}
		}

		public override Cursor.State send_cursor (Cursor.State cursorstate, Point_short cursorpos)
		{
			if (draggedicon)
			{
				if (cursorstate == Cursor.State.CLICKING)
				{
					if (drop_icon (draggedicon.get (), new Point_short (cursorpos)))
					{
						remove_icon ();
						AppDebug.Log ($"remove_icon");
					}

					return cursorstate;
				}

				return Cursor.State.GRABBING;
			}
			else
			{
				bool clicked = cursorstate == Cursor.State.CLICKING || cursorstate == Cursor.State.VSCROLLIDLE;
				var focusedelement = get (focused);
				if (focusedelement != null)
				{
					if (focusedelement.is_active ())
					{
						remove_cursor (focusedelement.get_type ());

						return focusedelement.send_cursor (clicked, cursorpos);
					}
					else
					{
						focused = UIElement.Type.NONE;

						return cursorstate;
					}
				}
				else
				{
					if (!clicked)
					{
						dragged = null;

						var front = get_front (new Point_short (cursorpos));
						if (front != null)
						{
							UIElement.Type front_type = front.get_type ();

							if (tooltipparent != (int)UIElement.Type.NONE)
							{
								if (front_type != (UIElement.Type)tooltipparent)
								{
									clear_tooltip (tooltipparent);
								}
							}

							remove_cursor (front_type);

                            if (front is UIJoystick || front is UIActionButton)
                            {
								Stage.get().send_cursor(clicked, new Point_short(cursorpos));
							}
							return front.send_cursor (clicked, cursorpos);
						}
						else
						{
							remove_cursors ();

							return Stage.get ().send_cursor (clicked, new Point_short (cursorpos));
						}
					}
					else
					{
						if (dragged == null)
						{
							UIElement.Type drag_element_type = UIElement.Type.NONE;

							var llnLastPrivous = elementorder.Last;
							while (llnLastPrivous != null)
                            {
								var element = elements[llnLastPrivous.Value];

								if (element != null && element.is_active() && element.is_in_range(cursorpos))
								{
									dragged = element;
									drag_element_type = llnLastPrivous.Value;
									break;
								}

								llnLastPrivous = llnLastPrivous.Previous;
							}

							if (drag_element_type != UIElement.Type.NONE)
							{
								elementorder.Remove (drag_element_type);
								elementorder.AddLast (drag_element_type);
							}
						}

						if (dragged != null)
						{
							return dragged.send_cursor (clicked, new Point_short (cursorpos));
						}
						else
						{
							return Stage.get ().send_cursor (clicked, new Point_short (cursorpos));
						}
					}
				}
			}
		}

		public override void send_scroll (double yoffset)
		{
			foreach (var type in elementorder)
			{
				var element = elements[type];

				if (element != null && element.is_active ())
				{
					element.send_scroll (yoffset);
				}
			}
		}

		public override void send_close ()
		{
			UI.get ().emplace<UIQuit> (stats);
		}

		public override void drag_icon (Icon drgic)
		{
			draggedicon = drgic;
		}

		public override void clear_tooltip (Tooltip.Parent parent)
		{
			if (parent == tooltipparent)
			{
				eqtooltip.set_equip (Tooltip.Parent.NONE, 0);
				ittooltip.set_item (0);
				tetooltip.set_text ("");
				matooltip.reset ();
				tooltip = new Optional<Tooltip> ();
				tooltipparent = Tooltip.Parent.NONE;
			}
		}

		public override void show_equip (Tooltip.Parent parent, short slot)
		{
			eqtooltip.set_equip (parent, slot);

			if (slot != 0)
			{
				tooltip = eqtooltip;
				tooltipparent = parent;
			}
		}

		public override void show_item (Tooltip.Parent parent, int itemid)
		{
			ittooltip.set_item (itemid);

			if (itemid != 0)
			{
				tooltip = ittooltip;
				tooltipparent = parent;
			}
		}

		public override void show_skill (Tooltip.Parent parent, int skill_id, int level, int masterlevel, long expiration)
		{
			sktooltip.set_skill (skill_id, level, masterlevel, expiration);

			if (skill_id != 0)
			{
				tooltip = sktooltip;
				tooltipparent = parent;
			}
		}

		public override void show_text (Tooltip.Parent parent, string text)
		{
			tetooltip.set_text (text);

			if (!string.IsNullOrEmpty (text))
			{
				tooltip = tetooltip;
				tooltipparent = parent;
			}
		}

		public override void show_map (Tooltip.Parent parent, string name, string description, int mapid, bool bolded)
		{
			matooltip.set_name (parent, name, bolded);
			matooltip.set_desc (description);
			matooltip.set_mapid (mapid);

			if (!string.IsNullOrEmpty (name))
			{
				tooltip = matooltip;
				tooltipparent = parent;
			}
		}


		UIElement.Type[] silent_types = new[]
		{
			UIElement.Type.STATUSMESSENGER,
			UIElement.Type.STATUSBAR,
			UIElement.Type.CHATBAR,
			UIElement.Type.MINIMAP,
			UIElement.Type.BUFFLIST,
			UIElement.Type.NPCTALK,
			UIElement.Type.SHOP
		};

		public T emplace<T> (params object[] args) where T : UIElement
		{
			var type = typeof (T);
			var uiElement_Type = (UIElement.Type)(type.GetField ("TYPE", Constants.get ().bindingFlags_UIElementInfo)?.GetRawConstantValue () ?? UIElement.Type.NONE);
			var uiElement_toggled = (bool)(type.GetField ("TOGGLED", Constants.get ().bindingFlags_UIElementInfo)?.GetRawConstantValue () ?? false);
			var uiElement_focused = (bool)(type.GetField ("FOCUSED", Constants.get ().bindingFlags_UIElementInfo)?.GetRawConstantValue () ?? false);

			var uiElementType_New = pre_add (uiElement_Type, uiElement_toggled, uiElement_focused);
			if (uiElementType_New != UIElement.Type.NONE)
			{
				return actual_add<T> (uiElementType_New, args);
			}

			if (!silent_types.Contains (uiElement_Type))
			{
				if (uiElement_Type == UIElement.Type.WORLDMAP)
					new Sound (Sound.Name.WORLDMAPOPEN).play ();
				else
					new Sound (Sound.Name.MENUUP).play ();

				UI.get ().send_cursor (false);
			}

			return null;
		}

		public override UIElement.Type pre_add (UIElement.Type type, bool is_toggled, bool is_focused)
		{
			var element = elements.TryGetValue (type);

			if (element != null && is_toggled)
			{
				elementorder.Remove (type);
				elementorder.AddLast(type);

				bool active = element.is_active ();

				element.toggle_active ();

				if (active != element.is_active ())
				{
					if (element.is_active ())
					{
						if (type == UIElement.Type.WORLDMAP)
						{
							new Sound (Sound.Name.WORLDMAPOPEN).play ();
						}
						else
						{
							new Sound (Sound.Name.MENUUP).play ();
						}

						UI.get ().send_cursor (false);
					}
					else
					{
						if (type == UIElement.Type.WORLDMAP)
						{
							new Sound (Sound.Name.WORLDMAPCLOSE).play ();
						}
						else
						{
							new Sound (Sound.Name.MENUDOWN).play ();
						}

						element.remove_cursor ();

						if (draggedicon)
						{
							if (element.get_type () == icon_map[draggedicon.get ().get_type ()])
							{
								remove_icon ();
							}
						}

						UI.get ().send_cursor (false);
					}
				}

				return UIElement.Type.NONE;
				//return elements;
			}
			else
			{
				remove (type);
				elementorder.AddLast(type);

				if (is_focused)
				{
					focused = type;
				}

				//return elements.find (type);
				return type;
			}
		}

		public override T actual_add<T> (UIElement.Type type, params object[] args)
		{
			var uiElement = (UIElement)System.Activator.CreateInstance (typeof (T), args);
			if (elements.TryAdd (type, uiElement))
			{
				uiElement?.OnAdd ();
				uiElement?.OnActivityChange (uiElement.is_active());
			}
			return (T)uiElement;
		}

		public override void remove (UIElement.Type type)
		{
			if (type == focused)
			{
				focused = UIElement.Type.NONE;
			}

			if (type == (UIElement.Type)tooltipparent)
			{
				clear_tooltip (tooltipparent);
			}

			elementorder.Remove (type);

			if (elements.TryRemove (type, out var removedElement))
			{
				removedElement?.OnRemove ();
			}
		}

		public override UIElement get (UIElement.Type type)
		{
			var wantedElement = elements.TryGetValue (type);
			wantedElement?.OnGet ();
			return wantedElement;
		}

		public override UIElement get_front (LinkedList<UIElement.Type> types)
		{
			var llnLastPrivous = elementorder.Last;
			while (llnLastPrivous != null)
			{
				foreach (var type in types)
                {
                    if (type == llnLastPrivous.Value)
                    {
						var element = elements[llnLastPrivous.Value];
						if (element != null && element.is_active())
						{
							return element;
						}
					}
				}

				llnLastPrivous = llnLastPrivous.Previous;
			}

			return null;
		}

		public override UIElement get_front (Point_short pos)
		{
			var llnLastPrivous = elementorder.Last;
			while (llnLastPrivous != null)
			{
				var element = elements[llnLastPrivous.Value];

				if (element != null && element.is_active() && element.is_in_range(pos))
				{
					return element;
				}

				llnLastPrivous = llnLastPrivous.Previous;
			}

			return null;
		}

		private readonly CharStats stats;

		private bool drop_icon (Icon icon, Point_short pos)
		{
			if (icon == null)
			{
				//AppDebug.Log ($"drop_icon() icon is null");
				return false;
			}

			UIElement front = get_front (new Point_short (pos));
			if (front != null)
			{
				return front.send_icon (icon, pos);
			}
			else
			{
				icon.drop_on_stage ();
			}

			return true;
		}

		private void remove_icon ()
		{
			draggedicon.get ().reset ();
			draggedicon = new Optional<Icon> ();
		}

		private void remove_cursors ()
		{
			foreach (var type in elementorder)
			{
				var element = elements.TryGetValue (type);

				if (element != null && element.is_active ())
				{
					element.remove_cursor ();
				}
			}
		}

		private void remove_cursor (UIElement.Type t)
		{
			foreach (var type in elementorder)
			{
				var element = elements.TryGetValue (type);

				if (element != null && element.is_active () && element.get_type () != t)
				{
					element.remove_cursor ();
				}
			}
		}

		private ConcurrentDictionary<UIElement.Type, UIElement> elements = new ConcurrentDictionary<UIElement.Type, UIElement> ();
		private LinkedList<UIElement.Type> elementorder = new LinkedList<UIElement.Type> ();
		private UIElement.Type focused;
		private UIElement dragged;

		private EquipTooltip eqtooltip = new EquipTooltip ();
		private ItemTooltip ittooltip = new ItemTooltip ();
		private SkillTooltip sktooltip = new SkillTooltip ();
		private TextTooltip tetooltip = new TextTooltip ();
		private MapTooltip matooltip = new MapTooltip ();
		private Optional<Tooltip> tooltip = new Optional<Tooltip> ();
		private Tooltip.Parent tooltipparent;

		private Optional<Icon> draggedicon = new Optional<Icon> ();

		private SortedDictionary<Icon.IconType, UIElement.Type> icon_map = new SortedDictionary<Icon.IconType, UIElement.Type> ()
		{
			{Icon.IconType.NONE, UIElement.Type.NONE},
			{Icon.IconType.SKILL, UIElement.Type.SKILLBOOK},
			{Icon.IconType.EQUIP, UIElement.Type.EQUIPINVENTORY},
			{Icon.IconType.ITEM, UIElement.Type.ITEMINVENTORY},
			{Icon.IconType.KEY, UIElement.Type.KEYCONFIG},
			{Icon.IconType.NUM_TYPES, UIElement.Type.NUM_TYPES}
		};

		private short VWIDTH;
		private short VHEIGHT;
	}
}