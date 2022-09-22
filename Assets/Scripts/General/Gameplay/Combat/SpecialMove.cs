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
	public abstract class SpecialMove : System.IDisposable
	{
		public enum ForbidReason
		{
			FBR_NONE,
			FBR_WEAPONTYPE,
			FBR_HPCOST,
			FBR_MPCOST,
			FBR_BULLETCOST,
			FBR_COOLDOWN,
			FBR_OTHER
		}

		public virtual void Dispose ()
		{
		}

		public abstract void apply_useeffects (Char user);
		public abstract void apply_prepareEffect (Char user);
		public abstract void apply_keydownEffect (Char user);
		public abstract void apply_keydownendEffect (Char user);
		public abstract void apply_actions (Char user, Attack.Type type);
		public abstract SkillAction get_action (Char user);
		public abstract void apply_stats (Char user, Attack attack);
		public abstract void apply_hiteffects (AttackUser user, Mob target);
		public abstract Animation get_bullet (Char user, int bulletid);

		public abstract bool is_attack ();
		public abstract bool is_skill ();
		public abstract int get_id ();

		public abstract bool has_skillPrepareEffect ();
		public abstract ForbidReason can_use (int level, Weapon.Type weapon, Job job, ushort hp, ushort mp, ushort bullets);

		protected BehaviourTree graph;
		public BehaviourTree BTree => graph ??= ResourcesManager.Instance.GetSkillBTree (get_id ().ToString ());

	}
}