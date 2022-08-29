using System;
using System.Collections.Generic;

namespace ms
{
	public class CharacterParser
	{
		static InventoryType.Id[] toparse =
		{
			InventoryType.Id.USE, InventoryType.Id.SETUP, InventoryType.Id.ETC, InventoryType.Id.CASH
		};

		public static void parse_inventory (InPacket recv, Inventory invent)
		{
			invent.set_meso (recv.read_int ());
			invent.set_slotmax (InventoryType.Id.EQUIP, (byte)recv.read_byte ());
			invent.set_slotmax (InventoryType.Id.USE, (byte)recv.read_byte ());
			invent.set_slotmax (InventoryType.Id.SETUP, (byte)recv.read_byte ());
			invent.set_slotmax (InventoryType.Id.ETC, (byte)recv.read_byte ());
			invent.set_slotmax (InventoryType.Id.CASH, (byte)recv.read_byte ());

			recv.skip (8);

			for (int i = 0; i < 3; i++)
			{
				InventoryType.Id inv = (i == 0) ? InventoryType.Id.EQUIPPED : InventoryType.Id.EQUIP;
				short pos = recv.read_short ();

				while (pos != 0)
				{
					short slot = (short)((i == 1) ? -pos : pos);
					ItemParser.parse_item (recv, inv, slot, invent);
					pos = recv.read_short ();
				}
			}

			recv.skip (2);


			for (int i = 0; i < 4; i++)
			{
				InventoryType.Id inv = toparse[i];
				sbyte pos = recv.read_byte ();

				while (pos != 0)
				{
					ItemParser.parse_item (recv, inv, pos, invent);
					pos = recv.read_byte ();
				}
			}
		}

		public static void parse_skillbook (InPacket recv, SkillBook skills)
		{
			short size = recv.read_short ();

			for (short i = 0; i < size; i++)
			{
				int skill_id = recv.read_int ();
				int level = recv.read_int ();
				long expiration = recv.read_long ();
				bool fourthtjob = ((skill_id % 100000) / 10000 == 2);
				int masterlevel = fourthtjob ? recv.read_int () : 0;
				//AppDebug.Log($"parse_skillbook, skill_id:{skill_id}\t level:{level}\t masterlevel:{masterlevel}\t expiration:{expiration}");//sever send skillId 100 which doesn't exist in skillWz; sever send  skillId 12 with skillLevel 0 which doesn't exist 
				skills.set_skill (skill_id, level, masterlevel, expiration);
			}
			AppDebug.LogError ($"parse_skillbook size:{size}");
			//todo suppose has learned skills blew
			/*skills.set_skill (Page.SWORD_ICE_BLOW, 1, 1, -1);
			skills.set_skill (Page.SWORD_FIRE_BLOW, 1, 1, -1);
			skills.set_skill (Page.SWORD_LIT_BLOW, 1, 1, -1);
			skills.set_skill (Page.SWORD_HOLY_BLOW, 1, 1, -1);

			skills.set_skill (WhiteKnight.SWORD_ICE_BLOW, 1, 1, -1);
			skills.set_skill (WhiteKnight.SWORD_FIRE_BLOW, 1, 1, -1);
			skills.set_skill (WhiteKnight.SWORD_LIT_BLOW, 1, 1, -1);
			skills.set_skill (WhiteKnight.SWORD_HOLY_BLOW, 1, 1, -1);

			skills.set_skill (Paladin.SWORD_ICE_BLOW, 1, 1, -1);
			skills.set_skill (Paladin.SWORD_FIRE_BLOW, 1, 1, -1);
			skills.set_skill (Paladin.SWORD_LIT_BLOW, 1, 1, -1);
			skills.set_skill (Paladin.SWORD_HOLY_BLOW, 1, 1, -1);*/
		}

