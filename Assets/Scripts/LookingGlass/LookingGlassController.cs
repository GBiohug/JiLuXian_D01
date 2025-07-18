using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
[ExecuteInEditMode]
public class LookingGlassSettings
{   
    [Header("Camera Settings")]
    public Camera targetCamera;
    public Vector3 virtualCameraOffset = new Vector3(0, 0, 5);
    public float depthScalar = 1.0f;
    public float sunSelector = 0.5f;
    
    [Header("Rendering")]
    public RenderTexture renderTexture;
    public int textureWidth = 1024;
    public int textureHeight = 1024;
    public RenderTextureFormat textureFormat = RenderTextureFormat.ARGB32;
    
    [Header("Glass Properties")]
    public LayerMask renderLayers = -1;
    public float nearClipPlane = 0.1f;
    public float farClipPlane = 100f;
    
    [Header("Debug")]
    public bool showDebugInfo = false;
    public bool enableEffect = true;
}

public class LookingGlassController : MonoBehaviour
{
    [SerializeField] private LookingGlassSettings settings = new LookingGlassSettings();
    
    private Camera mainCamera;
    private Camera virtualCamera;
    private RenderTexture sceneDepthTexture;
    private RenderTexture virtualRenderTexture;
    private Material glassMaterial;
    private MeshRenderer glassRenderer;
    private LookingGlassRenderFeature renderFeature;
    
    // Shader property IDs
    private static readonly int VirtualTexture = Shader.PropertyToID("_VirtualTexture");
    private static readonly int SceneDepthTexture = Shader.PropertyToID("_SceneDepthTexture");
    private static readonly int LookingGlassDepthScalar = Shader.PropertyToID("_LookingGlassDepthScalar");
    private static readonly int LookingGlassSunSelector = Shader.PropertyToID("_LookingGlassSunSelector");
    private static readonly int MainCameraPos = Shader.PropertyToID("_MainCameraPos");
    private static readonly int VirtualCameraPos = Shader.PropertyToID("_VirtualCameraPos");
    private static readonly int GlassWorldMatrix = Shader.PropertyToID("_GlassWorldMatrix");
    private static readonly int ProjectionParams = Shader.PropertyToID("_ProjectionParams");
    
    public LookingGlassSettings Settings => settings;
    public Camera VirtualCamera => virtualCamera;
    public RenderTexture VirtualRenderTexture => virtualRenderTexture;
    
    private void Awake()
    {
        InitializeComponents();
        CreateVirtualCamera();
        CreateRenderTextures();
        SetupMaterial();
    }
    
    private void InitializeComponents()
    {
        mainCamera = settings.targetCamera ? settings.targetCamera : Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("LookingGlass: No main camera found!");
            return;
        }
        
        glassRenderer = GetComponent<MeshRenderer>();
        if (glassRenderer == null)
        {
            Debug.LogError("LookingGlass: No MeshRenderer found on glass object!");
            return;
        }
        
