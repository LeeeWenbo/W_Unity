/*Name:		 				U_AddToT	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class C_AddToT : MonoBehaviour
{



    [ContextMenu("给所有带renderer的物体加上该脚本")]
    public void FindRenderer()
    {
        Renderer[] renderers = transform.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer ren in renderers)
        {
            Debug.Log(ren.name);
            ren.gameObject.AddComponent<C_FindRef>();
        }
    }

   


    [ContextMenu("找到所有挂有某一个脚本的游戏物体")]
    public void ShowTrans()
    {
        List<GameObject> gos = GetAllObjs_Recursion(transform);
        foreach (GameObject go in gos)
        {
            Debug.Log(go);
        }
    }


    //获取一个物体的子物体，无论是否激活
    public static List<GameObject> GetAllObjs_Recursion(Transform parent)//参数：根节点物体的transform
    {
        List<GameObject> objList = new List<GameObject>();
        for (int i = 0; i < parent.childCount; i++) //childCount的数量包括不显示的物体
        {
            Transform temp = parent.GetChild(i);

            objList.Add(temp.gameObject);

            if (temp.childCount > 0)
            {
                GetAllObjs_Recursion(temp);
            }
        }
        return objList;
    }
}
