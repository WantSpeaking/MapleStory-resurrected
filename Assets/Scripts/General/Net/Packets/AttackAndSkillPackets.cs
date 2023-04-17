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


using constants.skills;
using ms.Helper;
using Utility;

namespace ms
{
	// Notifies the server of an attack
	// The opcode is determined by the attack type
	// Attack::CLOSE = CLOSE_ATTACK(44)
	// Attack::RANGED = RANGED_ATTACK(45)
	// Attack::MAGIC = MAGIC_ATTACK(46)
	public class AttackPacket : OutPacket
	{
		public AttackPacket(AttackResult attack) : base((short)opcodefor(attack.type))
		{
			skip(1);
            if (attack.skill == ChiefBandit.MESO_EXPLOSION)
			{
				if (attack.mobcount == 0)
				{
					//attack.hitcount = 0;
				}
				else
				{
					//attack.hitcount = 1; 
                }
			}
			int numAttackedAndDamage = ((attack.mobcount << 4) | attack.hitcount);
            int numAttacked = attack.mobcount;
            int numDamage = attack.hitcount;

            write_byte((sbyte)numAttackedAndDamage);
			write_int(attack.skill);

			if (attack.charge > 0)
			{
				write_int(attack.charge);
			}

			skip(8);

			write_byte((sbyte)attack.display);
			write_byte(attack.toleft.ToSByte ());
			write_byte((sbyte)attack.stance);

			if (attack.skill == ChiefBandit.MESO_EXPLOSION)
			{
                var bulletIdList = ms.Stage.get().get_drops().find_loot_inRange(attack.range, 1);
                if (numAttackedAndDamage == 0)
				{
                    skip(10);

					write_byte((sbyte)bulletIdList.Count);
					foreach (var bulletId in bulletIdList)
					{
						write_int(bulletId);
						skip(1);
					} 
					return;
                }
				else
				{
                    skip(6);
                }

                for (int i = 0; i < numAttacked+1; i++)
                {
	                var oid= attack.damagelines.Keys.TryGet (i);
	                write_int (oid);
	                if (i<numAttacked)
	                {
		                skip(12);

		                write_byte((sbyte)attack.damagelines[oid].Count);
		                foreach (var singledamage in attack.damagelines[oid])
		                {
			                write_int(singledamage.Item1);
			                // TODO: Add critical here
		                }

		                skip(4);
	                }
	                else
	                {
		                write_byte((sbyte)bulletIdList.Count);
		                foreach (var bulletId in bulletIdList)
		                {
			                write_int(bulletId);
			                skip(1);
		                }
	                }
                }
                /*foreach (var damagetomob in attack.damagelines)
                {
                    write_int(damagetomob.Key);

                    skip(12);

					write_byte((sbyte)damagetomob.Value.Count);
                    foreach (var singledamage in damagetomob.Value)
                    {
                        write_int(singledamage.Item1);
                        // TODO: Add critical here
                    }

                    skip(4);
                }

                write_int(0);
                write_byte((sbyte)bulletIdList.Count);
                foreach (var bulletId in bulletIdList)
                {
                    write_int(bulletId);
                    skip(1);
                }*/
			}
			else
			{
                skip(1);

                write_byte((sbyte)attack.speed);

                if (attack.type == Attack.Type.Ranged)
                {
                    skip(1);
                    write_byte(attack.toleft.ToSByte());
                    skip(7);
                    // TODO: skip(4); If hurricane, piercing arrow or rapidfire.
                    if (attack.skill == Bowmaster.HURRICANE || attack.skill == Marksman.PIERCING_ARROW || attack.skill == Corsair.RAPID_FIRE || attack.skill == WindArcher.HURRICANE)
                    {
                        skip(4);
                    }
                }
                else
                {
                    skip(4);
                }

                foreach (var damagetomob in attack.damagelines)
                {
                    write_int(damagetomob.Key);

                    skip(14);

                    foreach (var singledamage in damagetomob.Value)
                    {
                        write_int(singledamage.Item1);
                        // TODO: Add critical here
                    }

                    if (attack.skill != 5221004)
                    {
                        skip(4);
                    }
                }
            }
			
		}

		private static OutPacket.Opcode opcodefor(Attack.Type type)
		{
			switch (type)
			{
			case Attack.Type.Close_Range:
				return OutPacket.Opcode.Close_Range_Attack;
			case Attack.Type.Ranged:
				return OutPacket.Opcode.RANGED_ATTACK;
			default:
				return OutPacket.Opcode.MAGIC_ATTACK;
			}
		}
	}

	// Tells the server that the player took damage
	// Opcode: TAKE_DAMAGE(48)
	public class TakeDamagePacket : OutPacket
	{
		public enum From : sbyte
		{
			TOUCH = -1
		}

		public TakeDamagePacket(sbyte from, byte element, int damage, int mobid, int oid, byte direction) : base((short)OutPacket.Opcode.TAKE_DAMAGE)
		{
			write_time();
			write_byte(from);
			write_byte((sbyte)element);
			write_int(damage);
			write_int(mobid);
			write_int(oid);
			write_byte((sbyte)direction);
		}

		// From mob attack result
		public TakeDamagePacket(MobAttackResult result, From from) : this((sbyte)@from, 0, result.damage, result.mobid, result.oid, result.direction)
		{
		}
	}

	// Packet which notifies the server of a skill usage
	// Opcode: USE_SKILL(91)
	public class UseSkillPacket : OutPacket
	{
		public UseSkillPacket(int skillid, int level) : base((short)OutPacket.Opcode.Special_Move)
		{
			write_time();
			write_int(skillid);
			write_byte((sbyte)level);

			// If monster magnet : some more bytes

			if (skillid % 10000000 == 1004)
			{
				skip(2); // TODO: No idea what this could be
			}

			// TODO: A point (4 bytes) could be added at the end
		}
	}

	//Skill_Effect 93
	public class SkillEffectPacket : OutPacket
	{
		public SkillEffectPacket (int skillid, int level, sbyte flags,int speed, sbyte aids) : base ((short)OutPacket.Opcode.Skill_Effect)
		{
			write_int (skillid);
			write_byte ((sbyte)level);
			write_byte (flags);
			write_int (speed);
			write_byte (aids);

		}
	}

	//Cancel_Buff 92
	public class Cancel_BuffPacket : OutPacket
	{
		public Cancel_BuffPacket (int skillid) : base ((short)OutPacket.Opcode.Cancel_Buff)
		{
			write_int (skillid);
		}
	}
}