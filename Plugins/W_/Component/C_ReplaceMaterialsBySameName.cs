/*Name:		 				C_ReplaceMaterialsBySameName	
 *Description: 				【未完成】本脚本用于复制整理好材质球的模型，给当前物体。当前是将所有带Renderer的全部替换
 *Author:       			李文博 
 *Date:         			2018-09-04
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using W_Enum;

public class C_ReplaceMaterialsBySameName : MonoBehaviour {

    public List<Renderer> selfAndChildrenRenderers_ori;

    public List<Renderer> selfAndChildrenRenderers_tar;
    public void GetRenderer()
    {
        selfAndChildrenRenderers_ori = new List<Renderer>();
        selfAndChildrenRenderers_ori = U_Component.GetTComponent<Renderer>(transform,WE_TransformRange.SelfChildren);
        selfAndChildrenRenderers_tar = new List<Renderer>();
        selfAndChildrenRenderers_tar = U_Component.GetTComponent<Renderer>(brother,WE_TransformRange.SelfChildren);
    }

    //没有renderer组件的算作不存在

    //替换材质球
    public Transform brother;
    public void GetBrother()
    {
        if(null==brother)
        {
            brother = U_Transform.GetCtrlD(transform);
        }
    }

    //只考虑层级，不考虑顺序的
    void Change_WithoutOrderConsider()
    {
        for (int i = 0; i < selfAndChildrenRenderers_ori.Count; i++)
        {
            selfAndChildrenRenderers_ori[i].materials = selfAndChildrenRenderers_tar[i].materials;
        }
    }


    [ContextMenu("获取Renderer")]
    public void Change()
    {
     
        GetBrother();
        GetRenderer();
    }
}
