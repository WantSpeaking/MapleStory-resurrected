using System;
using UnityEngine;

namespace ms
{
    // View on stage which follows the player object.
    public class Camera
    {
        // Initialize everything to 0, we need the player's spawnpoint first to properly set the position.
        public Camera()
        {
            x.set(0.0);
            y.set(0.0);

            VWIDTH = Constants.get().get_viewwidth();
            VHEIGHT = Constants.get().get_viewheight();
        }

        public void Reset ()
        {
            //GameUtil.Instance.mainCamera.transform.position = new Vector3 (VWIDTH, -VHEIGHT, -9999);
        }
        
        // Update the view with the current player position. (Or any other target)
        public void update(Point_short position)
        {
            //GameUtil.Instance.mainCamera.transform.position = new Vector3 (position.x (), -position.y (), -9999);
            //AppDebug.Log ($"player position: {position}");
            var new_width = Constants.get().get_viewwidth();
            var new_height = Constants.get().get_viewheight();

            if (VWIDTH != new_width || VHEIGHT != new_height)
            {
                VWIDTH = new_width;
                VHEIGHT = new_height;
            }

            double next_x = x.get();
            double hdelta = VWIDTH / 2 - position.x() - next_x;

            if (Math.Abs(hdelta) >= 5.0)
            {
                next_x += hdelta * (12.0 / VWIDTH);
            }

            double next_y = y.get();
            double vdelta = VHEIGHT / 2 - position.y() - next_y;

            if (Math.Abs(vdelta) >= 5.0)
            {
                next_y += vdelta * (12.0 / VHEIGHT);
            }

            if (next_x > hbounds.first() || hbounds.length() < VWIDTH)
            {
                next_x = hbounds.first();
                if (hbounds.length () < VWIDTH)
                {
	                next_x = hbounds.first () + (VWIDTH- hbounds.length ())/2;
                }
            }
            else if (next_x < hbounds.second() + VWIDTH)
            {
                next_x = hbounds.second() + VWIDTH;
            }

            if (next_y > vbounds.first() || vbounds.length() < VHEIGHT)
            {
                next_y = vbounds.first();
                if (vbounds.length () < VHEIGHT)
                {
	                next_y = vbounds.first () + (VHEIGHT - vbounds.length ()) / 2;
                }
            }
            else if (next_y < vbounds.second() + VHEIGHT)
            {
                next_y = vbounds.second() + VHEIGHT;
            }

            x = next_x;
            y = next_y;
            /*double next_x = -position.x ();
            double next_y = -position.y ();
            if (next_x > hbounds.first () || hbounds.length () < VWIDTH)
            {
	            next_x = hbounds.first ();
            }
            else if (next_x < hbounds.second () + VWIDTH)
            {
	            next_x = hbounds.second () + VWIDTH;
            }

            if (next_y > vbounds.first () || vbounds.length () < VHEIGHT)
            {
	            next_y = vbounds.first ();
            }
            else if (next_y < vbounds.second () + VHEIGHT)
            {
	            next_y = vbounds.second () + VHEIGHT;
            }
            x.set (next_x);
            y.set (next_y);*/
            /*x.set (-position.x ());
            y.set ( -position.y ());*/
            //AppDebug.Log($"{position}");
        }

        // Set the position, changing the view immediately.
        public void set_position(Point_short position)
        {
            var new_width = Constants.get().get_viewwidth();
            var new_height = Constants.get().get_viewheight();

            if (VWIDTH != new_width || VHEIGHT != new_height)
            {
                VWIDTH = new_width;
                VHEIGHT = new_height;
            }

            x.set(VWIDTH / 2 - position.x());
            y.set(VHEIGHT / 2 - position.y());
        }

        // Updates the view's boundaries. Determined by mapinfo or footholds.
        public void set_view(Range_short mapwalls, Range_short mapborders)
        {
            hbounds = new Range_short(-mapwalls);
            vbounds = new Range_short(-mapborders);
        }

        // Return the current position.
        public Point_short position()
        {
            var shortx = (short)Math.Round(x.get());
            var shorty = (short)Math.Round(y.get());

            return new Point_short(shortx, shorty);
        }

        // Return the interpolated position.
        public Point_short position(float alpha)
        {
            var interx = (short)Math.Round(x.get(alpha));
            var intery = (short)Math.Round(y.get(alpha));

            return new Point_short(interx, intery);
        }

        // Return the interpolated position.
        public Point_double realposition(float alpha)
        {
            return new Point_double(x.get(alpha), y.get(alpha));
        }



        // Movement variables.
        public Linear_double x = new Linear_double();
        public Linear_double y = new Linear_double();

        // View limits.
        public Range_short hbounds = new Range_short();
        public Range_short vbounds = new Range_short();

        private short VWIDTH;
        private short VHEIGHT;
    }
}