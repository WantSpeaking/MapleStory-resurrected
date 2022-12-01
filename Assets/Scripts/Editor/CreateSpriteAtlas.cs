using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.U2D;
using UnityEngine.U2D;

public class CreateSpriteAtlas
{
	static string rootPath = (Application.dataPath + "/").Replace ("\\", "/");
	static string pngExtension = ".png";
	static string spineExtension = ".asset";
	static string fontExtension = ".fontsettings";
	static string atlasExtension = ".spriteatlas";
	//static string atlasExtension = ".spriteatlasv2";
	//static string atlasNamePre = "atlas_";
	static string atlasNamePre = "";
	static string fileSearchPattern = "*.*";
	static int createAtlasCount = 0;
	static int deleteAtlasCount = 0;
	static List<AtlasInfo> atlasInfos = new List<AtlasInfo> ();

	static SpriteAtlasPackingSettings packSet = new SpriteAtlasPackingSettings ()
	{
		blockOffset = 1,
		enableRotation = false,
		enableTightPacking = false,
		padding = 2,
	};

	static SpriteAtlasTextureSettings textureSet = new SpriteAtlasTextureSettings ()
	{
		readable = false,
		generateMipMaps = false,
		sRGB = true,
		filterMode = FilterMode.Bilinear,
	};

	static TextureImporterPlatformSettings defaultPlatformSet = new TextureImporterPlatformSettings ()
	{
		name = "DefaultTexturePlatform",
		format = TextureImporterFormat.Automatic,
		//compressionQuality = 100,
	};

	static TextureImporterPlatformSettings standalonePlatformSet = new TextureImporterPlatformSettings ()
	{
		name = "Standalone",
		overridden = true,
		format = TextureImporterFormat.RGBA32,
		//compressionQuality = 100,
	};

	static TextureImporterPlatformSettings iPhonePlatformSet = new TextureImporterPlatformSettings ()
	{
		name = "iPhone",
		overridden = true,
		format = TextureImporterFormat.ASTC_6x6,
		//compressionQuality = 100,
	};

	static TextureImporterPlatformSettings androidPlatformSet = new TextureImporterPlatformSettings ()
	{
		name = "Android",
		overridden = true,
		format = TextureImporterFormat.ASTC_6x6,
		//compressionQuality = 100,
	};

	class AtlasInfo
	{
		public string atlasName;
		public string atlasPath;
		public List<string> texturePaths = new List<string> ();
	}

	[MenuItem ("AssetsTools/创建选择文件夹的SpriteAtlas", false, 1)]
	static void AtlasCreate ()
	{

		createAtlasCount = 0;
		atlasInfos.Clear ();
		Object[] selects = Selection.GetFiltered (typeof (Object), SelectionMode.Assets);
		for (int i = 0; i < selects.Length; ++i)
		{
			Object selected = selects[i];
			string path = AssetDatabase.GetAssetPath (selected);
			if (Directory.Exists (path))
			{
				AtlasCreateByFloder (path);
			}
		}

		AtlasCreateByinfo ();
		AssetDatabase.Refresh ();
		Debug.Log ("图集创建完毕！总计：" + createAtlasCount.ToString ());
	}

