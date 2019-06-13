/*Name:		 				U_Exchange	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-09-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class U_Exchange
{
    //将列表中的某两位对调,第三个参数不填就是跟下一个换。
    public static void ExchangeFromList<T>(ref List<T> ts, int index1, int index2 = 0)
    {
        if (index2 == 0)
        {
            index2 = index1 + 1;
        }
        T tempT = ts[index1];
        ts[index1] = ts[index2];
        ts[index2] = tempT;

    }

    //交换两个值或者对象实例。
    public static void Exchange<T>(ref T t1, ref T t2)
    {
        T tempT = t1;
        t1 = t2;
        t2 = tempT;
    }

    //交换两个值或者对象实例。
    public static void Exchange<T>(T t1, T t2)
    {
        T tempT = t1;
        t1 = t2;
        t2 = tempT;
    }
}
