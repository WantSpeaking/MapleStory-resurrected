using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ms.Helper;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using UnityEngine;

namespace ms
{
    public class Background
    {
        public Background(WzObject src)////back1stChild:Map/Map1/100000000.img/back/0
        {
            //Debug.Log(src.FullPath);
            VWIDTH = Constants.get().get_viewwidth();
            VHEIGHT = Constants.get().get_viewheight();
            WOFFSET = (short)(VWIDTH / 2);
            HOFFSET = (short)(VHEIGHT / 2);

            var backsrc = nl.nx.wzFile_map["Back"];//Map.wz/Back
            animated = src["ani"];//animated:Map/Map1/100000000.img/back/0/ani
            var node_0 = backsrc[src["bS"] + ".img"]?[animated ? "ani" : "back"]?[src["no"]?.ToString()];// Map.wz/Back/grassySoil.img/ani/0
            if(node_0 == null){Debug.LogWarning ($"Background() node_0 == null");}
            animation = new Animation(node_0);
            //animation = backsrc[src["bS"] + ".img"][animated ? "ani" : "back"][src["no"]]; //animation:Map.wz/Back/{Map/Map1/100000000.img/back/0/bS}.img/(ani|back)/{Map/Map1/100000000.img/back/0/no}   Map.wz/Back/grassySoil.img/ani/0
            opacity = src["a"];//Map/Map1/100000000.img/back/0/a
            flipped = src["f"];//Map/Map1/100000000.img/back/0/f
            cx = src["cx"];//Map/Map1/100000000.img/back/0/cx
            cy = src["cy"]; //Map/Map1/100000000.img/back/0/cy
            rx = src["rx"]; //Map/Map1/100000000.img/back/0/rx
            ry = src["ry"]; //Map/Map1/100000000.img/back/0/ry

            //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
            //ORIGINAL LINE: moveobj.set_x(src["x"]);
            x = src["x"];
            y = src["y"];
            //Debug.LogFormat("ini cx:{0}\t cy:{1}", cx, cy);

            moveobj.set_x(x);//Map/Map1/100000000.img/back/0/x
                             //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
                             //ORIGINAL LINE: moveobj.set_y(src["y"]);
            moveobj.set_y(y);//Map/Map1/100000000.img/back/0/y

            //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
            //ORIGINAL LINE: Type type = typebyid(src["type"]);
            Type type = typebyid(src["type"]);//Map/Map1/100000000.img/back/0/type

            settype(type);
            int.TryParse(src.Name, out orderInLayer);
            fullPath = src.FullPath;
        }

        //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void draw(double viewx, double viewy, float alpha) const
        public void draw(double viewx, double viewy, float alpha, int sortingLayer)
        {
            //animation.draw();
            //animation.draw(new DrawArgument(new Point<short>((short)x, (short)y), flipped, opacity / 255, cx, cy, sortingLayer, orderInLayer), alpha);
            animation.draw(new DrawArgument(new Point<short>((short)x, (short)y), flipped, opacity / 255,sortingLayer, orderInLayer), alpha);
            /*			double x;

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

						for (short tx = 0; tx < tw; tx += cx)
						{
							for (short ty = 0; ty < th; ty += cy)
							{
								animation.draw(new DrawArgument(new Point(ix + tx, iy + ty), flipped, opacity / 255), alpha);
							}  
						}*/
        }
        public void update()
        {
            //moveobj.move();
            //animation.update();
        }

        private enum Type
        {
            NORMAL,
            HTILED,
            VTILED,
            TILED,
            HMOVEA,
            VMOVEA,
            HMOVEB,
            VMOVEB
        }

        private static Type typebyid(int id)
        {
            /*if (id >= ((int)Type.NORMAL) != 0 && id <= Type.VMOVEB)
			{
				return (Type)id;
			}

			Console.Write("Unknown Background::Type id: [");
			Console.Write(id);
			Console.Write("]");
			Console.Write("\n");*/

            return Type.NORMAL;
        }

        private void settype(Type type)
        {
            short dim_x = animation.get_dimensions().x();
            short dim_y = animation.get_dimensions().y();

            // TODO: Double check for zero. Is this a WZ reading issue?
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
                case Type.HTILED:
                case Type.HMOVEA:
                    htile = (short)(VWIDTH / cx + 3);
                    break;
                case Type.VTILED:
                case Type.VMOVEA:
                    vtile = (short)(VHEIGHT / cy + 3);
                    break;
                case Type.TILED:
                case Type.HMOVEB:
                case Type.VMOVEB:
                    htile = (short)(VWIDTH / cx + 3);
                    vtile = (short)(VHEIGHT / cy + 3);
                    break;
            }

            switch (type)
            {
                case Type.HMOVEA:
                case Type.HMOVEB:
                    moveobj.hspeed = rx / 16;
                    break;
                case Type.VMOVEA:
                case Type.VMOVEB:
                    moveobj.vspeed = ry / 16;
                    break;
            }
        }

        private short VWIDTH;
        private short VHEIGHT;
        private short WOFFSET;
        private short HOFFSET;

        private Animation animation = new Animation();
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

        private MovingObject moveobj = new MovingObject();
    }

    public class MapBackgrounds
    {
        public MapBackgrounds(WzObject node_back)//back2NodePath:Map/Map1/100000000.img/back
        {
            if (node_back is WzSubProperty subProperty)
            {
                foreach (var wzProperty in subProperty.WzProperties)//directory:ap/Map1/100000000.img/back/0
                {
                    bool front = wzProperty["front"]; //front:Map/Map1/100000000.img/back/0/front
                    Background background = new Background(wzProperty);


                    if (front)
                    {
                        foregrounds.Add(background);
                    }
                    else
                    {
                        backgrounds.Add(background);
                    }
                }
            }

            //short no = 0;
            //var back = node_back[Convert.ToString(no)];//back1stChild:Map/Map1/100000000.img/back/0


            /*	while (back.size() > 0)
                {
                    bool front = back["front"].get_bool(); //front:Map/Map1/100000000.img/back/0/front

                    if (front)
                    {
                        foregrounds.Add(back);
                    }
                    else
                    {
                        backgrounds.Add(back);
                    }

                    no++;
                    //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
                    //ORIGINAL LINE: back = src[to_string(no)];
                    back=(src[Convert.ToString(no)]);
                }*/

            black = node_back["0"]["bS"].GetString() == "";
        }
        public MapBackgrounds()
        {
        }

        //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void drawbackgrounds(double viewx, double viewy, float alpha) const
        public void drawbackgrounds(double viewx, double viewy, float alpha)
        {
            if (black)
            {
                //GraphicsGL.get().drawscreenfill(0.0f, 0.0f, 0.0f, 1.0f);
            }

            foreach (var background in backgrounds)
            {
                background.draw(viewx, viewy, alpha, -2);
            }
        }
        //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void drawforegrounds(double viewx, double viewy, float alpha) const
        public void drawforegrounds(double viewx, double viewy, float alpha)
        {
            foreach (var foreground in foregrounds)
            {
                foreground.draw(viewx, viewy, alpha, -1);
            }
        }
        public void update()
        {
            foreach (var background in backgrounds)
            {
                background.update();
            }

            foreach (var foreground in foregrounds)
            {
                foreground.update();
            }
        }

        private List<Background> backgrounds = new List<Background>();
        private List<Background> foregrounds = new List<Background>();
        private bool black;
    }
}
