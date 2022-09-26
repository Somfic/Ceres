using System.Numerics;
using ImGuiNET;

namespace Ceres.Components;

public class TransformComponent : GuiComponent<TransformComponent>
{
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;
    
    public override TransformComponent Gui(TransformComponent transform)
    {
        ImGui.DragFloat3("Position", ref transform.Position);
        ImGui.DragFloat3("Rotation", ref transform.Rotation);
        ImGui.DragFloat3("Scale", ref transform.Scale);

        return transform;
    }
}