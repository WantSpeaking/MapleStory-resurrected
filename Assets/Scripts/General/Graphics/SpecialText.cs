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
	public class OutlinedText
	{
		public Text inner = new Text();
		public Text l = new Text();
		public Text r = new Text();
		public Text t = new Text();
		public Text b = new Text();

		public OutlinedText(Text.Font font, Text.Alignment alignment, Color.Name innerColor, Color.Name outerColor)
		{
			inner = new Text(font, alignment, innerColor);
			l = new Text(font, alignment, outerColor);
			r = new Text(font, alignment, outerColor);
			t = new Text(font, alignment, outerColor);
			b = new Text(font, alignment, outerColor);
		}

		public OutlinedText()
		{
		}

		public void draw(Point_short parentpos)
		{
			l.draw(parentpos + new Point_short(-1, 0));
			r.draw(parentpos + new Point_short(1, 0));
			t.draw(parentpos + new Point_short(0, -1));
			b.draw(parentpos + new Point_short(0, 1));
			inner.draw(parentpos);
		}

		public void change_text(string text)
		{
			inner.change_text(text);
			l.change_text(text);
			r.change_text(text);
			t.change_text(text);
			b.change_text(text);
		}

		public void change_color(Color.Name color)
		{
			inner.change_color(color);
		}

		public short width()
		{
			return inner.width();
		}
	}

	public class ShadowText
	{
		public Text top = new Text();
		public Text shadow = new Text();

		public ShadowText(Text.Font font, Text.Alignment alignment, Color.Name topColor, Color.Name shadowColor)
		{
			top = new Text(font, alignment, topColor);
			shadow = new Text(font, alignment, shadowColor);
		}

		public ShadowText()
		{
		}

		public void draw(Point_short parentpos)
		{
			shadow.draw(parentpos + new Point_short(1, 1));
			top.draw(parentpos);
		}

		public void change_text(string text)
		{
			top.change_text(text);
			shadow.change_text(text);
		}

		public void change_color(Color.Name color)
		{
			top.change_color(color);
		}

		public short width()
		{
			return top.width();
		}
	}
}