using Ceres.ECS.Components;
using DefaultEcs;
using ImGuiNET;
using Microsoft.Extensions.Logging;

namespace Ceres.ECS.Systems.UI;

public class HierarchySystem : ASystem<State>
{
    private readonly ILogger<HierarchySystem> _log;
    private readonly World _world;

    public HierarchySystem(ILogger<HierarchySystem> log, World world)
    {
        _log = log;
        _world = world;
    }
    
    protected override void OnUpdate(ref State state)
    {
        ImGui.Begin("Hierarchy");
        
        foreach (var entity in _world.GetEntities().AsSet().GetEntities())
        {
            if (ImGui.Button(entity.Get<NameComponent>().Name))
            {
                state.Ui.SelectedEntity = entity;
            }
        }
        
        ImGui.End();
    }
}