        glassMaterial = glassRenderer.material;
    }
    
    private void CreateVirtualCamera()
    {
        // Create virtual camera GameObject
        GameObject virtualCameraGO = new GameObject("LookingGlass_VirtualCamera");
        virtualCameraGO.transform.parent = transform;
        Debug.Log("VM created-");
        virtualCamera = virtualCameraGO.AddComponent<Camera>();
        
        // Copy main camera settings
        virtualCamera.CopyFrom(mainCamera);
        
        // Override specific settings
        virtualCamera.cullingMask = settings.renderLayers;
        virtualCamera.nearClipPlane = settings.nearClipPlane;
        virtualCamera.farClipPlane = settings.farClipPlane;
        virtualCamera.enabled = false; // We'll control rendering manually
        
        // Set URP specific settings
        var cameraData = virtualCamera.GetComponent<UniversalAdditionalCameraData>();
        if (cameraData == null)
        {
            cameraData = virtualCamera.gameObject.AddComponent<UniversalAdditionalCameraData>();
        }

        cameraData.antialiasing = AntialiasingMode.None;
        cameraData.renderType = CameraRenderType.Base;
        // cameraData.clearDepth = true;
        cameraData.renderShadows = true;
    }
    
    private void CreateRenderTextures()
    {
        // Create virtual camera render texture
        if (virtualRenderTexture != null)
        {
            virtualRenderTexture.Release();
        }
        
        virtualRenderTexture = new RenderTexture(settings.textureWidth, settings.textureHeight, 24, settings.textureFormat);
        virtualRenderTexture.name = "LookingGlass_VirtualTexture";
        virtualRenderTexture.Create();
        
        virtualCamera.targetTexture = virtualRenderTexture;
        
        // Create scene depth texture for masking
        if (sceneDepthTexture != null)
        {
            sceneDepthTexture.Release();
        }
        
        sceneDepthTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.RFloat);
        sceneDepthTexture.name = "LookingGlass_SceneDepth";
        sceneDepthTexture.Create();
    }
    
    private void SetupMaterial()
    {
        if (glassMaterial == null) return;
        
        glassMaterial.SetTexture(VirtualTexture, virtualRenderTexture);
        glassMaterial.SetTexture(SceneDepthTexture, sceneDepthTexture);
        glassMaterial.SetFloat(LookingGlassDepthScalar, settings.depthScalar);
        glassMaterial.SetFloat(LookingGlassSunSelector, settings.sunSelector);
    }
    
    private void Update()
    {
        if (!settings.enableEffect || mainCamera == null || virtualCamera == null) return;
        
        UpdateVirtualCamera();
        UpdateMaterialProperties();
        
        if (settings.showDebugInfo)
        {
            DrawDebugInfo();
        }
    }
    
    private void UpdateVirtualCamera()
    {
        // Calculate virtual camera position based on main camera and glass
        Vector3 glassPosition = transform.position;
        Vector3 mainCameraPosition = mainCamera.transform.position;
        
        // Calculate the reflection point through the glass
        Vector3 toGlass = glassPosition - mainCameraPosition;
        Vector3 glassNormal = transform.forward;
        
        // Project the offset onto the glass normal
        Vector3 projectedOffset = Vector3.Project(settings.virtualCameraOffset, glassNormal);
        Vector3 lateralOffset = settings.virtualCameraOffset - projectedOffset;
        
        // Calculate virtual camera position
        // Vector3 virtualPosition = glassPosition + projectedOffset + lateralOffset;
        Vector3 virtualPosition = mainCameraPosition + settings.virtualCameraOffset;
        // Apply perspective correction based on viewing angle
        // float viewingAngle = Vector3.Dot(toGlass.normalized, glassNormal);
        // virtualPosition += glassNormal * (viewingAngle * settings.virtualCameraOffset.magnitude * 0.5f);
        //
        virtualCamera.transform.position = virtualPosition;
        
        // Make virtual camera look at the same relative direction as main camera
        Vector3 lookDirection = mainCamera.transform.forward;
        virtualCamera.transform.rotation = Quaternion.LookRotation(lookDirection, mainCamera.transform.up);
        
        // Update projection matrix for perspective correction
        UpdateProjectionMatrix();
    }
    
    private void UpdateProjectionMatrix()
    {
        // Create oblique projection matrix for better perspective
        Vector3 glassPosition = transform.position;
        Vector3 glassNormal = transform.forward;
        
        // Calculate the oblique near plane
        Vector4 clipPlane = new Vector4(glassNormal.x, glassNormal.y, glassNormal.z, -Vector3.Dot(glassNormal, glassPosition));
        
        // Get the original projection matrix
        Matrix4x4 projection = virtualCamera.projectionMatrix;
        
        // Apply oblique projection
        Matrix4x4 oblique = CalculateObliqueMatrix(projection, clipPlane);
        virtualCamera.projectionMatrix = oblique;
    }
    
    private Matrix4x4 CalculateObliqueMatrix(Matrix4x4 projection, Vector4 clipPlane)
    {
        Vector4 q = projection.inverse * new Vector4(
            Mathf.Sign(clipPlane.x),
            Mathf.Sign(clipPlane.y),
            1.0f,
            1.0f
        );
        
        Vector4 c = clipPlane * (2.0f / Vector4.Dot(clipPlane, q));
        
        projection[2] = c.x - projection[3];
        projection[6] = c.y - projection[7];
        projection[10] = c.z - projection[11];
        projection[14] = c.w - projection[15];
        
        return projection;
    }
    
    private void UpdateMaterialProperties()
    {
        if (glassMaterial == null) return;
        
        glassMaterial.SetVector(MainCameraPos, mainCamera.transform.position);
        glassMaterial.SetVector(VirtualCameraPos, virtualCamera.transform.position);
        glassMaterial.SetMatrix(GlassWorldMatrix, transform.localToWorldMatrix);
        glassMaterial.SetFloat(LookingGlassDepthScalar, settings.depthScalar);
        glassMaterial.SetFloat(LookingGlassSunSelector, settings.sunSelector);
        
        // Set projection parameters
        float near = virtualCamera.nearClipPlane;
        float far = virtualCamera.farClipPlane;
        // glassMaterial.SetVector(ProjectionParams, new Vector4(1.0f, near, far, 1.0f / far));
    }
    
    private void LateUpdate()
    {
        if (!settings.enableEffect) return;
        
        // Render virtual camera
        if (virtualCamera != null && virtualRenderTexture != null)
        {
            virtualCamera.Render();
        }
    }
    
    private void DrawDebugInfo()
    {
        if (virtualCamera == null || mainCamera == null) return;
        
        // Draw debug lines
        Debug.DrawLine(transform.position, mainCamera.transform.position, Color.red);
        Debug.DrawLine(transform.position, virtualCamera.transform.position, Color.blue);
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.green);
    }
    
    public void SetDepthScalar(float value)
    {
        settings.depthScalar = value;
        if (glassMaterial != null)
        {
            glassMaterial.SetFloat(LookingGlassDepthScalar, value);
        }
    }
    
    public void SetSunSelector(float value)
    {
        settings.sunSelector = value;
        if (glassMaterial != null)
        {
            glassMaterial.SetFloat(LookingGlassSunSelector, value);
        }
    }
    
    public void EnableEffect(bool enable)
    {
        settings.enableEffect = enable;
        if (glassRenderer != null)
        {
            glassRenderer.enabled = enable;
        }
    }
    
    // Support for glass breaking/interaction
    public void OnGlassBroken(Vector3 impactPoint, float intensity)
    {
        // This can be extended for fracture effects
        // For now, just disable the effect
        EnableEffect(false);
    }
    
    public void RepairGlass()
    {
        EnableEffect(true);
    }
    
    private void OnDestroy()
    {
        if (virtualRenderTexture != null)
        {
            virtualRenderTexture.Release();
        }
        
        if (sceneDepthTexture != null)
        {
            sceneDepthTexture.Release();
        }
    }
    
    private void OnDrawGizmos()
    {
        if (!settings.showDebugInfo) return;
        
        // Draw glass bounds
        Gizmos.color = Color.cyan;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        
        // Draw virtual camera position
        if (virtualCamera != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.DrawWireSphere(virtualCamera.transform.position, 0.1f);
        }
    }
}