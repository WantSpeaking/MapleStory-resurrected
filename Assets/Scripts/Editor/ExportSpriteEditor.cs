using UnityEngine;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using ms;
using Sprite = UnityEngine.Sprite;
using static Codice.CM.Common.CmCallContext;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.Android;
using Jint.Runtime.Debugger;
using static PlasticGui.LaunchDiffParameters;
using UnityEngine.U2D;
using tools;

/// <summary>
/// 导出精灵工具
/// </summary>
public class ExportSpriteEditor
{
	[MenuItem("Tools/导出精灵")]
	static void ExportSprite()
	{
		foreach (var obj in Selection.objects)
		{
			if (obj is Texture2D)
			{
				var resourcesPath = "Assets/Resources/";
				var selectionPath = AssetDatabase.GetAssetPath(obj);
				if (selectionPath.StartsWith(resourcesPath))
				{
					string selectionExt = System.IO.Path.GetExtension(selectionPath);
					string loadPath = selectionPath.Remove(selectionPath.Length - selectionExt.Length);
					loadPath = loadPath.Substring(resourcesPath.Length);

					var sprites = Resources.LoadAll<UnityEngine.Sprite>(loadPath);
					if (sprites.Length > 0)
					{
						// 创建导出目录
						string exportPath = Application.dataPath + "/ExportSprite";
						System.IO.Directory.CreateDirectory(exportPath);

						foreach (UnityEngine.Sprite sprite in sprites)
						{
							Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height,
								sprite.texture.format, false);
							tex.SetPixels(sprite.texture.GetPixels((int)sprite.rect.xMin, (int)sprite.rect.yMin,
								(int)sprite.rect.width, (int)sprite.rect.height));
							tex.Apply();

							// 将图片数据写入文件
							System.IO.File.WriteAllBytes(exportPath + "/" + sprite.name + ".png", tex.EncodeToPNG());
						}
						Debug.Log("导出精灵到" + exportPath);
					}
					Debug.Log("导出精灵完成");
				}

			}
		}



		// 刷新资源
		AssetDatabase.Refresh();

		/*foreach (Object obj in Selection.objects)
        {
            string selectionPath = AssetDatabase.GetAssetPath (obj);
            if (selectionPath.StartsWith (resourcesPath))
            {
                string selectionExt = System.IO.Path.GetExtension (selectionPath);
                if (selectionExt.Length == 0)
                {
                    Debug.LogError ($"资源{selectionPath}的扩展名不对，请选择图片资源");
                    continue;
                }
                // 如果selectionPath = "Assets/Resources/UI/Common.png"
                // 那么loadPath = "UI/Common"
                string loadPath = selectionPath.Remove (selectionPath.Length - selectionExt.Length);
                loadPath = loadPath.Substring (resourcesPath.Length);
                // 加载此文件下的所有资源
                
            }
            else
            {
                Debug.LogError ($"请将资源放在{resourcesPath}目录下");
            }
        }*/
	}
}

public static class MyCustomTool
{
	static void SetPivot(ISpriteEditorDataProvider dataProvider)
	{
		// Get all the existing Sprites
		var spriteRects = dataProvider.GetSpriteRects();

		// Loop over all Sprites and update the pivots
		foreach (var rect in spriteRects)
		{
			var textureWzPaths = rect.name.Split('-');

			var textureWzO = ms.wz.wzFile_map["MapHelper.img"]["portal"]["game"];

			foreach (var textureWzPath in textureWzPaths)
			{
				textureWzO = textureWzO[textureWzPath];
			}
			Point_short origin = textureWzO?["origin"];
			//Debug.Log($"{rect.name} {textureWzO?["origin"]}");
			rect.alignment = SpriteAlignment.Custom;
			rect.pivot = new Vector2(origin?.x() / rect.rect.size.x ?? 0.5f, 1 - origin?.y() / rect.rect.size.y ?? 0.5f);
		}

		// Write the updated data back to the data provider
		dataProvider.SetSpriteRects(spriteRects);

		// Apply the changes
		//dataProvider.Apply();
	}

