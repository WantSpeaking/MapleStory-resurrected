using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using XLua;

/// <summary>
/// 网络通讯协议 观察者
/// </summary>
//[LuaCallCSharp]
public class SocketDispatcher: IDisposable
{

    #region 单例
    private static SocketDispatcher instance;
    public static SocketDispatcher Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SocketDispatcher();
            }
            return instance;
        }
    }
    public virtual void Dispose()
    {

    }
    #endregion

    //[CSharpCallLua]
    public delegate void OnActionHandler(byte[] buffer);

    #region 字典
    public Dictionary<ushort, OnActionHandler> actionDictionary = new Dictionary<ushort , OnActionHandler>();
    #endregion

    #region 添加监听
    public void AddListener(ushort actionCode, OnActionHandler listener)
    {
        if (actionDictionary.ContainsKey(actionCode))
        {
            actionDictionary[actionCode] += listener;
        }
        else
        {
            actionDictionary.Add(actionCode, listener);
        }

    }
    #endregion

    #region 移除监听
    public void RemoveListener(ushort actionCode, OnActionHandler listener)
    {
        if (actionDictionary.ContainsKey(actionCode))
        {
            actionDictionary[actionCode] -= listener;
        }
    }
    #endregion

    #region 派发消息
    public void Dispatch(ushort actionCode, byte[] actionParameter = null)
    {
        if (actionDictionary.ContainsKey(actionCode) && actionDictionary[actionCode] != null)
        {
            actionDictionary[actionCode](actionParameter);
        }
    }
    //public void Dispatch(C actionCode)
    //{
    //    Dispatch(actionCode, null);
    //}
    #endregion
}
