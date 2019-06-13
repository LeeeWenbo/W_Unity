/*Name:					W_CameraControlQuickOutline		
 *Description: 			QuickOutline插件的摄像头监测
 *Author:       		李文博
 *Date:         		2018-05-25
 *Copyright(C) 2018 by 	北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_CameraControlQuickOutline : MonoBehaviour
{


    private Camera cam;
    private W_QuickOutline quickOutline;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        TargetingRaycast();
    }

    public void TargetingRaycast()
    {
        Transform targetTransform = null;

        if (cam != null)
        {
            RaycastHit hitInfo;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo))
            {
                targetTransform = hitInfo.collider.transform;
            }
        }

        if (targetTransform != null)
        {
            if (targetTransform.GetComponent<W_QuickOutline>() != null)
            {
                targetTransform.GetComponent<W_QuickOutline>().OnceOn();
            }
            if (targetTransform.GetComponent<W_QuickOutline>() != null && Input.GetButtonDown("Fire1"))
            {
                targetTransform.GetComponent<W_QuickOutline>().ConstantSwitch();
            }
            if (targetTransform.GetComponent<W_QuickOutline>() != null && Input.GetButtonDown("Fire2"))
            {
                targetTransform.GetComponent<W_QuickOutline>().FlashingSwitch();
            }
        }
    }
}
