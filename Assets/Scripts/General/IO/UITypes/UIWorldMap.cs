#define USE_NX

using System;
using System.Collections.Generic;
using MapleLib.WzLib;




namespace ms
{
	[Beebyte.Obfuscator.Skip]
	public class UIWorldMap : UIDragElement<PosMAP>
	{
		public const Type TYPE = UIElement.Type.WORLDMAP;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public UIWorldMap (params object[] args) : this ()
		{
		}
		
		public UIWorldMap ()
		{
			//this.UIDragElement<PosMAP> = new <type missing>();
			//WzObject close = ms.wz.wzFile_ui["Basic.img"]["BtClose3"];
			WzObject close = ms.wz.wzFile_ui["UIWindow2.img"]?["Scenario"]?["BtClose"];
			WzObject WorldMap = ms.wz.wzFile_ui["UIWindow2.img"]["WorldMap"];
			WzObject WorldMapSearch = WorldMap["WorldMapSearch"];
			WzObject Border = WorldMap["Border"]["0"];
			WzObject backgrnd = WorldMapSearch["backgrnd"];
			WzObject MapHelper = ms.wz.wzFile_map["MapHelper.img"]["worldMap"];

			cur_pos = MapHelper["curPos"];

			for (uint i = 0; i < MAPSPOT_TYPE_MAX; i++)
			{
				npc_pos[i] = MapHelper["npcPos" + Convert.ToString (i)];
			}

			sprites.Add (Border);

			search_background = backgrnd;
			search_notice = WorldMapSearch["notice"];

			bg_dimensions = new Texture (Border).get_dimensions ();
			bg_search_dimensions = new Point_short (search_background.get_dimensions ());

			short bg_dimension_x = bg_dimensions.x ();
			background_dimensions = new Point_short (bg_dimension_x, 0);

			short base_x = (short)(bg_dimension_x / 2);
			short base_y = (short)(bg_dimensions.y () / 2);
			base_position = new Point_short (base_x, (short)(base_y + 15));

			Point_short close_dimensions = new Point_short ((short)(bg_dimension_x - 22), 4);

			buttons[(int)Buttons.BT_CLOSE] = new MapleButton (close, close_dimensions);
			buttons[(int)Buttons.BT_SEARCH] = new MapleButton (WorldMap["BtSearch"]);
			buttons[(int)Buttons.BT_varFLY] = new MapleButton (WorldMap["BtvarFly_1"]);
			buttons[(int)Buttons.BT_NAVIREG] = new MapleButton (WorldMap["BtNaviRegister"]);
			buttons[(int)Buttons.BT_SEARCH_CLOSE] = new MapleButton (close, close_dimensions + new Point_short (bg_search_dimensions.x (), 0));
			buttons[(int)Buttons.BT_ALLSEARCH] = new MapleButton (WorldMapSearch["BtAllsearch"], background_dimensions);

			Point_short search_text_pos = new Point_short ((short)(bg_dimension_x + 14), 25);
			Point_short search_box_dim = new Point_short (83, 15);
			Rectangle_short search_text_dim = new Rectangle_short (new Point_short (search_text_pos), search_text_pos + search_box_dim);

			search_text = new Textfield (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.BLACK, new Rectangle_short (search_text_dim), 8);

			set_search (true);

			dragarea = new Point_short (bg_dimension_x, 20);
		}

		public override void draw (float alpha)
		{
			base.draw_sprites (alpha);

			if (search)
			{
				search_background.draw (position + background_dimensions);
				search_notice.draw (position + background_dimensions);
				search_text.draw (position + new Point_short (1, -5));
			}

			base_img.draw (position + base_position);

			if (link_images.Count > 0)
			{
				foreach (var iter in buttons)
				{
					var button = iter.Value;
					if (button != null)
					{
						if (iter.Key >= ((int)Buttons.BT_LINK0) && button.get_state () == Button.State.MOUSEOVER)
						{
							if (link_images.ContainsKey ((ushort)iter.Key))
							{
								link_images[(ushort)iter.Key].draw (position + base_position);
								break;
							}
						}
					}
				}
			}

			if (show_path_img)
			{
				path_img.draw (position + base_position);
			}

			foreach (var spot in map_spots)
			{
				spot.Item2.marker.draw (spot.Item1 + position + base_position);
			}

			bool found = false;

			if (!found)
			{
				foreach (var spot in map_spots)
				{
					foreach (var map_id in spot.Item2.map_ids)
					{
						if (map_id == mapid)
						{
							found = true;
							npc_pos[spot.Item2.type].draw (spot.Item1 + position + base_position, alpha);
							cur_pos.draw (spot.Item1 + position + base_position, alpha);
							break;
						}
					}

					if (found)
					{
						break;
					}
				}
			}

			base.draw_buttons (alpha);
		}

