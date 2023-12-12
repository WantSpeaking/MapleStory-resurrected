using MapleLib.WzLib;

namespace provider
{
	public interface MapleDataProvider
	{
        MapleData getData(string imgPath);
        //MapleDataDirectoryEntry Root {get;}

        MapleData this[string imgPath] { get;}

    }

}