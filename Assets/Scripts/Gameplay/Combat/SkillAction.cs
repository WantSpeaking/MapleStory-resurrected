using System.Collections.Generic;
using MapleLib.WzLib;

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
	public abstract class SkillAction : System.IDisposable
	{
		public virtual void Dispose()
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void apply(Char& target, Attack::Type atype) const = 0;
		public abstract void apply(ref Char target, Attack.Type atype);
	}

	public class NoAction : SkillAction
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(Char&, Attack::Type) const override
		public override void apply(ref Char UnnamedParameter1, Attack.Type UnnamedParameter2)
		{
		}
	}

	public class RegularAction : SkillAction
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(Char& target, Attack::Type atype) const override
		public override void apply(ref Char target, Attack.Type atype)
		{
			Weapon.Type weapontype = target.get_weapontype();
			bool degenerate;

			switch (weapontype)
			{
			case Weapon.Type.BOW:
			case Weapon.Type.CROSSBOW:
			case Weapon.Type.CLAW:
			case Weapon.Type.GUN:
				degenerate = atype != Attack.Type.RANGED;
				break;
			default:
				degenerate = false;
				break;
			}

			target.attack(degenerate);
		}
	}

	public class SingleAction : SkillAction
	{
		public SingleAction(WzObject src)
		{
			action = src["action"]["0"].Name;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(Char& target, Attack::Type atype) const override
		public override void apply(ref Char target, Attack.Type atype)
		{
			target.attack(action);
		}

		private string action;
	}

	public class TwoHandedAction : SkillAction
	{
		public TwoHandedAction(WzObject src)
		{
			actions[false] = src["action"]["0"].Name;
			actions[true] = src["action"]["1"].Name;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(Char& target, Attack::Type atype) const override
		public override void apply(ref Char target, Attack.Type atype)
		{
			bool twohanded = target.is_twohanded();
			string action = actions[twohanded];

			target.attack(action);
		}

		private BoolPair<string> actions = new BoolPair<string>();
	}

	public class ByLevelAction : SkillAction
	{
		public ByLevelAction(WzObject src, int id)
		{
			foreach (var sub in src["level"])
			{
				int level = string_conversion.or_zero<int>(sub.Name);
				actions[level] = sub["action"].Name;
			}

			skillid = id;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(Char& target, Attack::Type atype) const override
		public override void apply(ref Char target, Attack.Type atype)
		{
			int level = target.get_skilllevel(skillid);
			if (actions.TryGetValue (level,out var action))
			{
				target.attack(action);
			}
			
			/*var iter = actions.find(level);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			if (iter != actions.end())
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
				target.attack(iter.second);
			}*/
		}

		private SortedDictionary<int, string> actions = new SortedDictionary<int, string>();
		private int skillid;
	}
}

