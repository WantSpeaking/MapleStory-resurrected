/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System;
using FairyGUI;
using FairyGUI.Utils;
using ms;

namespace ms_Unity
{
	public partial class FGUI_StatusBar
	{

		public ms.UIStatusBar uIStatusBar;
		public void OnCreate ()
		{
			_BT_CASHSHOP.onClick.Add (OnClick_BT_CASHSHOP);
			_BT_MENU.onClick.Add (OnClick_BT_MENU);
			_BT_MENU_QUEST.onClick.Add (OnClick_BT_MENU_QUEST);
			_BT_SETTING_CHANNEL.onClick.Add (OnClick_BT_SETTING_CHANNEL);
			_BT_COMMUNITY_FRIENDS.onClick.Add (OnClick_BT_COMMUNITY_FRIENDS);
			_BT_COMMUNITY_PARTY.onClick.Add (OnClick_BT_COMMUNITY_PARTY);
			_BT_CHARACTER_SKILL.onClick.Add (OnClick_BT_CHARACTER_SKILL);
			_Btn_OpenInventoryPanel.onClick.Add (OnClick_OpenInventoryPanel);
			_BT_BackToChooseChar.onClick.Add (OnClick_BackToChooseChar);
			_BT_BackToLogin.onClick.Add (OnClick_BackToLogin);
			_BT_CustomizeButtons.onClick.Add (OnClick_CustomizeButtons);
			_BT_Instance.onClick.Add (OnClick_BT_Instance);
			_BT_FuctionCenter.onClick.Add (OnClick_FuctionCenter);

            _Txt_Version.SetVar("count", GameUtil.Instance.Version).FlushVars();

        }

		private void OnClick_BT_Instance ()
		{
			new TalkToNPCPacket (GameUtil.Instance.testTalkNpcId_Instance).dispatch ();
			FGUI_Manager.Instance.GetFGUI<FGUI_NpcTalk>().beginScriptTalk = true;
		}
		private void OnClick_FuctionCenter ()
		{
			new TalkToNPCPacket (GameUtil.Instance.testTalkNpcId_FunctionCenter,GameUtil.Instance.testTalkNpcFileName_FunctionCenter).dispatch ();
			FGUI_Manager.Instance.GetFGUI<FGUI_NpcTalk>().beginScriptTalk = true;
		}
		private void OnClick_CustomizeButtons(EventContext context)
        {
			FGUI_Manager.Instance.OpenFGUI<FGUI_CustomizeJoystickAndButtons>().OnVisiblityChanged(true);
        }

        private void OnClick_OpenInventoryPanel (EventContext context)
		{
			UI.get ().emplace<UIItemInventory> (ms.Stage.get ().get_player ().get_inventory ());
		}

		private void OnClick_BT_CHARACTER_SKILL (EventContext context)
		{
			UI.get ().emplace<UISkillBook> (ms.Stage.get ().get_player ().get_stats (), ms.Stage.get ().get_player ().get_skills ());
		}

		private void OnClick_BT_COMMUNITY_PARTY (EventContext context)
		{
			OnClick_PARTY_And_Friends ();
		}

		private void OnClick_BT_COMMUNITY_FRIENDS (EventContext context)
		{
			OnClick_PARTY_And_Friends ();
		}
		
		private void OnClick_BackToChooseChar (EventContext context)
		{
			//OnClick_PARTY_And_Friends ();
		}
		
		private void OnClick_BackToLogin (EventContext context)
		{
			//ms.UI.get ().emplace<UIQuit> (ms.Stage.get ().get_player ().get_stats ());
			MapleStory.Instance.BackToLogin ();
			//OnClick_PARTY_And_Friends ();
		}
		
		private void OnClick_PARTY_And_Friends ()
		{
			/*var userlist = UI.get ().get_element<UIUserList> ();
			var tab = (id == (int)Buttons.BT_COMMUNITY_FRIENDS) ? UIUserList.Tab.FRIEND : UIUserList.Tab.PARTY;

			if (!userlist)
			{
				UI.get ().emplace<UIUserList> ((ushort)tab);
			}
			else
			{
				var cur_tab = userlist.get ().get_tab ();
				var is_active = userlist.get ().is_active ();

				if (cur_tab == (int)tab)
				{
					if (is_active)
					{
						userlist.get ().deactivate ();
					}
					else
					{
						userlist.get ().makeactive ();
					}
				}
				else
				{
					if (!is_active)
					{
						userlist.get ().makeactive ();
					}

					userlist.get ().change_tab ((byte)tab);
				}
			}*/
		}

		private void OnClick_BT_SETTING_CHANNEL (EventContext context)
		{
			UI.get ().emplace<UIChannel> ();
		}

		private void OnClick_BT_MENU_QUEST (EventContext context)
		{
			UI.get ().emplace<UIQuestLog> (ms.Stage.get ().get_player ().get_questlog ());
		}

		private void OnClick_BT_MENU (EventContext context)
		{
			
		}

		private void OnClick_BT_CASHSHOP (EventContext context)
		{
			new EnterCashShopPacket ().dispatch ();
		}

		protected override void OnUpdate ()
		{
			base.OnUpdate ();

			_Txt_Level.text = uIStatusBar.stats.get_stat (MapleStat.Id.LEVEL).ToString ();
			_Txt_Name.text = uIStatusBar.stats.get_name ();
			_Txt_EXP.SetVar ("persent", (uIStatusBar.getexppercent ()*100).ToString()).FlushVars ();

/*			_ProgressBar_HP.value = uIStatusBar.gethppercent ();
			_ProgressBar_MP.value = uIStatusBar.getmppercent ();*/

			_ProgressBar_EXP.value = uIStatusBar.getexppercent ();

			_ProgressBar_HP.value = uIStatusBar.stats.get_stat (MapleStat.Id.HP);
			_ProgressBar_HP.max = uIStatusBar.stats.get_total (EquipStat.Id.HP);

			_ProgressBar_MP.value = uIStatusBar.stats.get_stat (MapleStat.Id.MP);
			_ProgressBar_MP.max = uIStatusBar.stats.get_total (EquipStat.Id.MP);

			//AppDebug.Log ($"hp:{uIStatusBar.gethppercent ()} \t mp:{uIStatusBar.getmppercent ()} \t exp:{uIStatusBar.getexppercent ()}");
		}
	}
}