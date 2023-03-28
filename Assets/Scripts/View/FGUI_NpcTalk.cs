using client;
using FairyGUI;
using FairyGUI.Utils;
using ms;
using server.quest;
using Utility;
namespace ms_Unity
{
	public partial class FGUI_NpcTalk
	{
		public UINpcTalk.TalkType type;
		public UINpcTalk uINpcTalk;

		public void OnCreate ()
		{
			_Txt_text.onClickLink.Add (onClickLink);

			_Btn_Close.onClick.Add (onClick_Btn_Close);
			_Btn_Next.onClick.Add (onClick_Btn_Next);
			_Btn_OK.onClick.Add (onClick_Btn_OK);
			_Btn_Prev.onClick.Add (onClick_Btn_Prev);
			_Btn_YES.onClick.Add (onClick_Btn_YES);
			_Btn_No.onClick.Add (onClick_Btn_NO);
			_Btn_Accept.onClick.Add (onClick_Btn_Accept);
			_Btn_Decline.onClick.Add (onClick_Btn_Decline);

		}

		private void onClick_Btn_Close (EventContext context)
		{
			//ms_Unity.FGUI_Manager.Instance.CloseFGUI<ms_Unity.FGUI_NpcTalk> ();
			UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();
			if (isClientPage)
			{
				if (isInScriptedMode)
				{
					new NpcTalkMorePacket (rawMsgtype, -1).dispatch ();
				}
			}
			else
			{
				new NpcTalkMorePacket (rawMsgtype, -1).dispatch ();
			}
		}
		private void onClick_Btn_OK (EventContext context)
		{
			if (isClientPage)
			{
				if (isInScriptedMode)
				{
					new NpcTalkMorePacket (rawMsgtype, 1).dispatch ();
					UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();
				}
				else
				{
					if (isAsk)
					{
						if (chooseRightAnswer)
						{
							//完成任务
							new CompleteQuestPacket (currentQuestId, currentNpcId, currentSelection).dispatch ();
							UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();
						}
						else
						{
							UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();
						}
					}
					else
					{
						if (currentSayStageIndex == 1)
						{
							//完成任务
							new CompleteQuestPacket (currentQuestId, currentNpcId).dispatch ();
						}
						UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();
					}
				}
					
			}
			else
			{
				UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();

				new NpcTalkMorePacket (rawMsgtype, 1).dispatch ();
			}

		}

		private void onClick_Btn_Accept (EventContext context)
		{
			if (isClientPage)
			{
				if (isInScriptedMode)
				{
					new NpcTalkMorePacket (rawMsgtype, 1).dispatch ();
				}
				else
				{
					if (isAsk)
					{

					}
					else
					{
						if (currentSayStageIndex == 0)
						{
							new StartQuestPacket (currentQuestId, currentNpcId).dispatch ();
							AppDebug.Log ($"StartQuest ID:{currentQuestId}, NpcId:{currentNpcId}");
						}

						UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();
					}
				}
				
			}
			else
			{
				UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();

				new NpcTalkMorePacket (rawMsgtype, 1).dispatch ();
			}

		}
		private void onClick_Btn_Decline (EventContext context)
		{
			if (isClientPage)
			{
				if (isInScriptedMode)
				{
					new NpcTalkMorePacket (rawMsgtype, 0).dispatch ();
				}
				{
					if (isAsk)
					{

					}
					else
					{
						UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();
					}
				}

			}
			else
			{
				UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();

				new NpcTalkMorePacket (rawMsgtype, 0).dispatch ();
			}
		}

