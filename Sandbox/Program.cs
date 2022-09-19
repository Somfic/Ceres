using Ceres;
using Ceres.ECS.Components;
using DefaultEcs.System;
using DefaultEcs.Threading;

await CeresEngine
    .Create<SandboxApplication>()
    .AddSystem<UiRenderSystem>()
    .RunAsync();

public class SandboxApplication : CeresApplication
{
    protected override void Start()
    {
        CreateEntity("Hello world");
    }
}