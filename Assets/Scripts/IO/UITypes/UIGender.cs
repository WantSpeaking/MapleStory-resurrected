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
	public class UIGender : UIElement
	{
		public const Type TYPE = UIElement.Type.GENDER;
		public const bool FOCUSED = true;
		public const bool TOGGLED = false;

		public UIGender(System.Action oh) : base(new Point<short>(0, 15), new Point<short>(0, 0))
		{
			this.okhandler = oh;
			CUR_TIMESTEP = 0;

			WzObject Gender = nl.nx.wzFile_ui["Login.img"]["Gender"];

			for (uint i = 0; i < 3; i++)
			{
				gender_sprites[i] = Gender["scroll"]["0"][i.ToString ()];
			}

			sprites.Add(new Sprite(Gender["text"]["0"], new Point<short>(601, 326)));

			List<string> options = new List<string>();
			options.Add("Male");
			options.Add("Female");

			ushort default_option = 0;

			buttons[(int)Buttons.NO] = new MapleButton(Gender["BtNo"], new Point<short>(650, 349));
			buttons[(int)Buttons.YES] = new MapleButton(Gender["BtYes"], new Point<short>(578, 349));
			buttons[(int)Buttons.SELECT] =new MapleComboBox(MapleComboBox.Type.DEFAULT, options, default_option, position, new Point<short>(510, 283), 65);

			dimension = gender_sprites[2].get_dimensions();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float inter) const override
		public override void draw(float inter)
		{
			Point<short> gender_pos = new Point<short>(355, 185);

			if (CUR_TIMESTEP == 0)
			{
				gender_sprites[0].draw(position + gender_pos);
			}
			else if (CUR_TIMESTEP == Constants.TIMESTEP * 3)
			{
				gender_sprites[1].draw(position + gender_pos);
			}
			else if (CUR_TIMESTEP >= Constants.TIMESTEP * 6)
			{
				gender_sprites[2].draw(position + gender_pos);

				base.draw(inter);
			}
		}
		public override void update()
		{
			base.update();

			if (CUR_TIMESTEP <= Constants.TIMESTEP * 6)
			{
				CUR_TIMESTEP += Constants.TIMESTEP;
			}
		}

		public override Cursor.State send_cursor(bool clicked, Point<short> cursorpos)
		{
			var combobox = buttons[(int)Buttons.SELECT];

			if (combobox.is_pressed() && combobox.in_combobox(cursorpos))
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
				Cursor.State new_state = combobox.send_cursor (clicked, cursorpos);
				if (new_state!= Cursor.State.IDLE)
				{
					return new_state;
				}
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
			switch ((Buttons)buttonid)
			{
				case Buttons.NO:
				{
					deactivate();
					okhandler();

					return Button.State.NORMAL;
				}
				case Buttons.YES:
				{
					UI.get().emplace<UILoginWait>();

					ushort selected_value = buttons[(int)Buttons.SELECT].get_selected();
					new GenderPacket(selected_value == 1).dispatch();

					return Button.State.NORMAL;
				}
				case Buttons.SELECT:
				{
					buttons[(int)Buttons.SELECT].toggle_pressed();

					return Button.State.NORMAL;
				}
				default:
				{
					return Button.State.DISABLED;
				}
			}
		}

		private enum Buttons : ushort
		{
			NO,
			YES,
			SELECT
		}

		private Texture[] gender_sprites =new Texture[3];
		private ushort CUR_TIMESTEP;
		private System.Action okhandler;
	}
}




#if USE_NX
#endif
