using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

internal class XRBlitPass : ScriptableRenderPass
{
    ProfilingSampler sampler = new ProfilingSampler("XRBlit");
    Material material;
    static readonly int _FadeAmount = Shader.PropertyToID("_FadeAmount");

    public XRBlitPass(Material material)
    {
        this.material = material;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var camera = renderingData.cameraData.camera;
        if (camera.cameraType != CameraType.Game)
            return;

        if (material == null || Mathf.Approximately(material.GetFloat(_FadeAmount), 0.0f))
            return;

        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, sampler))
        {
            cmd.SetRenderTarget(
                renderingData.cameraData.renderer.cameraColorTarget,
                renderingData.cameraData.renderer.cameraDepthTarget
                );
            //The RenderingUtils.fullscreenMesh argument specifies that the mesh to draw is a quad.
            cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, material);
        }
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();

        CommandBufferPool.Release(cmd);
    }
}