using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HaCreator.Wz;
using MapleLib.WzLib;
using provider;
using provider.wz;
using UnityEngine;

namespace ms
{

	public class ExportToPng : MonoBehaviour
	{
		// Start is called before the first frame update
		void Start ()
		{

		}

		// Update is called once per frame
		void Update ()
		{

		}
#if  UNITY_EDITOR
		
		private void OnGUI ()
		{
			if (GUI.Button (new Rect (0, 0, 400, 200), "����png"))
			{
				Export ();
			}

			if (GUI.Button (new Rect (400, 0, 400, 200), "getData"))
			{
				getData (null);
			}
		}

		public string wzPath;
		public string wzFileName;
		public WzFile wzFile;
		public List<string> map_Number_ToExport_List = new List<string> ();

		WzFileManager WzManager;
		//List<string> sprite_AssetPath_List = new List<string> ();

		void Export ()
		{
			WzManager = new WzFileManager (wzPath);
			WzManager.LoadWzFile (wzFileName);
			wz.wzFile_map = WzManager.wzFiles[wzFileName.ToLower ()];

			foreach (var Map1 in map_Number_ToExport_List)
			{
				var s = wz.wzFile_map["Map"][Map1];
				foreach (var img in (wz.wzFile_map["Map"][Map1] as WzDirectory).WzImages)
				{
					var mapId = int.Parse( img.Name.Replace (".img", ""));
					Debug.Log($"mapId:{mapId}");
					Stage.Instance.load_map (mapId);
					var tiles = Stage.Instance.get_tilesobjs ();
					var backgrounds = Stage.Instance.get_backgrounds ();

					//sprite_AssetPath_List.Clear ();
					foreach (var pair in tiles.get_layers ())
					{
						foreach (var pair1 in pair.Value.get_tiles ())
						{
							foreach (var tile in pair1.Value)
							{
								TextureAndSpriteUtil.SaveTex2dToPng (tile.get_texture ().texture2D, "WzPng/" + tile.get_texture ().fullPath);
								//sprite_AssetPath_List.Add ("Assets/Resources/" + tile.get_texture ().fullPath + ".png");
							}
						}

						foreach (var pair1 in pair.Value.get_objs ())
						{
							foreach (var obj in pair1.Value)
							{
								foreach (var frame in obj.get_animation ().get_frames ())
								{
									TextureAndSpriteUtil.SaveTex2dToPng (frame.get_texture ().texture2D, "WzPng/" + frame.get_texture ().fullPath);
								}
							}
						}
					}
				}
			}

		

	/*		UnityEditor.AssetDatabase.Refresh ();
			foreach (var nodePath in sprite_AssetPath_List)
			{
				UnityEditor.AssetDatabase.StartAssetEditing ();
				var importer = UnityEditor.AssetImporter.GetAtPath ("Assets/Resources/" + nodePath + ".png") as UnityEditor.TextureImporter;
				importer.textureType = UnityEditor.TextureImporterType.Sprite;
				importer.filterMode = FilterMode.Point;
				UnityEditor.AssetDatabase.ImportAsset ("Assets/Resources/" + nodePath + ".png", UnityEditor.ImportAssetOptions.ForceUpdate);
			}*/
		}

		public virtual MapleData getData (string path)
		{
			lock (this)
			{
				var dataFile = new FileInfo (Constants.get().path_WzXmlFolder + "/Map.wz/Map/Map1/100000000.img" + ".xml");
				if (!dataFile.Exists)
				{
					return null; //bitches
				}
				FileStream fis;
				try
				{
					fis = new FileStream (dataFile.FullName, FileMode.Open, FileAccess.Read);
				}
				catch (FileNotFoundException)
				{
					throw new Exception ("Datafile " + path + " does not exist in ");
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
		
#endif

	}
}
