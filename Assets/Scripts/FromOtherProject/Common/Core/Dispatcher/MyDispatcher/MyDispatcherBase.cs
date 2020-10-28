using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDispatcherBase<C, P, I> : IDisposable where P : class where I : new()
{
    #region ����
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

    #region �ֵ�
    public Dictionary<C, Action<P>> actionDictionary = new Dictionary<C, Action<P>>();
    #endregion

    #region ��Ӽ���
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

    #region �Ƴ�����
    public void RemoveListener(C actionCode, Action<P> listener)
    {
        if (actionDictionary.ContainsKey(actionCode))
        {
            actionDictionary[actionCode] -= listener;
        }
    }
    #endregion

    #region �ɷ���Ϣ
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