		private void onClick_Btn_Next (EventContext context)
		{
			if (isClientPage)
			{
				if (isInScriptedMode)
				{
					new NpcTalkMorePacket (rawMsgtype, 1).dispatch ();
				}
				else
				{
					if (currentSayStageIndex == 0)
					{
						if (isInIntroducePage)
						{
							ShowIntroducePage ();
						}
						else if (isInYesPage)
						{
							ShowYesPage ();
						}
						else if (isInNoPage)
						{
							ShowNoPage ();
						}
					}
					else if (currentSayStageIndex == 1)
					{
						if (0 <= introducePageIndex && introducePageIndex < currentSayStage.introducePages.Count)
						{
							ShowIntroducePage ();
						}
						else if (0 <= yesPageIndex && yesPageIndex < currentSayStage.yesPages.Count)
						{
							ShowYesPage ();
						}
					}
				}

			}
			else
			{
				UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();

				new NpcTalkMorePacket (rawMsgtype, 1).dispatch ();
			}

		}
		private void onClick_Btn_Prev (EventContext context)
		{
			if (isClientPage)
			{
				if (isInScriptedMode)
				{
					new NpcTalkMorePacket (rawMsgtype, 0).dispatch ();
				}
				else
				{
					
				}

			}
			else
			{
				UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();

				new NpcTalkMorePacket (rawMsgtype, 0).dispatch ();
			}
		}

		private void onClick_Btn_YES (EventContext context)
		{
			if (isClientPage)
			{
				if (isInScriptedMode)
				{
					new NpcTalkMorePacket (rawMsgtype, 1).dispatch ();
				}
				else
				{
					if (isAsk)
					{

					}
					else
					{
						ShowYesPage ();
					}
				}
			}
			else
			{
				UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();

				new NpcTalkMorePacket (rawMsgtype, 1).dispatch ();
			}
		}
		private void onClick_Btn_NO (EventContext context)
		{
			if (isClientPage)
			{
				if (isAsk)
				{

				}
				else
				{
					ShowNoPage ();
				}
			}
			else
			{
				UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();

				new NpcTalkMorePacket (rawMsgtype, 0).dispatch ();
			}
		}



		UINpcTalk.TalkType GetIntroduceTalkType ()
		{
			if (introducePageIndex != -1)
			{
				if (isAsk)
				{
					//如果 有下一个对话  
					if (currentSayStage.introducePages.TryGet (introducePageIndex + 1, out var sayPage))
					{
						return UINpcTalk.TalkType.NONE;
					}
					else
					{
						return UINpcTalk.TalkType.SendOk;
					}
				}
				else
				{

					//如果 有下一个introduce对话 
					if (currentSayStage.introducePages.TryGet (introducePageIndex + 1, out var sayPage))
					{
						return UINpcTalk.TalkType.SendNext;
					}
					else
					{
                        
						if (currentSayStageIndex == 0)//可开始状态，介绍对话完了，直接进入接受 拒绝 界面
						{
                            yesPageIndex = 0;
                            noPageIndex = 0;
                            return UINpcTalk.TalkType.SendAcceptDecline;

                            /*bool hasYes = currentSayStage.yesPages.Count != 0;
							bool hasNo = currentSayStage.noPages.Count != 0 || currentSayStage.stopPages.Count != 0;
							if (hasYes && hasNo)
							{
								yesPageIndex = 0;
								noPageIndex = 0;
								return UINpcTalk.TalkType.SendYesNo;
							}
							else if (hasYes && !hasNo)
							{
								yesPageIndex = 0;
								return UINpcTalk.TalkType.SendOk;
							}
							else if ((!hasYes && !hasNo) || (!hasYes && hasNo))
							{
								return UINpcTalk.TalkType.SendAcceptDecline;
							}*/
						}
						else if (currentSayStageIndex == 1)
						{
							if (yesPageIndex <= 0)
							{
								yesPageIndex = 0;
							}

							//如果 有下一个yes对话 
							if (currentSayStage.yesPages.TryGet(yesPageIndex, out var yesPage))
							{
								return UINpcTalk.TalkType.SendNext;
							}
							else
							{
								return UINpcTalk.TalkType.SendOk;
							}
						}
					}
				}

			}

			return UINpcTalk.TalkType.NONE;
		}
		UINpcTalk.TalkType GetYesTalkType ()
		{
			if (yesPageIndex != -1)
			{
				if (isAsk)
				{

				}
				else
				{
					//如果 有下一个对话 
					if (currentSayStage.yesPages.TryGet (yesPageIndex + 1, out var sayPage))
					{
						return UINpcTalk.TalkType.SendNext;
					}
					else
					{
						if (currentSayStageIndex == 0)
						{
							return UINpcTalk.TalkType.SendAcceptDecline;
						}

						return UINpcTalk.TalkType.SendOk;
					}
				}
			}

			return UINpcTalk.TalkType.NONE;
		}
		UINpcTalk.TalkType GetNoTalkType ()
		{
			if (noPageIndex != -1)
			{
				if (isAsk)
				{

				}
				else
				{
					//如果 有下一个对话 
					if (currentSayStage.noPages.TryGet (noPageIndex + 1, out var sayPage))
					{
						return UINpcTalk.TalkType.SendNext;
					}
					else
					{
						bool hasStopPage = currentSayStage.stopPages.Count != 0;
						if (hasStopPage)
						{
							return UINpcTalk.TalkType.SendNext;
						}
						else
						{
							return UINpcTalk.TalkType.SendOk;
						}
					}
				}
			}

			return UINpcTalk.TalkType.NONE;
		}
		UINpcTalk.TalkType GetStopTalkType ()
		{
			return UINpcTalk.TalkType.SendOk;
		}


