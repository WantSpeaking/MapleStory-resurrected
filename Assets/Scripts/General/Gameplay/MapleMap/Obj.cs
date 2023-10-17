﻿#define USE_NX




using ms;
using ms.Helper;
using MapleLib.WzLib;
using System;
using System.IO;
using provider;

namespace ms
{
	// Represents a map decoration (object) on a map
	public class Obj: IDisposable
    {
		private int orderInLayer;
		private string path;

        public Obj (WzImageProperty node_100000000img_0_obj_0)
		{
			try
			{
				path = node_100000000img_0_obj_0.FullPath;
                int.TryParse(node_100000000img_0_obj_0.Name, out orderInLayer);

				//UnityEngine.AppDebug.Log(node_100000000img_0_obj_0["oS"] + ".img" + "\t" + node_100000000img_0_obj_0["l0"] + "\t" + node_100000000img_0_obj_0["l1"] + "\t" + node_100000000img_0_obj_0["l2"]);
				var oS = $"{node_100000000img_0_obj_0["oS"]}.img";
				var l0 = node_100000000img_0_obj_0["l0"].ToString();
				var l1 = node_100000000img_0_obj_0["l1"].ToString();
				var l2 = node_100000000img_0_obj_0["l2"].ToString();

				var oS_WZO = ms.wz.wzFile_map["Obj"][oS];
				var l0_WZO = oS_WZO?[l0];
				var l1_WZO = l0_WZO?[l1];
				var l2_WZO = l1_WZO?[l2];

                animation = new Animation(l2_WZO);

                pos = new Point_short(node_100000000img_0_obj_0["x"], node_100000000img_0_obj_0["y"]);
                flip = node_100000000img_0_obj_0["f"];
                //z = (byte)(255- node_100000000img_0_obj_0["z"].GetShort ().ToByte ());//orderInLayer wz和unity正好相反
                z = (node_100000000img_0_obj_0["z"]); //orderInLayer wz和unity正好相反
                /*if (z == 0)
                {
                    z = (node_100000000img_0_obj_0["zM"]);
                }*/
            }
			catch (Exception ex)
			{
				AppDebug.LogError($"{node_100000000img_0_obj_0.FullPath} Message:{ex.Message} StackTrace:{ex.StackTrace}");
			}
		}
        public Obj(MapleData node_100000000img_0_obj_0)
        {
            try
            {
                path = node_100000000img_0_obj_0.Name;
                int.TryParse(node_100000000img_0_obj_0.Name, out orderInLayer);

                //UnityEngine.AppDebug.Log(node_100000000img_0_obj_0["oS"] + ".img" + "\t" + node_100000000img_0_obj_0["l0"] + "\t" + node_100000000img_0_obj_0["l1"] + "\t" + node_100000000img_0_obj_0["l2"]);
                var oS = $"{node_100000000img_0_obj_0["oS"]}.img";
                var l0 = node_100000000img_0_obj_0["l0"].ToString();
                var l1 = node_100000000img_0_obj_0["l1"].ToString();
                var l2 = node_100000000img_0_obj_0["l2"].ToString();

                var oS_WZO = ms.wz.wzFile_map["Obj"][oS];
                var l0_WZO = oS_WZO?[l0];
                var l1_WZO = l0_WZO?[l1];
                var l2_WZO = l1_WZO?[l2];

                animation = new Animation(l2_WZO);

                pos = new Point_short(node_100000000img_0_obj_0["x"], node_100000000img_0_obj_0["y"]);
                flip = node_100000000img_0_obj_0["f"];
                //z = (byte)(255- node_100000000img_0_obj_0["z"].GetShort ().ToByte ());//orderInLayer wz和unity正好相反
                z = (node_100000000img_0_obj_0["z"]); //orderInLayer wz和unity正好相反
                /*if (z == 0)
                {
                    z = (node_100000000img_0_obj_0["zM"]);
                }*/
            }
            catch (Exception ex)
            {
                AppDebug.LogError($"{node_100000000img_0_obj_0.Name} Message:{ex.Message} StackTrace:{ex.StackTrace}");
            }
        }
        // Update animation
        public void update ()
		{
			animation.update ();
		}
		public void ClearDisplayObj()
		{

		}
		// Draw the object at the specified position
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: void draw(Point_short viewpos, float inter) const
		public void draw (Point_short viewpos, float inter, int layerId)
		{
			//var tempPoint = new Point_short ((short)(pos.x () + viewpos.x ()), (short)(pos.y () + viewpos.y ()));
			//animation.draw (new DrawArgument (tempPoint, layerId, z), inter);
			
			//animation.draw(new DrawArgument(pos + viewpos, flip,layerId, z), inter);
			animation.draw(new DrawArgument(pos + viewpos, flip), inter);
		}

		// Return the depth of the object
		public int getz ()
		{
			return z;
		}
		public string getPath()
		{
			return path;
		}
        public void Dispose()
        {
            animation?.Dispose ();
        }

        private Animation animation = new Animation ();
		private Point_short pos = new Point_short ();
		private int z;
		private bool flip;

        public Animation get_Ani() => animation;
        public Point_short get_Point() => pos;
    }
}

#if USE_NX
#endif