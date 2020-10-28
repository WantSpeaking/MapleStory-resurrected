//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-05-10 23:15:48
//备    注：
//===================================================
using UnityEngine;
using System.Collections;

public class AppDebug
{
    public static void Log(object message)
    {
#if DEBUG_LOG
        Debug.Log(message);
#endif
    }

    public static void LogError(object message)
    {
#if DEBUG_LOG
        Debug.LogError(message);
#endif
    }
}