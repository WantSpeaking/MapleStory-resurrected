#define USE_NX

using System;
using System.Collections.Generic;
using Beebyte.Obfuscator;
using MapleLib.WzLib;




namespace ms
{
	//[Skip]
	public class UIMiniMap : UIDragElement<PosMINIMAP>
	{
		public const UIElement.Type TYPE = UIElement.Type.MINIMAP;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public UIMiniMap (params object[]args):this((CharStats)args[0]){}

		public UIMiniMap (CharStats stats) : base (new Point_short (128, 20))
		{
			this.stats = stats;
			big_map = true;
			has_map = false;
			listNpc_enabled = false;
			listNpc_dimensions = new Point_short (150, 170);
			listNpc_offset = 0;
			selected = -1;

			type = (sbyte)Setting<MiniMapType>.get ().load ();
			user_type = type;
			simpleMode = Setting<MiniMapSimpleMode>.get ().load ();

			string node = simpleMode ? "MiniMapSimpleMode" : "MiniMap";
			MiniMap = ms.wz.wzFile_ui["UIWindow2.img"][node];
			listNpc = ms.wz.wzFile_ui["UIWindow2.img"]["MiniMap"]["ListNpc"];

			buttons[(int)Buttons.BT_MIN] = new MapleButton (MiniMap["BtMin"], new Point_short (195, -6));
			buttons[(int)Buttons.BT_MAX] = new MapleButton (MiniMap["BtMax"], new Point_short (209, -6));
			buttons[(int)Buttons.BT_SMALL] = new MapleButton (MiniMap["BtSmall"], new Point_short (223, -6));
			buttons[(int)Buttons.BT_BIG] = new MapleButton (MiniMap["BtBig"], new Point_short (223, -6));
			buttons[(int)Buttons.BT_MAP] = new MapleButton (MiniMap["BtMap"], new Point_short (237, -6));
			buttons[(int)Buttons.BT_NPC] = new MapleButton (MiniMap["BtNpc"], new Point_short (276, -6));

			region_text = new Text (Text.Font.A12B, Text.Alignment.LEFT, Color.Name.WHITE);
			town_text = new Text (Text.Font.A12B, Text.Alignment.LEFT, Color.Name.WHITE);
			combined_text = new Text (Text.Font.A12M, Text.Alignment.LEFT, Color.Name.WHITE);

			marker = Setting<MiniMapDefaultHelpers>.get ().load () ? ms.wz.wzFile_ui["UIWindow2.img"]["MiniMapSimpleMode"]["DefaultHelper"] : ms.wz.wzFile_mapLatest["MapHelper.img"]["minimap"];
			//marker = true ? nl.nx.wzFile_ui["UIWindow2.img"]["MiniMapSimpleMode"]["DefaultHelper"] : nl.nx.wzFile_mapLatest["MapHelper.img"]["minimap"];

			player_marker = new Animation (marker["user"]);
			selected_marker = new Animation (MiniMap["iconNpc"]);
		}

		public override void draw (float alpha)
		{
			if (type == (int)Type.MIN)
			{
				foreach (var sprite in min_sprites)
				{
					sprite.draw (new Point_short (position), alpha);
				}

				combined_text.draw (position + new Point_short (7, -3));
			}
			else if (type == (int)Type.NORMAL)
			{
				foreach (var sprite in normal_sprites)
				{
					sprite.draw (new Point_short (position), alpha);
				}

				if (has_map)
				{
					Animation portal_marker = new Animation (marker["portal"]);

					foreach (var sprite in static_marker_info)
					{
						portal_marker.draw (position + sprite.Item2, alpha);
					}

					draw_movable_markers (new Point_short (position), alpha);

					if (listNpc_enabled)
					{
						draw_npclist (new Point_short (normal_dimensions), alpha);
					}
				}
			}
			else
			{
				foreach (var sprite in max_sprites)
				{
					sprite.draw (new Point_short (position), alpha);
				}

				region_text.draw (position + new Point_short (48, 14));
				town_text.draw (position + new Point_short (48, 28));

				if (has_map)
				{
					Animation portal_marker = new Animation (marker["portal"]);

					foreach (var sprite in static_marker_info)
					{
						portal_marker.draw (position + sprite.Item2 + new Point_short (0, MAX_ADJ), alpha);
					}

					draw_movable_markers (position + new Point_short (0, MAX_ADJ), alpha);

					if (listNpc_enabled)
					{
						draw_npclist (new Point_short (max_dimensions), alpha);
					}
				}
			}

			base.draw (alpha);
		}

