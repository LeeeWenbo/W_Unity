/*Name:		 				C_TagChange	
 *Description: 				用于改变子物体tag
 *Author:       			李文博 
 *Date:         			2018-07-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_TagChange : MonoBehaviour {

    [Header("用于改变子物体tag")]
    public string tarTag;
    [ContextMenu("修改子物体和自己的tag")]
	public void ChangeChildrenTag()
    {
      foreach(Transform tran in U_Transform.GetChildren(transform, true))
        {
            tran.tag = tarTag;
        }
    }

}