		public override void update ()
		{
			int mid = Stage.get ().get_mapid ();

			if (mid != mapid)
			{
				mapid = mid;
				var prefix = mapid / 10000000;
				var parent_map = "WorldMap0" + Convert.ToString (prefix);
				user_map = parent_map;

				update_world (parent_map);
			}

			if (search)
			{
				search_text.update (new Point_short (position));
			}

			for (uint i = 0; i < MAPSPOT_TYPE_MAX; i++)
			{
				npc_pos[i].update (1);
			}

			cur_pos.update ();

			base.update ();
		}

		public override void toggle_active ()
		{
			base.toggle_active ();

			if (!active)
			{
				set_search (true);
				update_world (user_map);
			}
		}

		public override void remove_cursor ()
		{
			base.remove_cursor ();

			UI.get ().clear_tooltip (Tooltip.Parent.WORLDMAP);

			show_path_img = false;
		}

		public override Cursor.State send_cursor (bool clicked, Point_short cursorpos)
		{
			Cursor.State new_state = search_text.send_cursor (new Point_short (cursorpos), clicked);
			if (new_state != Cursor.State.IDLE)
			{
				return new_state;
			}

			show_path_img = false;

			foreach (var path in map_spots)
			{
				Point_short p = path.Item1 + position + base_position - (short)10;
				Point_short d = p + path.Item2.marker.get_dimensions ();
				Rectangle_short abs_bounds = new Rectangle_short (new Point_short (p), new Point_short (d));

				if (abs_bounds.contains (cursorpos))
				{
					path_img = path.Item2.path;
					show_path_img = path_img.is_valid ();

					UI.get ().show_map (Tooltip.Parent.WORLDMAP, path.Item2.title, path.Item2.description, path.Item2.map_ids[0], path.Item2.bolded);
					break;
				}
			}

			return base.send_cursor (clicked, new Point_short (cursorpos));
		}

