using System;
using System.Collections.Generic;
using MapleLib.WzLib;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ms.Helper;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using MapleLib.WzLib.WzStructure;
using MapleLib.WzLib.WzStructure.Data;
using UnityEngine;
using System.Security.Policy;


namespace ms
{
	public class Background:IDisposable
	{
		public Background (WzObject src,bool front)////back1stChildMap/Map1/100000000.img/back/0
		{
			//AppDebug.Log(src.FullPath);
			VWIDTH = Constants.get ().get_viewwidth ();
			VHEIGHT = Constants.get ().get_viewheight ();
			WOFFSET = (short)(VWIDTH / 2);
			HOFFSET = (short)(VHEIGHT / 2);

			var backsrc = ms.wz.wzFile_map["Back"];//Map.wz/Back

			/*     var node_0 = backsrc[src["bS"] + ".img"]?[animated ? "ani"  "back"]?[src["no"]?.ToString()];// Map.wz/Back/grassySoil.img/ani/0
           if(node_0 == null){AppDebug.Log ($"Background() node_0 == null");}
           animation = new Animation(node_0);*/
			animated = src["ani"];//animatedMap/Map1/100000000.img/back/0/ani
			animation = new Animation( backsrc[$"{src["bS"]}.img"]?[animated ? "ani" : "back"]?[src["no"]?.ToString ()],GameUtil.layerNameDefault,front? GameUtil.Instance.sortingLayerName_Foreground:GameUtil.Instance.sortingLayerName_Background); //animationMap.wz/Back/{Map/Map1/100000000.img/back/0/bS}.img/(ani|back)/{Map/Map1/100000000.img/back/0/no}   Map.wz/Back/grassySoil.img/ani/0
			opacity = src["a"];//Map/Map1/100000000.img/back/0/a
			flipped = src["f"];//Map/Map1/100000000.img/back/0/f
			cx = src["cx"];//Map/Map1/100000000.img/back/0/cx
			cy = src["cy"]; //Map/Map1/100000000.img/back/0/cy
			rx = src["rx"]; //Map/Map1/100000000.img/back/0/rx
			ry = src["ry"]; //Map/Map1/100000000.img/back/0/ry

			moveobj.set_x (src["x"]);//Map/Map1/100000000.img/back/0/x
			moveobj.set_y (src["y"]);//Map/Map1/100000000.img/back/0/y

			type = typebyid (src["type"]);//Map/Map1/100000000.img/back/0/type

			settype (type);

			int.TryParse (src.Name, out orderInLayer);
			fullPath = src.FullPath;

			{
				//Create (src);
			}


			/*var backSrcProp = backsrc[$"{src["bS"]}.img"]?[animated ? "ani" : "back"]?[src["no"]?.ToString ()];
			int a = src["a"];
			bool front = src["front"];
			BackgroundType type1 = (BackgroundType)(int)src["type"];

			List<IDXObject> frames = new List<IDXObject> ();


			if (backSrcProp is WzCanvasProperty property) //one-frame
			{
				if (fullPath == @"map.wz/Back/midForest.img/back/0")
				{
					AppDebug.Log ("");
				}

				var dxoItem = ConvertWzCanvasProperty_To_DXObject (property);
				if (fullPath == @"map.wz/Back/midForest.img/back/0")
				{
					AppDebug.Log ("");
				}
				backgroundItem = new BackgroundItem (cx, cy, (int)rx, (int)ry, type1, a, front, dxoItem, flipped, 0);
			}
			else if (backSrcProp is WzSubProperty) // animated
			{
				WzImageProperty _frameProp;
				int i = 0;

				while ((_frameProp = WzInfoTools.GetRealProperty ((WzImageProperty)backSrcProp[(i++).ToString ()])) != null)
				{
					*//*	if (_frameProp is WzSubProperty) // issue with 867119250
						{
							frames.AddRange (LoadFrames (texturePool, _frameProp, x, y, device, ref usedProps, null));
						}
						else*//*
					{
						WzCanvasProperty frameProp;

						if (_frameProp is WzUOLProperty) // some could be UOL. Ex: 321100000 Mirror world: [Mirror World] Leafre
						{
							WzObject linkVal = ((WzUOLProperty)_frameProp).LinkValue;
							if (linkVal is WzCanvasProperty linkCanvas)
							{
								frameProp = linkCanvas;
							}
							else
								continue;
						}
						else
						{
							frameProp = (WzCanvasProperty)_frameProp;
						}

						//int delay = (int)InfoTool.GetOptionalInt (frameProp["delay"], 100);
						frames.Add (ConvertWzCanvasProperty_To_DXObject (frameProp));
						//bool bLoadedSpine = LoadSpineMapObjectItem ((WzImageProperty)frameProp.Parent, frameProp, device, spineAni);
						*//*bool bLoadedSpine = false;
						if (!bLoadedSpine)
						{
							if (frameProp.MSTag == null)
							{
								string canvasBitmapPath = frameProp.FullPath;
								Texture2D textureFromCache = ms.Stage.Instance.texturePool.GetTexture (canvasBitmapPath);
								if (textureFromCache != null)
								{
									frameProp.MSTag = textureFromCache;
								}
								else
								{
									frameProp.MSTag = frameProp.GetLinkedWzCanvasBitmap ().ToTexture2D (device);

									// add to cache
									ms.Stage.Instance.texturePool.AddTextureToPool (canvasBitmapPath, (Texture2D)frameProp.MSTag);
								}
							}
						}*//*
						//usedProps.Add (frameProp);

						*//*if (frameProp.MSTagSpine != null)
						{
							WzSpineObject spineObject = (WzSpineObject)frameProp.MSTagSpine;
							MapleLib.WzLib.PointF origin = frameProp.GetCanvasOriginPosition ();

							frames.Add (new DXSpineObject (spineObject, x, y, origin, delay));
						}
						else if (frameProp.MSTag != null)
						{
							Texture2D texture = (Texture2D)frameProp.MSTag;
							MapleLib.WzLib.PointF origin = frameProp.GetCanvasOriginPosition ();

							frames.Add (new DXObject (x - (int)origin.X, y - (int)origin.Y, texture, delay));
						}
						else
						{
							Texture2D texture = Properties.Resrcs.placeholder.ToTexture2D (device);
							MapleLib.WzLib.PointF origin = frameProp.GetCanvasOriginPosition ();

							frames.Add (new DXObject (x - (int)origin.X, y - (int)origin.Y, texture, delay));
						}*//*
					}
				}

				if (frames.Count == 0)
				{
					AppDebug.Log ($"{fullPath} frames.Count == 0");
				}
				backgroundItem = new BackgroundItem (cx, cy, (int)rx, (int)ry, type1, a, front, frames, flipped, 0);
			}*/

		}
		private int drawCount;
		public void draw (double viewx, double viewy, float alpha, int sortingLayer)
		{
			//animation.draw(new DrawArgument(new Point_short((short)x, (short)y), flipped, opacity / 255), alpha);

			double x;

			if (moveobj.hmobile())
			{
				x = moveobj.get_absolute_x(viewx, alpha);
			}
			else
			{
				double shift_x = rx * (WOFFSET - viewx) / 100 + WOFFSET;
				x = moveobj.get_absolute_x(shift_x, alpha);
			}

			double y;

			if (moveobj.vmobile())
			{
				y = moveobj.get_absolute_y(viewy, alpha);
			}
			else
			{
				double shift_y = ry * (HOFFSET - viewy) / 100 + HOFFSET;
				y = moveobj.get_absolute_y(shift_y, alpha);
			}

			if (htile > 1)
			{
				while (x > 0)
				{
					x -= cx;
				}

				while (x < -cx)
				{
					x += cx;
				}
			}

			if (vtile > 1)
			{
				while (y > 0)
				{
					y -= cy;
				}

				while (y < -cy)
				{
					y += cy;
				}
			}

			short ix = (short)Math.Round(x);
			short iy = (short)Math.Round(y);

			short tw = (short)(cx * htile);
			short th = (short)(cy * vtile);
			//var counter = 0;
			short tx;
			short ty;

			drawCount = 0;
			for (tx = 0; tx < tw; tx += cx)
			{
				for (ty = 0; ty < th; ty += cy)
				{
					drawCount++;
					Animation a;
					if (drawCount> anim_cache.Count)
					{
						a = new Animation(animation);
						anim_cache.Add(a);

                    }
					else
					{
						a = anim_cache.TryGet(drawCount - 1);
                    }

                    a.draw(new DrawArgument(new Point_short((short)(ix + tx), (short)(iy + ty)), flipped, opacity / 255), alpha);
					//counter++;
				}
			}

			/*		var drawFrame = animation.get_frame();

					int X = GameUtil.Instance.bgDrawX ;
					int Y = GameUtil.Instance.bgDrawY;

					int _cx = cx == 0 ? drawFrame.Width : cx;
					int _cy = cy == 0 ? drawFrame.Height : cy;
					var renderWidth = Width;
					var renderHeight = Height;
					int bgMoveShiftX = 0;
					int bgMoveShiftY = 0;
					var TickCount = Time.frameCount;

					switch (type)
					{
						case BackgroundType.Regular:
							Draw2D( X, Y, drawFrame);
							break;
						case BackgroundType.HorizontalTiling:
							DrawHorizontalCopies( renderWidth, X, Y, _cx, drawFrame);
							break;
						case BackgroundType.VerticalTiling:
							DrawVerticalCopies( renderHeight, X, Y, _cy, drawFrame);
							break;
						case BackgroundType.HVTiling:
							DrawHVCopies( renderWidth, renderHeight, X, Y, _cx, _cy, drawFrame);
							break;
						case BackgroundType.HorizontalMoving:
							DrawHorizontalCopies( renderWidth, X + (int)bgMoveShiftX, Y, _cx, drawFrame);
							IncreaseShiftX(_cx, TickCount);
							break;
						case BackgroundType.VerticalMoving:
							DrawVerticalCopies( renderHeight, X, Y + (int)bgMoveShiftY, _cy, drawFrame);
							IncreaseShiftY(_cy, TickCount);
							break;
						case BackgroundType.HorizontalMovingHVTiling:
							DrawHVCopies( renderWidth, renderHeight, X + (int)bgMoveShiftX, Y, _cx, _cy, drawFrame);
							IncreaseShiftX(_cx, TickCount);
							break;
						case BackgroundType.VerticalMovingHVTiling:
							DrawHVCopies( renderWidth, renderHeight, X, Y + (int)bgMoveShiftY, _cx, _cy, drawFrame);
							IncreaseShiftX(_cy, TickCount);
							break;
						default:
							break;
					}
					//AppDebug.Log ($"{fullPath} background draw {counter} tx:{tx} ty:{ty} tw:{tw} th:{th} cx:{cx} cy:{cy}");

					//backgroundItem.Draw (GameUtil.Instance.Batch, GameUtil.Instance.Game.skeletonMeshRenderer, GameUtil.Instance.Game.gameTime, (int)viewx, (int)viewy, 640, 360, null, 1280, 720, 1, RenderResolution.Res_1280x720, Environment.TickCount);*/
		}
        double bgMoveShiftX = 0;
        double bgMoveShiftY = 0;

