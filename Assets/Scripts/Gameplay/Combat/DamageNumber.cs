#define USE_NX

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
	public class DamageNumber
	{
		public const uint NUM_TYPES = 3;

		public enum Type
		{
			NORMAL,
			CRITICAL,
			TOPLAYER
		}

		public DamageNumber(Type t, int damage, short starty, short x = 0)
		{
			type = t;

			if (damage > 0)
			{
				miss = false;

				string number = Convert.ToString(damage);
				firstnum = (sbyte)number[0];

				if (number.Length > 1)
				{
					restnum = number.Substring(1);
					multiple = true;
				}
				else
				{
					restnum = "";
					multiple = false;
				}

				short total = getadvance(firstnum, true);

				for (int i = 0; i < restnum.Length; i++)
				{
					sbyte c = (sbyte)restnum[i];
					short advance;

					if (i < restnum.Length - 1)
					{
						sbyte n = (sbyte)restnum[i + 1];
						advance = (short)((getadvance(c, false) + getadvance(n, false)) / 2);
					}
					else
					{
						advance = getadvance(c, false);
					}

					total += advance;
				}

				shift = (short)(total / 2);
			}
			else
			{
				shift = (short)(charsets[(int)type][true].getw((sbyte)'M') / 2);
				miss = true;
			}

			moveobj.set_x(x);
			moveobj.set_y(starty);
			moveobj.vspeed = -0.25;
			opacity.set(1.5f);
		}
		public DamageNumber()
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(double viewx, double viewy, float alpha) const
		public void draw(double viewx, double viewy, float alpha)
		{
			Point<short> absolute = moveobj.get_absolute(viewx, viewy, alpha);
			Point<short> position = absolute - new Point<short>(0, shift);
			float interopc = opacity.get(alpha);

			if (miss)
			{
				charsets[(int)type][true].draw((sbyte)'M',new DrawArgument (position, interopc));
			}
			else
			{
				var tempcha = charsets?[(int)type]?[false];if(tempcha == null) return;
				
				charsets?[(int)type]?[false]?.draw(firstnum, new DrawArgument ( position, interopc));//todo why damageNumber is null

				if (multiple)
				{
					short first_advance = getadvance(firstnum, true);
					position.shift_x(first_advance);

					for (int i = 0; i < restnum.Length; i++)
					{
						sbyte c = (sbyte)restnum[i];
						Point<short> yshift = new Point<short>(0, (short)((i % 2)>0 ? -2 : 2));
						charsets[(int)type][true].draw (c, new DrawArgument (position + yshift, interopc));

						short advance;

						if (i < restnum.Length - 1)
						{
							sbyte n = (sbyte)restnum[i + 1];
							short c_advance = getadvance(c, false);
							short n_advance = getadvance(n, false);
							advance = (short)((c_advance + n_advance) / 2);
						}
						else
						{
							advance = getadvance(c, false);
						}

						position.shift_x(advance);
					}
				}
			}
		}
		public void set_x(short headx)
		{
			moveobj.set_x(headx);
		}
		public bool update()
		{
			moveobj.move();

			const float FADE_STEP = Constants.TIMESTEP * 1.0f / FADE_TIME;
			opacity -= FADE_STEP;

			return opacity.last() <= 0.0f;
		}

		public static short rowheight(bool critical)
		{
			return (short)(critical ? 36 : 30);
		}
		public static void init()
		{
			charsets[(int)DamageNumber.Type.NORMAL].set(false, new Charset( nl.nx.wzFile_effect["BasicEff.img"]["NoRed1"], Charset.Alignment.LEFT));
			charsets[(int)DamageNumber.Type.NORMAL].set(true, new Charset(nl.nx.wzFile_effect["BasicEff.img"]["NoRed0"], Charset.Alignment.LEFT));
			charsets[(int)DamageNumber.Type.CRITICAL].set(false, new Charset(nl.nx.wzFile_effect["BasicEff.img"]["NoCri1"], Charset.Alignment.LEFT));
			charsets[(int)DamageNumber.Type.CRITICAL].set(true, new Charset(nl.nx.wzFile_effect["BasicEff.img"]["NoCri0"], Charset.Alignment.LEFT));
			charsets[(int)DamageNumber.Type.TOPLAYER].set(false, new Charset(nl.nx.wzFile_effect["BasicEff.img"]["NoViolet1"], Charset.Alignment.LEFT));
			charsets[(int)DamageNumber.Type.TOPLAYER].set(true, new Charset(nl.nx.wzFile_effect["BasicEff.img"]["NoViolet0"], Charset.Alignment.LEFT));
		}

		const uint LENGTH = 10;

		static readonly short[] advances = {24, 20, 22, 22, 24, 23, 24, 22, 24, 24};
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short getadvance(sbyte c, bool first) const
		private short getadvance(sbyte c, bool first)
		{
			uint index = (uint)(c - 48);

			if (index < LENGTH)
			{
				short advance = advances[index];

				switch (type)
				{
				case DamageNumber.Type.CRITICAL:
					if (first)
					{
						advance += 8;
					}
					else
					{
						advance += 4;
					}

					break;
				default:
					if (first)
					{
						advance += 2;
					}

					break;
				}

				return advance;
			}
			else
			{
				return 0;
			}
		}

		private const ushort FADE_TIME = 500;

		private Type type;
		private bool miss;
		private bool multiple;
		private sbyte firstnum;
		private string restnum;
		private short shift;
		private MovingObject moveobj = new MovingObject();
		private Linear_float opacity = new Linear_float();

		private static BoolPair<Charset>[] charsets =new BoolPair<Charset>[NUM_TYPES];
	}
}

#if USE_NX
#endif
