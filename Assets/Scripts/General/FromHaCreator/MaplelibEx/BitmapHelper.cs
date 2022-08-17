using System;
using System.Collections.Generic;
using System.Text;
using MapleLib.WzLib;
using Microsoft.Xna.Framework.Graphics;

namespace MapleLib.WzLib
{
	public static class BitmapHelper
	{
		public static Texture2D ToTexture2D (this Bitmap bitmap, GraphicsDevice graphicsDevice)
		{
			if (bitmap == null)
			{
				return null; //todo handle this in a useful way
			}

			Texture2D t2d;
			t2d = new Texture2D (graphicsDevice, bitmap.Width, bitmap.Height, false, XnaFormatHelper.RawPng_To_XNASurfaceFormat (bitmap.format));
			t2d.SetData (bitmap.RawBytes);
			return t2d;
		}
	}
}