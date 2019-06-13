/*Name:		 				C_MoveRotateList	
 *Description: 				一个列表的运动和旋转
 *Author:       			李文博 
 *Date:         			2018-07-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using W_Enum;

public class C_MoveRotateList : MonoBehaviour {

    public W_Axis moveAxis;
    public W_Axis rotateAxis;
    public W_UpdateMode updateMode;
    [Range(-10f, 10f)]
    public float moveSpeed = 0.2f;
    public float rotateTimer = 10;
    public float moveTimer = 10;
    [Range(-10, 10)]
    public float rotateSpeed = 1;
    public List<Transform> objects = new List<Transform>();

    protected virtual void Awake()
    {
        if (objects.Count == 0)
        {
            objects.Add(transform);
        }
    }
    protected virtual void FixedUpdate()
    {
       
        if (updateMode == W_UpdateMode.FixedUpdate)
        {
            MoveAndRotate();
        }
    }
    protected virtual void Update()
    {
        if (updateMode == W_UpdateMode.Update)
        {
            MoveAndRotate();
        }
    }
    protected virtual void LateUpdate()
    {
        if (updateMode == W_UpdateMode.LateUpdate)
        {
            MoveAndRotate();
        }
    }






    bool moveFlag;
    void MoveTimer()
    {
        if (!moveFlag)
        {
            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0)
            {
                moveFlag = true;
                rotateAxis = W_Axis.none;
                moveAxis = W_Axis.none;
            }
        }
    }

    bool ratateFlag;
    void RotateTimer()
    {
        if (!ratateFlag)
        {
            rotateTimer -= Time.deltaTime;
            if (rotateTimer <= 0)
            {
                ratateFlag = true;
                rotateAxis = W_Axis.none;
                moveAxis = W_Axis.none;
            }
        }
    }

    void MoveAndRotate()
    {
        Move();
        Rotate();
    }

    #region 移动
    protected virtual void Move()
    {
        switch (moveAxis)
        {
            case W_Axis.none: break;
            case W_Axis.localX: MoveLocalX(objects); break;
            case W_Axis.localY: MoveLocalY(objects); break;
            case W_Axis.localZ: MoveLocalZ(objects); break;
            case W_Axis.worldX: MoveWorldX(objects); break;
            case W_Axis.worldY: MoveWorldY(objects); break;
            case W_Axis.worldZ: MoveWorldZ(objects); break;
        }
    }
    protected virtual void MoveLocalX(List<Transform> transList)
    {
        foreach (Transform trans in transList)
        {
            trans.transform.Translate(trans.right * 0.001f * moveSpeed, Space.World);
        }
        MoveTimer();
    }
    protected virtual void MoveLocalY(List<Transform> transList)
    {
        foreach (Transform trans in transList)
        {
            trans.transform.Translate(trans.up * 0.001f * moveSpeed, Space.World);
        }
        MoveTimer();
    }
    protected virtual void MoveLocalZ(List<Transform> transList)
    {
        foreach (Transform trans in transList)
        {
            trans.transform.Translate(trans.forward * 0.001f * moveSpeed, Space.World);
        }
        MoveTimer();
    }
    protected virtual void MoveWorldX(List<Transform> transList)
    {
        foreach (Transform trans in transList)
        {
            trans.transform.Translate(Vector3.right * 0.001f * moveSpeed, Space.World);
        }
        MoveTimer();
    }
    protected virtual void MoveWorldY(List<Transform> transList)
    {
        foreach (Transform trans in transList)
        {
            trans.transform.Translate(Vector3.up * 0.001f * moveSpeed, Space.World);
        }
        MoveTimer();
    }
    protected virtual void MoveWorldZ(List<Transform> transList)
    {
        foreach (Transform trans in transList)
        {
            trans.transform.Translate(Vector3.forward * 0.001f * moveSpeed, Space.World);
        }
        MoveTimer();
    }
    #endregion

    #region 旋转
    protected virtual void Rotate()
    {
        switch (rotateAxis)
        {
            case W_Axis.none: break;
            case W_Axis.localX: RotateLocalX(objects); break;
            case W_Axis.localY: RotateLocalY(objects); break;
            case W_Axis.localZ: RotateLocalZ(objects); break;
            case W_Axis.worldX: RotateWorldX(objects); break;
            case W_Axis.worldY: RotateWorldY(objects); break;
            case W_Axis.worldZ: RotateWorldZ(objects); break;
        }
    }

    protected virtual void RotateLocalX(List<Transform> transList)
    {
        foreach (Transform trans in transList)
        {
            trans.Rotate(trans.right * rotateSpeed, Space.World);
        }
        RotateTimer();
    }
    protected virtual void RotateLocalY(List<Transform> transList)
    {
        foreach (Transform trans in transList)
        {
            trans.Rotate(trans.up * rotateSpeed, Space.World);
        }
        RotateTimer();
    }
    protected virtual void RotateLocalZ(List<Transform> transList)
    {
        foreach (Transform trans in transList)
        {
            trans.Rotate(trans.forward * rotateSpeed, Space.World);
        }
        RotateTimer();
    }
    protected virtual void RotateWorldX(List<Transform> transList)
    {
        foreach (Transform trans in transList)
        {
            trans.Rotate(Vector3.right * rotateSpeed, Space.World);
        }
        RotateTimer();
    }
    protected virtual void RotateWorldY(List<Transform> transList)
    {
        foreach (Transform trans in transList)
        {
            trans.Rotate(Vector3.up * rotateSpeed, Space.World);
        }
        RotateTimer();
    }
    protected virtual void RotateWorldZ(List<Transform> transList)
    {
        foreach (Transform trans in transList)
        {
            trans.Rotate(Vector3.forward * rotateSpeed, Space.World);
        }
        RotateTimer();
    }
    #endregion

}