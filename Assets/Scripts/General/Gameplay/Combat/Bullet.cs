using System;





namespace ms
{
    // Represents a projectile on a map
    public class Bullet
    {
        public Bullet (Animation a)
        {
            animation = new Animation (a);
        }
        public Bullet(Animation a, Point_short origin, bool toleft)
        {
            animation = new Animation(a);

            moveobj.set_x(origin.x() + (toleft ? -30.0 : 30.0));
            moveobj.set_y(origin.y() - 26.0);
        }
        public void set_Ray(Point_short origin, bool toleft)
		{
            moveobj.set_x (origin.x () + (toleft ? -30.0 : 30.0));
            moveobj.set_y (origin.y () - 26.0);
        }

        public void set_Ray (double originX, double originY, bool toleft)
        {
            moveobj.set_x (originX + (toleft ? -30.0 : 30.0));
            moveobj.set_y (originY - 26.0);
        }
        public void draw(double viewx, double viewy, float alpha)
        {
            Point_short bulletpos = moveobj.get_absolute(viewx, viewy, alpha);
            DrawArgument args = new DrawArgument(new Point_short(bulletpos), flip);
            animation.draw(args, alpha);
        }
        public bool settarget(Point_short target)
        {
            double xdelta = target.x() - moveobj.crnt_x();
            double ydelta = target.y() - moveobj.crnt_y();

            if (Math.Abs(xdelta) < 10.0)
            {
                return true;
            }

            flip = xdelta > 0.0;

            moveobj.hspeed = xdelta / 32;

            if (xdelta > 0.0)
            {
                if (moveobj.hspeed < 3.0)
                {
                    moveobj.hspeed = 3.0;
                }
                else if (moveobj.hspeed > 6.0)
                {
                    moveobj.hspeed = 6.0;
                }
            }
            else if (xdelta < 0.0)
            {
                if (moveobj.hspeed > -3.0)
                {
                    moveobj.hspeed = -3.0;
                }
                else if (moveobj.hspeed < -6.0)
                {
                    moveobj.hspeed = -6.0;
                }
            }

            moveobj.vspeed = moveobj.hspeed * ydelta / xdelta;

            return false;
        }
        public bool update(Point_short target)
        {
            animation.update();
            moveobj.move();

            short xdelta = (short)(target.x() - moveobj.get_x());
            return moveobj.hspeed > 0.0 ? xdelta < 10 : xdelta > 10;
        }

        private Animation animation = new Animation();
        private MovingObject moveobj = new MovingObject();
        private bool flip;
    }
}