		public override void update ()
		{
			int mid = Stage.get ().get_mapid ();

			if (mid != mapid)
			{
				mapid = mid;
				Map = NxHelper.Map.get_map_node_name (mapid);

				WzObject town = Map["info"]["town"];
				WzObject miniMap = Map["miniMap"];

				if (miniMap == null)
				{
					has_map = false;
					type = (int)Type.MIN;
				}
				else
				{
					has_map = true;

					if (town != null && town)
					{
						type = (int)Type.MAX;
					}
					else
					{
						type = user_type;
					}
					
					scale = (short)Math.Pow (2, (int)miniMap["mag"]);
					center_offset = new Point_short (miniMap["centerX"], miniMap["centerY"]);
				}

				update_text ();
				update_buttons ();
				update_canvas ();
				update_static_markers ();
				toggle_buttons ();
				update_npclist ();
			}

			if (type == (int)Type.MIN)
			{
				foreach (var sprite in min_sprites)
				{
					sprite.update ();
				}
			}
			else if (type == (int)Type.NORMAL)
			{
				foreach (var sprite in normal_sprites)
				{
					sprite.update ();
				}
			}
			else
			{
				foreach (var sprite in max_sprites)
				{
					sprite.update ();
				}
			}

			if (listNpc_enabled)
			{
				foreach (Sprite sprite in listNpc_sprites)
				{
					sprite.update ();
				}
			}

			if (selected >= 0)
			{
				selected_marker.update ();
			}

			base.update ();
		}

		public override void remove_cursor ()
		{
			base.remove_cursor ();

			listNpc_slider.remove_cursor ();

			UI.get ().clear_tooltip (Tooltip.Parent.MINIMAP);
		}

		public override Cursor.State send_cursor (bool clicked, Point_short cursorpos)
		{
			Cursor.State dstate = base.send_cursor (clicked, new Point_short (cursorpos));

			if (dragged)
			{
				return dstate;
			}

			Point_short cursor_relative = cursorpos - position;

			if (listNpc_slider.isenabled ())
			{
				Cursor.State new_state = listNpc_slider.send_cursor (new Point_short (cursor_relative), clicked);
				if (new_state != Cursor.State.IDLE)
				{
					return new_state;
				}
			}

			if (listNpc_enabled)
			{
				Point_short relative_point = cursor_relative - new Point_short ((short)(10 + (type == (int)((int)Type.MAX) ? max_dimensions : normal_dimensions).x ()), 23);
				Rectangle_short list_bounds = new Rectangle_short (0, LISTNPC_ITEM_WIDTH, 0, LISTNPC_ITEM_HEIGHT * 8);

				if (list_bounds.contains (relative_point))
				{
					short list_index = (short)(listNpc_offset + relative_point.y () / LISTNPC_ITEM_HEIGHT);
					bool in_list = list_index < listNpc_names.Count;

					if (clicked)
					{
						select_npclist ((short)(in_list ? list_index : -1));
					}
					else if (in_list)
					{
						UI.get ().show_text (Tooltip.Parent.MINIMAP, listNpc_full_names[list_index]);
					}

					return Cursor.State.IDLE;
				}
			}

			bool found = false;
			var npcs = Stage.get ().get_npcs ().get_npcs ();
			foreach (var npc in npcs)
			{
				Point_short npc_pos = (npc.Value.get_position () + center_offset) / scale + new Point_short (map_draw_origin_x, map_draw_origin_y);
				Rectangle_short marker_spot = new Rectangle_short (npc_pos - new Point_short (4, 8), new Point_short (npc_pos));

				if (type == (int)Type.MAX)
				{
					marker_spot.shift (new Point_short (0, MAX_ADJ));
				}

				if (marker_spot.contains (cursor_relative))
				{
					found = true;

					var n = (Npc)npc.Value;
					string name = n.get_name ();
					string func = n.get_func ();

					UI.get ().show_map (Tooltip.Parent.MINIMAP, name, func, 0, false);
					break;
				}
			}

			if (!found)
			{
				foreach (var sprite in static_marker_info)
				{
					Rectangle_short marker_spot = new Rectangle_short (sprite.Item2, sprite.Item2 + (short)8);

					if (type == (int)Type.MAX)
					{
						marker_spot.shift (new Point_short (0, MAX_ADJ));
					}

					if (marker_spot.contains (cursor_relative))
					{
						WzObject portal_tm = Map["portal"][sprite.Item1]["tm"];
						string portal_cat = NxHelper.Map.get_map_category (portal_tm);
						WzObject portal_name = ms.wz.wzFile_string["Map.img"][portal_cat][portal_tm.ToString ()]["mapName"];

						if (portal_name != null)
						{
							found = true;

							UI.get ().show_map (Tooltip.Parent.MINIMAP, portal_name.ToString (), "", portal_tm, false);
							break;
						}
					}
				}
			}

			return Cursor.State.IDLE;
		}

