


using ms_Unity;
using System;

namespace ms
{
    // Handles the changing of channels for players
    // Opcode: CHANGE_CHANNEL(16)
    public class ChangeChannelHandler : PacketHandler
    {
        public override void handle(InPacket recv)
        {
            LoginParser.parse_login(recv);

            var cashshop = UI.get().get_element<UICashShop>();

            if (cashshop)
            {
                cashshop.get().exit_cashshop();
            }
        }
    }

    // Notifies the client of changes in character stats
    // Opcode: CHANGE_STATS(31)
    public class ChangeStatsHandler : PacketHandler
    {
        public override void handle(InPacket recv)
        {
            recv.read_bool(); // 'itemreaction'
            int updatemask = recv.read_int();

            bool recalculate = false;

            foreach (var iter in MapleStat.codes)
            {
                if ((updatemask & iter.Value) != 0)
                {
                    recalculate |= handle_stat(iter.Key, recv);
                }
            }

            if (recalculate)
            {
                Stage.get().get_player().recalc_stats(false);
            }


            checkPlayerIsDied ();
            UI.get().enable();
        }

      
        private bool handle_stat(MapleStat.Id stat, InPacket recv)
        {
            Player player = Stage.get().get_player();

            bool recalculate = false;

            switch (stat)
            {
                case MapleStat.Id.SKIN:
                    player.change_look(stat, recv.read_short());
                    break;
                case MapleStat.Id.FACE:
                case MapleStat.Id.HAIR:
                    player.change_look(stat, recv.read_int());
                    break;
                case MapleStat.Id.LEVEL:
                    player.change_level((ushort)recv.readByte());
                    break;
                case MapleStat.Id.JOB:
                    player.change_job((ushort)recv.read_short());
                    break;
                case MapleStat.Id.EXP:
                    player.get_stats().set_exp(recv.read_int());
                    break;
                case MapleStat.Id.MESO:
                    player.get_inventory().set_meso(recv.read_int());
                    break;
                default:
                    player.get_stats().set_stat(stat, (ushort)recv.read_short());
                    recalculate = true;
                    break;
            }

   

            bool update_statsinfo = need_statsinfo_update(stat);

            if (update_statsinfo && !recalculate)
            {
                var statsinfo = UI.get().get_element<UIStatsInfo>();
                if (statsinfo)
                {
                    statsinfo.get().update_stat(stat);
                }
            }

            bool update_skillbook = need_skillbook_update(stat);

            if (update_skillbook)
            {
                short value = (short)player.get_stats().get_stat(stat);
                var skillbook = UI.get().get_element<UISkillBook>();
                if (skillbook)
                {
                    skillbook.get().update_stat(stat, value);
                }
            }

            return recalculate;
        }
        private bool need_statsinfo_update(MapleStat.Id stat)
        {
            switch (stat)
            {
                case MapleStat.Id.JOB:
                case MapleStat.Id.STR:
                case MapleStat.Id.DEX:
                case MapleStat.Id.INT:
                case MapleStat.Id.LUK:
                case MapleStat.Id.HP:
                case MapleStat.Id.MAXHP:
                case MapleStat.Id.MP:
                case MapleStat.Id.MAXMP:
                case MapleStat.Id.AP:
                    return true;
                default:
                    return false;
            }
        }
        private bool need_skillbook_update(MapleStat.Id stat)
        {
            switch (stat)
            {
                case MapleStat.Id.JOB:
                case MapleStat.Id.SP:
                    return true;
                default:
                    return false;
            }
        }

        private void checkPlayerIsDied ()
        {
            Player player = Stage.get ().get_player ();

            var hp = player.get_stats ().get_stat (MapleStat.Id.HP);
            if (hp <= 0)
            {
                player.die ();

                var nearbyTownMapId = player.get_stats ().get_mapid () / 100000000;
                if (!NxHelper.Map.exist (nearbyTownMapId))
                {
                    nearbyTownMapId = 100000000;
                }

                /*  ms_Unity.FGUI_OK.ShowNotice ("You are died！回到附近的城镇吗？", (b) => { new ChangeMapPacket (true, nearbyTownMapId, Stage.get ().get_portals ().get_portal_by_id (0)?.get_name (), false).dispatch(); });*/

                ms_Unity.FGUI_OK.ShowNotice ("You are died！回到附近的城镇吗？", (b) => { new ChangeMapPacket (true, nearbyTownMapId, String.Empty, false).dispatch (); });
            }
        }
    }

