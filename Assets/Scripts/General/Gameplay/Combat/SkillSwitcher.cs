using System;
using System.Collections.Generic;
using System.Linq;
using MapleLib.WzLib;

namespace ms
{
    /// <summary>
    /// Switch skill, based on current buffState
    /// </summary>
    public class SkillSwitcher : Singleton<SkillSwitcher>
    {
        private Dictionary<int, Func<int, int>> skillId_Switcher_dict;
        private Player player;
        public SkillSwitcher()
        {
            skillId_Switcher_dict = new Dictionary<int, Func<int, int>>();
            skillId_Switcher_dict.Add(1209010, Skill_1209010_Switcher);
        }

        public int DoSwitch(int skillId_in, Player player)
        {
            this.player = player;
            if (skillId_Switcher_dict.TryGetValue(skillId_in, out var skillUser))
            {
                return skillUser?.Invoke(skillId_in) ?? skillId_in;
            }
            else
            {
                return skillId_in;
            }
        }

        private ushort last_statusId = 0;
        private ushort combo_Counter = 0;
        private ushort combo_Counter2 = 0;
        private float duration_Charge = 10f;
        private float duration_Combo = 1f;
        private int Skill_1209010_Switcher(int skillId_in)//Witch Balde Element Attack 
        {
            ushort statusId = 1;
            var chargeBuff = player.get_buff(Buffstat.Id.WK_CHARGE);
            if (!chargeBuff.IsValid)
            {
                return -1;
            }

            switch (chargeBuff.skillid)
            {
                case Page.SWORD_ICE_CHARGE:
                    statusId = 1;
                    break;
                case Page.SWORD_FIRE_CHARGE:
                    statusId = 2;
                    break;
                case Page.SWORD_LIT_CHARGE:
                    statusId = 3;
                    break;
                case Page.SWORD_HOLY_CHARGE:
                    statusId = 4;
                    break;
            }
            player.AddComboStatus(duration_Combo, 1);
            var combo_Status = player.GetComboStatus();
            var combo_Value = combo_Status?.Value ?? 0;

            //if (combo_Value <= 6 && combo_Value % 3 == 0)
            if (combo_Value == 3 || combo_Value == 6)
            {
                player.AddChargeBlowStatus(chargeBuff.skillid, duration_Charge, 1);
            }
            var chargeBlow_Status = player.GetChargeBlowStatus(chargeBuff.skillid);
            var chargeBlow_Value = combo_Status?.Value ?? 0;
            var chargeBlow_Value_Clamped = Math.Clamp(chargeBlow_Value, 0, GetElementPhrase(chargeBuff.skillid));

            if (last_statusId != statusId)
            {
                last_statusId = statusId;
                combo_Status.Reset();

                combo_Counter = 0;
                combo_Counter2 = 0;
            }

            combo_Counter = (ushort)(combo_Counter % 4);

            if (combo_Counter == 3)
            {
                combo_Counter2++;
                combo_Counter2 = (ushort)Math.Clamp((ushort)combo_Counter2, (ushort)0, (ushort)GetElementPhrase(chargeBuff.skillid));
            }

            AppDebug.Log($"combo_Value:{combo_Value}\t chargeBlow_Value:{chargeBlow_Value_Clamped}");
            return GetSwitchedSkill((int)(statusId + chargeBlow_Value_Clamped * 4));
            //return GetSwitchedSkill(statusId + combo_Counter2 * 4);
        }

        private int GetElementPhrase(int chargeSkillId)
        {
            var phrase = 0;
            switch (chargeSkillId)
            {
                case Page.SWORD_ICE_CHARGE:
                    if (player.has_learned_skill(WhiteKnight.SWORD_ICE_CHARGE))
                    {
                        phrase = 1;
                    }

                    if (player.has_learned_skill(Paladin.SWORD_ICE_CHARGE))
                    {
                        phrase = 2;
                    }
                    break;
                case Page.SWORD_FIRE_CHARGE:
                    if (player.has_learned_skill(WhiteKnight.SWORD_FIRE_CHARGE))
                    {
                        phrase = 1;
                    }

                    if (player.has_learned_skill(Paladin.SWORD_FIRE_CHARGE))
                    {
                        phrase = 2;
                    }
                    break;
                case Page.SWORD_LIT_CHARGE:
                    if (player.has_learned_skill(WhiteKnight.SWORD_LIT_CHARGE))
                    {
                        phrase = 1;
                    }

                    if (player.has_learned_skill(Paladin.SWORD_LIT_CHARGE))
                    {
                        phrase = 2;
                    }
                    break;
                case Page.SWORD_HOLY_CHARGE:
                    if (player.has_learned_skill(WhiteKnight.SWORD_HOLY_CHARGE))
                    {
                        phrase = 1;
                    }

                    if (player.has_learned_skill(Paladin.SWORD_HOLY_CHARGE))
                    {
                        phrase = 2;
                    }
                    break;
            }

            return phrase;
        }

        private int GetSwitchedSkill(int skillOrder)
        {
            var skillId_out = Page.SWORD_ICE_BLOW;
            switch (skillOrder)
            {
                case 1:
                    skillId_out = Page.SWORD_ICE_BLOW;
                    break;
                case 2:
                    skillId_out = Page.SWORD_FIRE_BLOW;
                    break;
                case 3:
                    skillId_out = Page.SWORD_LIT_BLOW;
                    break;
                case 4:
                    skillId_out = Page.SWORD_HOLY_BLOW;
                    break;
                case 5:
                    skillId_out = WhiteKnight.SWORD_ICE_BLOW;
                    break;
                case 6:
                    skillId_out = WhiteKnight.SWORD_FIRE_BLOW;
                    break;
                case 7:
                    skillId_out = WhiteKnight.SWORD_LIT_BLOW;
                    break;
                case 8:
                    skillId_out = WhiteKnight.SWORD_HOLY_BLOW;
                    break;
                case 9:
                    skillId_out = Paladin.SWORD_ICE_BLOW;
                    break;
                case 10:
                    skillId_out = Paladin.SWORD_FIRE_BLOW;
                    break;
                case 11:
                    skillId_out = Paladin.SWORD_LIT_BLOW;
                    break;
                case 12:
                    skillId_out = Paladin.SWORD_HOLY_BLOW;
                    break;
            }

            return skillId_out;

        }
    }

}