		public override void send_scroll (double yoffset)
		{
			if (listNpc_enabled && listNpc_slider.isenabled ())
			{
				listNpc_slider.send_scroll (yoffset);
			}
		}

		public override void send_key (int keycode, bool pressed, bool escape)
		{
			if (has_map)
			{
				if (type < (int)Type.MAX)
				{
					type++;
				}
				else
				{
					type = (int)Type.MIN;
				}

				user_type = type;

				toggle_buttons ();
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
				case Buttons.BT_MIN:
					type -= 1;
					toggle_buttons ();
					return type == (int)((int)Type.MIN) ? Button.State.DISABLED : Button.State.NORMAL;
				case Buttons.BT_MAX:
					type += 1;
					toggle_buttons ();
					return type == (int)((int)Type.MAX) ? Button.State.DISABLED : Button.State.NORMAL;
				case Buttons.BT_SMALL:
				case Buttons.BT_BIG:
					big_map = !big_map;
					// TODO: Toggle scrolling map
					toggle_buttons ();
					break;
				case Buttons.BT_MAP:
					UI.get ().emplace<UIWorldMap> ();
					break;
				case Buttons.BT_NPC:
					set_npclist_active (!listNpc_enabled);
					break;
			}

			return Button.State.NORMAL;
		}

		private const short CENTER_START_X = 64;
		private const short BTN_MIN_Y = 4;
		private const short ML_MR_Y = 17;
		private const short MAX_ADJ = 40;
		private const short M_START = 36;
		private const short LISTNPC_ITEM_HEIGHT = 17;
		private const short LISTNPC_ITEM_WIDTH = 140;
		private const short LISTNPC_TEXT_WIDTH = 114;
		private static readonly Point_short WINDOW_UL_POS = Point_short.zero;

		private void update_buttons ()
		{
			// Add one pixel for a space to the right of each button
			bt_min_width = (short)(buttons[(int)Buttons.BT_MIN].width () + 1);
			bt_max_width = (short)(buttons[(int)Buttons.BT_MAX].width () + 1);
			bt_map_width = (short)(buttons[(int)Buttons.BT_MAP].width () + 1);

			combined_text_width = combined_text.width ();
		}

