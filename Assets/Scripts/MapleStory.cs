using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Assets.ms;
using HaRepacker;
using MapleLib.WzLib;
using ms;
using nl;
using UnityEngine;
using UnityEngine.UI;
using Char = ms.Char;
using Image = System.Drawing.Image;
using Sprite = UnityEngine.Sprite;

public class MapleStory : MonoBehaviour
{
    public Button button_load;

    public InputField inpuField_MapleFolder;
    public InputField inpuField_MapId;
    
    //public SpriteRenderer spriteRenderer;//用来显示图片
    public string maplestoryFolder = @"F:/BaiduYunDownload/079mg5/";
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
        button_load.onClick.AddListener (OnButtonLoadClick);
       
    }

    void init ()
    {
        NxFiles.init (maplestoryFolder);
        Char.init();
        Stage.get().init();
    }
    
    void OnButtonLoadClick ()
    {
        maplestoryFolder = inpuField_MapleFolder.text;
        mapIdToLoad = int.Parse (inpuField_MapId.text);

        init ();
        
        //nx.load_all(maplestoryFolder);
        //Stage.get().load_map(mapIdToLoad);
        //Stage.get().draw(1);

        var charEntry = parse_charentry (new InPacket ("",0));
        Stage.get().loadplayer (charEntry);
        Stage.get().load (100000000,0);
        Stage.get().draw(1);
        
    }
    // Update is called once per frame
    void Update()
    {

    }

    CharEntry parse_charentry(InPacket recv)
    {
        int cid = 0;
        StatsEntry stats = parse_stats(recv);
        LookEntry look = parse_look(recv);

        //recv.read_bool(); // 'rankinfo' bool

        /*if (recv.read_bool())
        {
            int currank = recv.read_int();
            int rankmv = recv.read_int();
            int curjobrank = recv.read_int();
            int jobrankmv = recv.read_int();
            sbyte rankmc = (rankmv > 0) ? '+' : (rankmv < 0) ? '-' : '=';
            sbyte jobrankmc = (jobrankmv > 0) ? '+' : (jobrankmv < 0) ? '-' : '=';

            stats.rank =new Tuple<int, sbyte> (currank, rankmc);
            stats.jobrank = new Tuple<int, sbyte> (curjobrank, jobrankmc);
        }*/

        return new CharEntry (stats, look, cid);
    }
    
    LookEntry parse_look(InPacket recv)
    {
        LookEntry look = new LookEntry ();

        look.female = false;
        look.skin = (byte)0;
        look.faceid = 20000;

        //recv.read_bool(); // megaphone

        look.hairid = 30000;

        /*byte eqslot = (byte)recv.read_byte();

        while (eqslot != 0xFF)
        {
            look.equips[eqslot] = recv.read_int();
            eqslot = (byte)recv.read_byte();
        }*/

        /*byte mskeqslot = (byte)recv.read_byte();

        while (mskeqslot != 0xFF)
        {
            look.maskedequips[(sbyte)mskeqslot] = recv.read_int();
            mskeqslot = (byte)recv.read_byte();
        }*/

        look.maskedequips[-111] = 0;

        for (byte i = 0; i < 3; i++)
            look.petids.Add(i);

        return look;
        
        
        /*LookEntry look = new LookEntry ();

        look.female = recv.read_bool();
        look.skin = (byte)recv.read_byte();
        look.faceid = recv.read_int();

        recv.read_bool(); // megaphone

        look.hairid = recv.read_int();

        byte eqslot = (byte)recv.read_byte();

        while (eqslot != 0xFF)
        {
            look.equips[eqslot] = recv.read_int();
            eqslot = (byte)recv.read_byte();
        }

        byte mskeqslot = (byte)recv.read_byte();

        while (mskeqslot != 0xFF)
        {
            look.maskedequips[(sbyte)mskeqslot] = recv.read_int();
            mskeqslot = (byte)recv.read_byte();
        }

        look.maskedequips[-111] = recv.read_int();

        for (byte i = 0; i < 3; i++)
            look.petids.Add(recv.read_int());

        return look;*/
    }
    
    StatsEntry parse_stats(InPacket recv)
    {
        StatsEntry statsentry = new StatsEntry ();

        statsentry.name = "HelloWorld";
        statsentry.female = false;

        /*recv.read_byte();	// skin
        recv.read_int();	// face
        recv.read_int();	// hair*/

        for (var i = 0; i < 3; i++)
            statsentry.petids.Add(i);

        statsentry.stats[ms.MapleStat.Id.LEVEL] = (ushort)1; // TODO: Change to recv.read_short(); to increase level cap
        statsentry.stats[ms.MapleStat.Id.JOB] = (ushort)100;
        statsentry.stats[ms.MapleStat.Id.STR] = (ushort)4;
        statsentry.stats[ms.MapleStat.Id.DEX] = (ushort)4;
        statsentry.stats[ms.MapleStat.Id.INT] = (ushort)4;
        statsentry.stats[ms.MapleStat.Id.LUK] = (ushort)4;
        statsentry.stats[ms.MapleStat.Id.HP] = (ushort)10;
        statsentry.stats[ms.MapleStat.Id.MAXHP] = (ushort)10;
        statsentry.stats[ms.MapleStat.Id.MP] = (ushort)10;
        statsentry.stats[ms.MapleStat.Id.MAXMP] = (ushort)10;
        statsentry.stats[ms.MapleStat.Id.AP] = (ushort)5;
        statsentry.stats[ms.MapleStat.Id.SP] = (ushort)5;
        statsentry.exp = 0;
        statsentry.stats[ms.MapleStat.Id.FAME] = (ushort)5;

        //recv.skip(4); // gachaexp

        statsentry.mapid = 100000000;
        statsentry.portal = (byte)0;

        //recv.skip(4); // timestamp

        return statsentry;
        
        /*// TODO: This is similar to CashShopParser.cpp, try and merge these.
        StatsEntry statsentry = new StatsEntry ();

        statsentry.name = recv.read_padded_string(13);
        statsentry.female = recv.read_bool();

        recv.read_byte();	// skin
        recv.read_int();	// face
        recv.read_int();	// hair

        for (var i = 0; i < 3; i++)
            statsentry.petids.Add(recv.read_long());

        statsentry.stats[ms.MapleStat.Id.LEVEL] = (ushort)recv.read_byte(); // TODO: Change to recv.read_short(); to increase level cap
        statsentry.stats[ms.MapleStat.Id.JOB] = (ushort)recv.read_short();
        statsentry.stats[ms.MapleStat.Id.STR] = (ushort)recv.read_short();
        statsentry.stats[ms.MapleStat.Id.DEX] = (ushort)recv.read_short();
        statsentry.stats[ms.MapleStat.Id.INT] = (ushort)recv.read_short();
        statsentry.stats[ms.MapleStat.Id.LUK] = (ushort)recv.read_short();
        statsentry.stats[ms.MapleStat.Id.HP] = (ushort)recv.read_short();
        statsentry.stats[ms.MapleStat.Id.MAXHP] = (ushort)recv.read_short();
        statsentry.stats[ms.MapleStat.Id.MP] = (ushort)recv.read_short();
        statsentry.stats[ms.MapleStat.Id.MAXMP] = (ushort)recv.read_short();
        statsentry.stats[ms.MapleStat.Id.AP] = (ushort)recv.read_short();
        statsentry.stats[ms.MapleStat.Id.SP] = (ushort)recv.read_short();
        statsentry.exp = recv.read_int();
        statsentry.stats[ms.MapleStat.Id.FAME] = (ushort)recv.read_short();

        recv.skip(4); // gachaexp

        statsentry.mapid = recv.read_int();
        statsentry.portal = (byte)recv.read_byte();

        recv.skip(4); // timestamp

        return statsentry;*/
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
