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
	public class UIJoypad : UIDragElement<PosJOYPAD>
	{
		public const Type TYPE = UIElement.Type.JOYPAD;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public UIJoypad (params object[] args) : this ()
		{
		}
		
		// TODO: Add combo boxes nl::nx::ui["Basic.img"]["ComboBox"] / ["ComboBox5"];
		public UIJoypad()
		{
			//this.UIDragElement<PosJOYPAD> = new <type missing>();
			alternative_settings = false; // TODO: Get user's key settings type

			WzObject JoyPad = nl.nx.wzFile_ui["UIWindow.img"]["JoyPad"];
			WzObject Basic = nl.nx.wzFile_ui["Basic.img"];

			backgrnd[true] = JoyPad["backgrnd_alternative"];
			backgrnd[false] = JoyPad["backgrnd_classic"];

			buttons[(int)Buttons.DEFAULT] = new MapleButton(JoyPad["BtDefault"]);
			buttons[(int)Buttons.CANCEL] = new MapleButton(Basic["BtCancel4"], new Point<short>(124, 303));
			buttons[(int)Buttons.OK] = new MapleButton(Basic["BtOK4"], new Point<short>(82, 303));

			for (int i = 0; i < key_text.Length; i++)
			{
				key_text[i]= new Text(Text.Font.A12M, Text.Alignment.LEFT, Color.Name.BLACK, "None");
			}
			dimension = backgrnd[true].get_dimensions();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float inter) const override
		public override void draw(float inter)
		{
			backgrnd[alternative_settings].draw(position);

			short x = 79;
			short y = 24;
			short y_adj = 18;

			for (uint i = 0; i < (ulong)Setting.SETTING_NUM; i++)
			{
				if (i == 0)
				{
					key_text[i].draw(position + new Point<short>(x, y));
				}
				else if (i > 0 && i < 4)
				{
					key_text[i].draw(position + new Point<short>((short)(x - 16), (short)(y + 44 + y_adj * (i - 1))));
				}
				else
				{
					key_text[i].draw(position + new Point<short>((short)(x - 16), (short)(y + 123 + y_adj * (i - 4))));
				}
			}

			base.draw(inter);
		}

		public override void send_key(int keycode, bool pressed, bool escape)
		{
			if (pressed)
			{
				if (escape)
				{
					cancel();
				}
				else if (keycode == (int)KeyAction.Id.RETURN)
				{
					save();
				}
			}
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
			case Buttons.DEFAULT:
				break;
			case Buttons.CANCEL:
				cancel();
				break;
			case Buttons.OK:
				save();
				break;
			default:
				break;
			}

			return Button.State.NORMAL;
		}

		private void cancel()
		{
			deactivate();
		}
		private void save()
		{
			cancel();
		}

		private enum Buttons : ushort
		{
			DEFAULT,
			CANCEL,
			OK
		}

		private bool alternative_settings;
		private BoolPair<Texture> backgrnd = new BoolPair<Texture>();

		private enum Setting : byte
		{
			// Joypad name
			NAME,

			// Keys
			ATTACK,
			JUMP,
			PICKUP,

			// Hot keys
			HOTKEY0,
			HOTKEY1,
			HOTKEY2,
			HOTKEY3,
			HOTKEY4,
			HOTKEY5,
			HOTKEY6,
			HOTKEY7,
			SETTING_NUM
		}

		private Text[] key_text =new Text[(int)UIJoypad.Setting.SETTING_NUM]; 
	}
}


#if USE_NX
#endif
