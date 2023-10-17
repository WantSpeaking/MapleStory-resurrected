﻿#define USE_NX




#if USE_NX
#endif

using System.Diagnostics;
using ms.Helper;
using MapleLib.WzLib;
using System;
using provider;

namespace ms
{
	// Represents a tile on a map
	public class Tile:IDisposable
	{
		private int orderInLayer;

		private Texture texture = new Texture ();

		private Point_short pos = new Point_short ();

		private int z;
		private string path;

        public Tile (WzImageProperty node_100000000img_0_Tile_0, string ts)
		{
			path = node_100000000img_0_Tile_0.FullPath;

            string tileName = "";
			
			int.TryParse (node_100000000img_0_Tile_0.Name, out orderInLayer);
			WzObject node_Tile_allblackTileimg_bsc_0 = wz.wzFile_map["Tile"][ts][node_100000000img_0_Tile_0["u"].ToString ()][node_100000000img_0_Tile_0["no"].ToString ()];
			texture = new Texture (node_Tile_allblackTileimg_bsc_0);
			pos = new Point_short (node_100000000img_0_Tile_0["x"], node_100000000img_0_Tile_0["y"]);

			z = node_100000000img_0_Tile_0["z"];
            if (z == 0)
            {
                z = orderInLayer;
            }
        }
        public Tile(MapleData node_100000000img_0_Tile_0, string ts)
        {
            path = node_100000000img_0_Tile_0.Name;

            string tileName = "";

            int.TryParse(node_100000000img_0_Tile_0.Name, out orderInLayer);
            WzObject node_Tile_allblackTileimg_bsc_0 = wz.wzFile_map["Tile"][ts][node_100000000img_0_Tile_0["u"].ToString()][node_100000000img_0_Tile_0["no"].ToString()];
            texture = new Texture(node_Tile_allblackTileimg_bsc_0);
            pos = new Point_short(node_100000000img_0_Tile_0["x"], node_100000000img_0_Tile_0["y"]);

            z = node_100000000img_0_Tile_0["z"];
            if (z == 0)
            {
                z = orderInLayer;
            }
        }
        public void Dispose()
        {
            texture?.Dispose ();
        }

        public void draw (Point_short viewpos, int layerId)
		{
			Point_short tempPoint = new Point_short ((short)(pos.x () + viewpos.x ()), (short)(pos.y () + viewpos.y ()));
			texture.draw (new DrawArgument (tempPoint));
		}

		public int getz ()
		{
			return z;
		}
        public string getPath()
        {
            return path;
        }
        public Texture get_Texture()=> texture;
		public Point_short get_Point() => pos;
    }
}