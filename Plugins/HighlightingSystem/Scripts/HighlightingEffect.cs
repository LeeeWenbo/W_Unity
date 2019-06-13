using UnityEngine;

// Delegate for the highlighting event
public delegate void HighlightingEventHandler(bool state, bool zWrite);

[RequireComponent(typeof(Camera))]
public class HighlightingEffect : MonoBehaviour
{
	// Event used to notify highlightable objects to change their materials before rendering to the highlighting buffer begins
	public static event HighlightingEventHandler highlightingEvent;

    #region Inspector Fields

    // Blurring intensity for the blur material
    [Range(0.2f, 2)]
    public float _blurIntensity = 0.7f;

    // Blur iterations 
    [Header("光晕迭代次数，就是层数 光晕强度越大，该值最好越大")]
    [Range(0, 10)]
    public int iterations = 5;

    // Stencil (highlighting) buffer size downsample factor 
    [Header("整体光晕厚度,该值越大越模糊")]
    [Range(1, 5)]
    public int _downsampleFactor =1;

    [Header("光晕分散程度，源码=blurMinSpread + iteration * blurSpread")]
    [Range(0.1f, 3f)]
    public float w_BlurSpread = 0.8f;


    // Stencil (highlighting) buffer depth      作用未知 改成了私有
    private int stencilZBufferDepth = 0;




	// These properties available only in Editor - we don't need them in standalone build
	#if UNITY_EDITOR
	// Z-buffer writing state getter/setter
	public bool stencilZBufferEnabled
	{
		get
		{
			return (stencilZBufferDepth > 0);
		}
		set
		{
			if (stencilZBufferEnabled != value)
			{
				stencilZBufferDepth = value ? 16 : 0;
			}
		}
	}
	
	// Downsampling factor getter/setter
	public int downsampleFactor
	{
		get
		{
			if (_downsampleFactor == 1)
			{
				return 0;
			}
			if (_downsampleFactor == 2)
			{
				return 1;
			}
			return 2;
		}
		set
		{
			if (value == 0)
			{
				_downsampleFactor = 1;
			}
			if (value == 1)
			{
				_downsampleFactor = 2;
			}
			if (value == 2)
			{
				_downsampleFactor = 4;
			}
		}
	}
	
	// Blur alpha intensity getter/setter

        //光照强度
        //fff
	public float blurIntensity
	{
		get
		{
			return _blurIntensity;
		}
		set
		{
			if (_blurIntensity != value)
			{
				_blurIntensity = value;
				if (Application.isPlaying)
				{
					blurMaterial.SetFloat("_Intensity", _blurIntensity);
				}
			}
		}
	}
	#endif
	#endregion












	#region Private Fields
	// Highlighting camera layers culling mask
	private int layerMask = (1 << HighlightableObject.highlightingLayer);
	
	// This GameObject reference
	private GameObject go = null;
	
	// Camera for rendering stencil buffer GameObject
	private GameObject shaderCameraGO = null;
	
	// Camera for rendering stencil buffer
	private Camera shaderCamera = null;
	
	// RenderTexture with stencil buffer
	private RenderTexture stencilBuffer = null;
	
	// Camera reference
	private Camera refCam = null;
	
	// Blur Shader
	private static Shader _blurShader;
	private static Shader blurShader
	{
		get
		{
			if (_blurShader == null)
			{
				_blurShader = Shader.Find("Hidden/Highlighted/Blur");
			}
			return _blurShader;
		}
	}
	
	// Compositing Shader
	private static Shader _compShader;
	private static Shader compShader 
	{
		get
		{
			if (_compShader == null)
			{
				_compShader = Shader.Find("Hidden/Highlighted/Composite");
			}
			return _compShader;
		}
	}
	
