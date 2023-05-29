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
using ms_Unity;

namespace ms
{
	// Represents a single image loaded from a of game data
	public class Texture : IDisposable
	{
		public string GUIDString;
		
		private GameObject spriteObj;

		private SpriteRenderer spriteRenderer;

		public UnityEngine.Sprite sprite;

		public string fullPath = string.Empty;

		private byte[] textureData;

		private Bitmap bitmap;

		private short real_X;

		private short real_Y;

		public int textureWidth;

		public int textureHight;

		public byte[] data;

		private WzCanvasProperty canvasProperty;

		private Point_short pivot = new Point_short ();

		private Point_short dimensions = new Point_short ();

		private DrawArgument _drawArgument;

		private Texture2D mainTexture;

		public UnityEngine.LayerMask layerMask = LayerMask.NameToLayer ("Default");
		public UnityEngine.Color clearColor = UnityEngine.Color.magenta;

		private CommandBuffer clearBuffer = new CommandBuffer
		{
			name = "Clear Buffer"
		};

		private const short camera_left = 0;

		private const short camera_top = 0;

		private const short camera_right = 800;

		private const short camera_bottom = 600;

		private Rectangle_short cameraRange = new Rectangle_short (0, 800, 0, 600);

		private Rect cameraRect = new Rect (0f, 0f, 800f, 600f);

		private Rectangle_short textureRange;

		private Rect textureRect;

		private WzObject cache_src { get; set; }

		public Texture2D texture2D { get; set; }
		public FairyGUI.NTexture nTexture { get; set; }

		public bool hasBeenDrew = false;
		public RenderTexture target => SingletonMono<MapleStory>.Instance.target;

		public bool isSprite;

		public bool isDontDestoryOnLoad = false;
		public Texture SetIsDontDestoryOnLoad(bool b)
		{
			isDontDestoryOnLoad = b;

			return this;
        }

        private Rect textureRelativeToCamera => new Rect (textureRect.x - cameraRect.x, textureRect.y - cameraRect.y, textureRect.width, textureRect.height);

		public DrawObject DrawObject { get; set; }

        public Texture ()
		{
			GUIDString = Guid.NewGuid ().ToString();
		}

		public Texture (WzObject src)
		{
			Init (src);
		}

		public Texture (WzObject src, string layerMaskName)
		{
			Init (src);
			layerMask = LayerMask.NameToLayer (layerMaskName);
		}

		public Texture (ms.Texture srcTexture)
		{
			Init (srcTexture?.cache_src);
		}

		public void Init (WzObject src)
		{
            GUIDString = Guid.NewGuid().ToString();
            cache_src = src;

			/*if (cache_src == null)
			{
                pivot = Point_short.zero;
                dimensions = Point_short.zero;
            }
           else
			{
				*//*pivot = cache_src["origin"]?.GetPoint().ToMSPoint() ?? Point_short.zero;
                bitmap = cache_src.GetBitmapConsideringLink();*//*

				var point = cache_src.GetBitmapWidthHeightConsideringLink();
				pivot = cache_src["origin"]?.GetPoint().ToMSPoint() ?? Point_short.zero;
				dimensions = new Point_short((short)point.X, (short)point.Y);
				AppDebug.Log($"point:{point}");
			}

            TestURPBatcher.Instance.msTexInitTasks.Enqueue(new MsTexInitTask(this));*/

			InitCostTIme();
        }

		public void InitCostTIme()
		{
            
            if (cache_src != null && cache_src.IsTexture())
            {
                fullPath = cache_src.FullPath;
                pivot = cache_src["origin"]?.GetPoint().ToMSPoint() ?? Point_short.zero;
                bitmap = cache_src.GetBitmapConsideringLink();
                if (bitmap == null)
                {
                    Debug.Log(cache_src.FullPath + " bitmap is null");
                }
                textureData = bitmap.RawBytes;
                WzCanvasProperty canvasProperty = cache_src as WzCanvasProperty;
                if (canvasProperty != null)
                {
                }
                this.canvasProperty = cache_src as WzCanvasProperty;
                dimensions = new Point_short((short)bitmap.Width, (short)bitmap.Height);

                if (cache_src.FullPath.Contains("Map.wz\\Tile"))
                {
                    //tileName = $"{ts}-{node_100000000img_0_Tile_0["u"].ToString ()}-{node_100000000img_0_Tile_0["no"]}";
                    string tileName = cache_src.FullPath.Replace("Map.wz\\Tile\\", "").Replace("\\", "-");
                    MapleStory.Instance.tileSprites.TryGetValue(tileName, out sprite);
                    if (sprite != null)
                    {
                        texture2D = sprite.texture;
                        isSprite = true;
                    }
                    //AppDebug.Log ($"{tileName}\t {sprite}");
                }

                if (texture2D == null)
                {
                    texture2D = TextureAndSpriteUtil.PngDataToTexture2D(textureData, bitmap, pivot, dimensions);
                }
                nTexture = new FairyGUI.NTexture(texture2D);
            }
        }
		public void Dispose ()
		{
			TestURPBatcher.Instance.UnSpawn (this);
		}

