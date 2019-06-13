/*Name:		 				U_Dotween	
 *Description: 				这个类意义不大，因为他也没法用他的链式回调。
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using W_Enum;

public static class U_Dotween 
{
    private static  void DotweenSetting(Ease ease,bool autoKill)
    {
        DOTween.defaultEaseType = ease;
        DOTween.defaultAutoKill = autoKill;
    }
      public static void BlendableRotateDegree(Transform trans, Vector3 vector3ToRotate, float duration, Space space = Space.Self, RotateMode rotateMode = RotateMode.FastBeyond360)
    {
        Vector3 localOri = trans.localEulerAngles;
        Vector3 worldOri = trans.eulerAngles;
        Debug.Log("转");
        switch (space)
        {
            case Space.Self: trans.DOLocalRotate(localOri + vector3ToRotate, duration, rotateMode); break;
            case Space.World: trans.DORotate(worldOri + vector3ToRotate, duration, rotateMode); break;
        }
    }
    public static void RotateDegree(Transform trans, W_Axis axis, float degree, float duration, RotateMode rotateMode = RotateMode.FastBeyond360)
    {
        Vector3 localOri = trans.localEulerAngles;
        Vector3 worldOri = trans.eulerAngles;
        Debug.Log("转");
        switch (axis)
        {
            case W_Axis.localX: trans.DOLocalRotate(localOri + new Vector3(degree, 0, 0), duration, rotateMode); break;
            case W_Axis.localY: trans.DOLocalRotate(localOri + new Vector3(0, degree, 0), duration, rotateMode); break;
            case W_Axis.localZ: trans.DOLocalRotate(localOri + new Vector3(0, 0, degree), duration, rotateMode); break;
            case W_Axis.worldX: trans.DORotate(worldOri + new Vector3(degree, 0, 0), duration, rotateMode); break;
            case W_Axis.worldY: trans.DORotate(worldOri + new Vector3(0, degree, 0), duration, rotateMode); break;
            case W_Axis.worldZ: trans.DORotate(worldOri + new Vector3(0, 0, degree), duration, rotateMode); break;
            case W_Axis.none: Debug.Log("请填入转轴"); break;
        }
    }
    public static void RotateDegree(Transform trans,Vector3 vector3ToRotate, float duration, Space space=Space.Self,RotateMode rotateMode = RotateMode.FastBeyond360)
    {
        Vector3 localOri = trans.localEulerAngles;
        Vector3 worldOri = trans.eulerAngles;
        Debug.Log("转");
        switch (space)
        {
            case Space.Self: trans.DOLocalRotate(localOri + vector3ToRotate, duration, rotateMode); break;
            case Space.World: trans.DORotate(worldOri + vector3ToRotate, duration, rotateMode); break;
        }
    }
    public static void MoveLength(Transform trans, Vector3 vector3ToMove, float duration, Space space = Space.Self,bool snap=false)
    {
        Vector3 localOri = trans.localPosition;
        Vector3 worldOri = trans.position;
        Debug.Log("跑");
        switch (space)
        {
            case Space.Self: trans.DOLocalMove(localOri + vector3ToMove, duration, snap); break;
            case Space.World: trans.DOMove(worldOri + vector3ToMove, duration, snap); break;
        }
    }
}
