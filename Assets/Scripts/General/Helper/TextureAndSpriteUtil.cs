using System;
using System.Drawing;
using System.IO;
using ms.Helper;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using ms;
using UnityEngine;
using Bitmap = MapleLib.WzLib.Bitmap;

public static class TextureAndSpriteUtil
{
	/*public static Texture2D GetTexrture2DFromPath (WzObject wzObject)
	{
		var bitMap = wzObject?.GetBitmap ();
		var width = bitMap?.Width ?? 0;
		var height = bitMap?.Height ?? 0;

		Texture2D t2d = new Texture2D (width, height);
		t2d.LoadImage (ImageToByte2 (bitMap));
		t2d.Apply ();
		return t2d;
	}*/

	public static UnityEngine.Sprite PngDataToSprite (byte[] pngData,Bitmap pngFormat, Point_short origin, Point_short dimensions)
	{
		if (pngData == null) return null;
		Texture2D t2d = new Texture2D (dimensions.x (), dimensions.y (), PngFormatToTextureFormat(pngFormat.format), false);
		//t2d.LoadRawTextureData (data);
		var rawTextureData = t2d.GetRawTextureData ();
		//Debug.Log ($"pngData.Length:{pngData.Length}\t rawTextureData.Length:{rawTextureData.Length}\t pngFormat:{pngFormat}\t {dimensions.x ()*dimensions.y ()}");

		t2d.SetPixelData (pngData,0,0);
		t2d.filterMode = FilterMode.Point;
		t2d.Apply ();

		var pivotX = origin.x ();
		var pivotY = origin.y ();
		var width = dimensions.x ();
		var height = dimensions.y ();
		var relativeAnchorX = (float)pivotX / width;
		var relativeAnchorY = (float)pivotY / height;

		UnityEngine.Sprite sprite = UnityEngine.Sprite.Create (t2d, new Rect (0, 0, dimensions.x (), dimensions.y ()), new Vector2 (relativeAnchorX, /*1 -*/ relativeAnchorY), 1);
		return sprite;
	}
	public static UnityEngine.Texture2D PngDataToTexture2D (byte[] pngData, Bitmap pngFormat, Point_short origin, Point_short dimensions)
	{
		if (pngData == null)
			return null;
		Texture2D t2d = new Texture2D (dimensions.x (), dimensions.y (), PngFormatToTextureFormat (pngFormat.format), false);
		//Debug.Log ($"pngData.Length:{pngData.Length}\t rawTextureData.Length:{rawTextureData.Length}\t pngFormat:{pngFormat}\t {dimensions.x ()*dimensions.y ()}");

		t2d.SetPixelData (pngData, 0, 0);
		t2d.filterMode = FilterMode.Point;
		t2d.Apply ();
		/*		//t2d.LoadRawTextureData (data);
				var rawTextureData = t2d.GetRawTextureData ();
				//Debug.Log ($"pngData.Length:{pngData.Length}\t rawTextureData.Length:{rawTextureData.Length}\t pngFormat:{pngFormat}\t {dimensions.x ()*dimensions.y ()}");

				t2d.SetPixelData (pngData, 0, 0);
				t2d.filterMode = FilterMode.Point;
				t2d.Apply ();

				var pivotX = origin.x ();
				var pivotY = origin.y ();
				var width = dimensions.x ();
				var height = dimensions.y ();
				var relativeAnchorX = (float)pivotX / width;
				var relativeAnchorY = (float)pivotY / height;

				UnityEngine.Sprite sprite = UnityEngine.Sprite.Create (t2d, new Rect (0, 0, dimensions.x (), dimensions.y ()), new Vector2 (relativeAnchorX, *//*1 -*//* relativeAnchorY), 1);*/
		return t2d;
	}
	public static UnityEngine.Texture2D PngDataToTexture2D (byte[] data, Point_short origin, Point_short dimensions)
	{
		if (data == null) return null;
		//Debug.Log ($"data.Length:{data.Length}\t {data.ToDebugLog ()}");
		Texture2D t2d = new Texture2D (dimensions.x (), dimensions.y (), TextureFormat.BGRA32, false);
		//t2d.LoadRawTextureData (data);
		t2d.SetPixelData (data, 0, 0);
		t2d.filterMode = FilterMode.Point;
		t2d.Apply ();
		return t2d;
	}

	public static Texture2D AddWatermark (Texture2D background, Texture2D watermark, int startPositionX, int startPositionY)
	{
		//only read and rewrite the area of the watermark
		for (int x = startPositionX; x < background.width; x++)
		{
			for (int y = startPositionY; y < background.height; y++)
			{
				if (x - startPositionX < watermark.width && y - startPositionY < watermark.height)
				{
					var bgColor = background.GetPixel (x, y);
					var wmColor = watermark.GetPixel (x - startPositionX, y - startPositionY);

					var finalColor = UnityEngine.Color.Lerp (bgColor, wmColor, wmColor.a / 1.0f);

					background.SetPixel (x, y, finalColor);
				}
			}
		}

		background.Apply ();
		return background;
	}

	/*public static UnityEngine.Sprite BitmapToSprite (Bitmap bitmap, Point_short origin, Point_short dimensions)
	{
		Texture2D t2d = new Texture2D (dimensions.x (), dimensions.y ());
		t2d.LoadImage (ImageToByte2 (bitmap));
		t2d.filterMode = FilterMode.Point;
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

	public static UnityEngine.Texture2D BitmapToUnityTexture2d (Bitmap bitmap, Point_short dimensions)
	{
		Texture2D t2d = new Texture2D (dimensions.x (), dimensions.y ());
		t2d.LoadImage (ImageToByte2 (bitmap));
		t2d.Apply ();

		return t2d;
	}

	public static byte[] ImageToByte2 (Image bitmap)
	{
		/*using (var stream = new MemoryStream())
		{
		    img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
		    return stream.ToArray();
		}#1#
#if !UNITY_EDITOR
		string filePath = Application.temporaryCachePath + "/Cache_Player.png";
#else
		string filePath = Application.temporaryCachePath + "/Cache_Editor.png";
#endif
		bitmap.Save (filePath);
		Stream stream = File.OpenRead (filePath);
		byte[] data = new byte[stream.Length];
		stream.Seek (0, SeekOrigin.Begin);
		stream.Read (data, 0, Convert.ToInt32 (stream.Length));
		stream.Close ();
		File.Delete (filePath);
		return data;
	}*/

	public static TextureFormat PngFormatToTextureFormat (int pngFormat)
	{
		TextureFormat result = TextureFormat.RGBA32;
		switch (pngFormat)
		{
			case 1:
			case 2:
			case 3:
			case 1026:
			case 2050:
				result = TextureFormat.RGBA32;
				break;
			case 513:
			case 517:
				result = TextureFormat.RGB565;
				break;
		}

		return result;
	}

	public static UnityEngine.Sprite LoadSpriteFromAB(string abName,string assetName)
	{
		return AssetBundleLoaderMgr.Instance.LoadSubAsset<UnityEngine.Sprite>(abName, assetName);
	}
}