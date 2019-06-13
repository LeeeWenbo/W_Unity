/*Name:		 				W_StringUtils	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class U_String
{
    public const string TextSplitString = "===============================================================";
    public enum En_RemoveLine { 不去, 去掉空行, 去掉所有换行 }
    public enum En_Trim { 不去, 去掉前后空格, 去掉前空格, 去掉后空格, 去掉所有空格 };

    public const string indent= "\u3000\u3000" ;
    public const string enter = "\n";



    /// <summary>
    /// 给一段话加上首行缩进
    /// </summary>
    /// <param name="oriStr"></param>
    /// <returns></returns>
    public static string UGUI_Text_Indent(string oriStr)
    {
        string str = oriStr;
        string[] strS = oriStr.Split('\n');
        str = U_String.indent + string.Join("\n" + U_String.indent, strS);
        return str;
    }
    public static void DebugStrS(string[] str)
    {
        for(int i=0;i<str.Length;i++)
        {
            Debug.Log(i+"："+str[i]);
        }
    }





    /// <summary>
    /// 字符串去掉行
    /// </summary>
    /// <param name="oriStr"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static string StrRemoveLine(string oriStr, En_RemoveLine mode = En_RemoveLine.去掉空行)
    {
        string str = oriStr;
        if (mode == En_RemoveLine.去掉所有换行)
            str = oriStr.Replace("\n", string.Empty);
        else if (mode == En_RemoveLine.去掉空行)
        {
            str = Regex.Replace(oriStr, @"\n\s*\n", "\r\n");
        }
        return str;
    }

    public static string StrTrim(string oriStr, En_Trim trimMode = En_Trim.去掉前后空格)
    {
        string str = oriStr;
        if (trimMode == En_Trim.去掉前后空格)
            str = str.Trim();
        else if (trimMode == En_Trim.去掉前空格)
            str = str.TrimStart();
        else if (trimMode == En_Trim.去掉后空格)
            str = str.TrimEnd();
        else if (trimMode == En_Trim.去掉所有空格)
            str = str.Replace(" ", string.Empty);
        return str;
    }
    /// <summary>
    /// 字符串数组去空格
    /// </summary>
    /// <param name="oriStr"></param>
    public static string[] StrSTrim(string[] oriStr, En_Trim trimMode = En_Trim.去掉前后空格)
    {
        string[] strS = oriStr;
        if (trimMode == En_Trim.去掉前后空格)
        {
            for (int i = 0; i < oriStr.Length; i++)
            {
                strS[i] = oriStr[i].Trim();
            }
        }
        else if (trimMode == En_Trim.去掉前空格)
        {
            for (int i = 0; i < oriStr.Length; i++)
            {
                strS[i] = oriStr[i].TrimStart();
            }
        }
        else if (trimMode == En_Trim.去掉后空格)
        {
            for (int i = 0; i < oriStr.Length; i++)
            {
                strS[i] = oriStr[i].TrimEnd();
            }
        }
        else if (trimMode == En_Trim.去掉所有空格)
        {
            for (int i = 0; i < oriStr.Length; i++)
            {
                strS[i] = oriStr[i].Replace(" ", string.Empty);
            }
        }
        return strS;
    }

    /// <summary>
    /// 正数的，多用于名字字符串，例如1转换为01，99就是99
    /// </summary>
    /// <param name="i"></param>
    /// <param name="weishu"></param>
    /// <returns></returns>
    public static string IntToString(int i, int weishu = 2)
    {
        string str;
        if (weishu == 1)
        {
            if (i > 9)
                str = (i % 10).ToString();
            else
                str = i.ToString();
        }
        else if (weishu == 2)
        {
            if (i < 10)
            {
                str = "0" + i.ToString();
            }
            else if (i > 99)
            {
                str = (i % 100).ToString();
            }
            else
                str = i.ToString();
        }
        else if (weishu == 3)
        {
            if (i < 10)
            {
                str = "00" + i.ToString();
            }
            else if (i < 100)
            {
                str = "0" + i.ToString();
            }
            else if (i > 999)
            {
                str = (i % 1000).ToString();
            }
            else
                str = i.ToString();
        }
        //四位以上的就不考虑了。
        else
            str = i.ToString();
        return str;
    }

    //比较两个字符串是否相等
    public static bool CompareString(string str1, string str2)
    {
        if (string.Compare(str1, str2) == 0)
            return true;
        else
            return false;
    }
    //比较两个字符串列表的值，返回每一位是否相等
    public static List<bool> CompareStrsReturnBools(List<string> strList1, List<string> strList2, bool byFirst = true)
    {
        List<bool> boolCompare = new List<bool>();
        if (byFirst)
        {
            for (int i = 0; i < strList1.Count; i++)
            {
                if (string.Compare(strList1[i], strList2[i]) == 0)
                    boolCompare.Add(true);
                else
                    boolCompare.Add(false);
            }
        }
        else
        {
            for (int i = 0; i < strList2.Count; i++)
            {
                if (string.Compare(strList1[i], strList2[i]) == 0)
                    boolCompare.Add(true);
                else
                    boolCompare.Add(false);
            }
        }
        return boolCompare;
    }

    //比较两个字符串列表的值，返回不相等的字符的列表
    public static List<string> CompareStrsReturnStrs(List<string> strList1, List<string> strList2, bool byFirst = true)
    {
        List<string> strs = new List<string>();
        if (byFirst)
        {
            for (int i = 0; i < strList1.Count; i++)
            {
                if (string.Compare(strList1[i], strList2[i]) != 0)
                    strs.Add(strList1[i]);
            }
        }
        else
        {
            for (int i = 0; i < strList2.Count; i++)
            {
                if (string.Compare(strList1[i], strList2[i]) != 0)
                    strs.Add(strList2[i]);
            }
        }
        return strs;
    }

    // 截取后几位
    public static string GetLastChar(string str, int length)
    {
        str = str.Substring(str.Length - length, length);
        return str;
    }
    // 截取前几位
    public static string GetFirstChar(string str, int length)
    {
        str = str.Substring(0, length);
        return str;
    }

    // 字符串 倒数第几个开始截取 截取几个
    public static string Substring(string str, int lastIndex, int length)
    {
        str = str.Substring(str.Length - lastIndex, length);

        return str;
    }

    public static bool JudeContain(string str, string content)
    {
        if (str.Contains(content))
            return true;
        else
            return false;
    }
    // 是否在后几位包含某字符串
    public static bool JudeLastContain(string str, string content, int length)
    {
        str = GetLastChar(str, length);
        if (str.Contains(content))
            return true;
        else
            return false;
    }

    // 返回一个字符串列表中，带有某个字符的字符串。
    public static List<string> GetLastContainList(List<string> strList, string content, int lenth)
    {
        List<string> stringList = new List<string>();
        for (int i = 0; i < strList.Count; i++)
        {
            if (JudeLastContain(strList[i], content, lenth))
            {
                stringList.Add(strList[i]);
            }
        }
        return stringList;
    }

    // 最后几位变成int
    public static int LastStringToInt(string str, int length)
    {
        int i = 0;
        str = GetLastChar(str, length);

        try
        {
            i = int.Parse(str);
        }
        catch
        {
            Debug.Log("后" + i + "位并非字符串");
        }
        return i;

    }

    public static string[] SplitString(string oriStr, string splitStr = TextSplitString,En_Trim en_Trim=En_Trim.去掉前后空格)
    {
        string[] strS;
        strS = oriStr.Split(new[] { splitStr }, StringSplitOptions.None);
        strS = U_String.StrSTrim(strS, en_Trim);
        return strS;
    }

    /// <summary>
    /// 根据字符串切割字符串，返回第一个
    /// </summary>
    /// <param name="oriStr"></param>
    /// <param name="splitStr"></param>
    /// <returns></returns>
    public static string CutOutByStrng_First(string oriStr, string splitStr)
    {
        string result = "";
        string[] strS;
        strS = oriStr.Split(new[] { splitStr }, StringSplitOptions.None);
        result = strS[0];
        return result;
    }
    /// <summary>
    /// 根据字符串切割字符串，返回最后一个一个
    /// </summary>
    /// <param name="oriStr"></param>
    /// <param name="splitStr"></param>
    /// <returns></returns>
    public static string CutOutByStrng_Last(string oriStr, string splitStr)
    {
        string result = "";
        string[] strS;
        strS = oriStr.Split(new[] { splitStr }, StringSplitOptions.None);
        result = strS[strS.Length - 1];
        return result;
    }
}


public static class U_Str_Extren
{
    public static Vector3 ToVec3(this string str)
    {
        str = str.Replace("(", "").Replace(")", "");
        string[] s = str.Split(',');
        return new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
    }

    public static Vector2 ToVec2(this string str)
    {
        str = str.Replace("(", "").Replace(")", "");
        string[] s = str.Split(',');
        return new Vector2(float.Parse(s[0]), float.Parse(s[1]));
    }
}