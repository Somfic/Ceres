using Svelto.ECS;
using Veldrid;

namespace Ceres.Engines;

public interface IRenderEngine : IQueryingEntitiesEngine
{
    public void Render(CommandList commands, GraphicsDevice graphics);
}