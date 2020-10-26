using System.Collections.Generic;
using System.Linq;
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
	// Interface for hit effects, animations applied to a mob for each hit.
	public abstract class SkillHitEffect : System.IDisposable
	{
		public virtual void Dispose ()
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void apply(const AttackUser& user, Mob& target) const = 0;
		public abstract void apply (AttackUser user, Mob target);

		protected class Effect
		{
			public Effect (WzObject src)
			{
				animation = src;
				pos = (sbyte)src["pos"];
				z = (sbyte)src["z"];
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(Mob& target, bool flip) const
			public void apply (Mob target, bool flip)
			{
				target.show_effect (animation, pos, z, flip);
			}

			private Animation animation;
			private sbyte pos;
			private sbyte z;
		}
	}

	// No animation
	public class NoHitEffect : SkillHitEffect
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(const AttackUser&, Mob&) const override
		public override void apply (AttackUser UnnamedParameter1, Mob UnnamedParameter2)
		{
		}
	}

	// A single animation
	public class SingleHitEffect : SkillHitEffect
	{
		public SingleHitEffect (WzObject src)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.effect = new ms.SkillHitEffect.Effect(src["hit"]["0"]);
			this.effect = new ms.SkillHitEffect.Effect (src["hit"]["0"]);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(const AttackUser& user, Mob& target) const override
		public override void apply (AttackUser user, Mob target)
		{
			effect.apply (target, user.flip);
		}

		private Effect effect;
	}

	// The animation changes depending on the weapon used
	public class TwoHandedHitEffect : SkillHitEffect
	{
		public TwoHandedHitEffect (WzObject src)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.effects = new ms.BoolPair<Effect>(src["hit"]["0"], src["hit"]["1"]);
			this.effects = new ms.BoolPair<Effect> (new Effect (src["hit"]["0"]), new Effect (src["hit"]["1"]));
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(const AttackUser& user, Mob& target) const override
		public override void apply (AttackUser user, Mob target)
		{
			effects[user.secondweapon].apply (target, user.flip);
		}

		private BoolPair<Effect> effects = new BoolPair<Effect> ();
	}

	// The animation changes with the character level
	public class ByLevelHitEffect : SkillHitEffect
	{
		public ByLevelHitEffect (WzObject src)
		{
			foreach (var sub in src["CharLevel"])
			{
				ushort level = string_conversion.or_zero<ushort> (sub.Name);
				effects.Add (level, new Effect (sub["hit"]["0"]));
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(const AttackUser& user, Mob& target) const override
		public override void apply (AttackUser user, Mob target)
		{
			effects.FirstOrDefault (pair => user.level > pair.Key).Value?.apply (target, user.flip);

			/*if (effects.Count == 0)
			{
				return;
			}

			var iter = effects.GetEnumerator();
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			for (; iter != effects.end() && user.level > iter.first; ++iter)
			{
			}

			if (iter != effects.GetEnumerator())
			{
				iter--;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			iter.second.apply(target, user.flip);*/
		}

		private SortedDictionary<ushort, Effect> effects = new SortedDictionary<ushort, Effect> ();
	}

	// The animation changes with the character level and weapon used
	public class ByLevelTwoHHitEffect : SkillHitEffect
	{
		public ByLevelTwoHHitEffect (WzObject src)
		{
			foreach (var sub in src["CharLevel"])
			{
				var level = string_conversion.or_zero<ushort> (sub.Name);
				effects.Add (level, new BoolPair<Effect> (new Effect (sub["hit"]["0"]), new Effect (sub["hit"]["1"])));
				//effects.emplace (piecewise_construct, forward_as_tuple (level), forward_as_tuple (sub["hit"]["0"], sub["hit"]["1"]));
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(const AttackUser& user, Mob& target) const override
		public override void apply (AttackUser user, Mob target)
		{
			if (effects.Count == 0)
			{
				return;
			}

			effects.FirstOrDefault (pair => user.level > pair.Key).Value?[user.secondweapon].apply (target, user.flip);

			/*var iter = effects.GetEnumerator ();
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			for (; iter != effects.end () && user.level > iter.first; ++iter)
			{
			}

			if (iter != effects.GetEnumerator ())
			{
				iter--;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			iter.second[user.secondweapon].apply (target, user.flip);*/
		}

		private SortedDictionary<ushort, BoolPair<Effect>> effects = new SortedDictionary<ushort, BoolPair<Effect>> ();
	}

	// The animation changes with the skill level
	public class BySkillLevelHitEffect : SkillHitEffect
	{
		public BySkillLevelHitEffect (WzObject src)
		{
			foreach (var sub in src["level"])
			{
				var level = string_conversion.or_zero<int> (sub.Name);
				effects.Add (level, new Effect (sub["hit"]["0"]));
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(const AttackUser& user, Mob& target) const override
		public override void apply (AttackUser user, Mob target)
		{
			if (effects.TryGetValue (user.skilllevel,out var effect))
			{
				effect.apply (target, user.flip);
			}
			/*var iter = effects.find (user.skilllevel);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			if (iter != effects.end ())
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
				iter.second.apply (target, user.flip);
			}*/
		}

		private SortedDictionary<int, Effect> effects = new SortedDictionary<int, Effect> ();
	}
}