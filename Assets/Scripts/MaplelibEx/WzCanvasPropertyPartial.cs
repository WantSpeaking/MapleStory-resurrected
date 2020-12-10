using System.Collections.Generic;
using System.IO;
using MapleLib.WzLib.Util;
using System;
using System.Drawing;

namespace MapleLib.WzLib.WzProperties
{
	/// <summary>
	/// A property that can contain sub properties and has one png image
	/// </summary>
	public partial class WzCanvasProperty : WzExtended, IPropertyContainer
	{
		public byte[] GetLinkedWzCanvasPngData(out PngInfo pngInfo)
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
			return imageProp.GetPngData (out pngInfo);
		}
	}
}