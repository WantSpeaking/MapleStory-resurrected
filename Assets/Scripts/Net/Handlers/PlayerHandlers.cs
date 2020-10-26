//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////



namespace ms
{
	// Handles the changing of channels for players
	// Opcode: CHANGE_CHANNEL(16)
	public class ChangeChannelHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle(InPacket recv)
		{
			LoginParser.parse_login(recv);

			/*todo var cashshop = UI.get().get_element<UICashShop>();

			if (cashshop != null)
			{
				cashshop.Dereference().exit_cashshop();
			}*/
		}
	}

	// Notifies the client of changes in character stats
	// Opcode: CHANGE_STATS(31)
	public class ChangeStatsHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle(InPacket recv)
		{
			recv.read_bool(); // 'itemreaction'
			int updatemask = recv.read_int();

			bool recalculate = false;

			foreach (var iter in MapleStat.codes)
			{
				if ((updatemask & iter.Value) != 0)
				{
					recalculate |= handle_stat(iter.Key, recv);
				}
			}

			if (recalculate)
			{
				Stage.get().get_player().recalc_stats(false);
			}

			UI.get().enable();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool handle_stat(MapleStat::Id stat, InPacket& recv) const
		private bool handle_stat(MapleStat.Id stat, InPacket recv)
		{
			Player player = Stage.get().get_player();

			bool recalculate = false;

			switch (stat)
			{
			case MapleStat.Id.SKIN:
				player.change_look(stat, recv.read_short());
				break;
			case MapleStat.Id.FACE:
			case MapleStat.Id.HAIR:
				player.change_look(stat, recv.read_int());
				break;
			case MapleStat.Id.LEVEL:
				player.change_level((ushort)recv.read_byte());
				break;
			case MapleStat.Id.JOB:
				player.change_job((ushort)recv.read_short());
				break;
			case MapleStat.Id.EXP:
				player.get_stats().set_exp(recv.read_int());
				break;
			case MapleStat.Id.MESO:
				player.get_inventory().set_meso(recv.read_int());
				break;
			default:
				player.get_stats().set_stat(stat, (ushort)recv.read_short());
				recalculate = true;
				break;
			}

			bool update_statsinfo = need_statsinfo_update(stat);

			if (update_statsinfo && !recalculate)
			{
				/*todo var statsinfo = UI.get ().get_element<UIStatsInfo> ();
				if (statsinfo)
				{
					statsinfo.update_stat(stat);
				}*/
			}

			bool update_skillbook = need_skillbook_update(stat);

			if (update_skillbook)
			{
				short value = (short)player.get_stats().get_stat(stat);
				/*todo var skillbook = UI.get ().get_element<UISkillBook> ();
				if (skillbook)
				{
					skillbook.update_stat(stat, value);
				}*/
			}

			return recalculate;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool need_statsinfo_update(MapleStat::Id stat) const
		private bool need_statsinfo_update(MapleStat.Id stat)
		{
			switch (stat)
			{
			case MapleStat.Id.JOB:
			case MapleStat.Id.STR:
			case MapleStat.Id.DEX:
			case MapleStat.Id.INT:
			case MapleStat.Id.LUK:
			case MapleStat.Id.HP:
			case MapleStat.Id.MAXHP:
			case MapleStat.Id.MP:
			case MapleStat.Id.MAXMP:
			case MapleStat.Id.AP:
				return true;
			default:
				return false;
			}
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool need_skillbook_update(MapleStat::Id stat) const
		private bool need_skillbook_update(MapleStat.Id stat)
		{
			switch (stat)
			{
			case MapleStat.Id.JOB:
			case MapleStat.Id.SP:
				return true;
			default:
				return false;
			}
		}
	}

	// Base class for packets which need to parse buff stats
	public abstract class BuffHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle(InPacket recv)
		{
			ulong firstmask = (ulong)recv.read_long();
			ulong secondmask = (ulong)recv.read_long();

			switch ((Buffstat.Id)secondmask)
			{
			case Buffstat.Id.BATTLESHIP:
				handle_buff(recv, Buffstat.Id.BATTLESHIP);
				return;
			}

			foreach (var iter in Buffstat.first_codes)
			{
				if ((firstmask & (ulong)iter.Value) != 0)
				{
					handle_buff(recv, iter.Key);
				}
			}

			foreach (var iter in Buffstat.second_codes)
			{
				if ((secondmask & (ulong)iter.Value) != 0)
				{
					handle_buff(recv, iter.Key);
				}
			}

			Stage.get().get_player().recalc_stats(false);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void handle_buff(InPacket& recv, Buffstat::Id stat) const = 0;
		protected abstract void handle_buff(InPacket recv, Buffstat.Id stat);
	}

	// Notifies the client that a buff was applied to the player
	// Opcode: GIVE_BUFF(32)
	public class ApplyBuffHandler : BuffHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle_buff(InPacket& recv, Buffstat::Id bs) const override
		protected override void handle_buff(InPacket recv, Buffstat.Id bs)
		{
			short value = recv.read_short();
			int skillid = recv.read_int();
			int duration = recv.read_int();

			Stage.get().get_player().give_buff(new Buff (bs, value, skillid, duration));
			/*todo var bufflist = UI.get ().get_element<UIBuffList> ();
			if (bufflist!=null)
			{
				bufflist.add_buff(skillid, duration);
			}*/
		}
	}

	// Notifies the client that a buff was canceled
	// Opcode: CANCEL_BUFF(33)
	public class CancelBuffHandler : BuffHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle_buff(InPacket& recv, Buffstat::Id bs) const override
		protected override void handle_buff(InPacket recv, Buffstat.Id bs)
		{
			Stage.get().get_player().cancel_buff(bs);
		}
	}

	// Force a stat recalculation
	// Opcode: RECALCULATE_STATS(35)
	public class RecalculateStatsHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket&) const override
		public override void handle(InPacket UnnamedParameter1)
		{
			Stage.get().get_player().recalc_stats(false);
		}
	}

	// Updates the player's skills with the client
	// Opcode: UPDATE_SKILL(36)
	public class UpdateSkillHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle(InPacket recv)
		{
			recv.skip(3);

			int skillid = recv.read_int();
			int level = recv.read_int();
			int masterlevel = recv.read_int();
			long expire = recv.read_long();

			Stage.get().get_player().change_skill(skillid, level, masterlevel, expire);
			/*todo var skillbook = UI.get ().get_element<UISkillBook> ();
			if (skillbook!=null)
			{
				skillbook.get ().update_skills(skillid);
			}*/

			UI.get().enable();
		}
	}

	// Parses skill macros
	// Opcode: SKILL_MACROS(124)
	public class SkillMacrosHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle(InPacket recv)
		{
			byte size = (byte)recv.read_byte();

			for (byte i = 0; i < size; i++)
			{
				recv.read_string(); // name
				recv.read_byte(); // 'shout' byte
				recv.read_int(); // skill 1
				recv.read_int(); // skill 2
				recv.read_int(); // skill 3
			}
		}
	}

	// Notifies the client that a skill is on cool-down
	// Opcode: ADD_COOLDOWN(234)
	public class AddCooldownHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle(InPacket recv)
		{
			int skill_id = recv.read_int();
			short cooltime = recv.read_short();

			Stage.get().get_player().add_cooldown(skill_id, cooltime);
		}
	}

	// Parses key mappings and sends them to the keyboard
	// Opcode: KEYMAP(335)
	public class KeymapHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle(InPacket recv)
		{
			recv.skip(1);

			for (byte i = 0; i < 90; i++)
			{
				byte type = (byte)recv.read_byte();
				int action = recv.read_int();

				UI.get().add_keymapping(i, type, action);
			}
		}
	}
}



