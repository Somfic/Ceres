using Ceres.ECS;
using Veldrid;
using Veldrid.Sdl2;

namespace Ceres;

public struct State
{
    public float DeltaTime;

    public Sdl2Window Window;

    public GraphicsDevice Graphics;

    public CommandList Commands;

    public InputSnapshot Input;

    public GuiState Gui;
    
}

public struct GuiState 
{
    public Entity? SelectedEntity;

    public IntPtr ViewportTexturePointer;
}