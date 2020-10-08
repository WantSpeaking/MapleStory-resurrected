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

using System.Drawing;
using System.IO;
using MapleLib.WzLib;
using UnityEngine;

namespace ms
{
    // Represents a single image loaded from a of game data
    public class Texture : System.IDisposable
    {
        GameObject spriteObj;
        SpriteRenderer spriteRenderer;
        public Texture()
        {
        }
        public Texture(WzObject src)
        {
            //if (src.ObjectType == WzObjectType.Image)
            {
                var tempOrigin = src["origin"].GetPoint();
                origin = new Point<short>((short)tempOrigin.X, (short)tempOrigin.Y);

                bitmap = src.GetBitmap();

                dimensions = new Point<short>((short)bitmap.Width, (short)bitmap.Height);

                spriteObj = new GameObject();
                spriteRenderer = spriteObj.AddComponent<SpriteRenderer>();
                //GraphicsGL.get().addbitmap(bitmap);todo render unity
            }
        }
        public void Dispose()
        {
        }

        public Point<short> get_dimensions()
        {
            return dimensions;
        }

        private Bitmap bitmap;
        private Point<short> origin = new Point<short>();
        private Point<short> dimensions = new Point<short>();
        public void draw(DrawArgument args)
        {
            var ms_Rect = args.get_rectangle(origin, dimensions);
            draw();
            if (spriteRenderer != null)
            {
                spriteRenderer.gameObject.name = args.fullPath;
                //spriteRenderer.sprite = TextureToSprite(bitmap, origin, dimensions,new Rect(ms_Rect.X, ms_Rect.Y, ms_Rect.Width, ms_Rect.Height));
                spriteRenderer.sprite = TextureToSprite(bitmap, origin, dimensions, new Rect(ms_Rect.X, ms_Rect.Y, ms_Rect.Width, ms_Rect.Height), args, out var pos);
                //spriteRenderer.sortingLayerID = args.isBack ? 0 : 1;
                spriteRenderer.sortingLayerName = args.isBack ? "Back" : "Front";
                spriteRenderer.sortingOrder = args.orderInLayer;
                setPos(pos);
            }
        }

        private void setPos(Vector3 pos)
        {
           
            if (spriteRenderer?.gameObject is GameObject gameObject)
            {
                gameObject.transform.position = pos;
                Debug.LogFormat("NEW x:{0}\t y:{1}", gameObject.transform.position.x, gameObject.transform.position.y);
            }
        }
        public void draw()
        {

        }

        public static Texture2D GetTexrture2DFromPath(WzObject wzObject)
        {
            var bitMap = wzObject?.GetBitmap();
            var width = bitMap?.Width ?? 0;
            var height = bitMap?.Height ?? 0;

            Texture2D t2d = new Texture2D(width, height);
            t2d.LoadImage(ImageToByte2(bitMap));
            t2d.Apply();
            return t2d;
        }

        private static Sprite TextureToSprite(Bitmap bitmap, Point<short> origin, Point<short> dimensions, Rect rect, DrawArgument args, out Vector3 pos)
        {
            Texture2D t2d = new Texture2D(dimensions.x(), dimensions.y());
            t2d.LoadImage(ImageToByte2(bitmap));
            t2d.Apply();
            //Sprite sprite = Sprite.Create(t2d, new Rect(args.get_Pos().x(), args.get_Pos().y(), dimensions.x(), dimensions.y()), new Vector2(origin.x(), origin.y()));
            //Sprite sprite = Sprite.Create(t2d, new Rect(0, 0, dimensions.x(), dimensions.y()), new Vector2(origin.x()/100, origin.y()/100));

            var o_posX = args.get_Pos().x();
            var o_posY = args.get_Pos().y();
            var posX = o_posX + args.cx;
            var posY = o_posY + args.cy;
            var pivotX = origin.x();
            var pivotY = origin.y();
            var width = dimensions.x();
            var height = dimensions.y();
            var relativeAnchorX = (float)pivotX / width;
            var relativeAnchorY = (float)pivotY / height;
            //Debug.LogFormat("OLD x:{0}\t y:{1}", posX, posY);


            //pos = new Vector3(posX, posY, 0);
            pos = new Vector3(o_posX, -o_posY, 0);
            Debug.LogFormat("OLD x:{0}\t y:{1}", pos.x, pos.y);

            Debug.LogFormat("posX:{0}\t posY:{1}\t pivotX:{2}\t pivotY:{3}\t width:{4}\t height:{5}\t relativeAnchorX:{6}\t relativeAnchorY:{7} o_posX:{8}\t o_posY:{9}\t args.cx:{10} args.cy:{11}", posX, posY, pivotX, pivotY, width, height, relativeAnchorX, relativeAnchorY, o_posX, o_posY, args.cx, args.cy);
            Sprite sprite = Sprite.Create(t2d, new Rect(0, 0, dimensions.x(), dimensions.y()), new Vector2(relativeAnchorX, relativeAnchorY), 1);

            //Sprite sprite = Sprite.Create(t2d, new Rect(0, 0, dimensions.x(), dimensions.y()), new Vector2(args.get_Pos().x()/100, args.get_Pos().y()/100));
            return sprite;
        }

        public static byte[] ImageToByte2(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}


#if USE_NX
#endif
