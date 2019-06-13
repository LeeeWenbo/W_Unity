/*Name:		 				U_Bool	
 *Description: 				Bool工具类
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/


using System;

public static class U_Bool
{

    /// <summary>
    /// 前者用来表示后者是否为过真
    /// </summary>
    public static void HaveBeenTrue(ref bool indicateBool, bool tarBool)
    {
        if (indicateBool || tarBool)
            indicateBool= true;
        else
            indicateBool= false;
    }


    /// <summary>
    /// 两个bool互斥，各调用一个
    /// </summary>
    public static void MutexBool(ref bool bool1, ref bool bool2, ref bool boolFlag1, ref bool boolFlag2)
    {
        if (bool1 && !boolFlag1)
        {
            boolFlag2 = false;
            boolFlag1 = true;
            bool2 = false;
        }
        if (bool2 && !boolFlag2)
        {
            boolFlag1 = false;
            boolFlag2 = true;
            bool1 = false;
        }
    }


    /// <summary>
    /// 一个bool调用两个方法，当面板上某一bool值被改变时，调用不同的方法一次
    /// </summary>
    public static void BoolTwoMethord(bool bool1, ref bool bool1Flag,
        MethordByBool trueMethord, MethordByBool falseMethrod)
    {
        if (bool1 && !bool1Flag)
        {
            bool1Flag = true;
            trueMethord();
        }
        else if (!bool1 && bool1Flag)
        {
            bool1Flag = false;
            falseMethrod();
        }
    }
    /// <summary>
    /// 一个bool调用两个方法，当面板上某一bool值被改变时，调用不同的方法一次
    /// </summary>
    public static void BoolTrueMethord(bool bool1, ref bool bool1Flag, MethordByBool trueMethord)
    {
        if (bool1 && !bool1Flag)
        {
            bool1Flag = true;
            trueMethord();
        }
        else if (!bool1 && bool1Flag)
        {
            bool1Flag = false;
        }
    }
    /// <summary>
         /// 一个bool调用两个方法，当面板上某一bool值被改变时，调用不同的方法一次
         /// </summary>
    public static void BoolTrueMethord_Button(ref bool trueBool, ref bool bool1Flag, MethordByBool trueMethord)
    {
        if (trueBool && !bool1Flag)
        {
            bool1Flag = true;
            trueMethord();
        }
        else if (!trueBool && bool1Flag)
        {
            bool1Flag = false;
        }
    }
    
    
    //两个bool互斥，在为真时各自执行一个方法
    public static void MutexBoolTwoMethord
        (ref bool bool1, ref bool boolFlag1, MethordByBool bool1Methord, 
        ref bool bool2, ref bool boolFlag2,MethordByBool bool2Methord)
    {

        if (bool1 && !boolFlag1)
        {
            boolFlag2 = false;
            boolFlag1 = true;
            bool2 = false;
            bool1Methord();
        }
        //后者被牵着所绊，前边一直执行
        if (bool2 && !boolFlag2)
        {
            boolFlag1 = false;
            boolFlag2 = true;
            bool1 = false;
            bool2Methord();
        }
    }
    
    //两个bool互斥，在为真为加都各自执行一个方法
    public static void MutexBoolFourMethord
        (ref bool bool1, ref bool boolFlag1, MethordByBool bool1TrueMethord, MethordByBool bool1FalseMethord,
        ref bool bool2, ref bool boolFlag2,  MethordByBool bool2TrueMethord, MethordByBool bool2FalseMethord)
    {

        if (bool1 && !boolFlag1)
        {
            boolFlag2 = false;
            boolFlag1 = true;
            bool2 = false;
            bool1TrueMethord();
            bool2FalseMethord();
        }
        //后者被牵着所绊，前边一直执行
        if (bool2 && !boolFlag2)
        {
            boolFlag1 = false;
            boolFlag2 = true;
            bool1 = false;
            bool2TrueMethord(); 
            bool1FalseMethord();
        }
    }
}
public delegate void MethordByBool();

public static class U_BoolExtern
{
    public static string ToTinyint(this bool b)
    {
        if (b)
            return "1";
        else
            return "0";
    }
    public static int ToInt(this bool b)
    {
        if (b)
            return 1;
        else
            return 0;
    }

}