        private int LastShiftIncreaseX = 0;
        private int LastShiftIncreaseY = 0;
        public void IncreaseShiftX(int cx, int TickCount)
        {
            bgMoveShiftX += rx * (TickCount - LastShiftIncreaseX) / 200d;
            bgMoveShiftX %= cx;
            LastShiftIncreaseX = TickCount;
        }
        public void IncreaseShiftY(int cy, int TickCount)
        {
            bgMoveShiftY += ry * (TickCount - LastShiftIncreaseY) / 200d;
            bgMoveShiftY %= cy;
            LastShiftIncreaseY = TickCount;
        }
        private void DrawHorizontalCopies(int simWidth, int x, int y,int cx, Frame frame)
        {
            int width = Width;
            Draw2D(x, y, frame);
            var copyX = x - cx;
            while (copyX + width > 0)
            {
                Draw2D(copyX, y, frame);
                copyX -= cx;
            }
            copyX = x + cx;
            while (copyX < simWidth)
            {
                Draw2D(copyX, y, frame);
                copyX += cx;
            }
        }

        private void DrawVerticalCopies(int simHeight,int x, int y,int cy, Frame frame)
        {
            int height = frame.Height;
            Draw2D(x, y, frame);
            var copyY = y - cy;
            while (copyY + height > 0)
            {
                Draw2D(x, copyY, frame);
                copyY -= cy;
            }
            copyY = y + cy;
            while (copyY < simHeight)
            {
                Draw2D( x, copyY, frame);
                copyY += cy;
            }
        }
        public void DrawHVCopies(int simWidth, int simHeight, int x, int y, int cx,int cy, Frame frame)
        {
            int width = frame.Width;
            DrawVerticalCopies(simHeight,x, y, cy, frame);
            var copyX = x - cx;
            while (copyX + width > 0)
            {
                DrawVerticalCopies(simHeight, copyX, y, cy, frame);
                copyX -= cx;
            }
            copyX = x + cx;
            while (copyX < simWidth)
            {
                DrawVerticalCopies(simHeight, copyX, y, cy, frame);
                copyX += cx;
            }
        }
        public void Draw2D(int x, int y, Frame drawFrame)
        {
            drawFrame.draw(new DrawArgument(new Point_short((short)(x), (short)(y)), flipped, opacity / 255));
        }