		public static void parse_cooldowns (InPacket recv, Player player)
		{
			short size = recv.read_short ();

			for (short i = 0; i < size; i++)
			{
				int skill_id = recv.read_int ();
				int cooltime = recv.read_short ();
				player.add_cooldown (skill_id, cooltime);
			}
		}

		public static void parse_questlog (InPacket recv, QuestLog quests)
		{
			short size = recv.read_short ();

			for (short i = 0; i < size; i++)
			{
				short questId = recv.read_short ();
				string questProgressdata = recv.read_string ();
				quests.add_in_progress (questId, questProgressdata);
				/*
								if (quests.is_started (questId))
								{
									short qidl = quests.get_last_started ();
									quests.add_in_progress (qidl, questId, questProgressdata);
									//i--; // This was causing issues
								}
								else
								{
									quests.add_started (questId, questProgressdata);
								}*/

			}

			size = recv.read_short ();

			for (short i = 0; i < size; i++)
			{
				short qid = recv.read_short ();
				long time = recv.read_long ();
				quests.add_completed (qid, time);
			}
		}

		public static void parse_ring1 (InPacket recv)
		{
			short rsize = recv.read_short ();

			for (short i = 0; i < rsize; i++)
			{
				recv.read_int ();
				recv.read_padded_string (13);
				recv.read_int ();
				recv.read_int ();
				recv.read_int ();
				recv.read_int ();
			}
		}

		public static void parse_ring2 (InPacket recv)
		{
			short rsize = recv.read_short ();

			for (short i = 0; i < rsize; i++)
			{
				recv.read_int ();
				recv.read_padded_string (13);
				recv.read_int ();
				recv.read_int ();
				recv.read_int ();
				recv.read_int ();
				recv.read_int ();
			}
		}

		public static void parse_ring3 (InPacket recv)
		{
			short rsize = recv.read_short ();

			for (short i = 0; i < rsize; i++)
			{
				recv.read_int ();
				recv.read_int ();
				recv.read_int ();
				recv.read_short ();
				recv.read_int ();
				recv.read_int ();
				recv.read_padded_string (13);
				recv.read_padded_string (13);
			}
		}

		public static void parse_minigame (InPacket recv)
		{
			recv.skip (2);
		}

		public static void parse_monsterbook (InPacket recv, MonsterBook monsterbook)
		{
			monsterbook.set_cover (recv.read_int ());

			recv.skip (1);

			short size = recv.read_short ();

			for (short i = 0; i < size; i++)
			{
				short cid = recv.read_short ();
				sbyte mblv = recv.read_byte ();

				monsterbook.add_card (cid, mblv);
			}
		}

		public static void parse_teleportrock (InPacket recv, TeleportRock teleportrock)
		{
			for (int i = 0; i < 5; i++)
				teleportrock.addlocation (recv.read_int ());

			for (int i = 0; i < 10; i++)
				teleportrock.addviplocation (recv.read_int ());
		}

		public static void parse_nyinfo (InPacket recv)
		{
			short nysize = recv.read_short ();

			for (short i = 0; i < nysize; i++)
			{
				recv.read_int (); // NewYear Id
				recv.read_int (); // NewYear SenderId
				recv.read_string (); // NewYear SenderName
				recv.read_bool (); // NewYear enderCardDiscarded
				recv.read_long (); // NewYear DateSent
				recv.read_int (); // NewYear ReceiverId
				recv.read_string (); // NewYear ReceiverName
				recv.read_bool (); // NewYear eceiverCardDiscarded
				recv.read_bool (); // NewYear eceiverCardReceived
				recv.read_long (); // NewYear DateReceived
				recv.read_string (); // NewYear Message
			}
		}

		public static void parse_areainfo (InPacket recv)
		{
			Dictionary<short, string> areainfo = new Dictionary<short, string> ();
			short arsize = recv.read_short ();

			for (short i = 0; i < arsize; i++)
			{
				short area = recv.read_short ();
				areainfo.TryAdd (area, recv.read_string ());
			}
		}
	}
}