		private void toggle_buttons ()
		{
			short bt_min_x;

			if (type == (int)Type.MIN)
			{
				buttons[(int)Buttons.BT_MAP].set_active (true);
				buttons[(int)Buttons.BT_MAX].set_active (true);
				buttons[(int)Buttons.BT_MIN].set_active (true);
				buttons[(int)Buttons.BT_NPC].set_active (false);
				buttons[(int)Buttons.BT_SMALL].set_active (false);
				buttons[(int)Buttons.BT_BIG].set_active (false);

				buttons[(int)Buttons.BT_MIN].set_state (Button.State.DISABLED);

				if (has_map)
				{
					buttons[(int)Buttons.BT_MAX].set_state (Button.State.NORMAL);
				}
				else
				{
					buttons[(int)Buttons.BT_MAX].set_state (Button.State.DISABLED);
				}

				bt_min_x = (short)(combined_text_width + 11);

				buttons[(int)Buttons.BT_MIN].set_position (new Point_short (bt_min_x, BTN_MIN_Y));

				bt_min_x += bt_min_width;

				buttons[(int)Buttons.BT_MAX].set_position (new Point_short (bt_min_x, BTN_MIN_Y));

				bt_min_x += bt_max_width;

				buttons[(int)Buttons.BT_MAP].set_position (new Point_short (bt_min_x, BTN_MIN_Y));

				min_dimensions = new Point_short ((short)(bt_min_x + bt_map_width + 7), 20);

				update_dimensions ();

				dragarea = new Point_short (dimension);

				set_npclist_active (false);
			}
			else
			{
				bool has_npcs = Stage.get ().get_npcs ().get_npcs ().size () > 0;

				buttons[(int)Buttons.BT_MAP].set_active (true);
				buttons[(int)Buttons.BT_MAX].set_active (true);
				buttons[(int)Buttons.BT_MIN].set_active (true);
				buttons[(int)Buttons.BT_NPC].set_active (has_npcs);

				if (big_map)
				{
					buttons[(int)Buttons.BT_BIG].set_active (false);
					buttons[(int)Buttons.BT_SMALL].set_active (true);
				}
				else
				{
					buttons[(int)Buttons.BT_BIG].set_active (true);
					buttons[(int)Buttons.BT_SMALL].set_active (false);
				}

				buttons[(int)Buttons.BT_MIN].set_state (Button.State.NORMAL);

				bt_min_x = (short)(middle_right_x - (bt_min_width + buttons[(int)Buttons.BT_SMALL].width () + 1 + bt_max_width + bt_map_width + (has_npcs ? buttons[(int)Buttons.BT_NPC].width () : 0)));

				buttons[(int)Buttons.BT_MIN].set_position (new Point_short (bt_min_x, BTN_MIN_Y));

				bt_min_x += bt_max_width;

				buttons[(int)Buttons.BT_MAX].set_position (new Point_short (bt_min_x, BTN_MIN_Y));

				bt_min_x += bt_max_width;

				buttons[(int)Buttons.BT_SMALL].set_position (new Point_short (bt_min_x, BTN_MIN_Y));
				buttons[(int)Buttons.BT_BIG].set_position (new Point_short (bt_min_x, BTN_MIN_Y));

				bt_min_x += bt_max_width;

				buttons[(int)Buttons.BT_MAP].set_position (new Point_short (bt_min_x, BTN_MIN_Y));

				bt_min_x += bt_map_width;

				buttons[(int)Buttons.BT_NPC].set_position (new Point_short (bt_min_x, BTN_MIN_Y));

				if (type == (int)Type.MAX)
				{
					buttons[(int)Buttons.BT_MAX].set_state (Button.State.DISABLED);
				}
				else
				{
					buttons[(int)Buttons.BT_MAX].set_state (Button.State.NORMAL);
				}

				set_npclist_active (listNpc_enabled && has_npcs);

				dragarea = new Point_short (dimension.x (), 20);
			}
		}

		private void update_text ()
		{
			NxHelper.Map.MapInfo map_info = NxHelper.Map.get_map_info_by_id (mapid);
			combined_text.change_text (map_info.full_name);
			region_text.change_text (map_info.name);
			town_text.change_text (map_info.street_name);
		}

