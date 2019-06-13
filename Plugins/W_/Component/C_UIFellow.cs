/*Name:		 				W_UIFellowObject	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class C_UIFellow : MonoBehaviour {

    public Transform tarObject;
    public Vector2 offset;

    RectTransform rectTrans;
    public Dictionary<Transform, Vector2> identifyObjectToOffset = new Dictionary<Transform, Vector2>();
    private void Awake()
    {
        rectTrans=GetComponent<RectTransform>();
    }
    private void Update()
    {
        Vector2 screenPos = U_Camera.WorldToScreen(tarObject);
        rectTrans.position = screenPos + offset;
    }
}