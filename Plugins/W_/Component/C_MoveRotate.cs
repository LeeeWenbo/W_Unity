/*Name:		 				W_MoveAndRotateObject	
 *Description: 				移动和旋转物体
 *Author:       			李文博 
 *Date:         			2018-08-03
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using W_Enum;

public class C_MoveRotate : MonoBehaviour
{
    #region Files
    public int locomotionNumber = 0;

    public Transform mTrans;

    public W_Axis moveAxis;
    public W_Axis rotateAxis;
    public W_UpdateMode updateMode;

    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;
    bool moveFlag;
    public bool moving;
    public float moveTimer = 3;
    bool rotateFlag;
    public bool rotating;
    public float rotateTimer = 3;
    #endregion

    #region 周期
    protected virtual void Awake()
    {

        if (!mTrans)
        {
            mTrans = transform;
        }
        GetNumber();
    }

    protected virtual void FixedUpdate()
    {
        if (updateMode == W_UpdateMode.FixedUpdate)
        {
            Move();
            Rotate();
        }
    }
    protected virtual void Update()
    {
        if (updateMode == W_UpdateMode.Update)
        {
            Move();
            Rotate();
        }
    }
    protected virtual void LateUpdate()
    {
        if (updateMode == W_UpdateMode.LateUpdate)
        {
            Move();
            Rotate();
        }
    }
    #endregion




    void GetNumber()
    {
        try
        {
        this.locomotionNumber = int.Parse(gameObject.name.Substring(gameObject.name.Length - 2, 2));
        }catch
        {
            //Debug.Log(e);
            this.locomotionNumber = 0;
        }
    }

    public void SetMove(W_Axis axis, float timer, float speed)
    {
        this.moving = true;
        this.moveFlag = false;
        this.moveAxis = axis;
        this.moveTimer = timer;
        this.moveSpeed = speed;
    }
    public void SetRotate(W_Axis axis, float timer, float speed)
    {
        this.rotating = true;
        this.rotateFlag = false;
        this.rotateAxis = axis;
        this.rotateTimer = timer;
        this.rotateSpeed = speed;
    }

    //旋转和位移同轴运动，旋转和位移的运动时间保持一定的比例
    public virtual void SetMoveAndRotate(W_Axis axis, float moveSpeed, float rotateSpeed, float timer, float rotateToMove = 1)
    {
        this.moving = true;
        this.moveFlag = false;
        this.rotating = true;
        this.rotateFlag = false;
        this.moveAxis = axis;
        this.rotateAxis = axis;
        this.moveTimer = timer;
        this.rotateTimer = timer * rotateToMove;
        this.moveSpeed = moveSpeed;
        this.rotateSpeed = rotateSpeed;
    }
    public void SetMoveAndRotate(W_Axis moveAxis, W_Axis rotateAxis, float moveSpeed, float rotateSpeed, float moveTimer, float rotateTimer)
    {
        this.moving = true;
        this.moveFlag = false;
        this.rotating = true;
        this.rotateFlag = false;
        this.moveAxis = moveAxis;
        this.rotateAxis = rotateAxis;
        this.moveTimer = moveTimer;
        this.rotateTimer = rotateTimer;
        this.moveSpeed = moveSpeed;
        this.rotateSpeed = rotateSpeed;
    }


    public void StopMove()
    {
        this.moveTimer = 0;
    }
    public void StopRotate()
    {
        this.rotateTimer = 0;
    }
    public void Stop()
    {
        StopMove();
        StopRotate();
    }
   

    #region 旋转方法
    protected virtual void Rotate()
    {
        if (!rotating)
            return;
        switch (rotateAxis)
        {
            case W_Axis.none: break;
            case W_Axis.localX: RotateLocalX(mTrans); break;
            case W_Axis.localY: RotateLocalY(mTrans); break;
            case W_Axis.localZ: RotateLocalZ(mTrans); break;
            case W_Axis.worldX: RotateWorldX(mTrans); break;
            case W_Axis.worldY: RotateWorldY(mTrans); break;
            case W_Axis.worldZ: RotateWorldZ(mTrans); break;
        }
    }
    protected virtual void RotateLocalX(Transform trans)
    {
        trans.Rotate(trans.right * rotateSpeed, Space.World);
        RotateTimer();
    }
    protected virtual void RotateLocalY(Transform trans)
    {
        trans.Rotate(trans.up * rotateSpeed, Space.World);
        RotateTimer();
    }
    protected virtual void RotateLocalZ(Transform trans)
    {
        trans.Rotate(trans.forward * rotateSpeed, Space.World);
        RotateTimer();
    }
    protected virtual void RotateWorldX(Transform trans)
    {
        trans.Rotate(Vector3.right * rotateSpeed, Space.World);
        RotateTimer();
    }
    protected virtual void RotateWorldY(Transform trans)
    {
        trans.Rotate(Vector3.up * rotateSpeed, Space.World);
        RotateTimer();
    }
    protected virtual void RotateWorldZ(Transform trans)
    {
        trans.Rotate(Vector3.forward * rotateSpeed, Space.World);
        RotateTimer();
    }
    void RotateTimer()
    {
        if (!rotateFlag)
        {
            rotateTimer -= Time.deltaTime;
            if (rotateTimer <= 0)
            {
                rotateFlag = true;
                rotateTimer = 0;

            }
        }
    }

    #endregion

    #region 移动方法
    protected virtual void Move()
    {
        if (!moving)
            return;
        switch (moveAxis)
        {
            case W_Axis.none: break;
            case W_Axis.localX: MoveLocalX(mTrans); break;
            case W_Axis.localY: MoveLocalY(mTrans); break;
            case W_Axis.localZ: MoveLocalZ(mTrans); break;
            case W_Axis.worldX: MoveWorldX(mTrans); break;
            case W_Axis.worldY: MoveWorldY(mTrans); break;
            case W_Axis.worldZ: MoveWorldZ(mTrans); break;
        }
    }
    protected virtual void MoveLocalX(Transform trans)
    {
        trans.transform.Translate(trans.right * 0.001f * moveSpeed, Space.World);
        MoveTimer();
    }
    protected virtual void MoveLocalY(Transform trans)
    {
        trans.transform.Translate(trans.up * 0.001f * moveSpeed, Space.World);
        MoveTimer();
    }
    protected virtual void MoveLocalZ(Transform trans)
    {
        trans.transform.Translate(trans.forward * 0.001f * moveSpeed, Space.World);
        MoveTimer();
    }
    protected virtual void MoveWorldX(Transform trans)
    {
        trans.transform.Translate(Vector3.right * 0.001f * moveSpeed, Space.World);
        MoveTimer();
    }
    protected virtual void MoveWorldY(Transform trans)
    {
        trans.transform.Translate(Vector3.up * 0.001f * moveSpeed, Space.World);
        MoveTimer();
    }
    protected virtual void MoveWorldZ(Transform trans)
    {
        trans.transform.Translate(Vector3.forward * 0.001f * moveSpeed, Space.World);
        MoveTimer();
    }
    void MoveTimer()
    {
        if (!moveFlag)
        {
            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0)
            {
                moveFlag = true;
                moving = false;
                moveTimer = 0;
            }
        }
    }
    #endregion


    public virtual void _OnComplete(ref int number)
    {
        Debug.Log(gameObject.name +"    "+ number + " 完成");
    }
}
