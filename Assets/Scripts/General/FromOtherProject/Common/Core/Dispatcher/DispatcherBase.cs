using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
/// <typeparam name="C">actionCode</typeparam>
/// <typeparam name="P">actionParameter</typeparam>
/// <typeparam name="I">InstanceClass</typeparam>
public class DispatcherBase<C, P, I> : IDisposable where P:class  where I : new() 
{
    #region 单例
    private static I instance;

    public static I Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new I();
            }
            return instance;
        }
    }
    public virtual void Dispose()
    {

    }
    #endregion

    #region 委托 原型及字典
    public delegate void OnAction(P actionParameter);
    public Dictionary<C, List<OnAction>> dic = new Dictionary<C, List<OnAction>>();
    #endregion

    #region 添加监听
    public void AddListener(C actionCode, OnAction callBack)
    {
        if (dic.ContainsKey(actionCode))
        {
            dic[actionCode].Add(callBack);
        }
        else
        {
            List<OnAction> callList = new List<OnAction>();
            callList.Add(callBack);
            dic[actionCode] = callList;
        }
    }
    #endregion

    #region 移除监听
    public void RemoveListener(C actionCode, OnAction callBack)
    {
        if (dic.ContainsKey(actionCode))
        {
            List<OnAction> callList = dic[actionCode];
            callList.Remove(callBack);
            if (callList.Count == 0)
            {
                dic.Remove(actionCode);
            }
        }
    }
    #endregion

    #region 派发消息
    public void Dispatch(C actionCode, P actionParameter)
    {
        if (dic.ContainsKey(actionCode))
        {
            List<OnAction> callList = dic[actionCode];
            if (callList != null && callList.Count > 0)
            {
                for (int i = 0; i < callList.Count; i++)
                {
                    if (callList[i] != null)
                    {
                        callList[i](actionParameter);
                    }
                }
            }
        }
    }
    public void Dispatch(C actionCode)
    {
        Dispatch(actionCode, null);
    }
    #endregion
}