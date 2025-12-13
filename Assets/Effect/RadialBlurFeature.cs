using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RadialBlurFeature : ScriptableRendererFeature
{
    class RadialBlurPass : ScriptableRenderPass
    {
        static readonly string k_RenderTag = "Radial Blur Pass";
        static readonly int TempTargetId = Shader.PropertyToID("_TempTarget");

        Material material;
        RadialBlur settings;

        public RadialBlurPass(Material mat)
        {
            material = mat;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var stack = VolumeManager.instance.stack;
            settings = stack.GetComponent<RadialBlur>();

            if (settings == null || !settings.IsActive())
                return;

            var cmd = CommandBufferPool.Get("Radial Blur");

            var renderer = renderingData.cameraData.renderer;
            var source = renderer.cameraColorTargetHandle;

            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;

            cmd.GetTemporaryRT(TempTargetId, descriptor);

            cmd.SetGlobalFloat("_Intensity", settings.intensity.value);
            cmd.SetGlobalVector("_Center", settings.center.value);
            cmd.SetGlobalFloat("_Radius", settings.radius.value);
            cmd.SetGlobalInt("_SampleCount", settings.sampleCount.value);

            // Source → Temp (Radial Blur)
            cmd.SetGlobalTexture("_SourceTex", source);
            Blit(cmd, source, TempTargetId, material);

            // Temp → Source (write back)
            Blit(cmd, TempTargetId, source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

    }

    RadialBlurPass pass;
    public Shader shader;

    public override void Create()
    {
        if (shader == null)
            shader = Shader.Find("Hidden/RadialBlur");

        Material mat = CoreUtils.CreateEngineMaterial(shader);
        pass = new RadialBlurPass(mat)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }
}
