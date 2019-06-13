/*Name:		 				Assemblage	
 *Description: 				本脚本适用于相互独立的物体的拆分。
 *Author:       			李文博 
 *Date:         			2018-06-19
 * 使用方法：
 * 1、改脚本挂在需要拆解的物体的父物体A上，其需要被拆分的子物体，挂一个空脚本AssembledObject，用于区分。
 * 2、Ctrl+D复制该父物体，以保证新物体B的名字为父物体名称+(1)。
 * 3、任意拖拽子物体，以满足需求。
 * 4、右键父物体A上的本脚本，选最后一项，即将目标位置，和原始位置都保存。本步可以多次执行用以修改。
 * 5、通过控制两个public bool 决定是否怎么拆或者怎么装。
 * 
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class C_Assemblage_OneByOne : MonoBehaviour {

    public Transform[] tarTransforms;
    public Vector3[] oriPos;
    public Vector3[] tarPos;
    public W_TranMoveMode moveMode = W_TranMoveMode.渐慢;

    [Header("点击一键拆解")]
    public bool isDisAssembleMoving;
    [Header("点击一键组装")]
    public bool isAssembleMoving;

    [Range(0.5f,5f)]
    public float moveSpeed = 1;
    public int moveIndex = 0;
    public float rate;

    private void FixedUpdate()
    {
        DisAssembleFunction();
        AssembleFunction();
    }
    float rateLimit;
    //如果不是按比例算，那么rate并不是=1就停
    void DisAssembleFunction()
    {
        if (isDisAssembleMoving)
        {
            TransMove(tarTransforms[moveIndex],oriPos[moveIndex],tarPos[moveIndex]);

            rate += 0.02f * moveSpeed;
            if (rate >= rateLimit)
            {
                moveIndex += 1;
                rate = 0;
                if (moveIndex >= tarTransforms.Length)
                {
                    moveIndex = tarTransforms.Length - 1;
                    isDisAssembleMoving = false;
                }
            }
        }
    }
    void AssembleFunction()
    {
        if (isAssembleMoving)
        {
            TransMove(tarTransforms[moveIndex], tarPos[moveIndex], oriPos[moveIndex]);

            rate += 0.02f * moveSpeed;
            if (rate >= rateLimit)
            {
                moveIndex -= 1;
                rate = 0;
                if (moveIndex <0)
                {
                    moveIndex = 0;
                    isAssembleMoving = false;
                }
            }
        }
    }

    void TransMove(Transform moveTrans, Vector3 oriPosition, Vector3 tarPosition)
    {
        if (moveMode == W_TranMoveMode.匀速)
        {
            rateLimit = 1.1f;
            moveTrans.position = Vector3.Lerp(oriPosition, tarPosition, rate);
        }

        else
        {
            rateLimit = 0.8f;
            moveTrans.position = Vector3.Lerp(moveTrans.position, tarPosition, rate);
        }
    }



    #region Editor初始化

    [ContextMenu("保存目标位置,需先复制一个")]
    void SaveTarPos()
    {
        ResetTar();
        InitMoveObject();
        SaveOriPos();
        InitTarPos();
    }

    void InitMoveObject()
    {
        //先找到需要变换的东西
        int assembleObjectCount = GetComponentsInChildren<C_AssembleOBJ>().Length;
        tarTransforms = new Transform[assembleObjectCount];
        for (int i = 0; i < assembleObjectCount; i++)
        {
            tarTransforms[i] = GetComponentsInChildren<C_AssembleOBJ>()[i].transform;
        }
    }
    void SaveOriPos()
    {
        //备份需要变换的东西
        int assembleObjectCount = GetComponentsInChildren<C_AssembleOBJ>().Length;
        oriPos = new Vector3[assembleObjectCount];
        for (int i = 0; i < assembleObjectCount; i++)
        {
            oriPos[i] = GetComponentsInChildren<C_AssembleOBJ>()[i].transform.position;
        }
    }
    void InitTarPos()
    {
        //再找到需要记录位置的
        C_AssembleOBJ[] assembleObjects = GameObject.Find(name + " (1)").GetComponentsInChildren<C_AssembleOBJ>();

        this.tarPos = new Vector3[assembleObjects.Length];
        for (int i = 0; i < assembleObjects.Length; i++)
        {
            this.tarPos[i] = assembleObjects[i].transform.position;
        }
    }
    private void ResetTar()
    {
        //清空已保存的
        this.tarTransforms = null;
        this.tarPos = null;
        this.oriPos = null;
    }
    #endregion
}
public enum W_TranMoveMode { 渐慢, 匀速 }