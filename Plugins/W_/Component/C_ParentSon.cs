/*Name:		 				C_ParentSon	
 *Description: 				一键建立和解除父子级关系
 *Author:       			李文博 
 *Date:         			2018-08-12
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ParentSon : MonoBehaviour {

    [Header("该组件用来设置父子级关系")]

    [Header("解散孩子")]
    public bool removerChildren=false;
    bool isSonFlag;
    [Header("建立父级")]
    public bool setParent;
    bool isFatherFlag;

    public Transform parent;
    public void Update()
    {
        //isFather，解除其所有子物体
        U_Bool.BoolTrueMethord
            (removerChildren, ref isSonFlag, () => U_Transform.RemoveChildren(transform));

        U_Bool.BoolTrueMethord
            (setParent, ref isFatherFlag,() => U_Transform.SetParent(transform, parent));
    }
}
