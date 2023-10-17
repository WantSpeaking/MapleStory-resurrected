using MapleLib.WzLib;

namespace provider
{
	public interface MapleDataProvider
	{
        /// <summary>
        /// imgPath
        /// </summary>
        /// <param name="path">imgPath</param>
        /// <returns></returns>
        MapleData getData(string path);
        MapleDataDirectoryEntry Root {get;}
	}

}