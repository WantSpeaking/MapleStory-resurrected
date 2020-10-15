using System;
using System.Collections.Generic;

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
	public class UIStateGame : UIState
	{
		public override void draw (float inter, Point<short> cursor)
		{
			throw new NotImplementedException ();
		}

		public override void update ()
		{
			throw new NotImplementedException ();
		}

		public override void doubleclick (Point<short> pos)
		{
			throw new NotImplementedException ();
		}

		public override void rightclick (Point<short> pos)
		{
			throw new NotImplementedException ();
		}

		public override void send_key(KeyType.Id type, int action, bool pressed, bool escape)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
			/*if (UIElement focusedelement = get(focused))
			{
				if (focusedelement.is_active())
				{
					return focusedelement.send_key(action, pressed, escape);
				}
				else
				{
					focused = UIElement.NONE;

					return;
				}
			}
			else*/
			/*{
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
									emplace<UIEquipInventory>(Stage.get().get_player().get_inventory());

									break;
								}
								case KeyAction.Id.ITEMS:
								{
									emplace<UIItemInventory>(Stage.get().get_player().get_inventory());

									break;
								}
								case KeyAction.Id.STATS:
								{
									emplace<UIStatsInfo>(Stage.get().get_player().get_stats());

									break;
								}
								case KeyAction.Id.SKILLS:
								{
									emplace<UISkillBook>(Stage.get().get_player().get_stats(), Stage.get().get_player().get_skills());

									break;
								}
								case KeyAction.Id.FRIENDS:
								case KeyAction.Id.PARTY:
								case KeyAction.Id.BOSSPARTY:
								{
									UIUserList.Tab tab;

									switch (action)
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

									var userlist = UI.get().get_element<UIUserList>();

									if (userlist != null && userlist.Dereference().get_tab() != tab && userlist.Dereference().is_active())
									{
										userlist.Dereference().change_tab(tab);
									}
									else
									{
										emplace<UIUserList>(tab);

										if (userlist != null && userlist.Dereference().get_tab() != tab && userlist.Dereference().is_active())
										{
											userlist.Dereference().change_tab(tab);
										}
									}

									break;
								}
								case KeyAction.Id.WORLDMAP:
								{
									emplace<UIWorldMap>();
									break;
								}
								case KeyAction.Id.MAPLECHAT:
								{
									var chat = UI.get().get_element<UIChat>();

									if (chat == null || !chat.Dereference().is_active())
									{
										emplace<UIChat>();
									}

									break;
								}
								case KeyAction.Id.MINIMAP:
								{
									if (auto minimap = UI.get().get_element<UIMiniMap>())
									{
										minimap.send_key(action, pressed, escape);
									}

									break;
								}
								case KeyAction.Id.QUESTLOG:
								{
									emplace<UIQuestLog>(Stage.get().get_player().get_quests());

									break;
								}
								case KeyAction.Id.KEYBINDINGS:
								{
									var keyconfig = UI.get().get_element<UIKeyConfig>();

									if (keyconfig == null || !keyconfig.Dereference().is_active())
									{
										emplace<UIKeyConfig>(Stage.get().get_player().get_inventory(), Stage.get().get_player().get_skills());
									}
									else if (keyconfig != null && keyconfig.Dereference().is_active())
									{
										keyconfig.Dereference().close();
									}

									break;
								}
								case KeyAction.Id.TOGGLECHAT:
								{
									if (auto chatbar = UI.get().get_element<UIChatBar>())
									{
										if (!chatbar.is_chatfieldopen())
										{
											chatbar.toggle_chat();
										}
									}

									break;
								}
								case KeyAction.Id.MENU:
								{
									if (auto statusbar = UI.get().get_element<UIStatusBar>())
									{
										statusbar.toggle_menu();
									}

									break;
								}
								case KeyAction.Id.QUICKSLOTS:
								{
									if (auto statusbar = UI.get().get_element<UIStatusBar>())
									{
										statusbar.toggle_qs();
									}

									break;
								}
								case KeyAction.Id.CASHSHOP:
								{
									EnterCashShopPacket().dispatch();
									break;
								}
								case KeyAction.Id.EVENT:
								{
									emplace<UIEvent>();
									break;
								}
								case KeyAction.Id.CHARINFO:
								{
									emplace<UICharInfo>(Stage.get().get_player().get_oid());

									break;
								}
								case KeyAction.Id.CHANGECHANNEL:
								{
									emplace<UIChannel>();
									break;
								}
								case KeyAction.Id.MAINMENU:
								{
									if (auto statusbar = UI.get().get_element<UIStatusBar>())
									{
										statusbar.send_key(action, pressed, escape);
									}

									break;
								}
								default:
								{
									Console.Write("Unknown KeyAction::Id action: [");
									Console.Write(action);
									Console.Write("]");
									Console.Write("\n");
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
						Stage.get().send_key(type, action, pressed);
						break;
					}
				}
			}*/
			
			Stage.get().send_key(type, action, pressed);
		}

		/*public override Cursor.State send_cursor (Cursor.State mst, Point<short> pos)
		{
			throw new NotImplementedException ();
		}*/

		public override void send_scroll (double yoffset)
		{
			throw new NotImplementedException ();
		}

		public override void send_close ()
		{
			throw new NotImplementedException ();
		}

		/*public override void drag_icon (Icon icon)
		{
			throw new NotImplementedException ();
		}*/

		public override void clear_tooltip (Tooltip.Parent parent)
		{
			throw new NotImplementedException ();
		}

		public override void show_equip (Tooltip.Parent parent, short slot)
		{
			throw new NotImplementedException ();
		}

		public override void show_item (Tooltip.Parent parent, int itemid)
		{
			throw new NotImplementedException ();
		}

		public override void show_skill (Tooltip.Parent parent, int skill_id, int level, int masterlevel, long expiration)
		{
			throw new NotImplementedException ();
		}

		public override void show_text (Tooltip.Parent parent, string text)
		{
			throw new NotImplementedException ();
		}

		public override void show_map (Tooltip.Parent parent, string name, string description, int mapid, bool bolded)
		{
			throw new NotImplementedException ();
		}

		public override EnumMap<UIElement.Type, UIElement> pre_add (UIElement.Type type, bool toggled, bool focused)
		{
			throw new NotImplementedException ();
		}

		public override void remove (UIElement.Type type)
		{
			throw new NotImplementedException ();
		}

		public override UIElement get (UIElement.Type type)
		{
			throw new NotImplementedException ();
		}

		public override UIElement get_front (LinkedList<UIElement.Type> types)
		{
			throw new NotImplementedException ();
		}

		public override UIElement get_front (Point<short> pos)
		{
			throw new NotImplementedException ();
		}

		/*public UIStateGame()
		{
			this.stats = Stage.get().get_player().get_stats();
			this.dragged = null;
			focused = UIElement.Type.NONE;
			tooltipparent = Tooltip.Parent.NONE;

			CharLook look = Stage.get().get_player().get_look();
			Inventory inventory = Stage.get().get_player().get_inventory();

			emplace<UIStatusMessenger>();
			emplace<UIStatusBar>(stats);
			emplace<UIChatBar>();
			emplace<UIMiniMap>(stats);
			emplace<UIBuffList>();
			emplace<UIShop>(look, inventory);

			VWIDTH = Constants.get().get_viewwidth();
			VHEIGHT = Constants.get().get_viewheight();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float inter, Point<short> cursor) const override
		public override void draw(float inter, Point<short> cursor)
		{
			foreach (var type in elementorder)
			{
				auto element = elements[type];

				if (element != null && element.is_active())
				{
					element.draw(inter);
				}
			}

			if (tooltip != null)
			{
				tooltip.Dereference().draw(cursor + new Point<short>(0, 22));
			}

			if (draggedicon != null)
			{
				draggedicon.Dereference().dragdraw(cursor);
			}
		}
		public override void update()
		{
			bool update_screen = false;
			short new_width = Constants.Constants.get().get_viewwidth();
			short new_height = Constants.Constants.get().get_viewheight();

			if (VWIDTH != new_width || VHEIGHT != new_height)
			{
				update_screen = true;
				VWIDTH = new_width;
				VHEIGHT = new_height;

				UI.get().remove(UIElement.Type.STATUSBAR);

				CharStats stats = Stage.get().get_player().get_stats();
				emplace<UIStatusBar>(stats);
			}

			foreach (var type in elementorder)
			{
				auto element = elements[type];

				if (element != null && element.is_active())
				{
					element.update();

					if (update_screen)
					{
						element.update_screen(new_width, new_height);
					}
				}
			}
		}

		public override void doubleclick(Point<short> pos)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: if (UIElement* front = get_front(pos))
			if (UIElement * front = get_front(new ms.Point(new ms.Point(pos))))
			{
				front.doubleclick(pos);
			}
		}
		public override void rightclick(Point<short> pos)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: if (UIElement* front = get_front(pos))
			if (UIElement * front = get_front(new ms.Point(new ms.Point(pos))))
			{
				front.rightclick(pos);
			}
		}
		public override void send_key(KeyType.Id type, int action, bool pressed, bool escape)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
			/*if (UIElement focusedelement = get(focused))
			{
				if (focusedelement.is_active())
				{
					return focusedelement.send_key(action, pressed, escape);
				}
				else
				{
					focused = UIElement.NONE;

					return;
				}
			}
			else#1#
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
									emplace<UIEquipInventory>(Stage.get().get_player().get_inventory());

									break;
								}
								case KeyAction.Id.ITEMS:
								{
									emplace<UIItemInventory>(Stage.get().get_player().get_inventory());

									break;
								}
								case KeyAction.Id.STATS:
								{
									emplace<UIStatsInfo>(Stage.get().get_player().get_stats());

									break;
								}
								case KeyAction.Id.SKILLS:
								{
									emplace<UISkillBook>(Stage.get().get_player().get_stats(), Stage.get().get_player().get_skills());

									break;
								}
								case KeyAction.Id.FRIENDS:
								case KeyAction.Id.PARTY:
								case KeyAction.Id.BOSSPARTY:
								{
									UIUserList.Tab tab;

									switch (action)
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

									var userlist = UI.get().get_element<UIUserList>();

									if (userlist != null && userlist.Dereference().get_tab() != tab && userlist.Dereference().is_active())
									{
										userlist.Dereference().change_tab(tab);
									}
									else
									{
										emplace<UIUserList>(tab);

										if (userlist != null && userlist.Dereference().get_tab() != tab && userlist.Dereference().is_active())
										{
											userlist.Dereference().change_tab(tab);
										}
									}

									break;
								}
								case KeyAction.Id.WORLDMAP:
								{
									emplace<UIWorldMap>();
									break;
								}
								case KeyAction.Id.MAPLECHAT:
								{
									var chat = UI.get().get_element<UIChat>();

									if (chat == null || !chat.Dereference().is_active())
									{
										emplace<UIChat>();
									}

									break;
								}
								case KeyAction.Id.MINIMAP:
								{
									if (auto minimap = UI.get().get_element<UIMiniMap>())
									{
										minimap.send_key(action, pressed, escape);
									}

									break;
								}
								case KeyAction.Id.QUESTLOG:
								{
									emplace<UIQuestLog>(Stage.get().get_player().get_quests());

									break;
								}
								case KeyAction.Id.KEYBINDINGS:
								{
									var keyconfig = UI.get().get_element<UIKeyConfig>();

									if (keyconfig == null || !keyconfig.Dereference().is_active())
									{
										emplace<UIKeyConfig>(Stage.get().get_player().get_inventory(), Stage.get().get_player().get_skills());
									}
									else if (keyconfig != null && keyconfig.Dereference().is_active())
									{
										keyconfig.Dereference().close();
									}

									break;
								}
								case KeyAction.Id.TOGGLECHAT:
								{
									if (auto chatbar = UI.get().get_element<UIChatBar>())
									{
										if (!chatbar.is_chatfieldopen())
										{
											chatbar.toggle_chat();
										}
									}

									break;
								}
								case KeyAction.Id.MENU:
								{
									if (auto statusbar = UI.get().get_element<UIStatusBar>())
									{
										statusbar.toggle_menu();
									}

									break;
								}
								case KeyAction.Id.QUICKSLOTS:
								{
									if (auto statusbar = UI.get().get_element<UIStatusBar>())
									{
										statusbar.toggle_qs();
									}

									break;
								}
								case KeyAction.Id.CASHSHOP:
								{
									EnterCashShopPacket().dispatch();
									break;
								}
								case KeyAction.Id.EVENT:
								{
									emplace<UIEvent>();
									break;
								}
								case KeyAction.Id.CHARINFO:
								{
									emplace<UICharInfo>(Stage.get().get_player().get_oid());

									break;
								}
								case KeyAction.Id.CHANGECHANNEL:
								{
									emplace<UIChannel>();
									break;
								}
								case KeyAction.Id.MAINMENU:
								{
									if (auto statusbar = UI.get().get_element<UIStatusBar>())
									{
										statusbar.send_key(action, pressed, escape);
									}

									break;
								}
								default:
								{
									Console.Write("Unknown KeyAction::Id action: [");
									Console.Write(action);
									Console.Write("]");
									Console.Write("\n");
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
						Stage.get().send_key(type, action, pressed);
						break;
					}
				}
			}
		}
		public override Cursor.State send_cursor(Cursor.State cursorstate, Point<short> cursorpos)
		{
			if (draggedicon != null)
			{
				if (cursorstate == Cursor.State.CLICKING)
				{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: if (drop_icon(*draggedicon, cursorpos))
					if (drop_icon(draggedicon.Indirection(), new ms.Point(new ms.Point(cursorpos))))
					{
						remove_icon();
					}

					return cursorstate;
				}

				return Cursor.State.GRABBING;
			}
			else
			{
				bool clicked = cursorstate == Cursor.State.CLICKING || cursorstate == Cursor.State.VSCROLLIDLE;

				if (auto focusedelement = get(focused))
				{
					if (focusedelement.is_active())
					{
						remove_cursor(focusedelement.get_type());

						return focusedelement.send_cursor(clicked, cursorpos);
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

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: if (auto front = get_front(cursorpos))
						if (auto front = get_front(new ms.Point(new ms.Point(cursorpos))))
						{
							UIElement.Type front_type = front.get_type();

							if (tooltipparent != UIElement.Type.NONE)
							{
								if (front_type != tooltipparent)
								{
									clear_tooltip(tooltipparent);
								}
							}

							remove_cursor(front_type);

							return front.send_cursor(clicked, cursorpos);
						}
						else
						{
							remove_cursors();

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return Stage::get().send_cursor(clicked, cursorpos);
							return Stage.get().send_cursor(clicked, new ms.Point(cursorpos));
						}
					}
					else
					{

						if (dragged == null)
						{
							UIElement.Type drag_element_type = UIElement.Type.NONE;

							for (var iter = elementorder.rbegin(); iter != elementorder.rend(); ++iter)
							{
								auto element = elements[*iter];

								if (element != null && element.is_active() && element.is_in_range(cursorpos))
								{
									dragged = element.get();
									drag_element_type = iter;
									break;
								}
							}

							if (drag_element_type != UIElement.Type.NONE)
							{
								elementorder.Remove(drag_element_type);
								elementorder.AddLast(drag_element_type);
							}
						}

						if (dragged != null)
						{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return dragged->send_cursor(clicked, cursorpos);
							return dragged.send_cursor(clicked, new ms.Point(cursorpos));
						}
						else
						{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return Stage::get().send_cursor(clicked, cursorpos);
							return Stage.get().send_cursor(clicked, new ms.Point(cursorpos));
						}
					}
				}
			}
		}
		public override void send_scroll(double yoffset)
		{
			foreach (var type in elementorder)
			{
				auto element = elements[type];

				if (element != null && element.is_active())
				{
					element.send_scroll(yoffset);
				}
			}
		}
		public override void send_close()
		{
			UI.get().emplace<UIQuit>(stats);
		}

		public override void drag_icon(Icon drgic)
		{
			draggedicon = drgic;
		}
		public override void clear_tooltip(Tooltip.Parent parent)
		{
			if (parent == tooltipparent)
			{
				eqtooltip.set_equip(Tooltip.Parent.NONE, 0);
				ittooltip.set_item(0);
				tetooltip.set_text("");
				matooltip.reset();
				tooltip = {};
				tooltipparent = Tooltip.Parent.NONE;
			}
		}
		public override void show_equip(Tooltip.Parent parent, short slot)
		{
			eqtooltip.set_equip(parent, slot);

			if (slot != 0)
			{
				tooltip = eqtooltip;
				tooltipparent = parent;
			}
		}
		public override void show_item(Tooltip.Parent parent, int itemid)
		{
			ittooltip.set_item(itemid);

			if (itemid != 0)
			{
				tooltip = ittooltip;
				tooltipparent = parent;
			}
		}
		public override void show_skill(Tooltip.Parent parent, int skill_id, int level, int masterlevel, long expiration)
		{
			sktooltip.set_skill(skill_id, level, masterlevel, expiration);

			if (skill_id != 0)
			{
				tooltip = sktooltip;
				tooltipparent = parent;
			}
		}
		public override void show_text(Tooltip.Parent parent, string text)
		{
			tetooltip.set_text(text);

			if (!string.IsNullOrEmpty(text))
			{
				tooltip = tetooltip;
				tooltipparent = parent;
			}
		}
		public override void show_map(Tooltip.Parent parent, string name, string description, int mapid, bool bolded)
		{
			matooltip.set_name(parent, name, bolded);
			matooltip.set_desc(description);
			matooltip.set_mapid(mapid);

			if (!string.IsNullOrEmpty(name))
			{
				tooltip = matooltip;
				tooltipparent = parent;
			}
		}

		public override EnumMap<UIElement.Type, std::unique_ptr<UIElement>, UIElement.Type.NUM_TYPES>.iterator pre_add(UIElement.Type type, bool is_toggled, bool is_focused)
		{
			auto element = elements[(int)type];

			if (element != null && is_toggled)
			{
				elementorder.Remove(type);
				elementorder.AddLast(type);

				bool active = element.is_active();

				element.toggle_active();

				if (active != element.is_active())
				{
					if (element.is_active())
					{
						if (type == UIElement.Type.WORLDMAP)
						{
							Sound(Sound.Name.WORLDMAPOPEN).play();
						}
						else
						{
							Sound(Sound.Name.MENUUP).play();
						}

						UI.get().send_cursor(false);
					}
					else
					{
						if (type == UIElement.Type.WORLDMAP)
						{
							Sound(Sound.Name.WORLDMAPCLOSE).play();
						}
						else
						{
							Sound(Sound.Name.MENUDOWN).play();
						}

						element.remove_cursor();

						if (draggedicon != null)
						{
							if (element.get_type() == icon_map[(int)draggedicon.get().get_type()])
							{
								remove_icon();
							}
						}

						UI.get().send_cursor(false);
					}
				}

				return elements.end();
			}
			else
			{
				remove(type);
				elementorder.AddLast(type);

				if (is_focused)
				{
					focused = type;
				}

				return elements.find(type);
			}
		}
		public override void remove(UIElement.Type type)
		{
			if (type == focused)
			{
				focused = UIElement.Type.NONE;
			}

			if (type == tooltipparent)
			{
				clear_tooltip(tooltipparent);
			}

			elementorder.Remove(type);

			if ((auto & element = elements[(int)type]) != 0)
			{
				element.deactivate();
				element.release();
			}
		}
		public override UIElement get(UIElement.Type type)
		{
			return elements[(int)type].get();
		}
		public override UIElement get_front(LinkedList<UIElement.Type> types)
		{
			var begin = elementorder.rbegin();
			var end = elementorder.rend();

			for (var iter = begin; iter != end; ++iter)
			{
				if (types.Contains(*iter))
				{
					auto element = elements[*iter];

					if (element != null && element.is_active())
					{
						return element.get();
					}
				}
			}

			return null;
		}
		public override UIElement get_front(Point<short> pos)
		{
			var begin = elementorder.rbegin();
			var end = elementorder.rend();

			for (var iter = begin; iter != end; ++iter)
			{
				auto element = elements[*iter];

				if (element != null && element.is_active() && element.is_in_range(pos))
				{
					return element.get();
				}
			}

			return null;
		}

		private readonly CharStats stats;

		private bool drop_icon(Icon icon, Point<short> pos)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: if (UIElement* front = get_front(pos))
			if (UIElement * front = get_front(new ms.Point(new ms.Point(pos))))
			{
				return front.send_icon(icon, pos);
			}
			else
			{
				icon.drop_on_stage();
			}

			return true;
		}
		private void remove_icon()
		{
			draggedicon.Dereference().reset();
			draggedicon = {};
		}
		private void remove_cursors()
		{
			foreach (var type in elementorder)
			{
				var element = elements[type].get();

				if (element && element.is_active())
				{
					element.remove_cursor();
				}
			}
		}
		private void remove_cursor(UIElement.Type t)
		{
			foreach (var type in elementorder)
			{
				var element = elements[type].get();

				if (element && element.is_active() && element.get_type() != t)
				{
					element.remove_cursor();
				}
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent in C# to C++11 variadic templates:
		private void emplace<T, typename...Args>(Args & ...args)
		{
			if (auto iter = pre_add(T.TYPE, T.TOGGLED, T.FOCUSED))
			{
				iter.second = std::make_unique<T>(std::forward<Args>(args)...);

				var silent_types = UIElement.Type.STATUSMESSENGER, UIElement.Type.STATUSBAR, UIElement.Type.CHATBAR, UIElement.Type.MINIMAP, UIElement.Type.BUFFLIST, UIElement.Type.NPCTALK, UIElement.Type.SHOP;

				if (std::find(silent_types.begin(), silent_types.end(), T.TYPE) == silent_types.end())
				{
					if (T.TYPE == UIElement.Type.WORLDMAP)
					{
						Sound(Sound.Name.WORLDMAPOPEN).play();
					}
					else
					{
						Sound(Sound.Name.MENUUP).play();
					}

					UI.get().send_cursor(false);
				}
			}
		}

		private EnumMap<UIElement.Type, std::unique_ptr<UIElement>, UIElement.Type.NUM_TYPES> elements = new EnumMap<UIElement.Type, std::unique_ptr<UIElement>, UIElement.Type.NUM_TYPES>();
		private LinkedList<UIElement.Type> elementorder = new LinkedList<UIElement.Type>();
		private UIElement.Type focused;
		private UIElement dragged;

		private EquipTooltip eqtooltip = new EquipTooltip();
		private ItemTooltip ittooltip = new ItemTooltip();
		private SkillTooltip sktooltip = new SkillTooltip();
		private TextTooltip tetooltip = new TextTooltip();
		private MapTooltip matooltip = new MapTooltip();
		private Optional<Tooltip> tooltip = new Optional<Tooltip>();
		private Tooltip.Parent tooltipparent;

		private Optional<Icon> draggedicon = new Optional<Icon>();

		private SortedDictionary<Icon.IconType, UIElement.Type> icon_map = new SortedDictionary<Icon.IconType, UIElement.Type>()
		{
			{Icon.IconType.NONE, UIElement.Type.NONE},
			{Icon.IconType.SKILL, UIElement.Type.SKILLBOOK},
			{Icon.IconType.EQUIP, UIElement.Type.EQUIPINVENTORY},
			{Icon.IconType.ITEM, UIElement.Type.ITEMINVENTORY},
			{Icon.IconType.KEY, UIElement.Type.KEYCONFIG},
			{Icon.IconType.NUM_TYPES, UIElement.Type.NUM_TYPES}
		};

		private short VWIDTH;
		private short VHEIGHT;*/
	}
}



