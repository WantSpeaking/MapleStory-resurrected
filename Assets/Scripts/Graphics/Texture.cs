#define USE_NX

//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////


#if USE_NX
#endif

using System;
using System.Drawing;
using System.IO;
using Assets.ms.Helper;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using UnityEngine;

namespace ms
{
	// Represents a single image loaded from a of game data
	public class Texture : System.IDisposable
	{
		GameObject spriteObj;
		SpriteRenderer spriteRenderer;
		string fullPath;

		public Texture ()
		{
		}

		public Texture (WzObject src)
		{
			if (src is WzCanvasProperty || src is WzUOLProperty)
			{
				fullPath = src.FullPath;
				origin = src["origin"]?.GetPoint ().ToMSPoint () ?? Point<short>.zero;

				bitmap = src.GetBitmap ();

				dimensions = new Point<short> ((short)bitmap.Width, (short)bitmap.Height);

				//GraphicsGL.get().addbitmap(bitmap);todo render unity
				//Debug.Log ($"{src?.FullPath} \t {src?.GetType ()}", spriteObj);
			}
		}

		public void Dispose ()
		{
		}

		public Point<short> get_origin ()
		{
			return origin;
		}

		public Point<short> get_dimensions ()
		{
			return dimensions;
		}

		private Bitmap bitmap;
		private Point<short> origin = new Point<short> ();
		private Point<short> dimensions = new Point<short> ();

		public void draw (DrawArgument args)
		{
			if (spriteRenderer == null)
			{
				spriteObj = new GameObject ();
				spriteRenderer = spriteObj.AddComponent<SpriteRenderer> ();
			}

			if (spriteRenderer != null)
			{
				Debug.Log ($"{fullPath} {origin}", spriteRenderer.gameObject);
				var ms_Rect = args.get_rectangle (origin, dimensions);
				spriteRenderer.gameObject.name = fullPath;
				spriteRenderer.sprite = TextureToSprite (bitmap, origin, dimensions, new Rect (ms_Rect.X, ms_Rect.Y, ms_Rect.Width, ms_Rect.Height), args, out var pos);
				spriteRenderer.sortingLayerName = args.sortingLayer.ToString ();
				spriteRenderer.sortingOrder = args.orderInLayer;
				setPos (pos);
			}
		}

		private void setPos (Vector3 pos)
		{
			if (spriteRenderer?.gameObject is GameObject gameObject)
			{
				gameObject.transform.position = pos;
			}
		}

		public void draw ()
		{
		}

		public void shift (Point<short> amount)
		{
			origin = origin - amount;
		}

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

		private static UnityEngine.Sprite TextureToSprite (Bitmap bitmap, Point<short> origin, Point<short> dimensions, Rect rect, DrawArgument args, out Vector3 pos)
		{
			Texture2D t2d = new Texture2D (dimensions.x (), dimensions.y ());
			t2d.LoadImage (ImageToByte2 (bitmap));
			t2d.Apply ();

			var posX = args.get_Pos ().x ();
			var posY = args.get_Pos ().y ();
			var pivotX = origin.x ();
			var pivotY = origin.y ();
			var width = dimensions.x ();
			var height = dimensions.y ();
			var relativeAnchorX = (float)pivotX / width;
			var relativeAnchorY = (float)pivotY / height;

			pos = new Vector3 (posX, -posY, 0);

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
}


#if USE_NX
#endif