using System.Collections.Generic;





namespace ms
{
	// Other client's players.
	public class OtherChar : Char
	{
		public OtherChar (int id, CharLook lk, byte lvl, short jb, string nm, sbyte st, Point_short pos) : base (id, lk, nm)
		{
			//this.Char = new < type missing > (id, lk, nm);
			level = lvl;
			jobId = jb;
			job.change_job ((ushort)jobId);
			
			set_position (pos);

			lastmove.xpos = pos.x ();
			lastmove.ypos = pos.y ();
			lastmove.newstate = (byte)st;
			timer = 0;

			attackspeed = 6;
			attacking = false;
		}

		// Update the character.
		public override sbyte update (Physics physics)
		{
			//AppDebug.Log ($"OtherChar update lastmove.xpos: {lastmove.xpos}\t lastmove.ypos: {lastmove.ypos}\t phobj.crnt_x ():{phobj.crnt_x ()}\t phobj.crnt_y ():{phobj.crnt_y ()}");

			if (timer > 1)
			{
				timer--;
			}
			else if (timer == 1)
			{
				if (movements.Count > 0)
				{
					lastmove = movements.Peek ();
					movements.Dequeue ();
				}
				else
				{
					timer = 0;
				}
			}

			if (!attacking)
			{
				byte laststate = lastmove.newstate;
				set_state (laststate);
			}

			phobj.hspeed = lastmove.xpos - phobj.crnt_x ();
			phobj.vspeed = lastmove.ypos - phobj.crnt_y ();
			phobj.move ();

			physics.get_fht ().update_fh (phobj);

			bool aniend = base.update (physics, get_stancespeed ());

			if (aniend && attacking)
			{
				attacking = false;
			}

			return get_layer ();
		}

		// Add the movements which this character will go through next.
		public void send_movement (List<Movement> newmoves)
		{
			movements.Enqueue (newmoves[newmoves.Count - 1]);

			if (timer == 0)
			{
				const ushort DELAY = 50;
				timer = DELAY;
			}
		}

		// Update a skill level.
		public void update_skill (int skillid, byte skilllevel)
		{
			skilllevels[skillid] = skilllevel;
		}

		// Update the attack speed.
		public void update_speed (byte @as)
		{
			attackspeed = @as;
		}

		// Update the character look.
		public void update_look (LookEntry newlook)
		{
			look = new CharLook (newlook);

			byte laststate = lastmove.newstate;
			set_state (laststate);
		}

		// Return the character's attacking speed.
		public override sbyte get_integer_attackspeed ()
		{
			return (sbyte)attackspeed;
		}

		// Return the character's level.
		public override ushort get_level ()
		{
			return level;
		}

		// Return the character's level of a skill.
		public override int get_skilllevel (int skillid)
		{
			if (!skilllevels.ContainsKey (skillid))
			{
				return 0;
			}


			return skilllevels[skillid];
		}

		public short get_jobId ()
		{
			return jobId;
		}
		
		public override Job get_job ()
		{
			return job;
		}
		
		private ushort level;
		private short jobId;
		private Job job = new Job();

		private Queue<Movement> movements = new Queue<Movement> ();
		private Movement lastmove = new Movement ();
		private ushort timer;
		
		private Dictionary<int, byte> skilllevels = new Dictionary<int, byte> ();
		private byte attackspeed;
	}
}