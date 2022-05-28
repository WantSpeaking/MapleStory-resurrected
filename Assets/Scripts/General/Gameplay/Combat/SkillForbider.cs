using System;
using System.Collections.Generic;

namespace ms
{
    public class SkillForbid : Singleton<SkillForbid>
    {
        private Dictionary<int, Func<bool>> skillId_Forbid_dict;
        private Player player;
        public SkillForbid()
        {
            skillId_Forbid_dict = new Dictionary<int, Func<bool>>();
            skillId_Forbid_dict.Add(1211002, Skill_1211002_Forbid);
            skillId_Forbid_dict.Add(1209010, Skill_1209010_Forbid);
        }

        public bool IsForbid(int skillId, Player player)
        {
            this.player = player;
            if (skillId_Forbid_dict.TryGetValue(skillId, out var skillUser))
            {
                return skillUser?.Invoke() ?? false;
            }
            else
            {
                return false;
            }
        }

        private bool Skill_1211002_Forbid()//圣骑士 属性攻击
        {
            return !player.has_buff(Buffstat.Id.WK_CHARGE);
        }

        private bool Skill_1209010_Forbid()//圣骑士 属性攻击
        {
            return !player.has_buff(Buffstat.Id.WK_CHARGE);
        }
    }
}