	[MenuItem ("AssetsTools/创建选择文件夹的SpriteAtlas1", false, 1)]
	static void AtlasCreate1 ()
	{
		//EditorApplication.ExecuteMenuItem ("Assets/Create/2D/Sprite Atlas");

		var spriteAtlas = new SpriteAtlas ();
		AssetDatabase.Refresh ();

		/*var selects = Selection.GetFiltered (typeof (Object), SelectionMode.DeepAssets);
		var atlas = new UnityEngine.U2D.SpriteAtlas ();
		atlas.Add (selects);*/

		var folderObjs = Selection.GetFiltered (typeof (UnityEngine.Object), SelectionMode.TopLevel);
		foreach (var folderObj in folderObjs)
		{
			var atlas = new UnityEngine.U2D.SpriteAtlas ();
			var folder = AssetDatabase.GetAssetPath (folderObj);//Assets/Resources/WzPng/Map.wz/Obj/acc1.img/darkWood/artificiality/0
			atlas.Add (new Object[] { folderObj });

			AssetDatabase.CreateAsset (atlas, folder + ".spriteatlas");
			AssetDatabase.SaveAssets ();
		}




		/*	EditorApplication.ExecuteMenuItem ("Assets/Create/2D/Sprite Atlas");
			Selection.A*/
	}
	[MenuItem ("Tools/testselect")]
	public static void testselect ()
	{
		UnityEngine.Object[] arr = Selection.GetFiltered (typeof (UnityEngine.Object), SelectionMode.TopLevel);
		Debug.LogError (Application.dataPath.Substring (0, Application.dataPath.LastIndexOf ('/')) + "/" + AssetDatabase.GetAssetPath (arr[0]));
	}
	[MenuItem ("AssetsTools/删除选择文件夹的SpriteAtlas", false, 2)]
	static void AtlasDelete ()
	{
		deleteAtlasCount = 0;
		Object[] selects = Selection.GetFiltered (typeof (Object), SelectionMode.Assets);
		for (int i = 0; i < selects.Length; ++i)
		{
			Object selected = selects[i];
			string path = AssetDatabase.GetAssetPath (selected);
			if (Directory.Exists (path))
			{
				AtlasDeleteByFloder (path);
			}
		}

		AssetDatabase.Refresh ();
		Debug.Log ("图集删除完毕！总计：" + deleteAtlasCount.ToString ());
	}

	[MenuItem ("AssetsTools/格式化选择文件夹的SpriteAtlas", false, 3)]
	static void SetAllAtlases ()
	{
		Object[] selects = Selection.GetFiltered (typeof (Object), SelectionMode.Assets);
		for (int i = 0; i < selects.Length; ++i)
		{
			Object selected = selects[i];
			string path = AssetDatabase.GetAssetPath (selected);
			if (Directory.Exists (path))
			{
				AtlasSetByFloder (path);
			}
		}

		AssetDatabase.Refresh ();
	}

	[MenuItem ("AssetsTools/Pack所有SpriteAtlas", false, 4)]
	static void PackAllAtlases ()
	{
#if UNITY_ANDROID
		SpriteAtlasUtility.PackAllAtlases (BuildTarget.Android);
#elif UNITY_IOS
        SpriteAtlasUtility.PackAllAtlases(BuildTarget.iOS);
#endif
	}

	static string GetAtlasName (string floderName)
	{
		return atlasNamePre + floderName + atlasExtension;
	}

	static string GetAssetsPath (string path)
	{
		string name = path.Replace ("\\", "/");
		name = name.Replace (rootPath, "Assets/");
		return name;
	}

	static void AtlasCreateByFloder (string path)
	{
		DirectoryInfo dir = new DirectoryInfo (path);
		DirectoryInfo[] dirs = dir.GetDirectories ();
		for (int i = 0; i < dirs.Length; ++i)
		{
			AtlasCreateByFloder (dirs[i].FullName);
		}

		FileInfo[] files = dir.GetFiles (fileSearchPattern, SearchOption.TopDirectoryOnly);
		List<string> textures = new List<string> ();
		for (int i = 0; i < files.Length; ++i)
		{
			FileInfo f = files[i];
			if (f.Extension.Equals (pngExtension))
			{
				string spinePath = f.FullName.Replace (pngExtension, spineExtension);
				string fontPath = f.FullName.Replace (pngExtension, fontExtension);
				if (!File.Exists (spinePath) && !File.Exists (fontPath))
				{
					textures.Add (f.FullName);
				}
			}
		}

		if (textures.Count > 0)
		{
			string atlasName = GetAtlasName (dir.Name);
			string atlasPath = Path.Combine (dir.FullName, atlasName);
			atlasPath = GetAssetsPath (atlasPath);
			AtlasInfo atlasInfo = new AtlasInfo ()
			{
				atlasName = atlasName,
				atlasPath = atlasPath,
				texturePaths = textures,
			};
			atlasInfos.Add (atlasInfo);
		}
	}