	[MenuItem("Custom/Set Portal Sprite Pivot")]
	static void UpdateSettings()
	{
		var maplestoryFolder = ms.Constants.get().path_MapleStoryFolder;

		ms.NxFiles.init(maplestoryFolder);

		foreach (var obj in Selection.objects)
		{
			if (obj is Texture2D)
			{
				var factory = new SpriteDataProviderFactories();
				factory.Init();
				var dataProvider = factory.GetSpriteEditorDataProviderFromObject(obj);
				dataProvider.InitSpriteEditorDataProvider();

				/* Use the data provider */
				// Get all the existing Sprites
				var spriteRects = dataProvider.GetSpriteRects();

				// Loop over all Sprites and update the pivots
				foreach (var rect in spriteRects)
				{
					var textureWzPaths = rect.name.Split('-');

					var textureWzO = wz.wzProvider_map["MapHelper.img"]["portal"]["game"];

					foreach (var textureWzPath in textureWzPaths)
					{
						textureWzO = textureWzO[textureWzPath];
					}
					Point_short origin = textureWzO?["origin"];
					//Debug.Log($"{rect.name} {textureWzO?["origin"]}");
					rect.alignment = SpriteAlignment.Custom;
					rect.pivot = new Vector2(origin?.x() / rect.rect.size.x ?? 0.5f, 1 - origin?.y() / rect.rect.size.y ?? 0.5f);
				}

				// Write the updated data back to the data provider
				dataProvider.SetSpriteRects(spriteRects);

				// Apply the changes made to the data provider
				dataProvider.Apply();

				// Reimport the asset to have the changes applied
				var assetImporter = dataProvider.targetObject as AssetImporter;
				assetImporter.SaveAndReimport();
			}
		}
	}

	[MenuItem("Custom/Set Obj Sprite Pivot")]
	static void UpdateSettings1()
	{
		var maplestoryFolder = ms.Constants.get().path_MapleStoryFolder;

		NxFiles.init(maplestoryFolder);

		foreach (var obj in Selection.objects)
		{
			if (obj is Texture2D)
			{
				var factory = new SpriteDataProviderFactories();
				factory.Init();
				var dataProvider = factory.GetSpriteEditorDataProviderFromObject(obj);
				dataProvider.InitSpriteEditorDataProvider();

				/* Use the data provider */
				// Get all the existing Sprites
				var spriteRects = dataProvider.GetSpriteRects();

				// Loop over all Sprites and update the pivots
				foreach (var rect in spriteRects)
				{
					var spriteName = rect.name;
					if (spriteName.Contains("etc.img-coconut-fontTime-"))
					{
						spriteName = spriteName.TrimEnd(' ').append("+");
					}

					var textureWzPaths = spriteName.Split('-');

					var textureWzO = wz.wzFile_map["Obj"];

					foreach (var textureWzPath in textureWzPaths)
					{
						var tempPath = textureWzPath;
						if (spriteName.Contains("NightMarketTW.img-NightMarketField-ladder") && textureWzPath.Contains("ladder"))
						{
							tempPath += " ";
						}
						else if (spriteName.Contains("SeoMoonJungTW.img-SeoMoonJungField-ladder") && textureWzPath.Contains("ladder"))
						{
							tempPath += " ";
						}
						else if (spriteName.Contains("shouwahouseJP.img-plasticsurgery") && textureWzPath.Contains("plasticsurgery"))
						{
							tempPath += " ";
						}
						else if (spriteName.Contains("path:houseTC.img-house5-elevator") && textureWzPath.Contains("elevator"))
						{
							tempPath += " ";
						}
						textureWzO = textureWzO?[tempPath];
					}
					if (textureWzO == null)
					{
						Debug.Log($"textureWzO is null, path:{rect.name} ");
						continue;
					}
					Point_short origin = textureWzO?["origin"];
					//Debug.Log($"{rect.name} {textureWzO?["origin"]}");
					rect.alignment = SpriteAlignment.Custom;
					rect.pivot = new Vector2(origin?.x() / rect.rect.size.x ?? 0.5f, 1 - origin?.y() / rect.rect.size.y ?? 0.5f);

					/* rect.pivot = new Vector2(0.1f, 0.2f);
                     rect.alignment = SpriteAlignment.Custom;*/
				}

				// Write the updated data back to the data provider
				dataProvider.SetSpriteRects(spriteRects);

				// Apply the changes made to the data provider
				dataProvider.Apply();

				// Reimport the asset to have the changes applied
				var assetImporter = dataProvider.targetObject as AssetImporter;
				assetImporter.SaveAndReimport();
			}
		}
	}

