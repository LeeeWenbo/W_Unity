/*Name:					W_EmissionChange		
 *Description: 		    VIVE教学场景，shader自发光，闪烁
 *Author:       		李文博
 *Date:         		2018-06-21
 *Copyright(C) 2018 by 	北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class C_MatEmission : MonoBehaviour {

    public Color originalColor;
    public Color alarmColor = new Color32(185,153,53,255);


    public bool flash;
    bool stopTwinkleingFlag;
    [Range(0, 5)]
    public float twinkeSpeed=2f;

    void Update () {
        if (flash)
        {
            GetComponent<MeshRenderer>().material.SetColor
      ("_EmissionColor", Color.Lerp(originalColor, alarmColor, Mathf.PingPong(Time.time* twinkeSpeed, 1f)));
            stopTwinkleingFlag = true;
        }
        else
        {
            if (stopTwinkleingFlag)
            {
                stopTwinkleingFlag = false;
                GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", originalColor);
            }
        }
    }

}
