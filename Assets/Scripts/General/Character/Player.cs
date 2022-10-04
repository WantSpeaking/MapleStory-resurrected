using System;
using System.Collections.Generic;
using System.Linq;
using constants.skills;
using UnityEngine;

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

		private static PlayerState get_state (State state)
		{
			switch (state)
			{
				case State.STAND:
					return standing;
				case State.WALK:
					return walking;
				case State.FALL:
					return falling;
				case State.PRONE:
					return lying;
				case State.LADDER:
				case State.ROPE:
					return climbing;
				case State.SIT:
					return sitting;
				case State.SWIM:
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

			set_state (State.STAND);
			set_direction (true);

			GameObject.DontDestroyOnLoad (MapGameObject);
			//MapGameObject.SetParent (MapleStory.Instance.gameObject);
		}
		public Player () : base (0, new CharLook (), "")
		{
		}

		// Respawn the player at the given position
		public void respawn (Point_short pos, bool uw)
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
				var stat = enumObject;
				int inventory_total = inventory.get_stat (stat);
				stats.add_value (stat, inventory_total);
			}

			var passive_skills = skillbook.collect_passives ();

			foreach (var passive in passive_skills)
			{
				int skill_id = passive.Key;
				int skill_level = passive.Value;

				passive_buffs.apply_buff (stats, skill_id, skill_level);
			}

			foreach (var pair in buffs)
			{
				active_buffs.apply_buff (stats, pair.Value.stat, pair.Value.value);
			}

			stats.close_totalstats ();

			var statsinfo = UI.get ().get_element<UIStatsInfo> ();
			if (statsinfo)
			{
				statsinfo.get ().update_all_stats ();
			}
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
					new UseItemPacket (slot, itemid).dispatch ();
				}
			}
		}

		// Draw the player
		public void draw (Layer.Id layer, double viewx, double viewy, float alpha)
		{
			foreach (var pair in buffs)
			{
				active_buffs.Draw (stats, pair.Value.stat, pair.Value.value);
			}

			//AppDebug.Log ($"Layer draw player :{(Layer.Id)get_layer ()}");
			if (layer == (Layer.Id)get_layer ())
			{
				draw (viewx, viewy, alpha);
			}


		}

		// Update the player's animation, physics and states.
		public override sbyte update (Physics physics)
		{
			StatusStorage.Update (Constants.TIMESTEP / 1000f);

			foreach (var pair in buffs)
			{
				active_buffs.Update (stats, pair.Value.stat, pair.Value.value);
			}

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
				//AppDebug.Log ($"newmove command:{newmove.command}\t xpos:{newmove.xpos}\t ypos:{newmove.ypos}");
				new MovePlayerPacket (newmove).dispatch ();
				lastmove = newmove;
			}

			climb_cooldown.update ();

			if (cooldowns.Count > 0)
			{
				skillCooldown_all.Clear ();
				skillCooldown_toBeRemoved.Clear ();
				skillCooldown_all.AddRange (cooldowns.Keys);
				for (int i = 0; i < cooldowns.Count; i++)
				{
					var skillId = skillCooldown_all[i];
					var cooldown = cooldowns[skillId];
					cooldown -= UnityEngine.Time.fixedDeltaTime;
					if (cooldown < 0)
					{
						skillCooldown_toBeRemoved.Add (skillId);
					}
					cooldowns[skillId] = cooldown;
					//AppDebug.Log ($"skillId:{skillId} cooldown:{cooldown}");
					i++;
				}
				/*foreach (var pair in cooldowns)
				{
					var skillId = pair.Key;
					var cooldown = pair.Value;
					cooldown -= UnityEngine.Time.fixedDeltaTime;
					if (cooldown < 0)
					{
						skillCooldown_toBeRemoved.Add (skillId);
					}
					cooldowns[skillId] = cooldown;
				}*/

				foreach (var skillId in skillCooldown_toBeRemoved)
				{
					cooldowns.Remove (skillId);
				}
			}


			return get_layer ();
		}
		List<int> skillCooldown_toBeRemoved = new List<int> ();
		List<int> skillCooldown_all = new List<int> ();
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
				//AppDebug.Log ($"{st}");

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
		public bool can_attack (int move_id, bool isForce = false)
		{
			bool isAttacking = false;
			if (isForce)
			{
				if (has_cooldown (move_id))
				{
					isAttacking = true;
				}
			}
			else
			{
				isAttacking = attacking;
			}
			return !isAttacking && !is_climbing () && !is_sitting () && look.get_equips ().has_weapon ();
		}

		// Return whether the player can use a skill or not
		public SpecialMove.ForbidReason can_use (SpecialMove move)
		{
			if (move == null)
			{
				return SpecialMove.ForbidReason.FBR_OTHER;
			}

			if (!look.get_equips ().has_weapon ())
			{
				return SpecialMove.ForbidReason.FBR_OTHER;
			}

			if (move.is_skill () && state == State.PRONE)
			{
				return SpecialMove.ForbidReason.FBR_OTHER;
			}

			if (move.is_skill () && SkillForbid.get ().IsForbid (move.get_id (), this))
			{
				return SpecialMove.ForbidReason.FBR_OTHER;
			}

			if (move.is_attack () && (state == State.LADDER || state == State.ROPE || state == State.SIT))
			{
				return SpecialMove.ForbidReason.FBR_OTHER;
			}

			if (has_cooldown (move.get_id ()))
			{
				return SpecialMove.ForbidReason.FBR_COOLDOWN;
			}

			int level = skillbook.get_level (move.get_id ());
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

			if (state == State.PRONE)
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
			attack.range = new Rectangle_short (stats.get_range ());
			attack.bullet = inventory.get_bulletid ();
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
				phobj.set_flag (PhysicsObject.Flag.TURNATEDGES);//turn at edges
			}
		}

		// Check whether the player is invincible
		public override bool is_invincible ()
		{
			if (state == State.DIED)
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
			bool immovable = ladder != false || state == State.DIED;
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

			foreach (var pair in buffs)
			{
				active_buffs.Add (stats, pair.Value.stat, pair.Value.value);
			}
		}

		// Cancel a buff
		public void cancel_buff (Buffstat.Id stat)
		{
			buffs[stat] = default;

			foreach (var pair in buffs)
			{
				active_buffs.Remove (stats, pair.Value.stat, pair.Value.value);
			}
		}

		// Return whether the buff is active
		public bool has_buff (Buffstat.Id stat)
		{
			return buffs[stat].value > 0;
		}
		public Buff get_buff (Buffstat.Id buffId)
		{
			if (!buffs.TryGetValue (buffId, out var buff))
			{
				buff = new Buff ();
			}

			return buff;
		}

		/// <summary>
		/// has SOULARROW buff
		/// </summary>
		/// <returns></returns>
		public bool can_useBow_withoutArrows ()
		{
			return get_buff (Buffstat.Id.SOULARROW).IsValid;
		}
		// Change a skill
		public void change_skill (int skill_id, int skill_level, int masterlevel, long expiration)
		{
			int old_level = skillbook.get_level (skill_id);
			skillbook.set_skill (skill_id, skill_level, masterlevel, expiration);

			if (old_level != skill_level)
			{
				recalc_stats (false);
			}
		}

		// Put a skill on cooldown
		public void add_cooldown (int skill_id, float cooltime)
		{
			cooldowns[skill_id] = cooltime;
		}
		public void remove_cooldown (int skill_id)
		{
			cooldowns.Remove(skill_id);
		}
		// Check if a skill is on cooldown
		public bool has_cooldown (int skill_id)
		{
			cooldowns.TryGetValue (skill_id, out var cool);
			return cool > 0;
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
			return skillbook.get_level (skillid);
			//return 30;
		}
		public bool has_learned_skill (int skillid)
		{
			return get_skilllevel (skillid) > 0;
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
			if (seat)
			{
				set_position (seat.get ().getpos ());
				set_state (State.SIT);
			}
		}

		// Change players x-position to the ladder x and change stance to Char.State.LADDER or Char.State.ROPE
		public void set_ladder (Optional<Ladder> ldr)
		{
			ladder = ldr;

			if ((bool)ladder)
			{
				phobj.set_x (ldr.get ().get_x ());

				phobj.hspeed = 0.0;
				phobj.vspeed = 0.0;
				phobj.fhlayer = 7;

				set_state (ldr.get ().is_ladder () ? State.LADDER : State.ROPE);
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
			return 0.05f + 0.11f * stats.get_total (EquipStat.Id.SPEED) / 100 * Constants.get ().walkSpeed;
		}

		// Returns the current jumping force, calculated from the total ES_JUMP stat.
		public float get_jumpforce ()
		{
			return 1.0f + 3.5f * stats.get_total (EquipStat.Id.JUMP) / 100 * Constants.get ().jumpSpeed;
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
		public QuestLog get_questlog ()
		{
			return questlog;
		}
		public CheckLog get_checklog ()
		{
			return checkLog;
		}
		public SayLog get_saylog ()
		{
			return saylog;
		}
		public Quest get_quest ()
		{
			return quest ??= new Quest ();
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

		public float gethppercent ()
		{
			short hp = (short)stats.get_stat (MapleStat.Id.HP);
			int maxhp = stats.get_total (EquipStat.Id.HP);

			return (float)hp / maxhp;
		}

		private Inventory inventory = new Inventory ();
		private SkillBook skillbook = new SkillBook ();
		private QuestLog questlog = new QuestLog ();
		private CheckLog checkLog = new CheckLog ();
		private SayLog saylog = new SayLog ();

		private Quest quest;

		private TeleportRock teleportrock = new TeleportRock ();
		private MonsterBook monsterbook = new MonsterBook ();


		// Return a pointer to the ladder the player is on
		public Optional<Ladder> get_ladder ()
		{
			return ladder;
		}


		private CharStats stats = new CharStats ();

		private EnumMap<Buffstat.Id, Buff> buffs = new EnumMap<Buffstat.Id, Buff> ();
		private ActiveBuffs active_buffs = new ActiveBuffs ();
		private PassiveBuffs passive_buffs = new PassiveBuffs ();

		private Dictionary<int, float> cooldowns = new Dictionary<int, float> ();

		private SortedDictionary<KeyAction.Id, bool> keysdown = new SortedDictionary<KeyAction.Id, bool> ();

		private Movement lastmove;

		private Randomizer randomizer = new Randomizer ();

		private Optional<Ladder> ladder = new Optional<Ladder> ();
		private TimedBool climb_cooldown = new TimedBool ();

		private bool underwater;

		public LocalStatusStorage statusStorage;
		public LocalStatusStorage StatusStorage => statusStorage ?? (statusStorage = new LocalStatusStorage (this));

		private LocalStatusSettingObject CHARGE_ICE_BLOW_StatusSettingObject = new LocalStatusSettingObject ();
		public LocalStatusSetting CHARGE_ICE_BLOW_StatusSetting => CHARGE_ICE_BLOW_StatusSettingObject.GetSetting ();

		private LocalStatusSettingObject CHARGE_FIRE_BLOW_StatusSettingObject = new LocalStatusSettingObject ();
		public LocalStatusSetting CHARGE_FIRE_BLOW_StatusSetting => CHARGE_FIRE_BLOW_StatusSettingObject.GetSetting ();

		private LocalStatusSettingObject CHARGE_LIT_BLOW_StatusSettingObject = new LocalStatusSettingObject ();
		public LocalStatusSetting CHARGE_LIT_BLOW_StatusSetting => CHARGE_LIT_BLOW_StatusSettingObject.GetSetting ();

		private LocalStatusSettingObject CHARGE_LIT__HOLY_StatusSettingObject = new LocalStatusSettingObject ();
		public LocalStatusSetting CHARGE_LIT__HOLY_StatusSetting => CHARGE_LIT__HOLY_StatusSettingObject.GetSetting ();

		private LocalStatusSettingObject COMBO_StatusSettingObject = new LocalStatusSettingObject ();
		public LocalStatusSetting COMBO_StatusSetting => COMBO_StatusSettingObject.GetSetting ();

		public void AddChargeBlowStatus (int chargeSkillId, float duration, decimal value)
		{
			switch (chargeSkillId)
			{
				case Page.SWORD_ICE_CHARGE:
					StatusStorage.Add (CHARGE_ICE_BLOW_StatusSetting, duration, value, this);
					break;
				case Page.SWORD_FIRE_CHARGE:
					StatusStorage.Add (CHARGE_FIRE_BLOW_StatusSetting, duration, value, this);
					break;
				case Page.SWORD_LIT_CHARGE:
					StatusStorage.Add (CHARGE_LIT_BLOW_StatusSetting, duration, value, this);
					break;
				case Page.SWORD_HOLY_CHARGE:
					StatusStorage.Add (CHARGE_LIT__HOLY_StatusSetting, duration, value, this);
					break;
			}
		}

		public LocalStatus GetChargeBlowStatus (int chargeSkillId)
		{
			LocalStatus returnValue = null;
			switch (chargeSkillId)
			{
				case Page.SWORD_ICE_CHARGE:
					returnValue = StatusStorage.GetLocalStatus (CHARGE_ICE_BLOW_StatusSetting);
					break;
				case Page.SWORD_FIRE_CHARGE:
					returnValue = StatusStorage.GetLocalStatus (CHARGE_FIRE_BLOW_StatusSetting);
					break;
				case Page.SWORD_LIT_CHARGE:
					returnValue = StatusStorage.GetLocalStatus (CHARGE_LIT_BLOW_StatusSetting);
					break;
				case Page.SWORD_HOLY_CHARGE:
					returnValue = StatusStorage.GetLocalStatus (CHARGE_LIT__HOLY_StatusSetting);
					break;
			}

			return returnValue;
		}

		public void AddComboStatus (float duration, decimal value)
		{
			StatusStorage.Add (COMBO_StatusSetting, duration, value, this);
		}

		public LocalStatus GetComboStatus ()
		{
			return StatusStorage.GetLocalStatus (COMBO_StatusSetting);
		}


	}
}