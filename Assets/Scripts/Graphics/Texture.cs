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
using ms.Helper;
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
		private UnityEngine.Sprite sprite;
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

				dimensions = new Point<short> ((short)(bitmap?.Width ?? 0), (short)(bitmap?.Height??0));

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

		public void erase ()
		{
			if (spriteRenderer != null)
			{
				spriteRenderer.enabled = false;
			}
		}

		public void draw (DrawArgument args)
		{
			if (bitmap == null) return;
			if (spriteRenderer == null)
			{
				spriteObj = new GameObject ();
				spriteRenderer = spriteObj.AddComponent<SpriteRenderer> ();
			}

			if (spriteRenderer != null)
			{
				spriteRenderer.enabled = true;
				//Debug.Log ($"{fullPath} {origin}", spriteRenderer.gameObject);
				if (sprite == null)
				{
					sprite = TextureAndSpriteUtil.TextureToSprite (bitmap, origin, dimensions);
				}

				spriteRenderer.gameObject.name = fullPath;
				spriteRenderer.sprite = sprite;
				spriteRenderer.sortingLayerName = args.sortingLayer.ToString ();
				spriteRenderer.sortingOrder = args.orderInLayer;
				setScale (new Vector3 (args.get_xscale (), args.get_yscale (), 1));
				setPos (new Vector3 (args.get_Pos ().x (), -args.get_Pos ().y (), 0));
			}
		}

		private void setPos (Vector3 pos)
		{
			if (spriteRenderer?.gameObject is GameObject gameObject)
			{
				gameObject.transform.position = pos;
			}
		}

		private void setScale (Vector3 pos)
		{
			if (spriteRenderer?.gameObject is GameObject gameObject)
			{
				gameObject.transform.localScale = pos;
			}
		}

		public void draw ()
		{
		}

		public void shift (Point<short> amount)
		{
			origin = origin - amount;
		}
	}
}


#if USE_NX
#endif