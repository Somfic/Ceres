using Ceres;
using Ceres.ECS.Systems;

await CeresEngine
    .Create<SandboxApplication>(new InitialisationOptions { Resizable = true })
    .AddSystem<UiRenderSystem>()
    .AddSystem<RenderSystem>()
    .RunAsync();

public class SandboxApplication : CeresApplication
{
    protected override void Start()
    {
        CreateEntity("Hello world");
    }
}