		private void update_canvas ()
		{
			min_sprites.Clear ();
			normal_sprites.Clear ();
			max_sprites.Clear ();

			WzObject Min;
			WzObject Normal;
			WzObject Max;

			if (simpleMode)
			{
				Min = (MiniMap["Window"]["Min"]);
				Normal = (MiniMap["Window"]["Normal"]);
				Max = (MiniMap["Window"]["Max"]);
			}
			else
			{
				Min = (MiniMap["Min"]);
				Normal = (MiniMap["MinMap"]);
				Max = (MiniMap["MaxMap"]);
			}

			map_sprite = new Texture (Map["miniMap"]?["canvas"]);
			Point_short map_dimensions = map_sprite.get_dimensions ();

			// 48 (offset for text) + longer text's Width + 10 (space for right side border)
			short mark_text_width = (short)(48 + Math.Max (region_text.width (), town_text.width ()) + 10);
			short c_stretch;
			short ur_x_offset;
			short m_stretch;
			short down_y_offset;
			short window_width = (short)Math.Max (178, Math.Max ((int)mark_text_width, map_dimensions.x () + 20));

			c_stretch = (short)Math.Max (0, window_width - 128);
			ur_x_offset = (short)(CENTER_START_X + c_stretch);
			map_draw_origin_x = (short)Math.Max (10, window_width / 2 - map_dimensions.x () / 2);

			if (map_dimensions.y () <= 20)
			{
				m_stretch = 5;
				down_y_offset = (short)(17 + m_stretch);
				map_draw_origin_y = (short)(10 + m_stretch - map_dimensions.y ());
			}
			else
			{
				m_stretch = (short)(map_dimensions.y () - 17);
				down_y_offset = (short)(17 + m_stretch);
				map_draw_origin_y = 20;
			}

			middle_right_x = (short)(ur_x_offset + 55);

			string Left = simpleMode ? "Left" : "w";
			string Center = simpleMode ? "Center" : "c";
			string Right = simpleMode ? "Right" : "e";

			string DownCenter = simpleMode ? "DownCenter" : "s";
			string DownLeft = simpleMode ? "DownLeft" : "sw";
			string DownRight = simpleMode ? "DownRight" : "se";
			string MiddleLeft = simpleMode ? "MiddleLeft" : "w";
			string MiddleRight = simpleMode ? "MiddleRight" : "e";
			string UpCenter = simpleMode ? "UpCenter" : "n";
			string UpLeft = simpleMode ? "UpLeft" : "nw";
			string UpRight = simpleMode ? "UpRight" : "ne";

			// SimpleMode's backdrop is opaque, the other is transparent but lightly colored
			// UI.wz v208 has normal center sprite in-linked to bottom right window frame, not sure why.
			WzObject MiddleCenter = simpleMode ? MiniMap["Window"]["Max"]["MiddleCenter"] : MiniMap["MaxMap"]["c"];

			short dl_dr_y = Math.Max (map_dimensions.y (), (short)10);

			// combined_text_width + 14 (7px buffer on both sides) + 4 (buffer between name and buttons) + 3 buttons' widths - 128 (length of left and right window borders)
			short min_c_stretch = (short)(combined_text_width + 18 + bt_min_width + bt_max_width + bt_map_width - 128);

			// Min sprites queue
			min_sprites.Add (new Sprite (Min[Center], new DrawArgument (WINDOW_UL_POS + new Point_short (CENTER_START_X, 0), new Point_short (min_c_stretch, 0))));
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: min_sprites.Add(Min[Left], DrawArgument(WINDOW_UL_POS));
			min_sprites.Add (new Sprite (Min[Left], new DrawArgument (new Point_short (WINDOW_UL_POS))));
			min_sprites.Add (new Sprite (Min[Right], new DrawArgument (WINDOW_UL_POS + new Point_short ((short)(min_c_stretch + CENTER_START_X), 0))));

			// Normal sprites queue
			// (7, 10) is the top left corner of the inner window
			// 114 = 128 (Width of left and right borders) - 14 (Width of middle borders * 2).
			// 27 = height of inner frame drawn on up and down borders
			normal_sprites.Add (new Sprite (MiddleCenter, new DrawArgument (new Point_short (7, 10), new Point_short ((short)(c_stretch + 114), (short)(m_stretch + 27)))));

			if (has_map)
			{
				normal_sprites.Add (new Sprite (Map["miniMap"]["canvas"], new DrawArgument (new Point_short (map_draw_origin_x, map_draw_origin_y))));
			}

			normal_sprites.Add (new Sprite (Normal[MiddleLeft], new DrawArgument (new Point_short (0, ML_MR_Y), new Point_short (0, m_stretch))));
			normal_sprites.Add (new Sprite (Normal[MiddleRight], new DrawArgument (new Point_short (middle_right_x, ML_MR_Y), new Point_short (0, m_stretch))));
			normal_sprites.Add (new Sprite (Normal[UpCenter], new DrawArgument (new Point_short (CENTER_START_X, 0) + WINDOW_UL_POS, new Point_short (c_stretch, 0))));
			normal_sprites.Add (new Sprite (Normal[UpLeft], WINDOW_UL_POS));
			normal_sprites.Add (new Sprite (Normal[UpRight], new DrawArgument (new Point_short (ur_x_offset, 0) + WINDOW_UL_POS)));
			normal_sprites.Add (new Sprite (Normal[DownCenter], new DrawArgument (new Point_short (CENTER_START_X, (short)(down_y_offset + 18)), new Point_short (c_stretch, 0))));
			normal_sprites.Add (new Sprite (Normal[DownLeft], new Point_short (0, down_y_offset)));
			normal_sprites.Add (new Sprite (Normal[DownRight], new Point_short (ur_x_offset, down_y_offset)));

			normal_dimensions = new Point_short ((short)(ur_x_offset + 64), (short)(down_y_offset + 27));

			// Max sprites queue
			max_sprites.Add (new Sprite (MiddleCenter, new DrawArgument (new Point_short (7, 50), new Point_short ((short)(c_stretch + 114), (short)(m_stretch + 27)))));

			if (has_map)
			{
				max_sprites.Add (new Sprite (Map["miniMap"]["canvas"], new DrawArgument (new Point_short (map_draw_origin_x, (short)(map_draw_origin_y + MAX_ADJ)))));
			}

			max_sprites.Add (new Sprite (Max[MiddleLeft], new DrawArgument (new Point_short (0, ML_MR_Y + MAX_ADJ), new Point_short (0, m_stretch))));
			max_sprites.Add (new Sprite (Max[MiddleRight], new DrawArgument (new Point_short (middle_right_x, ML_MR_Y + MAX_ADJ), new Point_short (0, m_stretch))));
			max_sprites.Add (new Sprite (Max[UpCenter], new DrawArgument (new Point_short (CENTER_START_X, 0) + WINDOW_UL_POS, new Point_short (c_stretch, 0))));
			max_sprites.Add (new Sprite (Max[UpLeft], WINDOW_UL_POS));
			max_sprites.Add (new Sprite (Max[UpRight], new DrawArgument (new Point_short (ur_x_offset, 0) + WINDOW_UL_POS)));
			max_sprites.Add (new Sprite (Max[DownCenter], new DrawArgument (new Point_short (CENTER_START_X, (short)(down_y_offset + MAX_ADJ + 18)), new Point_short (c_stretch, 0))));
			max_sprites.Add (new Sprite (Max[DownLeft], new Point_short (0, (short)(down_y_offset + MAX_ADJ))));
			max_sprites.Add (new Sprite (Max[DownRight], new Point_short (ur_x_offset, (short)(down_y_offset + MAX_ADJ))));
			max_sprites.Add (new Sprite (ms.wz.wzFile_mapLatest["MapHelper.img"]["mark"][Map["info"]["mapMark"].ToString ()], new DrawArgument (new Point_short (7, 17))));

			max_dimensions = normal_dimensions + new Point_short (0, MAX_ADJ);
		}

