/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

//uncomment the line below to create a space-time tradeoff (saving RAM by wasting more CPU cycles)
#define SPACETIME

using System;
using System.IO;
using HaCreator.MapEditor;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using HaCreator.Wz;
using MapleLib.WzLib.Serialization;
using ms;


namespace HaCreator.GUI
{
    public partial class Load
    {
	    private readonly MultiBoard multiBoard;

	    public Load (MultiBoard board)
	    {
		    //InitializeComponent ();

		    
		    this.multiBoard = board;
		    
	    }


        /// <summary>
        /// Load map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadButton_Click(object sender, EventArgs e)
        {
            WzImage mapImage = null;
            int mapid = -1;
            string mapName = null, streetName = "", categoryName = "";
            WzSubProperty strMapProp = null;
            var SelectedItem = "";
            {
                if (SelectedItem == null)
                    return; // racing event

                string selectedName = SelectedItem;

                if (selectedName.StartsWith("MapLogin")) // MapLogin, MapLogin1, MapLogin2, MapLogin3
                {
                    mapImage = (WzImage)wz.WzManager["ui"][selectedName + ".img"];
                    mapName = streetName = categoryName = selectedName;
                }
                else if (SelectedItem == "CashShopPreview")
                {
                    mapImage = (WzImage)wz.WzManager["ui"]["CashShopPreview.img"];
                    mapName = streetName = categoryName = "CashShopPreview";
                }
                else
                {
                    string mapid_str = SelectedItem.Substring(0, 9);
                    int.TryParse(mapid_str, out mapid);

                    string mapcat = "Map" + mapid_str.Substring(0, 1);

                    WzDirectory directory = wz.WzManager.FindMapWz(mapcat);
                    mapImage = (WzImage)directory[mapid_str + ".img"];

                    strMapProp = WzInfoTools.GetMapStringProp(mapid_str);
                    mapName = WzInfoTools.GetMapName(strMapProp);
                    streetName = WzInfoTools.GetMapStreetName(strMapProp);
                    categoryName = WzInfoTools.GetMapCategoryName(strMapProp);
                }
            }
            MapLoader.CreateMapFromImage(mapid, mapImage, mapName, streetName, categoryName, strMapProp, multiBoard);


        }

    }
}