	static void AtlasCreateByinfo ()
	{
		AtlasInfo[] atlasInfosArr = atlasInfos.ToArray ();
		for (int i = 0; i < atlasInfosArr.Length; ++i)
		{
			AtlasInfo atlasInfo = atlasInfosArr[i];
			string atlasPath = atlasInfo.atlasPath;
			SpriteAtlas atlas;
			bool isNewAtlas = false;
			if (File.Exists (atlasPath))
			{
				//atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasPath);
				//atlas.Remove(atlas.GetPackables());
				File.Delete (atlasPath);
			}

			isNewAtlas = true;
			atlas = new SpriteAtlas ();
			atlas.SetIncludeInBuild (true);
			atlas.SetPackingSettings (packSet);
			atlas.SetTextureSettings (textureSet);
			atlas.SetPlatformSettings (defaultPlatformSet);
			atlas.SetPlatformSettings (standalonePlatformSet);
			atlas.SetPlatformSettings (iPhonePlatformSet);
			atlas.SetPlatformSettings (androidPlatformSet);

			List<Sprite> sp_list = new List<Sprite> ();
			string[] pathArr = atlasInfo.texturePaths.ToArray ();
			for (int j = 0; j < pathArr.Length; ++j)
			{
				string tPath = pathArr[j];
				Sprite sp = AssetDatabase.LoadAssetAtPath<Sprite> (GetAssetsPath (tPath));
				if (sp != null)
				{
					int width = sp.texture.width;
					int height = sp.texture.height;
					if (height > 1024 & width > 512)
					{
					}
					else if (width > 1024 & height > 512)
					{
					}
					else if (width <= 2048 & height <= 2048)
					{
						sp_list.Add (sp);
					}
				}
			}

			if (sp_list.Count > 0)
			{
				atlas.Add (sp_list.ToArray ());
				if (isNewAtlas)
				{
					AssetDatabase.CreateAsset (atlas, atlasPath);
				}

				AssetDatabase.SaveAssets ();
				createAtlasCount++;
			}
		}
	}

	static void AtlasDeleteByFloder (string path)
	{
		DirectoryInfo dir = new DirectoryInfo (path);
		DirectoryInfo[] dirs = dir.GetDirectories ();
		for (int i = 0; i < dirs.Length; ++i)
		{
			AtlasDeleteByFloder (dirs[i].FullName);
		}

		FileInfo[] files = dir.GetFiles (fileSearchPattern, SearchOption.TopDirectoryOnly);
		for (int i = 0; i < files.Length; ++i)
		{
			FileInfo f = files[i];
			string atlasName = GetAtlasName (dir.Name);
			if (f.FullName.EndsWith (atlasName))
			{
				File.Delete (f.FullName);
				deleteAtlasCount++;
			}
		}
	}

	static void AtlasSetByFloder (string path)
	{
		DirectoryInfo dir = new DirectoryInfo (path);
		DirectoryInfo[] dirs = dir.GetDirectories ();
		for (int i = 0; i < dirs.Length; ++i)
		{
			AtlasSetByFloder (dirs[i].FullName);
		}

		FileInfo[] files = dir.GetFiles (fileSearchPattern, SearchOption.TopDirectoryOnly);
		for (int i = 0; i < files.Length; ++i)
		{
			FileInfo f = files[i];
			string atlasName = GetAtlasName (dir.Name);
			if (f.FullName.EndsWith (atlasName))
			{
				SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas> (GetAssetsPath (f.FullName));
				atlas.SetIncludeInBuild (true);
				atlas.SetPackingSettings (packSet);
				atlas.SetTextureSettings (textureSet);
				atlas.SetPlatformSettings (defaultPlatformSet);
				atlas.SetPlatformSettings (standalonePlatformSet);
				atlas.SetPlatformSettings (iPhonePlatformSet);
				atlas.SetPlatformSettings (androidPlatformSet);
				AssetDatabase.SaveAssets ();
			}
		}
	}
}