/*Name:		 				C_HierarchySort	
 *Description: 				按照名称排序，使用方法：右键，排序
 *Author:       			李文博 
 *Date:         			2018-07-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class C_HierarchySort : MonoBehaviour {


     int childCount;
     List<Transform> children = new List<Transform>();
     List<string> childrenName = new List<string>();
     List<int> childrenNumber = new List<int>();


    //1、清除上一次的痕迹
    void ResetBeforeSort()
    {
        childCount = 0;
        children.Clear();
        childrenName.Clear();
        childrenNumber.Clear();
    }
    //2、获取子物体列表
    void GetChildren()
    {
        childCount = transform.childCount;
        for (int i = 0; i<childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }
    }

    //3、获取子物体名称
    void GetChildrenName()
    {
        foreach (Transform trans in children)
        {
            childrenName.Add(trans.name);
        }
    }
    //4、去掉空格
    void ChildrenTrim()
    {
        for (int i = 0; i < childCount; i++)
        {
            children[i].name = childrenName[i].Trim();
        }
    }
    //5、获取子物体后两位名称
    void GetChildrenNumber()
    {
        foreach (string str in childrenName)
        {
            childrenNumber.Add(int.Parse(str.Substring(str.Length - 2, 2)));
        }
    }
    //6、排序
    void SortByNumber()
    {
        for (int i = 0; i < childCount; i++)
        {
            children[i].SetSiblingIndex(childrenNumber[i]-1);
        }
    }
    //7、检查是否排完序
    void CheckOrder()
    {
        for (int i = 0; i < childCount; i++)
        {
            if (children[i].GetSiblingIndex() != childrenNumber[i] - 1)
            {
                SortByNumber();
            }
        }

    }
    //8、删除本脚本
    void RemomoveThis()
    {
        DestroyImmediate(this);
    }
    //9、排序
    void Sort()
    {
        ResetBeforeSort();
        GetChildren();
        GetChildrenName();
        ChildrenTrim();
        GetChildrenNumber();
        SortByNumber();
        CheckOrder();
        //RemomoveThis();
    }
    //10、执行
    [ContextMenu("按最后两位数字排序")]
    public void SortByLastTwoNumber()
    {
        Sort();
    }
}
