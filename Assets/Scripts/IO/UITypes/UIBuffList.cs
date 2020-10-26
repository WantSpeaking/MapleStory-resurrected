#define USE_NX

using System.Collections.Generic;
using MapleLib.WzLib;
using System.Linq;
using UnityEngine;

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
	public class BuffIcon
	{
		public BuffIcon(int buff, int dur)
		{
			this.cover = new ms.IconCover(IconCover.Type.BUFF, dur - FLASH_TIME);
			buffid = buff;
			duration = dur;
			opacity.set(1.0f);
			opcstep = -0.05f;

			if (buffid >= 0)
			{
				string strid = string_format.extend_id(buffid, 7);
				WzObject src = nl.nx.wzFile_skill[strid.Substring(0, 3) + ".img"]["skill"][strid];
				icon = src["icon"];
			}
			else
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: icon = ItemData::get(-buffid).get_icon(true);
				icon=(ItemData.get(-buffid).get_icon(true));
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point<short> position, float alpha) const
		public void draw(Point<short> position, float alpha)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: icon.draw(DrawArgument(position, opacity.get(alpha)));
			icon.draw(new DrawArgument(position, opacity.get(alpha)));
			cover.draw(position + new Point<short>(1, -31), alpha);
		}
		public bool update()
		{
			if (duration <= FLASH_TIME)
			{
				opacity += opcstep;

				bool fadedout = opcstep < 0.0f && opacity.last() <= 0.0f;
				bool fadedin = opcstep > 0.0f && opacity.last() >= 1.0f;

				if (fadedout || fadedin)
				{
					opcstep = -opcstep;
				}
			}

			cover.update();

			duration -= Constants.TIMESTEP;

			return duration < Constants.TIMESTEP;
		}

		private const ushort FLASH_TIME = 3_000;

		private Texture icon;
		private IconCover cover;
		private int buffid;
		private int duration;
		private Linear<float> opacity = new Linear<float>();
		private float opcstep;
	}


	public class UIBuffList : UIElement
	{
		public const Type TYPE = UIElement.Type.BUFFLIST;
		public const bool FOCUSED = false;
		public const bool TOGGLED = false;

		public UIBuffList()
		{
			short height = Constants.get().get_viewheight();
			short width = Constants.get().get_viewwidth();

			update_screen(width, height);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(float alpha) const override
		public override void draw(float alpha)
		{
			Point<short> icpos = position;

			foreach (var icon in icons)
			{
				icon.Value.draw(icpos, alpha);
				icpos.shift_x(-32);
			}
		}

		private readonly List<KeyValuePair<int,BuffIcon>> cache = new List<KeyValuePair<int, BuffIcon>> ();
		public override void update()
		{
			cache.Clear ();
			foreach (var pair in icons)
			{
				if (pair.Value.update ())
				{
					cache.Add (pair);
				}
			}

			foreach (var pair in cache)
			{
				icons.Remove (pair.Key);
			}
			/*for (var iter = icons.GetEnumerator(); iter. != icons.end();)
			{
				bool expired = iter.Current.Value.update();

				if (expired)
				{
					iter = icons.Remove(iter);
				}
				else
				{
					iter++;
				}
			}*/
		}
		public override void update_screen(short new_width, short new_height)
		{
			position = new Point<short>((short)(new_width - 35), 55);
			dimension = new Point<short>(position.x(), 32);
		}

		public override Cursor.State send_cursor(bool pressed, Point<short> cursorposition)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return UIElement::send_cursor(pressed, cursorposition);
			return base.send_cursor(pressed, cursorposition);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type()
		{
			return TYPE;
		}

		public void add_buff(int buffid, int duration)
		{
			icons.Add (buffid,new BuffIcon (buffid,duration));
		}

		private Dictionary<int, BuffIcon> icons = new Dictionary<int, BuffIcon>();
	}
}


#if USE_NX
#endif
