/*Name:		 				C_Camera_Third	
 *Description: 				第三人称摄像头
 *Author:       			lwb
 *Date:         			2019-06-
 *Copyright(C) 2019 by 		company@zhiwyl.com*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Camera_Third : MonoBehaviour
{



    [Header("跟随目标")]
    public Transform target;
    private Vector3 offset; 
    [Header("跟随距离")]
    public float distanceMuti=1f;
     float distance;

    private Vector3[] points = new Vector3[5];//5个点作为摄像机位置的选择

    private Vector3 targetPos;//筛选后的摄像机位置

    void Awake()
    {
        PlayerInit();
        offset = transform.position - target.position;
        distance = offset.magnitude* distanceMuti;
    }

    void FixedUpdate()
    {
        //更新5个点的位置
        points[0] = target.position + offset;
        points[4] = target.position + Vector3.up * distance;

        points[1] = Vector3.Lerp(points[0], points[4], 0.25f);
        points[2] = Vector3.Lerp(points[0], points[4], 0.5f);
        points[3] = Vector3.Lerp(points[0], points[4], 0.75f);
        points[4] = Vector3.Lerp(points[0], points[4], 0.9f);

        targetPos = FindCameraTarget();

        AdjustCamera();
    }
    void PlayerInit()
    {
        if (target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }
    }

    private Vector3 FindCameraTarget()
    {
        Vector3 result = points[points.Length - 1];//头顶位置

        //从低到高遍历
        for (int i = 0; i < points.Length; ++i)
        {
            if (IsHitPlayer(points[i], target.position))
            {
                result = points[i];
                break;
            }
        }

        return result;
    }

    private Ray ray;
    private RaycastHit hit;
    /// <summary>
    /// 从origin发射一条射线检测是否碰到player，
    /// 碰到则表示摄像机在此位置可以看到player
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    bool IsHitPlayer(Vector3 origin, Vector3 target)
    {
        bool result = false;

        Vector3 dir = target - origin;
        ray = new Ray(origin, dir);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Player")
            {
                result = true;
            }
        }
        return result;
    }

    /// <summary>
    /// 调整摄像机位置
    /// </summary>
    void AdjustCamera()
    {

        transform.position = Vector3.Slerp(transform.position, targetPos, Time.deltaTime * 6);

        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 33f);

    }
}

