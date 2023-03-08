


using MapleLib.WzLib;

namespace ms
{
	// Class that uses physics engines and the collection of platforms to determine object movement
	public class Physics
	{
		public Physics(WzObject node_100000000img_foothold)
		{
			fht = new FootholdTree(node_100000000img_foothold);
			//fht = src;
		}

		// Move the specified object over the specified game-time
		public void move_object(PhysicsObject phobj)
		{
			// Determine which platform the object is currently on
			fht.update_fh(phobj);

			// Use the appropriate physics for the terrain the object is on
			switch (phobj.type)
			{
			case PhysicsObject.Type.NORMAL:
				move_normal(phobj);
				fht.limit_movement(phobj);
				break;
			case PhysicsObject.Type.FLYING:
				move_flying(phobj);
				fht.limit_movement(phobj);
				break;
			case PhysicsObject.Type.SWIMMING:
				move_swimming(phobj);
				fht.limit_movement(phobj);
				break;
			case PhysicsObject.Type.FIXATED:
			default:
				break;
			}

			// Move the object forward
			phobj.move();
		}
		// Determine the point on the ground below the specified position
		public Point_short get_y_below(Point_short position)
		{
			short ground = fht.get_y_below(position);

			return new Point_short(position.x(), (short)(ground - 1));
		}
		// Return a reference to the collection of platforms
		public FootholdTree get_fht()
		{
			return fht;
		}

		private void move_normal(PhysicsObject phobj)
		{
			phobj.vacc = 0.0;
			phobj.hacc = 0.0;

			if (phobj.onground)
			{
				phobj.vacc += phobj.vforce;
				phobj.hacc += phobj.hforce;

				if (phobj.hacc == 0.0 && phobj.hspeed < 0.1 && phobj.hspeed > -0.1)
				{
					phobj.hspeed = 0.0;
				}
				else
				{
					double inertia = phobj.hspeed / GROUNDSLIP;
					double slopef = phobj.fhslope;

					if (slopef > 0.5)
					{
						slopef = 0.5;
					}
					else if (slopef < -0.5)
					{
						slopef = -0.5;
					}

					phobj.hacc -= (FRICTION + SLOPEFACTOR * (1.0 + slopef * -inertia)) * inertia;
				}
			}
			else if (phobj.is_flag_not_set(PhysicsObject.Flag.NOGRAVITY))
			{
				phobj.vacc += GRAVFORCE;
			}

			phobj.hforce = 0.0;
			phobj.vforce = 0.0;

			phobj.hspeed += phobj.hacc;
			phobj.vspeed += phobj.vacc;
		}
		private void move_flying(PhysicsObject phobj)
		{
			phobj.hacc = phobj.hforce;
			phobj.vacc = phobj.vforce;
			phobj.hforce = 0.0;
			phobj.vforce = 0.0;

			phobj.hacc -= FLYFRICTION * phobj.hspeed;
			phobj.vacc -= FLYFRICTION * phobj.vspeed;

			phobj.hspeed += phobj.hacc;
			phobj.vspeed += phobj.vacc;

			if (phobj.hacc == 0.0 && phobj.hspeed < 0.1 && phobj.hspeed > -0.1)
			{
				phobj.hspeed = 0.0;
			}

			if (phobj.vacc == 0.0 && phobj.vspeed < 0.1 && phobj.vspeed > -0.1)
			{
				phobj.vspeed = 0.0;
			}
		}
		private void move_swimming(PhysicsObject phobj)
		{
			phobj.hacc = phobj.hforce;
			phobj.vacc = phobj.vforce;
			phobj.hforce = 0.0;
			phobj.vforce = 0.0;

			phobj.hacc -= SWIMFRICTION * phobj.hspeed;
			phobj.vacc -= SWIMFRICTION * phobj.vspeed;

			if (phobj.is_flag_not_set(PhysicsObject.Flag.NOGRAVITY))
			{
				phobj.vacc += SWIMGRAVFORCE;
			}

			phobj.hspeed += phobj.hacc;
			phobj.vspeed += phobj.vacc;

			if (phobj.hacc == 0.0 && phobj.hspeed < 0.1 && phobj.hspeed > -0.1)
			{
				phobj.hspeed = 0.0;
			}

			if (phobj.vacc == 0.0 && phobj.vspeed < 0.1 && phobj.vspeed > -0.1)
			{
				phobj.vspeed = 0.0f;
			}
		}

		private FootholdTree fht;
		
		public const double GRAVFORCE = 0.14;
		public const double SWIMGRAVFORCE = 0.03;
		public const double FRICTION = 0.3;
		public const double SLOPEFACTOR = 0.1;
		public const double GROUNDSLIP = 3.0;
		public const double FLYFRICTION = 0.05;
		public const double SWIMFRICTION = 0.08;
	}
}