        public void update ()
		{
			//moveobj.move ();
			animation.update ();
		}

		private enum BackgroundType
		{
			Regular,
			HorizontalTiling,
			VerticalTiling,
			HVTiling,
			HorizontalMoving,
			VerticalMoving,
			HorizontalMovingHVTiling,
			VerticalMovingHVTiling
		}

		private static BackgroundType typebyid (int id)
		{
			if (id >= ((int)BackgroundType.Regular) && id <= (int)BackgroundType.VerticalMovingHVTiling)
			{
				return (BackgroundType)id;
			}

			AppDebug.Log ($"Unknown BackgroundType id [{id}]");

			return BackgroundType.Regular;
		}

		private void settype (BackgroundType type)
		{
			short dim_x = animation.get_dimensions ().x ();
			short dim_y = animation.get_dimensions ().y ();

			// TODO Double check for zero. Is this a WZ reading issue?
			if (cx == 0)
			{
				cx = (short)((dim_x > 0) ? dim_x : 1);
			}

			if (cy == 0)
			{
				cy = (short)((dim_y > 0) ? dim_y : 1);
			}

			htile = 1;
			vtile = 1;

			switch (type)
			{
				case BackgroundType.HorizontalTiling:
				case BackgroundType.HorizontalMoving:
					htile = (short)(VWIDTH / cx + 3);
					break;
				case BackgroundType.VerticalTiling:
				case BackgroundType.VerticalMoving:
					vtile = (short)(VHEIGHT / cy + 3);
					break;
				case BackgroundType.HVTiling:
				case BackgroundType.HorizontalMovingHVTiling:
				case BackgroundType.VerticalMovingHVTiling:
					htile = (short)(VWIDTH / cx + 3);
					vtile = (short)(VHEIGHT / cy + 3);
					break;
			}

			switch (type)
			{
				case BackgroundType.HorizontalMoving:
				case BackgroundType.HorizontalMovingHVTiling:
					moveobj.hspeed = rx / 16;
					break;
				case BackgroundType.VerticalMoving:
				case BackgroundType.VerticalMovingHVTiling:
					moveobj.vspeed = ry / 16;
					break;
			}
		}