		public void BeginChooseQuestSay (Npc npc, SayPage sayPage)
		{
			//type = TalkType.NONE;
			currentNpc = npc;
			currentNpcId = npc.get_id ();
			pageIndex = sayPage.pageIndex;

			currentSayPage = sayPage;
		}
		public void BeginQuestSay (MapleQuest mapleQuest, int stageIndex, int npcId)
		{
			AppDebug.Log ($"Choosed Quest {mapleQuest.Id}\t {mapleQuest.Name} stageIndex:{stageIndex}");
            currentQuestId = mapleQuest.Id;

            var sayStage = mapleQuest.GetSayStage(stageIndex);
			if (sayStage != null)
			{
                currentSayStage = sayStage;
                isAsk = sayStage.isAsk;
                //ParseSayStage (sayStage);
                introducePageIndex = 0;

                if (isInScriptedMode)
                {
                    if (currentSayStageIndex == 0)
                    {
                        new ScriptedStartQuest(currentQuestId, npcId).dispatch();
                    }
                    else if (currentSayStageIndex == 1)
                    {
                        new ScriptedEndQuest(currentQuestId, npcId).dispatch();
                    }
                }
                else
                {
                    if (stageIndex == 0)//can start
                    {
                        ShowIntroducePage();
                    }
                    else if (stageIndex == 1)//in progress
                    {
                        var canComplete = MapleQuest.getInstance(mapleQuest.Id).canComplete(MapleCharacter.Player, currentNpcId);
                        if (canComplete)
                        {
                            ShowIntroducePage();
                        }
                        else
                        {
                            stopPageIndex = 0;
                            ShowStopPage();
                        }
                    }
                    else//can complete
                    {

                    }
                }
            }
			else
			{
	/*			if (stageIndex == 2)//没有 可完成的 SayStage 直接完成任务
				{
                    //完成任务
                    new CompleteQuestPacket(currentQuestId, currentNpcId).dispatch();
                    UI.get().get_element<UINpcTalk>().get()?.deactivate();
                }*/
			}
			
		}


		private void ShowIntroducePage ()
		{
			if (currentSayStage.introducePages.TryGet (introducePageIndex, out var sayPage))
			{
				pageIndex = sayPage.Value.pageIndex;
				currentSayPage = sayPage.Value;
				uINpcTalk.change_text (currentNpcId, GetIntroduceTalkType (), 0, 0, currentSayPage.text);
				introducePageIndex++;
			}
		}
		private void ShowYesPage ()
		{
			if (currentSayStage.yesPages.TryGet (yesPageIndex, out var sayPage))
			{
				pageIndex = sayPage.pageIndex;
				currentSayPage = sayPage;
				uINpcTalk.change_text (currentNpcId, GetYesTalkType (), 0, 0, currentSayPage.text);
				yesPageIndex++;
			}
		}
		private void ShowNoPage ()
		{
			if (currentSayStage.noPages.TryGet (noPageIndex, out var sayPage))
			{
				pageIndex = sayPage.pageIndex;
				currentSayPage = sayPage;
				uINpcTalk.change_text (currentNpcId, GetNoTalkType (), 0, 0, currentSayPage.text);
				noPageIndex++;
			}
		}
		private void ShowStopPage ()
		{
			if (currentSayStage.stopPages.TryGet (stopPageIndex, out var sayPage))
			{
				pageIndex = sayPage.pageIndex;
				currentSayPage = sayPage;
				uINpcTalk.change_text (currentNpcId, GetStopTalkType (), 0, 0, currentSayPage.text);
				stopPageIndex++;
			}
		}


