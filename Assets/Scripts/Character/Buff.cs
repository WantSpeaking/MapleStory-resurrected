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
	namespace Buffstat
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
			LENGTH
		}
	}

	public struct Buff
	{
		public Buffstat.Id stat;
		public short value;
		public int skillid;
		public int duration;

		public Buff(Buffstat.Id stat, short value, int skillid, int duration)
		{
			this.stat = stat;
			this.value = value;
			this.skillid = skillid;
			this.duration = duration;
		}
	}
}