		private short VWIDTH;
		private short VHEIGHT;
		private short WOFFSET;
		private short HOFFSET;

		private Animation animation = new Animation ();
		private List<Animation> anim_cache = new List<Animation>();
		private bool animated;
		private short cx;
		private short cy;
		private double rx;
		private double ry;
		private short htile;
		private short vtile;
		private float opacity;
		private bool flipped;
		private double x;
		private double y;
		private int orderInLayer;
		private string fullPath;
        private BackgroundType type;

        private MovingObject moveobj = new MovingObject ();
        public void Dispose()
        {
            animation?.Dispose();
			foreach (var item in anim_cache)
			{
				item?.Dispose();
            }
			anim_cache.Clear();
        }

        #region MapleStory-GM-Client

        private short CX;
		private short CY;
		private double RX;
		private double RY;
		private double Alpha;
		private double AniAlpha;
		private int ZLayer;
		private double PosX;
		private double PosY;
		private int BackType;
		int Front;
		int MoveR;
		int MoveP;
		int MoveW;
		int MoveH;
		int MoveType;
		bool MirrorX;
		bool FHasAnim;
		int FFrame = 0;
		WzObject Entry;
		WzObject AniEntry;
		WzObject ImageEntry;

		public static string BackColor;
		TBlendingEffect BlendMode;