		private void draw_movable_markers (Point_short init_pos, float alpha)
		{
			if (!has_map)
			{
				return;
			}

			Animation marker_sprite = new Animation ();
			Point_short sprite_offset = new Point_short ();

			/// NPCs
			MapObjects npcs = Stage.get ().get_npcs ().get_npcs ();
			marker_sprite = new Animation (marker["npc"]);
			sprite_offset = marker_sprite.get_dimensions () / new Point_short (2, 0);

			foreach (var npc in npcs)
			{
				Point_short npc_pos = npc.Value.get_position ();
				marker_sprite.draw ((npc_pos + center_offset) / scale - sprite_offset + new Point_short (map_draw_origin_x, map_draw_origin_y) + init_pos, alpha);
			}

			/// Other characters
			MapObjects chars = Stage.get ().get_chars ().get_chars ();
			marker_sprite = new Animation (marker["another"]);
			sprite_offset = marker_sprite.get_dimensions () / new Point_short (2, 0);

			foreach (var chr in chars)
			{
				Point_short chr_pos = chr.Value.get_position ();
				marker_sprite.draw ((chr_pos + center_offset) / scale - sprite_offset + new Point_short (map_draw_origin_x, map_draw_origin_y) + init_pos, alpha);
			}

			/// Player
			Point_short player_pos = Stage.get ().get_player ().get_position ();
			sprite_offset = player_marker.get_dimensions () / new Point_short (2, 0);
			player_marker.draw ((player_pos + center_offset) / scale - sprite_offset + new Point_short (map_draw_origin_x, map_draw_origin_y) + init_pos, alpha);
		}

		private void update_static_markers ()
		{
			static_marker_info.Clear ();

			if (!has_map)
			{
				return;
			}

			Animation marker_sprite = new Animation ();

			/// Portals
			WzObject portals = Map["portal"];
			marker_sprite = new Animation (marker["portal"]);
			Point_short marker_offset = marker_sprite.get_dimensions () / new Point_short (2, 0);
			foreach (var portal in portals)
			{
				int portal_type = portal["pt"];

				if (portal_type == 2)
				{
					Point_short marker_pos = (new Point_short (portal["x"], portal["y"]) + center_offset) / scale - marker_offset + new Point_short (map_draw_origin_x, map_draw_origin_y);
					static_marker_info.Add (new Tuple<string, Point_short> (portal.Name, marker_pos));
				}
			}
		}

		private void set_npclist_active (bool active)
		{
			listNpc_enabled = active;

			if (!active)
			{
				select_npclist (-1);
			}

			update_dimensions ();
		}

