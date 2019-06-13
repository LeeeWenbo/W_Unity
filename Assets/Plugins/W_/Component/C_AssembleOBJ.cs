/*Name:		 				C_Assemble	
 *Description: 				需要拆装的物体，挂载该组件
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Events;

public class C_AssembleOBJ : MonoBehaviour
{
    [Header("分别在第几组运动")]
    public List<int> groups = new List<int>();

    [Header("同一级有多少个")]
    public List<int> sameOrderCount= new List<int>();

    [HideInInspector]
    public Vector3 oriPostion;
    [HideInInspector]
    public Vector3 oriEuler;
    [Header("目标位置")]
    public List<Vector3> tarPosS=new List<Vector3>();
    [Header("目标角度")]
    public List<Vector3> tarEulerS = new List<Vector3>();

    public Dictionary<int, Transform> DicGroupTarTrans = new Dictionary<int, Transform>();

    public W_AssembleState state = W_AssembleState.装ED;

    public bool disperseOnComplete = false;



    public override string ToString()
    {
        return this.transform.name;
    }
}

public static class C_AssembleExtension
{
    public static C_AssembleOBJ SetAssemble(this C_AssembleOBJ assemble, Transform tarTran, int group,int sameOrderCount)
    {
        assemble.oriPostion = assemble.transform.localPosition;
        assemble.oriEuler = assemble.transform.localEulerAngles;

        assemble.groups.Add(group);
        assemble.sameOrderCount.Add(sameOrderCount);

        assemble.tarPosS.Add(tarTran.localPosition);
        assemble.tarEulerS.Add(tarTran.localEulerAngles);

        assemble.DicGroupTarTrans.Add(group, tarTran);
        return assemble;
    }
}


public enum W_AssembleState { 装ED, 拆ING, 拆ED, 装ING }
