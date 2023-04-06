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



using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;

namespace ms
{
	/// <summary>
	/// Base class for attacks and buffs
	/// </summary>
	public class SpecialMove : System.IDisposable
	{
		public enum ForbidReason
		{
			FBR_NONE,
			FBR_WEAPONTYPE,
			FBR_HPCOST,
			FBR_MPCOST,
			FBR_BULLETCOST,
			FBR_COOLDOWN,
			FBR_OTHER,
			FBR_SkillLevel_LessThan0_or_GreateThanMaxLevel,
			FBR_JobCanNotUseThisSkill,
			FBR_NoWeapon,
			FBR_MoveIsNull,
			FBR_IsProningNowCantSkill,
			FBR_IsForbid,
			FBR_IsLADDER_Rope_Sit_Now_CantAttack,
		}

		public virtual void Dispose ()
		{
		}

		public virtual void apply_useeffects (Char user) { }
		public virtual void apply_prepareEffect (Char user) { }
		public virtual void apply_keydownEffect (Char user) { }
		public virtual void apply_keydownendEffect (Char user) { }
		public virtual void apply_actions (Char user, Attack.Type type) { }
		public virtual SkillAction get_action (Char user) { return null; }
		public virtual void apply_stats (Char user, Attack attack) { }
		public virtual void apply_hiteffects (AttackUser user, Mob target) { }
		public virtual Animation get_bullet (Char user, int bulletid) { return null; }

		public virtual bool is_attack () { return false; }
		public virtual bool is_skill () { return false; }
        public virtual bool is_teleportSkill() 
		{ 
			if(get_id() == (int)SkillId.Id.TELEPORT_FP || get_id() == (int)SkillId.Id.IL_TELEPORT || get_id() == (int)SkillId.Id.PRIEST_TELEPORT)
			return true; 
			return false;
        }

        public virtual int get_id () { return 0; }

		public virtual bool has_skillPrepareEffect () { return false; }
		public virtual ForbidReason can_use (int level, Weapon.Type weapon, Job job, ushort hp, ushort mp, ushort bullets) { return ForbidReason.FBR_NONE; }

		protected BehaviourTree graph;
		public virtual BehaviourTree BTree => graph ??= ResourcesManager.Instance.GetSkillBTree (get_id ().ToString ());

	}
}