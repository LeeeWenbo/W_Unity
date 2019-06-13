/*Name:		 				C_MovePingpong	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-07-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_MovePingpong : MonoBehaviour {

    public float range = 0.5f;
    public float speed = 0.25f;
    public float highFloat;
    public void FixedUpdate()
    {
        highFloat= Mathf.PingPong(Time.time* speed, range);
        transform.localPosition = new Vector3(transform.localPosition.x, highFloat, transform.localPosition.z);
    }

}
