using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using client;
using config;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using ms;
using ms.Util;
using provider;
using SD.Tools.Algorithmia.GeneralDataStructures;
using server.quest.actions;
using server.quest.requirements;
using static client.MapleQuestStatus;

namespace server.quest
{
    public class MapleQuest
    {
        private static IDictionary<int, MapleQuest> quests = new Dictionary<int, MapleQuest>();
        private static MultiValueDictionary<int, MapleQuest> quests_byNpc = new MultiValueDictionary<int, MapleQuest>();
        private static IDictionary<int, int> infoNumberQuests = new Dictionary<int, int>();
        private static IDictionary<short, int> medals = new Dictionary<short, int>();

        private static readonly ISet<short> exploitableQuests = new HashSet<short>();
        //private static SortedDictionary<short, MapleQuestSay> questId_Info_Dict = new SortedDictionary<short, MapleQuestSay> ();

        static MapleQuest()
        {
            exploitableQuests.Add((short)2338); // there are a lot more exploitable quests, they need to be nit-picked
            exploitableQuests.Add((short)3637);
            exploitableQuests.Add((short)3714);
            exploitableQuests.Add((short)21752);
        }

        protected internal IDictionary<MapleQuestRequirementType, MapleQuestRequirement> startReqs = new Dictionary<MapleQuestRequirementType, MapleQuestRequirement>();
        protected internal IDictionary<MapleQuestRequirementType, MapleQuestRequirement> completeReqs = new Dictionary<MapleQuestRequirementType, MapleQuestRequirement>();
        protected internal IDictionary<MapleQuestActionType, MapleQuestAction> startActs = new Dictionary<MapleQuestActionType, MapleQuestAction>();
        protected internal IDictionary<MapleQuestActionType, MapleQuestAction> completeActs = new Dictionary<MapleQuestActionType, MapleQuestAction>();

        protected internal LinkedList<int> relevantMobs = new LinkedList<int>();


        private static readonly MapleDataProvider questData = wz.wzProvider_quest;
        private static readonly MapleData questInfo = questData.getData("QuestInfo.img");
        private static readonly MapleData questAct = questData.getData("Act.img");
        private static readonly MapleData questReq = questData.getData("Check.img");
        private static readonly MapleData questSay = questData.getData("Say.img");
        bool repeatable;

