/*Name:					C_GetRotateAngle		
*Description: 			获取一个因变的旋转运动的圈数，速度，方向，以及总角度。
*Author:       		    李文博
*Date:         		    2018-06-21
*Copyright(C) 2018 by 	北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class C_GetRotateAngle : MonoBehaviour
{
    [Header("是否开始统计")]
    public bool calculate=true;



    [Header("临界值，超过这个点判断+了一圈")]
    public int linjie = 350;

    public float oriEulerZ;
    public float curEulerZ = 0;
    public float curAngleZ = 0;
    [Range(-1500,1500)]
    public float sumAngleZ = 0;
    public int rounds = 0;
    public W_RotateDirection rotateDirection = W_RotateDirection.停止;
    bool quanshuAddFlag;
    [Header("采样间隔")]
    public float judgeTimeIntval = 0.02f;
    private int littleJudgTimes = 0;
    private float secondTimer;
    private float[] eulers = new float[4];
    private float rotateSpeed_02;
    private float rotateSpeed_13;
    public float rotateSpeed;


    protected virtual void Start()
    {
        oriEulerZ = transform.localEulerAngles.z;
    }

    //当前的角度，curEuler分为[0,ori][ori,360)两种情况
    //要想获得当前的旋转度curAngle的[0,360)，如果是当前角度-ori，则有[-ori,0][0,360-ori)两种情况，代表着一圈
    public CurEulerState curEulerState;

    protected virtual void FixedUpdate()
    {
        if (!calculate)
        {
            return;
        }

        if (isBegin)
        {
            SetCurEulerZ();
            SetCurAngleZ();
            SetSumAngleZ();
        }

        SpeedAndDirectionJudge();
        QuanshuJude();
    }

    //当前欧拉角
    void SetCurEulerZ()
    {
        curEulerZ = transform.localEulerAngles.z;
    }

    //要实现的功能是，当圈数走到一定圈，就不在统计圈数
    //技术难点在于，又启动的时候，如何判断启动了。


    //当前圈的旋转角度
    void SetCurAngleZ()
    {
        curAngleZ = curEulerZ - oriEulerZ;      

        //只有之前四祯里是正转或者反转会进行这一步调整，停止不会。
        if (rotateDirection == W_RotateDirection.正转)
        {
            if (transform.localEulerAngles.z - oriEulerZ <= 0)
            {
                curEulerState = CurEulerState.正_0_ori_调;
                curAngleZ = curEulerZ - oriEulerZ + 360;
            }
            else
                curEulerState = CurEulerState.正ori_360;
        }

        else if (rotateDirection == W_RotateDirection.反转)
        {
            if (transform.localEulerAngles.z - oriEulerZ <= 0)
            {
                curEulerState = CurEulerState.负ori_360_调;
                curAngleZ = curEulerZ - oriEulerZ + 360;
            }
            else
                curEulerState = CurEulerState.负_0_ori;
        }

    }

    //累计旋转角度
    void SetSumAngleZ()
    {
        if (!quanshuAddFlag)
        {
            //非换圈时间
            sumAngleZ = rounds * 360 + curAngleZ;
        }
        else
        {
            if (rotateDirection == W_RotateDirection.正转)
            {
                //换圈时间内，给之前的速度
                sumAngleZ += rotateSpeed;
            }
            else if (rotateDirection == W_RotateDirection.反转)
            {
                //Debug.Log("负向换圈时间");
                //换圈时间内，给之前的速度
                sumAngleZ += rotateSpeed;
            }
        }
    }

    [Header("是否在运动")]
    public bool isBegin;

    //判断速度和方向
    //由于还是存在0和360的突变，设定以judgeTimeIntval秒为间隔，【当前帧】和【judgeTimeIntval秒以前的帧】进行两次差值，
    //两次都差值为正，顺时针；两次差值都为负，逆时针；两次差值为0，停止;两次差值不一样，认定为是出现了突变，维持方向判定
    void SpeedAndDirectionJudge()
    {
        secondTimer += Time.deltaTime;
        if (secondTimer>=judgeTimeIntval)
        {
            eulers[littleJudgTimes] = transform.eulerAngles.z;
            littleJudgTimes += 1;
            secondTimer = 0;
            if (littleJudgTimes>=4)
            {
                isBegin = true;

                littleJudgTimes = 0;
                rotateSpeed_02 = eulers[1] - eulers[0];
                rotateSpeed_13 = eulers[3] - eulers[2];
                if (Mathf.Abs(rotateSpeed_02 - rotateSpeed_13) < 5)
                {
                    rotateSpeed = (rotateSpeed_02 + rotateSpeed_13) /2f;
                }

                if (rotateSpeed > 0.1f)
                    rotateDirection = W_RotateDirection.正转;
                else if (rotateSpeed < -0.1f)
                    rotateDirection = W_RotateDirection.反转;
                else
                    rotateDirection = W_RotateDirection.停止;
                eulers[0] = 0;
                eulers[1] = 0;
                eulers[2] = 0;
                eulers[3] = 0;
            }
        }
    }

    void QuanshuJude()
    {
        if (curAngleZ >= linjie && !quanshuAddFlag)
        {
            if (rotateDirection == W_RotateDirection.正转)
            {
                rounds += 1;
            }
            else if (rotateDirection == W_RotateDirection.反转)
            {
                sumAngleZ -= 360;
                rounds -= 1;
            }
            
            quanshuAddFlag = true;
            StartCoroutine(FlagReset());
        }
    }

    /// <summary>
    /// 清除加减圈标志位
    /// </summary>
    /// <returns></returns>
    IEnumerator FlagReset()
    {
        if (rotateSpeed == 0)
        {
            rotateSpeed = 1;
        }
        //换圈flag时间，在这个时间内，只允许换一次。
        yield return new WaitForSeconds((0.2f / Mathf.Abs(rotateSpeed)));
        quanshuAddFlag = false;
    }
}
public enum W_RotateDirection { 停止, 正转, 反转, 忽略 }
public enum CurEulerState { 正ori_360, 正_0_ori_调, 负ori_360_调, 负_0_ori };