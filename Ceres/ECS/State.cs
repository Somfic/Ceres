using Veldrid;
using Veldrid.Sdl2;

namespace Ceres.ECS;

public struct State
{
    public float DeltaTime;

    public Sdl2Window Window;

    public GraphicsDevice Graphics;

    public CommandList Commands;

    public InputSnapshot Input;
}