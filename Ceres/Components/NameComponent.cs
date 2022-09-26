using ImGuiNET;

namespace Ceres.Components;

public class NameComponent : GuiComponent<NameComponent>
{
    public string Name;
    
    public override NameComponent Gui(NameComponent name)
    {
        ImGui.InputText("Name", ref name.Name, 100);
        return name;
    }
}