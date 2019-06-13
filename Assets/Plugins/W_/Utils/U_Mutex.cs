/*Name:		 				U_Mutex	
 *Description: 				只能运行一个本程序
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class U_Mutex
{
    private static U_Mutex _mutex;
    public static U_Mutex Instance()
    {
        if (null == _mutex)
        {
            _mutex = new U_Mutex();
        }
        return _mutex;
    }
    Mutex mutex = null;
    /// <summary>
    /// 判断程序是否在运行
    /// </summary>
    /// <param name="appID">程序名称</param>
    /// <returns>如果程序是第一次运行，返回True</returns>
	public bool IsFirst(string appID)
    {
        bool bExist = false;
        mutex = new System.Threading.Mutex(true, appID, out bExist);
        if (bExist && mutex.WaitOne(1))
        {
            return true;
        }
        else
        {
            Relase();
            return false;
        }
    }

    public static bool Exist(string appID)
    {
        bool bExist = false;
        var localmutex = new Mutex(false, appID, out bExist);
        localmutex.Close();
        localmutex = null;
        return !bExist;
    }

    private void Relase()
    {
        if (null != mutex)
        {
            mutex.Close();
            mutex = null;
        }
    }

    /// <summary>
    /// 释放占用
    /// </summary>
    public void Close()
    {
        if (null != mutex)
        {
            mutex.ReleaseMutex();
            Relase();
        }
    }


}
