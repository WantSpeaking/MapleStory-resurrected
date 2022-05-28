using System;
using System.Collections.Generic;
using System.Text;
//using Microsoft.Xna.Framework.Graphics;

namespace MapleLib.WzLib
{
	public class Bitmap
	{
		public byte[] RawBytes;
		public int Width;
		public int Height;
		public int format;
		//public SurfaceFormat surfaceFormat;

		public Bitmap (byte[] rawBytes, int width, int height, int format)
		{
			RawBytes = rawBytes;
			this.Width = width;
			this.Height = height;
			this.format = format;
		}
		public Bitmap (int width, int height)
		{
			RawBytes = new byte[4];
			this.Width = width;
			this.Height = height;
			this.format = 3;
		}

		public bool isValid => RawBytes != null && Width > 1 && Height > 1;
	}
}
