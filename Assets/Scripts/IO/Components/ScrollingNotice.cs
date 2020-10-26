using System;

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
	// The scrolling server notice at the top of the screen
	public class ScrollingNotice
	{
		public ScrollingNotice()
		{
			width = 800;
			background = new ColorBox(width, 23, Color.Name.BLACK, 0.535f);
			notice = new Text(Text.Font.A12M, Text.Alignment.LEFT, Color.Name.YELLOW);

			xpos.set(0.0);
			active = false;
		}

		public void setnotice(string n)
		{
			notice.change_text(n);
			xpos.set((double)width);
			active = n.Length > 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float alpha) const
		public void draw(float alpha)
		{
			if (active)
			{
				short interx = (short)Math.Round(xpos.get(alpha));
				var position = new Point<short>(interx, -1);

				background.draw(new Point<short>(0, 0));
				notice.draw(position);
			}
		}
		public void update()
		{
			if (active)
			{
				short new_width = Constants.get().get_viewwidth();

				if (new_width != width)
				{
					width = new_width;
					background.setwidth(width);
					xpos.set((double)width);
				}

				xpos -= 0.5;

				var xmin = (double)(-notice.width());

				if (xpos.last() < xmin)
				{
					xpos.set((double)width);
				}
			}
		}

		private ColorBox background = new ColorBox();
		private Text notice = new Text();
		private Linear<double> xpos = new Linear<double>();
		private bool active;
		private short width;
	}
}
