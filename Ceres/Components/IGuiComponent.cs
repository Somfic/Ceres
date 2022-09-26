namespace Ceres.Components;

public interface IGuiComponent
{
    public dynamic Gui(dynamic component);
}

public abstract class GuiComponent<T> : IGuiComponent
{
    public abstract T Gui(T component);
    
    public dynamic Gui(dynamic component) => Gui((T)component)!;
}