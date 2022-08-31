using System.Collections.Generic;
using SD.Tools.Algorithmia.GeneralDataStructures;





namespace ms
{
	/// <summary>
	///  Draw bullets, damage numbers etc.void draw(double viewx, double viewy, float alpha);Tangible Method Implementation Not Foundms.Combat-Combat Poll attacks, damage effects, etc.
	/// </summary>
	public class Combat
	{
		public Combat (Player in_player, MapChars in_chars, MapMobs in_mobs, MapReactors in_reactors)
		{
			//player = in_player;//player maybe null at this time
			chars = in_chars;
			mobs = in_mobs;
			reactors = in_reactors;

			attackresults = new TimedQueue<AttackResult> (apply_attack);
			bulleteffects = new TimedQueue<BulletEffect> (apply_bullet_effect);
			damageeffects = new TimedQueue<DamageEffect> (apply_damage_effect);
			/*attackresults ((AttackResult attack) =>
			{
				apply_attack (attack);
			};*/
		}

		public void draw (double viewx, double viewy, float alpha)
		{
			foreach (var be in bullets)
			{
				be.bullet.draw (viewx, viewy, alpha);
			}

			foreach (var dn in damagenumbers)
			{
				dn.draw (viewx, viewy, alpha);
			}
		}

		public void update ()
		{
			attackresults.update ();
			bulleteffects.update ();
			damageeffects.update ();

			bullets.remove_if (canRemoveBulletEffect);

			damagenumbers.remove_if ((DamageNumber dn) => dn.update ());
		}

		private bool canRemoveBulletEffect (BulletEffect mb)
		{
			int target_oid = mb.damageeffect.target_oid;
			AppDebug.Log ($"canRemoveBulletEffect:target_oid:{target_oid}");
			if (mobs.contains (target_oid))
			{
				mb.target = (mobs.get_mob_head_position (target_oid));
				bool apply = mb.bullet.update (mb.target);

				if (apply)
				{
					apply_damage_effect (mb.damageeffect);
				}

				return apply;
			}
			else
			{
				return mb.bullet.update (mb.target);
			}
		}
		// Make the player use a special move
		public void use_move (int move_id, bool down = false, bool pressing = false)
		{
			var move_id_Switched = SkillSwitcher.Instance.DoSwitch (move_id, player);
			SpecialMove move = get_move (move_id_Switched);

			if (!move.has_skillPrepareEffect () && down == false)//如果 是 普通技能 不是持续性技能，且没有按下 就返回
				return;

			if (down == true && pressing == false && move.has_skillPrepareEffect ())//begin
			{
				//AppDebug.Log ("begin");

			}
			else if (down == false && pressing == true && move.has_skillPrepareEffect ())//end
			{
				//AppDebug.Log ("end");
				player.remove_cooldown (move_id);
			}

			if (!player.can_attack (move_id, move.has_skillPrepareEffect ()))
			{
				return;
			}

			SpecialMove.ForbidReason reason = player.can_use (move);
			Weapon.Type weapontype = player.get_stats ().get_weapontype ();
		
			switch (reason)
			{
				case SpecialMove.ForbidReason.FBR_NONE:
					apply_move (move, down, pressing);
					if (down == true && pressing == true && move.has_skillPrepareEffect ())//moving
					{
						//AppDebug.Log ("moving");
						//if (ForceSkill.IsForce (move_id))
						{
							player.add_cooldown (move_id, player.get_total_attackdelay (move, move.get_action (player).actionStr));
						}
					}
					break;
				default:
					new ForbidSkillMessage (reason, weapontype).drop ();
					break;
			}
		}

		// Add an attack to the attack queue
		public void push_attack (AttackResult attack)
		{
			attackresults.push (400, attack);
		}

		// Show a buff effect
		public void show_buff (int cid, int skillid, sbyte level)
		{
			Optional<OtherChar> ouser = chars.get_char (cid);
			if (ouser)
			{
				OtherChar user = ouser;
				user.update_skill (skillid, (byte)level);

				SpecialMove move = get_move (skillid);
				move.apply_useeffects (user);
				move.apply_actions (user, Attack.Type.MAGIC);
			}
		}

