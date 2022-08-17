using System.Collections.Generic;
using System.IO;
using MapleLib.WzLib.Util;
using System;
using MapleLib.WzLib;
using System.Text.RegularExpressions;

namespace MapleLib.WzLib.WzProperties
{
	/// <summary>
	/// A property that can contain sub properties and has one png image
	/// </summary>
	public partial class WzCanvasProperty : WzExtended, IPropertyContainer
	{
        /*public byte[] GetLinkedWzCanvasPngData(out PngInfo pngInfo)
		{
			string _inlink = ((WzStringProperty)this[InlinkPropertyName])?.Value; // could get nexon'd here. In case they place an _inlink that's not WzStringProperty
			string _outlink = ((WzStringProperty)this[OutlinkPropertyName])?.Value; // could get nexon'd here. In case they place an _outlink that's not WzStringProperty

			if (_inlink != null)
			{
				WzObject currentWzObj = this; // first object to work with
				while ((currentWzObj = currentWzObj.Parent) != null)
				{
					if (!(currentWzObj is WzImage))  // keep looping if its not a WzImage
						continue;

					WzImage wzImageParent = (WzImage)currentWzObj;
					WzImageProperty foundProperty = wzImageParent.GetFromPath(_inlink);
					if (foundProperty != null && foundProperty is WzImageProperty property)
					{
						return property.GetPngData(out pngInfo);
					}
				}
			}
			else if (_outlink != null)
			{
				WzObject currentWzObj = this; // first object to work with
				while ((currentWzObj = currentWzObj.Parent) != null)
				{
					if (!(currentWzObj is WzDirectory))  // keep looping if its not a WzImage
						continue;

					WzFile wzFileParent = ((WzDirectory)currentWzObj).wzFile;
					WzObject foundProperty = wzFileParent.GetObjectFromPath(_outlink);
					if (foundProperty != null && foundProperty is WzImageProperty property)
					{
						return property.GetPngData(out pngInfo);
					}
				}
			}
			return this.GetPngData(out pngInfo);
		}
		
		public override byte[] GetPngData (out PngInfo pngInfo)
		{
			pngInfo = new PngInfo();
			pngInfo.Width = imageProp.Width;
			pngInfo.height = imageProp.Height;
			pngInfo.surfaceFormat = imageProp.GetXNASurfaceFormat();
			return imageProp.GetDecodedData ();
			//return imageProp.GetRawImage(true);
			//return imageProp.GetPngData (out pngInfo);
		}*/

        /// <summary>
        /// Gets the '_inlink' WzCanvasProperty of this.
        /// 
        /// '_inlink' is not implemented as part of WzCanvasProperty as I dont want to override existing Wz structure. 
        /// It will be handled via HaRepackerMainPanel instead.
        /// </summary>
        /// <returns></returns>
        public Bitmap GetLinkedWzCanvasBitmap ()
        {
	        return GetLinkedWzImageProperty ().GetBitmap ();
        }

        /// <summary>
        /// Gets the '_inlink' WzCanvasProperty of this.
        /// 
        /// '_inlink' is not implemented as part of WzCanvasProperty as I dont want to override existing Wz structure. 
        /// It will be handled via HaRepackerMainPanel instead.
        /// </summary>
        /// <returns></returns>
        public WzImageProperty GetLinkedWzImageProperty ()
        {
            string _inlink = ((WzStringProperty)this[InlinkPropertyName])?.Value; // could get nexon'd here. In case they place an _inlink that's not WzStringProperty
            string _outlink = ((WzStringProperty)this[OutlinkPropertyName])?.Value; // could get nexon'd here. In case they place an _outlink that's not WzStringProperty

            if (_inlink != null)
            {
                WzObject currentWzObj = this; // first object to work with
                while ((currentWzObj = currentWzObj.Parent) != null)
                {
                    if (!(currentWzObj is WzImage))  // keep looping if its not a WzImage
                        continue;

                    WzImage wzImageParent = (WzImage)currentWzObj;
                    WzImageProperty foundProperty = wzImageParent.GetFromPath (_inlink);
                    if (foundProperty != null && foundProperty is WzImageProperty property)
                    {
                        return property;
                    }
                }
            }
            else if (_outlink != null)
            {
                WzObject currentWzObj = this; // first object to work with
                while ((currentWzObj = currentWzObj.Parent) != null)
                {
                    if (!(currentWzObj is WzDirectory))  // keep looping if its not a WzImage
                        continue;
                    WzFile wzFileParent = ((WzDirectory)currentWzObj).wzFile;

                    // TODO
                    // Given the way it is structured, it might possibility also point to a different WZ file (i.e NPC.wz instead of Mob.wz).
                    // Mob001.wz/8800103.img/8800103.png has an outlink to "Mob/8800141.img/8800141.png"
                    // https://github.com/lastbattle/Harepacker-resurrected/pull/142

                    Match match = Regex.Match (wzFileParent.Name, @"^([A-Za-z]+)([0-9]*).wz");
                    string prefixWz = match.Groups[1].Value + "/"; // remove ended numbers and .wz from wzfile name 

                    WzObject foundProperty;

                    if (_outlink.StartsWith (prefixWz))
                    {
                        // fixed root path
                        string realpath = _outlink.Replace (prefixWz, WzFileParent.Name.Replace (".wz", "") + "/");
                        foundProperty = wzFileParent.GetObjectFromPath (realpath);
                    }
                    else
                    {
                        foundProperty = wzFileParent.GetObjectFromPath (_outlink);
                    }
                    if (foundProperty != null && foundProperty is WzImageProperty property)
                    {
                        return property;
                    }
                }
            }
            return this;
        }
    }
}