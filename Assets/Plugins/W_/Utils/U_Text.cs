/*Name:		 				U_Text	
 *Description: 				Text工具类
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
public static class U_Text
{
    public static string ShowHierarchyName(Text text, GameObject gameObject)
    {
        text.text = gameObject.name;
        return gameObject.name;
    }
    public static string ShowHierarchyName(Text text, Transform trans)
    {
        text.text = trans.name;
        return trans.name;
    }
    public static string ShowHierarchyNameOnlyChinese(Text text, GameObject gameObject)
    {
        string str = gameObject.name;
        str = ToOnlyChinese(str);
        text.text = str;
        return str;
    }
    public static string ShowHierarchyNameOnlyChinese(Text text, Transform trans)
    {
        string str = trans.name;
        str = ToOnlyChinese(str);
        text.text = str;
        return str;
    }
    //显示列表里的第一个物体的汉字,若是空的，保持上次的值。
    public static string ShowChineseByFirstList(Text text, List<Transform> tList)
    {
        if (tList.Count <= 0)
            return text.text;

        string str = tList[0].name;
        str = ToOnlyChinese(str);
        text.text = str;
        return str;
    }
    public static string ShowChineseByFirstList(Text text, List<GameObject> tList)
    {
        if (tList.Count <= 0)
            return text.text;

        string str = tList[0].name;
        str = ToOnlyChinese(str);
        text.text = str;
        return str;
    }


    public static string ToOnlyChinese(string str)
    {
        str= Regex.Replace(str, "[^\u4e00-\u9fa5]", "");
        return str;
    }

}
