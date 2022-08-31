/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using ms;
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
			//new NpcTalkMorePacket ((sbyte)type, -1).dispatch ();
		}
		private void onClick_Btn_Next (EventContext context)
		{
			if (isClientPage)
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
			else
			{
				UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();

				new NpcTalkMorePacket ((sbyte)type, 1).dispatch ();
			}

		}
		private void onClick_Btn_OK (EventContext context)
		{
			if (isClientPage)
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
					UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();
				}
			}
			else
			{
				UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();

				new NpcTalkMorePacket ((sbyte)type, 1).dispatch ();
			}

		}
		private void onClick_Btn_Accept (EventContext context)
		{
			if (isClientPage)
			{
				if (isAsk)
				{

				}
				else
				{
					new StartQuestPacket(currentQuestId, currentNpcId).dispatch ();

					UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();
				}
			}
			else
			{
				UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();

				new NpcTalkMorePacket ((sbyte)type, 1).dispatch ();
			}

		}
		private void onClick_Btn_Prev (EventContext context)
		{
			UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();

			//ms_Unity.FGUI_Manager.Instance.CloseFGUI<ms_Unity.FGUI_NpcTalk> ();

			new NpcTalkMorePacket ((sbyte)type, 0).dispatch ();
		}
		private void onClick_Btn_YES (EventContext context)
		{
			if (isClientPage)
			{
				if (isAsk)
				{

				}
				else
				{
					ShowYesPage ();
				}
			}
			else
			{
				UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();

				new NpcTalkMorePacket ((sbyte)type, 1).dispatch ();
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

				new NpcTalkMorePacket ((sbyte)type, 0).dispatch ();
			}
		}
		private void onClick_Btn_Decline (EventContext context)
		{
			if (isClientPage)
			{
				if (isAsk)
				{

				}
				else
				{
					UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();
				}
			}
			else
			{
				UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();

				new NpcTalkMorePacket ((sbyte)type, 0).dispatch ();
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
			AppDebug.Log ($"uINpcTalk OnVisiblityChanged isVisible:{isVisible}");

		}

		public void change_text ()
		{
			_GLoader_speaker.texture = uINpcTalk.speaker.nTexture;
			_GLoader_nametag.texture = uINpcTalk.nametag.nTexture;
			_Txt_name.text = uINpcTalk.name.get_text ();
			_Txt_text.text = uINpcTalk.text.get_text ();

			_c_TalkType.selectedIndex = (int)uINpcTalk.type;
		}

		public void BeginQuestSay (SayInfo sayInfo, bool isQuestStarted)
		{
			var stageIndex = isQuestStarted ? 1 : 0;
			var sayStage = sayInfo.sayStages[stageIndex];
			currentSayStage = sayStage;
			currentQuestId = sayInfo.questId;
			//ParseSayStage (sayStage);
			introducePageIndex = 0;

			ShowIntroducePage ();
		}

		bool isInIntroducePage => introducePageIndex < (currentSayStage?.introducePages.Count ?? 0);
		bool isInYesPage => yesPageIndex < (currentSayStage?.yesPages.Count ?? 0);
		bool isInNoPage => noPageIndex < (currentSayStage?.noPages.Count ?? 0);

		SayStage currentSayStage;


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

		private bool isClientPage => currentNpc?.hasQuest () ?? false;
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

		private void ShowIntroducePage ()
		{
			if (currentSayStage.introducePages.TryGet (introducePageIndex, out var sayPage))
			{
				pageIndex = sayPage.pageIndex;
				currentSayPage = sayPage;
				isAsk = sayPage.sayPageType == SayPageType.Ask;
				uINpcTalk.change_text (currentNpcId, (sbyte)GetIntroduceTalkType (), 0, 0, currentSayPage.text);
				introducePageIndex++;
			}
		}

		private void ShowYesPage ()
		{
			if (currentSayStage.yesPages.TryGet (yesPageIndex, out var sayPage))
			{
				pageIndex = sayPage.pageIndex;
				currentSayPage = sayPage;
				isAsk = sayPage.sayPageType == SayPageType.Ask;
				uINpcTalk.change_text (currentNpcId, (sbyte)GetYesTalkType (), 0, 0, currentSayPage.text);
				yesPageIndex++;
			}
		}
		private void ShowNoPage ()
		{
			if (currentSayStage.noPages.TryGet (noPageIndex, out var sayPage))
			{
				pageIndex = sayPage.pageIndex;
				currentSayPage = sayPage;
				isAsk = sayPage.sayPageType == SayPageType.Ask;
				uINpcTalk.change_text (currentNpcId, (sbyte)GetNoTalkType (), 0, 0, currentSayPage.text);
				noPageIndex++;
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
						return UINpcTalk.TalkType.SENDOK;
					}
				}
				else
				{

					//如果 有下一个对话 
					if (currentSayStage.introducePages.TryGet (introducePageIndex + 1, out var sayPage))
					{
						return UINpcTalk.TalkType.SENDNEXT;
					}
					else
					{
						bool hasYes = currentSayStage.yesPages.Count != 0;
						bool hasNo = currentSayStage.noPages.Count != 0 || currentSayStage.stopPages.Count != 0;
						if (hasYes && hasNo)
						{
							yesPageIndex = 0;
							noPageIndex = 0;
							return UINpcTalk.TalkType.SENDYESNO;
						}
						else if (hasYes && !hasNo)
						{
							yesPageIndex = 0;
							return UINpcTalk.TalkType.SENDOK;
						}
						else if (!hasYes && !hasNo)
						{
							return UINpcTalk.TalkType.SENDACCEPTDECLINE;
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
						return UINpcTalk.TalkType.SENDNEXT;
					}
					else
					{
						/*						bool hasStop = currentSayStage.stopPages.Count != 0;
												if (hasStop)
												{
													return UINpcTalk.TalkType.SENDNEXT;
												}
												else*/
						{
							return UINpcTalk.TalkType.SENDOK;
						}
					}
				}
			}

			return UINpcTalk.TalkType.NONE;
		}
		UINpcTalk.TalkType GetNoTalkType ()
		{
			if (yesPageIndex != -1)
			{
				if (isAsk)
				{

				}
				else
				{
					//如果 有下一个对话 
					if (currentSayStage.noPages.TryGet (noPageIndex + 1, out var sayPage))
					{
						return UINpcTalk.TalkType.SENDNEXT;
					}
					else
					{
						bool hasStopPage = currentSayStage.stopPages.Count != 0;
						if (hasStopPage)
						{
							return UINpcTalk.TalkType.SENDNEXT;
						}
						else
						{
							return UINpcTalk.TalkType.SENDOK;
						}
					}
				}
			}

			return UINpcTalk.TalkType.NONE;
		}


		public void InitChooseQuestSayPage (Npc npc, SayPage sayPage)
		{
			//type = TalkType.NONE;
			currentNpc = npc;
			currentNpcId = npc.get_id ();
			pageIndex = sayPage.pageIndex;

			currentSayPage = sayPage;
		}


		private new void onClickLink (EventContext context)
		{
			short.TryParse ((string)context.data, out var seletIndex);
			if (isClientPage)
			{
				//init page
				if (pageIndex == -1)
				{
					var questInfo = currentNpc.GetQuestSayInfo (seletIndex, out var isQuestStarted);
					if (questInfo != null)
					{
						BeginQuestSay (questInfo, isQuestStarted);
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
							uINpcTalk.change_text (currentNpcId, (sbyte)UINpcTalk.TalkType.SENDOK, 0, 0, currentSayPage.wrongAnswer_index_Texts[seletIndex]);
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
	}
}