		private void update_dimensions ()
		{
			if (type == (int)Type.MIN)
			{
				dimension = new Point_short (min_dimensions);
			}
			else
			{
				Point_short base_dims = type == (int)((int)Type.MAX) ? max_dimensions : normal_dimensions;
				dimension = new Point_short (base_dims);

				if (listNpc_enabled)
				{
					dimension += listNpc_dimensions;
					dimension.set_y (Math.Max (base_dims.y (), listNpc_dimensions.y ()));
				}
			}
		}

		private void update_npclist ()
		{
			listNpc_sprites.Clear ();
			listNpc_names.Clear ();
			listNpc_full_names.Clear ();
			listNpc_list.Clear ();
			selected = -1;
			listNpc_offset = 0;

			if (simpleMode)
			{
				return;
			}

			var npcs = Stage.get ().get_npcs ().get_npcs ();
			foreach (var npc in npcs)
			{
				listNpc_list.Add (npc.Value);

				var n = (Npc)npc.Value;
				string name = n.get_name ();
				string func = n.get_func ();

				if (func != "")
				{
					name += " (" + func + ")";
				}

				Text name_text = new Text (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.WHITE, name);

				listNpc_names.Add (name_text);
				listNpc_full_names.Add (name);
			}
			

			for (int i = 0; i < listNpc_names.Count; i++)
			{
				string_format.format_with_ellipsis (listNpc_names[i], LISTNPC_TEXT_WIDTH - (listNpc_names.Count > 8 ? 0 : 20));
			}

			Point_short listNpc_pos = new Point_short (type == (int)((int)Type.MAX) ? max_dimensions.x () : normal_dimensions.x (), 0);
			short c_stretch = 20;
			short m_stretch = 102;

			if (listNpc_names.Count > 8)
			{
				listNpc_slider = new Slider ((int)Slider.Type.DEFAULT_SILVER, new Range_short (23, 11 + LISTNPC_ITEM_HEIGHT * 8), (short)(listNpc_pos.x () + LISTNPC_ITEM_WIDTH + 1), 8, (short)listNpc_names.Count, (bool upwards) =>
				{
					short shift = (short)(upwards ? -1 : 1);
					bool above = listNpc_offset + shift >= 0;
					bool below = listNpc_offset + 8 + shift <= listNpc_names.Count;

					if (above && below)
					{
						listNpc_offset += shift;
					}
				});

				c_stretch += 12;
			}
			else
			{
				listNpc_slider.setenabled (false);
				m_stretch = (short)(LISTNPC_ITEM_HEIGHT * listNpc_names.Count - 34);
				c_stretch -= 17;
			}

			listNpc_sprites.Add (new Sprite (listNpc["c"], new DrawArgument (listNpc_pos + new Point_short (CENTER_START_X, M_START), new Point_short (c_stretch, m_stretch))));
			listNpc_sprites.Add (new Sprite (listNpc["w"], new DrawArgument (listNpc_pos + new Point_short (0, M_START), new Point_short (0, m_stretch))));
			listNpc_sprites.Add (new Sprite (listNpc["e"], new DrawArgument (listNpc_pos + new Point_short ((short)(CENTER_START_X + c_stretch), M_START), new Point_short (0, m_stretch))));
			listNpc_sprites.Add (new Sprite (listNpc["n"], new DrawArgument (listNpc_pos + new Point_short (CENTER_START_X, 0), new Point_short (c_stretch, 0))));
			listNpc_sprites.Add (new Sprite (listNpc["s"], new DrawArgument (listNpc_pos + new Point_short (CENTER_START_X, (short)(M_START + m_stretch)), new Point_short (c_stretch, 0))));
			listNpc_sprites.Add (new Sprite (listNpc["nw"], new DrawArgument (listNpc_pos + new Point_short (0, 0))));
			listNpc_sprites.Add (new Sprite (listNpc["ne"], new DrawArgument (listNpc_pos + new Point_short ((short)(CENTER_START_X + c_stretch), 0))));
			listNpc_sprites.Add (new Sprite (listNpc["sw"], new DrawArgument (listNpc_pos + new Point_short (0, (short)(M_START + m_stretch)))));
			listNpc_sprites.Add (new Sprite (listNpc["se"], new DrawArgument (listNpc_pos + new Point_short ((short)(CENTER_START_X + c_stretch), (short)(M_START + m_stretch)))));

			listNpc_dimensions = new Point_short ((short)(CENTER_START_X * 2 + c_stretch), (short)(M_START + m_stretch + 30));

			update_dimensions ();
		}

