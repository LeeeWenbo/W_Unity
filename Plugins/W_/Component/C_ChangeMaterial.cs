/*Name:		 				W_ChangeMaterialByBool	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ChangeMaterial : MonoBehaviour
{


    [Header("请将材质球放到Resources/Materials下")]
    public string matName;

    public Material DefaultMaterial;
    [ContextMenu("改变材质球")]
    public void ChangeMaterial()
    {
        U_Renderer.LoadMatByBool(transform, true, "Materials/" + matName);
    }

    [ContextMenu("移除所有Shaderd材质")]
    public void RemoveAllShaderMaterial()
    {
        if (null == DefaultMaterial)
            DefaultMaterial = new Material(Shader.Find("Standard"));
        List<Renderer> renderers = U_Component.GetChildrenComponents<Renderer>(transform, true);
        foreach (Renderer ren in renderers)
        {
            if(ren.materials.Length<2)
            {
                ren.material = DefaultMaterial;
            }
            else
            {
                for (int i = 0; i < ren.sharedMaterials.Length; i++)
                {
                    ren.sharedMaterials[i].CopyPropertiesFromMaterial(DefaultMaterial);
                }
            }
        }
    }
}