		int PatternWidth;
		int PatternHeight;
		int Width;
		int Height;

		Point_short Origin;
		Point_short Offset;


		public void Create (WzObject src)
		{
			var Iter = src;

			var bS = Iter["bS"].ToString ();
			var _Front = Iter["front"];
			var Flip = Iter["f"];
			int Ani = Iter["ani"];
			if (bS == "grassySoil_new")

				BackColor = "#FFFF6502";
			if (bS == "YumYum")
				BackColor = "$FFF7CF3A";
			if (bS == "glacierExplorer")
				BackColor = "$FFF87B19";
			if (bS == "colossus")
				BackColor = "$FFFF4905";
			if (bS == "grandisPantheon")
				BackColor = "$FFCC6835";
			if (bS == "Wz2_Mukhyun")
				BackColor = "$FFffeedd";
			if (bS == "kain")
				BackColor = "$FF4f2200";


			var No = Iter["no"].ToString ();
			if ((bS == "critias")
				&& (Ani == 0) && (No == "2"))
				No = "1";
			if (bS == "Arks2"
				&& No == "21")
				//continue;
				if (bS == "nightDesert"
				&& (Ani == 0) && (No == "0"))

				{
					BackColor = "$FF1F0B06";
					//Continue;
				};
			if (bS == "extinction" && (Ani == 0) && (No == "0"))
			{
				BackColor = "$FF291b11";
				//Continue;
			};

			if (Ani > 1)
				Ani = 0;

			CX = Iter["cx"];
			CY = Iter["cy"];
			Alpha = Iter["a"];

			ZLayer = Iter.Name.ToInt ();

			if (bS == "")
				//continue;
				return;
			if (bS == "lebenLab" && Ani == 1)
				//continue;
				return;
			if (Ani == 0)
			{
				Entry = ms.wz.wzFile_map.GetObjectFromPath ("Back/" + bS + ".img/back/" + No);
				if (Entry == null)
				{
					return;
				}
			}
			if (Ani == 1)
			{
				AniEntry = ms.wz.wzFile_map.GetObjectFromPath ("Back/" + bS + ".img/ani/" + No);
				if (AniEntry == null)
				{
					return;
				}
			}

			PosX = Iter["x"];
			PosY = Iter["y"];
			BackType = Iter["type"];
			RX = Iter["rx"];
			RY = Iter["ry"];
			Front = _Front;
			if (Flip == 1)
				MirrorX = true;
			if (Ani == 1)
				FHasAnim = true;

			else
				FHasAnim = false;
			FFrame = 0;

			//ImageLib = Images;
			if (Ani == 0)

			{
				InfoPath = Entry.FullPath;
				ImageEntry = Entry;
				if (ImageEntry["blend"])
				{
					BlendMode = TBlendingEffect.Add;
				}
			}

			if (Ani == 1)

			{
				InfoPath = AniEntry.FullPath;
				ImageEntry = AniEntry["/0"];
			}

			MoveType = ImageEntry["moveType"];
			MoveR = ImageEntry["moveR"];
			MoveP = ImageEntry["moveP"];
			MoveW = ImageEntry["moveW"];
			MoveH = ImageEntry["moveH"];

			Width = PatternWidth;
			Height = PatternHeight;

			Origin = ImageEntry["origin"];

			switch (MirrorX)
			{
				case true:
					Offset.set_x ((short)(-Origin.x () + PatternWidth));
					break;
				case false:
					Offset.set_x (Origin.x ());
					break;
				default:
			}

			Offset.set_y (Origin.y ());

			if (CX == 0)
				Width = PatternWidth;

			else
				Width = CX;

			if (CY == 0)
				Height = PatternHeight;

			else
				Height = CY;

			var WX = SpriteEngine.WorldX;
			var WY = SpriteEngine.WorldY;

			//SetMapSize (1, 1);
			var X = -PosX - (100 + RX) / 100 * (WX + DisplaySize.X / 2) + WX;
			var Y = -PosY - (100 + RY) / 100 * (WY + DisplaySize.Y / 2) + WY;
			var Z = ZLayer;

			var AX = X;
			var AY = Y;

			var UIVersion = 1;
			if (UIVersion == 1)
			{
				if (bS == "dryRock" && No == "1")
					BackType = 1;
			}

			switch (BackType)
			{
				case 0:
					Tiled = false;
					break;
				case 1:
					Tiled = true;
					TileMode = TileMode.tmHorizontal;
					break;
				case 2:
					Tiled = true;
					TileMode = TileMode.tmVertical;
					break;
				case 3:
					Tiled = true;
					TileMode = TileMode.tmFull;
					break;
				case 4:
					Tiled = true;
					TileMode = TileMode.tmHorizontal;
					break;
				case 5:
					Tiled = true;
					TileMode = TileMode.tmVertical;
					break;
				case 6:
					Tiled = true;
					TileMode = TileMode.tmFull;
					break;
				case 7:
					Tiled = true;
					TileMode = TileMode.tmFull;
					break;
				default:
					break;
			}

		}