	[MenuItem("Custom/Set Back Sprite Pivot")]
	static void UpdateSettings2()
	{
		var maplestoryFolder = ms.Constants.get().path_MapleStoryFolder;

		NxFiles.init(maplestoryFolder);

		foreach (var obj in Selection.objects)
		{
			if (obj is Texture2D)
			{
				var factory = new SpriteDataProviderFactories();
				factory.Init();
				var dataProvider = factory.GetSpriteEditorDataProviderFromObject(obj);
				dataProvider.InitSpriteEditorDataProvider();

				/* Use the data provider */
				// Get all the existing Sprites
				var spriteRects = dataProvider.GetSpriteRects();

				// Loop over all Sprites and update the pivots
				foreach (var rect in spriteRects)
				{
					var spriteName = rect.name;
					if (spriteName.Contains("etc.img-coconut-fontTime-"))
					{
						spriteName = spriteName.TrimEnd(' ').append("+");
					}

					var textureWzPaths = spriteName.Split('-');

					var textureWzO = wz.wzFile_map["Back"];

					foreach (var textureWzPath in textureWzPaths)
					{
						var tempPath = textureWzPath;
						/*if (spriteName.Contains("NightMarketTW.img-NightMarketField-ladder") && textureWzPath.Contains("ladder"))
                        {
                            tempPath += " ";
                        }
                        else if (spriteName.Contains("SeoMoonJungTW.img-SeoMoonJungField-ladder") && textureWzPath.Contains("ladder"))
                        {
                            tempPath += " ";
                        }
                        else if (spriteName.Contains("shouwahouseJP.img-plasticsurgery") && textureWzPath.Contains("plasticsurgery"))
                        {
                            tempPath += " ";
                        }
                        else if (spriteName.Contains("path:houseTC.img-house5-elevator") && textureWzPath.Contains("elevator"))
                        {
                            tempPath += " ";
                        }*/
						textureWzO = textureWzO?[tempPath];
					}
					if (textureWzO == null)
					{
						Debug.Log($"textureWzO is null, path:{rect.name} ");
						continue;
					}
					Point_short origin = textureWzO?["origin"];
					//Debug.Log($"{rect.name} {textureWzO?["origin"]}");
					rect.alignment = SpriteAlignment.Custom;
					rect.pivot = new Vector2(origin?.x() / rect.rect.size.x ?? 0.5f, 1 - origin?.y() / rect.rect.size.y ?? 0.5f);

					/* rect.pivot = new Vector2(0.1f, 0.2f);
                     rect.alignment = SpriteAlignment.Custom;*/
				}

				// Write the updated data back to the data provider
				dataProvider.SetSpriteRects(spriteRects);

				// Apply the changes made to the data provider
				dataProvider.Apply();

				// Reimport the asset to have the changes applied
				var assetImporter = dataProvider.targetObject as AssetImporter;
				assetImporter.SaveAndReimport();
			}
		}
	}