    // Base class for packets which need to parse buff stats
    public abstract class BuffHandler : PacketHandler
    {
        public override void handle(InPacket recv)
        {
            ulong firstmask = (ulong)recv.read_long();
            ulong secondmask = (ulong)recv.read_long();

            //AppDebug.Log($"firstmask:{firstmask}\t secondmask:{secondmask}");
            switch ((Buffstat.Id)secondmask)
            {
                case Buffstat.Id.BATTLESHIP:
                    handle_buff(recv, Buffstat.Id.BATTLESHIP);
                    return;
            }

            foreach (var iter in Buffstat.first_codes)
            {
                if ((firstmask & (ulong)iter.Value) != 0)
                {
                    handle_buff(recv, iter.Key);
                }
            }

            foreach (var iter in Buffstat.second_codes)
            {
                if ((secondmask & (ulong)iter.Value) != 0)
                {
                    handle_buff(recv, iter.Key);
                }
            }

            Stage.get().get_player().recalc_stats(false);
        }

        protected abstract void handle_buff(InPacket recv, Buffstat.Id stat);
    }

    // Notifies the client that a buff was applied to the player
    // Opcode: GIVE_BUFF(32)
    public class ApplyBuffHandler : BuffHandler
    {
        protected override void handle_buff(InPacket recv, Buffstat.Id bs)
        {
            if (recv.length() < 10)
            {
                AppDebug.LogWarning($"ApplyBuffHandler, InPacket length is not enough, Buffstat Id:{bs},maybe same value buff has been handled early");
                return;
            }
            short value = recv.read_short();
            int skillid = recv.read_int();
            int duration = recv.read_int();

            Stage.get().get_player().give_buff(new Buff(bs, value, skillid, duration));
            var bufflist = UI.get().get_element<UIBuffList>();
            if (bufflist)
            {
                bufflist.get().add_buff(skillid, duration);
            }
        }
    }

    // Notifies the client that a buff was canceled
    // Opcode: CANCEL_BUFF(33)
    public class CancelBuffHandler : BuffHandler
    {
        protected override void handle_buff(InPacket recv, Buffstat.Id bs)
        {
            Stage.get().get_player().cancel_buff(bs);
        }
    }

    // Force a stat recalculation
    // Opcode: RECALCULATE_STATS(35)
    public class RecalculateStatsHandler : PacketHandler
    {
        public override void handle(InPacket UnnamedParameter1)
        {
            Stage.get().get_player().recalc_stats(false);
        }
    }

    // Updates the player's skills with the client
    // Opcode: UPDATE_SKILL(36)
    public class UpdateSkillHandler : PacketHandler
    {
        public override void handle(InPacket recv)
        {
            recv.skip(3);

            int skillid = recv.read_int();
            int level = recv.read_int();
            int masterlevel = recv.read_int();
            long expire = recv.read_long();

            Stage.get().get_player().change_skill(skillid, level, masterlevel, expire);
            var skillbook = UI.get().get_element<UISkillBook>();
            if (skillbook)
            {
                skillbook.get().update_skills(skillid);
            }

            UI.get().enable();
        }
    }

    // Parses skill macros
    // Opcode: SKILL_MACROS(124)
    public class SkillMacrosHandler : PacketHandler
    {
        public override void handle(InPacket recv)
        {
            byte size = (byte)recv.read_byte();

            for (byte i = 0; i < size; i++)
            {
                recv.read_string(); // name
                recv.read_byte(); // 'shout' byte
                recv.read_int(); // skill 1
                recv.read_int(); // skill 2
                recv.read_int(); // skill 3
            }
        }
    }

    // Notifies the client that a skill is on cool-down
    // Opcode: ADD_COOLDOWN(234)
    public class AddCooldownHandler : PacketHandler
    {
        public override void handle(InPacket recv)
        {
            int skill_id = recv.read_int();
            short cooltime = recv.read_short();

            Stage.get().get_player().add_cooldown(skill_id, cooltime);
        }
    }

    // Parses key mappings and sends them to the keyboard
    // Opcode: KEYMAP(335)
    public class KeymapHandler : PacketHandler
    {
        public override void handle(InPacket recv)
        {
            recv.skip(1);

            for (byte i = 0; i < 90; i++)
            {
                byte type = (byte)recv.read_byte();
                int action = recv.read_int();

                UI.get().add_keymapping(i, type, action);
            }

            if (UI.get().get_element<UIKeyConfig>().get() is UIKeyConfig keyConfig)
            {
                keyConfig.reset();
            }

            FGUI_Manager.Instance.GetFGUI<FGUI_ActionButtons>().UpdateIcon();
        }
    }
}