        public MapleQuest(int id)
        {
            this.Id = (short)id;

            var reqData = questReq.getChildByPath(id.ToString());
            if (reqData == null)
            { //most likely infoEx
                return;
            }

            if (questInfo != null)
            {
                var reqInfo = questInfo.getChildByPath(id.ToString());
                if (reqInfo != null)
                {
                    /*		var tempName = reqInfo["name"]?.ToString ();
							this.Name = string.IsNullOrEmpty (tempName) ? $"name none,id:{id}" : tempName;*/
                    this.Name = MapleDataTool.getString("name", reqInfo,"");
                    this.Parent = MapleDataTool.getString("parent", reqInfo, "");

                    
                    
                    

                    this.timeLimit = MapleDataTool.getInt("timeLimit", reqInfo, 0);
                    this.timeLimit2 = MapleDataTool.getInt("timeLimit2", reqInfo, 0);
                    this.AutoStart = MapleDataTool.getInt("autoStart", reqInfo, 0) == 1;
                    this.AutoPreComplete = MapleDataTool.getInt("autoPreComplete", reqInfo, 0) == 1; 
                    this.AutoComplete = MapleDataTool.getInt("autoComplete", reqInfo, 0) == 1;

                    int medalid = MapleDataTool.getInt("viewMedalItem", reqInfo, 0);
                    if (medalid != 0)
                    {
                        medals[this.Id] = medalid;
                    }
                    
                    this.Info_started = NpcTextParser.inst.Parse(MapleDataTool.getString("0", reqInfo, ""));
                    this.Info_in_progress = NpcTextParser.inst.Parse(MapleDataTool.getString("1", reqInfo, ""));
                    this.Info_completed = NpcTextParser.inst.Parse(MapleDataTool.getString("2", reqInfo, ""));
                    this.Area = MapleDataTool.getInt("area", reqInfo, 0);
                    this.Order = MapleDataTool.getInt("order", reqInfo, 0);

                    this.oneShot = MapleDataTool.getInt("oneShot", reqInfo, 0);
                    this.Summary = NpcTextParser.inst.Parse(MapleDataTool.getString("summary", reqInfo, ""));
                    this.DemandSummary = NpcTextParser.inst.Parse(MapleDataTool.getString("demandSummary", reqInfo, ""));
                    this.RewardSummary = NpcTextParser.inst.Parse(MapleDataTool.getString("rewardSummary", reqInfo, ""));

                    this.medalCategory = MapleDataTool.getInt("medalCategory", reqInfo, 0); 
                    this.viewMedalItem = MapleDataTool.getInt("viewMedalItem", reqInfo, 0); 
                    this.timerUI = MapleDataTool.getString("timerUI", reqInfo, ""); 
                    this.selectedMob = MapleDataTool.getInt("selectedMob", reqInfo, 0); 
                    this.sortkey = MapleDataTool.getString("sortkey", reqInfo, ""); 
                    this.autoAccept = MapleDataTool.getInt("autoAccept", reqInfo, 0) == 1; 
                    this.type = MapleDataTool.getString("type", reqInfo, "");
                    this.showLayerTag = MapleDataTool.getString("showLayerTag", reqInfo, ""); 
                    this.selectedSkillID = MapleDataTool.getInt("selectedSkillID", reqInfo, 0); 
                    this.dailyPlayTime = MapleDataTool.getInt("dailyPlayTime", reqInfo, 0); 
                }
                else
                {
                    var tempName = $"Quest doesn't exist,id:{id}";
                    this.Name = tempName;
                    AppDebug.LogWarning(tempName);
                }
            }

            var startReqData = reqData.getChildByPath("0");
            if (startReqData != null)
            {
                foreach (var startReq in startReqData)
                {
                    MapleQuestRequirementType type = MapleQuestRequirementType.getByWZName(startReq.Name);
                    if (type.Equals(MapleQuestRequirementType.INTERVAL))
                    {
                        repeatable = true;
                    }
                    else if (type.Equals(MapleQuestRequirementType.MOB))
                    {
                        foreach (var mob in startReq)
                        {
                            relevantMobs.AddLast(MapleDataTool.getInt(mob.getChildByPath("id")));
                        }
                    }

                    MapleQuestRequirement req = this.getRequirement(type, startReq);
                    if (req == null)
                    {
                        continue;
                    }

                    startReqs[type] = req;
                }
            }

            var completeReqData = reqData.getChildByPath("1");
            if (completeReqData != null)
            {
                foreach (var completeReq in completeReqData)
                {
                    MapleQuestRequirementType type = MapleQuestRequirementType.getByWZName(completeReq.Name);

                    MapleQuestRequirement req = this.getRequirement(type, completeReq);
                    if (req == null)
                    {
                        continue;
                    }

                    if (type.Equals(MapleQuestRequirementType.MOB))
                    {
                        foreach (var mob in completeReq)
                        {
                            relevantMobs.AddLast(MapleDataTool.getInt(mob.getChildByPath("id")));
                        }
                    }
                    completeReqs[type] = req;
                }
            }

            var actData = questAct.getChildByPath(id.ToString());
            if (actData == null)
            {
                return;
            }

            var startActData = actData.getChildByPath("0");
            if (startActData != null)
            {
                foreach (var startAct in startActData)
                {
                    MapleQuestActionType questActionType = MapleQuestActionType.getByWZName(startAct.Name);
                    MapleQuestAction act = this.getAction(questActionType, startAct);

                    if (act == null)
                    {
                        continue;
                    }

                    startActs[questActionType] = act;
                }
            }

            var completeActData = actData.getChildByPath("1");
            if (completeActData != null)
            {
                foreach (var completeAct in completeActData)
                {
                    MapleQuestActionType questActionType = MapleQuestActionType.getByWZName(completeAct.Name);
                    MapleQuestAction act = this.getAction(questActionType, completeAct);

                    if (act == null)
                    {
                        continue;
                    }

                    completeActs[questActionType] = act;
                }
            }
        }
        private MapleQuestRequirement getRequirement(MapleQuestRequirementType type, MapleData data)
        {
            MapleQuestRequirement ret = null;
            switch (type.innerEnumValue)
            {
                /*	case server.quest.MapleQuestRequirementType.InnerEnum.END_DATE:
						ret = new EndDateRequirement (this, data);
						break;*/
                case server.quest.MapleQuestRequirementType.InnerEnum.JOB:
                    ret = new JobRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.QUEST:
                    ret = new QuestRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.FIELD_ENTER:
                    ret = new FieldEnterRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.INFO_NUMBER:
                    ret = new InfoNumberRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.INFO_EX:
                    ret = new InfoExRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.INTERVAL:
                    ret = new IntervalRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.COMPLETED_QUEST:
                    ret = new CompletedQuestRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.ITEM:
                    ret = new ItemRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.MAX_LEVEL:
                    ret = new MaxLevelRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.MESO:
                    ret = new MesoRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.MIN_LEVEL:
                    ret = new MinLevelRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.MIN_PET_TAMENESS:
                    ret = new MinTamenessRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.MOB:
                    ret = new MobRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.MONSTER_BOOK:
                    ret = new MonsterBookCountRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.NPC:
                    ret = new NpcRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.PET:
                    ret = new PetRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.BUFF:
                    ret = new BuffRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.EXCEPT_BUFF:
                    ret = new BuffExceptRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.SCRIPT:
                    ret = new ScriptRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.START:
                    ret = new StartTimeRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.END:
                    ret = new EndTimeRequirement(this, data);
                    break;
                case server.quest.MapleQuestRequirementType.InnerEnum.NORMAL_AUTO_START:
                default:
                    AppDebug.LogWarning("Unhandled Requirement Type: " + type.ToString() + " QuestID: " + this.Id);
                    //FilePrinter.printError(FilePrinter.EXCEPTION_CAUGHT, "Unhandled Requirement Type: " + type.toString() + " QuestID: " + this.getId());
                    break;
            }
            return ret;
        }

