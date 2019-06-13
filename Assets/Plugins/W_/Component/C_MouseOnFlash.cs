/*Name:		 				W_MouseOnAndTwinkleing	
 *Description: 				闪烁，这个只考虑到了单材质球和单个物体在两种颜色间变化的情况，并且闪烁效果做的不好，有时间重写一个。写一个多物体，在有颜色和无颜色之间变换。永Pingpong
 *Author:       			李文博 
 *Date:         			2017-11-1 
 *Copyright(C) 2017 by 		北京兆泰源信息技术有限公司*/
using UnityEngine;
using System.Collections;

public class C_MouseOnFlash : MonoBehaviour
{
    public float speedToAlarm = 0.8f;
    public float speedToOriginal = 0.8f;
    public Color alarmColor = new Color32(180, 0, 0, 255);
    Color originalColor ;

    bool boolToAlarmColor;
    bool boolToOriginalColor;

    public static Transform whichIsTwinkleing;

    public bool alwaysTwinkleing;
    MeshRenderer transfromMeshRender;
    public GameObject gamDeviceInfomation;

    float tempTime;

    void Awake()
    {
        transfromMeshRender = GetComponent<MeshRenderer>();
        originalColor = GetComponent<MeshRenderer>().material.color;
    }



    public bool boolTwinkle;
    private void OnMouseEnter()
    {

        whichIsTwinkleing = transform;
        boolTwinkle = true;
    }
    private void OnMouseExit()
    {
        whichIsTwinkleing = null;
           boolTwinkle = false;
    }


    void Update()
    {
        tempTime += Time.deltaTime;
        if (alwaysTwinkleing)
        {
            JudeChangeToWhichColor();
            ColorChange(originalColor);
        }

        else if (boolTwinkle)
        {
            JudeChangeToWhichColor();
            ColorChange(originalColor);
        }
        else if (transfromMeshRender.material.color != originalColor)
        {
            
            //foreach (Material mat in materials)
            //{
            //    mat.color = tarColor;
            //}
            transfromMeshRender.material.color = originalColor;
        }

        if (whichIsTwinkleing!=null)
        {
            if (Input.GetMouseButtonDown(0) && whichIsTwinkleing.tag == "变压器")
            {
                if (!gamDeviceInfomation.transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    gamDeviceInfomation.transform.GetChild(0).gameObject.SetActive(true);
                }

            }
        }
        
    }


     


    /// <summary>
    /// 判断往什么方向变
    /// </summary>
    void JudeChangeToWhichColor()
    {
        if (transfromMeshRender.material.color == originalColor)
        {
            boolToAlarmColor = true;
            boolToOriginalColor = false;
        }
        else if (transfromMeshRender.material.color == alarmColor)
        {
            boolToAlarmColor = false;
            boolToOriginalColor = true;
        }
    }
    /// <summary>
    /// 变色方法
    /// </summary>
    /// <param name="targetColor"></param>
    void ColorChange(Color targetColor)
    {

        if (boolToAlarmColor)
        {
            transfromMeshRender.material.color = Color.Lerp(transfromMeshRender.material.color, alarmColor, speedToAlarm);
        }

        else if (boolToOriginalColor)
        {
            transfromMeshRender.material.color = Color.Lerp(transfromMeshRender.material.color, targetColor, speedToOriginal);
        }
    }

}