		//Point_short DisplaySize = new Point_short (1920, 1080);
		bool Tiled;
		TileMode TileMode;
		bool ResetPos;
		double FDelta;
		float AX;
		float AY;
		double Angle;
		int DrawMode;
		private string InfoPath;
		double FTime;

		public void DoMove (float Movecount)
		{
			var SpriteEngineVelX = 0;
			var SpriteEngineVelY = 0;
			var X = 0d;
			var Y = 0d;
			switch (BackType)
			{
				case 0:
				case 1:
				case 2:
				case 3:
					if (SpriteEngineVelX != 0)
						X = X - RX * SpriteEngineVelX / 100;
					if (SpriteEngineVelY != 0)
						Y = Y - RY * SpriteEngineVelY / 100;
					break;

				case 4:
				case 6:
					if (SpriteEngineVelX != 0)
						X = X + SpriteEngineVelX;
					if (SpriteEngineVelY != 0)
						Y = Y - RY * SpriteEngineVelY / 100;
					X = X + RX * 5 / 60;
					break;

				case 5:
				case 7:
					if (SpriteEngineVelX != 0)
						X = X - RX * SpriteEngineVelX / 100;
					if (SpriteEngineVelY != 0)
						Y = Y + SpriteEngineVelY;
					Y = Y - RY * 5 / 60;
					break;
				default:
					break;
			}

			if (ResetPos)
			{

				X = -PosX - (100 + RX) / 100 * (SpriteEngine.WorldX + DisplaySize.X / 2) + SpriteEngine.WorldX;
				Y = -PosY - (100 + RY) / 100 * (SpriteEngine.WorldY + DisplaySize.Y / 2 + TMap.OffSetY) + SpriteEngine.WorldY;

			}

			if (MoveType != 0)
			{
				FDelta = FDelta + 0.017;
				switch (MoveType)
				{
					case 1:
						if (MoveP != 0)
							X = AX + MoveW * Math.Cos (FDelta * 1000 * 2 * Math.PI / MoveP) / 60;
						else
							X = AX + MoveW * Math.Cos (FDelta) / 60;
						break;
					case 2:
						if (MoveP != 0)
							Y = Y + Y + MoveH * Math.Cos (FDelta * 2 * Math.PI * 1000 / MoveP) / 60;
						else
							Y = Y + MoveH * Math.Cos (FDelta) / 60;
						break;
					case 3:
						DrawMode = 1;
						Angle = Angle + (17 / MoveR) * Math.PI * 2;
						Offset.set_x (0);
						Offset.set_y (0);
						break;
				}

			}

			if (FHasAnim)
			{

				ImageEntry = ms.wz.wzFile_map.GetObjectFromPath (InfoPath + "/" + FFrame.ToString ());
				var Delay = ImageEntry["delay"];
				var a0 = ImageEntry["a0"];
				var a1 = ImageEntry["a1"];

				FTime = FTime + 17;
				if (FTime > Delay)
				{

					FFrame = FFrame + 1;

					/*if not WzData.ContainsKey (InfoPath + "/" + FFrame.ToString) then
					   FFrame = 0;
					FTime = 0;
					end;*/

					if (a0 != -1
						&& a1 == -1)
					{
						Alpha = ImageEntry["a0"];
						AniAlpha = a0 - (a0 - a1) * FTime / Delay;
					}

					if (FTime > 0)
						Alpha = Math.Floor (AniAlpha);

					//if ImageEntry.Get ("origin") <> nil then
					Origin = ImageEntry["origin"];
					if (MirrorX)
					{
						Offset.set_x ((short)(-Origin.x () + PatternWidth));
					}
					else
					{
						Offset.set_x (Origin.x ());
					}

					Offset.set_y (Origin.y ());

				}
			}
		}

        
    }
	public struct SpriteEngine
	{
		public const int WorldX = 0;
		public const int WorldY = 0;
	}

