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
using UnityEngine.Rendering;
using Graphics = UnityEngine.Graphics;

namespace ms
{
	// Represents a single image loaded from a of game data
	public class Texture : System.IDisposable
	{
		private GameObject spriteObj;
		private SpriteRenderer spriteRenderer;
		private UnityEngine.Sprite sprite;
		public string fullPath = string.Empty;

		public Texture ()
		{
		}

		public Texture (WzObject src)
		{
			if (src?.IsTexture () ?? false)
			{
				fullPath = src.FullPath;
				pivot = src["origin"]?.GetPoint ().ToMSPoint () ?? Point<short>.zero;

				bitmap = src.GetBitmap ();

				dimensions = new Point<short> ((short)(bitmap?.Width ?? 0), (short)(bitmap?.Height ?? 0));

				//GraphicsGL.get().addbitmap(bitmap);todo render unity
				//Debug.Log ($"{src?.FullPath} \t {src?.GetType ()}", spriteObj);
			}
		}

		public void Dispose ()
		{
			//bitmap?.Dispose ();
			UnityEngine.Object.Destroy (spriteObj);
		}

		private Bitmap bitmap;
		private Point<short> pivot = new Point<short> ();
		private Point<short> dimensions = new Point<short> ();

		public void erase ()
		{
			if (spriteRenderer != null)
			{
				spriteRenderer.enabled = false;
			}
		}

		public void draw ()
		{
		}

		public void draw (DrawArgument args)
		{
			if (bitmap == null) return;
			if (spriteRenderer == null)
			{
				spriteObj = new GameObject ();
				if(MapleStory.Instance.AddToParent)
					AddToParent (spriteObj, fullPath);
				spriteRenderer = spriteObj.AddComponent<SpriteRenderer> ();
			}

			if (spriteRenderer != null)
			{
				spriteRenderer.enabled = true;
				//Debug.Log ($"{fullPath} {origin}", spriteRenderer.gameObject);
				if (sprite == null)
				{
					//Debug.Log ($"fullPath:{fullPath}\t Width:{bitmap.Width}\t Height:{bitmap.Height}\t dimensions:{dimensions}");
					sprite = TextureAndSpriteUtil.BitmapToSprite (bitmap, pivot, dimensions);
				}

				spriteRenderer.gameObject.name = fullPath;
				spriteRenderer.sprite = sprite;
				spriteRenderer.sortingLayerName = args.sortingLayer.ToString ();
				spriteRenderer.sortingOrder = args.orderInLayer;
				setScale (new Vector3 (args.get_xscale (), args.get_yscale (), 1));
				setPos (new Vector3 (args.get_Pos ().x (), -args.get_Pos ().y (), 0));
			}

			/*textureRange = new Rectangle<short> ((short)(args.get_Pos ().x () - pivot.x ()), (short)(args.get_Pos ().y () - pivot.y ()), dimensions.x (), dimensions.y ());
			textureRect = new Rect (args.get_Pos ().x () - pivot.x (), args.get_Pos ().y () - pivot.y (), dimensions.x (), dimensions.y ());
			if (bitmap == null) return;
			if (mainTexture == null)
			{
				mainTexture = TextureAndSpriteUtil.BitmapToUnityTexture2d (bitmap, dimensions);
			}

			if (overlaps ())
				DrawTextureToTarget ();*/
		}

		private Texture2D mainTexture;
		public RenderTexture target => MapleStory.Instance.target;
		public UnityEngine.Color clearColor = UnityEngine.Color.magenta;

		CommandBuffer clearBuffer = new CommandBuffer () {name = "Clear Buffer"};

		public void DrawTextureToTarget ()
		{
			Graphics.SetRenderTarget (target);
			//clearBuffer.Clear ();
			//clearBuffer.ClearRenderTarget (true, true, clearColor);
			//Graphics.ExecuteCommandBuffer (clearBuffer);

			GL.PushMatrix ();
			GL.LoadPixelMatrix (0, target.width, target.height, 0);
			//GL.LoadPixelMatrix(0, target.width, 0, target.height);
			Graphics.DrawTexture (textureRelativeToCamera, mainTexture);
			GL.PopMatrix ();
		}

		private Rect textureRelativeToCamera => new Rect (textureRect.x - cameraRect.x, textureRect.y - cameraRect.y, textureRect.width, textureRect.height);
		private const short camera_left = 0;
		private const short camera_top = 0;
		private const short camera_right = 800;
		private const short camera_bottom = 600;
		Rectangle<short> cameraRange = new Rectangle<short> (camera_left, camera_right, camera_top, camera_bottom);
		Rect cameraRect = new Rect (camera_left, camera_top, camera_right, camera_bottom);
		private Rectangle<short> textureRange;
		private Rect textureRect;

		private bool overlaps ()
		{
			return cameraRange.overlaps (textureRange);
		}

		private bool contains ()
		{
			return cameraRange.contains (textureRange);
		}

		public void draw (DrawArgument args, Range<short> vertical)
		{
			/*if (!is_valid())
				return;

			GraphicsGL::get().draw(
				bitmap,
				args.get_rectangle(origin, dimensions),
				vertical,
				args.get_color(),
				args.get_angle()
			);*/
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


		public void shift (Point<short> amount)
		{
			pivot = pivot - amount;
		}

		public bool is_valid ()
		{
			return bitmap != null; /*bitmap.id() > 0;*/
		}

		public short width ()
		{
			return dimensions.x ();
		}

		public short height ()
		{
			return dimensions.y ();
		}

		public Point<short> get_origin ()
		{
			return pivot;
		}

		public Point<short> get_dimensions ()
		{
			return dimensions;
		}

		private void AddToParent (GameObject gObj, string path)
		{
			if (gObj == null || string.IsNullOrEmpty (path)) return;
			if (MapleStory.Instance.Map_Parent == null) return;
			if (MapleStory.Instance.Character_Parent == null) return;
			if (MapleStory.Instance.Mob_Parent == null) return;
			if (path.Contains ("map"))
			{
				gObj.SetParent (MapleStory.Instance.Map_Parent.transform);
			}
			else if (path.Contains ("character"))
			{
				gObj.SetParent (MapleStory.Instance.Character_Parent.transform);
			}
			else if (path.Contains ("mob"))
			{
				gObj.SetParent (MapleStory.Instance.Mob_Parent.transform);
			}
		}
	}
}


#if USE_NX
#endif