/*Name:		 				C_FollowMouse	
 *Description: 				
 *Author:       			lwb
 *Date:         			2019-06-
 *Copyright(C) 2019 by 		company@zhiwyl.com*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_FollowMouse : MonoBehaviour
{
    public float distance=2;

    private Vector3 mousePosition;
    private Vector3 targetPosition;
    public Vector3 offset = new Vector3(-5,-5,0);
    private void Update()
    {
        mousePosition = Input.mousePosition+ offset;
        targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, distance));
        transform.position = targetPosition;
    }



    void MoveToMouse()
    {
        Vector3 world;
        float speed;
        Vector3 screenpos = Camera.main.WorldToScreenPoint(transform.position);//物体的世界坐标转化成屏幕坐标  
        Vector3 e = Input.mousePosition;//鼠标的位置  
                                        //当点击鼠标左键时  
                                        //if (Input.GetMouseButton(0))
        {
            e.z = screenpos.z;//1.因为鼠标的屏幕 Z 坐标的默认值是0，所以需要一个z坐标  
            world = Camera.main.ScreenToWorldPoint(e);
            speed = 1;
        }
        if (transform.position == world)
        {
            speed = 0;
        }
        transform.LookAt(world); //物体朝向鼠标      
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }





}