		// Show a buff effect
		public void show_player_buff (int skillid)
		{
			get_move (skillid).apply_useeffects (player);
		}

		private struct DamageEffect
		{
			public AttackUser user;
			public DamageNumber number;
			public int damage;
			public bool toleft;
			public int target_oid;
			public int move_id;

			public DamageEffect (AttackUser user, DamageNumber number, int damage, bool toleft, int target_oid, int move_id)
			{
				this.user = new AttackUser ();
				this.user = user;
				this.number = number;
				this.damage = damage;
				this.toleft = toleft;
				this.target_oid = target_oid;
				this.move_id = move_id;
			}
		}

		private struct BulletEffect
		{
			public DamageEffect damageeffect;
			public Bullet bullet;
			public Point_short target;

			public BulletEffect (DamageEffect damageeffect, Bullet bullet, Point_short target)
			{
				this.damageeffect = damageeffect;
				this.bullet = bullet;
				this.target = target;
			}
		}

		private void apply_attack (AttackResult attack)
		{
			Optional<OtherChar> ouser = chars.get_char (attack.attacker);
			if (ouser)
			{
				OtherChar user = ouser;
				user.update_skill (attack.skill, attack.level);
				user.update_speed (attack.speed);

				SpecialMove move = get_move (attack.skill);
				move.apply_useeffects (user);

				Stance.Id stance = Stance.by_id (attack.stance);
				if (stance != 0)
				{
					user.attack (stance);
				}
				else
				{
					move.apply_actions (user, attack.type);
				}

				user.set_afterimage (attack.skill);

				extract_effects (user, move, attack);
			}
		}

		private void apply_move (SpecialMove move, bool down = false, bool pressing = false)
		{
			if (move.is_attack ())
			{
				Attack attack = player.prepare_attack (move.is_skill ());

				move.apply_useeffects (player);
				move.apply_actions (player, attack.type);

				player.set_afterimage (move.get_id ());

				move.apply_stats (player, attack);


				Point_short origin = new Point_short (attack.origin);
				Rectangle_short range = new Rectangle_short (attack.range);
				short hrange = (short)(range.left () * attack.hrange);

				MapleStory.Instance.attackRange = range;
				//AppDebug.Log ($"center:{center}\t size:{size}\t attackRange:{attackRange}");
				if (attack.toleft)
				{
					range = new Rectangle_short (
						(short)(origin.x () + hrange),
						(short)(origin.x () + range.right ()),
						(short)(origin.y () + range.top ()),
						(short)(origin.y () + range.bottom ()));
				}
				else
				{
					range = new Rectangle_short (
						(short)(origin.x () - range.right ()),
						(short)(origin.x () - hrange),
						(short)(origin.y () + range.top ()),
						(short)(origin.y () + range.bottom ()));
				}

				MapleStory.Instance.attackRangeAfter = range;

				// This approach should also make it easier to implement PvP
				byte mobcount = attack.mobcount;
				AttackResult result = new AttackResult (attack);

				MapObjects mob_objs = mobs.get_mobs ();
				MapObjects reactor_objs = reactors.get_reactors ();

				List<int> mob_targets = find_closest (mob_objs, new Rectangle_short (range), new Point_short (origin), mobcount, true);//todo 2 跳起来 攻击不到，按键输入还有问题 没法起跳时就攻击
				List<int> reactor_targets = find_closest (reactor_objs, new ms.Rectangle_short (range), new ms.Point_short (origin), mobcount, false);

				mobs.send_attack (result, attack, mob_targets, mobcount);
				result.attacker = player.get_oid ();
				extract_effects (player, move, result);

				apply_use_movement (move);
				apply_result_movement (move, result);

				new AttackPacket (result).dispatch ();
				if (down == true && pressing == false && move.has_skillPrepareEffect())//begin
				{
					new SkillEffectPacket (move.get_id (), Stage.get ().get_player ().get_skills ().get_masterlevel (move.get_id ()), 22, attack.toleft ? -128 : 0, 6).dispatch ();
					move.apply_prepareEffect (player);
					AppDebug.Log ("begin");
				
				}
				else if (down == false && pressing == true && move.has_skillPrepareEffect ())//end
				{
					new Cancel_BuffPacket (move.get_id ()).dispatch ();
					move.apply_keydownendEffect (player);
					AppDebug.Log ("end");

				}
				else if (down == true && pressing == true && move.has_skillPrepareEffect ())//moving
				{
					move.apply_keydownEffect (player);
					AppDebug.Log ("moving");

				}
				if (reactor_targets.Count != 0)
				{
					Optional<MapObject> reactor = reactor_objs.get (reactor_targets[0]);
					if (reactor)
					{
						new DamageReactorPacket (reactor.get ().get_oid (), player.get_position (), 0, 0).dispatch ();
					}
				}
			}
			else
			{
				move.apply_useeffects (player);
				move.apply_actions (player, Attack.Type.MAGIC);

				int moveid = move.get_id ();
				int level = player.get_skills ().get_level (moveid);
				new UseSkillPacket (moveid, level).dispatch ();
			}
		}