		public override void send_key (int keycode, bool pressed, bool escape)
		{
			if (pressed && escape)
			{
				if (search)
				{
					set_search (false);
				}
				else
				{
					if (parent_map == "")
					{
						toggle_active ();

						update_world (user_map);
					}
					else
					{
						new Sound (Sound.Name.SELECTMAP).play ();

						update_world (parent_map);
					}
				}
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public override Button.State button_pressed (ushort buttonid)
		{
			switch ((Buttons)buttonid)
			{
				case Buttons.BT_CLOSE:
					deactivate ();
					break;
				case Buttons.BT_SEARCH:
					set_search (!search);
					break;
				case Buttons.BT_SEARCH_CLOSE:
					set_search (false);
					break;
				default:
					break;
			}

			if (buttonid >= (int)Buttons.BT_LINK0)
			{
				update_world (link_maps[buttonid]);

				return Button.State.IDENTITY;
			}

			return Button.State.NORMAL;
		}

		private const byte MAPSPOT_TYPE_MAX = (byte)4u;

		private void set_search (bool enable)
		{
			search = enable;

			buttons[(int)Buttons.BT_SEARCH_CLOSE].set_active (enable);
			buttons[(int)Buttons.BT_ALLSEARCH].set_active (enable);

			if (enable)
			{
				search_text.set_state (Textfield.State.NORMAL);
				dimension = bg_dimensions + new Point_short (bg_search_dimensions.x (), 0);
			}
			else
			{
				search_text.set_state (Textfield.State.DISABLED);
				dimension = new Point_short (bg_dimensions);
			}
		}

		private void update_world (string map)
		{
			WzObject WorldMap = ms.wz.wzFile_map["WorldMap"][$"{map}.img"];

			if (WorldMap == null)
			{
				WorldMap = ms.wz.wzFile_map["WorldMap"]["WorldMap.img"];
			}

			base_img = WorldMap["BaseImg"][0.ToString ()];
			parent_map = WorldMap["info"]?["parentMap"]?.ToString ();

			link_images.Clear ();
			link_maps.Clear ();

			foreach (var iter in buttons)
			{
				var button = iter.Value;
				if (button != null)
				{
					if (iter.Key >= (ulong)Buttons.BT_LINK0)
					{
						button.set_active (false);
					}
				}
			}

			ushort i = (ushort)Buttons.BT_LINK0;

			if (WorldMap["MapLink"] != null)
			{
				foreach (var link in WorldMap["MapLink"])
				{
					WzObject l = link["link"];
					Texture link_image = l["linkImg"];

					link_images[(ushort)i] = link_image;
					link_maps[i] = l["linkMap"].ToString ();

					buttons[i] = new AreaButton (base_position - link_image.get_origin (), link_image.get_dimensions ());
					buttons[i].set_active (true);

					i++;
				}
			}

			WzObject mapImage = ms.wz.wzFile_map["MapHelper.img"]["worldMap"]["mapImage"];

			map_spots.Clear ();

			foreach (var list in WorldMap["MapList"])
			{
				WzObject desc = list["desc"];
				WzObject mapNo = list["mapNo"];
				WzObject path = list["path"];
				WzObject spot = list["spot"];
				WzObject title = list["title"];
				WzObject type = list["type"];
				WzObject marker = mapImage[type.ToString ()];

				List<int> map_ids = new List<int> ();

				foreach (var map_no in mapNo)
				{
					map_ids.Add (map_no);
				}

				if (desc == null && title == null)
				{
					NxHelper.Map.MapInfo map_info = NxHelper.Map.get_map_info_by_id (mapNo[0.ToString ()]);

					map_spots.Add (new Tuple<Point_short, MapSpot> (spot, new MapSpot (map_info.description, path, map_info.full_name, type, marker, true, map_ids)));
				}
				else
				{
					map_spots.Add (new Tuple<Point_short, MapSpot> (spot, new MapSpot (desc?.ToString (), path, title?.ToString (), type, marker, false, map_ids)));
				}
			}
		}

		private enum Buttons
		{
			BT_CLOSE,
			BT_SEARCH,
			BT_varFLY,
			BT_NAVIREG,
			BT_ALLSEARCH,
			BT_SEARCH_CLOSE,
			BT_LINK0,
			BT_LINK1,
			BT_LINK2,
			BT_LINK3,
			BT_LINK4,
			BT_LINK5,
			BT_LINK6,
			BT_LINK7,
			BT_LINK8,
			BT_LINK9
		}

		private class MapSpot
		{
			public MapSpot (string description, Texture path, string title, byte type, Texture marker, bool bolded, List<int> map_ids)
			{
				this.description = description;
				this.title = title;
				this.type = type;
				this.bolded = bolded;
			}

			public string description;
			public Texture path = new Texture ();
			public string title;
			public byte type;
			public Texture marker = new Texture ();
			public bool bolded;
			public List<int> map_ids = new List<int> ();
		}

		private bool search;
		private bool show_path_img;

		private int mapid;

		private string parent_map;
		private string user_map;

		private Texture search_background = new Texture ();
		private Texture search_notice = new Texture ();
		private Texture base_img = new Texture ();
		private Texture path_img = new Texture ();

		private Animation cur_pos = new Animation ();
		private Animation[] npc_pos = new Animation[MAPSPOT_TYPE_MAX];

		private Textfield search_text = new Textfield ();

		private SortedDictionary<ushort, Texture> link_images = new SortedDictionary<ushort, Texture> ();
		private SortedDictionary<ushort, string> link_maps = new SortedDictionary<ushort, string> ();

		private List<System.Tuple<Point_short, MapSpot>> map_spots = new List<System.Tuple<Point_short, MapSpot>> ();

		private Point_short bg_dimensions = new Point_short ();
		private Point_short bg_search_dimensions = new Point_short ();
		private Point_short background_dimensions = new Point_short ();
		private Point_short base_position = new Point_short ();
	}
}


#if USE_NX
#endif