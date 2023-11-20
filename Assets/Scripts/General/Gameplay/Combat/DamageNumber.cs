#define USE_NX

using System;






namespace ms
{
	public class DamageNumber:IDisposable
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
            //AppDebug.Log($"DamageNumber:{damage}");

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

		public void draw(double viewx, double viewy, float alpha)
		{
			Point_short absolute = moveobj.get_absolute(viewx, viewy, alpha);
			Point_short position = absolute - new Point_short(0, shift);
			float interopc = opacity.get(alpha);

			if (miss)
			{
				//charsets[(int)type][true].reset();

                charsets[(int)type][true].draw((sbyte)'M',new DrawArgument (position, interopc));
			}
			else
			{
				//charsets?[(int)type]?[false].reset ();
				
				var tempcha = charsets?[(int)type]?[false];
				if(tempcha == null) return;
				
				charsets?[(int)type]?[false]?.draw(firstnum, new DrawArgument ( position, interopc));//todo 2 why damageNumber is null
				/*position.shift_x(25);
				for (int i = 0; i < restnum.Length; i++)
				{
					sbyte c = (sbyte)restnum[i];
					charsets?[(int)type]?[false]?.draw(c, new DrawArgument ( position, interopc));
					position.shift_x(25); 
				}*/

				if (multiple)
				{
					short first_advance = getadvance(firstnum, true);
					position.shift_x(first_advance);

					for (int i = 0; i < restnum.Length; i++)
					{
						sbyte c = (sbyte)restnum[i];
						Point_short yshift = new Point_short(0, (short)((i % 2)>0 ? -2 : 2));
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
			for (int i = 0; i < charsets.Length; i++)
			{
				charsets[i] = new BoolPairNew<Charset> ();
			}

			var BasicEff = ms.wz.wzProvider_effect["BasicEff.img"];

			charsets[(int)DamageNumber.Type.NORMAL].set(false, new Charset(BasicEff["NoRed1"], Charset.Alignment.LEFT));
			charsets[(int)DamageNumber.Type.NORMAL].set(true, new Charset(BasicEff["NoRed0"], Charset.Alignment.LEFT));
			charsets[(int)DamageNumber.Type.CRITICAL].set(false, new Charset(BasicEff["NoCri1"], Charset.Alignment.LEFT));
			charsets[(int)DamageNumber.Type.CRITICAL].set(true, new Charset(BasicEff["NoCri0"], Charset.Alignment.LEFT));
			charsets[(int)DamageNumber.Type.TOPLAYER].set(false, new Charset(BasicEff["NoViolet1"], Charset.Alignment.LEFT));
			charsets[(int)DamageNumber.Type.TOPLAYER].set(true, new Charset(BasicEff["NoViolet0"], Charset.Alignment.LEFT));
		}

		const uint LENGTH = 10;

		static readonly short[] advances = {24, 20, 22, 22, 24, 23, 24, 22, 24, 24};
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

        public void Dispose()
        {
			foreach (var item in charsets)
			{
				item[true]?.reset();
                item[false]?.reset();
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

		private static BoolPairNew<Charset>[] charsets = new BoolPairNew<Charset>[NUM_TYPES];
	}
}