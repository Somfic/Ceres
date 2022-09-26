using System.Numerics;
using Ceres.Components;
using Ceres.ECS;
using Ceres.ECS.Serialization;
using Ceres.ECS.System;
using ImGuiNET;

namespace Ceres.Systems.Gui;

public class GuiSystem : System<State>
{
    private readonly World _world;

    public GuiSystem(World world)
    {
        _world = world;
    }

    protected override void OnUpdate(ref State state)
    {
        DrawHierarchy(ref state);
        DrawViewport(ref state);
        DrawProperties(ref state);
    }

    private void DrawHierarchy(ref State state)
    {
        ImGui.SetNextItemWidth(300);
        ImGui.SetNextWindowPos(new Vector2(0, 0));
        ImGui.Begin("Hierarchy", ImGuiWindowFlags.NoResize);
        
        foreach (var entity in _world.GetEntities().AsSet().GetEntities())
        {
            if (ImGui.Button(entity.Get<NameComponent>().Name))
            {
                state.Gui.SelectedEntity = entity;
            }
        }
        
        ImGui.End();
    }

    private void DrawViewport(ref State state)
    {
        ImGui.SetNextWindowPos(new Vector2(300, 0));
        ImGui.SetNextItemWidth(state.Window.Width - 300 - 300);
        ImGui.Begin("Viewport", ImGuiWindowFlags.NoResize);
        ImGui.Image(state.Gui.ViewportTexturePointer, new Vector2(state.Window.Width - 300 - 300));
        ImGui.End();
    }

    private void DrawProperties(ref State state)
    {
        ImGui.SetNextItemWidth(300);
        ImGui.SetNextWindowPos(new Vector2(state.Window.Width - 300, 0));
        ImGui.Begin("Properties", ImGuiWindowFlags.NoResize);
        if(state.Gui.SelectedEntity.HasValue)
        {
            var entity = state.Gui.SelectedEntity.Value;
            
            ImGui.Text(state.Gui.SelectedEntity.Value.Get<NameComponent>().Name);
            entity.ReadAllComponents(new ComponentReader());
        }
        ImGui.End();
    }
}


internal class ComponentReader : IComponentReader
{
    public void OnRead<T>(in T component, in Entity entity)
    {
        if (component is IGuiComponent guiComponent)
        {
            guiComponent.Gui(component);
        }
        else
        {
            ImGui.TextDisabled("Unsupported component");
        }
    }
}