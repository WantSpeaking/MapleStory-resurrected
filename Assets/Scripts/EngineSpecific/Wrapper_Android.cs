using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ms
{
    #region Caller
    public class Caller<TResult>
    {
        public static TResult Call(object ajo, string methodName, params object[] args)
        {
            var argsTypeString = "";
            /*    if (args != null && args.Length > 0)
               {
                   argsTypeString = args.Select(i => i.GetType()).ToDebugLog();
               } */
            Debug.Log("Caller param type:" + ajo.GetType() + " methodName:" + methodName + " argsType:" + argsTypeString);
            return (ajo as AndroidJavaObject).Call<TResult>(methodName, args);
        }
    }

    public class StringCaller : Caller<string>
    {

    }
    public class Int_StringCaller : Caller<string>
    {
        public static string Call(AndroidJavaObject ajo, string methodName, int args)
        {

            return (ajo as AndroidJavaObject).Call<string>(methodName, args);
        }
    }
    public class Int32Caller : Caller<int>
    {
    }
    public class FLoatCaller : Caller<float>
    {
    }
    public class DoubleCaller : Caller<double>
    {
    }
    public class BoolCaller : Caller<bool>
    {
    }
    public class AndroidJavaObjectCaller : Caller<AndroidJavaObject>
    {
    }
    #endregion

    #region StaticCaller
    public class StaticCaller<TResult>
    {
        public static TResult Call(object ajo, string methodName, params object[] args)
        {
            Debug.Log("Caller param type:" + ajo.GetType() + " methodName:" + methodName);
            return (ajo as AndroidJavaClass).CallStatic<TResult>(methodName, args);
        }
    }

    public class StringStaticCaller : StaticCaller<string>
    {
    }
    public class Int32StaticCaller : StaticCaller<int>
    {
    }
    public class FLoatStaticCaller : StaticCaller<float>
    {
    }
    public class DoubleStaticCaller : StaticCaller<double>
    {
    }
    public class BoolStaticCaller : StaticCaller<bool>
    {
    }
    public class AndroidJavaObjectStaticCaller : StaticCaller<AndroidJavaObject>
    {
    }
    #endregion

    #region GeterStatic
    public class GeterStatic<TResult>
    {
        public static TResult GetStatic(object ajc, string methodName)
        {
            Debug.Log("GeterStatic param type:" + ajc.GetType() + " methodName:" + methodName);
            return (ajc as AndroidJavaClass).GetStatic<TResult>(methodName);
        }
    }
    public class AndroidJavaObjectGeterStatic : GeterStatic<AndroidJavaObject>
    {
    }

    public class StringGeterStatic : GeterStatic<string>
    {
    }
    public class Int32GeterStatic : GeterStatic<int>
    {
    }
    public class FLoatGeterStatic : GeterStatic<float>
    {
    }
    public class DoubleGeterStatic : GeterStatic<double>
    {
    }
    public class BoolGeterStatic : GeterStatic<bool>
    {
    }
    #endregion

}