	/*private static void ExportSubAsset(Object obj)
    {
        var exportDir = Path.GetDirectoryName(AssetDatabase.GetAssetPath(_current));
        var exportName = obj.name + " (Exported)." + GetFileExtension(obj);
        var uniquePath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(exportDir, exportName));
        AssetDatabase.CreateAsset(Object.Instantiate(obj), uniquePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }*/
	public static void ExtractFromAsset(Object subAsset, string destinationPath)
	{
		string assetPath = AssetDatabase.GetAssetPath(subAsset);
		var subAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
		foreach (var o in subAssets)
		{
			if (o is Sprite s)
			{
				Debug.Log($"{s.name} {s.GetType()}");
				var paths = s.name.Replace("-", "\\");

				var imgName = s.name.Substring(0, s.name.IndexOf(".img") + 4);
				Directory.CreateDirectory(Application.dataPath + $"/GameMain/Test/{imgName}");
				AssetDatabase.CreateAsset(Object.Instantiate(s), $"Assets/GameMain/Test/{imgName}/{s.name}.asset");
			}
		}
	}

	[MenuItem("Custom/Extract Sprite")]
	static void UpdateSettings3()
	{
		/*foreach (var obj in Selection.objects)
        {
            if (obj is Texture2D)
            {
                ExtractFromAsset(obj, "Assets/GameMain/Test");
            }
        }*/
		AssetBundle.UnloadAllAssetBundles(true);
		var ab = AssetBundle.LoadFromFile("G:\\Unity-Projects\\ForeverStory\\AssetBundle\\Working\\Android\\wzpng\\map\\back\\greenCrystalCave");
		var os = ab.LoadAllAssets();
		var c = ab.LoadAsset("greenCrystalCave.img_Sheet0");
		var b = ab.LoadAsset("greenCrystalCave.img-back-0");
		var a = ab.LoadAsset<UnityEngine.Sprite>("greenCrystalCave.img-back-0");
		var d = ab.LoadAllAssets<UnityEngine.Sprite>().FirstOrDefault(a => a.name.Contains("greenCrystalCave.img-back-0"));
		foreach (var o in os)
		{
			Debug.Log($"{o.name} {o.GetType()}");
		}
		Debug.Log($"a greenCrystalCave.img-back-0:{a?.name} {a?.GetType()}");

		Debug.Log($"b greenCrystalCave.img-back-0:{b?.name} {b?.GetType()}");
		Debug.Log($"c greenCrystalCave.img-back-0:{c?.name} {c?.GetType()}");
		Debug.Log($"d greenCrystalCave.img-back-0:{d?.name} {d?.GetType()}");
		/*var fullPath = "Map.wz\\Back\\Amoria.img\\back\\1";
        var dotimgIndex = fullPath.IndexOf(".img");
        var imgPath = fullPath.Substring(0, dotimgIndex + 4);
        var img_FileInfo = new FileInfo(imgPath);
        var imgName = img_FileInfo.Name;
        var imgName_Remove_Img = imgName.Replace(".img", "");
        var abName = $"wzpng\\map\\back\\{imgName_Remove_Img}";

        var imgNameIndex = fullPath.IndexOf(imgName);
        var texPath = fullPath.Substring(imgNameIndex);
        Debug.Log(texPath);
        var assetName = texPath.Replace("\\", "-");
        Debug.Log(assetName);*/

	}

