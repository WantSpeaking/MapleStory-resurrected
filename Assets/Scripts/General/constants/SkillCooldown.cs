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
using System.Collections.Generic;
using constants.skills;

namespace ms
{
	/// 
	/// <summary>
	/// @author BubblesDev
	/// </summary>
	public class SkillCooldown
	{
		public static Dictionary<int, float> SkillCooldowns = new Dictionary<int, float>
		{
			{Bowmaster.HURRICANE, 0.1f},
		};

		public const int DOUBLE_SWING = 21000002;
		public const int TRIPLE_SWING = 21100001;
		public const int COMBO_ABILITY = 21000000;
		public const int COMBAT_STEP = 21001001;
		public const int POLEARM_BOOSTER = 21001003;
		public const int MAPLE_WARRIOR = 21121000;
		public const int FREEZE_STANDING = 21121003;
		public const int SNOW_CHARGE = 21111005;
		public const int HEROS_WILL = 21121008;
		public const int HIGH_DEFENSE = 21120004;
		public const int BODY_PRESSURE = 21101003;
		public const int COMBO_DRAIN = 21100005;
		public const int COMBO_SMASH = 21100004;
		public const int COMBO_FENRIR = 21110004;
		public const int COMBO_CRITICAL = 21110000;
		public const int FULL_SWING = 21110002;
		public const int ROLLING_SPIN = 21110006;
		public const int HIDDEN_FULL_DOUBLE = 21110007;
		public const int HIDDEN_FULL_TRIPLE = 21110008;
		public const int SMART_KNOCKBACK = 21111001;
		public const int OVER_SWING = 21120002;
		public const int COMBO_TEMPEST = 21120006;
		public const int COMBO_BARRIER = 21120007;
		public const int HIDDEN_OVER_DOUBLE = 21120009;
		public const int HIDDEN_OVER_TRIPLE = 21120010;
		public const int HIGH_MASTERY = 21120001;
	}
	public class ForceSkill
	{
		public static List<int> SkillCooldowns = new List<int>
		{
			Bowmaster.HURRICANE,
		};

		public static bool IsForce (int skillId)
		{
			return SkillCooldowns.Contains (skillId);
		}
		public const int DOUBLE_SWING = 21000002;
		public const int TRIPLE_SWING = 21100001;
		public const int COMBO_ABILITY = 21000000;
		public const int COMBAT_STEP = 21001001;
		public const int POLEARM_BOOSTER = 21001003;
		public const int MAPLE_WARRIOR = 21121000;
		public const int FREEZE_STANDING = 21121003;
		public const int SNOW_CHARGE = 21111005;
		public const int HEROS_WILL = 21121008;
		public const int HIGH_DEFENSE = 21120004;
		public const int BODY_PRESSURE = 21101003;
		public const int COMBO_DRAIN = 21100005;
		public const int COMBO_SMASH = 21100004;
		public const int COMBO_FENRIR = 21110004;
		public const int COMBO_CRITICAL = 21110000;
		public const int FULL_SWING = 21110002;
		public const int ROLLING_SPIN = 21110006;
		public const int HIDDEN_FULL_DOUBLE = 21110007;
		public const int HIDDEN_FULL_TRIPLE = 21110008;
		public const int SMART_KNOCKBACK = 21111001;
		public const int OVER_SWING = 21120002;
		public const int COMBO_TEMPEST = 21120006;
		public const int COMBO_BARRIER = 21120007;
		public const int HIDDEN_OVER_DOUBLE = 21120009;
		public const int HIDDEN_OVER_TRIPLE = 21120010;
		public const int HIGH_MASTERY = 21120001;
	}

}