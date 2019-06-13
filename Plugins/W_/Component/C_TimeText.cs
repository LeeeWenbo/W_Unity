/*Name:					W_NowTime		
 *Description: 		    当前时间，这个跟Text耦合在一起了，有时间拆开，放到一个工具类里。
 *Author:       		李文博
 *Date:         		2018-06-21
 *Copyright(C) 2018 by 	北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class C_TimeText : MonoBehaviour
{

    public Text timeText;
    void Start()
    {
        if (timeText == null)
        {
            timeText = GetComponent<Text>();
        }
        if (System.DateTime.Now.Minute < 10)
        {
            timeText.text = System.DateTime.Now.Hour + ":" + "0" + System.DateTime.Now.Minute;
        }
        else
            timeText.text = System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute;
    }

    float timer = 1;
    bool showMaohao = true;
    int int_hour;
    int int_mintue;
    string str_hour;
    string str_mintue;


    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 1;
            if (showMaohao)
            {
                if (System.DateTime.Now.Minute < 10)
                {
                    timeText.text = System.DateTime.Now.Hour + ":" + "0" + System.DateTime.Now.Minute;
                }
                else
                    timeText.text = System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute;
                showMaohao = false;
            }
            else
            {
                if (System.DateTime.Now.Minute < 10)
                {
                    timeText.text = System.DateTime.Now.Hour + " " + "0" + System.DateTime.Now.Minute;
                }
                else
                    timeText.text = System.DateTime.Now.Hour + " " + System.DateTime.Now.Minute;
                showMaohao = true;
            }

        }
    }
}
