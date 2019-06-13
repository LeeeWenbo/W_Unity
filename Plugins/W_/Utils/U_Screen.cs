/*Name:		 				U_Screen	
 *Description: 				屏幕工具类
 *Author:       			李文博 
 *Date:         			2018-08-30
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class U_Screen
{
    /// <summary>
    /// 获得当前分辨率的大小
    /// </summary>
    /// <returns></returns>
	public static Vector2 GetPixelSize()
    {
        Vector2 size;
        size = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        return size;
    }

}
