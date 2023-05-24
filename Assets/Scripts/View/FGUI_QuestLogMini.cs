using client;
using FairyGUI;
using FairyGUI.Utils;
using ms;
using server.quest;
using System.Text;

namespace ms_Unity
{
    public partial class FGUI_QuestLogMini
    {
        public void OnCreate()
        {
            _GList_QuestInfo_in_progress.itemRenderer = ListItemRenderer;
        }

        private void ListItemRenderer(int index,GObject item)
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

                foreach (var mobId_Count_Pair in mobs)
                {
                    int.TryParse(progresses.TryGet(i), out var progress);

                    stringBuilder.Append($"已狩猎{Mob.get_name(mobId_Count_Pair.Key)}{progress}只，需{mobId_Count_Pair.Value}只");
                    i++;
                }

            }

            
            if (items != null)
            {
                //stringBuilder.AppendLine("");

                foreach (var itemId_Count_Pair in items)
                {
                    var itemId = itemId_Count_Pair.Key;
                    var itemCount = itemId_Count_Pair.Value;

                    stringBuilder.Append($"已有{ItemData.get(itemId).get_name()}{ms.Stage.get().get_player().get_inventory().get_total_item_count(itemId)}个，需{itemCount}个");
                }

            }
            ListItem_QuestLog._Txt_Desc.text = stringBuilder.ToString();
        }

        private StringBuilder stringBuilder = new StringBuilder();

        public void UpdateQuest()
        {
            _GList_QuestInfo_in_progress.numItems = MapleCharacter.Player.getStartedQuests().Count;
        }
    }
}