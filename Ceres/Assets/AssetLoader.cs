using System.Reflection;
using Ceres.Assets.Primitives;
using Microsoft.Extensions.Logging;

namespace Ceres.Assets;

public class AssetLoader
{
    private readonly ILogger<AssetLoader> _log;
    private readonly Dictionary<Type, BinaryAssetSerializer> _serializers = DefaultSerializers.Get();

    public AssetLoader(ILogger<AssetLoader> log)
    {
        _log = log;
    }
    
    public T LoadEmbeddedAsset<T>(string name)
    {
        if (!_serializers.TryGetValue(typeof(T), out var serializer))
        {
            throw new InvalidOperationException("No serializer registered for type " + typeof(T).Name);
        }

        using var stream = OpenEmbeddedAssetStream(name);

        var reader = new BinaryReader(stream);
        return (T)serializer.Read(reader);
    }
    
    public Stream OpenEmbeddedAssetStream(string name)
    {
        var assembly = Assembly.GetEntryAssembly();
        var stream = assembly.GetManifestResourceStream(name);

        if (stream == null)
        {
            throw new InvalidOperationException("No embedded asset with the name " + name);
        }

        return stream;
    }

    public Stream OpenFileSteam(string path)
    {
        return File.OpenRead(path);
    }
}