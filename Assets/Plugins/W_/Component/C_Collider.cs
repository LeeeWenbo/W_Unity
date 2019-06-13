using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Collider : MonoBehaviour
{

    [ContextMenu("MeshRenderer勾选Convex")]
    public void MeshRendererConvex()
    {
        List<MeshCollider> colliders = U_Component.GetChildrenComponents<MeshCollider>(transform, true);
        foreach (MeshCollider collider in colliders)
        {
            collider.convex = true;
        }
    }

    [ContextMenu("MeshRenderer变成BoxCollider(勾选了Convex不用)")]
    public void MeshRendererToBoxRenderer()
    {
        List<MeshCollider> colliders = U_Component.GetChildrenComponents<MeshCollider>(transform, true);
        foreach (MeshCollider collider in colliders)
        {
            if (!collider.convex)
            {
                if (collider.transform.localScale.x > 0 && collider.transform.localScale.y > 0 && collider.transform.localScale.z > 0)
                {
                    collider.gameObject.AddComponent<BoxCollider>();
                    DestroyImmediate(collider);
                }
            }
        }
    }

    [ContextMenu("给带有Renderer的所有子物体添加BoxCollider（已经有Collider的不加）")]
    public void AddBoxColliderToObjHaveRenderer()
    {
        List<Renderer> renderers = U_Component.GetChildrenComponents<Renderer>(transform, true);
        foreach (Renderer renderer in renderers)
        {
            if (renderer.gameObject.GetComponent<Collider>()==null)
            {
                renderer.gameObject.AddComponent<BoxCollider>();
            }
        }
    }

    [ContextMenu("给带有Renderer的所有子物体添加MeshCollider（已经有Collider的不加）")]
    public void AddMeshColliderToObjHaveRenderer()
    {
        List<Renderer> renderers = U_Component.GetChildrenComponents<Renderer>(transform, true);
        foreach (Renderer renderer in renderers)
        {
            if (renderer.gameObject.GetComponent<Collider>() == null)
            {
                renderer.gameObject.AddComponent<MeshCollider>();
            }
        }
    }

    [ContextMenu("去掉所有的Collider")]
    public void RemoveAllCollider()
    {
        List<Renderer> renderers = U_Component.GetChildrenComponents<Renderer>(transform, true);
        foreach (Renderer renderer in renderers)
        {
            if (renderer.gameObject.GetComponent<Collider>()!= null)
            {
                DestroyImmediate(renderer.gameObject.GetComponent<Collider>());
            }
        }
    }

}
