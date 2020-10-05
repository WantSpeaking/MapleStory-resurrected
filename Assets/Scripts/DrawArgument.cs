using System.Drawing;

namespace ms
{
    public class DrawArgument
    {
        public DrawArgument()
        {
        }

        public DrawArgument(Point<short> position, bool flip, float opacity)
        {
            pos = position;
            center = position;
            xscale = 1;
            yscale = 1;
        }

        public DrawArgument(Point<short> position, bool flip, float opacity,short cx,short cy)
        {
            pos = position;
            center = position;
            xscale = 1;
            yscale = 1;
            this.cx = cx;
            this.cy = cy;
        }

        public Point<short> get_Pos()
        {
            return pos;
        }
        public Rectangle get_rectangle(Point<short> origin, Point<short> dimensions)
        {
            short w = stretch.x();

            if (w == 0)
            {
                w = dimensions.x();
            }

            short h = stretch.y();

            if (h == 0)
            {
                h = dimensions.y();
            }

            Point<short> rlt = new Point<short>((short)(pos.x() - center.x() - origin.x()), (short)(pos.y() - center.y() - origin.y()));
            short rl = rlt.x();
            short rr = (short)(rlt.x() + w);
            short rt = rlt.y();
            short rb = (short)(rlt.y() + h);
            short cx = center.x();
            short cy = center.y();

            return new Rectangle(cx + (short)(xscale * rl), cx + (short)(xscale * rr), cy + (short)(yscale * rt), cy + (short)(yscale * rb));
        }

        private Point<short> pos = new Point<short>();
        private Point<short> center = new Point<short>();
        private Point<short> stretch = new Point<short>();
        private float xscale;
        private float yscale;
        private float angle;
        public short cx;
        public short cy;
    }
}