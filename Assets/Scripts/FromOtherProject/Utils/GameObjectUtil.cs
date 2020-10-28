//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2015-12-01 21:45:22
//备    注：
//===================================================
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class GameObjectUtil
{
    /// <summary>
    /// 获取或创建组建
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T GetOrCreatComponent<T>(this GameObject obj) where T : MonoBehaviour
    {
        T t = obj.GetComponent<T>();
        if (t == null)
        {
            t = obj.AddComponent<T>();
        }
        return t;
    }

    /// <summary>
    /// 设置物体的层
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="layerName"></param>
    public static void SetLayer(this GameObject obj, string layerName)
    {
        Transform[] transArr = obj.transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < transArr.Length; i++)
        {
            transArr[i].gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }

    /// <summary>
    /// 设置物体的父物体
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="parent"></param>
    public static void SetParent(this GameObject obj, Transform parent)
    {
        obj.transform.parent = parent;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.localEulerAngles = Vector3.zero;
    }

    public static void SetNull(this MonoBehaviour[] arr)
    {
        if (arr != null)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = null;
            }
        }
        arr = null;
    }

    public static void SetNull(this Transform[] arr)
    {
        if (arr != null)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = null;
            }
        }
        arr = null;
    }

    public static void SetNull(this Sprite[] arr)
    {
        if (arr != null)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = null;
            }
        }
        arr = null;
    }

    //UI扩展==============================================


    /// <summary>
    /// 设置Text
    /// </summary>
    /// <param name="text"></param>
    /// <param name="content"></param>
    //public static void SetText(this Text text, string content)
    //{
    //    if (text != null)
    //    {
    //        text.text = content;
    //    }
    //}
    //public static void SetText(this Text text, string content, bool isTrue)
    //{
    //    if (text != null)
    //    {
    //        text.text = content;
    //    }
    //}

    public static void SetText(this Text txtObj, string text, bool isAnimation = false, ScrambleMode scrambleMode = ScrambleMode.None)
    {
        if (txtObj != null)
        {
            if (isAnimation)
            {
                txtObj.text = "";
                txtObj.DOText(text, .2f, true, scrambleMode);
            }
            else
            {
                txtObj.text = text;
            }
        }
    }



    /// <summary>
    /// 设置Slider
    /// </summary>
    /// <param name="slider"></param>
    /// <param name="value"></param>
    public static void SetSliderValue(this Slider slider, float value)
    {
        if (slider != null)
        {
            slider.value = value;
        }
    }

    public static void SetImage(this Image image, Sprite sprite, bool isAnimation = false, ScrambleMode scrambleMode = ScrambleMode.None)
    {
        if (image != null)
        {
            if (isAnimation)
            {
                image.overrideSprite = sprite;
            }
        }
    }

    //public static RectTransform rectTransform(this Component cp)
    //{
    //    return cp.transform as RectTransform;
    //}
}
