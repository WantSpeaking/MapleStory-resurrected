using System.Collections.Generic;

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
	public class Buffstat
	{
		public enum Id
		{
			NONE,
			MORPH,
			RECOVERY,
			MAPLE_WARRIOR,
			STANCE,
			SHARP_EYES,
			MANA_REFLECTION,
			SHADOW_CLAW,
			INFINITY_,
			HOLY_SHIELD,
			HAMSTRING,
			BLIND,
			CONCENTRATE,
			ECHO_OF_HERO,
			GHOST_MORPH,
			AURA,
			CONFUSE,
			BERSERK_FURY,
			DIVINE_BODY,
			SPARK,
			FINALATTACK,
			BATTLESHIP,
			WATK,
			WDEF,
			MATK,
			MDEF,
			ACC,
			AVOID,
			HANDS,
			SHOWDASH,
			SPEED,
			JUMP,
			MAGIC_GUARD,
			DARKSIGHT,
			BOOSTER,
			POWERGUARD,
			HYPERBODYHP,
			HYPERBODYMP,
			INVINCIBLE,
			SOULARROW,
			STUN,
			POISON,
			SEAL,
			DARKNESS,
			COMBO,
			SUMMON,
			WK_CHARGE,
			DRAGONBLOOD,
			HOLY_SYMBOL,
			MESOUP,
			SHADOWPARTNER,
			PICKPOCKET,
			PUPPET,
			MESOGUARD,
			WEAKEN,
			DASH,
			DASH2,
			ELEMENTAL_RESET,
			ARAN_COMBO,
			COMBO_DRAIN,
			COMBO_BARRIER,
			BODY_PRESSURE,
			SMART_KNOCKBACK,
			PYRAMID_PQ,
			ENERGY_CHARGE,
			MONSTER_RIDING,
			HOMING_BEACON,
			SPEED_INFUSION,
		}

		public static Dictionary<Id, long> first_codes = new Dictionary<Id, long> ()
		{
			{Id.DASH2, 0x8000000000000L},
			{Id.DASH, 0x10000000000000L},
			{Id.ELEMENTAL_RESET, 0x200000000L},
			{Id.ARAN_COMBO, 0x1000000000L},
			{Id.COMBO_DRAIN, 0x2000000000L},
			{Id.COMBO_BARRIER, 0x4000000000L},
			{Id.BODY_PRESSURE, 0x8000000000L},
			{Id.SMART_KNOCKBACK, 0x10000000000L},
			{Id.PYRAMID_PQ, 0x20000000000L},
			{Id.ENERGY_CHARGE, 0x4000000000000L},
			{Id.MONSTER_RIDING, 0x20000000000000L},
			{Id.HOMING_BEACON, 0x80000000000000L},
			{Id.SPEED_INFUSION, 0x100000000000000L}
		};

		public static Dictionary<Id, long> second_codes = new Dictionary<Id, long> ()
		{
			{Id.NONE, 0x0},
			{Id.MORPH, 0x2},
			{Id.RECOVERY, 0x4},
			{Id.MAPLE_WARRIOR, 0x8},
			{Id.STANCE, 0x10},
			{Id.SHARP_EYES, 0x20},
			{Id.MANA_REFLECTION, 0x40},
			{Id.SHADOW_CLAW, 0x100},
			{Id.INFINITY_, 0x200},
			{Id.HOLY_SHIELD, 0x400},
			{Id.HAMSTRING, 0x800},
			{Id.BLIND, 0x1000},
			{Id.CONCENTRATE, 0x2000},
			{Id.ECHO_OF_HERO, 0x8000},
			{Id.GHOST_MORPH, 0x20000},
			{Id.AURA, 0x40000},
			{Id.CONFUSE, 0x80000},
			{Id.BERSERK_FURY, 0x8000000},
			{Id.DIVINE_BODY, 0x10000000},
			{Id.SPARK, 0x20000000L},
			{Id.FINALATTACK, 0x80000000L},
			{Id.BATTLESHIP, 0xA00000040L},
			{Id.WATK, 0x100000000L},
			{Id.WDEF, 0x200000000L},
			{Id.MATK, 0x400000000L},
			{Id.MDEF, 0x800000000L},
			{Id.ACC, 0x1000000000L},
			{Id.AVOID, 0x2000000000L},
			{Id.HANDS, 0x4000000000L},
			{Id.SHOWDASH, 0x4000000000L},
			{Id.SPEED, 0x8000000000L},
			{Id.JUMP, 0x10000000000L},
			{Id.MAGIC_GUARD, 0x20000000000L},
			{Id.DARKSIGHT, 0x40000000000L},
			{Id.BOOSTER, 0x80000000000L},
			{Id.POWERGUARD, 0x100000000000L},
			{Id.HYPERBODYHP, 0x200000000000L},
			{Id.HYPERBODYMP, 0x400000000000L},
			{Id.INVINCIBLE, 0x800000000000L},
			{Id.SOULARROW, 0x1000000000000L},
			{Id.STUN, 0x2000000000000L},
			{Id.POISON, 0x4000000000000L},
			{Id.SEAL, 0x8000000000000L},
			{Id.DARKNESS, 0x10000000000000L},
			{Id.COMBO, 0x20000000000000L},
			{Id.SUMMON, 0x20000000000000L},
			{Id.WK_CHARGE, 0x40000000000000L},
			{Id.DRAGONBLOOD, 0x80000000000000L},
			{Id.HOLY_SYMBOL, 0x100000000000000L},
			{Id.MESOUP, 0x200000000000000L},
			{Id.SHADOWPARTNER, 0x400000000000000L},
			{Id.PICKPOCKET, 0x800000000000000L},
			{Id.PUPPET, 0x800000000000000L},
			{Id.MESOGUARD, 0x1000000000000000L},
			{Id.WEAKEN, 0x4000000000000000L}
		};
	}

	public struct Buff
	{
		public Buffstat.Id stat;
		public short value;
		public int skillid;
		public int duration;

		public Buff (Buffstat.Id stat, short value, int skillid, int duration)
		{
			this.stat = stat;
			this.value = value;
			this.skillid = skillid;
			this.duration = duration;
		}
	}
}