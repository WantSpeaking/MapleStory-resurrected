/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System;
using System.Text;
using client;
using FairyGUI;
using FairyGUI.Utils;
using ms;
using server.quest;

namespace ms_Unity
{
	public partial class FGUI_QuestLog
	{
		UIQuestLog uIQuestLog;
		void OnCreate ()
		{
			_GList_QuestInfo_Available.itemRenderer = ItemRenderer_canStarted;
			_GList_QuestInfo_in_progress.itemRenderer = ItemRenderer_in_progress;
			_GList_QuestInfo_completed.itemRenderer = ItemRenderer_completed;

			_GList_QuestInfo_Available.onClickItem.Add (OnClick_QuestInfo_started);
			_GList_QuestInfo_in_progress.onClickItem.Add (OnClick_QuestInfo_in_progress);
			_GList_QuestInfo_completed.onClickItem.Add (QuestInfo_QuestInfo_completed);

			_Btn_ForfeitQuest.onClick.Add (onClick_Btn_ForfeitQuest);

			_c_QuestState.onChanged.Add (onChanged_c_QuestState);
		}

		private void onChanged_c_QuestState (EventContext context)
		{
			_Txt_Desc.text = "";
			_Txt_mob.text = "";
			_Txt_item.text = "";

			_GList_QuestInfo_Available.selectedIndex = -1;
			_GList_QuestInfo_in_progress.selectedIndex = -1;
			_GList_QuestInfo_completed.selectedIndex = -1;
		}

		private void onClick_Btn_ForfeitQuest (EventContext context)
		{
			if (_GList_QuestInfo_in_progress.selectedIndex != -1)
			{
				new ForfeitQuestPacket (currentQuestId).dispatch ();
			}
		}

		private void ItemRenderer_canStarted (int index, GObject item)
		{
			var listPool = UnityEngine.Rendering.ListPool<short>.Get ();
			listPool.AddRange (MapleCharacter.Player.CanStartedQuests.Keys);
			var questId = listPool[index];
			var mapleQuest = MapleQuest.getInstance (questId);
			var ListItem_QuestLog = item as FGUI_ListItem_QuestLog;
			ListItem_QuestLog._Txt_Name.text = mapleQuest == null ? $"quest:{questId} 不存在" : $"{mapleQuest?.Id} {mapleQuest?.Name}";
			ListItem_QuestLog.data = questId;
			UnityEngine.Rendering.ListPool<short>.Release (listPool);
		}
		private void ItemRenderer_in_progress (int index, GObject item)
		{
			var qs = MapleCharacter.Player.getStartedQuests ().TryGet (index);
			var questId = qs.QuestID;
			var mapleQuest = MapleQuest.getInstance (questId);
			var ListItem_QuestLog = item as FGUI_ListItem_QuestLog;
			ListItem_QuestLog._Txt_Name.text = mapleQuest == null ? $"quest:{questId} 不存在" : $"{mapleQuest?.Id} {mapleQuest?.Name}";
			ListItem_QuestLog.data = questId;

			_Btn_ForfeitQuest.visible = true;
		}
		private void ItemRenderer_completed (int index, GObject item)
		{
			var qs = MapleCharacter.Player.getCompletedQuests ().TryGet (index);
			var questId = qs.QuestID;
			var mapleQuest = MapleQuest.getInstance (questId);
			var ListItem_QuestLog = item as FGUI_ListItem_QuestLog;
			ListItem_QuestLog._Txt_Name.text = mapleQuest == null ? $"quest:{questId} 不存在" : $"{mapleQuest?.Id} {mapleQuest?.Name}";
			ListItem_QuestLog.data = questId;
		}

		short currentQuestId = 0;
		private void OnClick_QuestInfo_started (EventContext context)
		{
			var ListItem_QuestLog = context.data as FGUI_ListItem_QuestLog;
			var questId = (short)ListItem_QuestLog.data;
			currentQuestId = questId;
			var questInfo = MapleQuest.getInstance (questId);
			_Txt_Desc.text = questInfo.Info_started ?? $"quest {questId} Info_started not exist";

		}

		StringBuilder stringBuilder = new StringBuilder ();
		private void OnClick_QuestInfo_in_progress (EventContext context)
		{
			var ListItem_QuestLog = context.data as FGUI_ListItem_QuestLog;
			var questId = (short)ListItem_QuestLog.data;
			currentQuestId = questId;
			var mapleQuest = MapleQuest.getInstance (questId);
			//var checkInfo = checkLog.GetCheckInfo (questId);
			_Txt_Desc.text = mapleQuest.Info_in_progress;

			var progressData = MapleCharacter.Player.getQuest (questId).getProgress (questId);
			var progresses = progressData.GetSeparateSubString (3);
			var mobs = mapleQuest.getRequiredMobs ();

			if (mobs != null)
			{
				var i = 0;
				stringBuilder.Clear ();

				foreach (var mobId_Count_Pair in mobs)
				{
					int.TryParse (progresses.TryGet (i), out var progress);

					stringBuilder.Append ($"已狩猎{Mob.get_name (mobId_Count_Pair.Key)}{progress}只，需狩猎{mobId_Count_Pair.Value}只; ");
					i++;
				}
				_Txt_mob.text = stringBuilder.ToString ();
			}

			var items = mapleQuest.getCompleteItems ();
			if (items != null)
			{
				stringBuilder.Clear ();

				foreach (var itemId_Count_Pair in items)
				{
					var itemId = itemId_Count_Pair.Key;
					var itemCount = itemId_Count_Pair.Value;

					stringBuilder.Append ($"已有{ItemData.get (itemId).get_name ()}{ms.Stage.get ().get_player ().get_inventory ().get_total_item_count (itemId)}个，需{itemCount}个; ");
				}
				_Txt_item.text = stringBuilder.ToString ();
			}
		}

		private void QuestInfo_QuestInfo_completed (EventContext context)
		{
			var ListItem_QuestLog = context.data as FGUI_ListItem_QuestLog;
			var questId = (short)ListItem_QuestLog.data;
			currentQuestId = questId;
			var questInfo = MapleQuest.getInstance (questId);
			_Txt_Desc.text = questInfo.Info_completed;
		}

		public void OnVisiblityChanged (bool isVisible)
		{
            if (isVisible)
			{
				UpdateQuest ();
			}
		}

		Quest quest => ms.Stage.Instance.get_player ().get_quest ();
		CheckLog checkLog => quest.checkLog;

		public void UpdateQuest ()
		{
			_Btn_ForfeitQuest.visible = false;
			_Txt_Desc.text = "";

			MapleCharacter.Player.RefreshCanStarted_Quest (true);
			MapleCharacter.Player.LogAllQuest ();

			_GList_QuestInfo_Available.numItems = MapleCharacter.Player.CanStartedQuests.Count;
			_GList_QuestInfo_in_progress.numItems = MapleCharacter.Player.getStartedQuests ().Count;
			_GList_QuestInfo_completed.numItems = MapleCharacter.Player.getCompletedQuests ().Count;

		}
	}
}