﻿using System.Collections.Generic;
using constants.skills;
using ms_Unity;
using RuntimeEditor.Examples;
using SD.Tools.Algorithmia.GeneralDataStructures;
using UnityEngine;

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

		private bool canRemoveBulletEffect (BulletEffect bulletEffect)
		{
			int target_oid = bulletEffect.damageeffect.target_oid;
			//AppDebug.Log ($"canRemoveBulletEffect:target_oid:{target_oid}");
			if (mobs.contains (target_oid))
			{
				bulletEffect.target = mobs.get_mob_head_position (target_oid);
				bool apply = bulletEffect.bullet.update (bulletEffect.target);

				if (apply)
				{
					apply_damage_effect (bulletEffect.damageeffect);
				}

				return apply;
			}
			else
			{
				return bulletEffect.bullet.update (bulletEffect.target);
			}
		}
		// Make the player use a special move
		public void use_move (int move_id, bool down = false, bool pressing = false)
		{
			//AppDebug.Log ($"use_move:{move_id}");
			var move_id_Switched = SkillSwitcher.Instance.DoSwitch (move_id, player);
			SpecialMove move = get_move (move_id_Switched);

            if (move.has_skillPrepareEffect() && move is Skill skill)//持续技能
			{
                if (!player.can_attack(false))
                {
                    return;
                }

                SpecialMove.ForbidReason reason = player.can_use(move);
				if (reason != SpecialMove.ForbidReason.FBR_NONE)//有禁用原因
				{
                    AppDebug.Log($"skill Forbid Use Reason :{reason}");
                    Weapon.Type weapontype = player.get_stats().get_weapontype();
                    new ForbidSkillMessage(reason, weapontype).drop();
					return;
                }

                var prepareEffect = skill.GetPrepareEffect();
                var keydownEffect = skill.GetKeydownEffect();
                var keydownEndEffect = skill.GetKeyDownEndEffect();

                if (down == true && pressing == false)//begin
                {
					if (prepareEffect == null) return;
					player.add_cooldown(skill.get_id(), prepareEffect.GetAniLength()/1000f);
					prepareEffect?.apply(player);

                    //AppDebug.Log ("skill begin");

                }
				else if (down == true && pressing == true)//pressing
				{
                    if (keydownEffect == null) return;
                    player.add_cooldown(skill.get_id(), keydownEffect.GetAvergeFrameLength() / 1000f);
                    keydownEffect?.apply(player);
                    //AppDebug.Log("skill pressing");
                }
                else if (down == false && pressing == true)//end
                {
                    if (keydownEndEffect == null) return;
                    player.add_cooldown(skill.get_id(), keydownEndEffect.GetAniLength() / 1000f);
                    keydownEndEffect?.apply(player);
                    //AppDebug.Log ("skill end");
                    player.remove_cooldown(move_id);
                }

                apply_move(move, move_id, down, pressing);
            }
            else//瞬发技能
			{
                if (!player.can_attack())
                    return;

                SpecialMove.ForbidReason reason = player.can_use(move);
                Weapon.Type weapontype = player.get_stats().get_weapontype();


                switch (reason)
                {
                    case SpecialMove.ForbidReason.FBR_NONE:
                        apply_move(move, move_id, down, pressing);
                        break;
                    default:
                        //AppDebug.Log($"Forbid Use Reason :{reason}");
                        new ForbidSkillMessage(reason, weapontype).drop();
                        break;
                }

                /*if (move is Skill s)
                {
                    player.add_cooldown(move.get_id(), s.GetUseeffect().GetAniLength() / 1000f);
                }*/
            }
               
		}

		// Add an attack to the attack queue
		public void push_attack (AttackResult attack)
		{
			attackresults.push (400, attack);
		}
		/*public void push_damageEffect (AttackResult result, AttackUser attackuser)
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
						damageeffects.push (100, new DamageEffect (attackuser, number, line.Value[(int)i].Item1, result.toleft, oid, result.skill, result.hforce, result.vforce));
						i++;
					}
				}
			}
		}*/
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
				move.apply_actions (user, Attack.Type.Magic);
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
			public float hforce;
			public float vforce;

			public DamageEffect (AttackUser user, DamageNumber number, int damage, bool toleft, int target_oid, int move_id, float hforce, float vforce)
			{
				this.user = new AttackUser ();
				this.user = user;
				this.number = number;
				this.damage = damage;
				this.toleft = toleft;
				this.target_oid = target_oid;
				this.move_id = move_id;
				this.hforce = hforce;
				this.vforce = vforce;
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
			SpecialMove move = get_move (attack.skill);
			
			Optional<OtherChar> ouser = chars.get_char (attack.attacker);
			Char c;
			if (ouser)//其他玩家
			{
				OtherChar user = ouser;
				user.update_skill (attack.skill, attack.skilllevel);
				user.update_speed (attack.speed);

				c = ouser.get ();
				
				move.apply_useeffects (c);

				Stance.Id stance = Stance.by_id (attack.stance);
				if (stance != 0)
				{
					c.attack (stance);
				}
				else
				{
					move.apply_actions (c, attack.type);
				}

				c.set_afterimage (attack.skill);
			}
			else//玩家
			{
				c = player;
			}
			
			extract_effects (c, move, attack);
			
		}

		private void apply_move (SpecialMove move, int move_id, bool down = false, bool pressing = false)
		{
			if (move.is_attack ())
			{
				GameUtil.Instance.stopwatch.Start();
				AppDebug.Log ($"使用攻击技能：{move_id} ");
				if (move.get_id() == 0)
				{

				}
				if (player.PlaySkillBTree (move.BTree, move, move_id))
				{
					return;
				}

				Attack attack = player.prepare_attack (move.is_skill ());

				move.apply_useeffects (player);
				move.apply_actions (player, attack.type);

				player.set_afterimage (move.get_id ());

				move.apply_stats (player, attack);


				Point_short origin = new Point_short (attack.origin);
				Rectangle_short range = new Rectangle_short (attack.range);
				short hrange = (short)(range.left () * attack.hrange);
                /*int x = attack.x;
				int y = attack.y;*///金钱炸弹 超出范围 x=500，y=0 ，攻击不到
                int x = 0;
                int y = 0;

                MapleStory.Instance.attackRange = range;
				//AppDebug.Log ($"center:{center}\t size:{size}\t attackRange:{attackRange}");
				if (attack.toleft)
				{
					range = new Rectangle_short (
						(short)(origin.x () + hrange - x),
						(short)(origin.x () + range.right () - x),
						(short)(origin.y () + range.top () + y),
						(short)(origin.y () + range.bottom () + y));
				}
				else
				{ 
					range = new Rectangle_short (
						(short)(origin.x () - range.right ()+x),
						(short)(origin.x () - hrange+x),
						(short)(origin.y () + range.top ()+y),
						(short)(origin.y () + range.bottom ()+y));
				}

				MapleStory.Instance.attackRangeAfter = range;

				attack.range = range;
				
				if (move_id == ChiefBandit.MESO_EXPLOSION)
				{
					if (ms.Stage.get ().get_drops ().find_loot_inRange (range, 1).Count == 0)
					{
						attack.hitcount = 0;
					}
					else
					{
						attack.hitcount = 1;
					}
				}
				/*else if (move_id == Thief.DoubleDart)
				{
					if (player.has_buff (Buffstat.Id.SHADOWPARTNER))
					{
						attack.hitcount = 4;
					}
				}
				else if (move_id == NightLord.TRIPLE_THROW)
				{
					if (player.has_buff (Buffstat.Id.SHADOWPARTNER))
					{
						attack.hitcount = 6;
					}
				}*/

				if (player.has_buff( Buffstat.Id.SHADOWPARTNER))
				{
					attack.hitcount *= 2;

                }
                // This approach should also make it easier to implement PvP
                byte mobcount = attack.mobcount;
				AttackResult result = new AttackResult (attack);

				MapObjects mob_objs = mobs.get_mobs ();
				MapObjects reactor_objs = reactors.get_reactors ();

				List<int> mob_targets = find_closest (mob_objs, new Rectangle_short (range), new Point_short (origin), mobcount, true);//todo 2 跳起来 攻击不到，按键输入还有问题 没法起跳时就攻击
				List<int> reactor_targets = find_closest (reactor_objs, new ms.Rectangle_short (range), new ms.Point_short (origin), mobcount, false);

				mobs.send_attack (result, attack, mob_targets, mobcount);
				result.attacker = player.get_oid ();
				//extract_effects (player, move, result);//使用技能的时候不应用，服务器返回攻击信息才应用

				apply_use_movement (move);
				apply_result_movement (move, result);

				if(GameUtil.Instance.sendAttackPacket)
				new AttackPacket (result).dispatch ();
			/*	if (down == true && pressing == false && move.has_skillPrepareEffect ())//begin
				{
                    if (GameUtil.Instance.sendAttackPacket)
                        new SkillEffectPacket (move.get_id (), Stage.get ().get_player ().get_skills ().get_masterlevel (move.get_id ()), 22, attack.toleft ? -128 : 0, 6).dispatch ();
					move.apply_prepareEffect (player);
					AppDebug.Log ("begin");

				}
				else if (down == false && pressing == true && move.has_skillPrepareEffect ())//end
				{
                    if (GameUtil.Instance.sendAttackPacket)
                        new Cancel_BuffPacket (move.get_id ()).dispatch ();
					move.apply_keydownendEffect (player);
					AppDebug.Log ("end");

				}
				else if (down == true && pressing == true && move.has_skillPrepareEffect ())//moving
				{
					move.apply_keydownEffect (player);
					AppDebug.Log ("moving");

				}*/

				if (reactor_targets.Count != 0)
				{
					Optional<MapObject> reactor = reactor_objs.get (reactor_targets[0]);
					if (reactor)
					{
                        if (GameUtil.Instance.sendAttackPacket)
                            new DamageReactorPacket (reactor.get ().get_oid (), player.get_position (), 0, 0).dispatch ();
					}
				}
                GameUtil.Instance.stopwatch.Stop();
                //AppDebug.Log($"apply_move time：{GameUtil.Instance.stopwatch.ElapsedMilliseconds}");
                GameUtil.Instance.stopwatch.Restart();

            }
            else
			{
				AppDebug.Log ($"使用Buff：{move_id} ");
				move.apply_useeffects (player);
				move.apply_actions (player, Attack.Type.Magic);
                apply_use_movement(move);

                int moveid = move.get_id ();
				int level = player.get_skills ().get_level (moveid);
                if (GameUtil.Instance.sendAttackPacket)
                    new UseSkillPacket (moveid, level).dispatch ();
			}
		}

		/*       MultiValueDictionary<ushort, int> distances_mobs = new MultiValueDictionary<ushort, int>();

			   List<int> targets_mobs = new List<int>();

			   MultiValueDictionary<ushort, int> distances_reactors = new MultiValueDictionary<ushort, int>();

			   List<int> targets_reactors = new List<int>();*/
		public List<int> find_closest (MapObjects objs, Rectangle_short range, Point_short origin, byte objcount, bool use_mobs)
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
						if (mob.hasBossHPBar())
						{
							lastestAttackedBoss = mob;
						}
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

		public void apply_use_movement (SpecialMove move)
		{
			switch (move.get_id ())
			{
				case FPWizard.FP_TELEPORT:
				case ILWizard.IL_TELEPORT:
				case Cleric.Cleric_TELEPORT:
					apply_TELEPORT_movement((Skill)move);
                    break;
                case Hermit.FLASH_JUMP:
                    apply_FLASH_JUMP_movement((Skill)move);
                    break;
                case Hero.MONSTER_MAGNET:
                case Paladin.MONSTER_MAGNET:
                case DarkKnight.MONSTER_MAGNET:
                    apply_MONSTER_MAGNET((Skill)move);
                    break;
                default:
					break;
			}
		}

		//二段跳
        public void apply_FLASH_JUMP_movement(Skill skill)
		{
			AppDebug.Log("apply_FLASH_JUMP_movement");
			player.addforceXY(2* (player.facing_right ? 1 : -1), -5); 
		}

        public void apply_TELEPORT_movement(Skill skill)
		{
			var skillID = skill.get_id ();
			var skillLevel = player.get_skilllevel(skillID);
			var skillData = SkillData.get(skillID);
			var stats = skillData.get_stats(skillLevel);
			var hrange = stats.hrange * 100f;

            Point_short pos = player.get_position();

			var moveX =  hrange * (player.is_key_down( KeyAction.Id.RIGHT) ? 1 : (player.is_key_down(KeyAction.Id.LEFT)?-1:0));
            var moveY = hrange * (player.is_key_down(KeyAction.Id.DOWN) ? 1 : (player.is_key_down(KeyAction.Id.UP) ? -1 : 0));
			if (Mathf.Abs( moveX) < 0.2f && Mathf.Abs(moveY) < 0.2f)
			{
                moveX = hrange * (player.facing_right ? 1 : -1);
            }
			//AppDebug.Log($"moveX:{moveX}\t moveY:{moveY}");
            var targetX = pos.x() + moveX ;
            var targetY = pos.y() + moveY;
            //player.rushXY(targetX, targetY);

			var targetPos = ms.Stage.get().GetFootholdTree().get_fhid_vertical(targetX, targetY);
			if (targetPos.Item1 != 0 && targetPos.Item2 != 0)
			{
                //AppDebug.Log($"a:{Mathf.Pow((float)(targetPos.Item1 - pos.x()), 2)}\t b:{Mathf.Pow((float)(targetPos.Item2 - pos.y()), 2)}\t c:{Mathf.Pow(hrange, 2)*1.5}");
                if (Mathf.Pow((float)(targetPos.Item1 - pos.x()), 2) + Mathf.Pow((float)(targetPos.Item2 - pos.y()), 2) <= Mathf.Pow(hrange, 2)*1.5)
                {
                    player.teleportXY(targetPos.Item1, targetPos.Item2);
                }
            }
			
            //player.rush(targetX);
        }
		//private List<Mob> MONSTER_MAGNET_List = new List<Mob>();

        public void apply_MONSTER_MAGNET(Skill skill)
		{
			//MultiValueDictionary<ushort, int> distances = new MultiValueDictionary<ushort, int>();
			var range = new Rectangle_short(MONSTER_MAGNET_Range);
			var origin = player.get_position();
            range.shift(origin);

            foreach (var mmo in mobs.get_mobs())
			{
				Mob mob = (Mob)mmo.Value;

				if (mob != null && mob.is_alive() && mob.is_in_range(range, true) && !mob.isBoss() && mob.get_Control())
				{
					/*int oid = mob.get_oid();
					var distance = mob.get_position().distance(origin);
					distances.Add((ushort)distance, oid);
					MONSTER_MAGNET_List.Add(mob);*/
					mob.set_position(origin);
                }
			}

        }
        public void apply_result_movement (SpecialMove move, AttackResult result)
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
				//bullets.Last.Value.bullet?.Dispose();
                bullets.Last.Value.damageeffect.number?.Dispose();

                bullets.RemoveLast ();
			}
		}

		private void apply_damage_effect (DamageEffect effect)
		{
			Point_short head_position = mobs.get_mob_head_position (effect.target_oid);
			damagenumbers.AddLast (effect.number);
			damagenumbers.Last.Value.set_x (head_position.x ());

			SpecialMove move = get_move (effect.move_id);
			mobs.apply_damage (effect.target_oid, effect.damage, effect.toleft, effect.user, move, effect.hforce, effect.vforce);
		}

		public void extract_effects (Char user, SpecialMove move, AttackResult result)
		{
			AttackUser attackuser = new AttackUser(user.get_skilllevel(move.get_id()), user.get_level(), user.is_twohanded(), !result.toleft, user);

			if (result.bulletId != 0)
			{
				Bullet bullet = new Bullet(move.get_bullet(user, result.bulletId), user.get_position(), result.toleft);

				foreach (var line in result.damagelines)
				{
					int oid = line.Key;

					if (mobs.contains(oid))
					{
						List<DamageNumber> numbers = place_numbers(oid, line.Value);
						Point_short head = mobs.get_mob_head_position(oid);

						uint i = 0;

						foreach (var number in numbers)
						{
							DamageEffect effect = new DamageEffect(attackuser, number, line.Value[(int)i].Item1, result.toleft, oid, move.get_id(), result.hforce, result.vforce);
							bulleteffects.push(user.get_attackdelay(i), new BulletEffect(effect, bullet, head));
							i++;
						}
					}
				}

				if (result.damagelines.Count == 0)
				{
					short xshift = (short)(result.toleft ? -400 : 400);
					Point_short target = user.get_position() + new Point_short(xshift, -26);

					for (byte i = 0; i < result.hitcount; i++)
					{
						DamageEffect effect = new DamageEffect(attackuser, new DamageNumber(), 0, false, 0, 0, result.hforce, result.vforce);
						bulleteffects.push(user.get_attackdelay(i), new BulletEffect(effect, bullet, target));
					}
				}
			}
			else
			{
				foreach (var line in result.damagelines)
				{
					int oid = line.Key;

					if (mobs.contains(oid))
					{
						List<DamageNumber> numbers = place_numbers(oid, line.Value);

						uint i = 0;

						foreach (var number in numbers)
						{
							damageeffects.push(user.get_attackdelay(i), new DamageEffect(attackuser, number, line.Value[(int)i].Item1, result.toleft, oid, move.get_id(), result.hforce, result.vforce));
							i++;
						}
					}
				}
			}
		}

        List<DamageNumber> numbers = new List<DamageNumber>();
        private List<DamageNumber> place_numbers (int oid, List<System.Tuple<int, bool>> damagelines)
		{
			foreach (var n in numbers)
			{
				n.Dispose();
			}
			numbers.Clear();
           
			short head = mobs.get_mob_head_position (oid).y ();

			foreach (var line in damagelines)
			{
				int amount = line.Item1;
				bool critical = line.Item2;
				DamageNumber.Type type = critical ? DamageNumber.Type.CRITICAL : DamageNumber.Type.NORMAL;
				numbers.Add (new DamageNumber (type, amount, head));
                //AppDebug.Log($"place_numbers damage:{line.Item1}");
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

			if (move_id == 0 || move_id == 1)
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

		public void clear ()
		{
            lastestAttackedBoss = null;
        }

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

		private Mob lastestAttackedBoss;
		private Rectangle_short MONSTER_MAGNET_Range = new Rectangle_short((short)(-Constants.get().get_viewwidth() / 2), (short)(Constants.get().get_viewwidth() / 2), (short)(-Constants.get().get_viewheight() / 2), (short)(Constants.get().get_viewheight() / 2));
        public Mob get_lastestAttackedBoss() => lastestAttackedBoss;
	}
}

