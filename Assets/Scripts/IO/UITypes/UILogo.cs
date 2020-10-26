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
	public class UILogo : UIElement
	{
		public const Type TYPE = UIElement.Type.START;
		public const bool FOCUSED = false;
		public const bool TOGGLED = false;

		public UILogo() : base(new Point<short>(0, 0), new Point<short>(800, 600))
		{
			new Music("BgmUI.img/NxLogo").play_once();

			nexon_ended = false;
			wizet_ended = false;
			user_clicked = false;

			WzObject Logo = nl.nx.wzFile_ui["Logo.img"];

			Nexon = Logo["Nexon"];
			Wizet = Logo["Wizet"];
			WizetEnd = Logo["Wizet"]["40"];
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float inter) const override
		public override void draw(float inter)
		{
			if (!user_clicked)
			{
				if (!nexon_ended)
				{
					Nexon.draw(position + new Point<short>(440, 360), inter);
				}
				else
				{
					if (!wizet_ended)
					{
						Wizet.draw(position + new Point<short>(263, 195), inter);
					}
					else
					{
						WizetEnd.draw(position + new Point<short>(263, 195));
					}
				}
			}
			else
			{
				WizetEnd.draw(position + new Point<short>(263, 195));
			}
		}
		public override void update()
		{
			if (!nexon_ended)
			{
				nexon_ended = Nexon.update();
			}
			else
			{
				if (!wizet_ended)
				{
					wizet_ended = Wizet.update();
				}
				else
				{
					Configuration.get().set_start_shown(true);

					UI.get().remove(UIElement.Type.START);
					UI.get().emplace<UILogin>();
				}
			}
		}

		public override Cursor.State send_cursor(bool clicked, Point<short> cursorpos)
		{
			Cursor.State ret = clicked ? Cursor.State.CLICKING : Cursor.State.IDLE;

			if (clicked && !user_clicked)
			{
				user_clicked = true;
			}

			return ret;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type()
		{
			return TYPE;
		}

		private Animation Nexon = new Animation();
		private Animation Wizet = new Animation();
		private Texture WizetEnd = new Texture();

		private bool nexon_ended;
		private bool wizet_ended;
		private bool user_clicked;
	}
}




#if USE_NX
#endif
