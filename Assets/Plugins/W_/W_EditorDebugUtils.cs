/*Name:		 				W_DebugPosition	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-07-
 *Copyright(C) 2018 by 		智网易联*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_EditorDebugUtils : MonoBehaviour {


    public List<Transform> carriers = new List<Transform>();
    public List<Transform> children = new List<Transform>();


    public Transform[] all;


    //获取场景中的所有物体
    void GetAllObjects()
    {
        all = (Transform[])GameObject.FindObjectsOfType(typeof(Transform)) ;
    }
    //获取场景中携带携带者脚本的物体
    void GetCarrier()
    {
        GetAllObjects();
        for (int i = 0; i <all.Length; i++)
        {
            if (all[i].GetComponent<C_Carrier>())
            {
                carriers.Add(all[i]);
            }
        }
    }
    [ContextMenu("打印携带者的本地坐标和旋转度")]
    void DebugCarrierPositionAndEuler()
    {
        carriers.Clear();
        GetCarrier();
        foreach (Transform carrier in carriers)
        {
            Debug.Log(carrier.name + "         position " + carrier.localPosition + "        euler    " + carrier.localEulerAngles);
        }
    }

    [ContextMenu("打印根目录子物体的本地位置和本地旋转度")]
    public void DebugChildrenLocalPositionAndLocalEuler()
    {
        children= U_Transform.GetSonS(transform);
        foreach (Transform child in children)
        {
            Debug.Log(child.name +"         position "+child.localPosition+"        euler    "+child.localEulerAngles);
        }
    }




}
