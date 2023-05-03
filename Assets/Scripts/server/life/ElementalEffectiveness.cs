using System.Collections.Generic;

/*
	This file is part of the OdinMS Maple Story Server
    Copyright (C) 2008 Patrick Huy <patrick.huy@frz.cc>
		       Matthias Butz <matze@odinms.de>
		       Jan Christian Meyer <vimes@odinms.de>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation version 3 as published by
    the Free Software Foundation. You may not use, modify or distribute
    this program under any other version of the GNU Affero General Public
    License.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace server.life
{
	public sealed class ElementalEffectiveness
	{
		public static readonly ElementalEffectiveness NORMAL = new ElementalEffectiveness("NORMAL", InnerEnum.NORMAL);
		public static readonly ElementalEffectiveness IMMUNE = new ElementalEffectiveness("IMMUNE", InnerEnum.IMMUNE);
		public static readonly ElementalEffectiveness STRONG = new ElementalEffectiveness("STRONG", InnerEnum.STRONG);
		public static readonly ElementalEffectiveness WEAK = new ElementalEffectiveness("WEAK", InnerEnum.WEAK);
		public static readonly ElementalEffectiveness NEUTRAL = new ElementalEffectiveness("NEUTRAL", InnerEnum.NEUTRAL);

		private static readonly IList<ElementalEffectiveness> valueList = new List<ElementalEffectiveness>();

		static ElementalEffectiveness()
		{
			valueList.Add(NORMAL);
			valueList.Add(IMMUNE);
			valueList.Add(STRONG);
			valueList.Add(WEAK);
			valueList.Add(NEUTRAL);
		}

		public enum InnerEnum
		{
			NORMAL,
			IMMUNE,
			STRONG,
			WEAK,
			NEUTRAL
		}

		public readonly InnerEnum innerEnumValue;
		private readonly string nameValue;
		private readonly int ordinalValue;
		private static int nextOrdinal = 0;

		private ElementalEffectiveness(string name, InnerEnum innerEnum)
		{
			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		public static ElementalEffectiveness getByNumber(int num)
		{
			switch (num)
			{
				case 1:
					return IMMUNE;
				case 2:
					return STRONG;
				case 3:
					return WEAK;
				case 4:
					return NEUTRAL;
				default:
					throw new System.ArgumentException("Unkown effectiveness: " + num);
			}
		}

		public static IList<ElementalEffectiveness> values()
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

		public static ElementalEffectiveness valueOf(string name)
		{
			foreach (ElementalEffectiveness enumInstance in ElementalEffectiveness.valueList)
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
