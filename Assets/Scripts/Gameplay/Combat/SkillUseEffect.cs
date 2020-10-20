using System;
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
	// Interface for skill effects
	public abstract class SkillUseEffect : System.IDisposable
	{
		public virtual void Dispose ()
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void apply(Char& target) const = 0;
		public abstract void apply (ref Char target);

		protected class Effect
		{
			public Effect (WzObject src)
			{
				animation = src;
				z = (sbyte)src["z"];
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(Char& target) const
			public void apply (ref Char target)
			{
				target.show_attack_effect (animation, z);
			}

			private Animation animation = new Animation ();
			private sbyte z;
		}
	}

	// No animation
	public class NoUseEffect : SkillUseEffect
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(Char&) const override
		public override void apply (ref Char UnnamedParameter1)
		{
		}
	}

	// An effect which displays an animation over the Character's position
	public class SingleUseEffect : SkillUseEffect
	{
		public SingleUseEffect (WzObject src)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.effect = new ms.SkillUseEffect.Effect(src["effect"]);
			this.effect = new ms.SkillUseEffect.Effect (src["effect"]);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(Char& target) const override
		public override void apply (ref Char target)
		{
			effect.apply (ref target);
		}

		private Effect effect;
	}

	// An effect which displays an animation over the Character's position
	// The effect changes based on whether the Character uses a two-handed weapon
	public class TwoHandedUseEffect : SkillUseEffect
	{
		public TwoHandedUseEffect (WzObject src)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.effects = new ms.BoolPair<Effect>(src["effect"]["0"], src["effect"]["1"]);
			this.effects = new ms.BoolPair<Effect> (new Effect (src["effect"]["0"]), new Effect (src["effect"]["1"]));
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(Char& target) const override
		public override void apply (ref Char target)
		{
			bool twohanded = target.is_twohanded ();
			effects[twohanded].apply (ref target);
		}

		private BoolPair<Effect> effects = new BoolPair<Effect> ();
	}

	// An effect which displays multiple animations over the Character's position
	public class MultiUseEffect : SkillUseEffect
	{
		public MultiUseEffect (WzObject src)
		{
			sbyte no = -1;
			WzObject sub = src["effect"];

			while (sub != null)
			{
				effects.Add (new Effect (sub));

				no++;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: sub = src["effect" + std::to_string(no)];
				sub = (src["effect" + Convert.ToString (no)]);
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(Char& target) const override
		public override void apply (ref Char target)
		{
			foreach (var effect in effects)
			{
				effect.apply (ref target);
			}
		}

		private List<Effect> effects = new List<Effect> ();
	}

	// The animation changes with the Character level
	public class ByLevelUseEffect : SkillUseEffect
	{
		public ByLevelUseEffect (WzObject src)
		{
			foreach (var sub in src["CharLevel"])
			{
				var level = string_conversion.or_zero<ushort> (sub.Name);
				effects.Add (level, new Effect (sub["effect"]));
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(Char& target) const override
		public override void apply (ref Char target)
		{
			if (effects.Count == 0)
			{
				return;
			}

			ushort level = target.get_level ();
			if (effects.TryGetValue (level, out var effect))
			{
				effect.apply (ref target);
			}
		}

		private SortedDictionary<ushort, Effect> effects = new SortedDictionary<ushort, Effect> ();
	}

	// Use effect for Iron Body
	public class IronBodyUseEffect : SkillUseEffect
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void apply(Char& target) const override
		public override void apply (ref Char target)
		{
			target.show_iron_body ();
		}
	}
}