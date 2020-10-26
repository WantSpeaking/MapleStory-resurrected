using System.Collections.Generic;

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
	public class StatusInfo
	{
		public StatusInfo(string str, Color.Name color)
		{
			text = new Text(Text.Font.A12M, Text.Alignment.RIGHT, color, str);
			shadow = new Text(Text.Font.A12M, Text.Alignment.RIGHT, Color.Name.BLACK, str);

			opacity.set(1.0f);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point<short> position, float alpha) const
		public void draw(Point<short> position, float alpha)
		{
			float interopc = opacity.get(alpha);

			shadow.draw(new DrawArgument(position + new Point<short>(1, 1), interopc));
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: text.draw(DrawArgument(position, interopc));
			text.draw(new DrawArgument(position, interopc));
		}
		public bool update()
		{
			const float FADE_STEP = Constants.TIMESTEP * 1.0f / FADE_DURATION;
			opacity -= FADE_STEP;

			return opacity.last() < FADE_STEP;
		}

		private Text text = new Text();
		private Text shadow = new Text();
		private Linear<float> opacity = new Linear<float>();

		// 8 seconds
		private const long FADE_DURATION = 8_000;
	}


	public class UIStatusMessenger : UIElement
	{
		public const Type TYPE = UIElement.Type.STATUSMESSENGER;
		public const bool FOCUSED = false;
		public const bool TOGGLED = false;

		public UIStatusMessenger()
		{
			short height = Constants.get().get_viewheight();
			short width = Constants.get().get_viewwidth();

			update_screen(width, height);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float inter) const override
		public override void draw(float inter)
		{
			Point<short> infopos = new Point<short>(position.x(), position.y());

			foreach (StatusInfo info in statusinfos)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: info.draw(infopos, inter);
				info.draw(infopos, inter);
				infopos.shift_y(-14);
			}
		}
		public override void update()
		{
			foreach (StatusInfo info in statusinfos)
			{
				info.update();
			}
		}
		public override void update_screen(short new_width, short new_height)
		{
			short y_adj = (short)((new_width > 800) ? 37 : 0);

			position = new Point<short>((short)(new_width - 6), (short)(new_height - 145 + y_adj));
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type()
		{
			return TYPE;
		}

		public void show_status(Color.Name color, string message)
		{
			statusinfos.AddFirst(new StatusInfo(message, color));

			if (statusinfos.Count > MAX_MESSAGES)
			{
				statusinfos.RemoveLast();
			}
		}

		private const uint MAX_MESSAGES = 6;

		private LinkedList<StatusInfo> statusinfos = new LinkedList<StatusInfo>();
	}
}

