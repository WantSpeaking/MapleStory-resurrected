using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using ms;
using provider;

namespace ms
{
	public class Frame : IDisposable
	{
		public Frame (WzObject src, string layerMaskName,string sortingLayerName) // Map.wz/Back/grassySoil.img/ani/0/0
		{
			if (src == null)
			{
				AppDebug.LogError("Frame src is null");
			}
			if (src is WzUOLProperty uOLProperty)
			{
				src = uOLProperty.LinkValue;
            }

			temp = src;

			texture = new Texture (src, layerMaskName, sortingLayerName);
			bounds = new Rectangle_short (src);
			head = src["head"];
			delay = src["delay"];
			//if (temp?.FullPath.Contains ("1210100.img") ?? false)
			//AppDebug.Log ($"\t delay:{delay}");
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
        public Frame(MapleData src, string layerMaskName, string sortingLayerName) // Map.wz/Back/grassySoil.img/ani/0/0
        {
            if (src == null)
            {
                AppDebug.LogError("Frame src is null");
            }
            /*if (src is WzUOLProperty uOLProperty)
            {
                src = uOLProperty.LinkValue;
            }*/

            temp_maple = src;

            texture = new Texture(src, layerMaskName, sortingLayerName);
            bounds = new Rectangle_short(src);
            head = src["head"];
            delay = src["delay"];
            //if (temp?.FullPath.Contains ("1210100.img") ?? false)
            //AppDebug.Log ($"\t delay:{delay}");
            if (delay == 0)
                delay = 100;

			bool hasa0 = src["a0"] != null ;
            bool hasa1 = src["a1"] != null;

            if (hasa0 && hasa1)
            {
                opacities = new Tuple<byte, byte>(src["a0"], src["a1"]);
            }
            else if (hasa0)
            {
                byte a0 = src["a0"];
                opacities = new Tuple<byte, byte>(a0, (byte)(255 - a0));
            }
            else if (hasa1)
            {
                byte a1 = src["a1"];
                opacities = new Tuple<byte, byte>((byte)(255 - a1), a1);
            }
            else
            {
                opacities = new Tuple<byte, byte>(255, 255);
            }

            bool hasz0 = src["z0"]!=null;

            bool hasz1 = src["z1"] != null;


            if (hasz0 && hasz1)
                scales = new Tuple<short, short>(src["z0"], src["z1"]);
            else if (hasz0)
                scales = new Tuple<short, short>(src["z0"], 0);
            else if (hasz1)
                scales = new Tuple<short, short>(100, src["z1"]);
            else
                scales = new Tuple<short, short>(100, 100);
        }
        public Frame ()
		{
			delay = 0;
			opacities = new Tuple<byte, byte> (0, 0);
			scales = new Tuple<short, short> (0, 0);

			head = new Point_short ();
			bounds = new Rectangle_short ();
			texture = new Texture ();
		}

		public void erase ()
		{
			texture.erase ();
		}

		private WzObject temp;
		private MapleData temp_maple;

        public void draw (DrawArgument args)
		{
			//AppDebug.Log ($"frame draw FullPath:{temp?.FullPath}");

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

		public Point_short get_origin ()
		{
			return texture.get_origin ();
		}

		public Point_short get_dimensions ()
		{
			return texture.get_dimensions ();
		}

		public Point_short get_head ()
		{
			return head;
		}

		public Rectangle_short get_bounds (bool debug = false)
		{
			if (debug)
			{
				//AppDebug.Log ($"frame FullPath:{temp?.FullPath}\t get_bounds:{bounds} ");
			}
			return new Rectangle_short (bounds);
			//return bounds;
		}

		public float opcstep (ushort timestep)
		{
			return timestep * (float)(opacities.Item2 - opacities.Item1) / delay;
		}

		public float scalestep (ushort timestep)
		{
			return timestep * (float)(scales.Item2 - scales.Item1) / delay;
		}

		public virtual void Dispose ()
		{
			texture.Dispose ();
		}
		public short Width => get_dimensions()?.x() ?? 0;
        public short Height => get_dimensions()?.y() ?? 0;

        private Point_short head;
		private Rectangle_short bounds;
		private Texture texture;
		private ushort delay;
		private Tuple<byte, byte> opacities;

		private Tuple<short, short> scales;
        //private Rectangle_short bounds = new Rectangle_short();
        //private Point_short head = new Point_short();

        public Texture get_Texture() => texture;
        //public Point_short get_Point() => pos;

    }

	public class Animation : IDisposable
	{
		//private SortedSet<short> frameids = new SortedSet<short> ();
		SortedDictionary<short,string> frameid_name_dict = new SortedDictionary<short,string> ();
		private WzObject cache_src { get; set; }
		private MapleData cache_src_mapledata { get; set; }
        private string layerMaskName;
		private string sortingLayerName;

        public Animation (WzObject src) // Map.wz/Back/grassySoil.img/ani/0
		{
			Init (src);
		}
        public Animation(WzObject src, string layerMaskName = GameUtil.layerNameDefault, string sortingLayerName = GameUtil.sortingLayerName_Default) // Map.wz/Back/grassySoil.img/ani/0
        {
			
            Init(src, layerMaskName, sortingLayerName);
        }
        public Animation(MapleData src, string layerMaskName = GameUtil.layerNameDefault, string sortingLayerName = GameUtil.sortingLayerName_Default) // Map.wz/Back/grassySoil.img/ani/0
        {

            Init(src, layerMaskName, sortingLayerName);
        }

        public Animation ()
		{
			animated = false;
			zigzag = false;

			frames.Add (new Frame ());

			reset ();
		}

		public Animation (Animation srcAnimation)
		{
			if (srcAnimation?.cache_src != null)
			{
                Init(srcAnimation?.cache_src, srcAnimation?.layerMaskName, srcAnimation?.sortingLayerName);
            }
			else
			{
                Init(srcAnimation?.cache_src_mapledata, srcAnimation?.layerMaskName, srcAnimation?.sortingLayerName);
            }
        }

		private void Init (WzObject src,string layerMaskName = GameUtil.layerNameDefault,string sortingLayerName = GameUtil.sortingLayerName_Default)
		{
            this.layerMaskName = layerMaskName;
            this.sortingLayerName = sortingLayerName;

            if (src == null)//801040004.img/ back/0/bS 值为空
            {
				//AppDebug.Log ($"Animation(src):src == null");
				frames.Add (new Frame ());
				return;
			}

			cache_src = src;

			bool istexture = src.IsTexture ();

			if (istexture) //WzCanvasProperty
			{
				frames.Add (new Frame (src, layerMaskName,sortingLayerName));
			}
			else
			{
				if (src is WzImageProperty node_Anim)
				{
					foreach (var sub in node_Anim.WzProperties)
					{
						//AppDebug.Log ($"{sub.FullPath} istexture:{istexture} type:{(sub as WzImageProperty)?.PropertyType}");
						if (sub.IsTexture())
						{
							short fid = string_conversion.or_default (sub.Name, (short)-1);
							
							if (fid >= 0)
							{
								//frameids.Add (fid);
								frameid_name_dict.Add(fid, sub.Name);
							}
						}
					}

					foreach (var fid in frameid_name_dict.Values)
					{
						var sub = src[fid];
						frames.Add (new Frame (sub, layerMaskName,sortingLayerName));//KualaLumpur_MY.img/Adventure Rides/adventure/3/3空格，名字中含有空格，转换过来的没有空格，导致读取不到
                    }

					if (frames.Count == 0)
					{
						frames.Add(new Frame());
					}
				}
			}

			animated = frames.Count > 1;
			zigzag = src["zigzag"];

			reset ();
		}
        private void Init(MapleData src, string layerMaskName = GameUtil.layerNameDefault, string sortingLayerName = GameUtil.sortingLayerName_Default)
        {
            this.layerMaskName = layerMaskName;
            this.sortingLayerName = sortingLayerName;

            if (src == null)//801040004.img/ back/0/bS 值为空
            {
                //AppDebug.Log ($"Animation(src):src == null");
                frames.Add(new Frame());
                return;
            }

            cache_src_mapledata = src;

            bool istexture = src.IsTexture();

            if (istexture) //WzCanvasProperty
            {
                frames.Add(new Frame(src, layerMaskName, sortingLayerName));
            }
            else
            {
                if (src is MapleData node_Anim)
                {
                    foreach (var sub in node_Anim)
                    {
                        //AppDebug.Log ($"{sub.FullPath} istexture:{istexture} type:{(sub as WzImageProperty)?.PropertyType}");
                        if (sub.IsTexture())
                        {
                            short fid = string_conversion.or_default(sub.Name, (short)-1);

                            if (fid >= 0)
                            {
                                //frameids.Add (fid);
                                frameid_name_dict.Add(fid, sub.Name);
                            }
                        }
                    }

                    foreach (var fid in frameid_name_dict.Values)
                    {
                        var sub = src[fid];
                        frames.Add(new Frame(sub, layerMaskName, sortingLayerName));//KualaLumpur_MY.img/Adventure Rides/adventure/3/3空格，名字中含有空格，转换过来的没有空格，导致读取不到
                    }

                    if (frames.Count == 0)
                    {
                        frames.Add(new Frame());
                    }
                }
            }

            animated = frames.Count > 1;
            zigzag = src["zigzag"];

            reset();
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

		public void eraseAllFrame ()
		{
			foreach (var frame in frames)
			{
				frame.erase ();
			}
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

			var additionalArgs = new DrawArgument (interscale, interscale, interopc);
			if (frames.Count == 1)
			{
				//additionalArgs.SetDrawOnce (true);
			}
			if (modifyopc || modifyscale)
				frames[interframe].draw (args + additionalArgs);
			else
				frames[interframe].draw (args.SetDrawOnce (true));

			lastDraw_interframe = interframe;

			/*if ((cache_src?.FullPath.Contains ("1311001") ?? false)&&interframe==0)
				AppDebug.Log ($"draw interopc : {interopc}\t {cache_src?.FullPath}");*/
			/*if ((cache_src?.FullPath.Equals (@"skill.wz\131.img\skill\1311001\effect") ?? false))
				AppDebug.Log ($"animation:{cache_src?.FullPath}\t frame:{interframe}\t alpha:{interopc}");*/
		}

		/*public bool update ()
		{
			return update((ushort)(Constants.TIMESTEP*Constants.get ().frameDelay));
		}*/

		public bool update (ushort timestep = Constants.TIMESTEP)
		{
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

				//if (temp_src?.FullPath.Contains ("1210100.img") ?? false)
				//AppDebug.Log ($"update nextframe : {nextframe}\t threshold:{threshold}\t timestep:{timestep}\t delay:{delay}\t delta:{delta}");

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
		public void ClearDisplayObj ()
		{
			foreach (var frame in frames)
			{
				
				//frame.erase
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
		public int get_total_delay ()
		{
			return frames.Sum (frame => frame.get_delay ());
		}

        public int GetAvergeFrameLength()
		{
            return (int)frames.Average(f=>f.get_delay ());
        }
        
        public Point_short get_origin ()
		{
			return get_frame ().get_origin ();
		}

		public Point_short get_dimensions ()
		{
			return get_frame ().get_dimensions ();
		}

		public Point_short get_head ()
		{
			return get_frame ().get_head ();
		}

		public Rectangle_short get_bounds (bool debug = false)
		{
			return get_frame ().get_bounds (debug);
		}

		public Frame get_frame (bool debug = false)
		{
			return frames[frame.get ()];
			//return frames[0];
		}

		public virtual void Dispose ()
		{
			foreach (var frame in frames)
			{
				frame.Dispose ();
			}
		}

		private List<Frame> frames = new List<Frame> ();
		private bool animated;
		private bool zigzag;

		private Nominal_short frame = new Nominal_short ();
		private Linear_float opacity = new Linear_float ();
		private Linear_float xyscale = new Linear_float ();

		private ushort delay;
		private short framestep;
		private float opcstep;

        public IReadOnlyList<Frame> get_frames()
        {
            return frames;
        }

		public WzObject get_CacheWzObject ()
		{
			return cache_src;
		}
    }

	public enum Enum_Animation
	{
		ShowChargeHeavyAttack,
	}
}