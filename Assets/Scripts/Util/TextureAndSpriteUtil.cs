using System;
using System.Drawing;
using System.IO;
using ms.Helper;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using ms;
using UnityEngine;

public static class TextureAndSpriteUtil
{
	public static Texture2D GetTexrture2DFromPath (WzObject wzObject)
	{
		var bitMap = wzObject?.GetBitmap ();
		var width = bitMap?.Width ?? 0;
		var height = bitMap?.Height ?? 0;

		Texture2D t2d = new Texture2D (width, height);
		t2d.LoadImage (ImageToByte2 (bitMap));
		t2d.Apply ();
		return t2d;
	}

	public static UnityEngine.Sprite TextureToSprite (Bitmap bitmap, Point<short> origin, Point<short> dimensions)
	{
		Texture2D t2d = new Texture2D (dimensions.x (), dimensions.y ());
		t2d.LoadImage (ImageToByte2 (bitmap));
		t2d.Apply ();

		var pivotX = origin.x ();
		var pivotY = origin.y ();
		var width = dimensions.x ();
		var height = dimensions.y ();
		var relativeAnchorX = (float)pivotX / width;
		var relativeAnchorY = (float)pivotY / height;

		UnityEngine.Sprite sprite = UnityEngine.Sprite.Create (t2d, new Rect (0, 0, dimensions.x (), dimensions.y ()), new Vector2 (relativeAnchorX, 1 - relativeAnchorY), 1);

		return sprite;
	}

	public static byte[] ImageToByte2 (Image bitmap)
	{
		/*using (var stream = new MemoryStream())
		{
		    img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
		    return stream.ToArray();
		}*/

		string filePath = Application.temporaryCachePath + "/Cache.png";
		bitmap.Save (filePath);
		Stream stream = File.OpenRead (filePath);
		byte[] data = new byte[stream.Length];
		stream.Seek (0, SeekOrigin.Begin);
		stream.Read (data, 0, Convert.ToInt32 (stream.Length));
		stream.Close ();
		File.Delete (filePath);
		return data;
	}
}