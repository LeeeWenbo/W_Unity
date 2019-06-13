/*Name:					W_HandHint		
 *Description: 		    用于位置提示的手，循环往前指☞
 *Author:       		李文博
 *Date:         		2018-06-21
 *Copyright(C) 2018 by 	北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_HandPointerHint : MonoBehaviour
{

    [Range(0, 1f)]
    public float zOffset = 0.15f;
    [Range(0, 1f)]
    public float speed = 0.25f;

    public float scale = 0.15f;
    float oriLocalScaleX;
    float oriLocalScaleY;
    float oriLocalScaleZ;

    float oriLocalPositionZ;

    public bool isChangePosition = true;
    public bool isChangeScale = true;
    void Start()
    {
        oriLocalScaleX = transform.localScale.x;
        oriLocalScaleY = transform.localScale.y;
        oriLocalScaleZ = transform.localScale.z;
        oriLocalPositionZ = transform.localPosition.z;
        scale *= oriLocalScaleX;
    }

    void Update()
    {

        if (isChangePosition)
        {
            transform.localPosition =
            new Vector3(transform.localPosition.x, transform.localPosition.y, oriLocalPositionZ - Mathf.PingPong(Time.time * speed, zOffset));
        }


        if (isChangeScale)
        {

            transform.localScale =
    new Vector3
    (oriLocalScaleX + Mathf.PingPong(Time.time * speed * (scale / zOffset), scale),
    oriLocalScaleY + Mathf.PingPong(Time.time * speed * (scale / zOffset), scale),
    oriLocalScaleZ + Mathf.PingPong(Time.time * speed * (scale / zOffset), scale));

        }

    }
}
