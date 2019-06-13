/*Name:		 				U_List	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class U_List
{
    public static List<T> ArrayToList<T>(T[] ts)
    {
        List<T> tList = new List<T>(ts);
        return tList;
    }


    //本方法用于使用bool来改变组件状态，并决定是否加入列表的情况
    //例如高亮插件，可以再外部通过Inspector来判断是否高亮，高亮则在列表里，否则清除。
    //需要在Update()中调用
    public static void UpdateListByBool<T>(T t,bool accroding, List<T> list)
    {
        if (accroding)
        {
            if(!list.Contains(t))
            list.Add(t);
        }
        else
        {
            if (list.Contains(t))
                list.Remove(t);
        }
    }
   // List<T> 
    //清空列表,参数是列表数组，可填入多个
    public static void ClearList<T>(params List<T>[] tList)
    {
        for (int i = 0; i < tList.Length; i++)
        {
            if(tList[i].Count!=0)
            {
                tList[i].Clear();
            }
        }
    }

    //复制一个列表。否则是指向的同一个列表。
    public static List<T> CopyList<T>(List<T> oriTs)
    {
        List<T> tarTs = new List<T>();
        tarTs.AddRange(oriTs);
        return tarTs;
    }

    public static void DebugList<T>(List<T> ts)
    {
        foreach (T t in ts)
        {
            Debug.Log( ts.IndexOf(t) + "：" + t);
        }
    }
 /// <summary>
 /// 打印列表的列表
 /// </summary>
 /// <typeparam name="T"></typeparam>
 /// <param name="tss"></param>
    public static void DebugListOfList<T>(List<List<T>> tss)
    {
        foreach (List<T> ts in tss)
        {
            foreach (T t in ts)
            {
                Debug.Log(tss.IndexOf(ts) + "-" + ts.IndexOf(t) +"："+t);
            }
        }
    }

    /// <summary>
    /// 返回一个实例化好的List<List<string>>
    /// </summary>
    /// <param name="newCount"></param>
    /// <returns></returns>
    public static List<List<T>> NewListOfList<T>(int newCount)
    {
        List<List<T>> strSList = new List<List<T>>();
        for (int i = 0; i < newCount; i++)
        {
            strSList.Add(new List<T>());
        }
        return strSList;
    }

    public static void Clear<T>(params T[] ts)
    {
        for (int i = 0; i < ts.Length; i++)
        {
            ts[i] = default(T);
        }
    }

    public static void Clear<T>(ref List<T> ts)
    {
        ts.Clear();
    }

    //public static List<List<T>> NewList<T>(int newCount)
    //{
    //    List<T> strSList = new List<T>();
    //    for (int i = 0; i < newCount; i++)
    //    {

    //        strSList.Add(T);
    //    }
    //    return strSList;
    //}
}
