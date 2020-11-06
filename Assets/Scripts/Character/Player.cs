using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
	public class Player : Char, Playable
	{
		private static PlayerStandState standing = new PlayerStandState ();
		private static PlayerWalkState walking = new PlayerWalkState ();
		private static PlayerFallState falling = new PlayerFallState ();
		private static PlayerProneState lying = new PlayerProneState ();
		private static PlayerClimbState climbing = new PlayerClimbState ();
		private static PlayerSitState sitting = new PlayerSitState ();
		private static PlayerFlyState flying = new PlayerFlyState ();

		private PlayerNullState nullstate = new PlayerNullState ();

		private static PlayerState get_state (Char.State state)
		{
			switch (state)
			{
				case Char.State.STAND:
					return standing;
				case Char.State.WALK:
					return walking;
				case Char.State.FALL:
					return falling;
				case Char.State.PRONE:
					return lying;
				case Char.State.LADDER:
				case Char.State.ROPE:
					return climbing;
				case Char.State.SIT:
					return sitting;
				case Char.State.SWIM:
					return flying;
				default:
					return null;
			}
		}

		// Construct a player object from the given Character entry
		public Player (CharEntry entry) : base (entry.id, new CharLook (entry.look), entry.stats.name)
		{
			stats = new CharStats (entry.stats);
			attacking = false;
			underwater = false;

			set_state (Char.State.STAND);
			set_direction (true);
		}

		public Player () : base (0, new CharLook (), "")
		{
		}

		// Respawn the player at the given position
		public void respawn (Point<short> pos, bool uw)
		{
			set_position (pos.x (), pos.y ());
			underwater = uw;
			keysdown.Clear ();
			attacking = false;
			ladder = null;
			nullstate.update_state (this);
		}

		// Sends a Keyaction to the player's state, to apply forces, change the state and other behavior.
		public void send_action (KeyAction.Id action, bool down)
		{
			PlayerState pst = get_state (state);

			pst?.send_action (this, action, down);

			keysdown[action] = down;
		}

		// Recalculates the total stats from base stats, inventories and skills.
		public void recalc_stats (bool equipchanged)
		{
			Weapon.Type weapontype = get_weapontype ();

			stats.set_weapontype (weapontype);
			stats.init_totalstats ();

			if (equipchanged)
			{
				inventory.recalc_stats (weapontype);
			}

			foreach (EquipStat.Id enumObject in Enum.GetValues (typeof (EquipStat.Id)))
			{
				var stat = (EquipStat.Id)enumObject;
				int inventory_total = inventory.get_stat ((EquipStat.Id)stat);
				stats.add_value ((EquipStat.Id)stat, inventory_total);
			}

			/*todo var passive_skills = skillbook.collect_passives();

			foreach (var passive in passive_skills)
			{
			    int skill_id = passive.first;
			    int skill_level = passive.second;

			    passive_buffs.apply_buff(stats, skill_id, skill_level);
			}*/

			/*foreach (var pair in buffs)
			{
			    todo active_buffs.apply_buff(stats, pair.Value.stat, pair.Value.value);
			}*/

			stats.close_totalstats ();

			/*if (var statsinfo = UI.get().get_element<UIStatsInfo>())
		   {
			   statsinfo.update_all_stats();
		   }*/
		}

		// Change the equipment at the specified slot and recalculate stats
		public void change_equip (short slot)
		{
			int itemid = inventory.get_item_id (InventoryType.Id.EQUIPPED, slot);
			if (itemid != 0)
			{
				look.add_equip (itemid);
			}

			else
			{
				look.remove_equip (EquipSlot.by_id (slot));
			}
		}

		// Use the item from the player's inventory with the given id
		public void use_item (int itemid)
		{
			InventoryType.Id type = InventoryType.by_item_id (itemid);
			short slot = inventory.find_item (type, itemid);
			if (slot != null)
			{
				if (type == InventoryType.Id.USE)
				{
					new UseItemPacket(slot, itemid).dispatch();
				}
			}
		}

		// Draw the player
		public void draw (Layer.Id layer, double viewx, double viewy, float alpha)
		{
			//Debug.Log ($"Layer draw player :{(Layer.Id)get_layer ()}");
			if (layer == (Layer.Id)get_layer ())
			{
				draw (viewx, viewy, alpha);
			}
		}

		// Update the player's animation, physics and states.
		public override sbyte update (Physics physics)
		{
			PlayerState pst = get_state (state);

			if (pst != null)
			{
				pst.update (this);
				physics.move_object (phobj);

				bool aniend = base.update (physics, get_stancespeed ());

				if (aniend && attacking)
				{
					attacking = false;
					nullstate.update_state (this);
				}
				else
				{
					pst.update_state (this);
				}
			}

			byte stancebyte = (byte)(facing_right ? state : state + 1);
			Movement newmove = new Movement (phobj, stancebyte);
			bool needupdate = lastmove.hasmoved (newmove);

			if (needupdate)
			{
				//Debug.Log ($"newmove command:{newmove.command}\t xpos:{newmove.xpos}\t ypos:{newmove.ypos}");
				new MovePlayerPacket(newmove).dispatch();
				lastmove = newmove;
			}

			climb_cooldown.update ();

			return get_layer ();
		}

		// Return the Character's attacking speed
		public override sbyte get_integer_attackspeed ()
		{
			int weapon_id = look.get_equips ().get_weapon ();

			if (weapon_id <= 0)
			{
				return 0;
			}

			WeaponData weapon = WeaponData.get (weapon_id);

			sbyte base_speed = stats.get_attackspeed ();
			sbyte weapon_speed = (sbyte)weapon.get_speed ();

			return (sbyte)(base_speed + weapon_speed);
		}

		// Set flipped ignore if attacking
		public override void set_direction (bool flipped)
		{
			if (!attacking)
			{
				base.set_direction (flipped);
			}
		}

		// Set state ignore if attacking
		public override void set_state (State st)
		{
			if (!attacking)
			{
				//Debug.Log ($"{st}");

				base.set_state (st);

				PlayerState pst = get_state (st);

				pst?.initialize (this);
			}
		}

		// Return if the player is attacking
		public bool is_attacking ()
		{
			return attacking;
		}

		// Return whether the player can attack or not
		public bool can_attack ()
		{
			return !attacking && !is_climbing () && !is_sitting () && look.get_equips ().has_weapon ();
		}

		// Return whether the player can use a skill or not
		public SpecialMove.ForbidReason can_use (SpecialMove move)
		{
			if (move.is_skill () && state == Char.State.PRONE)
			{
				return SpecialMove.ForbidReason.FBR_OTHER;
			}

			if (move.is_attack () && (state == Char.State.LADDER || state == Char.State.ROPE))
			{
				return SpecialMove.ForbidReason.FBR_OTHER;
			}

			if (has_cooldown (move.get_id ()))
			{
				return SpecialMove.ForbidReason.FBR_COOLDOWN;
			}

			int level = skillbook.get_level(move.get_id());
			Weapon.Type weapon = get_weapontype ();
			Job job = stats.get_job ();
			ushort hp = stats.get_stat (MapleStat.Id.HP);
			ushort mp = stats.get_stat (MapleStat.Id.MP);
			ushort bullets = inventory.get_bulletcount ();

			return move.can_use (level, weapon, job, hp, mp, bullets);
		}

		// Create an attack struct using the player's stats
		public Attack prepare_attack (bool skill)
		{
			Attack.Type attacktype;
			bool degenerate;

			if (state == Char.State.PRONE)
			{
				degenerate = true;
				attacktype = Attack.Type.CLOSE;
			}
			else
			{
				Weapon.Type weapontype;
				weapontype = get_weapontype ();

				switch (weapontype)
				{
					case Weapon.Type.BOW:
					case Weapon.Type.CROSSBOW:
					case Weapon.Type.CLAW:
					case Weapon.Type.GUN:
					{
						degenerate = !inventory.has_projectile ();
						attacktype = degenerate ? Attack.Type.CLOSE : Attack.Type.RANGED;
						break;
					}
					case Weapon.Type.WAND:
					case Weapon.Type.STAFF:
					{
						degenerate = !skill;
						attacktype = degenerate ? Attack.Type.CLOSE : Attack.Type.MAGIC;
						break;
					}
					default:
					{
						attacktype = Attack.Type.CLOSE;
						degenerate = false;
						break;
					}
				}
			}

			Attack attack = new Attack ();
			attack.type = attacktype;
			attack.mindamage = stats.get_mindamage ();
			attack.maxdamage = stats.get_maxdamage ();

			if (degenerate)
			{
				attack.mindamage /= 10;
				attack.maxdamage /= 10;
			}

			attack.critical = stats.get_critical ();
			attack.ignoredef = stats.get_ignoredef ();
			attack.accuracy = stats.get_total (EquipStat.Id.ACC);
			attack.playerlevel = (short)stats.get_stat (MapleStat.Id.LEVEL);
			attack.range = new Rectangle<short> (stats.get_range ());
			attack.bullet = inventory.get_bulletid ();
			attack.bullet = 0;
			attack.origin = get_position ();
			attack.toleft = !facing_right;
			attack.speed = (byte)get_integer_attackspeed ();

			return attack;
		}

		// Execute a rush movement
		public void rush (double targetx)
		{
			if (phobj.onground)
			{
				ushort delay = get_attackdelay (1);
				phobj.movexuntil (targetx, delay);
				phobj.set_flag (PhysicsObject.Flag.TURNATEDGES);
			}
		}

		// Check whether the player is invincible
		public override bool is_invincible ()
		{
			if (state == Char.State.DIED)
			{
				return true;
			}

			if (has_buff (Buffstat.Id.DARKSIGHT))
			{
				return true;
			}

			return base.is_invincible ();
		}

		// Handle an attack to the player
		public MobAttackResult damage (MobAttack attack)
		{
			int damage = stats.calculate_damage (attack.watk);
			show_damage (damage);

			bool fromleft = attack.origin.x () > phobj.get_x ();

			bool missed = damage <= 0;
			bool immovable = ladder != null || state == Char.State.DIED;
			bool knockback = !missed && !immovable;

			if (knockback && randomizer.above (stats.get_stance ()))
			{
				phobj.hspeed = fromleft ? -1.5 : 1.5;
				phobj.vforce -= 3.5;
			}

			byte direction = (byte)(fromleft ? 0 : 1);

			return new MobAttackResult (attack, damage, direction);
		}

		// Apply a buff to the player
		public void give_buff (Buff buff)
		{
			buffs[buff.stat] = buff;
		}

		// Cancel a buff
		public void cancel_buff (Buffstat.Id stat)
		{
			buffs[stat] = default;
		}

		// Return whether the buff is active
		public bool has_buff (Buffstat.Id stat)
		{
			return buffs[stat].value > 0;
		}

		// Change a skill
		public void change_skill (int skill_id, int skill_level, int masterlevel, long expiration)
		{
			/*todo int old_level = skillbook.get_level(skill_id);
			skillbook.set_skill(skill_id, skill_level, masterlevel, expiration);

			if (old_level != skill_level)
			{
			    recalc_stats(false);
			}*/
		}

		// Put a skill on cooldown
		public void add_cooldown (int skill_id, int cooltime)
		{
			cooldowns[skill_id] = cooltime;
		}

		// Check if a skill is on cooldown
		public bool has_cooldown (int skill_id)
		{
			cooldowns.TryGetValue (skill_id, out var cool);
			return cool > 0;
			/* var iter = cooldowns.find(skill_id);

			 if (iter == cooldowns.end())
			 {
			     return false;
			 }

			 //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			 return iter.second > 0;*/
		}

		// Change the player's level, display the "level up" effect.
		public void change_level (ushort level)
		{
			ushort oldlevel = get_level ();

			if (level > oldlevel)
			{
				show_effect_id (CharEffect.Id.LEVELUP);
			}

			stats.set_stat (MapleStat.Id.LEVEL, level);
		}

		// Return the Character's level
		public override ushort get_level ()
		{
			return stats.get_stat (MapleStat.Id.LEVEL);
		}

		// Return the Character's level of a skill
		public override int get_skilllevel (int skillid)
		{
			//todo return skillbook.get_level(skillid);
			return 30;
		}

		// Change the player's job, display the job change effect.
		public void change_job (ushort jobid)
		{
			show_effect_id (CharEffect.Id.JOBCHANGE);
			stats.change_job (jobid);
		}

		// Change players position to the seat's position and stance to Char.State.SIT
		public void set_seat (Optional<Seat> seat)
		{
			if (seat != null)
			{
				set_position (seat.Dereference ().getpos ());
				set_state (Char.State.SIT);
			}
		}

		// Change players x-position to the ladder x and change stance to Char.State.LADDER or Char.State.ROPE
		public void set_ladder (Optional<Ladder> ldr)
		{
			//ladder = new Optional<Ladder> (ldr.get ());//todo may null
			ladder = ldr;

			if ((bool)ladder)
			{
				phobj.set_x (ldr.Dereference ().get_x ());

				phobj.hspeed = 0.0;
				phobj.vspeed = 0.0;
				phobj.fhlayer = 7;

				set_state (ldr.Dereference ().is_ladder () ? Char.State.LADDER : Char.State.ROPE);
			}
		}

		// Sets a quick cooldown on climbing so when jumping off a ladder or rope, it doesn't start climb again.
		public void set_climb_cooldown ()
		{
			climb_cooldown.set_for (1000);
		}

		// Checks if the player can climb
		public bool can_climb ()
		{
			return !(bool)climb_cooldown;
		}

		// Returns the current walking force, calculated from the total ES_SPEED stat.
		public float get_walkforce ()
		{
			return 0.05f + 0.11f * (float)stats.get_total (EquipStat.Id.SPEED) / 100 * Constants.get ().walkSpeed;
		}

		// Returns the current jumping force, calculated from the total ES_JUMP stat.
		public float get_jumpforce ()
		{
			return 1.0f + 3.5f * (float)stats.get_total (EquipStat.Id.JUMP) / 100 * Constants.get ().jumpSpeed;
		}

		// Returns the climbing force, calculated from the total ES_SPEED stat.
		public float get_climbforce ()
		{
			return (float)stats.get_total (EquipStat.Id.SPEED) / 100 * Constants.get ().climbSpeed;
		}

		// Returns the flying force
		public float get_flyforce ()
		{
			return 0.25f * Constants.get ().flySpeed;
		}

		// Return whether the player is underwater
		public bool is_underwater ()
		{
			return underwater;
		}

		// Returns if a Keyaction is currently active 
		public bool is_key_down (KeyAction.Id action)
		{
			return keysdown.Any (pair => pair.Key == action) && keysdown[action];
		}

		// Obtain a reference to the player's stats
		public CharStats get_stats ()
		{
			return stats;
		}

		// Obtain a reference to the player's inventory
		public Inventory get_inventory ()
		{
			return inventory;
		}

		// Obtain a reference to the player's skills
		public SkillBook get_skills ()
		{
			return skillbook;
		}

		// Obtain a reference to the player's QuestLog
		public QuestLog get_quests ()
		{
			return questlog;
		}

		// Obtain a reference to the player's TeleportRock locations
		public TeleportRock get_teleportrock ()
		{
			return teleportrock;
		}

		// Obtain a reference to the player's MonsterBook
		public MonsterBook get_monsterbook ()
		{
			return monsterbook;
		}

		private Inventory inventory = new Inventory ();
		private SkillBook skillbook = new SkillBook ();
		private QuestLog questlog = new QuestLog ();
		private TeleportRock teleportrock = new TeleportRock ();
		private MonsterBook monsterbook = new MonsterBook ();


		// Return a pointer to the ladder the player is on
		public Optional<Ladder> get_ladder ()
		{
			return ladder;
		}


		private CharStats stats = new CharStats ();

		private EnumMap<Buffstat.Id, Buff> buffs = new EnumMap<Buffstat.Id, Buff> ();
		/*todo private ActiveBuffs active_buffs = new ActiveBuffs();
		todo private PassiveBuffs passive_buffs = new PassiveBuffs();*/

		private Dictionary<int, int> cooldowns = new Dictionary<int, int> ();

		private SortedDictionary<KeyAction.Id, bool> keysdown = new SortedDictionary<KeyAction.Id, bool> ();

		private Movement lastmove = new Movement ();

		private Randomizer randomizer = new Randomizer ();

		private Optional<Ladder> ladder = new Optional<Ladder> ();
		private TimedBool climb_cooldown = new TimedBool ();

		private bool underwater;
	}
}