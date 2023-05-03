using MapleLib.WzLib;

namespace provider
{
	public interface MapleDataProvider
	{
		WzObject getData(string path);
		WzFile Root {get;}
	}

}