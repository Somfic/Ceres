using Ceres;
using Ceres.ECS.Components;
using Ceres.ECS.Systems;
using Ceres.ECS.Systems.UI;
using Veldrid;

await CeresEngine
    .Create<SandboxApplication>(new InitialisationOptions { Resizable = true, Backend = GraphicsBackend.OpenGL})
    .AddSystem<HierarchySystem>()
    .AddSystem<PropertiesSystem>()
    .AddSystem<UiRenderSystem>()
    .AddSystem<RenderSystem>()
    .RunAsync();

public class SandboxApplication : CeresApplication
{
    protected override void Start()
    {
        CreateEntity("Hello world").Set(new TransformComponent());
    }
}