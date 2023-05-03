using System.Collections.Generic;





namespace ms
{
	// Spawn a character on the stage
	// Opcode: SPAWN_CHAR(160)
	public class SpawnCharHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			int cid = recv.read_int ();

			// We don't need to spawn the player twice
			if (Stage.get ().is_player (cid))
			{
				return;
			}

			byte level = (byte)recv.read_byte ();
			string name = recv.read_string ();

			recv.read_string (); // guildname
			recv.read_short (); // guildlogobg
			recv.read_byte (); // guildlogobgcolor
			recv.read_short (); // guildlogo
			recv.read_byte (); // guildlogocolor

			recv.skip (8);

			bool morphed = recv.read_int () == 2;
			int buffmask1 = recv.read_int ();
			short buffvalue = 0;

			if (buffmask1 != 0)
			{
				buffvalue = morphed ? recv.read_short () : recv.read_byte ();
			}

			recv.read_int (); // buffmask 2

			recv.skip (43);

			recv.read_int (); // 'mount'

			recv.skip (61);

			short job = recv.read_short ();
			LookEntry look = LoginParser.parse_look (recv);

			recv.read_int (); // count of 5110000
			recv.read_int (); // 'itemeffect'
			recv.read_int (); // 'chair'

			Point_short position = recv.read_point ();
			sbyte stance = recv.read_byte ();

			recv.skip (3);

			for (uint i = 0; i < 3; i++)
			{
				sbyte available = recv.read_byte ();

				if (available == 1)
				{
					recv.read_byte (); // 'byte2'
					recv.read_int (); // petid
					recv.read_string (); // name
					recv.read_int (); // unique id
					recv.read_int ();
					recv.read_point (); // pos
					recv.read_byte (); // stance
					recv.read_int (); // fhid
				}
				else
				{
					break;
				}
			}

			recv.read_int (); // mountlevel
			recv.read_int (); // mountexp
			recv.read_int (); // mounttiredness

			// TODO: Shop stuff
			recv.read_byte ();
			// TODO: Shop stuff end

			bool chalkboard = recv.read_bool ();
			string chalktext = chalkboard ? recv.read_string () : "";

			recv.skip (3);
			recv.read_byte (); // team

