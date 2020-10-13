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
	public class Afterimage
	{
		public Afterimage (int skill_id, string name, string stance_name, short level)
		{
			WzObject src = null;

			if (skill_id > 0)
			{
				string strid = string_format.extend_id (skill_id, 7);
				src = nl.nx.wzFile_skill[strid.Substring (0, 3) + ".img"]["skill"][strid]["afterimage"][name][stance_name];
			}

			if (src == null)
			{
				src = nl.nx.wzFile_character["Afterimage"][name + ".img"][(level / 10).ToString ()][stance_name];
			}

			range = new Rectangle<short> (src);
			firstframe = 0;
			displayed = false;

			
			foreach (var sub in ((WzImageProperty)src).WzProperties)
			{
				byte frame = string_conversion<byte>.or_default (sub.Name, (byte)255);

				if (frame < 255)
				{
					animation =new Animation (sub);
					firstframe = frame;
				}
			}
		}

		public Afterimage ()
		{
			firstframe = 0;
			displayed = true;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(byte stframe, const DrawArgument& args, float alpha) const
		public void draw (byte stframe, DrawArgument args, float alpha)
		{
			if (!displayed && stframe >= firstframe)
			{
				animation.draw (args, alpha);
			}
		}

		public void update (byte stframe, ushort timestep)
		{
			if (!displayed && stframe >= firstframe)
			{
				displayed = animation.update (timestep);
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: byte get_first_frame() const
		public byte get_first_frame ()
		{
			return firstframe;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Rectangle<short> get_range() const
		public Rectangle<short> get_range ()
		{
			return range;
		}

		private Animation animation = new Animation ();
		private Rectangle<short> range = new Rectangle<short> ();
		private byte firstframe;
		private bool displayed;
	}
}


#if USE_NX
#endif