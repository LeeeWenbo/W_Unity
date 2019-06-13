using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OccluderObject : MonoBehaviour
{
    #region Editable Fields

    // Builtin layer reserved for the highlighting
    public static int highlightingLayer = 15;

    // Default transparent cutoff value used for shaders without _Cutoff property
    private static float transparentCutoff = 0.5f;

    #endregion

    #region 暴露字段

    [Header("是否是遮光板，调用OccluderSwitch()")]
    public  bool occluder = true;
    // Occlusion color (DON'T TOUCH THIS!)?
    //private readonly Color occluderColor = new Color(0.0f, 0.0f, 0.0f, 0.005f);
      public static Color occluderColor = new Color32(255, 255, 255, 0);
    //public  Color occluderColor = new Color32(255, 218, 10, 0);
    #endregion

    #region 私有字段


    // Currently used shaders ZWriting state
    private bool zWrite = false;





    #region Private Fields

    // 2 * PI constant required for flashing
    private const float doublePI = 2f * Mathf.PI;

    // Cached materials  ？？
    private List<HighlightingRendererCache> highlightableRenderers;

    // 层缓存数组 Cached layers of highlightable objects
    private int[] layersCache;

    // Need to reinit materials flag
    private bool materialsIsDirty = true;

    // Current materials highlighting color
    private Color currentColor;

    // Transition is active flag
    private bool transitionActive = false;

    // Current transition value
    private float transitionValue = 0f;

    #endregion

    private Material highlightingMaterial
    {
        get { return zWrite ? opaqueZMaterial : opaqueMaterial; }
    }

    // Common (for this component) replacement material for opaque geometry highlighting
    private Material _opaqueMaterial;

    private Material opaqueMaterial
    {
        get
        {
            if (_opaqueMaterial == null)
            {
                _opaqueMaterial = new Material(opaqueShader);
                _opaqueMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            return _opaqueMaterial;
        }
    }

    // Common (for this component) replacement material for opaque geometry highlighting with Z-Buffer writing enabled
    private Material _opaqueZMaterial;

    private Material opaqueZMaterial
    {
        get
        {
            if (_opaqueZMaterial == null)
            {
                _opaqueZMaterial = new Material(opaqueZShader);
                _opaqueZMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            return _opaqueZMaterial;
        }
    }

    // 
    private static Shader _opaqueShader;

    private static Shader opaqueShader
    {
        get
        {
            if (_opaqueShader == null)
            {
                _opaqueShader = Shader.Find("Hidden/Highlighted/StencilOpaque");
            }

            return _opaqueShader;
        }
    }

    // 
    private static Shader _transparentShader;

    public static Shader transparentShader
    {
        get
        {
            if (_transparentShader == null)
            {
                _transparentShader = Shader.Find("Hidden/Highlighted/StencilTransparent");
            }

            return _transparentShader;
        }
    }

    // 
    private static Shader _opaqueZShader;

    private static Shader opaqueZShader
    {
        get
        {
            if (_opaqueZShader == null)
            {
                _opaqueZShader = Shader.Find("Hidden/Highlighted/StencilOpaqueZ");
            }

            return _opaqueZShader;
        }
    }

    // 
    private static Shader _transparentZShader;

    private static Shader transparentZShader
    {
        get
        {
            if (_transparentZShader == null)
            {
                _transparentZShader = Shader.Find("Hidden/Highlighted/StencilTransparentZ");
            }

            return _transparentZShader;
        }
    }

    #endregion

    #region Common
    // Internal class for renderers caching
    private class HighlightingRendererCache
    {
        public Renderer rendererCached;
        public GameObject goCached;
        private Material[] sourceMaterials;
        private Material[] replacementMaterials;
        private List<int> transparentMaterialIndexes;

        // Constructor
        public HighlightingRendererCache(Renderer rend, Material[] mats, Material sharedOpaqueMaterial, bool writeDepth)
        {
            rendererCached = rend;
            goCached = rend.gameObject;
            sourceMaterials = mats;
            replacementMaterials = new Material[mats.Length];
            transparentMaterialIndexes = new List<int>();

            for (int i = 0; i < mats.Length; i++)
            {
                Material sourceMat = mats[i];
                if (sourceMat == null)
                {
                    continue;
                }

                string tag = sourceMat.GetTag("RenderType", true);
                if (tag == "Transparent" || tag == "TransparentCutout")
                {
                    Material replacementMat = new Material(writeDepth ? transparentZShader : transparentShader);
                    if (sourceMat.HasProperty("_MainTex"))
                    {
                        replacementMat.SetTexture("_MainTex", sourceMat.mainTexture);
                        replacementMat.SetTextureOffset("_MainTex", sourceMat.mainTextureOffset);
                        replacementMat.SetTextureScale("_MainTex", sourceMat.mainTextureScale);
                    }

                    replacementMat.SetFloat("_Cutoff",
                        sourceMat.HasProperty("_Cutoff") ? sourceMat.GetFloat("_Cutoff") : transparentCutoff);

                    replacementMaterials[i] = replacementMat;
                    transparentMaterialIndexes.Add(i);
                }
                else
                {
                    replacementMaterials[i] = sharedOpaqueMaterial;
                }
            }
        }

        // Based on given state variable, replaces materials of this cached renderer to the highlighted ones and back
        public void SetState(bool state)
        {
            rendererCached.sharedMaterials = state ? replacementMaterials : sourceMaterials;
        }

        // Sets given color as the highlighting color on all transparent materials for this cached renderer
        public void SetColorForTransparent(Color clr)
        {
            for (int i = 0; i < transparentMaterialIndexes.Count; i++)
            {
                replacementMaterials[transparentMaterialIndexes[i]].SetColor("_Outline", clr);
            }
        }
    }

    // 
    private void OnEnable()
    {
        // Subscribe to highlighting event
        HighlightingEffect.highlightingEvent += UpdateEventHandler;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        // Unsubscribe from highlighting event
        HighlightingEffect.highlightingEvent -= UpdateEventHandler;

        // Clear cached renderers
        if (highlightableRenderers != null)
        {
            highlightableRenderers.Clear();
        }

        // Reset highlighting parameters to default values
        layersCache = null;
        materialsIsDirty = true;
        currentColor = Color.clear;
        transitionActive = false;
        transitionValue = 0f;
        // occluder = false;
        zWrite = false;

        /* 
        // Reset custom parameters of the highlighting
        onceColor = Color.red;
        flashingColorMin = new Color(0f, 1f, 1f, 0f);
        flashingColorMax = new Color(0f, 1f, 1f, 1f);
        flashingFreq = 2f;
        constantColor = Color.yellow;
        */

        if (_opaqueMaterial)
        {
            //xg
            //DestroyImmediate(_opaqueMaterial);
            Destroy(_opaqueMaterial);
        }

        if (_opaqueZMaterial)
        {
            //xg
            //DestroyImmediate(_opaqueZMaterial);
            Destroy(_opaqueZMaterial);
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Materials reinitialization. 
    /// Call this method if your highlighted object changed his materials or child objects.
    /// Can be called multiple times per update - renderers reinitialization will occur only once.
    /// </summary>
    public void ReinitMaterials()
    {
        materialsIsDirty = true;
    }

    /// <summary>
    /// Immediately restore original materials. Obsolete. Use ReinitMaterials() instead.
    /// </summary>
    public void RestoreMaterials()
    {
        Debug.LogWarning("HighlightingSystem : RestoreMaterials() is obsolete. Please use ReinitMaterials() instead.");
        ReinitMaterials();
    }
    
    
    /// <summary>
    /// Enable occluder mode
    /// </summary>
    public void OccluderOn()
    {
        Debug.Log("OccluderOn");
        occluder = true;
    }

    /// <summary>
    /// Disable occluder mode
    /// </summary>
    public void OccluderOff()
    {
        Debug.Log("OccluderOff");
        occluder = false;
    }

    /// <summary>
    /// Switch occluder mode
    /// </summary>
    public void OccluderSwitch()
    {
        occluder = !occluder;
    }

    /// <summary>
    /// Turn off all types of highlighting. 
    /// </summary>
    public void Off()
    {
        Debug.Log("Off");
        // Turn off all types of highlighting
        // Set transition value to 0
        transitionValue = 0f;
        // Stop transition
        transitionActive = false;
    }

    /// <summary>
    /// Destroy this HighlightableObject component.
    /// </summary>
    public void Die()
    {
        Destroy(this);
    }

    #endregion

    #region Private Methods

    //[Header("是否包含带相同脚本的子物体，容易出现阴影，慎点")]
    //public bool isIncludeHighlightableChidren = false;

    /// <summary>
    ///添加的代码，让带有改脚本的子物体不显示
    /// </summary>
    public void HighlightObjectReset()
    {
        MeshRenderer[] mr;
        SkinnedMeshRenderer[] smr;
   
        mr = GetComponentsInChildren<MeshRenderer>();
        smr = GetComponentsInChildren<SkinnedMeshRenderer>();

        CacheRenderers(mr);
        CacheRenderers(smr);
    }



    // Materials initialisation
    private void InitMaterials(bool writeDepth)
    {
        zWrite = writeDepth;

        highlightableRenderers = new List<HighlightingRendererCache>();

        HighlightObjectReset();


        //#if !UNITY_FLASH
        //ClothRenderer[] cr = GetComponentsInChildren<ClothRenderer>();
        //CacheRenderers(cr);
        //#endif
        materialsIsDirty = false;
        currentColor = Color.clear;
    }

    // Cache given renderers properties
    private void CacheRenderers(Renderer[] renderers)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] materials = renderers[i].sharedMaterials;

            if (materials != null)
            {
                highlightableRenderers.Add(new HighlightingRendererCache(renderers[i], materials, highlightingMaterial,
                    zWrite));
            }
        }
    }

    // Update highlighting color to a given value
    private void SetColor(Color c)
    {
        if (currentColor == c)
        {
            return;
        }

        if (zWrite)
        {
            opaqueZMaterial.SetColor("_Outline", c);
        }
        else
        {
            opaqueMaterial.SetColor("_Outline", c);
        }

        for (int i = 0; i < highlightableRenderers.Count; i++)
        {
            highlightableRenderers[i].SetColorForTransparent(c);
        }

        currentColor = c;
    }


    // Calculate new transition value if needed.
    private void PerformTransition()
    {
        if (transitionActive == false)
        {
            return;
        }

        if (Time.timeScale != 0f)
        {
            transitionValue = Mathf.Clamp01(transitionValue);
        }
        else
        {
            return;
        }
    }

    // Highlighting event handler 高亮主方法
    private void UpdateEventHandler(bool trigger, bool writeDepth)
    {
        // Update and enable
        if (trigger)
        {
            // ZWriting state changed?
            if (zWrite != writeDepth)
            {
                materialsIsDirty = true;
            }

            // Initialize new materials if needed
            if (materialsIsDirty)
            {
                InitMaterials(writeDepth);
            }

            if (occluder)
            {
                SetColor(occluderColor);
                PerformTransition();
                if (highlightableRenderers != null)
                {
                    layersCache = new int[highlightableRenderers.Count];
                    for (int i = 0; i < highlightableRenderers.Count; i++)
                    {
                        GameObject go = highlightableRenderers[i].goCached;
                        go.layer = highlightingLayer;
                        highlightableRenderers[i].SetState(true);
                    }
                }
            }



            //五种状态
         //   currentState = (once || flashing || constantly || transitionActive || occluder);
        }

        // Disable
        else
        {
            if (occluder && highlightableRenderers != null)
            {
                for (int i = 0; i < highlightableRenderers.Count; i++)
                {
                    highlightableRenderers[i].goCached.layer = layersCache[i];
                    highlightableRenderers[i].SetState(false);
                }
            }
        }
    }


    #endregion
}