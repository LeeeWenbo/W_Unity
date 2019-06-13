/*Name:		 				U_Camera	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class U_Camera
{

    /// <summary>
    /// 画线
    /// </summary>
    /// <param name="cam"></param>
    /// <param name="distance"></param>
    public static void DrawCorners(Camera cam, float distance)
    {
        Vector3[] corners = GetCorners(cam, distance);
        // for debugging
        Debug.DrawLine(corners[0], corners[1], Color.red);
        Debug.DrawLine(corners[1], corners[3], Color.blue);
        Debug.DrawLine(corners[3], corners[2], Color.green);
        Debug.DrawLine(corners[2], corners[0], Color.yellow);
    }

    public static Vector3 GetCorners_LeftUp(Camera cam, float distance)
    {
        Transform tx = cam.transform;
        float halfFOV = (cam.fieldOfView * 0.5f) * Mathf.Deg2Rad;
        float aspect = cam.aspect;
        float height = distance * Mathf.Tan(halfFOV);
        float width = height * aspect;
        Vector3 corner = tx.position - (tx.right * width);
        corner += tx.up * height;
        corner += tx.forward * distance;
        return corner;
    }
    public static Vector3 GetCorners_RightUp(Camera cam, float distance)
    {
        Transform tx = cam.transform;
        float halfFOV = (cam.fieldOfView * 0.5f) * Mathf.Deg2Rad;
        float aspect = cam.aspect;
        float height = distance * Mathf.Tan(halfFOV);
        float width = height * aspect;
        Vector3 corner = tx.position + (tx.right * width);
        corner += tx.up * height;
        corner += tx.forward * distance;
        return corner;
    }
    public static Vector3 GetCorners_LeftDown(Camera cam, float distance)
    {
        Transform tx = cam.transform;
        float halfFOV = (cam.fieldOfView * 0.5f) * Mathf.Deg2Rad;
        float aspect = cam.aspect;
        float height = distance * Mathf.Tan(halfFOV);
        float width = height * aspect;
        Vector3 corner = tx.position - (tx.right * width);
        corner -= tx.up * height;
        corner += tx.forward * distance;
        return corner;
    }
    public static Vector3 GetCorners_RightDown(Camera cam, float distance)
    {
        Transform tx = cam.transform;
        float halfFOV = (cam.fieldOfView * 0.5f) * Mathf.Deg2Rad;
        float aspect = cam.aspect;
        float height = distance * Mathf.Tan(halfFOV);
        float width = height * aspect;
        Vector3 corner = tx.position + (tx.right * width);
        corner -= tx.up * height;
        corner += tx.forward * distance;
        return corner;
    }

    /// <summary>
    /// 获取四个角
    /// </summary>
    /// <param name="cam"></param>
    /// <param name="distance"></param>
    /// <returns>左上，右上，左下，右下</returns>
    public static Vector3[] GetCorners(Camera cam, float distance)
    {
        Transform tx = cam.transform;
        Vector3[] corners = new Vector3[4];

        float halfFOV = (cam.fieldOfView * 0.5f) * Mathf.Deg2Rad;
        float aspect = cam.aspect;

        float height = distance * Mathf.Tan(halfFOV);
        float width = height * aspect;

        //左上
        corners[0] = tx.position - (tx.right * width);
        corners[0] += tx.up * height;
        corners[0] += tx.forward * distance;

        // 右上
        corners[1] = tx.position + (tx.right * width);
        corners[1] += tx.up * height;
        corners[1] += tx.forward * distance;

        //左下
        corners[2] = tx.position - (tx.right * width);
        corners[2] -= tx.up * height;
        corners[2] += tx.forward * distance;

        //右下
        corners[3] = tx.position + (tx.right * width);
        corners[3] -= tx.up * height;
        corners[3] += tx.forward * distance;

        return corners;
    }

    /// <summary>
    /// 返回上下左右
    /// </summary>
    /// <param name="cam"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public static Vector4 GetCorners_Vector4(Camera cam, float distance)
    {
        Vector3[] corners = GetCorners(cam, distance);
        //Debug.Log("左上" + corners[0]);
        //Debug.Log("右下" + corners[3]);
        Vector4 area = new Vector4(corners[0].z, corners[3].z, corners[0].x, corners[3].x);
        return area;
    }
    /// <summary>
    /// 返回某个高度的视锥的截面积。
    /// </summary>
    /// <param name="cam"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public static Vector4 GetCorners_Area(Camera cam, float distance)
    {
        Vector3[] corners = GetCorners(cam, distance);
        Vector2 area = new Vector2(Mathf.Abs(corners[0].z-corners[3].z), Mathf.Abs(corners[0].x-corners[3].x)) ;
        return area;
    }
    //世界坐标转成摄像机所拍摄到的屏幕坐标        多用来判断UI
    public static Vector3 WorldToScreen(Transform trans)
    {
      return  Camera.main.WorldToScreenPoint(trans.position);
    }

    //世界坐标转换成摄像机的视野坐标       多用来判断是否在视野里
    public static bool WorldToView(Transform trans)
    {
        Vector3 viewVector3 = Camera.main.WorldToViewportPoint(trans.position);
        if (viewVector3.x > 0.1 && viewVector3.x < 1)
        {
            if (viewVector3.y > 0.1 && viewVector3.y < 1)
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }






}