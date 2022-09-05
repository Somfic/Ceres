using Svelto.ECS;

namespace Ceres.Engines;

public interface IUpdateEngine : IQueryingEntitiesEngine
{
    public void Update(double deltaTime);
}