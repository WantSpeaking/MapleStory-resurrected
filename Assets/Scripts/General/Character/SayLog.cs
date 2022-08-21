using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;

namespace ms
{
	// Class that stores information on the quest log of an individual character
	public class SayLog
	{
		public SayLog ()
		{

		}

		QuestLog questLog => Stage.Instance.get_player ().get_questlog ();

		public SayInfo GetSayInfo (short questId)
		{
			if (!questId_Info_Dict.TryGetValue (questId, out var sayInfo))
			{
				var questInfo = questLog.GetQuestInfo (questId);
				sayInfo = new SayInfo ();
				sayInfo.questName = questLog.GetQuestInfo (questId).Name;
				sayInfo.sayStages = new List<SayStage> ();
				if (wz.wzFile_quest.GetObjectFromPath ($"Quest.wz/Say.img/{questId}") is WzSubProperty sayimg_1000)
				{
					var count = sayimg_1000.Count ();
					for (int i = 0; i < count; i++)
					/*{
						wzObj_Checkimg_1000[i]
					}
					foreach (var checkimg_1000_0 in wzObj_Checkimg_1000)*/
					{
						var sayimg_1000_0 = sayimg_1000[i.ToString ()];
						if (sayimg_1000_0 == null)
						{
							//4940
							AppDebug.LogError ($"Quest.wz/Check.img/{questId}/{i} is null");
							continue;
						}

						/*if (!HasDialogue (sayimg_1000_0))
							continue;*/
						var pageIndex = 0;

						var sayStage = new SayStage ();
						sayStage.introducePages = new List<SayPage> ();
						sayStage.yesPages = new List<SayPage> ();
						sayStage.noPages = new List<SayPage> ();
						sayStage.stopPages = new List<SayPage> ();
						sayInfo.sayStages.Add (sayStage);

						//每个任务都有的起始页 0
		/*				var page0 = new SayPage ();
						page0.pageIndex = pageIndex++;
						page0.text = questInfo.Name;
						page0.talkType = UINpcTalk.TalkType.SENDYESNO;
						sayStage.introducePages.Add (page0);*/

						bool isAsk = sayimg_1000_0["ask"];
						/*				if (isAsk)
										{
											var sayimg_1000_0_ask = sayimg_1000_0_0;

											var askPage = new SayPage ();
											askPage.pageIndex = pageIndex++;
											askPage.question = sayimg_1000_0_0?.ToString ();
											askPage.talkType = UINpcTalk.TalkType.SENDNEXTPREV;
											sayStage.introducePages.Add (askPage);
										}
										else*/
						{
							foreach (var sayimg_1000_0_0 in sayimg_1000_0)
							{
								if (sayimg_1000_0_0 == null || sayimg_1000_0_0 is WzNullProperty)
									continue;

								bool isNumberIntroducePage = sayimg_1000_0_0.Name.isNumber ();
								if (isNumberIntroducePage)
								{
									var numberIntroducePage = new SayPage ();
									numberIntroducePage.pageIndex = pageIndex++;
									if (isAsk)
									{
										numberIntroducePage.text = sayimg_1000_0_0?.ToString ();
										numberIntroducePage.question = sayimg_1000_0_0?.ToString ();
										if (sayimg_1000_0["stop"]?[sayimg_1000_0_0.Name] is WzImageProperty sayimg_1000_0_stop_0)
										{
											numberIntroducePage.rightAnswerChoiceNumber = sayimg_1000_0_stop_0?["answer"];

											foreach (var sayimg_1000_0_stop_0_0 in sayimg_1000_0_stop_0)
											{
												numberIntroducePage.wrongAnswer_index_Texts = new SortedDictionary<int, string> ();
												bool isNumber_WrongAnswer = sayimg_1000_0_stop_0_0.Name.isNumber ();
												if (isNumber_WrongAnswer)
												{
													numberIntroducePage.wrongAnswer_index_Texts.Add (Convert.ToInt32 (sayimg_1000_0_stop_0_0.Name), sayimg_1000_0_stop_0_0?.ToString ());
												}
											}
											numberIntroducePage.sayPageType = SayPageType.Ask;

										}
										else
										{
											numberIntroducePage.sayPageType = SayPageType.Normal;

											AppDebug.LogError ($"questId:{questId} sayimg_1000_0.FullPath:{sayimg_1000_0.FullPath} sayimg_1000_0[stop]?[sayimg_1000_0_0.Name] is null");
										}

										numberIntroducePage.talkType = UINpcTalk.TalkType.SENDNEXTPREV;

									}
									else
									{
										numberIntroducePage.text = sayimg_1000_0_0?.ToString ();
										numberIntroducePage.talkType = UINpcTalk.TalkType.SENDNEXTPREV;
										sayStage.introducePages.Add (numberIntroducePage);
									}

								}
								else
								{
									switch (sayimg_1000_0_0.Name)
									{
										case "yes":
											if (sayimg_1000_0_0 == null || sayimg_1000_0_0 is WzNullProperty)
												continue;
											var sayimg_1000_0_yes = sayimg_1000_0_0;
											foreach (var sayimg_1000_0_yes_0 in sayimg_1000_0_yes)
											{
												var numberYesPage = new SayPage ();
												numberYesPage.pageIndex = pageIndex++;


												numberYesPage.text = sayimg_1000_0_yes_0?.ToString ();
												numberYesPage.talkType = UINpcTalk.TalkType.SENDNEXTPREV;
												numberYesPage.sayPageType = SayPageType.Normal;


												sayStage.yesPages.Add (numberYesPage);
											}
											break;
										case "no":
											if (sayimg_1000_0_0 == null || sayimg_1000_0_0 is WzNullProperty)
												continue;
											var sayimg_1000_0_no = sayimg_1000_0_0;
											foreach (var sayimg_1000_0_no_0 in sayimg_1000_0_no)
											{
												var numberNoPage = new SayPage ();
												numberNoPage.pageIndex = pageIndex++;
												numberNoPage.text = sayimg_1000_0_no_0?.ToString ();
												numberNoPage.talkType = UINpcTalk.TalkType.SENDNEXTPREV;
												sayStage.noPages.Add (numberNoPage);
											}
											break;
										case "stop":
											if (sayimg_1000_0_0 == null || sayimg_1000_0_0 is WzNullProperty)
												continue;
											var sayimg_1000_0_stop = sayimg_1000_0_0;
											foreach (var sayimg_1000_0_stop_0 in sayimg_1000_0_stop)
											{
												bool isNumberStopPage = sayimg_1000_0_0.Name.isNumber ();

												if (isNumberStopPage)
												{
													var numberNoPage = new SayPage ();
													numberNoPage.pageIndex = pageIndex++;
													numberNoPage.text = sayimg_1000_0_stop_0?.ToString ();
													numberNoPage.talkType = UINpcTalk.TalkType.SENDNEXTPREV;
													sayStage.stopPages.Add (numberNoPage);
												}
												else
												{
													switch (sayimg_1000_0_stop_0.Name)
													{
														case "item":
															var sayimg_1000_0_stop_item = sayimg_1000_0_stop_0;
															var stopItemPage = new SayPage ();
															stopItemPage.pageIndex = pageIndex++;
															stopItemPage.text = sayimg_1000_0_stop_item["0"]?.ToString ();
															stopItemPage.talkType = UINpcTalk.TalkType.SENDOK;
															sayStage.stopPages.Add (stopItemPage);
															break;
														case "npc":
															var sayimg_1000_0_stop_npc = sayimg_1000_0_stop_0;
															var stopNpcPage = new SayPage ();
															stopNpcPage.pageIndex = pageIndex++;
															stopNpcPage.text = sayimg_1000_0_stop_npc["0"]?.ToString ();
															stopNpcPage.talkType = UINpcTalk.TalkType.SENDOK;
															sayStage.stopPages.Add (stopNpcPage);
															break;
														default:
															break;
													}
												}

											}
											break;
										default:
											break;
									}
								}

							}
						}


					}

				}
				else
				{
					var tempName = $"Quest check doesn't exist,id:{questId}";
					//checkInfo.Name = tempName;
					AppDebug.LogWarning (tempName);
				}
				questId_Info_Dict.Add (questId, sayInfo);
			}

			return sayInfo;
		}

