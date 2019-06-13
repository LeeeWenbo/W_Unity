/*Name:		 				U_TextAssets	
 *Description: 				通过TextAssets读文件
 *Author:       			李文博 
 *Date:         			2018-11-28
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class U_TextAssets
{
    public enum En_ListSplitType { 根究组别分, 根据类别分, }
    /// <summary>
    /// Txt转化成字符串，要求按标志符分好的
    /// </summary>
    /// <param name="txt">textAsset</param>
    /// <param name="typeCount">几个分一组</param>
    /// <param name="splitStr">分隔符</param>
    /// <param name="isRemoveLine"></param>
    /// <returns></returns>
    public static List<List<string>> TxtToStrListList(TextAsset txt, int typeCount, En_ListSplitType splitType = En_ListSplitType.根据类别分, string splitStr = U_String.TextSplitString,U_String.En_RemoveLine removeLineMode = U_String.En_RemoveLine.去掉空行, U_String.En_Trim enTrim = U_String.En_Trim.去掉前后空格)
    {
        List<List<string>> strListList = new List<List<string>>();
        string allStr = ReadTxt(txt, removeLineMode);
        string[] strS = U_String.SplitString(allStr, splitStr,enTrim);
        if (splitType == En_ListSplitType.根据类别分)
            strListList = StrSTo_Type_List(typeCount, strS);
        else if (splitType == En_ListSplitType.根究组别分)
            strListList = StrSTo_Group_List(typeCount, strS);
        return strListList;
    }
    public static List<List<string>> TxtToStrListList(string allStr, int typeCount, En_ListSplitType splitType = En_ListSplitType.根据类别分, string splitStr = U_String.TextSplitString, U_String.En_Trim enTrim = U_String.En_Trim.去掉前后空格)
    {
        List<List<string>> strListList = new List<List<string>>();
        string[] strS = U_String.SplitString(allStr, splitStr, enTrim);
        if (splitType == En_ListSplitType.根据类别分)
            strListList = StrSTo_Type_List(typeCount, strS);
        else if (splitType == En_ListSplitType.根究组别分)
            strListList = StrSTo_Group_List(typeCount, strS);
        return strListList;
    }


    /// <summary>
    /// 读Txt转化成string
    /// </summary>
    /// <param name="txt"></param>
    /// <param name="isRemoveLine"></param>
    /// <returns></returns>
    public static string ReadTxt(TextAsset txt, U_String.En_RemoveLine removeMode = U_String.En_RemoveLine.去掉空行)
    {
        string allStr;
        allStr = txt.text;
        allStr = U_String.StrRemoveLine(allStr, removeMode);
        return allStr;
    }

    /// <summary>
    /// 将一个字符串数组根据i分类，按类别分组
    /// </summary>
    /// <param name="length"></param>
    /// <param name="ori"></param>
    /// <returns></returns>
    public static List<List<string>> StrSTo_Type_List(int length, string[] ori)
    {
        List<List<string>> strSList = U_List.NewListOfList<string>(length);
        for (int i = 0; i < ori.Length; i++)
        {
            int yushu = i % length;
            strSList[yushu].Add(ori[i]);
        }
        return strSList;
    }
    /// <summary>
    /// 将一个字符串数组根据i分类，按物体分组
    /// </summary>
    /// <param name="length"></param>
    /// <param name="ori"></param>
    /// <returns></returns>
    public static List<List<string>> StrSTo_Group_List(int length, string[] ori)
    {
        int strlenth = ori.Length / length;
        List<List<string>> strSList = U_List.NewListOfList<string>(strlenth);
        int bigIndex = -1;
        for (int i = 0; i < ori.Length; i++)
        {
            if (i % length == 0)
            {
                bigIndex += 1;
            }
            strSList[bigIndex].Add(ori[i]);
        }
        return strSList;
    }
}
