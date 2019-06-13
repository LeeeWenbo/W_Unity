/*Name:					W_Hint		
 *Description: 			对象激活后，过timer秒后自动隐藏
 *Author:       		阿文
 *Date:         		2017-11-21
 *Copyright(C) 2017 by 	北京兆泰源信息技术有限公司*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Hint : MonoBehaviour 
{
        //注意！Awake（）只执行一次！！并不是每次激活该物体后都执行！！
    private void OnEnable()
    {
        
        StartCoroutine(IESetActiveFalse());
    }


    IEnumerator IESetActiveFalse()
    {

        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }

    [Range(1, 3)]
    public float timer = 1.5f;

 
}
