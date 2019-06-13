/*Name:		 				C_CharacterController	
 *Description: 				角色控制器 WASD space ↑↓←→       尚需完善，方向需要改
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class C_CharacterController : MonoBehaviour {

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 9.8f;
    private Vector3 moveDirection = Vector3.zero;

    CharacterController controller;

    protected virtual void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    
    public virtual void Move()
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //从本地方向转换为世界方向
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }
        //让他一直往下掉
        moveDirection.y -= gravity * Time.deltaTime;

        //这种写法是让他一直Move，只不过方向用上边的动态改，速度是0的话，就是无方向。
        controller.Move(moveDirection * Time.deltaTime);
    }

    protected virtual void Update()
    {
        Move();
    }
}
