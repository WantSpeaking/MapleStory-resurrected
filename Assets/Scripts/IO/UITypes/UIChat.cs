#define USE_NX

using System;
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
	public class UIChat : UIDragElement<PosMAPLECHAT>
	{
		public const Type TYPE = UIElement.Type.CHAT;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public UIChat()
		{
			//this.UIDragElement<PosMAPLECHAT> = new <type missing>();
			show_weekly = Configuration.get().get_show_weekly();

			WzObject socialChatEnter = nl.nx.wzFile_ui["UIWindow2.img"]["socialChatEnter"];

			WzObject backgrnd = socialChatEnter["backgrnd"];
			WzObject backgrnd4 = socialChatEnter["backgrnd4"];
			WzObject backgrnd5 = socialChatEnter["backgrnd5"];

			rank_shift = new Point<short>(86, 130);
			name_shift = new Point<short>(50, 5);

			origin_left = new Texture(backgrnd4).get_origin();
			origin_right = new Texture(backgrnd5).get_origin();

			origin_left = new Point<short>(Math.Abs(origin_left.x()), Math.Abs(origin_left.y()));
			origin_right = new Point<short>(Math.Abs(origin_right.x()), Math.Abs(origin_right.y()));

			sprites.Add(socialChatEnter["ribbon"]);
			sprites.Add(backgrnd);
			sprites.Add(socialChatEnter["backgrnd2"]);
			sprites.Add(socialChatEnter["backgrnd3"]);
			sprites.Add(backgrnd4);
			sprites.Add(backgrnd5);

			buttons[(int)Buttons.CLOSE] = new MapleButton(socialChatEnter["btX"]);
			buttons[(int)Buttons.CHAT_DUO] = new MapleButton(socialChatEnter["duoChat"]);
			buttons[(int)Buttons.CHAT_FRIEND] = new MapleButton(socialChatEnter["groupChatFrd"]);
			buttons[(int)Buttons.CHAT_RANDOM] = new MapleButton(socialChatEnter["groupChatRnd"]);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: charset = Charset(socialChatEnter["number"], Charset::Alignment::RIGHT);
			charset = new Charset(socialChatEnter["number"], Charset.Alignment.RIGHT);

			name_left = new Text(Text.Font.A12B, Text.Alignment.CENTER, Color.Name.WHITE);
			name_right = new Text(Text.Font.A12B, Text.Alignment.CENTER, Color.Name.WHITE);

			dimension = new Texture(backgrnd).get_dimensions();

			if (show_weekly)
			{
				UI.get().emplace<UIRank>();
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float inter) const override
		public override void draw(float inter)
		{
			base.draw(inter);

			charset.draw("0", position + origin_left + rank_shift);
			charset.draw("0", position + origin_right + rank_shift);

			name_left.draw(position + origin_left + name_shift);
			name_right.draw(position + origin_right + name_shift);
		}

		public override void send_key(int keycode, bool pressed, bool escape)
		{
			if (pressed && escape)
			{
				close();
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
			case Buttons.CLOSE:
				close();
				break;
			case Buttons.CHAT_DUO:
				break;
			case Buttons.CHAT_FRIEND:
				break;
			case Buttons.CHAT_RANDOM:
				break;
			default:
				break;
			}

			return Button.State.NORMAL;
		}

		private void close()
		{
			deactivate();
		}

		private enum Buttons : ushort
		{
			CLOSE,
			CHAT_DUO,
			CHAT_FRIEND,
			CHAT_RANDOM
		}

		private bool show_weekly;
		private Point<short> rank_shift = new Point<short>();
		private Point<short> name_shift = new Point<short>();
		private Point<short> origin_left = new Point<short>();
		private Point<short> origin_right = new Point<short>();
		private Charset charset = new Charset();
		private Text name_left = new Text();
		private Text name_right = new Text();
	}

	public class UIRank : UIDragElement<PosMAPLECHAT>
	{
		public const Type TYPE = UIElement.Type.CHATRANK;
		public const bool FOCUSED = false;
		public const bool TOGGLED = true;

		public UIRank()
		{
			//this.UIDragElement<PosMAPLECHAT> = new <type missing>();
			Configuration.get().set_show_weekly(false);

			WzObject socialRank = nl.nx.wzFile_ui["UIWindow2.img"]["socialRank"];

			WzObject backgrnd = socialRank["backgrnd"];
			WzObject backgrnd4 = socialRank["backgrnd4"];
			WzObject backgrnd5 = socialRank["backgrnd5"];

			rank_shift = new Point<short>(86, 130);
			name_shift = new Point<short>(52, 4);

			origin_left = new Texture(backgrnd4).get_origin();
			origin_right = new Texture(backgrnd5).get_origin();

			origin_left = new Point<short>((short)(Math.Abs(origin_left.x()) - 1), Math.Abs(origin_left.y()));
			origin_right = new Point<short>(Math.Abs(origin_right.x()), Math.Abs(origin_right.y()));

			sprites.Add(socialRank["ribbon"]);
			sprites.Add(backgrnd);
			sprites.Add(socialRank["backgrnd2"]);
			sprites.Add(socialRank["backgrnd3"]);
			sprites.Add(backgrnd4);
			sprites.Add(backgrnd5);

			buttons[(int)Buttons.CLOSE] = new MapleButton(socialRank["btX"]);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: charset = Charset(socialRank["number"], Charset::Alignment::RIGHT);
			charset = new Charset(socialRank["number"], Charset.Alignment.RIGHT);

			name_left = new Text(Text.Font.A12B, Text.Alignment.CENTER, Color.Name.WHITE);
			name_right = new Text(Text.Font.A12B, Text.Alignment.CENTER, Color.Name.WHITE);

			dimension = new Texture(backgrnd).get_dimensions();
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: position = position + Point<short>(211, 124);
			position=(position + new Point<short>(211, 124));
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float inter) const override
		public override void draw(float inter)
		{
			base.draw(inter);

			charset.draw("0", position + origin_left + rank_shift);
			charset.draw("0", position + origin_right + rank_shift);

			name_left.draw(position + origin_left + name_shift);
			name_right.draw(position + origin_right + name_shift);
		}

		public override void send_key(int keycode, bool pressed, bool escape)
		{
			if (pressed && escape)
			{
				close();
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
			case Buttons.CLOSE:
				close();
				break;
			default:
				break;
			}

			return Button.State.NORMAL;
		}

		private void close()
		{
			deactivate();
		}

		private enum Buttons : ushort
		{
			CLOSE
		}

		private Point<short> rank_shift = new Point<short>();
		private Point<short> name_shift = new Point<short>();
		private Point<short> origin_left = new Point<short>();
		private Point<short> origin_right = new Point<short>();
		private Charset charset = new Charset();
		private Text name_left = new Text();
		private Text name_right = new Text();
	}
}



#if USE_NX
#endif
