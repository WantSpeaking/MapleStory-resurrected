using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using ms;
using UnityEngine;

namespace ms
{
    public class Frame
    {
        public Frame(WzObject src)// Map.wz/Back/grassySoil.img/ani/0
        {
            texture = new Texture(src);
            //bounds = src;
            //head = src["head"];
            //delay = src["delay"];

        }
        public Frame()
        {
            /*delay = 0;
			opacities = { 0, 0};
			scales = { 0, 0};*/
        }
        public void draw(DrawArgument args)
        {
            texture.draw(args);
        }
        public void draw()
        {
            texture.draw();

        }

        public Point<short> get_dimensions()
        {
            return texture.get_dimensions();
        }

        private global::ms.Texture texture = new global::ms.Texture();
        private ushort delay;
        private System.Tuple<byte, byte> opacities = new System.Tuple<byte, byte>(0, 0);
        private System.Tuple<short, short> scales = new System.Tuple<short, short>(0, 0);
        //private Rectangle<short> bounds = new Rectangle<short>();
        //private Point<short> head = new Point<short>();


    }

    class Animation
    {
        public Animation(WzObject src)// Map.wz/Back/grassySoil.img/ani/0
        {
            bool istexture = (src is WzCanvasProperty);
            var temp1 = src;
            var temp2 = src["0"];
            //Debug.Log("temp1\t" + temp1 + "\t" + temp1.FullPath + "\t" + (temp1 is WzCanvasProperty) + "\t" + (temp1 is WzSubProperty));
            //Debug.Log("temp2\t" + temp2 + "\t" + src.FullPath + "\t" + (temp2 is WzCanvasProperty) + "\t" + (temp2 is WzSubProperty));

            if (istexture)//WzCanvasProperty
            {
                var frame = new Frame(src);
                frames.Add(frame);
            }
            else//WzSubProperty
            {
                /*if (src is WzImageProperty node_Anim)
                {
                    foreach (var node_Frame in node_Anim.WzProperties)
                    {
                        var frame = new Frame(node_Frame);
                        frames.Add(frame);
                    }
                }*/
                var frame = new Frame(src["0"]);
                frames.Add(frame);
            }
            /*else
			{
				SortedSet<short> frameids = new SortedSet<short>();

				foreach (var sub in src)
				{
					if (sub.data_type() == nl.node.type.bitmap)
					{
						short fid = string_conversion.GlobalMembers.or_default<short>(sub.name(), -1);

						if (fid >= 0)
						{
							frameids.Add(fid);
						}
					}
				}

				foreach (var fid in frameids)
				{
					var sub = src[Convert.ToString(fid)];
					frames.Add(sub);
				}

				if (frames.Count == 0)
				{
					frames.Add(new Frame());
				}
			}

			animated = frames.Count > 1;
			zigzag = src["zigzag"].get_bool();

			reset();*/
        }
        public Animation()
        {
            animated = false;
            zigzag = false;

            frames.Add(new Frame());

            //reset();
        }

        public void update()
        {

        }
        public void draw(DrawArgument args, float alpha)
        {
            /*short interframe = frame.get(alpha);
			float interopc = opacity.get(alpha) / 255;
			float interscale = xyscale.get(alpha) / 100;*/

            short interframe = 0;
            float interopc = 1;
            float interscale = 1;

            bool modifyopc = interopc != 1.0f;
            bool modifyscale = interscale != 1.0f;

            /*if (modifyopc || modifyscale)
			{
				frames[interframe].draw(args + new DrawArgument(interscale, interscale, interopc));
			}
			else*/
            {
                frames[interframe].draw(args);
            }
        }
        public void draw()
        {
            foreach (var frame in frames)
            {
                frame.draw();

            }
        }

        public Point<short> get_dimensions()
        {
            return get_frame().get_dimensions();
        }

        private Frame get_frame()
        {
            //return frames[frame.get()];
            return frames[0];
        }

        private List<Frame> frames = new List<Frame>();
        private bool animated;
        private bool zigzag;

        /*private Nominal<short> frame = new Nominal<short>();
		private Linear<float> opacity = new Linear<float>();
		private Linear<float> xyscale = new Linear<float>();*/

        private ushort delay;
        private short framestep;
        private float opcstep;
    }
}
