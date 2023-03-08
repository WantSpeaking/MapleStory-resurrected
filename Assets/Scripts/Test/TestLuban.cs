using SimpleJSON;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class TestLuban : SingletonMono<TestLuban>
{
    public const string gameConfDir = "\\Resources\\GenerateDatas";

    private static cfg.Tables _tables;
    [ShowInInspector]
    private SystemLanguage systemLanguage = SystemLanguage.ChineseSimplified;

    public static cfg.Tables Tables => _tables ??= new cfg.Tables(file =>
            JSON.Parse(File.ReadAllText(Application.dataPath + gameConfDir + "/" + file + ".json")));

    public SystemLanguage SystemLanguage { get => systemLanguage; private set => systemLanguage = value; }
    public string GetL10nText(string key)
    {
        try
        {
            switch (SystemLanguage)
            {
                case SystemLanguage.ChineseSimplified:
                    return Tables.TbTextMapper.Get(key).OriginText ?? string.Empty;
                case SystemLanguage.English:
                    return Tables.TbTextMapper.Get(key).TextEn ?? string.Empty;
                default:
                    return key;
            }
        }
        catch (KeyNotFoundException ex)
        {
            return key;
        }
    }
}
