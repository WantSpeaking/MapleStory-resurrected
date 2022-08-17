using System.Collections.Generic;
using System.Linq;
using MapleLib.WzLib;

namespace ms
{
	// Interface for hit effects, animations applied to a mob for each hit.
	public abstract class SkillHitEffect : System.IDisposable
	{
		public virtual void Dispose ()
		{
		}

		public abstract void apply (AttackUser user, Mob target);

		protected class Effect
		{
			public Effect (WzObject src)
			{
				animation = src;
				pos = (sbyte)src["pos"];
				z = (sbyte)src["z"];
			}

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
		public override void apply (AttackUser UnnamedParameter1, Mob UnnamedParameter2)
		{
		}
	}

	// A single animation
	public class SingleHitEffect : SkillHitEffect
	{
		public SingleHitEffect (WzObject src)
		{
			this.effect = new ms.SkillHitEffect.Effect (src["hit"]["0"]);
		}

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
			this.effects = new ms.BoolPair<Effect> (new Effect (src["hit"]["0"]), new Effect (src["hit"]["1"]));
		}

		public override void apply (AttackUser user, Mob target)
		{
			effects[user.secondweapon].apply (target, user.flip);
		}

		private BoolPair<Effect> effects;
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

		public override void apply (AttackUser user, Mob target)
		{
			effects.FirstOrDefault (pair => user.level > pair.Key).Value?.apply (target, user.flip);
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

		public override void apply (AttackUser user, Mob target)
		{
			if (effects.TryGetValue (user.skilllevel,out var effect))
			{
				effect.apply (target, user.flip);
			}
		}

		private SortedDictionary<int, Effect> effects = new SortedDictionary<int, Effect> ();
	}

    public class White_Knight_Charge_1211002_HitEffect : SkillHitEffect
    {
        public White_Knight_Charge_1211002_HitEffect(WzObject src)
        {
            foreach (var sub in src["hit"]["0"])
            {
                var level = string_conversion.or_zero<ushort>(sub.Name);
                effects.Add(level, new Effect(sub));
            }
        }

        public override void apply(AttackUser user, Mob target)
        {
            if (effects.Count == 0)
            {
                return;
            }

            if (user.user is Player player)
            {
                ushort statusId = 1;
                var chargeBuff = player.get_buff(Buffstat.Id.WK_CHARGE);
                switch ((SkillId.Id)chargeBuff.skillid)
                {
                    case SkillId.Id.CHARGE_Ice:
                        statusId = 1;
                        break;
                    case SkillId.Id.CHARGE_Fire:
                        statusId = 2;
                        break;
                    case SkillId.Id.CHARGE_Thunder:
                        statusId = 3;
                        break;
                    case SkillId.Id.CHARGE_Holy:
                        statusId = 5;
                        break;
                }
                effects.TryGetValue(statusId, out var effect);
                effect?.apply(target, user.flip);
            }
            else
            {
                effects.First().Value.apply(target, user.flip);
            }
        }

        private SortedDictionary<ushort, Effect> effects = new SortedDictionary<ushort, Effect>();
    }
}