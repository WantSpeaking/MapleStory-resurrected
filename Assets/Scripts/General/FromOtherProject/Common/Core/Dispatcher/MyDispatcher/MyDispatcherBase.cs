using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDispatcherBase<C, P, I> : IDisposable where P : class where I : new()
{
    #region µ¥Àý
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

    #region ×Öµä
    public Dictionary<C, Action<P>> actionDictionary = new Dictionary<C, Action<P>>();
    #endregion

    #region Ìí¼Ó¼àÌý
    public void AddListener(C actionCode, Action<P> listener)
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

    #region ÒÆ³ý¼àÌý
    public void RemoveListener(C actionCode, Action<P> listener)
    {
        if (actionDictionary.ContainsKey(actionCode))
        {
            actionDictionary[actionCode] -= listener;
        }
    }
    #endregion

    #region ÅÉ·¢ÏûÏ¢
    public void Dispatch(C actionCode, P actionParameter=null)
    {
        if (actionDictionary.ContainsKey(actionCode)&& actionDictionary[actionCode]!=null)
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
