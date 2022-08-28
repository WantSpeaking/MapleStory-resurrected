namespace ms
{
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
						return (ushort)(id / 100);
					case Level.SECOND:
						return (ushort)(id / 10 * 10);
					case Level.THIRD:
						return (ushort)(level == Level.FOURTH ? id - 1 : id);
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

		public static string get_name (ushort jid)
		{
			switch (jid)
			{
				case 0:
					return "Beginner";
				case 100:
					return "Swordsman";
				case 110:
					return "Fighter";
				case 111:
					return "Crusader";
				case 112:
					return "Hero";
				case 120:
					return "Page";
				case 121:
					return "White Knight";
				case 122:
					return "Paladin";
				case 130:
					return "Spearman";
				case 131:
					return "Dragon Knight";
				case 132:
					return "Dark Knight";
				case 200:
					return "Magician";
				case 210:
					return "Wizard (F/P)";
				case 211:
					return "Mage (F/P)";
				case 212:
					return "Archmage (F/P)";
				case 220:
					return "Wizard (I/L)";
				case 221:
					return "Mage (I/L)";
				case 222:
					return "Archmage (I/L)";
				case 230:
					return "Cleric";
				case 231:
					return "Priest";
				case 232:
					return "Bishop";
				case 300:
					return "Archer";
				case 310:
					return "Hunter";
				case 311:
					return "Ranger";
				case 312:
					return "Bowmaster";
				case 320:
					return "Crossbowman";
				case 321:
					return "Sniper";
				case 322:
					return "Marksman";
				case 400:
					return "Rogue";
				case 410:
					return "Assassin";
				case 411:
					return "Hermit";
				case 412:
					return "Nightlord";
				case 420:
					return "Bandit";
				case 421:
					return "Chief Bandit";
				case 422:
					return "Shadower";
				case 500:
					return "Pirate";
				case 510:
					return "Brawler";
				case 511:
					return "Marauder";
				case 512:
					return "Buccaneer";
				case 520:
					return "Gunslinger";
				case 521:
					return "Outlaw";
				case 522:
					return "Corsair";
				case 1000:
					return "Noblesse";
				case 1100:
				case 1110:
				case 1111:
				case 1112:
					return "Dawn Warrior";
				case 1200:
				case 1210:
				case 1211:
				case 1212:
					return "Blaze Wizard";
				case 1300:
				case 1310:
				case 1311:
				case 1312:
					return "Wind Archer";
				case 1400:
				case 1410:
				case 1411:
				case 1412:
					return "Night Walker";
				case 1500:
				case 1510:
				case 1511:
				case 1512:
					return "Thunder Breaker";
				case 2000:
				case 2100:
				case 2110:
				case 2111:
				case 2112:
					return "Aran";
				case 900:
					return "GM";
				case 910:
					return "SuperGM";
				default:
					return "";
			}
		}

		private string name;
		private ushort id;
		private Level level;
	}
}