        private MapleQuestAction getAction(MapleQuestActionType type, MapleData data)
        {
            MapleQuestAction ret = null;
            switch (type.innerEnumValue)
            {
                case server.quest.MapleQuestActionType.InnerEnum.BUFF:
                    ret = new BuffAction(this, data);
                    break;
                case server.quest.MapleQuestActionType.InnerEnum.EXP:
                    ret = new ExpAction(this, data);
                    break;
                case server.quest.MapleQuestActionType.InnerEnum.FAME:
                    ret = new FameAction(this, data);
                    break;
                case server.quest.MapleQuestActionType.InnerEnum.ITEM:
                    ret = new ItemAction(this, data);
                    break;
                case server.quest.MapleQuestActionType.InnerEnum.MESO:
                    ret = new MesoAction(this, data);
                    break;
                case server.quest.MapleQuestActionType.InnerEnum.NEXTQUEST:
                    ret = new NextQuestAction(this, data);
                    break;
                case server.quest.MapleQuestActionType.InnerEnum.PETSKILL:
                    ret = new PetSkillAction(this, data);
                    break;
                case server.quest.MapleQuestActionType.InnerEnum.QUEST:
                    ret = new QuestAction(this, data);
                    break;
                case server.quest.MapleQuestActionType.InnerEnum.SKILL:
                    ret = new actions.SkillAction(this, data);
                    break;
                case server.quest.MapleQuestActionType.InnerEnum.PETTAMENESS:
                    ret = new PetTamenessAction(this, data);
                    break;
                case server.quest.MapleQuestActionType.InnerEnum.PETSPEED:
                    ret = new PetSpeedAction(this, data);
                    break;
                case server.quest.MapleQuestActionType.InnerEnum.INFO:
                    ret = new InfoAction(this, data);
                    break;
                default:
                    AppDebug.LogWarning("Unhandled Action Type: " + type.ToString() + " QuestID: " + this.Id);
                    //FilePrinter.printError(FilePrinter.EXCEPTION_CAUGHT, "Unhandled Action Type: " + type.toString() + " QuestID: " + this.getId());
                    break;
            }
            return ret;
        }

        public static MapleQuest getInstance(int id)
        {
            if (quests.ContainsKey(id))
            {
                return quests.TryGetValue(id);
            }
            else
            {
                AppDebug.LogWarning($"MapleQuest is null,id:{id}");
                return null;
            }
        }

