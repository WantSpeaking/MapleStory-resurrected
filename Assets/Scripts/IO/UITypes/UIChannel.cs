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
	public class UIChannel : UIDragElement<PosCHANNEL>
	{
		public const Type TYPE = UIElement.Type.CHANNEL;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public UIChannel()
		{
			//this.UIDragElement<PosCHANNEL> = new <type missing>();
			byte selected_world = 1; // TODO: Need to get current world user is on
			current_channel = 9; // TODO: Need to get current channel user is on
			selected_channel = current_channel;
			channel_count = 20; // TODO: Need to get total number of channels on world

			WzObject Channel = nl.nx.wzFile_ui["UIWindow2.img"]["Channel"];

			WzObject backgrnd = Channel["backgrnd"];
			Texture bg = backgrnd;

			sprites.Add(new Sprite (backgrnd, new Point<short>(1, 0)));
			sprites.Add(Channel["backgrnd2"]);
			sprites.Add(Channel["backgrnd3"]);
			sprites.Add(new Sprite(Channel["world"][selected_world.ToString ()], new Point<short>(16, 30)));

			buttons[(int)Buttons.CANCEL] = new MapleButton(Channel["BtCancel"]);
			buttons[(int)Buttons.CHANGE] = new MapleButton(Channel["BtChange"], new Point<short>(-20, 0));

			channel[true] = Channel["channel1"];
			channel[false] = Channel["channel0"];

			uint x = 0;
			uint y = 0;

			for (uint i = 0; i < channel_count; i++)
			{
				if (x >= 5)
				{
					x = 0;
					y++;
				}

				ch.Add(new Sprite(Channel["ch"][i.ToString ()], new Point<short>((short)(19 + 70 * x), (short)(60 + 20 * y))));
				buttons[(ushort)((int)Buttons.CH + i)] = new AreaButton(new Point<short>((short)(11 + 70 * x), (short)(55 + 20 * y)), channel[true].get_dimensions());

				if (i == selected_channel)
				{
					current_channel_x = (short)(11 + 70 * x);
					current_channel_y = (short)(55 + 20 * y);
					selected_channel_x = current_channel_x;
					selected_channel_y = current_channel_y;
				}

				x++;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: dimension = bg.get_dimensions();
			dimension=(bg.get_dimensions());
			dragarea = new Point<short>(dimension.x(), 20);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float inter) const override
		public override void draw(float inter)
		{
			base.draw(inter);

			if (current_channel == selected_channel)
			{
				channel[true].draw(new DrawArgument(position.x() + selected_channel_x, position.y() + selected_channel_y));
			}
			else
			{
				channel[true].draw(new DrawArgument(position.x() + selected_channel_x, position.y() + selected_channel_y));
				channel[false].draw(new DrawArgument(position.x() + current_channel_x, position.y() + current_channel_y));
			}

			foreach (var sprite in ch)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: sprite.draw(position, inter);
				sprite.draw(position, inter);
			}
		}
		public override void update()
		{
			base.update();

			foreach (var sprite in ch)
			{
				sprite.update();
			}
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
					change_channel();
				}
				else if (keycode == (int)KeyAction.Id.UP)
				{
					if (selected_channel > 4)
					{
						selected_channel -= 5;
					}
					else
					{
						for (uint i = 0; i < 3; i++)
						{
							selected_channel += 5;
						}
					}

					if (selected_channel == current_channel)
					{
						if (selected_channel > 4)
						{
							selected_channel -= 5;
						}
						else
						{
							for (uint i = 0; i < 3; i++)
							{
								selected_channel += 5;
							}
						}
					}

					update_selected_channel_position();
				}
				else if (keycode == (int)KeyAction.Id.DOWN)
				{
					if (selected_channel < 15)
					{
						selected_channel += 5;
					}
					else
					{
						for (uint i = 0; i < 3; i++)
						{
							selected_channel -= 5;
						}
					}

					if (selected_channel == current_channel)
					{
						if (selected_channel < 15)
						{
							selected_channel += 5;
						}
						else
						{
							for (uint i = 0; i < 3; i++)
							{
								selected_channel -= 5;
							}
						}
					}

					update_selected_channel_position();
				}
				else if (keycode == (int)KeyAction.Id.LEFT)
				{
					if (selected_channel != 0)
					{
						selected_channel--;
					}
					else
					{
						selected_channel = (byte)(channel_count - 1);
					}

					if (selected_channel == current_channel)
					{
						if (selected_channel != 0)
						{
							selected_channel--;
						}
						else
						{
							selected_channel = (byte)(channel_count - 1);
						}
					}

					update_selected_channel_position();
				}
				else if (keycode == (int)KeyAction.Id.RIGHT)
				{
					if (selected_channel != channel_count - 1)
					{
						selected_channel++;
					}
					else
					{
						selected_channel = 0;
					}

					if (selected_channel == current_channel)
					{
						if (selected_channel != channel_count - 1)
						{
							selected_channel++;
						}
						else
						{
							selected_channel = 0;
						}
					}

					update_selected_channel_position();
				}
			}
		}
		public override Cursor.State send_cursor(bool clicked, Point<short> cursorpos)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: Cursor::State dstate = UIDragElement::send_cursor(clicked, cursorpos);
			Cursor.State dstate = base.send_cursor(clicked, cursorpos);

			if (dragged)
			{
				return dstate;
			}

			Cursor.State ret = clicked ? Cursor.State.CLICKING : Cursor.State.IDLE;

			for (uint i = 0; i < (ulong)(channel_count + Buttons.CH); i++)
			{
				if (buttons[(ushort)i].is_active() && buttons[(ushort)i].bounds(position).contains(cursorpos))
				{
					if (buttons[(ushort)i].get_state() == Button.State.NORMAL)
					{
						if (i < (ulong)Buttons.CH)
						{
							new Sound(Sound.Name.BUTTONOVER).play();

							buttons[(ushort)i].set_state(Button.State.MOUSEOVER);
							ret = Cursor.State.CANCLICK;
						}
						else
						{
							buttons[(ushort)i].set_state(Button.State.MOUSEOVER);
							ret = Cursor.State.IDLE;
						}
					}
					else if (buttons[(ushort)i].get_state() == Button.State.MOUSEOVER)
					{
						if (clicked)
						{
							if (i < (ulong)Buttons.CH)
							{
								new Sound(Sound.Name.BUTTONCLICK).play();
							}

							buttons[(ushort)i].set_state(button_pressed((ushort)i));

							ret = Cursor.State.IDLE;
						}
						else
						{
							if (i < (ulong)Buttons.CH)
							{
								ret = Cursor.State.CANCLICK;
							}
							else
							{
								ret = Cursor.State.IDLE;
							}
						}
					}
				}
				else if (buttons[(ushort)i].get_state() == Button.State.MOUSEOVER)
				{
					buttons[(ushort)i].set_state(Button.State.NORMAL);
				}
			}

			return ret;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type()
		{
			return TYPE;
		}

		public override Button.State button_pressed(ushort buttonid)
		{
			if (buttonid < (int)Buttons.CH)
			{
				switch ((Buttons)buttonid)
				{
				case Buttons.CANCEL:
					cancel();
					break;
				case Buttons.CHANGE:
					change_channel();
					break;
				default:
					break;
				}
			}
			else
			{
				if (buttonid - (int)Buttons.CH == current_channel)
				{
					return Button.State.NORMAL;
				}

				selected_channel = (byte)(buttonid - Buttons.CH);
				update_selected_channel_position();
			}

			return Button.State.NORMAL;
		}

		private void cancel()
		{
			deactivate();

			current_channel = 9; // TODO: Need to get current channel user is on
			selected_channel = current_channel;
			selected_channel_x = current_channel_x;
			selected_channel_y = current_channel_y;
		}
		private void change_channel()
		{
			// TODO: Send packet to change channel?
			cancel();
		}
		private void update_selected_channel_position()
		{
			uint x = 0;
			uint y = 0;

			for (uint i = 0; i < channel_count; i++)
			{
				if (x >= 5)
				{
					x = 0;
					y++;
				}

				if (i == selected_channel)
				{
					selected_channel_x = (short)(11 + 70 * x);
					selected_channel_y = (short)(55 + 20 * y);
					break;
				}

				x++;
			}
		}

		private enum Buttons : ushort
		{
			CANCEL,
			CHANGE,
			CH
		}

		private byte current_channel;
		private byte selected_channel;
		private byte channel_count;
		private BoolPair<Texture> channel = new BoolPair<Texture>();
		private List<Sprite> ch = new List<Sprite>();
		private short current_channel_x;
		private short current_channel_y;
		private short selected_channel_x;
		private short selected_channel_y;
	}
}




#if USE_NX
#endif
