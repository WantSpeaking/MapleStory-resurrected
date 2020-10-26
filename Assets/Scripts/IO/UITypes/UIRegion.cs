#define USE_NX

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
	public class UIRegion : UIElement
	{
		public const Type TYPE = UIElement.Type.REGION;
		public const bool FOCUSED = false;
		public const bool TOGGLED = false;

		public UIRegion() : base(new Point<short>(0, 0), new Point<short>(800, 600))
		{
			WzObject Common = nl.nx.wzFile_ui["Login.img"]["Common"];
			WzObject frame = nl.nx.wzFile_mapLatest["Obj"]["login.img"]["Common"]["frame"]["2"]["0"];
			WzObject Gateway = nl.nx.wzFile_ui["Gateway.img"]["WorldSelect"];
			WzObject na = Gateway["BtButton0"];
			WzObject eu = Gateway["BtButton1"];

			sprites.Add(Gateway["backgrnd2"]);
			sprites.Add(new Sprite (frame, new Point<short>(400, 300)));
			sprites.Add(new Sprite (Common["frame"], new Point<short>(400, 300)));

			short pos_y = 84;
			Point<short> na_pos = new Point<short>(155, pos_y);
			Point<short> eu_pos = new Point<short>(424, pos_y);

			buttons[(int)Buttons.NA] = new MapleButton(na, na_pos);
			buttons[(int)Buttons.EU] = new MapleButton(eu, eu_pos);
			buttons[(int)Buttons.EXIT] = new MapleButton(Common["BtExit"], new Point<short>(0, 540));

			Point<short> na_dim = new Texture(na["normal"]["0"]).get_dimensions();
			Point<short> eu_dim = new Texture(eu["normal"]["0"]).get_dimensions();

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: na_rect = Rectangle<short>(na_pos, na_pos + na_dim);
			na_rect = new Rectangle<short>(na_pos, na_pos + na_dim);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: eu_rect = Rectangle<short>(eu_pos, eu_pos + eu_dim);
			eu_rect = new Rectangle<short>(eu_pos, eu_pos + eu_dim);
		}

		public override Cursor.State send_cursor(bool clicked, Point<short> cursorpos)
		{
			clear_tooltip();

			if (na_rect.contains(cursorpos))
			{
				UI.get().show_text(Tooltip.Parent.TEXT, "Warning: You may experience latency and connection issues when connecting to the NA server from Europe.");
			}

			if (eu_rect.contains(cursorpos))
			{
				UI.get().show_text(Tooltip.Parent.TEXT, "Warning: You may experience latency and connection issues when connecting to the EU server from North America.");
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return UIElement::send_cursor(clicked, cursorpos);
			return base.send_cursor(clicked, cursorpos);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type()
		{
			return TYPE;
		}

		public override Button.State button_pressed(ushort buttonid)
		{
			clear_tooltip();

			switch ((Buttons)buttonid)
			{
				case Buttons.NA:
				case Buttons.EU:
				{
					// TODO: Update UIWorldSelect after selecting new region
					//uint8_t region = (buttonid == Buttons::NA) ? 5 : 6;
					var worldselect = UI.get ().get_element<UIWorldSelect> ();
					if (worldselect!=null)
					{
						UI.get().remove(UIElement.Type.REGION);

						//worldselect->set_region(region);
						worldselect.get ().makeactive();
					}

					break;
				}
				case Buttons.EXIT:
				{
					UI.get().quit();
					break;
				}
				default:
				{
					break;
				}
			}

			return Button.State.NORMAL;
		}

		private void clear_tooltip()
		{
			UI.get().clear_tooltip(Tooltip.Parent.TEXT);
		}

		private enum Buttons : ushort
		{
			NA,
			EU,
			EXIT
		}

		private Rectangle<short> na_rect = new Rectangle<short>();
		private Rectangle<short> eu_rect = new Rectangle<short>();
	}
}




#if USE_NX
#endif
