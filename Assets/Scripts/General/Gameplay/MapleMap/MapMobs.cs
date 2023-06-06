using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ms
{
	// A collection of mobs on a map.
	public class MapMobs
	{
		public MapMobs() 
		{
            TestURPBatcher.Instance.StartCoroutine(SpawnMob());
        }
        // Draw all mobs on a layer.
        public void draw (Layer.Id layer, double viewx, double viewy, float alpha)
		{
			mobs.draw (layer, viewx, viewy, alpha);
		}

		// Update all mobs.
		public void update (Physics physics)
		{
            mobs.update(physics);
        }

        public IEnumerator SpawnMob()
        {
            while (true)
            {
                yield return null;
                Physics physics = Stage.get().get_Physics();

                for (; spawns.Count > 0; spawns.Dequeue())
                {
                    MobSpawn spawn = spawns.Peek();

                    var mobRef = mobs.get(spawn.get_oid());
                    if (mobRef.get() is Mob mob)
                    {
                        sbyte mode = spawn.get_mode();

                        if (mode > 0)
                        {
                            mob.set_control(mode);
                        }

                        mob.makeactive();
                    }
                    else
                    {
                        mobs.add(spawn.instantiate());
                    }

                    yield return new WaitForSecondsRealtime(GameUtil.Instance.spawnMobInterval);
                }

               

            }
        }

        // Spawn a new mob.
        public void spawn (MobSpawn spawn)
		{
			//AppDebug.Log ($"MapMobs spawn : oid:{spawn.get_oid ()}\t mode:{spawn.get_mode ()}");
			spawns.Enqueue (spawn);
		}

		// Kill a mob.
		public void remove (int oid, sbyte animation)
		{
			if (mobs.get (oid).get () is Mob mob)
			{
				mob.kill (animation);
			}
		}

		// Remove all mobs.
		public void clear ()
		{
			mobs.clear ();
		}

		// Update who a mob is controlled by.
		public void set_control (int oid, sbyte mode)
		{
			//sbyte mode = (sbyte)(control ? 1 : 0);

			if (mobs.get (oid).get () is Mob mob)
			{
				mob.set_control (mode);
			}
		}

		// Update a mob's hp display.
		public void send_mobhp (int oid, sbyte percent, ushort playerlevel)
		{
			if (mobs.get (oid).get () is Mob mob)
			{
				mob.show_hp (percent, playerlevel);
			}
		}
        public void send_bosshp(int oid, int currHP,int maxHP)
        {
            if (mobs.get(oid).get() is Mob mob)
            {
                mob.bosshp(currHP, maxHP);
            }
        }
        // Update a mob's movements.
        public void send_movement (int oid, Point_short start, List<Movement> movements)
		{
			if (mobs.get (oid).get () is Mob mob)
			{
				mob.send_movement (start, movements);
			}
		}

		// Calculate the results of an attack.
		public void send_attack (AttackResult result, Attack attack, List<int> targets, byte mobcount)
		{
			foreach (var target in targets)
			{
				if (mobs.get (target).get () is Mob mob)
				{
					result.damagelines[target] = mob.calculate_damage (attack);
					result.mobcount++;

					if (result.mobcount == 1)
					{
						result.first_oid = target;
					}

					if (result.mobcount == mobcount)
					{
						result.last_oid = target;
					}
				}
			}
		}

		// Applies damage to a mob.
		public void apply_damage (int oid, int damage, bool toleft, AttackUser user, SpecialMove move, float hforce = 0, float vforce = 0)
		{
			if (mobs.get (oid).get () is Mob mob)
			{
				mob.apply_damage (damage, toleft, hforce, vforce);
				mob.add_Force (hforce, vforce);
				// TODO: Maybe move this into the method above too?
				move.apply_hiteffects (user, mob);
			}
		}

		// Check if the mob with the specified oid exists.
		public bool contains (int oid)
		{
			return mobs.contains (oid);
		}

		Range_short horizontal = new Range_short ();
		Range_short vertical = new Range_short ();

		Rectangle_short player_rect = new Rectangle_short ();

		// Return the id of the first mob who collides with the object.
		public int find_colliding (MovingObject moveobj)
		{
			horizontal.Set (moveobj.get_last_x (), moveobj.get_x ());
			vertical.Set (moveobj.get_last_y (), moveobj.get_y ());
			player_rect.Set (horizontal.smaller (), horizontal.greater (), (short)(vertical.smaller () - 50), vertical.greater ());
			
			return mobs.Objects.FirstOrDefault (pair => pair.Value != null && ((pair.Value as Mob)?.is_alive () ?? false) && ((pair.Value as Mob)?.is_in_range (player_rect) ?? false)).Key;
		}

		// Create an attack by the specified mob.
		public MobAttack create_attack (int oid)
		{
			if (mobs.get (oid).get () is Mob mob)
			{
				return mob.create_touch_attack ();
			}

			return null;
		}

		// Return the position of a mob.
		public Point_short get_mob_position (int oid)
		{
			if (mobs.get (oid).get () is Mob mob)
			{
				return mob.get_position ();
			}
			else
			{
				return new Point_short (0, 0);
			}
		}

		// Return the head position of a mob.
		public Point_short get_mob_head_position (int oid)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
			if (mobs.get (oid).get () is Mob mob)
			{
				return mob.get_head_position ();
			}
			else
			{
				return new Point_short (0, 0);
			}
		}

		// Return all mob map objects
		public MapObjects get_mobs ()
		{
			return mobs;
		}

		private MapObjects mobs = new MapObjects ();

		private Queue<MobSpawn> spawns = new Queue<MobSpawn> ();

		public void clearSpawns()
		{
			spawns.Clear ();
		}
	}
}