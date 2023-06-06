


namespace ms
{
	public class CashShopParser
	{
		public enum Jobs : ushort
		{
			EVAN = 2001,
			EVAN1 = 2200,
			EVAN2 = 2210,
			EVAN3 = 2211,
			EVAN4 = 2212,
			EVAN5 = 2213,
			EVAN6 = 2214,
			EVAN7 = 2215,
			EVAN8 = 2216,
			EVAN9 = 2217,
			EVAN10 = 2218
		}
		
		public static StatsEntry parseCharacterInfo(InPacket recv)
		{
			recv.read_long();
			recv.read_byte();

			StatsEntry statsentry = parseCharStats(recv);

			Player player = Stage.get().get_player();

			player.get_stats().set_mapid(statsentry.mapid);
			player.get_stats().set_portal(statsentry.portal);

			recv.read_byte(); // 'buddycap'

			if (recv.read_bool())
				recv.read_string(); // 'linkedname'

			CharacterParser.parse_inventory(recv, player.get_inventory());
			CharacterParser.parse_skillbook(recv, player.get_skills());
			CharacterParser.parse_cooldowns(recv, player);
			CharacterParser.parse_questlog(recv, player.get_questlog());
			CharacterParser.parse_minigame(recv);
			CharacterParser.parse_ring1(recv);
			CharacterParser.parse_ring2(recv);
			CharacterParser.parse_ring3(recv);
			CharacterParser.parse_teleportrock(recv, player.get_teleportrock());
			CharacterParser.parse_monsterbook(recv, player.get_monsterbook());
			CharacterParser.parse_nyinfo(recv);
			CharacterParser.parse_areainfo(recv);

			player.recalc_stats(true);

			recv.read_short();

			return statsentry;
		}

		public static StatsEntry parseCharStats(InPacket recv)
		{
			recv.read_int(); // character id

			// TODO: This is similar to LoginParser.cpp, try and merge these.
			StatsEntry statsentry = new StatsEntry ();

			statsentry.name = recv.read_padded_string(13);
			statsentry.female = recv.read_bool();

			recv.read_byte();	// skin
			recv.read_int();	// face
			recv.read_int();	// hair

			for (int i = 0; i < 3; i++)
				statsentry.petids.Add(recv.read_long());

			statsentry.stats[MapleStat.Id.LEVEL] = (ushort)recv.readByte(); // TODO: Change to recv.read_short(); to increase level cap

			var job = recv.read_short();

			statsentry.stats[MapleStat.Id.JOB] = (ushort)job;
			statsentry.stats[MapleStat.Id.STR] = (ushort)recv.read_short();
			statsentry.stats[MapleStat.Id.DEX] = (ushort)recv.read_short();
			statsentry.stats[MapleStat.Id.INT] = (ushort)recv.read_short();
			statsentry.stats[MapleStat.Id.LUK] = (ushort)recv.read_short();
			statsentry.stats[MapleStat.Id.HP] = (ushort)recv.read_short();
			statsentry.stats[MapleStat.Id.MAXHP] = (ushort)recv.read_short();
			statsentry.stats[MapleStat.Id.MP] = (ushort)recv.read_short();
			statsentry.stats[MapleStat.Id.MAXMP] = (ushort)recv.read_short();
			statsentry.stats[MapleStat.Id.AP] = (ushort)recv.read_short();

			if (hasSPTable(job))
				parseRemainingSkillInfo(recv);
			else
				recv.read_short(); // remaining sp

			statsentry.exp = recv.read_int();
			statsentry.stats[MapleStat.Id.FAME] = (ushort)recv.read_short();

			recv.skip(4); // gachaexp

			statsentry.mapid = recv.read_int();
			statsentry.portal = (byte)recv.read_byte();

			recv.skip(4); // timestamp

			return statsentry;
		}

		public static bool hasSPTable(short job)
		{
			switch ((Jobs)job)
			{
			case Jobs.EVAN:
			case Jobs.EVAN1:
			case Jobs.EVAN2:
			case Jobs.EVAN3:
			case Jobs.EVAN4:
			case Jobs.EVAN5:
			case Jobs.EVAN6:
			case Jobs.EVAN7:
			case Jobs.EVAN8:
			case Jobs.EVAN9:
			case Jobs.EVAN10:
				return true;
			default:
				return false;
			}
		}

		public static void parseRemainingSkillInfo(InPacket recv)
		{
			int count = recv.read_byte();

			for (int i = 0; i < count; i++)
			{
				recv.read_byte(); // Remaining SP index for job 
				recv.read_byte(); // The actual SP for that class
			}
		}
	}
}


