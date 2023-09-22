using client;
using FairyGUI;
using FairyGUI.Utils;
using ms;
using server.quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ms_Unity
{
    public partial class FGUI_QuestLogMini
    {
        public void OnCreate()
        {
            _GList_QuestInfo_in_progress.itemRenderer = ListItemRenderer_Quest;
            _GList_Party.itemRenderer = ListItemRenderer_Party;

            _Btn_CreateParty.onClick.Add(OnClick_Btn_CreateParty);
            _Btn_QuitParty.onClick.Add(OnClick_Btn_QuitParty);

        }

        private void OnClick_Btn_CreateParty(EventContext context)
        {
            new CreatePartyPacket().dispatch();
        }
        private void OnClick_Btn_QuitParty(EventContext context)
        {
            new LeavePartyPacket().dispatch();
        }
        private void ListItemRenderer_Quest(int index,GObject item)
        {
            var qs = MapleCharacter.Player.getStartedQuests().TryGet(index);
            var questId = qs.QuestID;
            var mapleQuest = MapleQuest.getInstance(questId);
            var ListItem_QuestLog = item as FGUI_ListItem_QuestLogMini;
            if (mapleQuest == null)
            {
                AppDebug.Log($"index:{index}\tquestId:{questId}");
                return;
            }
            ListItem_QuestLog._Txt_Name.text = mapleQuest == null ? $"quest:{questId} 不存在" : $"{mapleQuest?.Id} {mapleQuest?.Name}";

            stringBuilder.Clear();


            var progressData = MapleCharacter.Player.getQuest(questId).getProgress(questId);
            var progresses = progressData.GetSeparateSubString(3);
            var mobs = mapleQuest.getRequiredMobs();
            var items = mapleQuest.getCompleteItems();

            if (mobs == null && items == null)
            {
                stringBuilder.Append(mapleQuest.Info_in_progress);
            }

            if (mobs != null)
            {
                //stringBuilder.AppendLine("");
                var i = 0;
                var k = 0;
                var c = mobs.Count;
                foreach (var mobId_Count_Pair in mobs)
                {
                    k++;
                    int.TryParse(progresses.TryGet(i), out var progress);

                    stringBuilder.AppendLine($"狩猎 {Mob.get_name(mobId_Count_Pair.Key)} ({progress}/{mobId_Count_Pair.Value})");
                    i++;
                    if (k < c)
                    {
                        stringBuilder.AppendLine("");
                    }
                }

            }

            
            if (items != null)
            {
                //stringBuilder.AppendLine("");
                var k = 0;
                var c = items.Count;
                foreach (var itemId_Count_Pair in items)
                {
                    k++;
                    
                    var itemId = itemId_Count_Pair.Key;
                    var itemCount = itemId_Count_Pair.Value;

                    stringBuilder.Append($"获取 {ItemData.get(itemId).get_name()} ({ms.Stage.get().get_player().get_inventory().get_total_item_count(itemId)}/{itemCount})");
                    if (k < c)
                    {
                        stringBuilder.AppendLine("");
                    }
                }

            }
            ListItem_QuestLog._Txt_Desc.text = stringBuilder.ToString();
        }

        private void ListItemRenderer_Party(int index, GObject item)
        {
            var parUI = item as FGUI_ListItem_Party;
            var parData = partyMembers.TryGet(index);

            if (parUI != null)
            {
                parUI._Txt_Name.text = parData.Name;
            }
        }

        private StringBuilder stringBuilder = new StringBuilder();

        public void UpdateQuest()
        {
            _GList_QuestInfo_in_progress.numItems = MapleCharacter.Player.getStartedQuests().Count;
            _GList_QuestInfo_in_progress.ResizeToFit(3);

            /*foreach (var item in _GList_QuestInfo_in_progress.GetChildren())
            {
                AppDebug.Log(item.position);
            }*/
        }

        public void OnPartyDataChanged(List<MaplePartyCharacter> partyMemberArray, int leaderId)
        {
            partyMembers.Clear();
            partyMembers.AddRange(partyMemberArray.Where(c=>c.isValid));

            PartyLeaderId = leaderId;

            _GList_Party.numItems = partyMembers.Count;

            _c_PartyStatus.selectedIndex = PartyLeaderId != 0 ? 1 : 0;
           
        }

        private List<MaplePartyCharacter> partyMembers = new List<MaplePartyCharacter>();
        private int PartyLeaderId;

        public void UpdateHpBar_Char(int cid, int curHp, int maxHp)
        {
            for (int i = 0; i < partyMembers.Count; i++)
            {
                if (partyMembers[i].id == cid && partyMembers[i].isValid)
                {
                    var partyItemUI = _GList_Party.GetChildAt(i) as FGUI_ListItem_Party;
                    if(partyItemUI != null)
                    {
                        partyItemUI._ProgressBar_HP.value = curHp;
                        partyItemUI._ProgressBar_HP.max = maxHp;
                        partyItemUI._ProgressBar_HP.Update(curHp);
                    }
                }
            }
        }
       
    }
}