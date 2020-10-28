using System;
using System.Collections.Generic;
//using XLua;
/// <summary>
/// UI按钮点击的 观察者
/// </summary>
//[LuaCallCSharp]
public class UIDispatcher : IDisposable
{
    #region 单例
    private static UIDispatcher instance;
    public static UIDispatcher Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UIDispatcher();
            }
            return instance;
        }
    }
    public virtual void Dispose()
    {

    }
    #endregion

    //[CSharpCallLua]
    public delegate void OnActionHandler(string[] param);

    #region 字典
    public Dictionary<string, OnActionHandler> actionDictionary = new Dictionary<string, OnActionHandler>();
    #endregion

    #region 添加监听
    public void AddListener(string actionCode, OnActionHandler listener)
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
    public void RemoveListener(string actionCode, OnActionHandler listener)
    {
        if (actionDictionary.ContainsKey(actionCode))
        {
            actionDictionary[actionCode] -= listener;
        }
    }
    #endregion

    #region 派发消息
    public void Dispatch(string actionCode, string[] actionParameter = null)
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
