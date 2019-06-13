/*Name:		 				W_Slider	
 *Description: 				进度条程序，注意，改进度条下有一个名为handle的做游标，content的做进度。
 *Author:       			李文博 
 *Date:         			2018-07-06
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class C_ProgressBar : MonoBehaviour
{
    private RectTransform handle;
    private RectTransform content;
    private float totalWidth;
    protected virtual void Start()
    {
        totalWidth = transform.GetComponent<RectTransform>().sizeDelta.x;
        handle = transform.Find("handle").GetComponent<RectTransform>();
        content = transform.Find("content").GetComponent<RectTransform>();
        speed =  ((1/speedByTime)/50)* totalWidth;
    }
    [Header("进度条脚本，将填充框命名为content，游标命名为handle。都为Image，并且将他们的锚点起始点设置为LeftMiddle")]
    [Header("开始运行")]
    public bool isRun;
   protected virtual void FixedUpdate()
    {
        Run();
    }
    [Range(1f,20)]
    [Header("几秒跑完")]
    public float speedByTime = 5f;

    float speed;
    float changeHorizontal;
    public float rate;
    protected virtual void Run()
    {
        if (!isRun)
            return;
        rate = content.sizeDelta.x/ totalWidth;
        if (rate >= 1)
        {
            isRun = false;
        }
        changeHorizontal += speed;
        handle.transform.localPosition += new Vector3(speed,0,0);
        content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, changeHorizontal);
        
    }

}