		/*       MultiValueDictionary<ushort, int> distances_mobs = new MultiValueDictionary<ushort, int>();

			   List<int> targets_mobs = new List<int>();

			   MultiValueDictionary<ushort, int> distances_reactors = new MultiValueDictionary<ushort, int>();

			   List<int> targets_reactors = new List<int>();*/
		private List<int> find_closest (MapObjects objs, Rectangle_short range, Point_short origin, byte objcount, bool use_mobs)
		{
			MultiValueDictionary<ushort, int> distances = new MultiValueDictionary<ushort, int> ();

			foreach (var mmo in objs)
			{
				if (use_mobs)
				{
					Mob mob = (Mob)mmo.Value;

					if (mob != null && mob.is_alive () && mob.is_in_range (range, true))
					{
						int oid = mob.get_oid ();
						var distance = mob.get_position ().distance (new Point_short (origin));
						distances.Add ((ushort)distance, oid);
					}
				}
				else
				{
					// Assume Reactor
					Reactor reactor = (Reactor)mmo.Value;

					if (reactor != null && reactor.is_hittable () && reactor.is_in_range (range))
					{
						int oid = reactor.get_oid ();
						ushort distance = (ushort)reactor.get_position ().distance (new ms.Point_short (origin));
						distances.Add (distance, oid);
					}
				}
			}

			List<int> targets = new List<int> ();

			foreach (var iter in distances)
			{
				if (targets.Count >= objcount)
				{
					break;
				}

				targets.AddRange (iter.Value);
			}

			return targets;
		}

		private void apply_use_movement (SpecialMove move)
		{
			switch ((SkillId.Id)move.get_id ())
			{
				case SkillId.Id.TELEPORT_FP:
				case SkillId.Id.IL_TELEPORT:
				case SkillId.Id.PRIEST_TELEPORT:
				case SkillId.Id.FLASH_JUMP:
				default:
					break;
			}
		}

		private void apply_result_movement (SpecialMove move, AttackResult result)
		{
			switch ((SkillId.Id)move.get_id ())
			{
				case SkillId.Id.RUSH_HERO:
				case SkillId.Id.RUSH_PALADIN:
				case SkillId.Id.RUSH_DK:
					apply_rush (result);
					break;
				default:
					break;
			}
		}

		private void apply_rush (AttackResult result)
		{
			if (result.mobcount == 0)
			{
				return;
			}

			Point_short mob_position = mobs.get_mob_position (result.last_oid);
			short targetx = mob_position.x ();
			player.rush (targetx);
		}

		private void apply_bullet_effect (BulletEffect effect)
		{
			bullets.AddLast (effect);

			if (bullets.Last.Value.bullet.settarget (effect.target))
			{
				apply_damage_effect (effect.damageeffect);
				bullets.RemoveLast ();
			}
		}

