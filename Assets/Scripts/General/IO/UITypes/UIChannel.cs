#define USE_NX

using System.Collections.Generic;
using Beebyte.Obfuscator;
using MapleLib.WzLib;





namespace ms
{
	[Skip]
	public class UIChannel : UIDragElement<PosCHANNEL>
	{
		public const Type TYPE = UIElement.Type.CHANNEL;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public UIChannel (params object[] args) : this ()
		{
		}
		
		public UIChannel()
		{
			//this.UIDragElement<PosCHANNEL> = new <type missing>();
			byte selected_world = 1; // TODO: Need to get current world user is on
			current_channel = 9; // TODO: Need to get current channel user is on
			selected_channel = current_channel;
			channel_count = 20; // TODO: Need to get total number of channels on world

			WzObject Channel = ms.wz.wzFile_ui["UIWindow2.img"]["Channel"];

			WzObject backgrnd = Channel["backgrnd"];
			Texture bg = backgrnd;

			sprites.Add(new Sprite (backgrnd, new Point_short(1, 0)));
			sprites.Add(Channel["backgrnd2"]);
			sprites.Add(Channel["backgrnd3"]);
			sprites.Add(new Sprite(Channel["world"][selected_world.ToString ()], new Point_short(16, 30)));

			buttons[(int)Buttons.CANCEL] = new MapleButton(Channel["BtCancel"]);
			buttons[(int)Buttons.CHANGE] = new MapleButton(Channel["BtChange"], new Point_short(-20, 0));

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

				ch.Add(new Sprite(Channel["ch"][i.ToString ()], new Point_short((short)(19 + 70 * x), (short)(60 + 20 * y))));
				buttons[(ushort)((int)Buttons.CH + i)] = new AreaButton(new Point_short((short)(11 + 70 * x), (short)(55 + 20 * y)), channel[true].get_dimensions());

				if (i == selected_channel)
				{
					current_channel_x = (short)(11 + 70 * x);
					current_channel_y = (short)(55 + 20 * y);
					selected_channel_x = current_channel_x;
					selected_channel_y = current_channel_y;
				}

				x++;
			}

			dimension=new Point_short (bg.get_dimensions());
			dragarea = new Point_short(dimension.x(), 20);
		}

		public override void draw(float inter)
		{
			base.draw(inter);

			if (current_channel == selected_channel)
			{
				channel[true].draw(new DrawArgument((short)(position.x () + selected_channel_x), (short)(position.y () + selected_channel_y)));
			}
			else
			{
				channel[true].draw(new DrawArgument((short)(position.x () + selected_channel_x), (short)(position.y () + selected_channel_y)));
				channel[false].draw(new DrawArgument((short)(current_channel_x), (short)(position.y () + current_channel_y)));
			}

			foreach (var sprite in ch)
			{
				sprite.draw(new Point_short (position) , inter);
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
		public override Cursor.State send_cursor(bool clicked, Point_short cursorpos)
		{
			Cursor.State dstate = base.send_cursor(clicked, new Point_short (cursorpos));

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
		private BoolPairNew<Texture> channel = new BoolPairNew<Texture>();
		private List<Sprite> ch = new List<Sprite>();
		private short current_channel_x;
		private short current_channel_y;
		private short selected_channel_x;
		private short selected_channel_y;
	}
}




#if USE_NX
#endif