			Stage.get ().get_chars ().spawn (new CharSpawn (cid, look, level, job, name, stance, position));
		}
	}

	// Remove a character from the stage
	// Opcode: REMOVE_CHAR(161)
	public class RemoveCharHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			int cid = recv.read_int ();

			Stage.get ().get_chars ().remove (cid);
		}
	}

	// Spawn a pet on the stage
	// Opcode: SPAWN_PET(168)
	public class SpawnPetHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			int cid = recv.read_int ();
			Optional<Char> character = Stage.get ().get_character (cid);

			if (character == false)
			{
				return;
			}

			byte petindex = (byte)recv.read_byte ();
			sbyte mode = recv.read_byte ();

			if (mode == 1)
			{
				recv.skip (1);

				int itemid = recv.read_int ();
				string name = recv.read_string ();
				int uniqueid = recv.read_int ();

				recv.skip (4);

				Point_short pos = recv.read_point ();
				byte stance = (byte)recv.read_byte ();
				int fhid = recv.read_int ();

				character.get ().add_pet (petindex, itemid, name, uniqueid, pos, stance, fhid);
			}
			else if (mode == 0)
			{
				bool hunger = recv.read_bool ();

				character.get ().remove_pet (petindex, hunger);
			}
		}
	}

	// Move a character
	// Opcode: CHAR_MOVED(185)
	public class CharMovedHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			int cid = recv.read_int ();
			recv.skip (4);
			List<Movement> movements = MovementParser.parse_movements (recv);

			//AppDebug.Log ($"CharMovedHandler xpos:{movements[0].xpos}\t ypos:{movements[0].ypos}");
			Stage.get ().get_chars ().send_movement (cid, movements);
		}
	}
	
	// Update the look of a character
	// Opcode: UPDATE_CHARLOOK(197)
	public class UpdateCharLookHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			int cid = recv.read_int ();
			recv.read_byte ();
			LookEntry look = LoginParser.parse_look (recv);

			Stage.get ().get_chars ().update_look (cid, look);
		}
	}

	// Display an effect on a character
	// Opcode: SHOW_FOREIGN_EFFECT(198)
	public class ShowForeignEffectHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			int cid = recv.read_int ();
			sbyte effect = recv.read_byte ();

			if (effect == 10) // recovery
			{
				recv.read_byte (); // 'amount'
			}
			else if (effect == 13) // card effect
			{
				Stage.get ().show_character_effect (cid, CharEffect.Id.MONSTER_CARD);
			}
			else if (recv.available ()) // skill
			{
				int skillid = recv.read_int ();
				recv.read_byte (); // 'direction'
				// 9 more bytes after this

				Stage.get ().get_combat ().show_buff (cid, skillid, effect);
			}
			else
			{
				// TODO: Blank
			}
		}
	}

	// Spawn a mob on the stage
	// Opcode: SPAWN_MOB(236)
	public class SpawnMobHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			int oid = recv.read_int ();
			recv.read_byte (); // 5 if controller == null
			int id = recv.read_int ();

			recv.skip (16);

			Point_short position = recv.read_point ();
			sbyte stance = recv.read_byte ();

			recv.skip (2);

			ushort fh = (ushort)recv.read_short ();
			sbyte effect = recv.read_byte ();

			if (effect > 0)
			{
				recv.read_byte ();
				recv.read_short ();

				if (effect == 15)
				{
					recv.read_byte ();
				}
			}

			sbyte team = recv.read_byte ();

			recv.skip (4);

			//AppDebug.Log ($"SpawnMobHandler oid:{oid}\t id:{id}\t stance:{stance}\t fh:{fh}\t position:{position}");
			Stage.get ().get_mobs ().spawn (new MobSpawn (oid, id, 0, stance, fh, effect == -2, team, position));
		}
	}

	// Remove a map from the stage, either by killing it or making it invisible.
	// Opcode: KILL_MOB(237)
	public class KillMobHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			int oid = recv.read_int ();
			sbyte animation = recv.read_byte ();

			Stage.get ().get_mobs ().remove (oid, animation);
		}
	}

	// Spawn a mob on the stage and take control of it
	// Opcode: SPAWN_MOB_C(238)
	public class SpawnMobControllerHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			sbyte mode = recv.read_byte ();
			int oid = recv.read_int ();

			if (mode == 0)//makeInvis, 怪物不在附近，取消控制 
			{
				Stage.get ().get_mobs ().set_control (oid, 0);
			}
			else if (mode == 2 || mode == 1)//requestController
			{
				var aggro = mode == 2;//server mplew.write(aggro ? 2 : 1);

				if (recv.available ())
				{
					var controllerIsNull = recv.read_byte () == 5;//server mplew.write(life.getController() == null ? 5 : 1);
					Stage.get ().get_mobs ().set_control (oid, mode);
					int id = recv.read_int ();

					recv.skip (16);//encodeTemporary buffs

					Point_short position = recv.read_point ();
					sbyte stance = recv.read_byte ();

					recv.skip (2);//Origin FH //life.getStartFh()

					ushort fh = (ushort)recv.read_short ();
					sbyte effect = recv.read_byte ();

					if (effect > 0)
					{
						recv.read_byte ();
						recv.read_short ();

						if (effect == 15)
						{
							recv.read_byte ();
						}
					}

					sbyte team = recv.read_byte ();

					recv.skip (4);// getItemEffect

					Stage.get ().get_mobs ().spawn (new MobSpawn (oid, id, mode, stance, fh, effect == -2, team, position));
				}
				else
				{
					// TODO: Remove monster invisibility, not used (maybe in an event script?), Check this!
				}
			}
		}
	}

	// Update mob state and position with the client
	// Opcode: MOB_MOVED(239)
	public class MobMovedHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			int oid = recv.read_int ();

			recv.read_byte ();
			recv.read_byte (); // useskill
			recv.read_byte (); // skill
			recv.read_byte (); // skill 1
			recv.read_byte (); // skill 2
			recv.read_byte (); // skill 3
			recv.read_byte (); // skill 4

			Point_short position = recv.read_point ();
			List<Movement> movements = MovementParser.parse_movements (recv);

			Stage.get ().get_mobs ().send_movement (oid, new Point_short (position), movements);
		}
	}

	// Update mob state and position with the client
	// Opcode: MOB_MOVED(239)
	public class MoveMonsterResponseHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			int objectid = recv.read_int ();
			short moveid = recv.read_short ();
			bool useSkills = recv.read_bool ();
			short currentMp = recv.read_short ();
			sbyte skillId = recv.read_byte ();
			sbyte skillLevel = recv.read_byte ();

			/*recv.read_byte ();
			recv.read_byte (); // useskill
			recv.read_byte (); // skill
			recv.read_byte (); // skill 1
			recv.read_byte (); // skill 2
			recv.read_byte (); // skill 3
			recv.read_byte (); // skill 4

			Point_short position = recv.read_point ();
			List<Movement> movements = MovementParser.parse_movements (recv);

			Stage.get ().get_mobs ().send_movement (oid, new Point_short (position), movements);*/
		}
	}
	
	// Updates a mob's hp with the client
	// Opcode: SHOW_MOB_HP(250)
	public class ShowMobHpHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			int oid = recv.read_int ();
			sbyte hppercent = recv.read_byte ();
			ushort playerlevel = Stage.get ().get_player ().get_stats ().get_stat (MapleStat.Id.LEVEL);

			Stage.get ().get_mobs ().send_mobhp (oid, hppercent, playerlevel);
		}
	}

	// Spawn an NPC on the current stage
	// Opcode: SPAWN_NPC(257)
	public class SpawnNpcHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			//AppDebug.Log ($"SpawnNpcHandler: {recv.arr.ToDebugLog ()}");
			int oid = recv.read_int ();
			int id = recv.read_int ();
			Point_short position = recv.read_point ();
			bool flip = recv.read_bool ();
			ushort fh = (ushort)recv.read_short ();

			recv.read_short (); // 'rx'
			recv.read_short (); // 'ry'
			Stage.get ().get_npcs ().spawn (new NpcSpawn (oid, id, position, flip, fh));
		}
	}

	// Spawn an NPC on the current stage and take control of it
	// Opcode: SPAWN_NPC_C(259)
	public class SpawnNpcControllerHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			sbyte mode = recv.read_byte ();
			int oid = recv.read_int ();

			if (mode == 0)
			{
				Stage.get().get_npcs().remove(oid);
			}
			else
			{
				int id = recv.read_int ();
				Point_short position = recv.read_point ();
				bool flip = recv.read_bool ();
				ushort fh = (ushort)recv.read_short ();

				recv.read_short (); // 'rx'
				recv.read_short (); // 'ry'
				recv.read_bool (); // 'minimap'

				Stage.get ().get_npcs ().spawn (new NpcSpawn (oid, id, position, flip, fh));
			}
		}
	}

	// Drop an item on the stage
	// Opcode: DROP_LOOT(268)
	public class DropLootHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			sbyte mode = recv.read_byte ();
			int oid = recv.read_int ();
			bool isMeso = recv.read_bool ();
			int itemid = recv.read_int ();
			int owner = recv.read_int ();
			sbyte pickuptype = recv.read_byte ();
			Point_short dropto = recv.read_point ();

			recv.skip (4);

			Point_short dropfrom = new Point_short ();

			if (mode != 2)
			{
				dropfrom = new Point_short (recv.read_point ());

				recv.skip (2);

				new Sound (Sound.Name.DROP).play ();
			}
			else
			{
				dropfrom = new Point_short (dropto);
			}

			if (!isMeso)
			{
				recv.skip (8);
			}

			bool playerdrop = !recv.read_bool ();

			int meso1 = 0;
			if (recv.available (4))
			{
				meso1 = recv.read_int ();
			}
			//AppDebug.Log ($"DropLootHandler oid:{oid}\t itemid:{itemid}");
			Stage.get ().get_drops ().spawn (new DropSpawn (oid, itemid, isMeso, owner, dropfrom, dropto, pickuptype, mode, playerdrop,meso1));
		}
	}

	// Remove an item from the stage
	// Opcode: REMOVE_LOOT(269)
	public class RemoveLootHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			sbyte mode = recv.read_byte ();
			int oid = recv.read_int ();

			Optional<PhysicsObject> looter = new Optional<PhysicsObject> ();
			if (mode > 1)
			{
				int cid = recv.read_int ();
				var character = Stage.get ().get_character (cid);

				if (recv.length () > 0)
				{
					recv.read_byte (); // pet
				}
				else if (character)
				{
					looter = character.get ().get_phobj();
				}

				new Sound (Sound.Name.PICKUP).play ();
			}

			Stage.get ().get_drops ().remove (oid, mode, looter.get ());
		}
	}

	// Change state of reactor
	// Opcode: HIT_REACTOR(277)
	public class HitReactorHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			int oid = recv.read_int ();
			sbyte state = recv.read_byte ();
			Point_short point = recv.read_point ();
			sbyte stance = recv.read_byte (); // TODO: When is this different than state?
			recv.skip (2); // TODO: Unused
			recv.skip (1); // "frame" delay but this is in the WZ file?

			Stage.get ().get_reactors ().trigger (oid, state);
		}
	}

	// Parse a ReactorSpawn and send it to the Stage spawn queue
	// Opcode: SPAWN_REACTOR(279)
	public class SpawnReactorHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			int oid = recv.read_int ();
			int rid = recv.read_int ();
			sbyte state = recv.read_byte ();
			Point_short point = recv.read_point ();

			// TODO: Unused, Check this!
			// uint16_t fhid = recv.read_short();
			// recv.read_byte()

			Stage.get ().get_reactors ().spawn (new ReactorSpawn (oid, rid, state, point));
		}
	}

	// Remove a reactor from the stage
	// Opcode: REMOVE_REACTOR(280)
	public class RemoveReactorHandler : PacketHandler
	{
		public override void handle (InPacket recv)
		{
			int oid = recv.read_int ();
			sbyte state = recv.read_byte ();
			Point_short point = recv.read_point ();

			Stage.get ().get_reactors ().remove (oid, state, new Point_short (point));
		}
	}
}