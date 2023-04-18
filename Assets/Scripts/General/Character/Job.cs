namespace ms
{
	public enum MapleJob
	{
		BEGINNER = 0,

		WARRIOR = 100,
		FIGHTER = 110, CRUSADER = 111, HERO = 112,
		PAGE = 120, WHITEKNIGHT = 121, PALADIN = 122,
		SPEARMAN = 130, DRAGONKNIGHT = 131, DARKKNIGHT = 132,

		MAGICIAN = 200,
		FP_WIZARD = 210, FP_MAGE = 211, FP_ARCHMAGE = 212,
		IL_WIZARD = 220, IL_MAGE = 221, IL_ARCHMAGE = 222,
		CLERIC = 230, PRIEST = 231, BISHOP = 232,
		ElementDivision = 240,

		BOWMAN = 300,
		HUNTER = 310, RANGER = 311, BOWMASTER = 312,
		CROSSBOWMAN = 320, SNIPER = 321, MARKSMAN = 322,

		THIEF = 400,
		ASSASSIN = 410, HERMIT = 411, NIGHTLORD = 412,
		BANDIT = 420, CHIEFBANDIT = 421, SHADOWER = 422,

		PIRATE = 500,
		BRAWLER = 510, MARAUDER = 511, BUCCANEER = 512,
		GUNSLINGER = 520, OUTLAW = 521, CORSAIR = 522,

		MAPLELEAF_BRIGADIER = 800,
		GM = 900, SUPERGM = 910,

		NOBLESSE = 1000,
		DAWNWARRIOR1 = 1100, DAWNWARRIOR2 = 1110, DAWNWARRIOR3 = 1111, DAWNWARRIOR4 = 1112,
		BLAZEWIZARD1 = 1200, BLAZEWIZARD2 = 1210, BLAZEWIZARD3 = 1211, BLAZEWIZARD4 = 1212,
		WINDARCHER1 = 1300, WINDARCHER2 = 1310, WINDARCHER3 = 1311, WINDARCHER4 = 1312,
		NIGHTWALKER1 = 1400, NIGHTWALKER2 = 1410, NIGHTWALKER3 = 1411, NIGHTWALKER4 = 1412,
		THUNDERBREAKER1 = 1500, THUNDERBREAKER2 = 1510, THUNDERBREAKER3 = 1511, THUNDERBREAKER4 = 1512,

		LEGEND = 2000, EVAN = 2001,
		ARAN1 = 2100, ARAN2 = 2110, ARAN3 = 2111, ARAN4 = 2112,

		EVAN1 = 2200, EVAN2 = 2210, EVAN3 = 2211, EVAN4 = 2212, EVAN5 = 2213, EVAN6 = 2214,
		EVAN7 = 2215, EVAN8 = 2216, EVAN9 = 2217, EVAN10 = 2218,

		//墨玄
		MH1 = 17500, MH2 = 17510, MH3 = 17511, MH4 = 17512
	}

	public class Job
	{
		public enum Level : ushort
		{
			BEGINNER,
			FIRST,
			SECOND,
			THIRD,
			FOURTH
		}


		MapleJob mapleJob;
		public static Level get_next_level (Level level)
		{
			switch (level)
			{
				case Level.BEGINNER:
					return Level.FIRST;
				case Level.FIRST:
					return Level.SECOND;
				case Level.SECOND:
					return Level.THIRD;
				default:
					return Level.FOURTH;
			}
		}

		public Job (ushort i)
		{
			change_job (i);
		}
		public Job ()
		{
			change_job (0);
		}

		public void change_job (ushort i)
		{
			id = i;
			name = get_name (id);
			mapleJob = (MapleJob)id;

			if (id == 0)
			{
				level = Level.BEGINNER;
			}
			else if (id % 100 == 0)
			{
				level = Level.FIRST;
			}
			else if (id % 10 == 0)
			{
				level = Level.SECOND;
			}
			else if (id % 10 == 1)
			{
				level = Level.THIRD;
			}
			else
			{
				level = Level.FOURTH;
			}
		}
		public bool is_sub_job (ushort subid)
		{
			for (int lvit = (int)Level.BEGINNER; lvit <= (int)Level.FOURTH; lvit++)
			{
				Level lv = (Level)lvit;

				if (subid == get_subjob (lv))
				{
					return true;
				}
			}

			return false;
		}

		public bool is_EquipRequiredJob (ushort requiredJobIdPlus100)
		{
			switch (requiredJobIdPlus100/100)
			{
				case 0:
					return true;
				case 1:
					return id / 100 == 1;
				case 2:
					return id / 100 == 2;
				case 4:
					return id / 100 == 3;
				case 8:
					return id / 100 == 4;
				case 16:
					return id / 100 == 5;
				default:
					break;
			}

			return false;
		}
		public bool can_use (int skill_id)
		{
			ushort required = (ushort)(skill_id / 10000);
			return is_sub_job (required);
		}
		public ushort get_id ()
		{
			return id;
		}
		public ushort get_subjob (Level lv)
		{
			if (lv <= level)
			{
				switch (lv)
				{
					case Level.BEGINNER:
						return 0;
					case Level.FIRST:
						return (ushort)((id / 100) * 100);//todo 2 reqire job is 1,but there is 100
						//return (ushort)(id / 100);
					case Level.SECOND:
						return (ushort)(id / 10 * 10);
					case Level.THIRD:
						return (ushort)(level == Level.FOURTH ? id - 1 : id);//当前是4转，id-1；当前是3转，就为id
					case Level.FOURTH:
						return id;
				}
			}

			return 0;
		}
		public Job.Level get_level ()
		{
			return level;
		}
		public string get_name ()
		{
			return name;
		}
		public EquipStat.Id get_primary (Weapon.Type weapontype)
		{
			switch (id / 100)
			{
				case 2:
					return EquipStat.Id.INT;
				case 3:
					return EquipStat.Id.DEX;
				case 4:
					return EquipStat.Id.LUK;
				case 5:
					return weapontype == Weapon.Type.GUN ? EquipStat.Id.DEX : EquipStat.Id.STR;
				default:
					return EquipStat.Id.STR;
			}
		}
		public EquipStat.Id get_secondary (Weapon.Type weapontype)
		{
			switch (id / 100)
			{
				case 2:
					return EquipStat.Id.LUK;
				case 3:
					return EquipStat.Id.STR;
				case 4:
					return EquipStat.Id.DEX;
				case 5:
					return weapontype == Weapon.Type.GUN ? EquipStat.Id.STR : EquipStat.Id.DEX;
				default:
					return EquipStat.Id.DEX;
			}
		}
		public MapleJob get_MapleJob()
		{
			return mapleJob;
		}
		public static string get_name (ushort jid)
		{
			switch (jid)
			{
				case 0:
					return TestLuban.Get().GetL10nText("Beginner");
				case 100:
					return TestLuban.Get().GetL10nText("Swordsman");
				case 110:
					return TestLuban.Get().GetL10nText("Fighter");
				case 111:
					return TestLuban.Get().GetL10nText("Crusader");
				case 112:
					return TestLuban.Get().GetL10nText("Hero");
				case 120:
					return TestLuban.Get().GetL10nText("Page");
				case 121:
					return TestLuban.Get().GetL10nText("White Knight");
				case 122:
					return TestLuban.Get().GetL10nText("Paladin");
				case 130:
					return TestLuban.Get().GetL10nText("Spearman");
				case 131:
					return TestLuban.Get().GetL10nText("Dragon Knight");
				case 132:
					return TestLuban.Get().GetL10nText("Dark Knight");
				case 200:
					return TestLuban.Get().GetL10nText("Magician");
				case 210:
					return TestLuban.Get().GetL10nText("Wizard (F/P)");
				case 211:
					return TestLuban.Get().GetL10nText("Mage (F/P)");
				case 212:
					return TestLuban.Get().GetL10nText("Archmage (F/P)");
				case 220:
					return TestLuban.Get().GetL10nText("Wizard (I/L)");
				case 221:
					return TestLuban.Get().GetL10nText("Mage (I/L)");
				case 222:
					return TestLuban.Get().GetL10nText("Archmage (I/L)");
				case 230:
					return TestLuban.Get().GetL10nText("Cleric");
				case 231:
					return TestLuban.Get().GetL10nText("Priest");
				case 232:
					return TestLuban.Get().GetL10nText("Bishop");
				case 300:
					return TestLuban.Get().GetL10nText("Archer");
				case 310:
					return TestLuban.Get().GetL10nText("Hunter");
				case 311:
					return TestLuban.Get().GetL10nText("Ranger");
				case 312:
					return TestLuban.Get().GetL10nText("Bowmaster");
				case 320:
					return TestLuban.Get().GetL10nText("Crossbowman");
				case 321:
					return TestLuban.Get().GetL10nText("Sniper");
				case 322:
					return TestLuban.Get().GetL10nText("Marksman");
				case 400:
					return TestLuban.Get().GetL10nText("Rogue");
				case 410:
					return TestLuban.Get().GetL10nText("Assassin");
				case 411:
					return TestLuban.Get().GetL10nText("Hermit");
				case 412:
					return TestLuban.Get().GetL10nText("Nightlord");
				case 420:
					return TestLuban.Get().GetL10nText("Bandit");
				case 421:
					return TestLuban.Get().GetL10nText("Chief Bandit");
				case 422:
					return TestLuban.Get().GetL10nText("Shadower");
				case 500:
					return TestLuban.Get().GetL10nText("Pirate");
				case 510:
					return TestLuban.Get().GetL10nText("Brawler");
				case 511:
					return TestLuban.Get().GetL10nText("Marauder");
				case 512:
					return TestLuban.Get().GetL10nText("Buccaneer");
				case 520:
					return TestLuban.Get().GetL10nText("Gunslinger");
				case 521:
					return TestLuban.Get().GetL10nText("Outlaw");
				case 522:
					return TestLuban.Get().GetL10nText("Corsair");
				case 1000:
					return TestLuban.Get().GetL10nText("Noblesse");
				case 1100:
				case 1110:
				case 1111:
				case 1112:
					return TestLuban.Get().GetL10nText("Dawn Warrior");//魂骑士
				case 1200:
				case 1210:
				case 1211:
				case 1212:
					return TestLuban.Get().GetL10nText("Blaze Wizard");
				case 1300:
				case 1310:
				case 1311:
				case 1312:
					return TestLuban.Get().GetL10nText("Wind Archer");
				case 1400:
				case 1410:
				case 1411:
				case 1412:
					return TestLuban.Get().GetL10nText("Night Walker");
				case 1500:
				case 1510:
				case 1511:
				case 1512:
					return TestLuban.Get().GetL10nText("Thunder Breaker");
				case 2000:
				case 2100:
				case 2110:
				case 2111:
				case 2112:
					return TestLuban.Get().GetL10nText("Aran");
				case 900:
					return TestLuban.Get().GetL10nText("GM");
				case 910:
					return TestLuban.Get().GetL10nText("SuperGM");
				case 17500:
				case 17510:
				case 17511:
				case 17512:
					return TestLuban.Get().GetL10nText("墨玄");
				default:
					return "Unknown";
			}
		}

		public bool isA (MapleJob basejob)
		{  // thanks Steve (kaito1410) for pointing out an improvement here
			var basejobId = (int)basejob;
			int basebranch = (int)basejob / 10;
			return (getId () / 10 == basebranch && getId () >= basejobId) || (basebranch % 10 == 0 && getId () / 100 == basejobId / 100);
		}
		public bool isJob (MapleJob job)
		{  
			return getId () == (ushort)job;
		}
        public MapleJob get_BaseJob()
        {
            //112
            return (MapleJob)(getId() / 100 * 100);
        }

        public ushort getId () => id;
		public bool isFourthJob ()
		{
			if (id == 2212)
			{
				return false;
			}
			if (id == 22170001 || id == 22171003 || id == 22171004 || id == 22181002 || id == 22181003)
			{
				return true;
			}
			return id % 10 == 2;
		}

		private string name;
		private ushort id;
		private Level level;
	}
}
