/*Name:		 				U_Sort	
 *Description: 			    各种排序
 *Author:       			李文博 
 *Date:         			2018-09-05
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class U_Sort : MonoBehaviour
{
    //public List<int> oriList = new List<int> { 9, 3, 2, 4, 3, 656, 9, 5, 62, 846, 32, 54, 1, 65, 6, 97, 6516 };
    //public List<int> tarList = new List<int>();

    public void Maopao(List<int> oris, W_SortMode mode = W_SortMode.升序)
    {
        if (mode == W_SortMode.升序)
        {
            for (int i = 0; i < oris.Count - 1; i++)
            {
                for (int j = 0; j < oris.Count - 1 - i; j++)
                {
                    if (oris[j] > oris[j + 1])
                        U_Exchange.ExchangeFromList(ref oris, j);
                }
            }
        }
        else
        {
            for (int i = 0; i < oris.Count - 1; i++)
            {
                for (int j = 0; j < oris.Count - 1 - i; j++)
                {
                    if (oris[j] < oris[j + 1])
                        U_Exchange.ExchangeFromList(ref oris, j);
                }
            }
        }
    }
    public List<int> Maopao_New(List<int> oris, W_SortMode mode = W_SortMode.升序)
    {
        List<int> tars = U_List.CopyList(oris);
        if (mode == W_SortMode.升序)
        {
            for (int i = 0; i < tars.Count - 1; i++)
            {
                for (int j = 0; j < tars.Count - 1 - i; j++)
                {
                    if (tars[j] > tars[j + 1])
                        U_Exchange.ExchangeFromList(ref tars, j);
                }
            }
        }
        else
        {
            for (int i = 0; i < tars.Count - 1; i++)
            {
                for (int j = 0; j < tars.Count - 1 - i; j++)
                {
                    if (tars[j] < tars[j + 1])
                        U_Exchange.ExchangeFromList(ref tars, j);
                }
            }
        }
        return tars;
    }
    public void MaopaoSiki(List<int> oris, W_SortMode mode = W_SortMode.升序)
    {
        bool swapped = true;
        if (mode == W_SortMode.升序)
        {
            do
            {
                swapped = false;
                for (int i = 0; i < oris.Count - 1; i++)
                {
                    if (oris[i] > oris[i + 1])
                    {
                        U_Exchange.ExchangeFromList(ref oris, i);
                        swapped = true;
                    }
                }
            }
            while (swapped);
        }
        else
        {
            do
            {
                swapped = false;
                for (int i = 0; i < oris.Count - 1; i++)
                {
                    if (oris[i] < oris[i + 1])
                    {
                        U_Exchange.ExchangeFromList(ref oris, i);
                        swapped = true;
                    }
                }
            }
            while (swapped);
        }

    }
    public void Maopao_OBJ<T>(List<T> oris,Func<T,T, bool> func, W_SortMode mode = W_SortMode.升序)
    {
        if(mode == W_SortMode.升序)
        {
            for (int i = 0; i < oris.Count - 1; i++)
            {
                for (int j = 0; j < oris.Count - 1 - i; j++)
                {
                    if (func(oris[j], oris[j + 1]))
                    {
                        U_Exchange.ExchangeFromList(ref oris, j);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < oris.Count - 1; i++)
            {
                for (int j = 0; j < oris.Count - 1 - i; j++)
                {
                    if (!func(oris[j], oris[j + 1]))
                    {
                        U_Exchange.ExchangeFromList(ref oris, j);
                    }
                }
            }
        }

    }



  public  Action action;
    [ContextMenu("排序")]
    public void E_Sort()
    {
        //获取它指向的所有的委托
        Delegate[] delegates=action.GetInvocationList();
         foreach(Delegate dele in delegates)
        {
            //执行某一个委托。
            dele.DynamicInvoke();
        }
    }
    public enum W_SortMethord { s01冒泡, s02选择, s03插入, s04希尔, s05归并, s06快速, s07堆,s08计数,s09桶,s10基数 }
    public enum W_SortMode { 升序, 降序 }
    
}
