/*Name:		 				U_Test	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  static class U_Keyboard
{
    public static void Space_Down(TestByKey testByKey)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            testByKey();
        }
    }
    public static void Return_Down(TestByKey testByKey)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            testByKey();
        }
    }

    public static void LeftArrow(TestByKey testByKey)
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            testByKey();
        }
    }
    public static void RightArrow(TestByKey testByKey)
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            testByKey();
        }
    }
    public static void UpArrow(TestByKey testByKey)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            testByKey();
        }
    }
    public static void DownArrow(TestByKey testByKey)
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            testByKey();
        }
    }
    public static void C_Down(TestByKey testByKey)
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            testByKey();
        }
    }
    public static void Z_Down(TestByKey testByKey)
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            testByKey();
        }
    }
    public static void Alpha0_Down(TestByKey testByKey)
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            testByKey();
        }
    }
    public static void Alpha1_Down(TestByKey testByKey)
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            testByKey();
        }
    }
    public static void Alpha2_Down(TestByKey testByKey)
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            testByKey();
        }
    }
    public static void Alpha3_Down(TestByKey testByKey)
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            testByKey();
        }
    }
    public static void Alpha4_Down(TestByKey testByKey)
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            testByKey();
        }
    }
    public static void Alpha5_Down(TestByKey testByKey)
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            testByKey();
        }
    }
}
public delegate void  TestByKey();