		private new void onClickLink (EventContext context)
		{
			short.TryParse ((string)context.data, out var seletIndex);
			if (isClientPage)
			{
				//init page
				if (pageIndex == -1)
				{
					var pair = currentNpc.GetQuestSayInfo (seletIndex);
					if (pair.Item1 != null)
					{
						BeginQuestSay (pair.Item1, pair.Item2, currentNpc.get_id());
					}
				}
				else//ask page
				{
					if (isAsk)
					{
						var answerInex = currentSayPage.rightAnswerChoiceNumber - 1;
						currentSelection = seletIndex;
						chooseRightAnswer = seletIndex == answerInex;
						if (chooseRightAnswer)//答对了,进入下一页
						{
							ShowIntroducePage ();
						}
						else//答错了,进入相应的错误 页面
						{
							uINpcTalk.change_text (currentNpcId, UINpcTalk.TalkType.SendOk, 0, 0, currentSayPage.wrongAnswer_index_Texts[seletIndex]);
						}
					}
				}
			}
			else
			{

				new NpcTalkMorePacket (seletIndex).dispatch ();
				AppDebug.Log ($"onClickLink,L{context.data}");
			}
		}
		public void OnVisiblityChanged (bool isVisible, UINpcTalk uINpcTalk)
		{
			this.uINpcTalk = uINpcTalk;
			uINpcTalk.SetFGUI_NpcTalk (this);
			if (isVisible)
			{
				//SetGList ();
			}
			else
			{
				Rest ();
			}
			//AppDebug.Log ($"uINpcTalk OnVisiblityChanged isVisible:{isVisible}");

		}
		public sbyte rawMsgtype;
		public void change_text ()
		{
			_GLoader_speaker.texture = uINpcTalk.speaker.nTexture;
			_GLoader_nametag.texture = uINpcTalk.nametag.nTexture;
			_Txt_name.text = uINpcTalk.name.get_text ();
			_Txt_text.text = uINpcTalk.text.get_text ();

			_c_TalkType.selectedIndex = (int)uINpcTalk.type;
			rawMsgtype = uINpcTalk.rawMsgtype;
		}
		public void Rest ()
		{
			pageIndex = -2;
			currentSayPage = default;
			currentNpc = null;
			currentNpcId = 0;
			currentQuestId = 0;
			currentSelection = -1;
			isAsk = false;
			introducePageIndex = -1;
			yesPageIndex = -1;
			noPageIndex = -1;
			stopPageIndex = -1;
			chooseRightAnswer = false;
		}


		/// <summary>
		/// 空的 只有 Sayinfo.img/QuestId/0或1
		/// </summary>
		private bool isInScriptedMode => currentSayStage.introducePages.Count == 0 && currentSayStage.yesPages.Count == 0 && currentSayStage.stopPages.Count == 0;
		private bool isClientPage => currentNpc?.hasQuest () ?? false;

		private bool isInIntroducePage => introducePageIndex < (currentSayStage?.introducePages.Count ?? 0);
		private bool isInYesPage => yesPageIndex < (currentSayStage?.yesPages.Count ?? 0);
		private bool isInNoPage => noPageIndex < (currentSayStage?.noPages.Count ?? 0);
		private SayStage currentSayStage;
		private int currentSayStageIndex => currentSayStage?.stageIndex ?? 0;
		Quest quest => ms.Stage.Instance.get_player ().get_quest ();

		private int pageIndex = -2;
		private SayPage currentSayPage;
		private Npc currentNpc;
		private int currentNpcId;
		private short currentQuestId;
		private short currentSelection = -1;
		bool isAsk = false;
		int introducePageIndex = -1;
		int yesPageIndex = -1;
		int noPageIndex = -1;
		int stopPageIndex = -1;
		bool chooseRightAnswer = false;
	}
}