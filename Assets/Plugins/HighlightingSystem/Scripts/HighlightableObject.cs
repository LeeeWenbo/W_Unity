using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighlightableObject : MonoBehaviour
{
    #region Editable Fields

    // Builtin layer reserved for the highlighting
    public static int highlightingLayer =15;

    // Constant highlighting turning on speed
    private static float constantOnSpeed = 4.5f;

    // Constant highlighting turning off speed
    private static float constantOffSpeed = 4f;

    // Default transparent cutoff value used for shaders without _Cutoff property
    private static float transparentCutoff = 0.5f;

    #endregion

    #region 暴露字段

    [Header("是否是遮光板，调用OccluderSwitch()")]
    public bool occluder = false;


    [Header("是否连带子物体")]
    public bool isIncludeChildren = true;



    [Header("一帧，调用On()")]
    public bool once = false;
    public Color onceColor = new Color32(255, 218, 10, 255);

    [Header("常亮，调用ConstantSwitch()")]
    public bool constantly = false;
    [Range(0, 3)]
    public float toConstantSpeed = 3f;
    public Color constantColor = new Color32(255, 218, 10, 255);

    [Header("闪烁，调用FlashSwitch()")]
    public bool flicker = false;
    [Range(0, 5)]
    public float flickerSpeed = 1.8f;
    public Color flickerColorMin = new Color32(0, 0, 10, 0);
    public Color flickerColorMax = new Color32(255, 218, 10, 255);



    #endregion

    #region 私有字段


    // Currently used shaders ZWriting state
    private  bool zWrite = false;

    // Occlusion color (DON'T TOUCH THIS!)?
    //private readonly Color occluderColor = new Color(0.0f, 0.0f, 0.0f, 0.005f);


  //  public static Color occluderColor = new Color32(255, 255, 255, 0);

    public static Color occluderColor = new Color32(255, 218, 10, 0);

    #region Private Fields

    // 2 * PI constant required for flashing
    private const float doublePI = 2f * Mathf.PI;

    // Cached materials  ？？
    private List<HighlightingRendererCache> highlightableRenderers;

    // 层缓存数组 Cached layers of highlightable objects
    private int[] layersCache;

    // Need to reinit materials flag
    private bool materialsIsDirty = true;

    // Current state of highlighting
    private bool currentState = false;

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

    private void OnEnable()
    {
        StartCoroutine(EndOfFrame());
        // Subscribe to highlighting event
        HighlightingEffect.highlightingEvent += UpdateEventHandler;
    }

    // 
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
        currentState = false;
        currentColor = Color.clear;
        transitionActive = false;
        transitionValue = 0f;
        once = false;
        flicker = false;
        constantly = false;
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
            //这个地方放到2018会报错
            //DestroyImmediate(_opaqueMaterial);
            Destroy(_opaqueMaterial);
        }

        if (_opaqueZMaterial)
        {
            //这个地方放到2018会报错
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
    /// Set color for one-frame highlighting mode.
    /// </summary>
    /// <param name='color'>
    /// Highlighting color.
    /// </param>
    public void OnParams(Color color)
    {
        onceColor = color;
    }

    /// <summary>
    /// Turn on one-frame highlighting.
    /// </summary>
    public void On()
    {
        // Highlight object only in this frame
        once = true;
    }

    /// <summary>
    /// Turn on one-frame highlighting with given color.
    /// Can be called multiple times per update, color only from the latest call will be used.
    /// </summary>
    /// <param name='color'>
    /// Highlighting color.
    /// </param>
    public void On(Color color)
    {
        // Set new color for one-frame highlighting
        onceColor = color;
        On();
    }

    /// <summary>
    /// Set flashing parameters.
    /// </summary>
    /// <param name='color1'>
    /// Starting color.
    /// </param>
    /// <param name='color2'>
    /// Ending color.
    /// </param>
    /// <param name='freq'>
    /// Flashing frequency.
    /// </param>
    public void FlashingParams(Color color1, Color color2, float freq)
    {
        flickerColorMin = color1;
        flickerColorMax = color2;
    }

    /// <summary>
    /// Turn on flashing.
    /// </summary>
    public void FlashingOn()
    {
        flicker = true;
    }

    /// <summary>
    /// Turn on flashing from color1 to color2.
    /// </summary>
    /// <param name='color1'>
    /// Starting color.
    /// </param>
    /// <param name='color2'>
    /// Ending color.
    /// </param>
    public void FlashingOn(Color color1, Color color2)
    {
        flickerColorMin = color1;
        flickerColorMax = color2;
        FlashingOn();
    }

    /// <summary>
    /// Turn on flashing from color1 to color2 with given frequency.
    /// </summary>
    /// <param name='color1'>
    /// Starting color.
    /// </param>
    /// <param name='color2'>
    /// Ending color.
    /// </param>
    /// <param name='freq'>
    /// Flashing frequency.
    /// </param>
    public void FlashingOn(Color color1, Color color2, float freq)
    {
        FlashingOn(color1, color2);
    }

    /// <summary>
    /// Turn on flashing with given frequency.
    /// </summary>
    /// <param name='f'>
    /// Flashing frequency.
    /// </param>
    public void FlashingOn(float freq)
    {
        FlashingOn();
    }

    /// <summary>
    /// Turn off flashing.
    /// </summary>
    public void FlashingOff()
    {
        flicker = false;
        transitionValue = 1f;
    }

    /// <summary>
    ///Flash切换，还有一个Constant切换
    /// </summary>
    public void FlashingSwitch()
    {
        flicker = !flicker;
    }

    /// <summary>
    /// Set constant highlighting color.
    /// </summary>
    /// <param name='color'>
    /// Constant highlighting color.
    /// </param>
    public void ConstantParams(Color color)
    {
        constantColor = color;
    }

    /// <summary>
    /// Fade in constant highlighting.
    /// </summary>
    public void ConstantOn()
    {
        // Enable constant highlighting
        constantly = true;
        // Start transition
        transitionActive = true;
    }

    /// <summary>
    /// Fade in constant highlighting with given color.
    /// </summary>
    /// <param name='color'>
    /// Constant highlighting color.
    /// </param>
    public void ConstantOn(Color color)
    {
        // Set constant highlighting color
        constantColor = color;
        ConstantOn();
    }

    /// <summary>
    /// Fade out constant highlighting.
    /// </summary>
    public void ConstantOff()
    {

        // Disable constant highlighting
        constantly = false;
        // Start transition
        transitionActive = true;
    }

    ///调用这个
    /// <summary>
    /// Switch Constant Highlighting.
    /// </summary>
    public void ConstantSwitch()
    {
        // Switch constant highlighting
        constantly = !constantly;
        // Start transition
        transitionActive = true;
    }

    /// <summary>
    /// Turn on constant highlighting immediately (without fading in).
    /// </summary>
    public void ConstantOnImmediate()
    {
        constantly = true;
        // Set transition value to 1
        transitionValue = 1f;
        // Stop transition
        transitionActive = false;
    }

    /// <summary>
    /// Turn on constant highlighting with given color immediately (without fading in).
    /// </summary>
    /// <param name='color'>
    /// Constant highlighting color.
    /// </param>
    public void ConstantOnImmediate(Color color)
    {
        // Set constant highlighting color
        constantColor = color;
        ConstantOnImmediate();
    }

    /// <summary>
    /// Turn off constant highlighting immediately (without fading out).
    /// </summary>
    public void ConstantOffImmediate()
    {
        constantly = false;
        // Set transition value to 0
        transitionValue = 0f;
        // Stop transition
        transitionActive = false;
    }

    /// <summary>
    /// Switch constant highlighting immediately (without fading in/out).
    /// </summary>
    public void ConstantSwitchImmediate()
    {
        constantly = !constantly;
        // Set transition value to the final value
        transitionValue = constantly ? 1f : 0f;
        // Stop transition
        transitionActive = false;
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
        once = false;
        flicker = false;
        constantly = false;
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
        if (isIncludeChildren)
        {
            //不包含带有相同脚本的物体
            //if (!isIncludeHighlightableChidren)
            //{
            //List<MeshRenderer> meshRenderList = new List<MeshRenderer>();
            //if (transform.GetComponent<MeshRenderer>())
            //{
            //    meshRenderList.Add(transform.GetComponent<MeshRenderer>());
            //}

            //List<SkinnedMeshRenderer> skinedRenderList = new List<SkinnedMeshRenderer>();
            //if (transform.GetComponent<SkinnedMeshRenderer>())
            //{
            //    skinedRenderList.Add(transform.GetComponent<SkinnedMeshRenderer>());
            //}

            //Transform[] transforms = GetComponentsInChildren<Transform>();
            //for (int i = 1; i < transforms.Length; i++)
            //{
            //    if (!transforms[i].GetComponent<HighlightableObject>() && transforms[i].GetComponent<MeshRenderer>())
            //    {
            //        meshRenderList.Add(transforms[i].GetComponent<MeshRenderer>());
            //    }

            //    if (!transforms[i].GetComponent<HighlightableObject>() &&
            //        transforms[i].GetComponent<SkinnedMeshRenderer>())
            //    {
            //        skinedRenderList.Add(transforms[i].GetComponent<SkinnedMeshRenderer>());
            //    }
            //}
            //mr = meshRenderList.ToArray();
            //smr = skinedRenderList.ToArray();
            //}
            ////包含带有相同脚本的物体
            //else
            //{
                mr = GetComponentsInChildren<MeshRenderer>();
                smr = GetComponentsInChildren<SkinnedMeshRenderer>();
            //}
        }
        else
        {
            mr = GetComponents<MeshRenderer>();
            smr = GetComponents<SkinnedMeshRenderer>();
        }

        CacheRenderers(mr);
        CacheRenderers(smr);
    }



    // Materials initialisation
    private void InitMaterials(bool writeDepth)
    {
        currentState = false;

        zWrite = writeDepth;

        highlightableRenderers = new List<HighlightingRendererCache>();

        //自己添的片段
        HighlightObjectReset();


        //#if !UNITY_FLASH
        //ClothRenderer[] cr = GetComponentsInChildren<ClothRenderer>();
        //CacheRenderers(cr);
        //#endif

        currentState = false;
        materialsIsDirty = false;
        currentColor = Color.clear;
    }

    // Cache given renderers properties
    private void CacheRenderers(Renderer[] renderers)
    {
        
        for (int i = 0; i < renderers.Length; i++)
        {
            //问题出在这，SharedMaterials不是当前的，是全局的，不会重新生成一个实例
            //Material[] materials = renderers[i].sharedMaterials;
            Material[] materials = renderers[i].materials;
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

    // Set new color if needed
    private void UpdateColors()
    {
        // Don't update colors if highlighting is disabled
        if (currentState == false)
        {
            return;
        }

        if (occluder)
        {
            SetColor(occluderColor);
            return;
        }

        if (once)
        {
            SetColor(onceColor);
            return;
        }

        if (flicker)
        {
             // Flashing frequency is not affected by Time.timeScale
             Color c = Color.Lerp(flickerColorMin, flickerColorMax,
                  Mathf.PingPong(Time.time * flickerSpeed, 1) );
           // ( 0.5f * Mathf.Sin(Time.realtimeSinceStartup * flashingFreq * doublePI) + 0.5f)* flashSpeed);
            SetColor(c);
            return;
        }

        if (transitionActive)
        {
            Color c = new Color(constantColor.r, constantColor.g, constantColor.b, constantColor.a * transitionValue);
            SetColor(c);
            return;
        }
        else if (constantly)
        {
            SetColor(constantColor);
            return;
        }
    }

    // Calculate new transition value if needed.
    private void PerformTransition()
    {
        if (transitionActive == false)
        {
            return;
        }

        float targetValue = constantly ? 1f : 0f;

        // Is transition finished?
        if (transitionValue == targetValue)
        {
            transitionActive = false;
            return;
        }

        if (Time.timeScale != 0f)
        {
            // Calculating delta time untouched by Time.timeScale
            float unscaledDeltaTime = Time.deltaTime / Time.timeScale ;

            // Calculating new transition value
            transitionValue += (constantly ? constantOnSpeed : -constantOffSpeed) * unscaledDeltaTime * toConstantSpeed;
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
            //五种状态
            currentState = (once || flicker || constantly || transitionActive || occluder);


            if (currentState)
            {
                UpdateColors();
                PerformTransition();

                if (highlightableRenderers != null)
                {
                    layersCache = new int[highlightableRenderers.Count];

                    for (int i = 0; i < highlightableRenderers.Count; i++)
                    {
                        //go是所有需要渲染轮廓的物体
                        GameObject go = highlightableRenderers[i].goCached;
                        //fff

                        //黑影问题就出在这，缓存层！专门用来记录父子渲染关系的。
                        // layersCache[i] = go.layer;

                        //让炫光在之前定义好的炫光层 其他的物体，即使没有脚本，只要在这个层，都会有炫光。
                        //这一层所有物体都炫光
                        // temporary set layer to renderable by the highlighting effect camera
                        go.layer = highlightingLayer;
                        highlightableRenderers[i].SetState(true);
                    }
                }

            }
        }

        // Disable
        else
        {
            if (currentState && highlightableRenderers != null)
            {
                for (int i = 0; i < highlightableRenderers.Count; i++)
                {
                    highlightableRenderers[i].goCached.layer = layersCache[i];
                    highlightableRenderers[i].SetState(false);
                }
            }
        }
    }

    IEnumerator EndOfFrame()
    {
        while (enabled)
        {
            yield return new WaitForEndOfFrame();
            // Reset one-frame highlighting state after each HighlightingEffect in the scene has finished rendering
            once = false;
        }
    }

    #endregion
}