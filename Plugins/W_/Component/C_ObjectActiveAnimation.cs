/*Name:		 				C_ObjectActiveAnimation	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-11-2
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[DisallowMultipleComponent]
public class C_ObjectActiveAnimation : MonoBehaviour {

    public enum En_ActiveMode { 无,缩放,旋转,播放动画}
    public En_ActiveMode activeMode = En_ActiveMode.缩放;
    public En_ActiveMode inactiveMode = En_ActiveMode.缩放;
    public float duration = 1f;

    protected Vector3 oriScale=new Vector3(2,2,2);
    protected Vector3 oriLocalPostion;
    protected Vector3 oriLocalEuler;
    #region Cycle
    private void Awake()
    {
        GetOriData();
    }
    public virtual void OnEnable()
    {
        Active();
    }

   public virtual void OnDisable()
    {
        InActive();
    }
    #endregion

    protected virtual void GetOriData()
    {
        oriScale = transform.localScale;
        oriLocalPostion = transform.localPosition;
        oriLocalEuler = transform.localEulerAngles;
    }

    protected virtual void Active()
    {
        switch (activeMode)
        {
            case En_ActiveMode.无:break;
            case En_ActiveMode.缩放:  Active_Scale(oriScale, duration); break;
            case En_ActiveMode.旋转: break;
            case En_ActiveMode.播放动画: break;
        }
    }
    protected virtual void InActive()
    {

    }



    public virtual void Active_Scale(Vector3 targetScale,float dur)
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(targetScale, dur);
    }
}
