/*Name:		 				W_JumpWalk	
 *Description: 				跳步前行
 *Author:       			李文博 
 *Date:         			2018-11-13
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_JumpWalk : MonoBehaviour {

    #region File
    [Header("行走的物体")]
    public GameObject obj;
    protected Rigidbody rig;
    [Header("地面名称")]
    public string groundName = "Terrain";
    [Range(50, 300)]
    [Header("整体力度")]
    public float floatForce = 270f;
    [Range(0, 5)]
    [Header("弹跳高度")]
    public float height = 0.5f;
    [Header("往后跳力度折损比")]
    public float backRate = 0.7f;
    [Header("往左右跳力度折损比")]
    public float leftRightRate = 0.7f;
    [Header("是否冻住轴")]
    public bool freezeRotation = true;
    [Header("是否在地面上")]
    public bool isOnGround;

    #endregion

    #region Cycle
    protected virtual void Awake()
    {
        ObjInit();
    }
    protected virtual void Update()
    {
        WalkeUpdate();
    
    }
    #endregion

    public virtual void WalkeUpdate()
    {
        PC_KeyDetect_ForMove();
    }

    public virtual void PC_KeyDetect_ForMove()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (isOnGround)
            {
                Vector3 tranForward = transform.forward + new Vector3(0, height, 0);
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    tranForward += transform.right;
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    tranForward += -transform.right;
                }
                rig.AddForce(tranForward * floatForce, ForceMode.Force);
                isOnGround = false;
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (isOnGround)
            {
                Vector3 tranBack = -transform.forward + new Vector3(0, height, 0);
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    tranBack += transform.right;
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    tranBack += -transform.right;
                }
                rig.AddForce(tranBack * floatForce * backRate, ForceMode.Force);
                isOnGround = false;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (isOnGround)
            {
                Vector3 tranRight = transform.right + new Vector3(0, height, 0);
                rig.AddForce(tranRight * floatForce * leftRightRate, ForceMode.Force);
                isOnGround = false;
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (isOnGround)
            {
                Vector3 tranLeft = -transform.right + new Vector3(0, height, 0);
                rig.AddForce(tranLeft * floatForce * leftRightRate, ForceMode.Force);
                isOnGround = false;
            }
        }
    }
    protected virtual void ObjInit()
    {
        if(null== obj)
        {
            obj = gameObject;
        }
        if (null == obj.GetComponent<Rigidbody>())
        {
            obj.AddComponent<Rigidbody>();
        }
        rig = obj.GetComponent<Rigidbody>();
        rig.freezeRotation = freezeRotation;
    }
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == groundName)
        {
            isOnGround = true;
        }
        else if(string.IsNullOrEmpty(groundName))
        {
            isOnGround = true;
        }
    }
    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.transform.name == groundName)
        {
            isOnGround = false;
        }
        else if (string.IsNullOrEmpty(groundName))
        {
            isOnGround = false;
        }
    }

}