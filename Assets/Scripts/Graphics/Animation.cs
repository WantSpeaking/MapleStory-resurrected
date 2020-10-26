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
		public Frame (WzObject src) // Map.wz/Back/grassySoil.img/ani/0
		{
			temp = src;

			texture = new Texture (src);
			bounds = new Rectangle<short> (src);
			head = src["head"];
			delay = src["delay"];

			if (delay == 0)
				delay = 100;

			bool hasa0 = ((src["a0"] as WzImageProperty)?.PropertyType ?? WzPropertyType.Null) == WzPropertyType.Int;
			bool hasa1 = ((src["a1"] as WzImageProperty)?.PropertyType ?? WzPropertyType.Null) == WzPropertyType.Int;

			if (hasa0 && hasa1)
			{
				opacities = new Tuple<byte, byte> (src["a0"], src["a1"]);
			}
			else if (hasa0)
			{
				byte a0 = src["a0"];
				opacities = new Tuple<byte, byte> (a0, (byte)(255 - a0));
			}
			else if (hasa1)
			{
				byte a1 = src["a1"];
				opacities = new Tuple<byte, byte> ((byte)(255 - a1), a1);
			}
			else
			{
				opacities = new Tuple<byte, byte> (255, 255);
			}

			bool hasz0 = ((src["z0"] as WzImageProperty)?.PropertyType ?? WzPropertyType.Null) == WzPropertyType.Int;

			bool hasz1 = ((src["z1"] as WzImageProperty)?.PropertyType ?? WzPropertyType.Null) == WzPropertyType.Int;


			if (hasz0 && hasz1)
				scales = new Tuple<short, short> (src["z0"], src["z1"]);
			else if (hasz0)
				scales = new Tuple<short, short> (src["z0"], 0);
			else if (hasz1)
				scales = new Tuple<short, short> (100, src["z1"]);
			else
				scales = new Tuple<short, short> (100, 100);
		}

		public Frame ()
		{
			delay = 0;
			opacities = new Tuple<byte, byte> (0, 0);
			scales = new Tuple<short, short> (0, 0);
		}

		public void erase ()
		{
			texture.erase ();
		}

		private WzObject temp;

		public void draw (DrawArgument args)
		{
			//Debug.Log ($"FullPath:{temp?.FullPath}");

			texture.draw (args);
		}

		public byte start_opacity ()
		{
			return opacities.Item1;
		}

		public ushort start_scale ()
		{
			return (ushort)scales.Item1;
		}

		public ushort get_delay ()
		{
			return delay;
		}

		public Point<short> get_origin ()
		{
			return texture.get_origin ();
		}

		public Point<short> get_dimensions ()
		{
			return texture.get_dimensions ();
		}

		public Point<short> get_head ()
		{
			return head;
		}

		public Rectangle<short> get_bounds ()
		{
			return bounds;
		}

		public float opcstep (ushort timestep)
		{
			return timestep * (float)(opacities.Item2 - opacities.Item1) / delay;
		}

		public float scalestep (ushort timestep)
		{
			return timestep * (float)(scales.Item2 - scales.Item1) / delay;
		}

		private Point<short> head = new Point<short> ();
		private Rectangle<short> bounds = new Rectangle<short> ();
		private Texture texture = new Texture ();
		private ushort delay;
		private Tuple<byte, byte> opacities = new Tuple<byte, byte> (0, 0);

		private Tuple<short, short> scales = new Tuple<short, short> (0, 0);
		//private Rectangle<short> bounds = new Rectangle<short>();
		//private Point<short> head = new Point<short>();
	}

	public class Animation
	{
		private SortedSet<short> frameids = new SortedSet<short> ();

		public Animation (WzObject src) // Map.wz/Back/grassySoil.img/ani/0
		{
			if (src == null)
			{
				Debug.LogWarning ($"Animation(src):{src}");
				frames.Add (new Frame ());
				return;
			}
			//bool istexture = (src is WzCanvasProperty);
			bool istexture = ((src as WzImageProperty)?.PropertyType ?? WzPropertyType.Null) == WzPropertyType.Canvas;
			//Debug.Log ($"{src.FullPath} istexture:{istexture} type:{(src as WzImageProperty)?.PropertyType}");

			//var temp1 = src;
			//var temp2 = src["0"];
			//Debug.Log("temp1\t" + temp1 + "\t" + temp1.FullPath + "\t" + (temp1 is WzCanvasProperty) + "\t" + (temp1 is WzSubProperty));
			//Debug.Log("temp2\t" + temp2 + "\t" + src.FullPath + "\t" + (temp2 is WzCanvasProperty) + "\t" + (temp2 is WzSubProperty));

			if (istexture) //WzCanvasProperty
			{
				frames.Add (new Frame (src));
			}
			/*else //WzSubProperty
			{
				/*if (src is WzImageProperty node_Anim)
				{
				    foreach (var node_Frame in node_Anim.WzProperties)
				    {
				        var frame = new Frame(node_Frame);
				        frames.Add(frame);
				    }
				}#1#
				var frame = new Frame (src["0"]);
				frames.Add (frame);
				
			}*/

			else
			{
				if (src is WzImageProperty node_Anim)
				{
					foreach (var sub in node_Anim.WzProperties)
					{
						//Debug.Log ($"{sub.FullPath} istexture:{istexture} type:{(sub as WzImageProperty)?.PropertyType}");
						if (sub.PropertyType == WzPropertyType.Canvas)
						{
							short fid = string_conversion.or_default (sub.Name, (short)-1);

							if (fid >= 0)
							{
								frameids.Add (fid);
							}
						}
					}

					foreach (var fid in frameids)
					{
						var sub = src[Convert.ToString (fid)];
						frames.Add (new Frame (sub));
					}

					if (frames.Count == 0)
					{
						frames.Add (new Frame ());
					}
				}
			}

			animated = frames.Count > 1;
			zigzag = src["zigzag"];

			reset ();
		}

		public Animation ()
		{
			animated = false;
			zigzag = false;

			frames.Add (new Frame ());

			reset ();
		}


		public void reset ()
		{
			frame.set (0);
			opacity.set (frames[0].start_opacity ());
			xyscale.set (frames[0].start_scale ());
			delay = frames[0].get_delay ();
			framestep = 1;
			lastDraw_args = null;
			lastDraw_interframe = -1;
		}

		private DrawArgument lastDraw_args;
		private short lastDraw_interframe = -1;

		private void erase (short interframe)
		{
			frames[interframe].erase ();
		}

		public void draw (DrawArgument args, float alpha)
		{
			if (lastDraw_interframe != -1)
			{
				erase (lastDraw_interframe);
			}

			short interframe = frame.get (alpha);
			float interopc = opacity.get (alpha) / 255;
			float interscale = xyscale.get (alpha) / 100;

			bool modifyopc = interopc != 1.0f;
			bool modifyscale = interscale != 1.0f;

			if (modifyopc || modifyscale)
				frames[interframe].draw (args + new DrawArgument (interscale, interscale, interopc));
			else
				frames[interframe].draw (args);

			lastDraw_interframe = interframe;
		}

		/*public bool update ()
		{
			return update((ushort)(Constants.TIMESTEP*Constants.get ().frameDelay));
		}*/

		public bool update (ushort timestep = Constants.TIMESTEP)
		{
			timestep = (ushort)(timestep * Constants.get ().frameDelay);
			
			Frame framedata = get_frame ();

			opacity += framedata.opcstep (timestep);

			if (opacity.last () < 0.0f)
			{
				opacity.set (0.0f);
			}
			else if (opacity.last () > 255.0f)
			{
				opacity.set (255.0f);
			}

			xyscale += framedata.scalestep (timestep);

			if (xyscale.last () < 0.0f)
			{
				opacity.set (0.0f);
			}

			if (timestep >= delay)
			{
				short lastframe = (short)(frames.Count - 1);
				short nextframe;
				bool ended;

				if (zigzag && lastframe > 0)
				{
					if (framestep == 1 && frame == lastframe)
					{
						framestep = (short)-framestep;
						ended = false;
					}
					else if (framestep == -1 && frame == 0)
					{
						framestep = (short)-framestep;
						ended = true;
					}
					else
					{
						ended = false;
					}

					nextframe = frame + framestep;
				}
				else
				{
					if (frame == lastframe)
					{
						nextframe = 0;
						ended = true;
					}
					else
					{
						nextframe = frame + 1;
						ended = false;
					}
				}

				ushort delta = (ushort)(timestep - delay);
				float threshold = (float)delta / timestep;
				frame.next (nextframe, threshold);

				delay = frames[nextframe].get_delay ();

				if (delay >= delta)
				{
					delay -= delta;
				}

				opacity.set (frames[nextframe].start_opacity ());
				xyscale.set (frames[nextframe].start_scale ());

				return ended;
			}
			else
			{
				frame.normalize ();

				delay -= timestep;

				return false;
			}
		}

		private ushort get_delay (short frame_id)
		{
			return (ushort)(frame_id < frames.Count ? frames[frame_id].get_delay () : 0);
		}

		private ushort getdelayuntil (short frame_id)
		{
			ushort total = 0;

			for (short i = 0; i < frame_id; i++)
			{
				if (i >= frames.Count)
					break;

				total += frames[frame_id].get_delay ();
			}

			return total;
		}

		public Point<short> get_origin ()
		{
			return get_frame ().get_origin ();
		}

		public Point<short> get_dimensions ()
		{
			return get_frame ().get_dimensions ();
		}

		public Point<short> get_head ()
		{
			return get_frame ().get_head ();
		}

		public Rectangle<short> get_bounds ()
		{
			return get_frame ().get_bounds ();
		}

		private Frame get_frame ()
		{
			return frames[frame.get()];
			//return frames[0];
		}

		private List<Frame> frames = new List<Frame> ();
		private bool animated;
		private bool zigzag;

		private Nominal<short> frame = new Nominal<short> ();
		private Linear<float> opacity = new Linear<float> ();
		private Linear<float> xyscale = new Linear<float> ();

		private ushort delay;
		private short framestep;
		private float opcstep;
	}
}