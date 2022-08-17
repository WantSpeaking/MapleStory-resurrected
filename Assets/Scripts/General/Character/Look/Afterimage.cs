#define USE_NX

using System;
using MapleLib.WzLib;





namespace ms
{
	public class Afterimage
	{
		public Afterimage (int skill_id, string name, string stance_name, short level)
		{
			WzObject src = null;

			if (skill_id > 0)
			{
				string strid = string_format.extend_id (skill_id, 7);
				src = ms.wz.wzFile_skill[strid.Substring (0, 3) + ".img"]?["skill"]?[strid]?["afterimage"]?[name]?[stance_name];
			}

			if (src == null)
			{
				src = ms.wz.wzFile_character["Afterimage"]?[name + ".img"]?[(level / 10).ToString ()]?[stance_name];
			}

			if (src == null)
			{
				AppDebug.Log ($"Afterimage() src == null");
				return;
			}
			
			range = new Rectangle_short (src);
			firstframe = 0;
			displayed = false;

			
			foreach (var sub in ((WzImageProperty)src).WzProperties)
			{
				byte frame = string_conversion.or_default (sub.Name, (byte)255);

				if (frame < 255)
				{
					animation =new Animation (sub);
					firstframe = frame;
				}
			}
		}

		public Afterimage ()
		{
			firstframe = 0;
			displayed = true;
		}

		//private static DrawArgument renderOrderArgs = new DrawArgument(Constants.get ().sortingLayer_Effect,0);
		public void draw (byte stframe, DrawArgument args, float alpha)
		{
			if (!displayed && stframe >= firstframe)
			{
				//animation.draw (args + renderOrderArgs, alpha);
				animation.draw (args, alpha);
			}
		}

		public void update (byte stframe, ushort timestep)
		{
			if (!displayed && stframe >= firstframe)
			{
				displayed = animation.update (timestep);
			}

			if (displayed)
			{
				animation?.Dispose ();
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: byte get_first_frame() const
		public byte get_first_frame ()
		{
			return firstframe;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Rectangle_short get_range() const
		public Rectangle_short get_range ()
		{
			return range;
		}

		private Animation animation = new Animation ();
		private Rectangle_short range = new Rectangle_short ();
		private byte firstframe;
		private bool displayed;
	}
}


#if USE_NX
#endif