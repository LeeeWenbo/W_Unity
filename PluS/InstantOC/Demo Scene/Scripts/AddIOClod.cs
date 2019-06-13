/*Name:		 				AddIOClod	
 *Description: 				
 *Author:       			lwb
 *Date:         			2019-06-
 *Copyright(C) 2019 by 		company@zhiwyl.com*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddIOClod : MonoBehaviour
{

    [ContextMenu("给所有带renderer的物体加上IOClod")]
    public void FindRenderer()
    {
        Renderer[] renderers = transform.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer ren in renderers)
        {
            if (ren.GetComponent<IOClod>() == null)
            {
                ren.gameObject.AddComponent<IOClod>();

            }
            if (ren.GetComponent<Collider>() == null)
            {
                ren.gameObject.AddComponent<MeshCollider>();

            }
        }
    }

    [ContextMenu("销毁所有的IoClod")]

    public void RemoveIOClod()
    {
        IOClod[] renderers = transform.GetComponentsInChildren<IOClod>(true);
        foreach (IOClod ren in renderers)
        {
            DestroyImmediate(ren);
        }
    }


}
