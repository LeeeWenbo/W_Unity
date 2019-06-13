/*Name:		 				U_Renderer	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class U_Renderer
{
    //改变材质球
    public static void ChangeOneMaterial(Transform trans, string path)
    {
        BackupMatierial(trans);
        dicHasReplaced[trans] = true;
        trans.GetComponent<Renderer>().material = Resources.Load<Material>(path);
    }

    public static Dictionary<Transform, Material> dicTransMat = new Dictionary<Transform, Material>();
    public static Dictionary<Transform, bool> dicHasReplaced = new Dictionary<Transform, bool>();

    //返回原材质
    public static void BackToOriMaterial(Transform trans)
    {
        if (!dicTransMat.ContainsKey(trans))
        {
            Debug.LogError("未存储该材质");
            return;
        }
        trans.GetComponent<Renderer>().material = dicTransMat[trans];
        dicHasReplaced[trans] = false;

    }
    //备份材质球
    static void BackupMatierial(Transform trans)
    {
       Material mat = trans.GetComponent<Renderer>().material;
        if(!dicTransMat.ContainsKey(trans))
        dicTransMat.Add(trans,mat);
        if (!dicHasReplaced.ContainsKey(trans))
            dicHasReplaced.Add(trans, false);
    }

    //改变多材质
    public static void ChangeMutilsMaterial(Transform trans, string path)
    {
    }

    
    //在Inspector上，某一个bool为真则替换，为假则回归原始
    public static void LoadMatByBool(Transform trans,bool accroding,string name)
    {
        if (accroding)
        {
            if (!dicTransMat.ContainsKey(trans))
            {
                Debug.Log("切换材质球");
                ChangeOneMaterial(trans, "Materials/" + name);
            }
            else if (dicTransMat.ContainsKey(trans) && !dicHasReplaced[trans])
            {
                ChangeOneMaterial(trans, name);
                Debug.Log("再次替换");
            }
        }
        else
        {
            if (dicTransMat.ContainsKey(trans) && dicHasReplaced[trans])
            {
                Debug.Log("返回材质球");
                BackToOriMaterial(trans);
            }
        }
    }

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
}