		private void apply_damage_effect (DamageEffect effect)
		{
			Point_short head_position = mobs.get_mob_head_position (effect.target_oid);
			damagenumbers.AddLast (effect.number);
			damagenumbers.Last.Value.set_x (head_position.x ());

			SpecialMove move = get_move (effect.move_id);
			mobs.apply_damage (effect.target_oid, effect.damage, effect.toleft, effect.user, move);
		}

		private void extract_effects (Char user, SpecialMove move, AttackResult result)
		{
			AttackUser attackuser = new AttackUser (user.get_skilllevel (move.get_id ()), user.get_level (), user.is_twohanded (), !result.toleft, user);

			if (result.bullet != 0)
			{
				Bullet bullet = new Bullet (move.get_bullet (user, result.bullet), user.get_position (), result.toleft);

				foreach (var line in result.damagelines)
				{
					int oid = line.Key;

					if (mobs.contains (oid))
					{
						List<DamageNumber> numbers = place_numbers (oid, line.Value);
						Point_short head = mobs.get_mob_head_position (oid);

						uint i = 0;

						foreach (var number in numbers)
						{
							DamageEffect effect = new DamageEffect (attackuser, number, line.Value[(int)i].Item1, result.toleft, oid, move.get_id ());
							bulleteffects.push (user.get_attackdelay (i), new BulletEffect (effect, bullet, head));
							i++;
						}
					}
				}

				if (result.damagelines.Count == 0)
				{
					short xshift = (short)(result.toleft ? -400 : 400);
					Point_short target = user.get_position () + new Point_short (xshift, -26);

					for (byte i = 0; i < result.hitcount; i++)
					{
						DamageEffect effect = new DamageEffect (attackuser, new DamageNumber (), 0, false, 0, 0);
						bulleteffects.push (user.get_attackdelay (i), new BulletEffect (effect, bullet, target));
					}
				}
			}
			else
			{
				foreach (var line in result.damagelines)
				{
					int oid = line.Key;

					if (mobs.contains (oid))
					{
						List<DamageNumber> numbers = place_numbers (oid, line.Value);

						uint i = 0;

						foreach (var number in numbers)
						{
							damageeffects.push (user.get_attackdelay (i), new DamageEffect (attackuser, number, line.Value[(int)i].Item1, result.toleft, oid, move.get_id ()));
							i++;
						}
					}
				}
			}
		}

		private List<DamageNumber> place_numbers (int oid, List<System.Tuple<int, bool>> damagelines)
		{
			List<DamageNumber> numbers = new List<DamageNumber> ();
			short head = mobs.get_mob_head_position (oid).y ();

			foreach (var line in damagelines)
			{
				int amount = line.Item1;
				bool critical = line.Item2;
				DamageNumber.Type type = critical ? DamageNumber.Type.CRITICAL : DamageNumber.Type.NORMAL;
				numbers.Add (new DamageNumber (type, amount, head));

				head -= DamageNumber.rowheight (critical);
			}

			return numbers;
		}

		private SpecialMove get_move (int move_id)
		{
			if (move_id == -1)
			{
				return null;
			}

			if (move_id == 0)
			{
				return regularattack;
			}

			if (!skills.TryGetValue (move_id, out var skill))
			{
				skill = new Skill (move_id);
				skills.Add (move_id, skill);
			}

			return skill;
		}

		private Player player => Stage.get ().get_player ();
		//private Player player;

		private MapChars chars;
		private MapMobs mobs;
		private MapReactors reactors;

		private Dictionary<int, Skill> skills = new Dictionary<int, Skill> ();
		private RegularAttack regularattack = new RegularAttack ();

		private TimedQueue<AttackResult> attackresults;
		private TimedQueue<BulletEffect> bulleteffects;
		private TimedQueue<DamageEffect> damageeffects;

		private LinkedList<BulletEffect> bullets = new LinkedList<BulletEffect> ();
		private LinkedList<DamageNumber> damagenumbers = new LinkedList<DamageNumber> ();
	}
}

