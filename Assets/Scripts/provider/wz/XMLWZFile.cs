using System;
using System.IO;

namespace provider.wz
{

	public class XMLWZFile : MapleDataProvider
	{
		private DirectoryInfo root;
		private WZDirectoryEntry rootForNavigation;

		public XMLWZFile (System.IO.DirectoryInfo fileIn)
		{
			root = fileIn;
			rootForNavigation = new WZDirectoryEntry (fileIn.Name, 0, 0, null);
			fillMapleDataEntitys (root, rootForNavigation);
		}

		private void fillMapleDataEntitys (System.IO.DirectoryInfo lroot, WZDirectoryEntry wzdir)
		{
			foreach (var file in lroot.GetFileSystemInfos())
			{
				string fileName = file.Name;
				if (file is DirectoryInfo directoryInfo && !fileName.EndsWith (".img", StringComparison.Ordinal))
				{
					WZDirectoryEntry newDir = new WZDirectoryEntry (fileName, 0, 0, wzdir);
					wzdir.addDirectory (newDir);
					fillMapleDataEntitys (directoryInfo, newDir);
				}
				else if (fileName.EndsWith (".xml", StringComparison.Ordinal))
				{
					wzdir.addFile (new WZFileEntry (fileName.Substring (0, fileName.Length - 4), 0, 0, wzdir));
				}
			}
		}

		public virtual MapleData getData (string path)
		{
			lock (this)
			{
				var dataFile = new FileInfo (root.FullName + "/" + path + ".xml");
				if (!dataFile.Exists)
				{
					return null; //bitches
				}
				Stream fis;
				try
				{
					fis = new FileStream (dataFile.FullName, FileMode.Open, FileAccess.Read);
					/*var searchString = @"StreamingAssets\";
					var searchIndex = dataFile.FullName.IndexOf (searchString);
					var wzPngPath1 = dataFile.FullName.Substring (searchIndex + searchString.Length);
					fis = BetterStreamingAssets.OpenRead (wzPngPath1);*/
				}
				catch (FileNotFoundException)
				{
					throw new Exception ("Datafile " + path + " does not exist in " + root.FullName);
				}

				XMLDomMapleData domMapleData;
				try
				{
					domMapleData = new XMLDomMapleData (fis, dataFile.Directory);
				}
				finally
				{
					try
					{
						fis.Close ();
					}
					catch (IOException e)
					{
						throw e;
					}
				}
				return domMapleData;
			}
		}

		public virtual MapleDataDirectoryEntry Root
		{
			get
			{
				return rootForNavigation;
			}
		}
	}

}