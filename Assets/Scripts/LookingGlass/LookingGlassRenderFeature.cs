using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LookingGlassRenderFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class LookingGlassSettings
    {
        public string profilerTag = "LookingGlass";
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
        public LayerMask layerMask = -1;
        public Material depthMaskMaterial;
        public bool copyDepthBuffer = true;
    }
    
    [SerializeField] private LookingGlassSettings settings = new LookingGlassSettings();
    
    private LookingGlassRenderPass renderPass;
    private LookingGlassDepthPass depthPass;
    
    public override void Create()
    {
        renderPass = new LookingGlassRenderPass(settings);
        depthPass = new LookingGlassDepthPass(settings);
        
        renderPass.renderPassEvent = settings.renderPassEvent;
        depthPass.renderPassEvent = RenderPassEvent.AfterRenderingPrePasses;
    }
    
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.copyDepthBuffer)
        {
            // depthPass.Setup(renderer.cameraDepthTargetHandle);
            renderer.EnqueuePass(depthPass);
        }
        
        // renderPass.Setup(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
        renderer.EnqueuePass(renderPass);
    }
    
    protected override void Dispose(bool disposing)
    {
        renderPass?.Dispose();
        depthPass?.Dispose();
    }
}

public class LookingGlassRenderPass : ScriptableRenderPass
{
    private LookingGlassRenderFeature.LookingGlassSettings settings;
    private RTHandle colorTarget;
    private RTHandle depthTarget;
    
    private static readonly int TempColorTextureID = Shader.PropertyToID("_TempColorTexture");
    private static readonly int SceneDepthTextureID = Shader.PropertyToID("_SceneDepthTexture");
    
    public LookingGlassRenderPass(LookingGlassRenderFeature.LookingGlassSettings settings)
    {
        this.settings = settings;
        profilingSampler = new ProfilingSampler(settings.profilerTag);
    }
    
    public void Setup(RTHandle colorTarget, RTHandle depthTarget)
    {
        this.colorTarget = colorTarget;
        this.depthTarget = depthTarget;
    }
    
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {   

         Setup(renderingData.cameraData.renderer.cameraColorTargetHandle, renderingData.cameraData.renderer.cameraDepthTargetHandle);
        if (settings.depthMaskMaterial == null) return;
        
        CommandBuffer cmd = CommandBufferPool.Get(settings.profilerTag);
        
        using (new ProfilingScope(cmd, profilingSampler))
        {
            // Set global depth texture for LookingGlass shaders
            if (depthTarget != null)
            {
                cmd.SetGlobalTexture(SceneDepthTextureID, depthTarget);
            }
            
            // Find all LookingGlass objects and render them
            var controllers = Object.FindObjectsOfType<LookingGlassController>();
            foreach (var controller in controllers)
            {
                if (controller.Settings.enableEffect && controller.VirtualCamera != null)
                {
                    RenderLookingGlass(cmd, controller, ref renderingData);
                }
            }
        }
        
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
    
    private void RenderLookingGlass(CommandBuffer cmd, LookingGlassController controller, ref RenderingData renderingData)
    {
        var renderer = controller.GetComponent<MeshRenderer>();
        if (renderer == null || !renderer.enabled) return;
        
        var filter = controller.GetComponent<MeshFilter>();
        if (filter == null || filter.sharedMesh == null) return;
        
        // Set up material properties
        var material = renderer.sharedMaterial;
        if (material == null) return;
        
        // Update material properties
        material.SetTexture("_VirtualTexture", controller.VirtualRenderTexture);
        
        // Render the glass mesh
        cmd.DrawMesh(filter.sharedMesh, controller.transform.localToWorldMatrix, material);
    }
    
    public void Dispose()
    {
        // Clean up resources
    }
}

public class LookingGlassDepthPass : ScriptableRenderPass
{
    private LookingGlassRenderFeature.LookingGlassSettings settings;
    private RTHandle depthCopyTarget;

    private static readonly int DepthCopyTextureID = Shader.PropertyToID("_DepthCopyTexture");

    public LookingGlassDepthPass(LookingGlassRenderFeature.LookingGlassSettings settings)
    {
        this.settings = settings;
        profilingSampler = new ProfilingSampler(settings.profilerTag + "_Depth");
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        if (depthCopyTarget == null)
        {
            var descriptor = cameraTextureDescriptor;
            descriptor.colorFormat = RenderTextureFormat.RFloat;
            descriptor.depthBufferBits = 0;

            RenderingUtils.ReAllocateIfNeeded(ref depthCopyTarget, descriptor, name: "_LookingGlassDepthCopy");
        }

        ConfigureTarget(depthCopyTarget);
        ConfigureClear(ClearFlag.All, Color.white);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var depthTarget = renderingData.cameraData.renderer.cameraDepthTargetHandle;
        if (depthTarget == null|| depthCopyTarget == null||depthTarget.rt == null)
            return;

        CommandBuffer cmd = CommandBufferPool.Get(settings.profilerTag + "_Depth");

        using (new ProfilingScope(cmd, profilingSampler))
        {
            // Copy depth buffer to a format accessible by shaders
            cmd.Blit(depthTarget, depthCopyTarget);
            cmd.SetGlobalTexture(DepthCopyTextureID, depthCopyTarget);
        }

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public void Dispose()
    {
        depthCopyTarget?.Release();
    }
}