	[MenuItem("Custom/Set Custom Png format")]
	static void UpdateSettings4()
	{
		var maplestoryFolder = ms.Constants.get().path_MapleStoryFolder;

		NxFiles.init(maplestoryFolder);

		foreach (var obj in Selection.objects)
		{
			if (obj is Texture2D)
			{
				var factory = new SpriteDataProviderFactories();
				factory.Init();
				var dataProvider = factory.GetSpriteEditorDataProviderFromObject(obj);
				dataProvider.InitSpriteEditorDataProvider();

				/* Use the data provider */
				// Get all the existing Sprites
				var spriteRects = dataProvider.GetSpriteRects();

				// Loop over all Sprites and update the pivots
				foreach (var rect in spriteRects)
				{
					var spriteName = rect.name;
					if (spriteName.Contains("etc.img-coconut-fontTime-"))
					{
						spriteName = spriteName.TrimEnd(' ').append("+");
					}

					var textureWzPaths = spriteName.Split('-');

					var textureWzO = wz.wzFile_map["Back"];

					foreach (var textureWzPath in textureWzPaths)
					{
						var tempPath = textureWzPath;

						textureWzO = textureWzO?[tempPath];
					}
					if (textureWzO == null)
					{
						Debug.Log($"textureWzO is null, path:{rect.name} ");
						continue;
					}
					Point_short origin = textureWzO?["origin"];
					//Debug.Log($"{rect.name} {textureWzO?["origin"]}");
					rect.alignment = SpriteAlignment.Custom;
					rect.pivot = new Vector2(origin?.x() / rect.rect.size.x ?? 0.5f, 1 - origin?.y() / rect.rect.size.y ?? 0.5f);

					/* rect.pivot = new Vector2(0.1f, 0.2f);
                     rect.alignment = SpriteAlignment.Custom;*/
				}

				// Write the updated data back to the data provider
				dataProvider.SetSpriteRects(spriteRects);

				// Apply the changes made to the data provider
				dataProvider.Apply();

				// Reimport the asset to have the changes applied
				var assetImporter = dataProvider.targetObject as AssetImporter;
				assetImporter.SaveAndReimport();
			}
		}
	}

	[MenuItem("Custom/Build ab")]
	static void UpdateSettings5()
	{

		/*foreach (var obj in Selection.objects)
		{
			
		}*/

		//打包（“输出路径”，备打文件集合，打包设置，目标平台）

		// Create the array of bundle build details.
		List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
		List<Object> objects = new List<Object>();
		List<string> assetNames = new List<string>();

		objects.AddRange(Selection.objects);
		var groupResult = objects.GroupBy(o =>
		{
			Debug.Log(o.name.Substring(0, o.name.Length - 1));
			return o.name.Substring(0, o.name.Length - 1);
		});
		foreach (var g in groupResult)
		{
			Debug.Log("key " + g.Key);
			AssetBundleBuild build = new AssetBundleBuild();
			var abName = g.Key.Contains(".img") ? g.Key.Substring(0, g.Key.IndexOf(".img")).ToLower() : g.Key;
			build.assetBundleName = abName;
			assetNames.Clear();

			foreach (var obj in g)
			{
				Debug.Log(obj.name);
				assetNames.Add(AssetDatabase.GetAssetPath(obj));
			}
			build.assetNames = assetNames.ToArray();
			builds.Add(build);
		}
		BuildPipeline.BuildAssetBundles("Assets/AssetBundle", builds.ToArray(), BuildAssetBundleOptions.None, BuildTarget.Android);

	}

	[MenuItem("Custom/Test")]
	static void UpdateSettings6()
	{
		Resources.UnloadUnusedAssets();
		AssetBundle.UnloadAllAssetBundles(true);
		AssetBundle.LoadFromFile("G:\\Unity-Projects\\ForeverStory\\Assets\\AssetBundle\\mainuihudatla");
		var ab = AssetBundle.LoadFromFile("G:\\Unity-Projects\\ForeverStory\\Assets\\AssetBundle\\spritesob");
		var assets = ab.LoadAllAssets();
		var ab1 = AssetBundle.LoadFromFile("G:\\Unity-Projects\\ForeverStory\\Assets\\AssetBundle\\spritesobj 1");
		var assets1 = ab1.LoadAllAssets();
		/*var ss = ab.LoadAssetWithSubAssets<Texture2D>("MainUIHudAtlas");
        var s = ss.First();
        var b = s.Sprite;
        Debug.Log(b?.name);*/
	}