	public struct DisplaySize
	{
		public const int X = 1920;
		public const int Y = 1080;
	}
	public struct TMap
	{
		public const int OffSetX = 0;
		public const int OffSetY = 0;
		public const bool SaveMap = false;
	}


	public enum TileMode
	{
		tmHorizontal,
		tmVertical,
		tmLeft,
		tmRight,
		tmTop,
		tmBottom,
		tmCenter,
		tmFull,
	}




	public enum TBlendingEffect
	{
		Normal,
		Add,
	}

	#endregion
	public class MapBackgrounds:IDisposable
	{
		public MapBackgrounds (WzObject src)//back2NodePathMap/Map1/100000000.img/back
		{
			if (src is WzSubProperty subProperty)
			{
				foreach (var wzProperty in subProperty.WzProperties)//directoryap/Map1/100000000.img/back/0
				{
					bool front = wzProperty["front"]; //frontMap/Map1/100000000.img/back/0/front
					Background background = new Background (wzProperty, front);


					if (front)
					{
						foregrounds.Add (background);
					}
					else
					{
						backgrounds.Add (background);
					}
				}
			}

			//short no = 0;//todo 2 mapBackgrounds
			//var back = src[Convert.ToString(no)];//back1stChildMap/Map1/100000000.img/back/0


			/*	while (back.size() > 0)
				{
					bool front = back["front"]; //frontMap/Map1/100000000.img/back/0/front

					if (front)
					{
						foregrounds.Add(back);
					}
					else
					{
						backgrounds.Add(back);
					}

					no++;
					back=(src[Convert.ToString(no)]);
				}*/

			black = src["0"]["bS"].GetString () == "";
		}
		public MapBackgrounds ()
		{
		}

		public void drawbackgrounds (double viewx, double viewy, float alpha)
		{
			if (black)
			{
				//GraphicsGL.get().drawscreenfill(0.0f, 0.0f, 0.0f, 1.0f);
			}

			foreach (var background in backgrounds)
			{
				background.draw (viewx, viewy, alpha, -2);
			}
		}
		public void drawforegrounds (double viewx, double viewy, float alpha)
		{
			foreach (var foreground in foregrounds)
			{
				foreground.draw (viewx, viewy, alpha, -1);
			}
		}
		public void update ()
		{
			foreach (var background in backgrounds)
			{
				background.update ();
			}

			foreach (var foreground in foregrounds)
			{
				foreground.update ();
			}
		}

        public void Dispose()
        {
			foreach (var b in backgrounds)
			{
				b?.Dispose();
			}
            foreach (var b in foregrounds)
            {
                b?.Dispose();
            }
        }

        private List<Background> backgrounds = new List<Background> ();
		private List<Background> foregrounds = new List<Background> ();
		private bool black;

	

    }

}
