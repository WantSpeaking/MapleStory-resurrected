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
	// A transparent rectangle with icon size (30x30)
	public class IconCover
	{
		public enum Type
		{
			BUFF,
			COOLDOWN
		}

		public IconCover(Type t, int duration)
		{
			cover = new ColorBox(30, 30, Color.Name.BLACK, 0.6f);

			if (duration <= Constants.TIMESTEP)
			{
				scalestep = 1.0f;
			}
			else
			{
				scalestep = Constants.TIMESTEP * 1.0f / duration;
			}

			type = t;

			switch (type)
			{
			case Type.BUFF:
				yscale.set(0.0f);
				break;
			case Type.COOLDOWN:
				yscale.set(1.0f);
				break;
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point<short> position, float alpha) const
		public void draw(Point<short> position, float alpha)
		{
			float interyscale = yscale.get(alpha);
			var interheight = (short)(30 * interyscale);

			if (interheight == 0)
			{
				return;
			}

			cover.draw(new DrawArgument(position + new Point<short>(0, (short)(30 - interheight)), new Point<short>(30, interheight)));
		}
		public void update()
		{
			switch (type)
			{
			case Type.BUFF:
				yscale += scalestep;

				if (yscale.last() >= 1.0f)
				{
					yscale.set(1.0f);
					scalestep = 0.0f;
				}

				break;
			case Type.COOLDOWN:
				yscale -= scalestep;

				if (yscale.last() <= 0.0f)
				{
					yscale.set(0.0f);
					scalestep = 0.0f;
				}

				break;
			}
		}

		private ColorBox cover = new ColorBox();
		private Linear<float> yscale = new Linear<float>();
		private float scalestep;
		private Type type;
	}
}