	[MenuItem("Custom/Create SObj")]
	static void UpdateSettings7()
	{
		List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
		List<Object> objects = new List<Object>();
		List<string> assetNames = new List<string>();
		Dictionary<Sprite, string> sprites = new Dictionary<Sprite, string>();

		foreach (var o in Selection.objects)
		{
			if (o is Texture2D texture2D)
			{
				string assetPath = AssetDatabase.GetAssetPath(o);
				var subAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
				var spritePath = assetPath.Contains(".img") ? assetPath.Substring(0, assetPath.IndexOf(".img")) : assetPath;

				foreach (var sub in subAssets)
				{
					if (sub is Sprite s)
					{
						sprites.Add(s, spritePath);
					}
				}
			}
		}

		foreach (var pair in sprites)
		{

			var aso = ScriptableObject.CreateInstance<SpriteSObj>();
			aso.Sprite = pair.Key;
			var dirFullPath = Path.Combine(Application.dataPath, pair.Value.Replace("Assets/", ""));

			var atlasPath = dirFullPath;
			Directory.CreateDirectory(atlasPath);

			AssetDatabase.CreateAsset(aso, $"{pair.Value}/{pair.Key.name.Substring(pair.Key.name.IndexOf(".img") + 5)}.asset");
			AssetDatabase.SaveAssets();
		}

		/*objects.AddRange(Selection.objects);
		var groupResult = objects.GroupBy(o => {
			Debug.Log(o.name.Substring(0, o.name.Length - 1));
			return o.name.Substring(0, o.name.Length - 1);
		});
		foreach (var g in groupResult)
		{
			Debug.Log("key " + g.Key);
			AssetBundleBuild build = new AssetBundleBuild();
			var abName = g.Key.Contains(".img") ? g.Key.Substring(0, g.Key.IndexOf(".img")).ToLower() : g.Key;
			build.assetBundleName = abName;
			assetNames.Clear();

			foreach (var obj in g)
			{
				Debug.Log(obj.name);
				assetNames.Add(AssetDatabase.GetAssetPath(obj));
			}
			build.assetNames = assetNames.ToArray();
			builds.Add(build);
		}
		BuildPipeline.BuildAssetBundles("Assets/AssetBundle", builds.ToArray(), BuildAssetBundleOptions.None, BuildTarget.Android);*/

	}

