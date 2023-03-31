using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using server.quest;

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
				var questInfo = MapleQuest.getInstance (questId);
				sayInfo = new SayInfo ();
				sayInfo.questName = questInfo.Name;
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
						sayStage.stageIndex = i;
						sayStage.isAsk = sayimg_1000_0["ask"];
						sayStage.introducePages = new SortedList<int, SayPage> ();
						sayStage.yesPages = new List<SayPage> ();
						sayStage.noPages = new List<SayPage> ();
						sayStage.stopPages = new List<SayPage> ();
						sayInfo.sayStages.Add (sayStage);

						bool isAsk = sayimg_1000_0["ask"];
						foreach (var sayimg_1000_0_0 in sayimg_1000_0)
						{
							if (sayimg_1000_0_0 == null || sayimg_1000_0_0 is WzNullProperty)
								continue;



							bool isNumberIntroducePage = sayimg_1000_0_0.Name.isNumber ();
							if (isNumberIntroducePage)
							{
								var numberSayPage = new SayPage ();

								if (isAsk)
								{

									numberSayPage.text = sayimg_1000_0_0?.ToString ();
									if (sayimg_1000_0["stop"]?[sayimg_1000_0_0.Name] is WzImageProperty sayimg_1000_0_stop_0)
									{
										numberSayPage.rightAnswerChoiceNumber = sayimg_1000_0_stop_0?["answer"];
										numberSayPage.wrongAnswer_index_Texts = new SortedDictionary<int, string> ();

										foreach (var sayimg_1000_0_stop_0_0 in sayimg_1000_0_stop_0)
										{
											bool isNumber_WrongAnswer = sayimg_1000_0_stop_0_0.Name.isNumber ();
											if (isNumber_WrongAnswer)
											{
												numberSayPage.wrongAnswer_index_Texts.Add (Convert.ToInt32 (sayimg_1000_0_stop_0_0.Name), sayimg_1000_0_stop_0_0?.ToString ());
											}
										}
									}
									else
									{
										AppDebug.LogWarning ($"questId:{questId} sayimg_1000_0.FullPath:{sayimg_1000_0.FullPath} sayimg_1000_0[stop]?[sayimg_1000_0_0.Name] is null");
									}
								}
								else
								{
									numberSayPage.text = sayimg_1000_0_0?.ToString ();
								}
								sayStage.introducePages.Add (int.Parse (sayimg_1000_0_0.Name), numberSayPage);
							}
							else
							{
								var stringSayPage = new SayPage ();
								switch (sayimg_1000_0_0.Name)
								{
									case "yes":
										var sayimg_1000_0_yes = sayimg_1000_0_0;
										for (int j = 0; j < sayimg_1000_0_yes.Count (); j++)
										{
											if (sayimg_1000_0_yes[j.ToString ()] != null)
											{
												var yesSayPage = new SayPage ();
												yesSayPage.text = sayimg_1000_0_yes[j.ToString ()]?.ToString ();
												sayStage.yesPages.Add (yesSayPage);
											}
										}
										break;
									case "no":
										var sayimg_1000_0_no = sayimg_1000_0_0;
										for (int j = 0; j < sayimg_1000_0_no.Count (); j++)
										{
											if (sayimg_1000_0_no[j.ToString ()] != null)
											{
												var noSayPage = new SayPage ();
												noSayPage.text = sayimg_1000_0_no[j.ToString ()]?.ToString ();
												sayStage.noPages.Add (noSayPage);
											}
										}
										break;
									case "stop":

										var sayimg_1000_0_stop = sayimg_1000_0_0;
										foreach (var sayimg_1000_0_stop_0 in sayimg_1000_0_stop)
										{
											var stopSayPage = new SayPage ();

											bool isNumberStopPage = sayimg_1000_0_stop_0.Name.isNumber ();

											if (isNumberStopPage)
											{
												stopSayPage.text = sayimg_1000_0_stop_0?.ToString ();
											}
											else
											{
												switch (sayimg_1000_0_stop_0.Name)
												{
													case "item":
														var sayimg_1000_0_stop_item = sayimg_1000_0_stop_0;
														stopSayPage.text = sayimg_1000_0_stop_item["0"]?.ToString ();
														break;
													case "npc":
														var sayimg_1000_0_stop_npc = sayimg_1000_0_stop_0;
														stopSayPage.text = sayimg_1000_0_stop_npc["0"]?.ToString ();
														break;
													case "mob":
														var sayimg_1000_0_stop_mob = sayimg_1000_0_stop_0;
														stopSayPage.text = sayimg_1000_0_stop_mob["0"]?.ToString ();
														break;
													case "info":
														var sayimg_1000_0_stop_info = sayimg_1000_0_stop_0;
														stopSayPage.text = sayimg_1000_0_stop_info["0"]?.ToString ();
														break;
													default:
														break;
												}
											}
											sayStage.stopPages.Add (stopSayPage);
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
		/// <summary>
		/// 0未开始;1进行中
		/// </summary>
		public int stageIndex { get; set; }
		public bool isAsk { get; set; }
		public SortedList<int, SayPage> introducePages { get; set; }
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
		public int rightAnswerChoiceNumber { get; set; }
		public SortedDictionary<int, string> wrongAnswer_index_Texts { get; set; }
		/// <summary>
		/// -1:Init Page
		/// </summary>
		public int pageIndex { get; set; }
		
		public string ScriptInfo { get; set; }
	}


}