		private void draw_npclist (Point_short minimap_dims, float alpha)
		{
			Animation npc_marker = new Animation (marker["npc"]);

			foreach (Sprite sprite in listNpc_sprites)
			{
				sprite.draw (new Point_short (position), alpha);
			}

			Point_short listNpc_pos = position + new Point_short ((short)(minimap_dims.x () + 10), 23);

			for (sbyte i = 0; i + listNpc_offset < listNpc_list.Count && i < 8; i++)
			{
				if (selected - listNpc_offset == i)
				{
					ColorBox highlight = new ColorBox ((short)(LISTNPC_ITEM_WIDTH - (listNpc_slider.isenabled () ? 0 : 30)), LISTNPC_ITEM_HEIGHT, Color.Name.YELLOW, 1.0f);
					highlight.draw (listNpc_pos);
				}

				npc_marker.draw (new DrawArgument (listNpc_pos + new Point_short (0, 2), false, npc_marker.get_dimensions () / 2), alpha);
				listNpc_names[listNpc_offset + i].draw (new DrawArgument (listNpc_pos + new Point_short (14, -2)));

				listNpc_pos.shift_y (LISTNPC_ITEM_HEIGHT);
			}

			if (listNpc_slider.isenabled ())
			{
				listNpc_slider.draw (new Point_short (position));
			}

			if (selected >= 0)
			{
				Point_short npc_pos = (listNpc_list[selected].get_position () + center_offset) / scale + new Point_short (map_draw_origin_x, (short)(map_draw_origin_y - npc_marker.get_dimensions ().y () + (type == (int)((int)Type.MAX) ? MAX_ADJ : 0)));

				selected_marker.draw (position + npc_pos, 0.5f);
			}
		}

		private void select_npclist (short choice)
		{
			if (selected == choice)
			{
				return;
			}

			if (selected >= 0 && selected < listNpc_names.Count)
			{
				listNpc_names[selected].change_color (Color.Name.WHITE);
			}

			if (choice > listNpc_names.Count || choice < 0)
			{
				selected = -1;
			}
			else
			{
				selected = (short)(choice != selected ? choice : -1);

				if (selected >= 0)
				{
					listNpc_names[selected].change_color (Color.Name.BLACK);
				}
			}
		}

		private enum Buttons
		{
			BT_MIN,
			BT_MAX,
			BT_SMALL,
			BT_BIG,
			BT_MAP,
			BT_NPC
		}

		private enum Type
		{
			MIN,
			NORMAL,
			MAX
		}

		/// Constants
		private int mapid;

		private sbyte type;
		private sbyte user_type;
		private bool simpleMode;
		private bool big_map;
		private bool has_map;
		private short scale = 1;
		private WzObject Map;
		private WzObject MiniMap;
		private WzObject marker;
		private Texture map_sprite = new Texture ();
		private Animation player_marker = new Animation ();
		private short combined_text_width;
		private short middle_right_x;
		private short bt_min_width;
		private short bt_max_width;
		private short bt_map_width;
		private List<Sprite> min_sprites = new List<Sprite> ();
		private List<Sprite> normal_sprites = new List<Sprite> ();
		private List<Sprite> max_sprites = new List<Sprite> ();
		private List<System.Tuple<string, Point_short>> static_marker_info = new List<System.Tuple<string, Point_short>> ();
		private short map_draw_origin_x;
		private short map_draw_origin_y;
		private Point_short center_offset = new Point_short ();
		private Point_short min_dimensions = new Point_short ();
		private Point_short normal_dimensions = new Point_short ();
		private Point_short max_dimensions = new Point_short ();
		private Text combined_text = new Text ();
		private Text region_text = new Text ();
		private Text town_text = new Text ();

		private bool listNpc_enabled;
		private WzObject listNpc;
		private List<Sprite> listNpc_sprites = new List<Sprite> ();
		private List<MapObject> listNpc_list = new List<MapObject> ();
		private List<Text> listNpc_names = new List<Text> ();
		private List<string> listNpc_full_names = new List<string> ();

		private Point_short listNpc_dimensions = new Point_short ();

		private Slider listNpc_slider = new Slider ();
		private short listNpc_offset;
		private short selected;
		private Animation selected_marker = new Animation ();

		private readonly CharStats stats;
	}
}


#if USE_NX
#endif