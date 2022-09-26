namespace Ceres.ECS.Resource;

public interface IResourceManager
{
    public IDisposable Manage(World world);
}