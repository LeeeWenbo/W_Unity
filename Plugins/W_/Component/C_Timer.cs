/*Name:		 				U_Timer	
 *Description: 				计时器类
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Timer : MonoBehaviour
{

    public static W_Second HalfSecond;
    public static W_Second HalfFixedSecond;
    public static W_Second Second;
    public static W_Second FixedSecond;


    static float fixedTemp=0.5f;
    public static float fixedSecond;
    public static float fixedTime = 0;
    //计时器
    public static float fixedTimer;
    private void FixedUpdate()
    {
        FixedTimer(fixedTimer);


        fixedTemp -= Time.fixedDeltaTime;
        if (fixedTemp<= 0)
        {
            fixedTemp += 0.5f;
            fixedSecond += 0.5f;

            if (null != HalfSecond)
                HalfSecond(runTimer);
        }
        fixedTime = fixedSecond + fixedTemp;
    }

    static float timer = 0.5f;
    public static float second;
    public static float time = 0;
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer += 0.5f;
            second += 0.5f;
        }
        time = second + timer;
    }

    public static float runTimer;
    public static void SetRunTimer(int timer)
    {
        runTimer = timer;
    }

    public static bool FixedTimer(float secondTimer)
    {
        secondTimer -= Time.fixedDeltaTime;
        if (secondTimer <= 0)
        {
            fixedTimer = 99999;
            return true;
        }    
        else
            return false;
    }
}
public delegate void W_Second(float runTimer);