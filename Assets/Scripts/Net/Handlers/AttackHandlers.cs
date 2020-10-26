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
	public class AttackHandler : PacketHandler
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void handle(InPacket& recv) const override
		public override void handle(InPacket recv)
		{
			int cid = recv.read_int();
			byte count = (byte)recv.read_byte();

			recv.skip(1);

			AttackResult attack = new AttackResult();
			attack.type = type;
			attack.attacker = cid;

			attack.level = (byte)recv.read_byte();
			attack.skill = (attack.level > 0) ? recv.read_int() : 0;

			attack.display = (byte)recv.read_byte();
			attack.toleft = recv.read_bool();
			attack.stance = (byte)recv.read_byte();
			attack.speed = (byte)recv.read_byte();

			recv.skip(1);

			attack.bullet = recv.read_int();

			attack.mobcount = (byte)((count >> 4) & 0xF);
			attack.hitcount = (byte)(count & 0xF);

			for (byte i = 0; i < attack.mobcount; i++)
			{
				int oid = recv.read_int();

				recv.skip(1);

				byte length = (attack.skill == (int)SkillId.Id.MESO_EXPLOSION) ? (byte)recv.read_byte() : attack.hitcount;

				for (byte j = 0; j < length; j++)
				{
					int damage = recv.read_int();
					bool critical = false; // TODO: ?
					var singledamage = System.Tuple.Create(damage, critical);
					attack.damagelines[oid].Add(singledamage);
				}
			}
                 
			Stage.get().get_combat().push_attack(attack);
		}

		protected AttackHandler(Attack.Type t)
		{
			type = t;
		}

		private Attack.Type type;
	}

	public class CloseAttackHandler : AttackHandler
	{
		public CloseAttackHandler() : base(Attack.Type.CLOSE)
		{
		}
	}

	public class RangedAttackHandler : AttackHandler
	{
		public RangedAttackHandler() : base(Attack.Type.RANGED)
		{
		}
	}

	public class MagicAttackHandler : AttackHandler
	{
		public MagicAttackHandler() : base(Attack.Type.MAGIC)
		{
		}
	}
}

