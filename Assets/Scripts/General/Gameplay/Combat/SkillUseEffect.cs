using System;
using System.Collections.Generic;
using System.Linq;
using MapleLib.WzLib;

namespace ms
{
    // Interface for skill effects
    public abstract class SkillUseEffect : System.IDisposable
    {
        public virtual void Dispose()
        {
        }

        public abstract void apply(Char target);

        public virtual int GetAniLength ()
        {
            return 0;
        }
        protected class Effect
        {
            public Effect(WzObject src)//todo src["effect"]["0"] == null
            {
                animation = src;
                z = (sbyte)src?["z"];
            }

            public void apply(Char target)
            {
                target.show_attack_effect(animation, z);
            }

            public int GetAniLength ()
            {
                return animation.get_total_delay();
            }

            private Animation animation;
            private sbyte z;
        }

       
    }

    // No animation
    public class NoUseEffect : SkillUseEffect
    {
        public override void apply(Char UnnamedParameter1)
        {
        }
    }

    // An effect which displays an animation over the Character's position
    public class SingleUseEffect : SkillUseEffect
    {
        public SingleUseEffect(WzObject src)
        {
            this.effect = new ms.SkillUseEffect.Effect(src["effect"]);
        }

        public override void apply(Char target)
        {
            effect.apply(target);
        }

		public override int GetAniLength ()
		{
			return effect.GetAniLength ();
		}
		private Effect effect;
    }

    // An effect which displays an animation over the Character's position
    // The effect changes based on whether the Character uses a two-handed weapon
    public class OneHandedUseEffect : SkillUseEffect
    {
        public OneHandedUseEffect(WzObject src)
        {
            this.effect = new ms.SkillUseEffect.Effect(src["effect"]["0"]);
        }

        public override void apply(Char target)
        {
            effect.apply(target);
        }
        public override int GetAniLength ()
        {
            return effect.GetAniLength ();
        }
        private Effect effect;
    }

    // An effect which displays an animation over the Character's position
    // The effect changes based on whether the Character uses a two-handed weapon
    public class TwoHandedUseEffect : SkillUseEffect
    {
        public TwoHandedUseEffect(WzObject src)
        {
            this.effects = new ms.BoolPair<Effect>(new Effect(src["effect"]["0"]), new Effect(src["effect"]["1"]));
        }

        public override void apply(Char target)
        {
            bool twohanded = target.is_twohanded();
            effects[twohanded].apply(target);
        }
        public override int GetAniLength ()
        {
            return effects[true].GetAniLength ();
        }
        private BoolPair<Effect> effects;
    }

    // An effect which displays multiple animations over the Character's position
    public class MultiUseEffect : SkillUseEffect
    {
        public MultiUseEffect(WzObject src)
        {
            sbyte no = -1;
            WzObject sub = src["effect"];

            while (sub != null)
            {
                effects.Add(new Effect(sub));

                no++;
                sub = (src["effect" + Convert.ToString(no)]);
            }
        }

        public override void apply(Char target)
        {
            foreach (var effect in effects)
            {
                effect.apply(target);
            }
        }
        public override int GetAniLength ()
        {
            return effects.TryGet(0)?.GetAniLength ()??0;
        }
        private List<Effect> effects = new List<Effect>();
    }

    // The animation changes with the Character level
    public class ByLevelUseEffect : SkillUseEffect
    {
        public ByLevelUseEffect(WzObject src)
        {
            foreach (var sub in src["CharLevel"])
            {
                var level = string_conversion.or_zero<ushort>(sub.Name);
                effects.Add(level, new Effect(sub["effect"]));
            }
        }

        public override void apply(Char target)
        {
            if (effects.Count == 0)
            {
                return;
            }

            ushort level = target.get_level();
            if (effects.TryGetValue(level, out var effect))
            {
                effect.apply(target);
            }
        }

        private SortedDictionary<ushort, Effect> effects = new SortedDictionary<ushort, Effect>();
    }

    // Use effect for Iron Body
    public class IronBodyUseEffect : SkillUseEffect
    {
        public override void apply(Char target)
        {
            target.show_iron_body();
        }
    }

    public class PrepareEffect : SkillUseEffect
    {
        public int time = 960;
        public string action = "shoot1";
        public PrepareEffect (WzObject src)
        {
            this.effect = new ms.SkillUseEffect.Effect (src);
            time = src["time"];
            action = src["action"]?.ToString();

        }

        public override void apply (Char target)
        {
            effect.apply (target);
        }

        private Effect effect;
    }
    public class OnKeyDownEffect : SkillUseEffect
    {
        public OnKeyDownEffect (WzObject src)
        {
            this.effect = new ms.SkillUseEffect.Effect (src);
        }

        public override void apply (Char target)
        {
            effect.apply (target);
        }

        public override int GetAniLength ()
        {
            return effect.GetAniLength ();
        }

        private Effect effect;
    }
    public class OnKeyDownEndEffect : SkillUseEffect
    {
        public OnKeyDownEndEffect (WzObject src)
        {
            this.effect = new ms.SkillUseEffect.Effect (src);
        }

        public override void apply (Char target)
        {
            effect.apply (target);
        }

        private Effect effect;
    }
    public class White_Knight_Charge_1211002_UseEffect : SkillUseEffect
    {
        public White_Knight_Charge_1211002_UseEffect(WzObject src)
        {
            foreach (var sub in src["effect"])
            {
                var level = string_conversion.or_zero<ushort>(sub.Name);
                effects.Add(level, new Effect(sub));
            }
        }

        private ushort last_statusId = 0;
        private ushort combo_Counter = 0;
        private ushort combo_Counter2 = 0;
        public override void apply(Char target)
        {
            if (effects.Count == 0)
            {
                return;
            }

            if (target is Player player)
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
                        statusId = 4;
                        break;
                }

                if (last_statusId != statusId)
                {
                    last_statusId = statusId;
                    combo_Counter = 0;
                    combo_Counter2 = 0;
                }

                combo_Counter = (ushort)(combo_Counter % 4);

                if (combo_Counter == 3)
                {
                    combo_Counter2++;
                    combo_Counter2 = (ushort)Math.Clamp((ushort)combo_Counter2, (ushort)0, (ushort)2);
                }

                effects.TryGetValue((ushort)(statusId + combo_Counter2 * 4), out var effect);
                effect?.apply(target);

               

                combo_Counter++;
            }
            else
            {
                effects.First().Value.apply(target);
            }
        }

        private SortedDictionary<ushort, Effect> effects = new SortedDictionary<ushort, Effect>();
    }
}