        public bool canStartQuestByStatus(MapleCharacter chr)
        {
            MapleQuestStatus mqs = chr.getQuest(this);

            if (mqs.getStatus() == Status.NOT_STARTED)
            {
                return true;
            }
            else
            {
                if (repeatable)
                {
                    if (mqs.getStatus() == Status.COMPLETED)
                    {
                        return true;
                    }
                }
            }

            return false;
            //return !(!mqs.getStatus ().Equals (MapleQuestStatus.Status.NOT_STARTED) && !(mqs.getStatus ().Equals (MapleQuestStatus.Status.COMPLETED) && repeatable));
        }

        public virtual bool canQuestByInfoProgress(MapleCharacter chr)
        {
            MapleQuestStatus mqs = chr.getQuest(this);
            IList<string> ix = mqs.getInfoEx();
            if (ix.Count > 0)
            {
                short questid = mqs.QuestID;
                short infoNumber = mqs.InfoNumber;
                if (infoNumber <= 0)
                {
                    infoNumber = questid; // on default infoNumber mimics questid
                }

                int ixSize = ix.Count;
                for (int i = 0; i < ixSize; i++)
                {
                    string progress = chr.getQuestProgress(infoNumber, i);
                    string ixProgress = ix[i];

                    if (progress != ixProgress)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public MapleQuestRequirement getStartReqs(MapleQuestRequirementType type)
        {
            startReqs.TryGetValue(type, out var mapleQuestRequirement);
            return mapleQuestRequirement;
        }

        public MapleQuestRequirement getCompleteReqs(MapleQuestRequirementType type)
        {
            completeReqs.TryGetValue(type, out var mapleQuestRequirement);
            return mapleQuestRequirement;
        }

        public int getStartReqNpc()
        {
            return (getStartReqs(MapleQuestRequirementType.NPC) as NpcRequirement)?.get() ?? 0;
        }

        public int getCompleteReqNpc()
        {
            return (getCompleteReqs(MapleQuestRequirementType.NPC) as NpcRequirement)?.get() ?? 0;
        }
        ms.Player getPlayer()
        {
            return ms.Stage.get().get_player();
        }


        public bool canStart(MapleCharacter chr, int npcid)
        {
            if (questReq.getChildByPath(Id.ToString()) == null)//如果 check.img/id 不存在
                return false;
            if (!canStartQuestByStatus(chr))
            {
                return false;
            }

            foreach (var r in startReqs.Values)
            {
                if (!r.check(chr, npcid))
                {
                    return false;
                }
            }

            if (!canQuestByInfoProgress(chr))
            {
                return false;
            }

            return true;
        }
        public virtual bool canComplete(MapleCharacter chr, int? npcid)
        {
            MapleQuestStatus mqs = chr.getQuest(this);
            if (!mqs.getStatus().Equals(MapleQuestStatus.Status.STARTED))
            {
                return false;
            }

            foreach (MapleQuestRequirement r in completeReqs.Values)
            {
                if (!r.check(chr, npcid))
                {
                    return false;
                }
            }

            if (!canQuestByInfoProgress(chr))
            {
                return false;
            }

            return true;
        }
        public virtual short getInfoNumber(MapleQuestStatus.Status qs)
        {
            bool checkEnd = qs.Equals(MapleQuestStatus.Status.STARTED);
            IDictionary<MapleQuestRequirementType, MapleQuestRequirement> reqs = !checkEnd ? startReqs : completeReqs;

            MapleQuestRequirement req = reqs.TryGetValue(MapleQuestRequirementType.INFO_NUMBER);
            if (req != null)
            {
                InfoNumberRequirement inReq = (InfoNumberRequirement)req;
                return inReq.InfoNumber;
            }
            else
            {
                return 0;
            }
        }
        public static void loadAllQuest()
        {
            try
            {
                AppDebug.Log($"loadAllQuest start");
                int counter = 0;
                foreach (var quest in questInfo)
                {
                    int.TryParse(quest.Name,out int questID);
                    counter++;

                    AppDebug.Log($"load Quest:{quest.Name}\t{questID}\t{counter}");
                    MapleQuest q = new MapleQuest 
                        (questID);
                    quests[questID] = q;
                    //quests_byNpc.Add (questID, q);

                    int infoNumber;

                    infoNumber = q.getInfoNumber(MapleQuestStatus.Status.STARTED);
                    if (infoNumber > 0)
                    {
                        infoNumberQuests[infoNumber] = questID;
                    }

                    infoNumber = q.getInfoNumber(MapleQuestStatus.Status.COMPLETED);
                    if (infoNumber > 0)
                    {
                        infoNumberQuests[infoNumber] = questID;
                    }
                }
                AppDebug.Log($"loadAllQuest end count:{quests.Count}");
            }
            catch (Exception ex)
            {

                AppDebug.Log(ex.ToString());
                AppDebug.Log(ex.StackTrace);
            }
        }
        public IList<string> getInfoEx(Status qs)
        {
            var checkEnd = qs.Equals(Status.STARTED);
            var reqs = !checkEnd ? startReqs : completeReqs;
            try
            {
                MapleQuestRequirement req = reqs.TryGetValue(MapleQuestRequirementType.INFO_EX);
                InfoExRequirement ixReq = (InfoExRequirement)req;
                return ixReq.Info;
            }
            catch (Exception e)
            {
                return new List<string>();
            }
        }
        public void reset(MapleCharacter chr)
        {
            MapleQuestStatus newStatus = new MapleQuestStatus(this, MapleQuestStatus.Status.NOT_STARTED);
            chr.updateQuestStatus(newStatus);
        }

        public virtual bool forfeit(MapleCharacter chr)
        {
            if (!chr.getQuest(this).getStatus().Equals(MapleQuestStatus.Status.STARTED))
            {
                return false;
            }

            MapleQuestStatus newStatus = new MapleQuestStatus(this, MapleQuestStatus.Status.NOT_STARTED);
            newStatus.Forfeited = chr.getQuest(this).Forfeited + 1;
            chr.updateQuestStatus(newStatus);
            return true;
        }

        public virtual bool forceStart(MapleCharacter chr, int npc)
        {
            MapleQuestStatus newStatus = new MapleQuestStatus(this, MapleQuestStatus.Status.STARTED, npc);

            MapleQuestStatus oldStatus = chr.getQuest(this.Id);
            foreach (KeyValuePair<int, string> e in oldStatus.Progress)
            {
                newStatus.setProgress(e.Key, e.Value);
            }

            if (Id / 100 == 35 && YamlConfig.config.server.TOT_MOB_QUEST_REQUIREMENT > 0)
            {
                int setProg = 999 - Math.Min(999, YamlConfig.config.server.TOT_MOB_QUEST_REQUIREMENT);

                foreach (int? pid in newStatus.Progress.Keys)
                {
                    if (pid >= 8200000 && pid <= 8200012)
                    {
                        string pr = StringUtil.getLeftPaddedStr(Convert.ToString(setProg), '0', 3);
                        newStatus.setProgress(pid.Value, pr);
                    }
                }
            }

            newStatus.Forfeited = chr.getQuest(this).Forfeited;
            newStatus.Completed = chr.getQuest(this).Completed;

            if (timeLimit > 0)
            {
                newStatus.ExpirationTime = DateTimeHelper.CurrentUnixTimeMillis() + (timeLimit * 1000);
                chr.questTimeLimit(this, timeLimit);
            }
            if (timeLimit2 > 0)
            {
                newStatus.ExpirationTime = DateTimeHelper.CurrentUnixTimeMillis() + timeLimit2;
                chr.questTimeLimit2(this, newStatus.ExpirationTime);
            }

            chr.updateQuestStatus(newStatus);

            return true;
        }

        public virtual bool forceComplete(MapleCharacter chr, int npc)
        {
            MapleQuestStatus newStatus = new MapleQuestStatus(this, MapleQuestStatus.Status.COMPLETED, npc);
            newStatus.Forfeited = chr.getQuest(this).Forfeited;
            newStatus.Completed = chr.getQuest(this).Completed;
            newStatus.CompletionTime = DateTimeHelper.CurrentUnixTimeMillis();
            chr.updateQuestStatus(newStatus);

            return true;
        }

        public int getStartItemAmountNeeded(int itemid)
        {
            MapleQuestRequirement req = startReqs.TryGetValue(MapleQuestRequirementType.ITEM);
            if (req == null)
                return int.MinValue;

            ItemRequirement ireq = (ItemRequirement)req;
            return ireq.getItemAmountNeeded(itemid, false);
        }

        public int getCompleteItemAmountNeeded(int itemid)
        {
            MapleQuestRequirement req = completeReqs.TryGetValue(MapleQuestRequirementType.ITEM);
            if (req == null)
                return int.MaxValue;

            ItemRequirement ireq = (ItemRequirement)req;
            return ireq.getItemAmountNeeded(itemid, true);
        }
        public virtual IDictionary<int, int> getCompleteItems()
        {
            MapleQuestRequirement req = completeReqs.TryGetValue(MapleQuestRequirementType.ITEM);
            if (req == null)
                return null;

            ItemRequirement ireq = (ItemRequirement)req;

            return ireq.getCompleteItems();
        }

        public int getMobAmountNeeded(int mid)
        {
            MapleQuestRequirement req = completeReqs.TryGetValue(MapleQuestRequirementType.MOB);
            if (req == null)
                return 0;

            MobRequirement mreq = (MobRequirement)req;

            return mreq.getRequiredMobCount(mid);
        }
        public virtual IDictionary<int, int> getRequiredMobs()
        {
            MapleQuestRequirement req = completeReqs.TryGetValue(MapleQuestRequirementType.MOB);
            if (req == null)
                return null;

            MobRequirement mreq = (MobRequirement)req;

            return mreq.getRequiredMobs();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public string Info_started { get; set; }
        public string Info_in_progress { get; set; }
        public string Info_completed { get; set; }
        public int Area { get; set; }
        public int Order { get; set; }
        public bool AutoStart { get; set; }
        public bool AutoPreComplete { get; set; }
        public bool AutoComplete { get; set; }
        public int oneShot { get; set; }

        public string Summary { get; set; }
        public string DemandSummary { get; set; }
        public string RewardSummary { get; set; }
        public int medalCategory { get; set; }
        public int viewMedalItem { get; set; }
        public int timeLimit { get; set; }
        public string timerUI { get; set; }
        public int selectedMob { get; set; }
        public string sortkey { get; set; }
        public bool autoAccept { get; set; }
        /// <summary>
        /// 技能 必须 转职
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 21705 james1 james2 james3 sordQuest magicQuest thiefQuest bowQuest pirateQuest
        /// </summary>
        public string showLayerTag { get; set; }
        public int selectedSkillID { get; set; }
        public int timeLimit2 { get; set; }
        public int dailyPlayTime { get; set; }

        public bool hasParent => !string.IsNullOrEmpty(Parent);
        public string DisplayName => hasParent ? Parent : Name;

        private List<SayStage> sayStages;
        public List<SayStage> SayStages
        {
            get
            {
                if (sayStages == null)
                {
                    ParseQuestSay();
                }
                return sayStages;
            }
        }
        public SayStage GetSayStage(int index)
        {
            return SayStages.TryGet(index);
        }
        private void ParseQuestSay()
        {
            sayStages = new List<SayStage>();
            if (questSay.getChildByPath($"{Id}") is MapleData sayimg_1000)
            {
                var count = sayimg_1000.Count();
                for (int i = 0; i < count; i++)
                {
                    var sayimg_1000_0 = sayimg_1000[i.ToString()];
                    if (sayimg_1000_0 == null)
                    {
                        //4940
                        AppDebug.LogError($"Quest.wz/Check.img/{Id}/{i} is null");
                        continue;
                    }

                    var pageIndex = 0;
                    if (Id == 1013)
                    {
                        AppDebug.Log($"");
                    }
                    var sayStage = new SayStage();
                    sayStage.stageIndex = i;
                    sayStage.isAsk = sayimg_1000_0["ask"];
                    sayStage.introducePages = new SortedList<int, SayPage>();
                    sayStage.yesPages = new List<SayPage>();
                    sayStage.noPages = new List<SayPage>();
                    sayStage.stopPages = new List<SayPage>();
                    sayStages.Add(sayStage);

                    bool isAsk = sayimg_1000_0["ask"];
                    foreach (var sayimg_1000_0_0 in sayimg_1000_0)
                    {
                        if (sayimg_1000_0_0 == null)
                            continue;



                        bool isNumberIntroducePage = sayimg_1000_0_0.Name.isNumber();
                        if (isNumberIntroducePage)
                        {
                            var numberSayPage = new SayPage();

                            if (isAsk)
                            {

                                numberSayPage.text = sayimg_1000_0_0?.ToString();
                                if (sayimg_1000_0["stop"]?[sayimg_1000_0_0.Name] is MapleData sayimg_1000_0_stop_0)
                                {
                                    numberSayPage.rightAnswerChoiceNumber = sayimg_1000_0_stop_0?["answer"];
                                    numberSayPage.wrongAnswer_index_Texts = new SortedDictionary<int, string>();

                                    foreach (var sayimg_1000_0_stop_0_0 in sayimg_1000_0_stop_0)
                                    {
                                        bool isNumber_WrongAnswer = sayimg_1000_0_stop_0_0.Name.isNumber();
                                        if (isNumber_WrongAnswer)
                                        {
                                            numberSayPage.wrongAnswer_index_Texts.Add(Convert.ToInt32(sayimg_1000_0_stop_0_0.Name), sayimg_1000_0_stop_0_0?.ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    AppDebug.LogWarning($"questId:{Id} sayimg_1000_0.FullPath:{sayimg_1000_0.Name} sayimg_1000_0[stop]?[sayimg_1000_0_0.Name] is null");
                                }
                            }
                            else
                            {
                                numberSayPage.text = sayimg_1000_0_0?.ToString();
                            }
                            sayStage.introducePages.Add(int.Parse(sayimg_1000_0_0.Name), numberSayPage);
                        }
                        else
                        {
                            var stringSayPage = new SayPage();
                            switch (sayimg_1000_0_0.Name)
                            {
                                case "yes":
                                    var sayimg_1000_0_yes = sayimg_1000_0_0;
                                    for (int j = 0; j < sayimg_1000_0_yes.Count(); j++)
                                    {
                                        if (sayimg_1000_0_yes[j.ToString()] != null)
                                        {
                                            var yesSayPage = new SayPage();
                                            yesSayPage.text = sayimg_1000_0_yes[j.ToString()]?.ToString();
                                            sayStage.yesPages.Add(yesSayPage);
                                        }
                                    }
                                    break;
                                case "no":
                                    var sayimg_1000_0_no = sayimg_1000_0_0;
                                    for (int j = 0; j < sayimg_1000_0_no.Count(); j++)
                                    {
                                        if (sayimg_1000_0_no[j.ToString()] != null)
                                        {
                                            var noSayPage = new SayPage();
                                            noSayPage.text = sayimg_1000_0_no[j.ToString()]?.ToString();
                                            sayStage.noPages.Add(noSayPage);
                                        }
                                    }
                                    break;
                                case "stop":

                                    var sayimg_1000_0_stop = sayimg_1000_0_0;
                                    foreach (var sayimg_1000_0_stop_0 in sayimg_1000_0_stop)
                                    {
                                        var stopSayPage = new SayPage();

                                        bool isNumberStopPage = sayimg_1000_0_stop_0.Name.isNumber();

                                        if (isNumberStopPage)
                                        {
                                            stopSayPage.text = sayimg_1000_0_stop_0?.ToString();
                                        }
                                        else
                                        {
                                            switch (sayimg_1000_0_stop_0.Name)
                                            {
                                                case "item":
                                                    var sayimg_1000_0_stop_item = sayimg_1000_0_stop_0;
                                                    stopSayPage.text = sayimg_1000_0_stop_item["0"]?.ToString();
                                                    break;
                                                case "npc":
                                                    var sayimg_1000_0_stop_npc = sayimg_1000_0_stop_0;
                                                    stopSayPage.text = sayimg_1000_0_stop_npc["0"]?.ToString();
                                                    break;
                                                case "mob":
                                                    var sayimg_1000_0_stop_mob = sayimg_1000_0_stop_0;
                                                    stopSayPage.text = sayimg_1000_0_stop_mob["0"]?.ToString();
                                                    break;
                                                case "info":
                                                    var sayimg_1000_0_stop_info = sayimg_1000_0_stop_0;
                                                    stopSayPage.text = sayimg_1000_0_stop_info["0"]?.ToString();
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        sayStage.stopPages.Add(stopSayPage);
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
                var tempName = $"Quest check doesn't exist,id:{Id}";
                //checkInfo.Name = tempName;
                AppDebug.LogWarning(tempName);
            }
        }

        public static IDictionary<int, MapleQuest> getAllQuest()
        {
            return quests;
        }
        public static MultiValueDictionary<int, MapleQuest> getQuests_byNpc() => quests_byNpc;

    }

}