	// Blur Material
	private static Material _blurMaterial = null;
	private static Material blurMaterial
	{
		get
		{
			if (_blurMaterial == null)
			{
				_blurMaterial = new Material(blurShader);
				_blurMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return _blurMaterial;
		}
	}
	
	// Compositing Material
	private static Material _compMaterial = null;
	private static Material compMaterial
	{
		get
		{
			if (_compMaterial == null)
			{
                //去掉这两句，就是全屏的黑屏
                _compMaterial = new Material(compShader);
                _compMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
			return _compMaterial;
		}
	}
	#endregion
	
	
	void Awake()
	{
		go = gameObject;
		refCam = GetComponent<Camera>();
	}
	
	void OnDisable()
	{
		if (shaderCameraGO != null)
		{
            //xg
			//DestroyImmediate(shaderCameraGO);
            Destroy(shaderCameraGO);
        }

        if (_blurShader)
		{
			_blurShader = null;
		}
		
		if (_compShader)
		{
			_compShader = null;
		}
		
		if (_blurMaterial)
		{
            //xg
            //DestroyImmediate(_blurMaterial);
            Destroy(_blurMaterial);

        }

        if (_compMaterial)
		{
            //xg
            //DestroyImmediate(_compMaterial);
            Destroy(_compMaterial);
        }

        if (stencilBuffer != null)
		{
			RenderTexture.ReleaseTemporary(stencilBuffer);
			stencilBuffer = null;
		}
	}
	
	void Start()
	{
        // Disable if Image Effects is not supported
        if (!SystemInfo.supportsImageEffects)
        {
            Debug.LogWarning("HighlightingSystem : Image effects is not supported on this platform! Disabling.");
            this.enabled = false;
            return;
        }
        
        // Disable if required Render Texture Format is not supported
        if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32))
        {
            Debug.LogWarning("HighlightingSystem : RenderTextureFormat.ARGB32 is not supported on this platform! Disabling.");
            this.enabled = false;
            return;
        }

        // Disable if HighlightingStencilOpaque shader is not supported
        if (!Shader.Find("Hidden/Highlighted/StencilOpaque").isSupported)
        {
            Debug.LogWarning("HighlightingSystem : HighlightingStencilOpaque shader is not supported on this platform! Disabling.");
            this.enabled = false;
            return;
        }

        // Disable if HighlightingStencilTransparent shader is not supported
        if (!Shader.Find("Hidden/Highlighted/StencilTransparent").isSupported)
        {
            Debug.LogWarning("HighlightingSystem : HighlightingStencilTransparent shader is not supported on this platform! Disabling.");
            this.enabled = false;
            return;
        }

        // Disable if HighlightingStencilOpaqueZ shader is not supported
        if (!Shader.Find("Hidden/Highlighted/StencilOpaqueZ").isSupported)
        {
            Debug.LogWarning("HighlightingSystem : HighlightingStencilOpaqueZ shader is not supported on this platform! Disabling.");
            this.enabled = false;
            return;
        }

        // Disable if HighlightingStencilTransparentZ shader is not supported
        if (!Shader.Find("Hidden/Highlighted/StencilTransparentZ").isSupported)
        {
            Debug.LogWarning("HighlightingSystem : HighlightingStencilTransparentZ shader is not supported on this platform! Disabling.");
            this.enabled = false;
            return;
        }

        // Disable if HighlightingBlur shader is not supported
        if (!blurShader.isSupported)
        {
            Debug.LogWarning("HighlightingSystem : HighlightingBlur shader is not supported on this platform! Disabling.");
            this.enabled = false;
            return;
        }

        // Disable if HighlightingComposite shader is not supported
        if (!compShader.isSupported)
        {
            Debug.LogWarning("HighlightingSystem : HighlightingComposite shader is not supported on this platform! Disabling.");
            this.enabled = false;
            return;
        }

        // Set the initial intensity in blur shader
        blurMaterial.SetFloat("_Intensity", _blurIntensity);
    }


	public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
	{
        blurMaterial.SetFloat("_OffsetScale", w_BlurSpread);
        Graphics.Blit(source, dest, blurMaterial);
	}
	
	// Downsamples source texture
	private void DownSample4x(RenderTexture source, RenderTexture dest)
	{
		float off = 1.0f;
		blurMaterial.SetFloat("_OffsetScale", off);
		Graphics.Blit(source, dest, blurMaterial);
	}








