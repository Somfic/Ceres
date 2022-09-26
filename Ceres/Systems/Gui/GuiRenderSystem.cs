using System.Text;
using Ceres.ECS.System;
using Veldrid;
using Veldrid.SPIRV;

namespace Ceres.Systems.Gui;

public class GuiRenderSystem : System<State>
{
    private ImGuiRenderer _renderer;

    private Texture _applicationTexture;
    private Framebuffer _applicationFramebuffer;
    
    private Pipeline _pipeline;
    private Shader[] _shaders;
    
    private const string VertexCode = @"
#version 450

layout(location = 0) in vec2 Position;
layout(location = 1) in vec4 Color;

layout(location = 0) out vec4 fsin_Color;

void main()
{
    gl_Position = vec4(Position, 0, 1);
    fsin_Color = Color;
}";

    private const string FragmentCode = @"
#version 450

layout(location = 0) in vec4 fsin_Color;
layout(location = 0) out vec4 fsout_Color;

void main()
{
    fsout_Color = fsin_Color;
}";

    public GuiRenderSystem()
    {
        
    }

    protected override void OnStart(ref State state)
    {
        _renderer = new ImGuiRenderer(
            state.Graphics, 
            state.Graphics.MainSwapchain.Framebuffer.OutputDescription, 
            state.Window.Width, 
            state.Window.Height);
        
        state.Window.Resized += () =>
        {
            _renderer.WindowResized(State.Window.Width, State.Window.Height);
            _applicationTexture = State.Graphics.ResourceFactory.CreateTexture(TextureDescription.Texture2D((uint)State.Window.Width, (uint)State.Window.Height, 1, 1, PixelFormat.R32_G32_B32_A32_Float, TextureUsage.RenderTarget | TextureUsage.Sampled));
        };

        _applicationTexture = State.Graphics.ResourceFactory.CreateTexture(TextureDescription.Texture2D((uint)State.Window.Width, (uint)State.Window.Height, 1, 1, PixelFormat.R32_G32_B32_A32_Float, TextureUsage.RenderTarget | TextureUsage.Sampled));

        var fbad = new FramebufferAttachmentDescription();
        fbad.Target = _applicationTexture;
        
        var fbd = new FramebufferDescription();
        fbd.ColorTargets = new FramebufferAttachmentDescription[1];
        fbd.ColorTargets[0] = fbad;

        _applicationFramebuffer = state.Graphics.ResourceFactory.CreateFramebuffer(fbd);

        var vertexLayout = new VertexLayoutDescription(
            new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
            new VertexElementDescription("Color", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4));
        
        var vertexShaderDesc = new ShaderDescription(
            ShaderStages.Vertex,
            Encoding.UTF8.GetBytes(VertexCode),
            "main");
        var fragmentShaderDesc = new ShaderDescription(
            ShaderStages.Fragment,
            Encoding.UTF8.GetBytes(FragmentCode),
            "main");

        _shaders = state.Graphics.ResourceFactory.CreateFromSpirv(vertexShaderDesc, fragmentShaderDesc);
        
        var pipelineDescription = new GraphicsPipelineDescription();
        pipelineDescription.BlendState = BlendStateDescription.SingleOverrideBlend;
        
        pipelineDescription.DepthStencilState = new DepthStencilStateDescription(
            depthTestEnabled: true,
            depthWriteEnabled: true,
            comparisonKind: ComparisonKind.LessEqual);
        
        pipelineDescription.RasterizerState = new RasterizerStateDescription(
            cullMode: FaceCullMode.Back,
            fillMode: PolygonFillMode.Solid,
            frontFace: FrontFace.Clockwise,
            depthClipEnabled: true,
            scissorTestEnabled: false);
        
        pipelineDescription.PrimitiveTopology = PrimitiveTopology.TriangleStrip;
        
        pipelineDescription.ResourceLayouts = System.Array.Empty<ResourceLayout>();
        
        pipelineDescription.ShaderSet = new ShaderSetDescription(
            vertexLayouts: new [] { vertexLayout },
            shaders: _shaders);

        pipelineDescription.Outputs = _applicationFramebuffer.OutputDescription;
        
        _pipeline = state.Graphics.ResourceFactory.CreateGraphicsPipeline(pipelineDescription);
        
        state.Gui.ViewportTexturePointer = _renderer.GetOrCreateImGuiBinding(state.Graphics.ResourceFactory, _applicationTexture);
    }

    protected override void OnUpdate(ref State state)
    {
        state.Commands.SetFramebuffer(state.Graphics.SwapchainFramebuffer);
        state.Commands.ClearColorTarget(0, RgbaFloat.Black);

        _renderer.Render(state.Graphics, state.Commands);
        _renderer.Update(1 / 60f, state.Input);
        
        state.Commands.SetFramebuffer(_applicationFramebuffer);
        state.Commands.SetPipeline(_pipeline);
        state.Commands.ClearColorTarget(0, RgbaFloat.Black);
    }
}