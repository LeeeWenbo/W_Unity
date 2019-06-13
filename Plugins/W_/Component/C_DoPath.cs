/*Name:		 				W_DoPath	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-07-10
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class W_DoPath : MonoBehaviour {

    public bool isMovePath;

    private bool movePathFlag;
    private void FixedUpdate()
    {
        if (isMovePath&& !movePathFlag)
        {
            movePathFlag = true;

            GetComponent<DOTweenPath>().DOPlay();
        }
    }

}
