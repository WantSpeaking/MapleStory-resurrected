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
using Utility.PoolSystem;
using Bitmap = MapleLib.WzLib.Bitmap;
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
		private WzObject cache_src { get; set; }

		private byte[] textureData;
		private Bitmap bitmap;
		public UnityEngine.Texture2D texture2D { get; set; }
		//private PngInfo pngInfo;

		private short real_X;
		private short real_Y;

		public Texture ()
		{
		}

		public Texture (WzObject src)
		{
			Init (src);
		}

		public Texture (Texture srcTexture)
		{
			Init (srcTexture?.cache_src);
		}

		private void Init (WzObject src)
		{
			cache_src = src;
			if (src?.IsTexture () ?? false)
			{
				fullPath = src.FullPath;
				pivot = src["origin"]?.GetPoint ().ToMSPoint () ?? Point_short.zero;

				bitmap = src.GetBitmapConsideringLink ();
				if (bitmap == null)
				{
					Debug.Log ($"{src.FullPath} bitmap is null");
				}
				textureData = bitmap.RawBytes;
				//textureData = src.GetPngData (out pngInfo);
				if (src is WzCanvasProperty canvasProperty)
				{
					//Debug.Log ($"pixelData:{canvasProperty.imageProp?.decodedData?.ToDebugLog ()}");
				}

				this.canvasProperty = src as WzCanvasProperty;
				dimensions = new Point_short ((short)(bitmap.Width), (short)(bitmap.Height));

				texture2D = TextureAndSpriteUtil.PngDataToTexture2D (textureData, bitmap, pivot, dimensions);
				//GraphicsGL.get().addbitmap(bitmap);todo render unity
				//Debug.Log ($"{src?.FullPath} \t {src?.GetType ()}", spriteObj);
			}
		}

		public void Dispose ()
		{
			//bitmap?.Dispose ();
			//UnityEngine.Object.Destroy (spriteObj);
		}

		public int textureWidth;
		public int textureHight;
		public byte[] data;
		private WzCanvasProperty canvasProperty;
		//private Bitmap bitmap;
		private Point_short pivot = new Point_short ();
		private Point_short dimensions = new Point_short ();

		public void erase ()
		{
			if (spriteRenderer != null)
			{
				//spriteRenderer.enabled = false;
			}
		}

		public void draw ()
		{
		}

		private DrawArgument _drawArgument;

		public void draw (DrawArgument args)
		{
			{
				/*if (bitmap == null) return;
				if (spriteRenderer == null)
				{
					spriteObj = new GameObject ();
					if (MapleStory.Instance.AddToParent)
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
				}*/
			}

			{
				/*textureRange = new Rectangle_short ((short)(args.get_Pos ().x () - pivot.x ()), (short)(args.get_Pos ().y () - pivot.y ()), dimensions.x (), dimensions.y ());
				textureRect = new Rect (args.get_Pos ().x () - pivot.x (), args.get_Pos ().y () - pivot.y (), dimensions.x (), dimensions.y ());
				if (bitmap == null) return;
				if (mainTexture == null)
				{
					mainTexture = TextureAndSpriteUtil.BitmapToUnityTexture2d (bitmap, dimensions);
				}

				if (overlaps ())
					DrawTextureToTarget ();*/
			}

			{
				/*if (bitmap != null)
				{
					if (mainTexture == null)
					{
						mainTexture = TextureAndSpriteUtil.BitmapToUnityTexture2d (bitmap, dimensions);
					}

					var batchItem = new BatchItem ();
					batchItem.posX = args.get_Pos ().x ();
					batchItem.posY = args.get_Pos ().y ();
					batchItem.pivotX = pivot.x ();
					batchItem.pivotY = pivot.y ();
					batchItem.width = bitmap?.Width??0;
					batchItem.height = bitmap?.Height??0;
					batchItem.data = textureData;
					batchItem.bmp = bitmap;
					batchItem.texture = mainTexture;
					SpriteBatch.Instance.Add (batchItem);
				}*/
			}

			{
				/*if (spriteRenderer == null)
				{
				    spriteObj = new GameObject();
				    if (MapleStory.Instance.AddToParent)
				        AddToParent(spriteObj, fullPath);
				    spriteRenderer = spriteObj.AddComponent<SpriteRenderer>();
				    spriteRenderer.flipY = true;
					spriteRenderer.gameObject.name = fullPath;
				}

				if (spriteRenderer != null)
				{
				    
					//Debug.Log ($"{fullPath} {origin}", spriteRenderer.gameObject);
					if (sprite == null)
				    {
				        //Debug.Log ($"fullPath:{fullPath}\t Width:{bitmap.Width}\t Height:{bitmap.Height}\t dimensions:{dimensions}");
				        sprite = TextureAndSpriteUtil.PngDataToSprite(textureData, pngFormat, pivot, dimensions);
						spriteRenderer.sprite = sprite;

					}
					spriteRenderer.enabled = true;
					if (spriteRenderer.gameObject.name.Contains(@"character.wz\00002003.img\walk2\2\body"))
					{
						Debug.Log($"Spawn spriteRenderer.sprite:{spriteRenderer.sprite}\t TextureHashCode:{GetHashCode()}");
					}
					//spriteRenderer.sortingLayerName = args.sortingLayer.ToString();
					spriteRenderer.sortingOrder = SpriteBatch.Instance.DrawOrder++;
					setScale(new Vector3(args.get_xscale(), args.get_yscale(), 1));
				    setPos(new Vector3(args.get_Pos().x(), -args.get_Pos().y(), 0));

					SpriteBatch.spriteRenderQueue.Enqueue(spriteRenderer);
				}*/
			}

			{
				/*_drawArgument = args;
				if (sprite == null)
				{
					//Debug.Log ($"fullPath:{fullPath}\t Width:{bitmap.Width}\t Height:{bitmap.Height}\t dimensions:{dimensions}");
					sprite = TextureAndSpriteUtil.PngDataToSprite (textureData, pngFormat, pivot, dimensions);
				}

				PoolManager.Spawn (MapleStory.Instance.prefab_SpriteDrawer, InitSpawn);*/
			}

			{
				/*if (sprite == null)
				{
					//Debug.Log ($"fullPath:{fullPath}\t Width:{bitmap.Width}\t Height:{bitmap.Height}\t dimensions:{dimensions}");
					sprite = TextureAndSpriteUtil.PngDataToSprite (textureData, pngFormat, pivot, dimensions);
				}

				var batchItem = new BatchItem ();
				batchItem.posX = args.get_Pos ().x ();
				batchItem.posY = args.get_Pos ().y ();
				batchItem.pivotX = pivot.x ();
				batchItem.pivotY = pivot.y ();
				batchItem.width = bitmap?.Width ?? 0;
				batchItem.height = bitmap?.Height ?? 0;
				batchItem.data = textureData;
				batchItem.bmp = bitmap;
				batchItem.texture = mainTexture;
				batchItem.color = args.get_color ();
				batchItem.sprite = sprite;
				SpriteBatch.Instance.Add (batchItem);*/
			}

			/*{
				var renderer = SpriteBatch.Instance.TryGetSpriteRenderer (this, SpriteRendererCreator);
				*//*if (spriteRenderer == null)
				{
					spriteObj = new GameObject ();
					if (MapleStory.Instance.AddToParent)
						AddToParent (spriteObj, fullPath);
					spriteRenderer = spriteObj.AddComponent<SpriteRenderer> ();
					spriteRenderer.flipY = true;
					spriteRenderer.gameObject.name = fullPath;
				}*//*

				if (renderer != null)
				{
					renderer.enabled = true;

					//renderer.sortingOrder = SpriteBatch.Instance.DrawOrder++;
					renderer.gameObject.transform.position = new Vector3 (args.get_Pos ().x (), -args.get_Pos ().y (), 0);
					renderer.gameObject.transform.localScale = new Vector3 (args.get_xscale (), args.get_yscale (), 1);
					SpriteBatch.spriteRenderQueue.Enqueue (renderer);


					*//*if (spriteRenderer.gameObject.name.Contains (@"character.wz\00002003.img\walk2\2\body"))
			{
				Debug.Log ($"Spawn spriteRenderer.sprite:{spriteRenderer.sprite}\t TextureHashCode:{GetHashCode ()}");
			}*//*

					//spriteRenderer.sortingLayerName = args.sortingLayer.ToString();
				}
			}*/

			if (bitmap is not null)
			{
				var position = new Vector3 (args.getpos ().x () + bitmap.Width / 2 - pivot.x (), -(args.getpos ().y ()) - bitmap.Height / 2 + pivot.y (), GameUtil.Instance.DrawOrder);
				if (fullPath == "Ui-new.wz\\Login.img\\Title\\BtNew\\normal\\0")
				{
					//Debug.Log ("");//288,-358
				}
				//var position = new Vector3 (args.getpos ().x (), -args.getpos ().y (), GameUtil.Instance.DrawOrder);
				var localScale = new Vector3 (format_Scale_X (args.get_xscale (), bitmap.format), format_Scale_Y (args.get_yscale (), bitmap.format), 1);
				TestURPBatcher.Instance.TryDraw (this, bitmap, position, localScale);

			}
		}

		/// <summary>
		/// is x flip? base on format
		/// </summary>
		/// <param name="X"></param>
		/// <param name="pngFormat"></param>
		/// <returns></returns>
		float format_Scale_X (float X, int pngFormat)
		{
			float result = 1;
			switch (pngFormat)
			{
				case 1:
					result = 1;
					break;
				case 2:
				case 3:
				case 1026:
				case 2050:
					result = 1;
					break;
				case 513:
				case 517:
					result = 1;
					break;
			}

			return result * X;
		}

		/// <summary>
		/// is Y flip? base on format
		/// </summary>
		/// <param name="X"></param>
		/// <param name="pngFormat"></param>
		/// <returns></returns>
		float format_Scale_Y (float X, int pngFormat)
		{
			float result = -1;
			switch (pngFormat)
			{
				case 1:
					result = -1;
					break;
				case 2:
				case 3:
				case 1026:
				case 2050:
					result = -1;
					break;
				case 513:
				case 517:
					result = -1;
					break;
			}

			return result * X;
		}

		private SpriteRenderer SpriteRendererCreator ()
		{
			var obj = new GameObject (fullPath);
			/*if (MapleStory.Instance.AddToParent)
				AddToParent (spriteObj, fullPath);*/
			var renderer = obj.AddComponent<SpriteRenderer> ();
			renderer.flipY = true;
			if (sprite == null)
			{
				//Debug.Log ($"fullPath:{fullPath}\t Width:{bitmap.Width}\t Height:{bitmap.Height}\t dimensions:{dimensions}");
				sprite = TextureAndSpriteUtil.PngDataToSprite (textureData, bitmap, pivot, dimensions);
				renderer.sprite = sprite;
			}

			return renderer;
		}

		/*private void InitSpawn (UnityPoolItem unityPoolItem)
		{
			if (unityPoolItem.PooledRef.IsValid)
			{
				var renderer = unityPoolItem.PooledRef.GetComponent<SpriteRenderer> ().Component;
				renderer.gameObject.name = fullPath;
				renderer.sprite = sprite;
				renderer.flipY = true;
				//renderer.sortingLayerName = _drawArgument.sortingLayer.ToString ();
				renderer.sortingOrder = SpriteBatch.Instance.DrawOrder++;

				var transform = unityPoolItem.PooledRef.GetComponent<Transform> ().Component;

				transform.position = new Vector3 (_drawArgument.get_Pos ().x (), -_drawArgument.get_Pos ().y (), 0);
				transform.localScale = new Vector3 (_drawArgument.get_xscale (), _drawArgument.get_yscale (), 1);

				//Debug.Log ($"Draw fullPath: {fullPath}\t sortingLayer: {_drawArgument.sortingLayer}\t DrawOrder: {SpriteBatch.Instance.DrawOrder}");
			}

			SpriteBatch.Instance.Add (unityPoolItem);
		}*/

		private Texture2D mainTexture;
		public RenderTexture target => MapleStory.Instance.target;
		public UnityEngine.Color clearColor = UnityEngine.Color.magenta;

		CommandBuffer clearBuffer = new CommandBuffer () { name = "Clear Buffer" };

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
		Rectangle_short cameraRange = new Rectangle_short (camera_left, camera_right, camera_top, camera_bottom);
		Rect cameraRect = new Rect (camera_left, camera_top, camera_right, camera_bottom);
		private Rectangle_short textureRange;
		private Rect textureRect;

		private bool overlaps ()
		{
			return cameraRange.overlaps (textureRange);
		}

		private bool contains ()
		{
			return cameraRange.contains (textureRange);
		}

		public void draw (DrawArgument args, Range_short vertical)
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

		public void shift (Point_short amount)
		{
			pivot = pivot - amount;
		}

		public bool is_valid ()
		{
			return textureData != null; /*bitmap.id() > 0;*/
		}

		public short width ()
		{
			return dimensions.x ();
		}

		public short height ()
		{
			return dimensions.y ();
		}

		public Point_short get_origin ()
		{
			return pivot;
		}

		public Point_short get_dimensions ()
		{
			return dimensions;
		}

		private void AddToParent (GameObject gObj, string path)
		{
			if (gObj == null || string.IsNullOrEmpty (path))
				return;
			if (MapleStory.Instance.Map_Parent == null)
				return;
			if (MapleStory.Instance.Character_Parent == null)
				return;
			if (MapleStory.Instance.Mob_Parent == null)
				return;
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
			else if (path.Contains ("effect"))
			{
				gObj.SetParent (MapleStory.Instance.Effect_Parent.transform);
			}
		}

		public override int GetHashCode ()
		{
			var hashCode = 339610899;
			//hashCode = hashCode * -1521134295 + real_X.GetHashCode ();
			hashCode = hashCode * -1521134295 /*+ real_Y.GetHashCode ()*/;
			return base.GetHashCode () + hashCode;
		}
	}
}


#if USE_NX
#endif