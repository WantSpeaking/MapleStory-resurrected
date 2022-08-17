using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace MapleLib.WzLib
{
	public static class XnaFormatHelper
	{
		public static SurfaceFormat RawPng_To_XNASurfaceFormat (int Format)
		{
			switch (Format)
			{
				case 1:
					return SurfaceFormat.Color;
				case 2:
					return SurfaceFormat.Color;
				case 3:
					return SurfaceFormat.Color;
				case 513:
				case 517:
					return SurfaceFormat.Bgr565;
				case 1026:
					return SurfaceFormat.Dxt3;
				case 2050:
					return SurfaceFormat.ColorSRgb;
				default:
					return SurfaceFormat.Color;
			}
		}
	}
}
