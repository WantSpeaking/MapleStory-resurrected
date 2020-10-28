//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-07-16 10:42:08
//备    注：
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 用户数据传输的类
/// </summary>
//[XLua.LuaCallCSharp]
public class TransferData
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public TransferData()
    {
        m_PutValues = new Dictionary<string, object>();
    }

    #region PutValues 数据字典
    /// <summary>
    /// 数据字典
    /// </summary>
    private Dictionary<string, object> m_PutValues;

    public Dictionary<string, object> PutValues
    {
        get { return m_PutValues; }
    }
    #endregion

    /// <summary>
    /// 存值
    /// </summary>
    /// <typeparam name="TM"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetValue<TM>(string key, TM value)
    {
        PutValues[key] = value;
    }

    /// <summary>
    /// 取值
    /// </summary>
    /// <typeparam name="TM"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public TM GetValue<TM>(string key)
    {
        if (PutValues.ContainsKey(key))
        {
            return (TM)PutValues[key];
        }
        return default(TM);
    }

    public object GetValue(string key)
    {
        if (PutValues.ContainsKey(key))
        {
            return PutValues[key];
        }
        return null;
    }
}