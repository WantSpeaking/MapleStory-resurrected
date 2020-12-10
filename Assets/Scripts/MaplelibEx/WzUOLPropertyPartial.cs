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
	public partial class WzUOLProperty
	{
		public override byte[] GetPngData (out PngInfo pngInfo)
		{
			return LinkValue.GetPngData (out pngInfo);
		}

		public override byte[] GetWavData (out WavInfo info)
		{
			if (LinkValue == null)
			{
				AppDebug.Log ($"LinkValue is null :linkPath:{Value}\t fullPath:{FullPath}");
				info = default;
				return null;
			}
			return LinkValue.GetWavData (out info);
		}
	}
}