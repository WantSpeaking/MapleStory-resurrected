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

		public virtual void Dispose()
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void apply_useeffects(Char& user) const = 0;
		public abstract void apply_useeffects( Char user);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void apply_actions(Char& user, Attack::Type type) const = 0;
		public abstract void apply_actions( Char user, Attack.Type type);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void apply_stats(const Char& user, Attack& attack) const = 0;
		public abstract void apply_stats(Char user, Attack attack);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void apply_hiteffects(const AttackUser& user, Mob& target) const = 0;
		public abstract void apply_hiteffects(AttackUser user, Mob target);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual Animation get_bullet(const Char& user, int bulletid) const = 0;
		public abstract Animation get_bullet(Char user, int bulletid);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual bool is_attack() const = 0;
		public abstract bool is_attack();
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual bool is_skill() const = 0;
		public abstract bool is_skill();
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual int get_id() const = 0;
		public abstract int get_id();

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual ForbidReason can_use(int level, Weapon::Type weapon, const Job& job, ushort hp, ushort mp, ushort bullets) const = 0;
		public abstract ForbidReason can_use(int level, Weapon.Type weapon, Job job, ushort hp, ushort mp, ushort bullets);
	}
}