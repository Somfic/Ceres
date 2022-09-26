using System.Numerics;
using Ceres.Assets.Primitives;
using Ceres.Components;
using Ceres.ECS;
using Ceres.ECS.Resource;
using ImGuiNET;
using Veldrid;

namespace Ceres.Assets.Managers;

public class ModelResourceManager : ResourceManager<string, Model>
{
    private readonly GraphicsDevice _graphics;
    private readonly AssetLoader _loader;
    
    public ModelResourceManager(GraphicsDevice graphics, AssetLoader loader)
    {
        _graphics = graphics;
        _loader = loader;
    }

    protected override Model Load(string path)
    {
        var processedModel = _loader.OpenFileSteam(path);
        return new Model(_graphics, _graphics.ResourceFactory, processedModel, new FileInfo(path).Extension,  
            new[] { VertexElementSemantic.Position, VertexElementSemantic.TextureCoordinate, VertexElementSemantic.Color, VertexElementSemantic.Normal }, new Model.ModelCreateInfo(new Vector3(0.5f, 0.5f, 0.5f), Vector2.One, Vector3.Zero));
    }

    protected override void OnResourceLoaded(in Entity entity, string info, Model model)
    {
        entity.Set(new ModelComponent { Model = model, Path = info });
    }
}

public class ModelComponent : GuiComponent<ModelComponent>
{
    public string Path;
    public Model Model;
    
    public override ModelComponent Gui(ModelComponent component)
    {
        ImGui.Text(Path);
        return component;
    }
}