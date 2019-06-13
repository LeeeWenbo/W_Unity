/*Name:		 				W_QuickOutline	
 *Description: 			    改编自QuickOutline插件，可包含多网格物体
 *Author:       			李文博 
 *Date:         			2018-06-09
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class W_QuickOutline : MonoBehaviour
{
    #region Files
    //想一想，他之前为什么要用static
    private  HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();

    public W_QuickOutlineMode outlineMode = W_QuickOutlineMode.可见部分轮廓;
    [Range(0f, 30f)]
    public float outlineWidth = 5f;
    public Color outlineColor = new Color32(255, 218, 10, 255);

    //需要添加材质球的Renderer,对于多网格的,就不用了,用他们的孩子代替
    private List<Renderer> singeSubMeshRendererList = new List<Renderer>();

    private Material outlineMaskMaterial;
    private Material outlineFillMaterial;
    List<Material> outlineFillMaterials = new List<Material>();
    List<Material> outlineMaskMaterials = new List<Material>();



    [Tooltip("常亮")]
    public bool isConstantOn;
    [Tooltip("闪烁")]
    public bool isFlashing;

    [Range(1, 30)]
    public int maxFlash = 7;
    [Range(0, 10)]
    public float flashingSpeed = 1;
    bool startFlashFlag = false;
    #endregion
    [Header("是否开启高性能，开启后，顶点越多，越慢")]
    public bool isHighQuality = false;



    #region 周期
    private void Start()
    {
        InstantiateMaterial();
        SplitMutliSubMesh();
        SetSingleSubMeshUV();
        AddMaterial();
        OutlineSelectAndShow();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            OnceOn();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ConstantSwitch();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            FlashingSwitch();
        }
        StateUpdate();
    }

    void StateUpdate()
    {
        if (isConstantOn)
        {
            outlineWidth = 5;
            ChangeInstanceMaterialsWidthAndGolor();
          //  outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
            outlineFillMaterial.SetColor("_OutlineColor", outlineColor);
            isConstantOffFlag = false;
        }
        if (!isConstantOn && !isConstantOffFlag)
        {
            isConstantOffFlag = true;
            outlineWidth = 0;
            ChangeInstanceMaterialsWidthAndGolor();
          //  outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
            outlineFillMaterial.SetColor("_OutlineColor", outlineColor);
        }
        if (isFlashing)
        {
            if (!startFlashFlag)
            {
                startFlashFlag = true;
            }
            outlineWidth = Mathf.PingPong(Time.time * flashingSpeed * maxFlash, maxFlash);
            ChangeInstanceMaterialsWidthAndGolor();
           // outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
            outlineFillMaterial.SetColor("_OutlineColor", outlineColor);
        }
        if (!isFlashing && startFlashFlag)
        {
            startFlashFlag = false;
            ChangeInstanceMaterialsWidthAndGolor();
          //  outlineFillMaterial.SetFloat("_OutlineWidth", beforeFlashingOutlineWideth);
        }
    }
    void OnDestroy()
    {
        DestoryMaterial();
        //测试清空现在是，只要Destory一次再进来，就会报错。
        //registeredMeshes = null;
        //MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        //for (int i = 0; i < meshFilters.Length; i++)
        //{
        //    if (meshFilters[i].mesh.subMeshCount == 1)
        //    {
        //        if (!registeredMeshes.Remove(meshFilters[i].mesh))
        //        {
        //            return;
        //        }
        //    }
        //}
    }
    #endregion


    #region 高亮和闪烁

    public void OnceOn()
    {
        if (!isConstantOn && !isFlashing)
        {
            outlineWidth = 5;
            ChangeInstanceMaterialsWidthAndGolor();
            StartCoroutine(OnceOnEndOfFrame());
        }
    }
    IEnumerator OnceOnEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        // yield return 0;
        outlineWidth = 0;
        ChangeInstanceMaterialsWidthAndGolor();
      //  outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
    }

    bool isConstantOffFlag;
    public void ConstantOn()
    {
        if (isFlashing)
            return;
        isConstantOn = true;
    }
    public void ConstantOff()
    {
        if (isFlashing)
            return;
        isConstantOn = false;
    }
    public void ConstantSwitch()
    {
        if (isConstantOn)
            isConstantOn = false;
        else
            isConstantOn = true;
    }

    public void FlashingOn()
    {
        isFlashing = true;
        startFlashFlag = true;
    }
    public void FlashingOff()
    {
        isFlashing = true;
    }
    public void FlashingSwitch()
    {
        if (isFlashing)
            isFlashing = false;
        else
            isFlashing = true;
    }

    #endregion



    #region 初始化和销毁
    void InstantiateMaterial()
    {
        outlineMaskMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineMask"));
        outlineFillMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineFill"));
        outlineMaskMaterial.name = "QuickOutline_Mask";
        outlineFillMaterial.name = "QuickOutline_Fill";
    }

    //上来先把多网格的物体拆了
    void SplitMutliSubMesh()
    {
        MeshFilter[] oriMeshFilters = GetComponentsInChildren<MeshFilter>();
        for (int i = 0; i < oriMeshFilters.Length; i++)
        {
            if (oriMeshFilters[i].mesh.subMeshCount > 1)
            {
                SplitSubMesh(oriMeshFilters[i]);
            }
        }
    }
    //将拆好之后的物体的所有单网格子物体SetUV（）
    void SetSingleSubMeshUV()
    {

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].mesh.subMeshCount == 1)
            {
                singeSubMeshRendererList.Add(meshFilters[i].GetComponent<Renderer>());
                if (!registeredMeshes.Add(meshFilters[i].mesh))
                {
                    return;
                }
                SetUV(meshFilters[i].mesh);
            }
        }
        //Debug.Log(meshFilters.Length);
    }

    //将多网格的物体拆成多个单网格的子物体
    void SplitSubMesh(MeshFilter multiMeshFilter)
    {
        var mesh = multiMeshFilter.mesh;
        //生成子物体 fff
        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            int[] subMeshTris = mesh.GetTriangles(i);
            GameObject child = new GameObject("QO_subMesh" + i + "  _" + multiMeshFilter.gameObject.name);
            child.AddComponent<MeshFilter>();
            child.GetComponent<MeshFilter>().mesh = CreateMesh(subMeshTris, i, mesh);
            child.AddComponent<MeshRenderer>();
            child.GetComponent<MeshRenderer>().material = multiMeshFilter.gameObject.GetComponent<Renderer>().materials[i];
            child.transform.parent = multiMeshFilter.gameObject.transform;
            child.transform.localPosition = new Vector3(0, 0, 0);
            child.transform.localRotation = Quaternion.identity;
            child.transform.localScale = new Vector3(1, 1, 1);
        }
        multiMeshFilter.gameObject.GetComponent<Renderer>().enabled = false;
    }
    //根据子网格生成物体，从另一个shaderGlow中扣出来的
    Mesh CreateMesh(int[] triangles, int index, Mesh mesh)
    {
        Mesh newMesh = new Mesh();
        newMesh.Clear();
        newMesh.vertices = mesh.vertices;
        newMesh.triangles = triangles;
        newMesh.uv = mesh.uv;
        newMesh.uv2 = mesh.uv2;
        newMesh.uv3 = mesh.uv3;
        newMesh.uv4 = mesh.uv4;
        newMesh.colors = mesh.colors;
        newMesh.subMeshCount = 1;
        newMesh.normals = mesh.normals;
        //AssetDatabase.CreateAsset(newMesh, "Assets/" + mesh.name + "_submesh[" + index + "].asset");
        return newMesh;
    }
    void SetUV(Mesh mesh)
    {
       // Debug.Log(mesh.vertices);
        // Group vertices by location
        var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);
        // Copy normals to a new list
        var smoothNormals = new List<Vector3>(mesh.normals);
        // Debug.Log(mesh.vertices);

        if (isHighQuality)
        {
            // Average normals for grouped vertices
            foreach (var group in groups)
            {
                // Skip single vertices
                if (group.Count() == 1)
                {
                    continue;
                }
                // Calculate the average normal
                var smoothNormal = Vector3.zero;
                foreach (var pair in group)
                {
                    smoothNormal += mesh.normals[pair.Value];
                }
                // Normalize and assign smooth normal to each vertex
                smoothNormal.Normalize();
                foreach (var pair in group)
                {
                    smoothNormals[pair.Value] = smoothNormal;
                }
            }
        }
        // Store smooth normals in UV3
        mesh.SetUVs(3, smoothNormals);


    }


    //将生成的材质球添加到renderers中
    void AddMaterial()
    {
        for (int i = 0; i < singeSubMeshRendererList.Count; i++)
        {
            var materials = singeSubMeshRendererList[i].sharedMaterials.ToList();
            materials.Add(outlineMaskMaterial);
            materials.Add(outlineFillMaterial);
            singeSubMeshRendererList[i].materials = materials.ToArray();
            InstanceMaterial(singeSubMeshRendererList[i]);
        }
        outlineWidth = 0;
    }

    void InstanceMaterial(Renderer meshRenderer)
    {
            //实例化材质球
            foreach (Material mat in meshRenderer.materials)
            {
                if (mat.name.Contains("QuickOutline_Mask"))
                {
                    outlineMaskMaterials.Add(mat);
                }
                if (mat.name.Contains("QuickOutline_Fill"))
                {
                    outlineFillMaterials.Add(mat);
                } 
        }
     
    }
    //轮廓方式选择并执行
    void OutlineSelectAndShow()
    {
        switch (outlineMode)
        {
            case W_QuickOutlineMode.全部轮廓: ToOutlineAll(); break;
            case W_QuickOutlineMode.可见部分轮廓: ToOutlineVisible(); break;
            case W_QuickOutlineMode.隐藏部分轮廓: ToOutlineHidden(); break;
            case W_QuickOutlineMode.轮廓和被遮挡实心轮廓: ToOutlineAndSilhouette(); break;
            case W_QuickOutlineMode.被遮挡实心轮廓: ToSilhouetteOnly(); break;
        }
    }

    //将renderers中的相关材质球移除
    void RemoveMaterial()
    {
        for (int i = 0; i < singeSubMeshRendererList.Count; i++)
        {
            var materials = singeSubMeshRendererList[i].sharedMaterials.ToList();
            materials.Remove(outlineMaskMaterial);
            materials.Remove(outlineFillMaterial);
            singeSubMeshRendererList[i].materials = materials.ToArray();
        }
    }

    //销毁生成的材质球实例
    void DestoryMaterial()
    {
        Destroy(outlineMaskMaterial);
        Destroy(outlineFillMaterial);
    }
    #endregion



    #region 几种高亮方式
    void ChangeInstanceMaterialsWidthAndGolor()
    {
        for (int i = 0; i < outlineFillMaterials.Count; i++)
        {
            outlineFillMaterials[i].SetColor("_OutlineColor", outlineColor);
            outlineFillMaterials[i].SetFloat("_OutlineWidth", outlineWidth);
        }
    }
    void ChangeInstanceMaterialsMaskType()
    {
        for (int i = 0; i < outlineMaskMaterials.Count; i++)
        {
            outlineMaskMaterials[i].SetColor("_OutlineColor", outlineColor);
            outlineFillMaterials[i].SetFloat("_OutlineWidth", outlineWidth);
        }
    }
    void ToOutlineAll()
    {
        for (int i = 0; i < outlineMaskMaterials.Count; i++)
        {
            outlineMaskMaterials[i].SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        }
        for (int i = 0; i < outlineFillMaterials.Count; i++)
        {
            outlineFillMaterials[i].SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        }
        ChangeInstanceMaterialsWidthAndGolor();
    }

    void ToOutlineVisible()
    {
        for (int i = 0; i < outlineMaskMaterials.Count; i++)
        {
            outlineMaskMaterials[i].SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        }
        for (int i = 0; i < outlineFillMaterials.Count; i++)
        {
            outlineFillMaterials[i].SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
        }
        ChangeInstanceMaterialsWidthAndGolor();
    }

    void ToOutlineHidden()
    {
        for (int i = 0; i < outlineMaskMaterials.Count; i++)
        {
            outlineMaskMaterials[i].SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        }
        for (int i = 0; i < outlineFillMaterials.Count; i++)
        {
            outlineFillMaterials[i].SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
        }
        ChangeInstanceMaterialsWidthAndGolor();
    }

    void ToOutlineAndSilhouette()
    {
        for (int i = 0; i < outlineMaskMaterials.Count; i++)
        {
            outlineMaskMaterials[i].SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
        }
        for (int i = 0; i < outlineFillMaterials.Count; i++)
        {
            outlineFillMaterials[i].SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        }
        ChangeInstanceMaterialsWidthAndGolor();
    }

    void ToSilhouetteOnly()
    {
        for (int i = 0; i < outlineMaskMaterials.Count; i++)
        {
            outlineMaskMaterials[i].SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
        }
        for (int i = 0; i < outlineFillMaterials.Count; i++)
        {
            outlineFillMaterials[i].SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
        }
        ChangeInstanceMaterialsWidthAndGolor();
    }
    #endregion

}

public enum W_QuickOutlineMode
{
    全部轮廓,
    可见部分轮廓,
    隐藏部分轮廓,
    轮廓和被遮挡实心轮廓,
    被遮挡实心轮廓
}