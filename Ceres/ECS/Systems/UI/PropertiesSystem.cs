using Ceres.ECS.Components;
using DefaultEcs;
using DefaultEcs.Serialization;
using ImGuiNET;
using Microsoft.Extensions.Logging;

namespace Ceres.ECS.Systems.UI;

public class PropertiesSystem : ASystem<State>
{
    private readonly ILogger<PropertiesSystem> _log;

    public PropertiesSystem(ILogger<PropertiesSystem> log)
    {
        _log = log;
    }

    protected override void OnUpdate(ref State state)
    {
        ImGui.Begin("Properties");
        
        if(state.Ui.SelectedEntity.HasValue)
        {
            var entity = state.Ui.SelectedEntity.Value;
            
            ImGui.Text(state.Ui.SelectedEntity.Value.Get<NameComponent>().Name);
            entity.ReadAllComponents(new ComponentReader());
            
        }
        
        ImGui.End();
    }
}

internal class ComponentReader : IComponentReader
{
    public void OnRead<T>(in T component, in Entity entity)
    {
        switch (component)
        {
            case NameComponent name:
            {
                var newValue = name;
                ImGui.InputText(name.Name, ref newValue.Name, 100);
                entity.Set(newValue);
                break;
            }

            case TransformComponent position:
            {
                var newValue = position;
                ImGui.InputFloat3("Position", ref newValue.Position);
                ImGui.InputFloat3("Rotation", ref newValue.Rotation);
                ImGui.InputFloat3("Scale", ref newValue.Scale);
                entity.Set(newValue);
                break;
            }
        }
    }
}