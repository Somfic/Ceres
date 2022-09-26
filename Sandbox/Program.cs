using Ceres;
using Ceres.Assets;
using Ceres.Assets.Managers;
using Ceres.Components;
using Ceres.ECS.Resource;
using Ceres.Systems;
using Ceres.Systems.Gui;
using Veldrid;

await CeresEngine
    .Create<SandboxApplication>(new InitialisationOptions { Resizable = true, Backend = GraphicsBackend.Metal })
    .AddSystem<GuiSystem>()
    .AddSystem<GuiRenderSystem>()
    .AddSystem<RenderSystem>()
    .AddResource<ModelResourceManager>()
    .AddResource<TextureResourceManager>()
    .RunAsync();

public class SandboxApplication : CeresApplication
{
    protected override void Start()
    {
        var baseEntity = CreateEntity("Monkey");

        baseEntity.Set(ManagedResource<Model>.Create("Models/monkey.obj"));
    }
}