	[MenuItem("Custom/Create AtlasObj")]
	static void UpdateSettings8()
	{
		List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
		List<Object> objects = new List<Object>();
		List<string> assetNames = new List<string>();
		Dictionary<Sprite, string> sprites = new Dictionary<Sprite, string>();
		List<string> fullPaths = new List<string>();

		foreach (var o in Selection.objects)
		{
			string assetPath = AssetDatabase.GetAssetPath(o);
			var fullPath = Path.Combine(Directory.GetCurrentDirectory(), assetPath);
			Debug.Log(fullPath);
			var files = Directory.GetFiles(fullPath, "*.png", SearchOption.AllDirectories);
			foreach (var file in files)
			{
				//var trimString = file.Substring(0, file.Length - 11);
				var trimString1 = file.Substring(file.IndexOf("Assets"));
				//Debug.Log(trimString1);

				fullPaths.Add(trimString1);
			}

			var groupResult = fullPaths.GroupBy(p => { return p.Substring(0, p.Length - 11); });
			foreach (var g in groupResult)
			{
				Debug.Log("key " + g.Key);

				var aso = ScriptableObject.CreateInstance<AtlasScriptObj>();

				foreach (var p in g)
				{
					Debug.Log(p);
					var subAssets = AssetDatabase.LoadAllAssetsAtPath(p);
					foreach (var sub in subAssets)
					{
						if (sub is Sprite s)
						{
							var trimName = $"{s.name.Substring(s.name.IndexOf(".img") + 5)}";
							Debug.Log($"sprite:{s}\t{trimName}");

							try
							{
								aso.Name_Sprites.TryAdd(trimName, s);
							}
							catch (System.Exception ex)
							{
								Debug.LogError($"{ex.Message}\t Key:{g.Key}\t trimName:{trimName}");
								break;
							}
						}
					}
					var asoPath = g.Key.Replace(".img", ".asset");
					AssetDatabase.CreateAsset(aso, asoPath);
					AssetDatabase.SaveAssets();
				}
			}
		}


	}
	[MenuItem("Custom/Create Atlas AB")]
	static void UpdateSettings9()
	{
		List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
		List<Object> objects = new List<Object>();
		List<string> assetNames = new List<string>();
		Dictionary<Sprite, string> sprites = new Dictionary<Sprite, string>();
		List<string> fullPaths = new List<string>();

		foreach (var o in Selection.objects)
		{
			string assetPath = AssetDatabase.GetAssetPath(o);
			var fullPath = Path.Combine(Directory.GetCurrentDirectory(), assetPath);
			//Debug.Log(fullPath);
			var files = Directory.GetFiles(fullPath, "*.png", SearchOption.AllDirectories);
			foreach (var file in files)
			{
				//var trimString = file.Substring(0, file.Length - 11);
				var trimString1 = file.Substring(file.IndexOf("Assets"));
				//Debug.Log(trimString1);

				fullPaths.Add(trimString1);
			}

			var groupResult = fullPaths.GroupBy(p => { return p.Substring(0, p.Length - 15); });
			//var groupResult2 = groupResult.Select(g=>g.Key).GroupBy(p=> p.Substring(0, p.LastIndexOf("\\")));
			//groupResult Assets/GameMain/WzPng/Mob\7220003
			foreach (var g in groupResult)
			{
				Debug.Log(g.Key);//Assets/GameMain/WzPng/Mob\7220003
				

				var abFolderPath = g.Key.Substring(g.Key.IndexOf("WzPng"));//WzPng/Mob\7220003
				var abFolderPath1 = abFolderPath.Substring(0, abFolderPath.LastIndexOf("\\"));//WzPng/Mob

				

					var abFolderPath2 = $"{Directory.GetCurrentDirectory()}/Assets/AssetBundle/{abFolderPath1}";
				Directory.CreateDirectory(abFolderPath2);
				assetNames.Clear();
				builds.Clear();

				/*var ap = p.Substring(p.IndexOf("Assets"));//Assets/GameMain/WzPng/Mob\7220003
					var ap2 = ap.Replace(".png", "");
					var ap3 = ap2.Substring(0, ap2.Length - 11) + ".asset";*/
				var ap3 = g.Key + ".asset";
				Debug.Log(ap3);

				var abName = g.Key.Substring(g.Key.LastIndexOf("\\") + 1).ToLower();
				//abName = abName.Substring(0, abName.Length - 11);
				//Debug.Log(abName);

				if (File.Exists($"{Directory.GetCurrentDirectory()}/Assets/AssetBundle/{abFolderPath1}/abName"))
					continue;

				AssetBundleBuild build = new AssetBundleBuild();
				build.assetBundleName = abName;
				assetNames.Add(ap3);
				build.assetNames = assetNames.ToArray();

				builds.Add(build);


				//Debug.Log(abFolderPath2);

				foreach (var p in g)
				{


				}
				BuildPipeline.BuildAssetBundles($"Assets/AssetBundle/{abFolderPath1}", builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);

			}


			foreach (var file in files)
			{


			}
		}

	}

