using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Assets.ms;
using HaRepacker;
using MapleLib.WzLib;
using nl;
using UnityEngine;

public class Init : MonoBehaviour
{
    //public SpriteRenderer spriteRenderer;//用来显示图片
    public string maplestoryFolder = "F:/BaiduYunDownload/079mg5/";
    /*    public string path = "F:/BaiduYunDownload/079mg5/UI.wz";
        public string subPath = "UI.wz/Login.img/LoginVer/GameGrade";*/
    public int mapIdToLoad = 100000000;
    WzFileManager wzFileManager;
    // Start is called before the first frame update
    void Start()
    {
        /* wzFileManager = new WzFileManager();
         var wzFile = wzFileManager.LoadWzFile(path);
         var wzObject = wzFile.GetObjectFromPath(subPath);
         Debug.LogFormat("Width:{0}\t Height:{1}", wzObject?.GetBitmap()?.Width, wzObject?.GetBitmap()?.Height);
         spriteRenderer.sprite = TextureToSprite(GetTexrture2DFromPath(wzObject));*/

        nx.load_all(maplestoryFolder);
        Stage.get().load_map(mapIdToLoad);
        Stage.get().draw(1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static Texture2D GetTexrture2DFromPath(WzObject wzObject)
    {
        var bitMap = wzObject?.GetBitmap();
        var width = bitMap?.Width ?? 0;
        var height = bitMap?.Height ?? 0;

        Texture2D t2d = new Texture2D(width, height);
        t2d.LoadImage(ImageToByte2(bitMap));
        t2d.Apply();
        return t2d;
    }

    private static Sprite TextureToSprite(Texture2D tex)
    {
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }

    public static byte[] ImageToByte2(Image img)
    {
        using (var stream = new MemoryStream())
        {
            img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }
    }
}
