#define USE_NX

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
	public class UIQuestLog : UIDragElement<PosQUEST>
	{
		public const Type TYPE = UIElement.Type.QUESTLOG;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public UIQuestLog(QuestLog ql)
		{
			//this.UIDragElement<PosQUEST> = new <type missing>();
			this.questlog = ql;
			tab = (int)Buttons.TAB0;

			WzObject close = nl.nx.wzFile_ui["Basic.img"]["BtClose3"];
			WzObject quest = nl.nx.wzFile_ui["UIWindow2.img"]["Quest"];
			WzObject list = quest["list"];

			WzObject backgrnd = list["backgrnd"];

			sprites.Add(backgrnd);
			sprites.Add(list["backgrnd2"]);

			notice_sprites.Add(list["notice0"]);
			notice_sprites.Add(list["notice1"]);
			notice_sprites.Add(list["notice2"]);

			WzObject taben = list["Tab"]["enabled"];
			WzObject tabdis = list["Tab"]["disabled"];

			buttons[(int)Buttons.TAB0] =new TwoSpriteButton(tabdis["0"], taben["0"]);
			buttons[(int)Buttons.TAB1] =new TwoSpriteButton(tabdis["1"], taben["1"]);
			buttons[(int)Buttons.TAB2] =new TwoSpriteButton(tabdis["2"], taben["2"]);
			buttons[(int)Buttons.CLOSE] = new MapleButton(close, new Point<short>(275, 6));
			buttons[(int)Buttons.SEARCH] = new MapleButton(list["BtSearch"]);
			buttons[(int)Buttons.ALL_LEVEL] = new MapleButton(list["BtAllLevel"]);
			buttons[(int)Buttons.MY_LOCATION] = new MapleButton(list["BtMyLocation"]);

			search_area = list["searchArea"];
			var search_area_dim = search_area.get_dimensions();
			var search_area_origin = search_area.get_origin().abs();

			var search_pos_adj = new Point<short>(29, 0);
			var search_dim_adj = new Point<short>(-80, 0);

			var search_pos = position + search_area_origin + search_pos_adj;
			var search_dim = search_pos + search_area_dim + search_dim_adj;

			search = new Textfield(Text.Font.A11M, Text.Alignment.LEFT, Color.Name.BOULDER, new Rectangle<short>(search_pos, search_dim), 19);
			placeholder = new Text(Text.Font.A11M, Text.Alignment.LEFT, Color.Name.BOULDER, "Enter the quest name.");

			slider = new Slider((int)Slider.Type.DEFAULT_SILVER, new Range<short>(0, 279), 150, 20, 5, (bool UnnamedParameter1) =>
			{
			});

			change_tab(tab);

			dimension = new Texture(backgrnd).get_dimensions();
			dragarea = new Point<short>(dimension.x(), 20);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float alpha) const override
		public override void draw(float alpha)
		{
			base.draw_sprites(alpha);

			Point<short> notice_position = new Point<short>(0, 26);

			if (tab == (int)Buttons.TAB0)
			{
				notice_sprites[tab].draw(position + notice_position + new Point<short>(9, 0), alpha);
			}
			else if (tab == (int)Buttons.TAB1)
			{
				notice_sprites[tab].draw(position + notice_position + new Point<short>(0, 0), alpha);
			}
			else
			{
				notice_sprites[tab].draw(position + notice_position + new Point<short>(-10, 0), alpha);
			}

			if (tab != (int)Buttons.TAB2)
			{
				search_area.draw(position);
				search.draw(new Point<short>(0, 0));

				if (search.get_state() == Textfield.State.NORMAL && search.empty())
				{
					placeholder.draw(position + new Point<short>(39, 51));
				}
			}

			slider.draw(position + new Point<short>(126, 75));

			base.draw_buttons(alpha);
		}

		public override void send_key(int keycode, bool pressed, bool escape)
		{
			if (pressed)
			{
				if (escape)
				{
					deactivate();
				}
				else if (keycode == (int)KeyAction.Id.TAB)
				{
					ushort new_tab = tab;

					if (new_tab < (int)Buttons.TAB2)
					{
						new_tab++;
					}
					else
					{
						new_tab = (int)Buttons.TAB0;
					}

					change_tab(new_tab);
				}
			}
		}
		public override Cursor.State send_cursor(bool clicking, Point<short> cursorpos)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: if (Cursor::State new_state = search.send_cursor(cursorpos, clicking))
			Cursor.State new_state = search.send_cursor (cursorpos, clicking);
			if (new_state != Cursor.State.IDLE)
			{
				return new_state;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return UIDragElement::send_cursor(clicking, cursorpos);
			return base.send_cursor(clicking, cursorpos);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type()
		{
			return TYPE;
		}

		public override Button.State button_pressed(ushort buttonid)
		{
			switch ((Buttons)buttonid)
			{
			case Buttons.TAB0:
			case Buttons.TAB1:
			case Buttons.TAB2:
				change_tab(buttonid);

				return Button.State.IDENTITY;
			case Buttons.CLOSE:
				deactivate();
				break;
			default:
				break;
			}

			return Button.State.NORMAL;
		}

		private void change_tab(ushort tabid)
		{
			ushort oldtab = tab;
			tab = tabid;

			if (oldtab != tab)
			{
				buttons[(uint)((int)Buttons.TAB0 + oldtab)].set_state(Button.State.NORMAL);
				buttons[(int)Buttons.MY_LOCATION].set_active(tab == (int)Buttons.TAB0);
				buttons[(int)Buttons.ALL_LEVEL].set_active(tab == (int)Buttons.TAB0);
				buttons[(int)Buttons.SEARCH].set_active(tab != (int)Buttons.TAB2);

				if (tab == (int)Buttons.TAB2)
				{
					search.set_state(Textfield.State.DISABLED);
				}
				else
				{
					search.set_state(Textfield.State.NORMAL);
				}
			}

			buttons[(uint)((int)Buttons.TAB0 + tab)].set_state(Button.State.PRESSED);
		}

		private enum Buttons : ushort
		{
			TAB0,
			TAB1,
			TAB2,
			CLOSE,
			SEARCH,
			ALL_LEVEL,
			MY_LOCATION
		}

		private readonly QuestLog questlog;

		private ushort tab;
		private List<Sprite> notice_sprites = new List<Sprite>();
		private Textfield search = new Textfield();
		private Text placeholder = new Text();
		private Slider slider = new Slider();
		private Texture search_area = new Texture();
	}
}


#if USE_NX
#endif
