/*Name:					W_ColorUtils		
 *Description: 			变色工具类。改变一个物体的所有子物体的所有材质至同一颜色，并还原。
 *Author:       		李文博
 *Date:         		2018-06-21
 *Copyright(C) 2018 by 	北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Color
{
    public static void ChangeAllChildAndMaterial(Transform trans, Color tarColor, bool isContainChildren = false)
    {
        SaveOriColors(trans);

        if (isContainChildren)
        {
            Renderer[] renderers = trans.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                //renderer.transform.GetComponent<Renderer>().material.color=tarColor;
                Material[] materials = renderer.transform.GetComponent<Renderer>().materials;
                
                foreach (Material mat in materials)
                {
                    mat.color = tarColor;
                }
            }
        }
        else
        {
            Material[] materials = trans.GetComponent<Renderer>().materials;
            foreach (Material mat in materials)
            {
                    mat.color = tarColor;
            }
        }
    }
    public static Dictionary<Transform, List<Color>> tempOriTransformForColor = new Dictionary<Transform, List<Color>>();


    //颜色备份
    private static void SaveOriColors(Transform transform)
    {
        
        //  tempOriTransformForColor.Clear();
        if (!tempOriTransformForColor.ContainsKey(transform))
        {
            List<Color> oriColors = new List<Color>();

            Renderer[] renderers = transform.GetComponentsInChildren<Renderer>(false);

            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.transform.GetComponent<Renderer>().materials;

                foreach (Material mat in materials)
                {
                    //这个地方,在几种插件的shader里加入了_Color
                    oriColors.Add(mat.color);
                }
            }
            tempOriTransformForColor.Add(transform, oriColors);
        }
    }

    //返回初始材质颜色
    public static void ToOriColor(Transform trans)
    {
        int whichMaterial = 0;
        Renderer[] renderers = trans.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.transform.GetComponent<Renderer>().materials;
            for (int i = 0; i < materials.Length; i++)
            {
                    materials[i].color = tempOriTransformForColor[trans][whichMaterial];
                    whichMaterial += 1;
            }
        }
        //防止这个存的数据太多
      //  tempOriTransformForColor.Remove(trans);
    }
    private void OnDestroy()
    {
        tempOriTransformForColor = null;
    }
}

public enum W_HighLitghMode { 无,颜色变化,QuickOutline, VRTK, HighlightingSystem, }