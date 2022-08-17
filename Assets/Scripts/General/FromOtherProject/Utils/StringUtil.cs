//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2015-11-16 22:26:09
//备    注：
//===================================================
using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public static class StringUtil 
{
    /// <summary>
    /// string 转化成 int
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int ToInt(this string str)
    {
        int temp = 0;
        int.TryParse(str, out temp);
        return temp;
    }

    /// <summary>
    /// string 转化成long
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static long ToLong(this string str)
    {
        long temp = 0;
        long.TryParse(str, out temp);
        return temp;
    }

    /// <summary>
    /// string 转化成 float
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static float ToFloat(this string str)
    {
        float temp = 0;
        float.TryParse(str, out temp);
        return temp;
    }
}