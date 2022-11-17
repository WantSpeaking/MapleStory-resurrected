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
namespace server.quest
{
	/// 
	/// <summary>
	/// @author Matze
	/// </summary>
	public sealed class MapleQuestActionType
	{
		public static readonly MapleQuestActionType UNDEFINED = new MapleQuestActionType("UNDEFINED", InnerEnum.UNDEFINED, -1);
		public static readonly MapleQuestActionType EXP = new MapleQuestActionType("EXP", InnerEnum.EXP, 0);
		public static readonly MapleQuestActionType ITEM = new MapleQuestActionType("ITEM", InnerEnum.ITEM, 1);
		public static readonly MapleQuestActionType NEXTQUEST = new MapleQuestActionType("NEXTQUEST", InnerEnum.NEXTQUEST, 2);
		public static readonly MapleQuestActionType MESO = new MapleQuestActionType("MESO", InnerEnum.MESO, 3);
		public static readonly MapleQuestActionType QUEST = new MapleQuestActionType("QUEST", InnerEnum.QUEST, 4);
		public static readonly MapleQuestActionType SKILL = new MapleQuestActionType("SKILL", InnerEnum.SKILL, 5);
		public static readonly MapleQuestActionType FAME = new MapleQuestActionType("FAME", InnerEnum.FAME, 6);
		public static readonly MapleQuestActionType BUFF = new MapleQuestActionType("BUFF", InnerEnum.BUFF, 7);
		public static readonly MapleQuestActionType PETSKILL = new MapleQuestActionType("PETSKILL", InnerEnum.PETSKILL, 8);
		public static readonly MapleQuestActionType YES = new MapleQuestActionType("YES", InnerEnum.YES, 9);
		public static readonly MapleQuestActionType NO = new MapleQuestActionType("NO", InnerEnum.NO, 10);
		public static readonly MapleQuestActionType NPC = new MapleQuestActionType("NPC", InnerEnum.NPC, 11);
		public static readonly MapleQuestActionType MIN_LEVEL = new MapleQuestActionType("MIN_LEVEL", InnerEnum.MIN_LEVEL, 12);
		public static readonly MapleQuestActionType NORMAL_AUTO_START = new MapleQuestActionType("NORMAL_AUTO_START", InnerEnum.NORMAL_AUTO_START, 13);
		public static readonly MapleQuestActionType PETTAMENESS = new MapleQuestActionType("PETTAMENESS", InnerEnum.PETTAMENESS, 14);
		public static readonly MapleQuestActionType PETSPEED = new MapleQuestActionType("PETSPEED", InnerEnum.PETSPEED, 15);
		public static readonly MapleQuestActionType INFO = new MapleQuestActionType("INFO", InnerEnum.INFO, 16);
		public static readonly MapleQuestActionType ZERO = new MapleQuestActionType("ZERO", InnerEnum.ZERO, 16);

		private static readonly IList<MapleQuestActionType> valueList = new List<MapleQuestActionType>();

		static MapleQuestActionType()
		{
			valueList.Add(UNDEFINED);
			valueList.Add(EXP);
			valueList.Add(ITEM);
			valueList.Add(NEXTQUEST);
			valueList.Add(MESO);
			valueList.Add(QUEST);
			valueList.Add(SKILL);
			valueList.Add(FAME);
			valueList.Add(BUFF);
			valueList.Add(PETSKILL);
			valueList.Add(YES);
			valueList.Add(NO);
			valueList.Add(NPC);
			valueList.Add(MIN_LEVEL);
			valueList.Add(NORMAL_AUTO_START);
			valueList.Add(PETTAMENESS);
			valueList.Add(PETSPEED);
			valueList.Add(INFO);
			valueList.Add(ZERO);
		}

		public enum InnerEnum
		{
			UNDEFINED,
			EXP,
			ITEM,
			NEXTQUEST,
			MESO,
			QUEST,
			SKILL,
			FAME,
			BUFF,
			PETSKILL,
			YES,
			NO,
			NPC,
			MIN_LEVEL,
			NORMAL_AUTO_START,
			PETTAMENESS,
			PETSPEED,
			INFO,
			ZERO
		}

		public readonly InnerEnum innerEnumValue;
		private readonly string nameValue;
		private readonly int ordinalValue;
		private static int nextOrdinal = 0;
		internal readonly sbyte type;

		private MapleQuestActionType(string name, InnerEnum innerEnum, int type)
		{
			this.type = (sbyte) type;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		public static MapleQuestActionType getByWZName(string name)
		{
			if (name.Equals("exp"))
			{
				return EXP;
			}
			else if (name.Equals("money"))
			{
				return MESO;
			}
			else if (name.Equals("item"))
			{
				return ITEM;
			}
			else if (name.Equals("skill"))
			{
				return SKILL;
			}
			else if (name.Equals("nextQuest"))
			{
				return NEXTQUEST;
			}
			else if (name.Equals("pop"))
			{
				return FAME;
			}
			else if (name.Equals("buffItemID"))
			{
				return BUFF;
			}
			else if (name.Equals("petskill"))
			{
				return PETSKILL;
			}
			else if (name.Equals("no"))
			{
				return NO;
			}
			else if (name.Equals("yes"))
			{
				return YES;
			}
			else if (name.Equals("npc"))
			{
				return NPC;
			}
			else if (name.Equals("lvmin"))
			{
				return MIN_LEVEL;
			}
			else if (name.Equals("normalAutoStart"))
			{
				return NORMAL_AUTO_START;
			}
			else if (name.Equals("pettameness"))
			{
				return PETTAMENESS;
			}
			else if (name.Equals("petspeed"))
			{
				return PETSPEED;
			}
			else if (name.Equals("info"))
			{
				return INFO;
			}
			else if (name.Equals("0"))
			{
				return ZERO;
			}
			else
			{
				return UNDEFINED;
			}
		}

		public static IList<MapleQuestActionType> values()
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

		public static MapleQuestActionType valueOf(string name)
		{
			foreach (MapleQuestActionType enumInstance in MapleQuestActionType.valueList)
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