	[MenuItem("Custom/Create Sound AB")]
	static void UpdateSettings10()
	{
		List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
		List<Object> objects = new List<Object>();
		List<string> assetNames = new List<string>();
		Dictionary<Sprite, string> sprites = new Dictionary<Sprite, string>();
		List<string> fullPaths = new List<string>();

		foreach (var o in Selection.objects)
		{
			string assetPath = AssetDatabase.GetAssetPath(o);
			var fullPath = Path.Combine(Directory.GetCurrentDirectory(), assetPath);
			//Debug.Log(fullPath);
			var files = Directory.GetFiles(fullPath, "*.mp3", SearchOption.AllDirectories);
			foreach (var file in files)
			{
				//var trimString = file.Substring(0, file.Length - 11);
				var trimString1 = file.Substring(file.IndexOf("Assets"));
				//Debug.Log(trimString1);

				fullPaths.Add(trimString1);
			}

			var groupResult = fullPaths.GroupBy(p => { return p.Substring(0, p.Length - 4); });
			//var groupResult2 = groupResult.Select(g=>g.Key).GroupBy(p=> p.Substring(0, p.LastIndexOf("\\")));
			//groupResult Assets/GameMain/WzPng/Mob\7220003
			foreach (var g in groupResult)
			{
				Debug.Log(g.Key);//Assets/GameMain/WzPng/Mob\7220003


				var abFolderPath = g.Key.Substring(g.Key.IndexOf("WzPng"));//WzPng/Mob\7220003
				var abFolderPath1 = abFolderPath.Substring(0, abFolderPath.LastIndexOf("\\"));//WzPng/Mob
				var abFolderPath2 = $"{Directory.GetCurrentDirectory()}/AssetBundle/{abFolderPath1}";

				var abFilePath = $"{Directory.GetCurrentDirectory()}/AssetBundle/{abFolderPath}";
				if (File.Exists(abFilePath))
				{
					continue;
				}

                Directory.CreateDirectory(abFolderPath2);
				assetNames.Clear();
				builds.Clear();

				var ap3 = g.Key + ".mp3";
				Debug.Log(ap3);

				var abName = g.Key.Substring(g.Key.LastIndexOf("\\") + 1).ToLower();
				//abName = abName.Substring(0, abName.Length - 11);
				//Debug.Log(abName);

				/*if (File.Exists($"{Directory.GetCurrentDirectory()}/Assets/AssetBundle/{abFolderPath1}/abName"))
					continue;*/

				AssetBundleBuild build = new AssetBundleBuild();
				build.assetBundleName = abName;
				assetNames.Add(ap3);
				build.assetNames = assetNames.ToArray();

				builds.Add(build);


				//Debug.Log(abFolderPath2);

				BuildPipeline.BuildAssetBundles(abFolderPath2, builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);

			}

		}

	}

	[MenuItem("Custom/Delete meta file")]
	static void UpdateSettings11()
	{
		var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "AssetBundle/WzPng");
		//Debug.Log(fullPath);
		var files = Directory.GetFiles(fullPath, "*.meta", SearchOption.AllDirectories);
		foreach (var file in files)
		{
			File.Delete(file);
		}

	}

	[MenuItem("Custom/Create Sound AB2")]
	static void UpdateSettings12()
	{
		List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
		List<Object> objects = new List<Object>();
		List<string> assetNames = new List<string>();
		Dictionary<Sprite, string> sprites = new Dictionary<Sprite, string>();
		List<string> fullPaths = new List<string>();

		foreach (var o in Selection.objects)
		{
			string assetPath = AssetDatabase.GetAssetPath(o);
			var fullPath = Path.Combine(Directory.GetCurrentDirectory(), assetPath);
			//Debug.Log(fullPath);
			var files = Directory.GetFiles(fullPath, "*.mp3", SearchOption.AllDirectories);
			builds.Clear();
			foreach (var file in files)//G:\Unity-Projects\ForeverStory\Assets/GameMain/WzPng/Sound\Bgm00.img\FloralLife.mp3
			{

				var ap3 = file.Substring(file.IndexOf("Assets"));//Assets/GameMain/WzPng/Sound\Bgm00.img\FloralLife.mp3
																		 //Debug.Log(trimString1);
				var abName = file.Substring(file.IndexOf("Sound")+6).Replace(".mp3","").Replace(".wz", "").Replace(".img", "").Replace("\\","_");//Bgm00.img\FloralLife.mp3

				if (File.Exists($"{Directory.GetCurrentDirectory()}\\AssetBundle\\wzpng\\Sound\\{abName}"))
				{
					continue;
				}
				AssetBundleBuild build = new AssetBundleBuild();
				build.assetBundleName = abName;
				assetNames.Clear();
				assetNames.Add(ap3);
				build.assetNames = assetNames.ToArray();
				builds.Add(build);
			}
			BuildPipeline.BuildAssetBundles($"{Directory.GetCurrentDirectory()}\\AssetBundle\\wzpng\\Sound", builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
		}

	}
}
