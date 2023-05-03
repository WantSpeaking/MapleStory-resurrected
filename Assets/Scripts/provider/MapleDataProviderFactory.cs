using System;
using ms;


namespace provider
{
	/*using WZFile = provider.wz.WZFile;
	using XMLWZFile = provider.wz.XMLWZFile;*/

	public class MapleDataProviderFactory
	{
		//private static readonly string wzPath = System.getProperty("wzpath");

		private static MapleDataProvider getWZ(string wzFileName, bool provideImages)
		{
			
			return wz.get_file (wzFileName);
			/*if (@in.Name.ToLower().EndsWith("wz", StringComparison.Ordinal) && !@in.Directory)
			{
				try
				{
					return new WZFile(@in, provideImages);
				}
				catch (Exception e)
				{
					throw new Exception("Loading WZ File failed", e);
				}
			}
			else
			{
				return new XMLWZFile(@in);
			}*/
		}

		public static MapleDataProvider getDataProvider(string wzFileName)
		{
			return getWZ(wzFileName, false);
		}

		public static MapleDataProvider getImageProvidingDataProvider(string wzFileName)
		{
			return getWZ(wzFileName, true);
		}

		/*public static File fileInWZPath(string filename)
		{
			return new File(wzPath, filename);
		}*/
	}
}