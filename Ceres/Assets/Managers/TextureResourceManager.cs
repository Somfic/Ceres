using Ceres.Assets.Primitives;
using Ceres.ECS;
using Ceres.ECS.Resource;
using Veldrid;

namespace Ceres.Assets.Managers;

public class TextureResourceManager : ResourceManager<string, Texture>
{
    private readonly GraphicsDevice _graphics;
    private readonly AssetLoader _loader;
    
    public TextureResourceManager(GraphicsDevice graphics, AssetLoader loader)
    {
        _graphics = graphics;
        _loader = loader;
    }
    
    protected override Texture Load(string path)
    {
        var processedTexture = _loader.LoadEmbeddedAsset<ProcessedTexture>(path);
        return processedTexture.CreateDeviceTexture(_graphics, _graphics.ResourceFactory, TextureUsage.Sampled);
    }

    protected override void OnResourceLoaded(in Entity entity, string info, Texture resource)
    {
        
    }
}