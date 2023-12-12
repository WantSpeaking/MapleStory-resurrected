using UnityEngine;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using ms;
using Sprite = UnityEngine.Sprite;
using static Codice.CM.Common.CmCallContext;
using System.IO;
using System.Linq;

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
                var paths = s.name.Replace("-","\\");

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
}