		bool HasDialogue (WzImageProperty wzObj_Sayimg_1000)
		{
			return wzObj_Sayimg_1000["info"] != null || wzObj_Sayimg_1000["0"] != null || wzObj_Sayimg_1000["yes"] != null || wzObj_Sayimg_1000["no"] != null || wzObj_Sayimg_1000["ask"] != null || wzObj_Sayimg_1000["stop"] != null;
		}
		private SortedDictionary<short, SayInfo> questId_Info_Dict = new SortedDictionary<short, SayInfo> ();

	}

	public class SayInfo
	{
		public string questName { get; set; }
		public List<SayStage> sayStages { get; set; }
	}

	public class SayStage
	{
		public List<SayPage> introducePages { get; set; }
		public List<SayPage> yesPages { get; set; }
		public List<SayPage> noPages { get; set; }
		public List<SayPage> stopPages { get; set; }

	}

	public enum SayPageType
	{
		Normal,
		Ask,
		Answer
	}

	public class SayPage
	{
		public int npcId { get; set; }

		public string text { get; set; }
		public string question { get; set; }
		public int rightAnswerChoiceNumber { get; set; }
		public SortedDictionary<int, string> wrongAnswer_index_Texts { get; set; }
		public int pageIndex { get; set; }

		public UINpcTalk.TalkType talkType { get; set; }
		public SayPageType sayPageType { get; set; }
	}


}


