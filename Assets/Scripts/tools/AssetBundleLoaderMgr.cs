﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class AssetBundleLoaderMgr
{
    /// <summary>
    /// 初始化，加载AssetBundleManifest，方便后面查找依赖
    /// </summary>
    public void Init()
    {
        string persistentAssetBundlePath = Path.Combine(Application.persistentDataPath, "AssetBundle\\Android");
        AssetBundle streamingAssetsAb = AssetBundle.LoadFromFile(persistentAssetBundlePath);
        m_manifest = streamingAssetsAb.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
    }

    /// <summary>
    /// 加载AssetBundle
    /// </summary>
    /// <param name="abName">AssetBundle名称</param>
    /// <returns></returns>
    public AssetBundle LoadAssetBundle(string abName)
    {
       
        abName = abName.ToLower();
        AssetBundle ab = null;
        if (!m_abDic.ContainsKey(abName))
        {
            string abResPath = Path.Combine(Application.persistentDataPath, "AssetBundle", abName);
            //AppDebug.Log(m_abDic.ToDebugLog());
            //AppDebug.Log(abName);
            ab = AssetBundle.LoadFromFile(abResPath);
            m_abDic[abName] = ab;
        }
        else
        {
            ab = m_abDic[abName];
        }

        //加载依赖
        string[] dependences = m_manifest.GetAllDependencies(abName);
        int dependenceLen = dependences.Length;
        if (dependenceLen > 0)
        {
            for (int i = 0; i < dependenceLen; i++)
            {
                string dependenceAbName = dependences[i];
                if (!m_abDic.ContainsKey(dependenceAbName))
                {
                    AssetBundle dependenceAb = LoadAssetBundle(dependenceAbName);
                    m_abDic[dependenceAbName] = dependenceAb;
                }
            }
        }

        return ab;
    }

    /// <summary>
    /// 从AssetBundle中加载Asset
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="abName">AssetBundle名 AssetBundle的子路径</param>
    /// <param name="assetName">Asset名</param>
    /// <returns></returns>
    public T LoadAsset<T>(string abName, string assetName) where T : Object
    {
        if (m_manifest == null)
        {
            return null;
        }

        AssetBundle ab = LoadAssetBundle(abName);
        T t = ab.LoadAsset<T>(assetName);
        return t;
    }
    public T LoadSubAsset<T>(string abName, string assetName) where T : Object
    {
        if (m_manifest == null)
        {
            return null;
        }

        AssetBundle ab = LoadAssetBundle(abName);
        var t = ab.LoadAllAssets<T>().FirstOrDefault(a=>a.name == assetName);

        //T t = ab.LoadAsset<T>(assetName);
        return t;
    }
    public bool UnloadAssetBundle(string abName)
    {
        abName = abName.ToLower();
        AssetBundle ab = null;
        if (!m_abDic.ContainsKey(abName))
        {
            return false;
        }
        else
        {
            m_abDic[abName].UnloadAsync(true);
            m_abDic.Remove(abName);
        }

        //加载依赖
        /*string[] dependences = m_manifest.GetAllDependencies(abName);
        int dependenceLen = dependences.Length;
        if (dependenceLen > 0)
        {
            for (int i = 0; i < dependenceLen; i++)
            {
                string dependenceAbName = dependences[i];
                if (m_abDic.ContainsKey(dependenceAbName))
                {
                    UnloadAssetBundle(dependenceAbName);
                    m_abDic.Remove(dependenceAbName);
                }
            }
        }*/

        return true;
    }


    /// <summary>
    /// 缓存加载的AssetBundle，防止多次加载
    /// </summary>
    private Dictionary<string, AssetBundle> m_abDic = new Dictionary<string, AssetBundle>();

    /// <summary>
    /// 它保存了各个AssetBundle的依赖信息
    /// </summary>
    private AssetBundleManifest m_manifest;

    /// <summary>
    /// 单例
    /// </summary>
    private static AssetBundleLoaderMgr s_instance;
    public static AssetBundleLoaderMgr Instance
    {
        get
        {
            if (null == s_instance)
                s_instance = new AssetBundleLoaderMgr();
            return s_instance;
        }
    }
}