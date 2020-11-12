using System.Collections.Generic;
using System.Linq;
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
	public class Charset
	{
		public enum Alignment
		{
			LEFT,
			CENTER,
			RIGHT
		}

		public Charset(WzObject src, Alignment alignment)
		{
			this.alignment =alignment;
			foreach (var sub in src)
			{
				string name = sub.Name;
				
				if (!sub.IsTexture () || string.IsNullOrEmpty(name))
				{
					continue;
				}

				var c = name.FirstOrDefault ();

				//sbyte c = name.GetEnumerator();

				if (c == '\\')
				{
					c = '/';
				}

				chars.TryAdd((sbyte)c,new Texture(sub),true);//to refresh?
			}
		}
		public Charset()
		{
			this.alignment = Charset.Alignment.LEFT;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(sbyte c, const DrawArgument& args) const
		public void draw(sbyte c, DrawArgument args)
		{
			foreach (var pair in chars)
			{
				pair.Value.draw(args);
			}
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short draw(const string& text, const DrawArgument& args) const
		public short draw(string text, DrawArgument args)
		{
			short shift = 0;
			short total = 0;

			switch (alignment)
			{
				case Charset.Alignment.CENTER:
				{
					foreach (sbyte c in text)
					{
						short width = getw(c);

						draw(c, args + new Point<short>(shift, 0));
						shift += (short)(width + 2);
						total += (short)width;
					}

					shift -= (short)(total / 2);
					break;
				}
				case Charset.Alignment.LEFT:
				{
					foreach (sbyte c in text)
					{
						draw(c, args + new Point<short>(shift, 0));
						shift += (short)(getw (c) + 1);
					}

					break;
				}
				case Charset.Alignment.RIGHT:
				{
					foreach (var c in text)
					{
						shift += getw((sbyte)c);
						draw((sbyte)c, args - new Point<short>(shift, 0));
					}
					break;
				}
			}

			return shift;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short draw(const string& text, short hspace, const DrawArgument& args) const
		public short draw(string text, short hspace, DrawArgument args)
		{
			uint length = (uint)text.Length;
			short shift = 0;

			switch (alignment)
			{
				case Charset.Alignment.CENTER:
				{
					shift -= (short)(hspace * (short)length / 2);
					break;
				}
				case Charset.Alignment.LEFT:
				{
					foreach (sbyte c in text)
					{
						draw(c, args + new Point<short>(shift, 0));

						shift += hspace;
					}

					break;
				}
				case Charset.Alignment.RIGHT:
				{
					foreach (sbyte c in text)
					{
						shift += hspace;

						draw(c, args - new Point<short>(shift, 0));
					}
					break;
				}
			}

			return shift;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short getw(sbyte c) const
		public short getw(sbyte c)
		{
			if (chars.TryGetValue (c, out var texture))
			{
				return texture.width ();
			}

			return 0;
		}

		private Dictionary<sbyte, Texture> chars = new Dictionary<sbyte, Texture>();
		private Alignment alignment;
	}
}
