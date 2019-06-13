/*Name:		 				U_Debug	
 *Description: 				调试类
 *Author:       			李文博 
 *Date:         			2018-08-20
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class U_Debug
{
    public static void Log123()
    {
        Debug.Log(123);
    }
    public static void Log456()
    {
        Debug.Log(456);
    }
    public static void LogErrorStr(string str)
    {
        Debug.LogError(str);
    }
    public static void LogError123()
    {
        Debug.LogError(123);
    }

    public static void LogError456()
    {
        Debug.LogError(456);
    }
    public static void LogStr(string str)
    {
        Debug.Log(str);
    }

    /// <summary>
    /// 打印列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ts"></param>
    public static void Debug_OBJList<T>(List<T> ts)
    {
        for (int i = 0; i < ts.Count; i++)
        {
            Debug.Log("下标：" + i + " " + ts[i]);
        }
    }

    /// <summary>
    /// 打印成员为List的列表，配合for使用,可打印全部，或者只打印某一组。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ts"></param>
    public static void Debug_OBJListOfList<T>(List<T> ts, int group)
    {
        for (int i = 0; i < ts.Count; i++)
        {
            Debug.Log("组下标：" + group + "小下标：" + i + " " + ts[i]);
        }
    }

    /// <summary>
    /// 打印成员为List的列表，配合for使用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tts">列表的列表</param>
    public static void Debug_OBJListOfList<T>(List<List<T>> tts)
    {
        for (int group = 0; group < tts.Count; group++)
        {
            List<T> ts = tts[group];
            for (int i = 0; i < ts.Count; i++)
            {
                Debug.Log("组下标：" + group + "        小下标：" + i + "           " + ts[i]);
            }
        }
    }
    /// <summary>
    /// 打印字典
    /// </summary>
    /// <typeparam name="K">键</typeparam>
    /// <typeparam name="V">值</typeparam>
    /// <param name="dic">字典</param>
    public static void Debug_Dictionary<K,V>(Dictionary<K,V> dic)
    {
        foreach (KeyValuePair<K,V> kv in dic)
        {
            Debug.Log("键："+kv.Key+"         值："+kv.Value);
        }
    }
}
