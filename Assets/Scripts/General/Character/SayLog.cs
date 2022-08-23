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
				sayInfo.questId = questId;
				sayInfo.sayStages = new List<SayStage> ();
				if (wz.wzFile_quest.GetObjectFromPath ($"Quest.wz/Say.img/{questId}") is WzSubProperty sayimg_1000)
				{
					var count = sayimg_1000.Count ();
					for (int i = 0; i < count; i++)
					{
						var sayimg_1000_0 = sayimg_1000[i.ToString ()];
						if (sayimg_1000_0 == null)
						{
							//4940
							AppDebug.LogError ($"Quest.wz/Check.img/{questId}/{i} is null");
							continue;
						}

						var pageIndex = 0;
						if (questId == 1013)
						{
							AppDebug.Log ($"");
						}
						var sayStage = new SayStage ();
						sayStage.introducePages = new List<SayPage> ();
						sayStage.yesPages = new List<SayPage> ();
						sayStage.noPages = new List<SayPage> ();
						sayStage.stopPages = new List<SayPage> ();
						sayInfo.sayStages.Add (sayStage);

						bool isAsk = sayimg_1000_0["ask"];
						foreach (var sayimg_1000_0_0 in sayimg_1000_0)
						{
							if (sayimg_1000_0_0 == null || sayimg_1000_0_0 is WzNullProperty)
								continue;

							var sayPage = new SayPage ();
							sayPage.sayPageType = isAsk ? SayPageType.Ask : SayPageType.Normal;
							sayPage.pageIndex = pageIndex++;

							bool isNumberIntroducePage = sayimg_1000_0_0.Name.isNumber ();
							if (isNumberIntroducePage)
							{
								if (isAsk)
								{
									sayPage.text = sayimg_1000_0_0?.ToString ();
									sayPage.question = sayimg_1000_0_0?.ToString ();
									if (sayimg_1000_0["stop"]?[sayimg_1000_0_0.Name] is WzImageProperty sayimg_1000_0_stop_0)
									{
										sayPage.rightAnswerChoiceNumber = sayimg_1000_0_stop_0?["answer"];
										sayPage.wrongAnswer_index_Texts = new SortedDictionary<int, string> ();

										foreach (var sayimg_1000_0_stop_0_0 in sayimg_1000_0_stop_0)
										{
											bool isNumber_WrongAnswer = sayimg_1000_0_stop_0_0.Name.isNumber ();
											if (isNumber_WrongAnswer)
											{
												sayPage.wrongAnswer_index_Texts.Add (Convert.ToInt32 (sayimg_1000_0_stop_0_0.Name), sayimg_1000_0_stop_0_0?.ToString ());
											}
										}
									}
									else
									{
										AppDebug.LogError ($"questId:{questId} sayimg_1000_0.FullPath:{sayimg_1000_0.FullPath} sayimg_1000_0[stop]?[sayimg_1000_0_0.Name] is null");
									}
								}
								else
								{
									sayPage.text = sayimg_1000_0_0?.ToString ();
								}
								sayStage.introducePages.Add (sayPage);
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
											sayPage.pageIndex = pageIndex++;
											sayPage.text = sayimg_1000_0_yes_0?.ToString ();
											sayStage.yesPages.Add (sayPage);
										}
										break;
									case "no":
										if (sayimg_1000_0_0 == null || sayimg_1000_0_0 is WzNullProperty)
											continue;
										var sayimg_1000_0_no = sayimg_1000_0_0;
										foreach (var sayimg_1000_0_no_0 in sayimg_1000_0_no)
										{
											sayPage.pageIndex = pageIndex++;
											sayPage.text = sayimg_1000_0_no_0?.ToString ();
											sayStage.noPages.Add (sayPage);
										}
										break;
									case "stop":
										if (sayimg_1000_0_0 == null || sayimg_1000_0_0 is WzNullProperty)
											continue;
										var sayimg_1000_0_stop = sayimg_1000_0_0;
										foreach (var sayimg_1000_0_stop_0 in sayimg_1000_0_stop)
										{
											bool isNumberStopPage = sayimg_1000_0_stop_0.Name.isNumber ();

											if (isNumberStopPage)
											{
												sayPage.pageIndex = pageIndex++;
												sayPage.text = sayimg_1000_0_stop_0?.ToString ();
											}
											else
											{
												switch (sayimg_1000_0_stop_0.Name)
												{
													case "item":
														var sayimg_1000_0_stop_item = sayimg_1000_0_stop_0;
														sayPage.pageIndex = pageIndex++;
														sayPage.text = sayimg_1000_0_stop_item["0"]?.ToString ();
														break;
													case "npc":
														var sayimg_1000_0_stop_npc = sayimg_1000_0_stop_0;
														sayPage.pageIndex = pageIndex++;
														sayPage.text = sayimg_1000_0_stop_npc["0"]?.ToString ();
														break;
													default:
														break;
												}
											}
											sayStage.stopPages.Add (sayPage);
										}
										break;
									default:
										break;
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
		public short questId { get; set; }
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


