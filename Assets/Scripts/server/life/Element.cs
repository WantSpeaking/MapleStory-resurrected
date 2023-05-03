using System.Collections.Generic;

namespace server.life
{
	public sealed class Element
	{
		public static readonly Element NEUTRAL = new Element("NEUTRAL", InnerEnum.NEUTRAL, 0);
		public static readonly Element PHYSICAL = new Element("PHYSICAL", InnerEnum.PHYSICAL, 1);
		public static readonly Element FIRE = new Element("FIRE", InnerEnum.FIRE, 2, true);
		public static readonly Element ICE = new Element("ICE", InnerEnum.ICE, 3, true);
		public static readonly Element LIGHTING = new Element("LIGHTING", InnerEnum.LIGHTING, 4);
		public static readonly Element POISON = new Element("POISON", InnerEnum.POISON, 5);
		public static readonly Element HOLY = new Element("HOLY", InnerEnum.HOLY, 6, true);
		public static readonly Element DARKNESS = new Element("DARKNESS", InnerEnum.DARKNESS, 7);

		private static readonly IList<Element> valueList = new List<Element>();

		static Element()
		{
			valueList.Add(NEUTRAL);
			valueList.Add(PHYSICAL);
			valueList.Add(FIRE);
			valueList.Add(ICE);
			valueList.Add(LIGHTING);
			valueList.Add(POISON);
			valueList.Add(HOLY);
			valueList.Add(DARKNESS);
		}

		public enum InnerEnum
		{
			NEUTRAL,
			PHYSICAL,
			FIRE,
			ICE,
			LIGHTING,
			POISON,
			HOLY,
			DARKNESS
		}

		public readonly InnerEnum innerEnumValue;
		private readonly string nameValue;
		private readonly int ordinalValue;
		private static int nextOrdinal = 0;

		private int value;
		private bool special = false;
		private Element(string name, InnerEnum innerEnum, int v)
		{
		this.value = v;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		private Element(string name, InnerEnum innerEnum, int v, bool special)
		{
		this.value = v;
		this.special = special;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		public bool Special
		{
			get
			{
			return special;
			}
		}

		public static Element getFromChar(char c)
		{
			switch (char.ToUpper(c))
			{
				case 'F':
					return FIRE;
				case 'I':
					return ICE;
				case 'L':
					return LIGHTING;
				case 'S':
					return POISON;
				case 'H':
					return HOLY;
				case 'D':
					return DARKNESS;
				case 'P':
					return NEUTRAL;
			}
			throw new System.ArgumentException("unknown elemnt char " + c);
		}

		public int Value
		{
			get
			{
			return value;
			}
		}

		public static IList<Element> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public override string ToString()
		{
			return nameValue;
		}

		public static Element valueOf(string name)
		{
			foreach (Element enumInstance in Element.valueList)
			{
				if (enumInstance.nameValue == name)
				{
					return enumInstance;
				}
			}
			throw new System.ArgumentException(name);
		}
	}

}