    //fff
    //这里边写了什么导致锁定Material，不能变色
    // Render all highlighted objects to the stencil buffer
    void OnPreRender()
    {
#if UNITY_4_0
		if (this.enabled == false || go.activeInHierarchy == false)
#else
        if (this.enabled == false || go.activeInHierarchy == false)
#endif
            return;

        if (stencilBuffer != null)
        {
            RenderTexture.ReleaseTemporary(stencilBuffer);
            stencilBuffer = null;
        }


        // 打开高亮Shader
        if (highlightingEvent != null)
        {
            //这一句，导致材质球问题，说白了，还在Object——ColorUpdate方法里。
            highlightingEvent(true, (stencilZBufferDepth > 0));
        }
        // We don't need to render the scene if there's no HighlightableObjects
        else
        {
            return;
        }



        stencilBuffer = RenderTexture.GetTemporary((int)GetComponent<Camera>().pixelWidth, (int)GetComponent<Camera>().pixelHeight, stencilZBufferDepth, RenderTextureFormat.ARGB32);

        if (!shaderCameraGO)
        {
            shaderCameraGO = new GameObject("HighlightingCamera", typeof(Camera));
            shaderCameraGO.GetComponent<Camera>().enabled = false;
            shaderCameraGO.hideFlags = HideFlags.HideAndDontSave;
        }

        if (!shaderCamera)
        {
            shaderCamera = shaderCameraGO.GetComponent<Camera>();
        }







        shaderCamera.CopyFrom(refCam);
        //shaderCamera.projectionMatrix = refCam.projectionMatrix;		// Uncomment this line if you have problems using Highlighting System with custom projection matrix on your camera
        shaderCamera.cullingMask = layerMask;
        shaderCamera.rect = new Rect(0f, 0f, 1f, 1f);
        shaderCamera.renderingPath = RenderingPath.VertexLit;
        shaderCamera.allowHDR = false;
        shaderCamera.useOcclusionCulling = false;
        shaderCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
        shaderCamera.clearFlags = CameraClearFlags.SolidColor;
        shaderCamera.targetTexture = stencilBuffer;
        shaderCamera.Render();

        // 关掉高亮Shader
        if (highlightingEvent != null)
        {
            highlightingEvent(false, false);
        }
    }






















    // Compose final frame with highlighting
    void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		// If stencilBuffer is not created by some reason
		if (stencilBuffer == null)
		{
			// Simply transfer framebuffer to destination
			Graphics.Blit(source, destination);
			return;
		}
		
		// Create two buffers for blurring the image
		int width = source.width / _downsampleFactor;
		int height = source.height / _downsampleFactor;
		RenderTexture buffer = RenderTexture.GetTemporary(width, height, stencilZBufferDepth, RenderTextureFormat.ARGB32);
		RenderTexture buffer2 = RenderTexture.GetTemporary(width, height, stencilZBufferDepth, RenderTextureFormat.ARGB32);
		
		// Copy stencil buffer to the 4x4 smaller texture
		DownSample4x(stencilBuffer, buffer);
		
		// Blur the small texture
		bool oddEven = true;
		for (int i = 0; i < iterations; i++)
		{
			if (oddEven)
			{
				FourTapCone(buffer, buffer2, i);
			}
			else
			{
				FourTapCone(buffer2, buffer, i);
			}
			
			oddEven = !oddEven;
		}
		
		// Compose
		compMaterial.SetTexture("_StencilTex", stencilBuffer);
		compMaterial.SetTexture("_BlurTex", oddEven ? buffer : buffer2);
		Graphics.Blit(source, destination, compMaterial);
		
		// Cleanup
		RenderTexture.ReleaseTemporary(buffer);
		RenderTexture.ReleaseTemporary(buffer2);
		if (stencilBuffer != null)
		{
			RenderTexture.ReleaseTemporary(stencilBuffer);
			stencilBuffer = null;
		}
	}
}