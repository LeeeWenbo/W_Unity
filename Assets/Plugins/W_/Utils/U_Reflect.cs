/*Name:		 				U_Reflect	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Reflect:MonoBehaviour
{

    private void Start()
    {
        A a=  StringToClass("A") as A;
        a.DebugA();
    }

    //string 转 class        返回object，
    public static object StringToClass(string classStr)
    {
        Type type = Type.GetType( new U_Reflect().GetType().Namespace + "." + classStr, true, true);
        object temp = Activator.CreateInstance(type);
        return temp;
    }
}

public class A
{
    public void DebugA()
    {
        Debug.Log("A");
    }
}
