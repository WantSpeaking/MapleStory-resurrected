using UnityEngine;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using ms;

/// <summary>
/// �������鹤��
/// </summary>
public class ExportSpriteEditor
{
    [MenuItem("Tools/��������")]
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
                        // ��������Ŀ¼
                        string exportPath = Application.dataPath + "/ExportSprite";
                        System.IO.Directory.CreateDirectory(exportPath);

                        foreach (UnityEngine.Sprite sprite in sprites)
                        {
                            Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height,
                                sprite.texture.format, false);
                            tex.SetPixels(sprite.texture.GetPixels((int)sprite.rect.xMin, (int)sprite.rect.yMin,
                                (int)sprite.rect.width, (int)sprite.rect.height));
                            tex.Apply();

                            // ��ͼƬ����д���ļ�
                            System.IO.File.WriteAllBytes(exportPath + "/" + sprite.name + ".png", tex.EncodeToPNG());
                        }
                        Debug.Log("�������鵽" + exportPath);
                    }
                    Debug.Log("�����������");
                }
                    
            }
        }

        
        
        // ˢ����Դ
        AssetDatabase.Refresh();

        /*foreach (Object obj in Selection.objects)
        {
            string selectionPath = AssetDatabase.GetAssetPath (obj);
            if (selectionPath.StartsWith (resourcesPath))
            {
                string selectionExt = System.IO.Path.GetExtension (selectionPath);
                if (selectionExt.Length == 0)
                {
                    Debug.LogError ($"��Դ{selectionPath}����չ�����ԣ���ѡ��ͼƬ��Դ");
                    continue;
                }
                // ���selectionPath = "Assets/Resources/UI/Common.png"
                // ��ôloadPath = "UI/Common"
                string loadPath = selectionPath.Remove (selectionPath.Length - selectionExt.Length);
                loadPath = loadPath.Substring (resourcesPath.Length);
                // ���ش��ļ��µ�������Դ
                
            }
            else
            {
                Debug.LogError ($"�뽫��Դ����{resourcesPath}Ŀ¼��");
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

                    var textureWzO = wz.wzFile_map["MapHelper.img"]["portal"]["game"];

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
}
