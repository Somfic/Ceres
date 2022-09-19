using System.Numerics;
using Ceres.ECS;
using Ceres.ECS.Systems;
using DefaultEcs;
using ImGuiNET;
using Microsoft.Extensions.Logging;
using Veldrid;

public class UiRenderSystem : ASystem<State>
{
    private readonly ILogger<UiRenderSystem>? _log;
    private readonly World _world;
    private bool _isInitialized;
    private ImGuiRenderer _renderer;

    public UiRenderSystem(ILogger<UiRenderSystem>? log, World world)
    {
        _log = log;
        _world = world;
    }

    public override void Update(State state)
    {
        if (!IsEnabled)
            return;
        
        if (!_isInitialized)
        {
            _renderer = new ImGuiRenderer(
                state.Graphics, 
                state.Graphics.MainSwapchain.Framebuffer.OutputDescription, 
                state.Window.Width, 
                state.Window.Height);
        
            state.Window.Resized += () => _renderer.WindowResized(state.Window.Width, state.Window.Height);

            _isInitialized = true;
        }
        
        _renderer.Update(1f / 60f, state.Input);

        ImGui.DockSpace(0, new Vector2(20, 20));
        ImGui.Begin("Hello, world!", ImGuiWindowFlags.DockNodeHost);
        ImGui.Text("This is some useful text.");
        ImGui.End();

        _renderer.Render(state.Graphics, state.Commands);
    }
}