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
using System.Collections.Generic;
using System.Linq;

namespace ms
{
    // Represents a single image loaded from a of game data
    public class Texture : IDisposable
    {
        public string GUIDString;



        public UnityEngine.Sprite sprite;

        public string fullPath = string.Empty;

        private Bitmap bitmap;

        private Point_short pivot = new Point_short();

        private Point_short dimensions = new Point_short();

        public UnityEngine.LayerMask layerMask = LayerMask.NameToLayer("Default");


        private WzObject cache_src { get; set; }

        public Texture2D texture2D { get; set; }
        public FairyGUI.NTexture nTexture { get; set; }

        public bool isSprite;

        public bool isDontDestoryOnLoad = false;

        public DrawObject DrawObject { get; set; }

        public void DestoryUnityObject()
        {
            UnityEngine.Object.DestroyImmediate(sprite,true);
            UnityEngine.Object.DestroyImmediate(texture2D,true);


            this.sprite = null;
            this.fullPath = null;
            this.bitmap = null;
            this.pivot = null;
            this.dimensions = null;
            this.cache_src = null;
            this.texture2D = null;
            this.nTexture = null;
            this.DrawObject = null;
        }

        public void Dispose()
        {


            TestURPBatcher.Instance.UnSpawn(this);

        }
        private static List<string> dontDestoryList = new List<string>() { "Character", "BasicEff.img", "icon", "skill/1111002" , "skill/1120003", "4111002/special" };
        public static Dictionary<string, Texture> cache_Texture = new Dictionary<string, Texture>();
        private static List<string> destoryList = new List<string>();
        public static void DestoryUnityObjects()
        {
            destoryList.Clear();

            foreach (var pair in cache_Texture)
            {
                if (pair.Value.fullPath == null) continue;
                if (dontDestoryList.Where(k => pair.Value.fullPath.Contains(k)).Any()) continue;

                pair.Value?.DestoryUnityObject();
                destoryList.Add(pair.Key);

                /*foreach (var keyWord in dontDestoryList)
                {

                }
                if (!(pair.Value.fullPath?.Contains("Character") ?? false))
                {
                    pair.Value?.DestoryUnityObject();
                    destoryList.Add(pair.Key);
                }*/
            }

            foreach (var item in destoryList)
            {
                cache_Texture.Remove(item);
            }
        }

        public Texture(ms.Texture srcTexture)
        {
            Init(srcTexture);

            //Init(srcTexture?.cache_src);
        }

        public Texture()
        {
            GUIDString = Guid.NewGuid().ToString();
        }

        public Texture(WzObject src)
        {
            Init(src);
        }

        public Texture(WzObject src, string layerMaskName)
        {
            Init(src);
            layerMask = LayerMask.NameToLayer(layerMaskName);
        }


        public void Init(ms.Texture srcTexture)
        {
            this.sprite = srcTexture.sprite;
            this.fullPath = srcTexture.fullPath;
            this.bitmap = srcTexture.bitmap;
            //this.pivot = srcTexture.pivot;
            this.pivot = srcTexture?.cache_src?["origin"]?.GetPoint().ToMSPoint() ?? Point_short.zero;
            this.dimensions = srcTexture.dimensions;
            this.layerMask = srcTexture.layerMask;
            this.cache_src = srcTexture.cache_src;
            this.texture2D = srcTexture.texture2D;
            this.nTexture = srcTexture.nTexture;
            this.isSprite = srcTexture.isSprite;
            this.isDontDestoryOnLoad = srcTexture.isDontDestoryOnLoad;
            this.DrawObject = srcTexture.DrawObject;
        }

        public void Init(WzObject src)
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

            if (cache_src == null) return;

            if (cache_Texture.TryGetValue(cache_src.FullPath, out var cacheTexture))
            {
                Init(cacheTexture);
            }
            else
            {
                InitCostTIme();
                cache_Texture.TryAdd(fullPath, this);
            }

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
                    texture2D = TextureAndSpriteUtil.PngDataToTexture2D(bitmap.RawBytes, bitmap, pivot, dimensions);
                }
                nTexture = new FairyGUI.NTexture(texture2D);
            }
        }


        public void erase()
        {

        }


        public void draw(DrawArgument args)
        {
            /*if (args.drawOnce && hasBeenDrew)
			{
				return;
			}

			hasBeenDrew = true;*/

            if (bitmap != null)
            {
                Vector3 position = new Vector3(args.getpos().x() + (args.FlipX ? -1 : 1) * (bitmap.Width / 2 - pivot.x()), -args.getpos().y() + (-bitmap.Height / 2 + pivot.y()), SingletonMono<GameUtil>.Instance.DrawOrder);
                /*Vector3 position = new Vector3 (args.getpos ().x () + bitmap.Width / 2 - pivot.x (), -args.getpos ().y () - bitmap.Height / 2 + pivot.y (), Singleton<GameUtil>.Instance.DrawOrder);*/
                if (fullPath == "Ui-new.wz\\Login.img\\Title\\BtNew\\normal\\0")
                {
                }
                Vector3 localScale = new Vector3(args.get_xscale(), -args.get_yscale(), 1f);
                //Vector3 localScale = new Vector3 (args.get_xscale () * (args.getstretch ().x ()==0?1:args.getstretch ().x ()), -args.get_yscale ()* (args.getstretch ().y ()==0?1:args.getstretch ().y ()), 1f);
                if (args.isDontDestoryOnLoad)
                {
                    isDontDestoryOnLoad = args.isDontDestoryOnLoad;

                }
                SingletonMono<TestURPBatcher>.Instance.TryDraw(this, bitmap, position, localScale, args.DrawParent);
            }
        }


        public void draw(DrawArgument args, Range_short vertical)
        {
        }

        public void shift(Point_short amount)
        {
            pivot -= amount;
        }

        public bool is_valid()
        {
            return dimensions != null;
        }

        public short width()
        {
            return dimensions.x();
        }

        public short height()
        {
            return dimensions.y();
        }

        public Point_short get_origin()
        {
            return pivot;
        }

        public Point_short get_dimensions()
        {
            return dimensions;
        }


        public override int GetHashCode()
        {
            int hashCode = 339610899;
            hashCode *= -1521134295;
            return base.GetHashCode() + hashCode;
        }
    }
}