		public void erase ()
		{
			if (!(spriteRenderer != null))
			{
			}
		}

		public void draw ()
		{
		}

		public void draw (DrawArgument args)
		{
			/*if (args.drawOnce && hasBeenDrew)
			{
				return;
			}

			hasBeenDrew = true;*/
			
			if (bitmap != null)
			{
				Vector3 position = new Vector3 (args.getpos ().x () + (args.FlipX ? -1 : 1) * (bitmap.Width / 2 - pivot.x ()), -args.getpos ().y () + (-bitmap.Height / 2 + pivot.y ()), SingletonMono<GameUtil>.Instance.DrawOrder);
				/*Vector3 position = new Vector3 (args.getpos ().x () + bitmap.Width / 2 - pivot.x (), -args.getpos ().y () - bitmap.Height / 2 + pivot.y (), Singleton<GameUtil>.Instance.DrawOrder);*/
				if (fullPath == "Ui-new.wz\\Login.img\\Title\\BtNew\\normal\\0")
				{
				}
				Vector3 localScale = new Vector3 (args.get_xscale (), -args.get_yscale (), 1f);
				//Vector3 localScale = new Vector3 (args.get_xscale () * (args.getstretch ().x ()==0?1:args.getstretch ().x ()), -args.get_yscale ()* (args.getstretch ().y ()==0?1:args.getstretch ().y ()), 1f);
				if (args.isDontDestoryOnLoad)
				{
                    isDontDestoryOnLoad = args.isDontDestoryOnLoad;

                }
                SingletonMono<TestURPBatcher>.Instance.TryDraw (this, bitmap, position, localScale, args.DrawParent);
			}
		}

		private float format_Scale_X (float X, int pngFormat)
		{
			float result = 1f;
			switch (pngFormat)
			{
				case 1:
					result = 1f;
					break;
				case 2:
				case 3:
				case 1026:
				case 2050:
					result = 1f;
					break;
				case 513:
				case 517:
					result = 1f;
					break;
			}
			return result * X;
		}

		private float format_Scale_Y (float X, int pngFormat)
		{
			float result = -1f;
			switch (pngFormat)
			{
				case 1:
					result = -1f;
					break;
				case 2:
				case 3:
				case 1026:
				case 2050:
					result = -1f;
					break;
				case 513:
				case 517:
					result = -1f;
					break;
			}
			return result * X;
		}

		private SpriteRenderer SpriteRendererCreator ()
		{
			GameObject obj = new GameObject (fullPath);
			SpriteRenderer renderer = obj.AddComponent<SpriteRenderer> ();
			renderer.flipY = true;
			if (sprite == null)
			{
				sprite = TextureAndSpriteUtil.PngDataToSprite (textureData, bitmap, pivot, dimensions);
				renderer.sprite = sprite;
			}
			return renderer;
		}

		public void DrawTextureToTarget ()
		{
			Graphics.SetRenderTarget (target);
			GL.PushMatrix ();
			GL.LoadPixelMatrix (0f, target.width, target.height, 0f);
			Graphics.DrawTexture (textureRelativeToCamera, mainTexture);
			GL.PopMatrix ();
		}

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
		}

		private void setPos (Vector3 pos)
		{
			GameObject gameObject = spriteRenderer?.gameObject;
			if ((object)gameObject != null)
			{
				gameObject.transform.position = pos;
			}
		}

		private void setScale (Vector3 pos)
		{
			GameObject gameObject = spriteRenderer?.gameObject;
			if ((object)gameObject != null)
			{
				gameObject.transform.localScale = pos;
			}
		}

		public void shift (Point_short amount)
		{
			pivot -= amount;
		}

		public bool is_valid ()
		{
			return textureData != null;
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
			if (!(gObj == null) && !string.IsNullOrEmpty (path) && !(SingletonMono<MapleStory>.Instance.Map_Parent == null) && !(SingletonMono<MapleStory>.Instance.Character_Parent == null) && !(SingletonMono<MapleStory>.Instance.Mob_Parent == null))
			{
				if (path.Contains ("map"))
				{
					gObj.SetParent (SingletonMono<MapleStory>.Instance.Map_Parent.transform);
				}
				else if (path.Contains ("character"))
				{
					gObj.SetParent (SingletonMono<MapleStory>.Instance.Character_Parent.transform);
				}
				else if (path.Contains ("mob"))
				{
					gObj.SetParent (SingletonMono<MapleStory>.Instance.Mob_Parent.transform);
				}
				else if (path.Contains ("effect"))
				{
					gObj.SetParent (SingletonMono<MapleStory>.Instance.Effect_Parent.transform);
				}
			}
		}

		public override int GetHashCode ()
		{
			int hashCode = 339610899;
			hashCode *= -1521134295;
			return base.GetHashCode () + hashCode;
		}
	}
}


#if USE_NX
#endif